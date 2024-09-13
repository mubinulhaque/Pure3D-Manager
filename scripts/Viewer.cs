using Godot;
using System;

public partial class Viewer : Control
{
	[Export]
	private LineEdit filenameEdit;
	[Export]
	private TextEdit textBox;
	[Export]
	private View3D view;

	public override void _Ready()
	{
		filenameEdit.TextSubmitted += OnFilenameSubmitted;
	}

	private void OnFilenameSubmitted(String newText) {
		// Load the model at the specified path
		String filename = newText.StripEdges();

		if (FileAccess.FileExists(filename)) {
			var file = new Pure3D.File();
			file.Load(filename);
			textBox.Text = "";
			LoadChunk(file.RootChunk, "", 0);
		} else if (textBox != null) {
			textBox.AddThemeColorOverride("font_readonly_color", new Color(255, 0, 0));
			textBox.Text = "No file matches " + filename;
		}
	}

	public void LoadChunk(Pure3D.Chunk chunk, String text, int indent)
	{
		textBox.AddThemeColorOverride("font_readonly_color", new Color(255, 255, 255));
		GD.Print(new String('\t', indent) + chunk.ToString());

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

			LoadChunk(child, text, indent + 1);
		}
	}
}
