using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
	// Converts a 3D scene into glTF data
	private GltfDocument _document = null;
	// Stores glTF data
	private GltfState _state = null;
	// Collection of each 3D item in the tree and their associated 3D scene
	private readonly Dictionary<TreeItem, Pure3D.Chunks.Image> _viewables2d = new Dictionary<TreeItem, Pure3D.Chunks.Image>();
	// Collection of each 3D item in the tree and their associated 3D scene
	private readonly Dictionary<TreeItem, Node3D> _viewables3d = new Dictionary<TreeItem, Node3D>();
	// Texture that is currently being viewed and can be exported
	private Pure3D.Chunks.Image _currentTexture = null;
	// Node3D that is currently being viewed and can be exported
	private Node3D _currentNode3D = null;

	// Load the P3D file at the specified path
	private void OnFilenameSubmitted(String newText) {
		String filename = newText.StripEdges();

		if (FileAccess.FileExists(filename))
		{
			var file = new Pure3D.File();
			file.Load(filename);
			_errorMessage.Text = "";
			LoadChunk(file.RootChunk, null);
		} else if (_errorMessage != null)
		{
			_errorMessage.Text = "No file matches " + filename;
		}
	}

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
				case Pure3D.Chunks.Skeleton _:
					// If the child is a Skeleton
					// Load the Skeleton's Joints
					// Add the Skeleton to the dictionary
					_viewables3d.Add(
						LoadChunk(child, item),
						_view3D.LoadSkeleton((Pure3D.Chunks.Skeleton)child)
					);
					break;
				
				case Pure3D.Chunks.ImageData:
					// If the child is ImageData
					if (chunk is Pure3D.Chunks.Image)
					{
						GD.Print("Found an image!");
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

	// Go through each TreeItem
	// Toggle whether their short names or their normal names are displayed 
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

	// Set up exporting 3D scenes
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

	// Finish exporting a 3D scene
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

	// Make the associated scene viewable when a TreeItem is selected
	private void OnItemSelected() {
		// Make the current node invisible
		if (_currentNode3D != null) _currentNode3D.Visible = false;

		// Make the selected item's associated Node visible
		if (_viewables3d.ContainsKey(_tree.GetSelected()))
		{
			// If the selected TreeItem is associated with a Node3D
			// Enable the export button to export the associated Node3D
			_exportButton.Disabled = false;
			_exportButton.Text = "Export as glTF";

			// And make said Node3D visible
			TreeItem item = _tree.GetSelected();
			_viewables3d[item].Visible = true;
			_currentNode3D = _viewables3d[item];
			
			_view2DParent.Visible = false;
			_view3DParent.Visible = true;
		} else if (_viewables2d.ContainsKey(_tree.GetSelected()))
		{
			// If the selected TreeItem is associated with a texture
			// Enable the export button to export the associated texture
			_exportButton.Disabled = false;
			_exportButton.Text = "Export as PNG";

			// And make said texture visible
			TreeItem item = _tree.GetSelected();
			_currentTexture = _viewables2d[item];

			_view2DParent.Visible = true;
			_view3DParent.Visible = false;
		} else
		{
			// If the selected TreeItem is not associated with any Node
			_exportButton.Disabled = true;
			_exportButton.Text = "Select an exportable asset";
		}
	}
}
