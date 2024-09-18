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
		_exportNode = rootNode;
		_exportButton.Disabled = rootNode == null ? true : false;
	}

	private void Export2Gltf()
	{
		
	}
}
