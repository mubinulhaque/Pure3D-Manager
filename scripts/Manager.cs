using Godot;
using Pure3D;
using System;
using System.Collections.Generic;

/// <summary>
/// Loads Pure3D chunks into a GUI appropriate for viewing
/// </summary>
public partial class Manager : Control
{
	#region Export Variables
	[Export]
	private Label _errorMessage;
	[Export]
	private Tree _chunk_tree; // Displays the chunks of a P3D file in a suitable hierarchy
	[Export]
	private Viewer _viewer; // Used for viewing assets
	[Export]
	public Detailer _details; // Used for viewing the details of a Chunk
	#endregion

	// Collection of each item in the tree and its associated chunk
	private readonly Dictionary<TreeItem, Chunk> _chunks = new Dictionary<TreeItem, Pure3D.Chunk>();

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
		item.SetText(0, chunk.ToShortString());
		item.SetTooltipText(0, chunk.ToString());

		// Add the chunk to the dictionary
		_chunks.Add(item, chunk);

		// Recursively load the children of each chunk
		foreach (var child in chunk.Children) LoadChunk(child, item);

		// Collapse all child chunks of the Root chunk
		if (parent != null) if (parent.GetText(0) == "Root") item.Collapsed = true;

		return item;
	}

	/// <summary>
	/// Go through each TreeItem and toggle whether only the chunk's name is displayed
	/// </summary>
	/// <param name="toggled">Wheter to display the chunk's properties</param>
	private void ToggleShortNames(bool toggled)
	{
		TreeItem root = _chunk_tree.GetRoot();

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
