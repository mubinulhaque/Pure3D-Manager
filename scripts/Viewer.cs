using Godot;
using System;
using System.Collections.Generic;

public partial class Viewer : Control
{
	[Export]
	private Label _errorMessage;
	[Export]
	private Tree _tree; // Displays the chunks of a P3D file in a suitable hierarchy
	[Export]
	private TextureRect _view2D; // Used for viewing textures
	[Export]
	private Container _view2DParent; // Container parent of the 2D TextureRect
	[Export]
	private View3D _view3D; // Used for viewing 3D objects from P3D files
	[Export]
	private SubViewportContainer _view3DParent; // Container parent of the 3D viewport
	[Export]
	private Button _exportButton; // Used for exporting assets

	// Collection of each item in the tree and its associated chunk
	private readonly Dictionary<TreeItem, Pure3D.Chunk> _chunks = new Dictionary<TreeItem, Pure3D.Chunk>();
	// Formats and file extensions that a glTF file can be exported to
	private string[] gltfFilters = {"*.gltf;glTF text file", "*.glb;glTF binary file"};
	// Formats and file extensions that a texture can be exported to
	private string[] texFilters = {"*.png;PNG texture file"};
	// Converts a 3D scene into glTF data
	private GltfDocument _document = null;
	// Stores glTF data
	private GltfState _state = null;
	// Collection of each 3D item in the tree and their associated 3D scene
	private readonly Dictionary<TreeItem, Pure3D.Chunks.Image> _viewables2D = new Dictionary<TreeItem, Pure3D.Chunks.Image>();
	// Collection of each 3D item in the tree and their associated 3D scene
	private readonly Dictionary<TreeItem, Node3D> _viewables3D = new Dictionary<TreeItem, Node3D>();
	// Texture that is currently being viewed and can be exported
	private Pure3D.Chunks.Image _currentTexture = null;
	// Image that displays the currently viewed texture
	private Image _currentImage = new Image();
	// Node3D that is currently being viewed and can be exported
	private Node3D _currentNode3D = null;

	/// <summary>
	/// Load a P3D file
	/// </summary>
	/// <param name="newText">Path of the file</param>
	private void OnFilenameSubmitted(String newText) {
		String filename = newText.StripEdges();

		if (FileAccess.FileExists(filename))
		{
			// Empty the tree
			TreeItem root = _tree.GetRoot();

			if (root != null)
			{
				while(root.GetChildCount() > 0)
				{
					root.GetFirstChild().Free();
				}

				root.Free();
			}

			// Load the file's chunks
			var file = new Pure3D.File();
			file.Load(filename);
			LoadChunk(file.RootChunk, null);

			// Reset the error message
			_errorMessage.Text = "";
		} else if (_errorMessage != null)
		{
			_errorMessage.Text = "No file matches " + filename;
		}
	}

	/// <summary>
	/// Recursively loads a chunk and its children
	/// </summary>
	/// <param name="chunk">Root chunk</param>
	/// <param name="parent">TreeItem to parent the child chunk to</param>
	/// <returns></returns>
	private TreeItem LoadChunk(Pure3D.Chunk chunk, TreeItem parent)
	{
		// Add a TreeItem and set its text to the chunk's string
		TreeItem item = _tree.CreateItem(parent);
		item.SetText(0, chunk.ToShortString());
		item.SetTooltipText(0, chunk.ToString());

		// Add the chunk to the dictionary
		_chunks.Add(item, chunk);

		// Loop through the children of each chunk
		foreach (var child in chunk.Children)
		{
			switch (child)
			{
				case Pure3D.Chunks.Skeleton:
					// If the child is a Skeleton
					// Load the Skeleton's Joints
					// Add the Skeleton to the dictionary
					_viewables3D.Add(
						LoadChunk(child, item),
						_view3D.LoadSkeleton((Pure3D.Chunks.Skeleton)child)
					);
					break;
				
				case Pure3D.Chunks.ImageData:
					// If the child is ImageData
					// And the parent is an Image
					if (chunk is Pure3D.Chunks.Image image)
					{
						_viewables2D.Add(item, image);
					}
					break;
				
				default:
					// If the child is not viewable
					// Load it normally
					LoadChunk(child, item);
					break;
			}
		}

		return item;
	}

	/// <summary>
	/// Go through each TreeItem and toggle whether only the chunk's name is displayed
	/// </summary>
	/// <param name="toggled">Wheter to display the chunk's properties</param>
	private void ToggleShortNames(bool toggled)
	{
		TreeItem root = _tree.GetRoot();

		if (root != null)
		{
			foreach (KeyValuePair<TreeItem, Pure3D.Chunk> item in _chunks)
			{
				var newName = toggled ? item.Value.ToShortString() : item.Value.ToString();
				item.Key.SetText(0, newName);
			}
		}
	}

	/// <summary>
	/// Decides whether to export a 3D scene or a texture
	/// </summary>
	private void OnExportButtonClicked()
	{
		if (_view2DParent.Visible)
		{
			Export2Png();
		} else if (_view3DParent.Visible)
		{
			Export2Gltf();
		}
	}

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
			texFilters,
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
			_view2D.Texture.GetImage().SavePng(path);
		} else
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
		_document.AppendFromScene(_currentNode3D, _state);

		// Show a File Dialog for the user to select where to save the glTF data
		DisplayServer.FileDialogShow(
			"Save as glTF file",
			DirAccess.GetDriveName(0),
			"new_model.gltf",
			true,
			DisplayServer.FileDialogMode.SaveFile,
			gltfFilters,
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
				} else
				{
					// If the user selected .glb
					path += ".glb";
				}
			}
			
			// Write the file
			_document.WriteToFilesystem(_state, path);
		} else
		{
			// If the user cancelled the saving
			GD.Print("Cancelled saving the glTF file!");
		}
	}

	/// <summary>
	/// Make the associated scene viewable when a TreeItem is selected
	/// </summary>
	private void OnItemSelected()
	{
		// Make the current Node3D invisible
		if (_currentNode3D != null) _currentNode3D.Visible = false;

		// Make the selected item's associated Node visible
		if (_viewables3D.ContainsKey(_tree.GetSelected()))
		{
			// If the selected TreeItem is associated with a Node3D
			// Enable the export button to export the associated Node3D
			_exportButton.Disabled = false;
			_exportButton.Text = "Export as glTF";

			// And make said Node3D visible
			TreeItem item = _tree.GetSelected();
			_viewables3D[item].Visible = true;
			_currentNode3D = _viewables3D[item];
			
			_view2DParent.Visible = false;
			_view3DParent.Visible = true;
		} else if (_viewables2D.ContainsKey(_tree.GetSelected()))
		{
			// If the selected TreeItem is associated with a texture
			// Enable the export button to export the associated texture
			_exportButton.Disabled = false;
			_exportButton.Text = "Export as PNG";

			// And make said texture visible
			TreeItem item = _tree.GetSelected();
			_currentTexture = _viewables2D[item];
			_view2D.Texture = loadCurrentTexture();

			_view2DParent.Visible = true;
			_view3DParent.Visible = false;
		} else
		{
			// If the selected TreeItem is not associated with any Node
			_exportButton.Disabled = true;
			_exportButton.Text = "Select an exportable asset";
		}
	}

	/// <summary>
	/// Loads the currently viewed texture into a format that a TextureRect supports
	/// </summary>
	/// <returns>Returns </returns>
	private ImageTexture loadCurrentTexture()
	{
		// Load the image from the chunk
		Error err = _currentImage.LoadPngFromBuffer(_currentTexture.LoadImageData());

		if (err == Error.Ok)
		{
			// If there are no problems loading the image
			// Convert it into a ImageTexture and return it
			return ImageTexture.CreateFromImage(_currentImage);
		} else
		{
			// If there are problems loading the image
			GD.PrintErr("Error while loading the current texture: " + err);
			return null;
		}
	}
}
