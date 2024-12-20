using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Pure3D;
using Pure3D.Chunks;

/// <summary>
/// Displays Pure3D textures and 3D assets
/// </summary>
public partial class Viewer : Node
{
	#region Export Variables
	/// <summary>
	/// Displays 2D assets
	/// </summary>
	[Export] private Container _2d_root;
	/// <summary>
	/// Views textures
	/// </summary>
	[Export] private TextureRect _texture_view;
	/// <summary>
	/// Displays 3D assets
	/// </summary>
	[Export] private Container _3d_root;
	/// <summary>
	/// Views 3D scenes
	/// </summary>
	[Export] private SubViewport _3d_view;
	/// <summary>
	/// Where the current 3D model is placed
	/// </summary>
	[Export] public Node3D _origin;
	/// <summary>
	/// Camera for 3D scenes
	/// </summary>
	[Export] public Camera3D cam;
	/// <summary>
	/// Text box defining the 
	/// distance of the camera from the origin
	/// </summary>
	[Export] public SpinBox zoomEdit;
	/// <summary>
	/// Displays details of Chunks
	/// </summary>
	[Export] private Tree _details;
	/// <summary>
	/// Plays skeletal animations
	/// </summary>
	[Export] private AnimationPlayer _animator;
	/// <summary>
	/// Plays the currently selected skeletal animation
	/// </summary>
	[Export] private Button _playButton;
	#endregion

	#region Private Variables
	/// <summary>
	/// Collection of each Pure3D Image and their associated texture
	/// </summary>
	private readonly Dictionary<ImageData, ImageTexture> _textures = new();
	/// <summary>
	/// Collection of each 3D item in the tree and their associated 3D scene
	/// </summary>
	private readonly Dictionary<Chunk, Node3D> _3d_scenes = new();
	/// <summary>
	/// Collection of each 3D item in the tree and their position in the world
	/// </summary>
	private readonly Dictionary<Chunk, Vector3> _3d_positions = new();
	/// <summary>
	/// Node3D that is currently being viewed and can be exported
	/// </summary>
	private Node3D _currentNode3D = null;
	/// <summary>
	/// Formats and file extensions that a glTF file can be exported to
	/// </summary>
	private string[] _gltfFilters = { "*.glb;glTF binary file", "*.gltf;glTF text file" };
	/// <summary>
	/// Formats and file extensions that a texture can be exported to
	/// </summary>
	private string[] _texFilters = { "*.png;PNG texture file" };
	/// <summary>
	/// Converts a 3D scene into glTF data
	/// </summary>
	private GltfDocument _document = null;
	/// <summary>
	/// Stores glTF data
	/// </summary>
	private GltfState _state = null;
	/// <summary>
	/// Material used for generated Pure3D meshes
	/// </summary>
	private StandardMaterial3D _meshMaterial;
	/// <summary>
	/// Distance from the 3D camera to the origin
	/// </summary>
	private float _zoomLevel = 0f;
	/// <summary>
	/// Distance from the 3D camera to the origin
	/// </summary>
	private AnimationLibrary _library;
	#endregion

	public override void _Ready()
	{
		base._Ready();

		// Set the current zoom level properly
		_zoomLevel = cam.Position.Z;
		zoomEdit.Value = _zoomLevel;

		// Set all generated meshes to view vertex colours
		_meshMaterial = new()
		{
			CullMode = BaseMaterial3D.CullModeEnum.Disabled,
			VertexColorUseAsAlbedo = true
		};

		// Instantiate the Animation Library
		_library = new();
		_animator.AddAnimationLibrary("anims", _library);
	}

	/// <summary>
	/// Views a selected chunk in the appropriate viewer
	/// </summary>
	/// <param name="chunk">Pure3D chunk to be viewed</param>
	public void ViewChunk(Chunk chunk)
	{
		// Make the current Node3D and "Play Animation" button invisible
		if (_currentNode3D != null) _currentNode3D.Visible = false;
		if (_playButton != null) _playButton.Visible = false;

		// Load the chunk based on its type
		switch (chunk)
		{
			case Skeleton skel:
				LoadSkeleton(skel);
				break;

			case MeshChunk mesh:
				LoadMesh(mesh);
				break;

			case ImageChunk image:
				LoadImage(image);
				break;

			case ImageData image:
				LoadImageData(image);
				break;

			case AnimationChunk anim:
				LoadAnimation(anim);
				break;

			case Intersect intersect:
				LoadIntersect(intersect);
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

	/// <summary>
	/// Changes the distance of the camera from the origin
	/// </summary>
	/// <param name="value">New zoom level</param>
	public void ChangeZoom(float value)
	{
		cam.Position = new Vector3(
			cam.Position.X,
			cam.Position.Y,
			value
		);
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
	private void LoadImage(ImageChunk image)
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
			Image newImage = new();
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
		_origin.Position = _3d_positions[chunk];
		_3d_root.Visible = true;
	}

	/// <summary>
	/// Loads a Pure3D Skeleton as a Skeleton3D
	/// </summary>
	/// <param name="bones">Pure3D Skeleton to be loaded</param>
	private void LoadSkeleton(Skeleton bones)
	{
		if (!_3d_scenes.ContainsKey(bones))
		{
			// If the Skeleton has not been loaded yet
			// Load the Skeleton from the chunk
			// Add a parent Node and make it invisible
			Node3D parent = new()
			{
				Name = "Skel_" + bones.Name,
				Visible = false
			};
			_3d_positions.Add(bones, Vector3.Zero);
			_3d_scenes.Add(bones, parent);
			_3d_root.AddChild(parent);

			// Add a new Skeleton3D
			Skeleton3D skeleton = new()
			{
				Name = bones.Name + "_skeleton"
			};
			parent.AddChild(skeleton);
			_animator.RootNode = skeleton.GetPath();

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
					Matrix restPose = joint.RestPose;

					Transform3D boneTransform = new(new Basis(
						new Vector3(restPose.M11, restPose.M12, restPose.M13),
						new Vector3(restPose.M21, restPose.M22, restPose.M23),
						new Vector3(restPose.M31, restPose.M32, restPose.M33)
					), new Vector3(restPose.M41, restPose.M42, restPose.M43));
					skeleton.SetBoneRest(boneIndex, boneTransform.Orthonormalized());

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
	private void LoadMesh(MeshChunk mesh)
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

			// For calculating the average vertex position later
			Vector3 totalPos = Vector3.Zero;
			uint totalVerts = 0;

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
							st.Begin(Mesh.PrimitiveType.Triangles);
							break;

						case PrimitiveGroup.PrimitiveTypes.TriangleStrip:
							st.Begin(Mesh.PrimitiveType.TriangleStrip);
							break;

						case PrimitiveGroup.PrimitiveTypes.LineList:
							st.Begin(Mesh.PrimitiveType.Lines);
							break;

						case PrimitiveGroup.PrimitiveTypes.LineStrip:
							st.Begin(Mesh.PrimitiveType.LineStrip);
							break;
					}

					st.SetMaterial(_meshMaterial);

					// Get List chunks
					Chunk colourList = prim.Children.Find(x => x is ColourList);
					Chunk matrixList = prim.Children.Find(x => x is MatrixList);
					Chunk matrixPalette = prim.Children.Find(x => x is MatrixPalette);
					Chunk normalList = prim.Children.Find(x => x is NormalList);
					Chunk packedNormalList = prim.Children.Find(x => x is PackedNormalList);
					Chunk uvList = prim.Children.Find(x => x is UVList);
					Chunk vertList = prim.Children.Find(x => x is PositionList);
					Chunk weightList = prim.Children.Find(x => x is WeightList);

					for (uint i = 0; i < prim.NumVertices; i++)
					{
						// Set the colour of the next vertex
						if (colourList is ColourList colours)
						{
							uint colour = colours.Colours[i];
							st.SetColor(Util.GetColour(colour));
						}

						// Set the bones of the next vertex
						if (matrixList is MatrixList matrices
							&& matrixPalette is MatrixPalette palette)
						{
							byte[] bones = matrices.Matrices[i];
							st.SetBones(new int[] {
								(int)palette.Matrices[bones[3]],
								(int)palette.Matrices[bones[2]],
								(int)palette.Matrices[bones[1]],
								(int)palette.Matrices[bones[0]]
							});
						}

						// Set the normal of the next vertex
						if (normalList is NormalList normals)
							st.SetNormal(normals.Normals[i]);

						if (packedNormalList is PackedNormalList pnl)
							st.SetNormal(pnl.Normals[i]);

						// Set the UV of the next vertex
						if (uvList is UVList uvs)
							st.SetUV(uvs.UVs[i]);

						// Set the bone weights of the next vertex
						if (weightList is WeightList weights)
						{
							Vector3 weight = weights.Weights[i];
							st.SetWeights(new float[4] { weight.X, weight.Y, weight.Z, 0 });
						}
						else if (matrixList is MatrixList)
							st.SetWeights(new float[4] { 1, 0, 0, 0 });

						// Add the next vertex
						if (vertList is PositionList verts)
						{
							st.AddVertex(verts.Positions[i]);
							totalPos += verts.Positions[i];
						}
					}

					// Add the number of vertices to the total
					totalVerts += prim.NumVertices;

					// Add tangents to the Mesh
					if (normalList is NormalList && uvList is UVList) st.GenerateTangents();

					// Add indices backwards
					// Godot uses clockwise winding order to determine front-faces of
					// primitive triangles, whereas Pure3D uses anti-clockwise
					// So, without this, Godot displays the mesh's back faces only
					var indexList = prim.Children.Find(x => x is IndexList);

					if (indexList is IndexList indices)
						for (int i = indices.Indices.Length - 1; i >= 0; i--)
							st.AddIndex((int)indices.Indices[i]);

					// Finish generating the new Mesh and add it to the scene
					newMesh = st.Commit();
					MeshInstance3D newInstance = new();
					newInstance.Name = prim.ShaderName + "_" + chunk.GetHashCode();
					newInstance.Mesh = newMesh;
					parent.AddChild(newInstance);

					// Bind the Mesh to a Skeleton3D, if needed
					if (GetNode(_animator.RootNode) is Skeleton3D skel
					&& mesh is SkinChunk)
						newInstance.Skeleton = skel.GetPath();
				}
			}

			// Add the average vertex position to the Dictionary
			_3d_positions.Add(mesh, totalPos / totalVerts);
		}

		// View the Mesh
		View3dScene(mesh);
	}

	/// <summary>
	/// Loads a Pure3D Animation as a Godot Animation
	/// </summary>
	/// <param name="anim">Pure3D Animation to be loaded</param>
	private void LoadAnimation(AnimationChunk anim)
	{
		if (!_library.HasAnimation(anim.Name))
		{
			// If the Godot Animation for this has not been created yet
			// Create a new Godot Animation
			Animation newAnim = new();
			GD.Print($"Creating animation {anim.Name}...");

			// Set the frame rate of the Godot Animation, if not already set
			if (!Mathf.IsEqualApprox(newAnim.Step, 1f / anim.FrameRate))
				newAnim.Step = 1f / anim.FrameRate;

			// Set the length of the Godot Animation
			newAnim.Length = anim.NumberOfFrames / anim.FrameRate;

			// Set the Godot Animation to be looping, if needed
			newAnim.LoopMode = anim.Looping == 1 ? Animation.LoopModeEnum.Linear : Animation.LoopModeEnum.None;

			// Iterate through each group in the Animation's Group List
			// Iterate through each group's Animation Channels
			// Add tracks to the Godot Animation
			var list = (AnimationGroupList)anim.Children.Find(x => x is AnimationGroupList);

			if (list != null)
				foreach (Chunk group in list.Children)
					if (group is AnimationGroup ag)
						foreach (Chunk channel in group.Children)
							switch (channel)
							{
								case CompressedQuaternionChannel cqc:
									if (cqc.Parameter == "ROT")
									{
										AddRotationFrames(
											newAnim,
											ag.Name,
											cqc.Frames,
											cqc.Values
										);
									}
									else
									{
										GD.PushError($"{anim.Name}'s Compressed Quaternion Channel is not for rotation!");
									}
									break;

								case QuaternionChannel qc:
									if (qc.Parameter == "ROT")
									{
										AddRotationFrames(
											newAnim,
											ag.Name,
											qc.Frames,
											qc.Values
										);
									}
									else
									{
										GD.PushError($"{anim.Name}'s Quaternion Channel is not for rotation!");
									}
									break;

								case Vector1Channel v1c:
									if (v1c.Parameter == "TRAN")
									{
										// Add a new position track for the bone to the animation
										int v1cTrack = newAnim.AddTrack(Animation.TrackType.Position3D);
										newAnim.TrackSetPath(v1cTrack, $":{ag.Name}");

										// Add a new position frame
										for (int i = 0; i < v1c.NumberOfFrames; i++)
										{
											Vector3 pos = Vector3.Zero;

											pos.X = v1c.Constants.X;
											pos.Y = v1c.Constants.Y;
											pos.Z = v1c.Constants.Z;

											pos[v1c.Mapping] = v1c.Values[i];

											newAnim.PositionTrackInsertKey(
												v1cTrack,
												v1c.Frames[i] * newAnim.Step,
												pos
											);
										}
									}
									else
									{
										GD.PushError($"{anim.Name}'s Vector 1 Channel is not for translation!");
									}
									break;

								case Vector2Channel v2c:
									if (v2c.Parameter == "TRAN")
									{
										// Add a new position track for the bone to the animation
										int v2cTrack = newAnim.AddTrack(Animation.TrackType.Position3D);
										newAnim.TrackSetPath(v2cTrack, $":{ag.Name}");

										// Add a new position frame
										for (int i = 0; i < v2c.NumberOfFrames; i++)
										{
											Vector3 pos = Vector3.Zero;

											pos.X = v2c.Constants.X;
											pos.Y = v2c.Constants.Y;
											pos.Z = v2c.Constants.Z;

											int[,] indices = { { 1, 2 }, { 0, 2 }, { 0, 1 } };

											pos[indices[v2c.Mapping, 0]] = v2c.Values[i].X;
											pos[indices[v2c.Mapping, 1]] = v2c.Values[i].Y;

											newAnim.PositionTrackInsertKey(
												v2cTrack,
												v2c.Frames[i] * newAnim.Step,
												pos
											);
										}
									}
									else
									{
										GD.PushError($"{anim.Name}'s Vector 2 Channel is not for translation!");
									}
									break;

								case Vector3Channel v3c:
									if (v3c.Parameter == "TRAN")
									{
										// Add a new position track for the bone to the animation
										int v3cTrack = newAnim.AddTrack(Animation.TrackType.Position3D);
										newAnim.TrackSetPath(v3cTrack, $":{ag.Name}");

										// Add a new position frame
										for (int i = 0; i < v3c.NumberOfFrames; i++)
										{
											Vector3 pos = Vector3.Zero;

											pos.X = v3c.Values[i].X;
											pos.Y = v3c.Values[i].Y;
											pos.Z = v3c.Values[i].Z;

											newAnim.PositionTrackInsertKey(
												v3cTrack,
												v3c.Frames[i] * newAnim.Step,
												pos
											);
										}
									}
									else
									{
										GD.PushError($"{anim.Name}'s Vector 3 Channel is not for translation!");
									}
									break;
							}

			// Add the Animation to the Library
			_library.AddAnimation(anim.Name, newAnim);
		}

		// Allow the user to play the current animation
		// by making a valid Skeleton3D visible
		// and assigning the current animation to the Animation Player
		_animator.AssignedAnimation = $"anims/{anim.Name}";
		Node maybeSkel = GetNode(_animator.RootNode);

		if (maybeSkel is Skeleton3D skel)
		{
			_playButton.Visible = true;
			_2d_root.Visible = false;
			_currentNode3D = (Node3D)skel.GetParent();
			_3d_root.Visible = true;
		}
	}

	/// <summary>
	/// Adds rotation frames from any Quaternion Channel's values
	/// </summary>
	/// <param name="anim">Godot Animation to add frames to</param>
	/// <param name="bone">Name of the bone to animate</param>
	/// <param name="times">Frames to add keys to</param>
	/// <param name="values">Array of rotations</param>
	private static void AddRotationFrames(
		Animation anim,
		string bone,
		ushort[] times,
		Quaternion[] values
	)
	{
		// Add a new rotation track for the bone to the animation
		int track = anim.AddTrack(Animation.TrackType.Rotation3D);
		anim.TrackSetPath(track, $":{bone}");

		// Add the rotation frames
		for (int i = 0; i < times.Length; i++)
		{
			Quaternion quat = Quaternion.Identity;

			quat.W = values[i].W;
			quat.X = values[i].X;
			quat.Y = values[i].Y;
			quat.Z = values[i].Z;

			anim.RotationTrackInsertKey(
				track,
				times[i] * anim.Step,
				quat
			);
		}
	}

	public void PlayCurrentAnimation()
	{
		if (_animator != null)
			if (GetNode(_animator.RootNode) is Skeleton3D skel)
				_animator.Play();
	}

	/// <summary>
	/// Loads a Pure3D Intersect and builds it as a Godot Mesh and Collision Shape
	/// </summary>
	/// <param name="intersect">Pure3D Intersect to be built</param>
	private void LoadIntersect(Intersect intersect)
	{
		if (!_3d_scenes.ContainsKey(intersect))
		{
			// Create a new Surface Tool
			SurfaceTool st = new();
			st.Begin(Mesh.PrimitiveType.Triangles);

			// Normally, we add normals here
			// But Godot doesn't allow adding face normals yet

			// Add the next vertex
			Vector3 totalPos = Vector3.Zero;

			foreach (Vector3 pos in intersect.Positions)
			{
				st.AddVertex(pos);
				totalPos += pos;
			}

			// Calculate the average vertex position
			_3d_positions.Add(intersect, totalPos / intersect.Positions.Length);

			// Add the next index
			for (int i = intersect.Indices.Length - 1; i >= 0; i--)
				st.AddIndex((int)intersect.Indices[i]);

			// Add the intersect Mesh to the scene
			ArrayMesh newIntersect = st.Commit();
			MeshInstance3D newInstance = new()
			{
				Mesh = newIntersect,
				Name = $"Intersect_{GetTree().GetNodeCountInGroup("intersects")}",
				Position = Vector3.Zero,
				Visible = false
			};
			AddChild(newInstance);
			newInstance.AddToGroup("intersects");
			_3d_scenes.Add(intersect, newInstance);
		}

		View3dScene(intersect);
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
			"new_model.glb",
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
					// If the user selected .glb
					path += ".glb";
				}
				else
				{
					// If the user selected .gltf
					path += ".glt";
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