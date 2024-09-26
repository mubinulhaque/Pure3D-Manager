using System;
using System.Collections.Generic;
using System.Numerics;
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
	private SubViewportContainer _3d_root; // Scene used to display 3D assets
	[Export]
	private SubViewport _3d_view; // Used for viewing 3D scenes
	#endregion

	#region Private Variables
	// Collection of each Pure3D Image and their associated texture
	private readonly Dictionary<Pure3D.Chunks.Image, Godot.ImageTexture> _textures = new();
	// Collection of each 3D item in the tree and their associated 3D scene
	private readonly Dictionary<TreeItem, Node3D> _viewables3D = new();
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
	#endregion

	/// <summary>
	/// Views a selected chunk in the appropriate viewer
	/// </summary>
	/// <param name="chunk">Pure3D chunk to be viewed</param>
	public void ViewChunk(Pure3D.Chunk chunk)
	{
		// TODO: Change viewer based on chunk
		GD.Print(chunk.ToShortString() + " is clicked!");
	}

	#region 2D Chunk Loading
	/// <summary>
	/// Views a Pure3D Image in a Texture Rect
	/// </summary>
	/// <param name="image">Pure3D Image chunk to be viewed</param>
	public void LoadImage(Pure3D.Chunks.Image image)
	{
		if (!_textures.ContainsKey(image))
		{
			// If the Image has not been loaded yet
			// Load the Image from the chunk
			Godot.Image newImage = new Godot.Image();
			Error err = newImage.LoadPngFromBuffer(image.LoadImageData());

			if (err == Error.Ok)
			{
				// If there are no problems loading the image
				// Convert it into a ImageTexture and add it to the dictionary
				_textures.Add(image, ImageTexture.CreateFromImage(newImage));
			}
			else
			{
				// If there are problems loading the image
				GD.PrintErr("Error while loading the current texture: " + err);
				return;
			}
		}

		// Display the associated ImageTexture
		_texture_view.Texture = _textures[image];
	}
	#endregion

	#region 3D Chunk Loading
	/// <summary>
	/// Loads a Pure3D Skeleton as a Skeleton3D
	/// </summary>
	/// <param name="bones">Pure3D Skeleton to be loaded</param>
	/// <returns></returns>
	public Node3D LoadSkeleton(Pure3D.Chunks.Skeleton bones)
	{
		// If this child is a Pure3D Skeleton
		// Add a parent Node
		Node3D parentNode = new Node3D();
		parentNode.Name = "Skel_" + bones.Name;
		_3d_root.AddChild(parentNode);

		// Add a new Skeleton3D
		Skeleton3D skeleton = new Skeleton3D();
		skeleton.Name = bones.Name + "_skeleton";
		parentNode.AddChild(skeleton);

		// Define a placeholder mesh
		SphereMesh sphere = new SphereMesh();
		sphere.Radius = .05f;
		sphere.Height = sphere.Radius * 2;

		// Iterate through the skeleton's bones and add them to the Godot Skeleton
		foreach (var child in bones.Children)
		{
			// For every bone in the Pure3D Skeleton
			if (child is Pure3D.Chunks.SkeletonJoint joint)
			{
				// Add bone to Skeleton3D
				int boneIndex = skeleton.AddBone(joint.Name);
				if (boneIndex != joint.SkeletonParent)
					skeleton.SetBoneParent(boneIndex, (int)joint.SkeletonParent);

				// Set the bone's rest pose
				Pure3D.Matrix restPose = joint.RestPose;

				Transform3D boneTransform = new Transform3D(new Basis(
					new Godot.Vector3(restPose.M11, restPose.M12, restPose.M13),
					new Godot.Vector3(restPose.M21, restPose.M22, restPose.M23),
					new Godot.Vector3(restPose.M31, restPose.M32, restPose.M33)
				), new Godot.Vector3(restPose.M41, restPose.M42, restPose.M43));
				skeleton.SetBoneRest(boneIndex, boneTransform);

				// Add a primitive sphere to the bone to indicate its position
				BoneAttachment3D attachment = new BoneAttachment3D();
				attachment.Name = joint.Name;
				attachment.BoneName = joint.Name;
				attachment.BoneIdx = boneIndex;
				attachment.SetUseExternalSkeleton(true);
				attachment.SetExternalSkeleton("../" + skeleton.Name);
				parentNode.AddChild(attachment);

				MeshInstance3D indicator = new MeshInstance3D();
				indicator.Mesh = sphere;
				attachment.AddChild(indicator);

				// Set the skeleton to its rest pose
				skeleton.ResetBonePoses();
			}
		}

		// Return the parent node after making it invisible
		parentNode.Visible = false;
		return parentNode;
	}

	/// <summary>
	/// Loads a Pure3D Mesh as a group of MeshInstance3Ds
	/// </summary>
	/// <param name="bones">Pure3D Mesh to be loaded</param>
	/// <returns></returns>
	public Node3D LoadMesh(Pure3D.Chunks.Mesh mesh)
	{
		// Add a Node to the scene tree for the mesh to be under
		Node3D parent = new();
		parent.Name = "M_" + mesh.Name;
		parent.Visible = false;
		_3d_root.AddChild(parent);

		// Iterate through the Mesh's children
		foreach (Pure3D.Chunk chunk in mesh.Children)
		{
			if (chunk is Pure3D.Chunks.PrimitiveGroup prim)
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

				// Set the mesh to cull front faces
				// Godot uses clockwise winding order to determine front-faces of
				// primitive triangles, whereas Pure3D uses anti-clockwise
				// So, without this, Godot displays the mesh's back faces only
				StandardMaterial3D mat = new();
				mat.CullMode = BaseMaterial3D.CullModeEnum.Front;
				st.SetMaterial(mat);

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
						st.SetColor(new Color(colour));
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
						st.SetSmoothGroup(index);
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

		return parent;
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
			GD.Print("Cancelled saving the glTF file!");
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
		_document.AppendFromScene(_3d_view, _state);

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