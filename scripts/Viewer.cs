using Godot;
using System;

public partial class Viewer : Control
{
	[Export]
	public LineEdit filenameEdit;

	public override void _Ready()
	{
		filenameEdit.TextSubmitted += OnFilenameSubmitted;
	}

	private void OnFilenameSubmitted(String newText) {
		GD.Print("Text submitted: " + newText);
	}
}
