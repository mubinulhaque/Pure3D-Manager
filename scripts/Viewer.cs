using Godot;
using System;

public partial class Viewer : Control
{
	[Export]
	private Label errorMessage;
	[Export]
	private Tree tree;
	[Export]
	private View3D view;

	private void OnFilenameSubmitted(String newText) {
		// Load the model at the specified path
		String filename = newText.StripEdges();

		if (FileAccess.FileExists(filename)) {
			var file = new Pure3D.File();
			file.Load(filename);
			errorMessage.Text = "";
			LoadChunk(file.RootChunk, null);
		} else if (errorMessage != null) {
			errorMessage.Text = "No file matches " + filename;
		}
	}

	private void LoadChunk(Pure3D.Chunk chunk, TreeItem parent)
	{
		TreeItem item = tree.CreateItem(parent);
		item.SetText(0, chunk.ToShortString());

		foreach (var child in chunk.Children)
		{
			if (child is Pure3D.Chunks.Skeleton && view != null)
			{
				// If this child is a Pure3D Skeleton
				// Add a new Skeleton3D
				Skeleton3D newNode = new Skeleton3D();
				AddChild(newNode);

				// Set the name of the Skeleton3D
				var skeleton = (Pure3D.Chunks.Skeleton)child;
				newNode.Name = skeleton.Name;

				// Load the Skeleton's Joints
				view.LoadSkeleton(skeleton.Children, newNode);
			}

			LoadChunk(child, item);
		}
	}

	private void ToggleShortNames(bool toggled) {
		GD.Print("Not implemented yet");
	}
}
