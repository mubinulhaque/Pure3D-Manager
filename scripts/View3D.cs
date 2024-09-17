using Godot;
using System;
using System.Collections.Generic;

public partial class View3D : SubViewport
{
	[Export]
	public String ModelPath { get; set; } = "";
	[Export]
	public Label label;
	
	public override void _Ready()
	{
		// Handle window resizing
		DisplayServer.WindowSetMinSize(new Vector2I(720, 720));
		GetTree().Root.GetViewport().SizeChanged += _OnSizeChanged;
		_OnSizeChanged();
	}

	public void LoadSkeleton(List<Pure3D.Chunk> children, Skeleton3D skeleton)
	{
		int boneIndex = 0;

		foreach (var child in children)
		{
			if (child is Pure3D.Chunks.SkeletonJoint)
			{
				var joint = (Pure3D.Chunks.SkeletonJoint)child;
				skeleton.AddBone(joint.Name);
				skeleton.EditorDescription += "Bone: " + joint.Name + "\n";

				var restPose = joint.RestPose;
				var boneTransform = new Transform3D(new Projection(
					new Vector4(joint.RestPose.M21, joint.RestPose.M22, joint.RestPose.M23, joint.RestPose.M24),
					new Vector4(joint.RestPose.M31, joint.RestPose.M32, joint.RestPose.M33, joint.RestPose.M34),
					new Vector4(joint.RestPose.M41, joint.RestPose.M42, joint.RestPose.M43, joint.RestPose.M44),
					new Vector4(joint.RestPose.M11, joint.RestPose.M12, joint.RestPose.M13, joint.RestPose.M14)
				));
				skeleton.SetBoneRest(boneIndex, boneTransform);

				MeshInstance3D boneIndicator = new MeshInstance3D();
				boneIndicator.Name = joint.Name;
				SphereMesh sphere = new SphereMesh();
				sphere.Radius = 0.1f;
				sphere.Height = sphere.Radius * 2;
				boneIndicator.Mesh = sphere;
				boneIndicator.Position = boneTransform.Origin;
				//skeleton.AddChild(boneIndicator);
			} else
			{
				GD.Print(child);
			}

			boneIndex++;
		}
	}

	private void _OnSizeChanged()
	{
		Size = GetTree().Root.Size / 2;
	}
}