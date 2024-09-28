using System.Collections.Generic;
using Godot;
using Pure3D;
using Pure3D.Chunks;

/// <summary>
/// Displays Pure3D textures and 3D assets
/// </summary>
public partial class Viewer : Node
{
	#region Export Variables
	[Export]
	private Container _2d_root; // Scene used to display 2D assets
	[Export]
	private TextureRect _texture_view; // Used for viewing textures
	[Export]
	private Container _3d_root; // Scene used to display 3D assets
	[Export]
	private SubViewport _3d_view; // Used for viewing 3D scenes
	[Export]
	private Tree _details;
	#endregion

	#region Private Variables
	// Collection of each Pure3D Image and their associated texture
	private readonly Dictionary<Pure3D.Chunks.ImageData, ImageTexture> _textures = new();
	// Collection of each 3D item in the tree and their associated 3D scene
	private readonly Dictionary<Chunk, Node3D> _3d_scenes = new();
	// Node3D that is currently being viewed and can be exported
	private Node3D _currentNode3D = null;
	// Formats and file extensions that a glTF file can be exported to
	private string[] _gltfFilters = { "*.gltf;glTF text file", "*.glb;glTF binary file" };
	// Formats and file extensions that a texture can be exported to
	private string[] _texFilters = { "*.png;PNG texture file" };
	// Converts a 3D scene into glTF data
	private GltfDocument _document = null;
	// Stores glTF data
	private GltfState _state = null;
	private StandardMaterial3D meshMaterial;
	#endregion

	public override void _Ready()
	{
		base._Ready();

		// Set the mesh to cull front faces and view vertex colours
		// Godot uses clockwise winding order to determine front-faces of
		// primitive triangles, whereas Pure3D uses anti-clockwise
		// So, without this, Godot displays the mesh's back faces only
		meshMaterial = new()
		{
			CullMode = BaseMaterial3D.CullModeEnum.Front,
			VertexColorUseAsAlbedo = true
		};
	}

	/// <summary>
	/// Views a selected chunk in the appropriate viewer
	/// </summary>
	/// <param name="chunk">Pure3D chunk to be viewed</param>
	public void ViewChunk(Chunk chunk)
	{
		// Make the current Node3D invisible
		if (_currentNode3D != null) _currentNode3D.Visible = false;

		// Load the chunk based on its type
		switch (chunk)
		{
			case Skeleton skel:
				LoadSkeleton(skel);
				break;

			case Pure3D.Chunks.Mesh mesh:
				LoadMesh(mesh);
				break;

			case Pure3D.Chunks.Image image:
				LoadImage(image);
				break;

			case ImageData image:
				LoadImageData(image);
				break;

			default:
				// If the chunk is not viewable
				// Make the 2D view and 3D view invisible
				_2d_root.Visible = false;
				_3d_root.Visible = false;
				_currentNode3D = null;
				break;
		}

		// Make the current Node3D visible
		if (_currentNode3D != null) _currentNode3D.Visible = true;
	}

	#region 2D Chunk Loading
	/// <summary>
	/// Makes the 2D view visible to view specific ImageData
	/// </summary>
	/// <param name="image">ImageData chunk to be viewed</param>
	private void ViewImage(ImageData image)
	{
		_3d_root.Visible = false;
		_texture_view.Texture = _textures[image];
		_2d_root.Visible = true;
	}

	/// <summary>
	/// Views a Pure3D Image in a Texture Rect
	/// </summary>
	/// <param name="image">Pure3D Image chunk to be viewed</param>
	private void LoadImage(Pure3D.Chunks.Image image)
	{
		LoadImageData(image.LoadImageData());
	}

	/// <summary>
	/// Views Pure3D ImageData in a Texture Rect
	/// </summary>
	/// <param name="data">Pure3D ImageData chunk to be viewed</param>
	private void LoadImageData(ImageData data)
	{
		if (!_textures.ContainsKey(data))
		{
			// If the Image Data has not been loaded yet
			// Load the data from the chunk
			Godot.Image newImage = new();
			Error err = newImage.LoadPngFromBuffer(data.Data);

			if (err == Error.Ok)
			{
				// If there are no problems loading the image
				// Convert it into a ImageTexture and add it to the dictionary
				_textures.Add(data, ImageTexture.CreateFromImage(newImage));
			}
			else
			{
				// If there are problems loading the image
				GD.PrintErr("Error while loading the current texture: " + err);
				return;
			}
		}

		// Display the associated ImageTexture
		ViewImage(data);
	}
	#endregion

	#region 3D Chunk Loading
	/// <summary>
	/// Makes the 3D view visible to view a specific 3D chunk
	/// </summary>
	/// <param name="chunk">3D chunk to view</param>
	private void View3dScene(Chunk chunk)
	{
		_2d_root.Visible = false;
		_currentNode3D = _3d_scenes[chunk];
		_3d_root.Visible = true;
	}

	/// <summary>
	/// Loads a Pure3D Skeleton as a Skeleton3D
	/// </summary>
	/// <param name="bones">Pure3D Skeleton to be loaded</param>
	private void LoadSkeleton(Pure3D.Chunks.Skeleton bones)
	{
		if (!_3d_scenes.ContainsKey(bones))
		{
			// If the Skeleton has not been loaded yet
			// Load the Skeleton from the chunk
			// Add a parent Node and make it invisible
			Node3D parent = new();
			parent.Name = "Skel_" + bones.Name;
			parent.Visible = false;
			_3d_scenes.Add(bones, parent);
			_3d_root.AddChild(parent);

			// Add a new Skeleton3D
			Skeleton3D skeleton = new();
			skeleton.Name = bones.Name + "_skeleton";
			parent.AddChild(skeleton);

			// Define a placeholder mesh
			SphereMesh sphere = new();
			sphere.Radius = .05f;
			sphere.Height = sphere.Radius * 2;

			// Iterate through the skeleton's bones and add them to the Godot Skeleton
			foreach (var child in bones.Children)
			{
				// For every bone in the Pure3D Skeleton
				if (child is SkeletonJoint joint)
				{
					// Add bone to Skeleton3D
					int boneIndex = skeleton.AddBone(joint.Name);
					if (boneIndex != joint.SkeletonParent)
						skeleton.SetBoneParent(boneIndex, (int)joint.SkeletonParent);

					// Set the bone's rest pose
					Pure3D.Matrix restPose = joint.RestPose;

					Transform3D boneTransform = new(new Basis(
						new Godot.Vector3(restPose.M11, restPose.M12, restPose.M13),
						new Godot.Vector3(restPose.M21, restPose.M22, restPose.M23),
						new Godot.Vector3(restPose.M31, restPose.M32, restPose.M33)
					), new Godot.Vector3(restPose.M41, restPose.M42, restPose.M43));
					skeleton.SetBoneRest(boneIndex, boneTransform);

					// Add a primitive sphere to the bone to indicate its position
					BoneAttachment3D attachment = new();
					attachment.Name = joint.Name;
					attachment.BoneName = joint.Name;
					attachment.BoneIdx = boneIndex;
					attachment.SetUseExternalSkeleton(true);
					attachment.SetExternalSkeleton("../" + skeleton.Name);
					parent.AddChild(attachment);

					MeshInstance3D indicator = new();
					indicator.Mesh = sphere;
					attachment.AddChild(indicator);

					// Set the skeleton to its rest pose
					skeleton.ResetBonePoses();
				}
			}
		}

		// View the Skeleton
		View3dScene(bones);
	}

	/// <summary>
	/// Loads a Pure3D Mesh as a group of MeshInstance3Ds
	/// </summary>
	/// <param name="mesh">Pure3D Mesh to be loaded</param>
	private void LoadMesh(Pure3D.Chunks.Mesh mesh)
	{
		if (!_3d_scenes.ContainsKey(mesh))
		{
			// If the Mesh has not been loaded yet
			// Load the Mesh from the chunk
			// Add a Node to the scene tree for the mesh to be under
			Node3D parent = new()
			{
				Name = "M_" + mesh.Name,
				Visible = false
			};

			_3d_scenes.Add(mesh, parent);
			_3d_root.AddChild(parent);

			// Iterate through the Mesh's children
			foreach (Chunk chunk in mesh.Children)
			{
				if (chunk is PrimitiveGroup prim)
				{
					// If the child is a Primitive Group
					// Create a Surface Tool for generating a mesh
					// and an ArrayMesh to store the mesh
					var st = new SurfaceTool();
					ArrayMesh newMesh;

					// Set the Tool's Primitive Type
					switch (prim.PrimitiveType)
					{
						case PrimitiveGroup.PrimitiveTypes.TriangleList:
							st.Begin(Godot.Mesh.PrimitiveType.Triangles);
							break;

						case PrimitiveGroup.PrimitiveTypes.TriangleStrip:
							st.Begin(Godot.Mesh.PrimitiveType.TriangleStrip);
							break;

						case PrimitiveGroup.PrimitiveTypes.LineList:
							st.Begin(Godot.Mesh.PrimitiveType.Lines);
							break;

						case PrimitiveGroup.PrimitiveTypes.LineStrip:
							st.Begin(Godot.Mesh.PrimitiveType.LineStrip);
							break;
					}

					st.SetMaterial(meshMaterial);

					// Get normals
					var normals = (NormalList)prim.Children.Find(x => x is NormalList);
					var colours = (ColourList)prim.Children.Find(x => x is ColourList);
					var uvs = (UVList)prim.Children.Find(x => x is UVList);
					var verts = (PositionList)prim.Children.Find(x => x is PositionList);

					for (uint i = 0; i < prim.NumVertices; i++)
					{
						// Set the normal of the next vertex
						if (normals != null)
						{
							Pure3D.Vector3 normal = normals.Normals[i];
							st.SetNormal(new Godot.Vector3(-normal.X, -normal.Y, -normal.Z));
						}

						// Set the colour of the next vertex
						if (colours != null)
						{
							uint colour = colours.Colours[i];
							st.SetColor(Util.GetColour(colour));
						}

						// Set the UV of the next vertex
						if (uvs != null)
						{
							Pure3D.Vector2 uv = uvs.UVs[i];
							st.SetUV(new Godot.Vector2(uv.X, uv.Y));
						}

						// Add the next vertex
						Pure3D.Vector3 vert = verts.Positions[i];
						st.AddVertex(new Godot.Vector3(vert.X, vert.Y, vert.Z));
					}

					// Add tangents to the Mesh
					if (normals != null && uvs != null) st.GenerateTangents();

					// Add indices
					var indices = (IndexList)prim.Children.Find(x => x is IndexList);

					if (indices != null)
						foreach (uint index in indices.Indices)
						{
							st.AddIndex((int)index);
						}

					// Finish generating the new Mesh and add it to the scene
					newMesh = st.Commit();
					MeshInstance3D newInstance = new();
					newInstance.Name = prim.ShaderName + "_" + chunk.GetHashCode();
					newInstance.Mesh = newMesh;
					parent.AddChild(newInstance);
				}
			}
		}

		// View the Mesh
		View3dScene(mesh);
	}
	#endregion

	#region Exporting
	/// <summary>
	/// Set up exporting textures
	/// </summary>
	private void Export2Png()
	{
		// Show a File Dialog for the user to select where to save the texture
		DisplayServer.FileDialogShow(
			"Save as PNG",
			DirAccess.GetDriveName(0),
			"new_texture.png",
			true,
			DisplayServer.FileDialogMode.SaveFile,
			_texFilters,
			new Callable(this, MethodName.SaveAsPng)
		);
	}

	/// <summary>
	/// Finish exporting a texture
	/// </summary>
	/// <param name="status">Whether the user wants to save the file</param>
	/// <param name="selected_paths">Single-element array with the path the file should be saved to</param>
	/// <param name="selected_filter_index">What file extension was chosen for the file</param>
	private void SaveAsPng(bool status, string[] selected_paths, int selected_filter_index)
	{
		if (status)
		{
			// If the user wants to save the file
			// Get the first path they want to save to
			// As the array only contains one element
			string path = selected_paths[0];

			// If the user saved the file with an incorrect extension
			if (!path.EndsWith(".png"))
			{
				path += ".png";
			}

			// Write the file
			_texture_view.Texture.GetImage().SavePng(path);
		}
		else
		{
			// If the user cancelled the saving
			GD.Print("Cancelled saving the PNG file!");
		}
	}

	/// <summary>
	/// Set up exporting 3D scenes
	/// </summary>
	private void Export2Gltf()
	{
		// Load a new glTF document and a new glTF state
		_document = new GltfDocument();
		_state = new GltfState();

		// Store Godot data as glTF in the state
		_document.AppendFromScene(_currentNode3D, _state);

		// Show a File Dialog for the user to select where to save the glTF data
		DisplayServer.FileDialogShow(
			"Save as glTF file",
			DirAccess.GetDriveName(0),
			"new_model.gltf",
			true,
			DisplayServer.FileDialogMode.SaveFile,
			_gltfFilters,
			new Callable(this, MethodName.SaveAsGltf)
		);
	}

	/// <summary>
	/// Finish exporting a 3D scene
	/// </summary>
	/// <param name="status">Whether the user wants to save the file</param>
	/// <param name="selected_paths">Single-element array with the path the file should be saved to</param>
	/// <param name="selected_filter_index">What file extension was chosen for the file</param>
	private void SaveAsGltf(bool status, string[] selected_paths, int selected_filter_index)
	{
		if (status)
		{
			// If the user wants to save the file
			// Get the first path they want to save to
			// As the array only contains one element
			string path = selected_paths[0];

			// If the user saved the file with an incorrect extension
			if (!path.EndsWith(".gltf") && !path.EndsWith(".glb"))
			{
				// Add the correct extension
				if (selected_filter_index == 0)
				{
					// If the user selected .gltf
					path += ".gltf";
				}
				else
				{
					// If the user selected .glb
					path += ".glb";
				}
			}

			// Write the file
			_document.WriteToFilesystem(_state, path);
		}
		else
		{
			// If the user cancelled the saving
			GD.Print("Cancelled saving the glTF file!");
		}
	}
	#endregion
}