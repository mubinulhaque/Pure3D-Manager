using Godot;
using Pure3D;
using Pure3D.Chunks;
using System;
using System.Collections.Generic;

/// <summary>
/// Loads Pure3D chunks into a GUI appropriate for viewing
/// </summary>
public partial class Manager : Control
{
	#region Export Variables
	/// <summary>
	/// Displays error messages
	/// </summary>
	[Export]
	private Label _errorMessage;
	/// <summary>
	/// Displays the chunks of a P3D file in a suitable hierarchy
	/// </summary>
	[Export]
	private Godot.Tree _chunk_tree;
	/// <summary>
	/// Views assets
	/// </summary>
	[Export]
	private Viewer _viewer;
	/// <summary>
	/// Views the details of a Chunk
	/// </summary>
	[Export]
	public Detailer _details;
	#endregion

	#region Private Variables
	private bool _useShortNames = true;
	private readonly List<uint> _unknownChunkIds = new List<uint>();
	#endregion

	/// <summary>
	/// Collection of each item in the tree and its associated chunk
	/// </summary>
	private readonly Dictionary<TreeItem, Chunk> _chunks = new Dictionary<TreeItem, Chunk>();

	public override void _Ready()
	{
		// Handle window resizing
		DisplayServer.WindowSetMinSize(new Vector2I(720, 720));
	}

	/// <summary>
	/// Load a P3D file
	/// </summary>
	/// <param name="newText">Path of the file</param>
	private void OnFilenameSubmitted(String newText)
	{
		String filename = newText.StripEdges();

		if (FileAccess.FileExists(filename))
		{
			// Empty the tree
			TreeItem root = _chunk_tree.GetRoot();
			if (root != null)
			{
				root.Free();
				_chunks.Clear();
			}

			// Load the file's chunks
			var file = new Pure3D.File();
			file.Load(filename);
			LoadChunk(file.RootChunk, null);

			// Reset the error message
			_errorMessage.Text = "";
		}
		else if (_errorMessage != null)
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
	private TreeItem LoadChunk(Chunk chunk, TreeItem parent)
	{
		// Add a TreeItem and set its text to the chunk's string
		TreeItem item = _chunk_tree.CreateItem(parent);
		item.SetText(0, _useShortNames ? chunk.ToShortString() : chunk.ToString());
		item.SetTooltipText(0, chunk.ToString());

		// Add the chunk to the dictionary
		_chunks.Add(item, chunk);

		// Push a warning if the chunk is unknown
		if (chunk is Unknown && !_unknownChunkIds.Contains(chunk.Type))
		{
			_unknownChunkIds.Add(chunk.Type);
			GD.PushWarning($"Unknown {chunk.ToShortString()} found!");
		}

		// Recursively load the children of each chunk
		foreach (var child in chunk.Children) LoadChunk(child, item);

		// Collapse everything but the Root chunk
		if (parent != null) item.Collapsed = true;

		return item;
	}

	/// <summary>
	/// Go through each TreeItem and toggle whether only the chunk's name is displayed
	/// </summary>
	/// <param name="toggled">Whether to display the chunk's properties</param>
	private void ToggleShortNames(bool toggled)
	{
		TreeItem root = _chunk_tree.GetRoot();
		_useShortNames = toggled;

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
	/// Make the associated chunk viewable when a TreeItem is selected
	/// </summary>
	private void OnItemSelected()
	{
		_viewer.ViewChunk(_chunks[_chunk_tree.GetSelected()]);
		_details.ViewChunk(_chunks[_chunk_tree.GetSelected()]);
	}
}
