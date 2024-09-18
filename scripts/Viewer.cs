using Godot;
using System;
using System.Collections.Generic;

public partial class Viewer : Control
{
	[Export]
	private Label _errorMessage;
	[Export]
	private Tree _tree;
	[Export]
	private View3D _view;
	[Export]
	private Button _exportButton;

	private Dictionary<TreeItem, Pure3D.Chunk> _chunks = new Dictionary<TreeItem, Pure3D.Chunk>();
	private Node3D _exportNode = null;
	private string[] gltfFilters = {"*.gltf;glTF text file", "*.glb;glTF binary file"};
	private GltfDocument _document = null;
	private GltfState _state = null;

	private void OnFilenameSubmitted(String newText) {
		// Load the model at the specified path
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

	private void LoadChunk(Pure3D.Chunk chunk, TreeItem parent)
	{
		// Add a TreeItem and set its text to the chunk's string
		TreeItem item = _tree.CreateItem(parent);
		item.SetText(0, chunk.ToShortString());
		item.SetTooltipText(0, chunk.ToString());

		// Add the chunk to the dictionary
		_chunks.Add(item, chunk);

		foreach (var child in chunk.Children)
		{
			if (child is Pure3D.Chunks.Skeleton && _view != null)
			{
				// Load the Skeleton's Joints
				_view.LoadSkeleton((Pure3D.Chunks.Skeleton)child);
			}

			LoadChunk(child, item);
		}
	}

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

	public void SetExportNode(Node3D rootNode)
	{
		// Set the new node and disable the export button if the new node is null
		_exportNode = rootNode;
		_exportButton.Disabled = rootNode == null ? true : false;
	}

	private void Export2Gltf()
	{
		// Load a new glTF document (for exporting glTF data)
		// and a new glTF state (for storing glTF data)
		_document = new GltfDocument();
		_state = new GltfState();

		// Store Godot data as glTF in the state
		_document.AppendFromScene(_exportNode, _state);

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

	private void SaveAsGltf(bool status, string[] selected_paths, int selected_filter_index)
	{
		if (status)
		{
			// If the user wants to save the file
			string path = selected_paths[0];

			if (!path.EndsWith(".gltf") && !path.EndsWith(".glb"))
			{
				if (selected_filter_index == 0)
				{
					path += ".gltf";
				} else
				{
					path += ".glb";
				}
			}

			_document.WriteToFilesystem(_state, path);
		} else
		{
			// If the user cancelled the saving
			GD.Print("Cancelled saving the glTF file!");
		}
	}
}
