using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class View3D : SubViewport
{
	public override void _Ready()
	{
		// Handle window resizing
		DisplayServer.WindowSetMinSize(new Vector2I(720, 720));
		GetTree().Root.GetViewport().SizeChanged += _OnSizeChanged;
		_OnSizeChanged();
	}

	public void LoadSkeleton(Pure3D.Chunks.Skeleton bones)
	{
		int boneIndex = 0;

		// If this child is a Pure3D Skeleton
		// Add a new Skeleton3D
		Skeleton3D skeleton = new Skeleton3D();
		skeleton.Name = bones.Name;
		AddChild(skeleton);

		// Define a placeholder mesh
		SphereMesh sphere = new SphereMesh();
		sphere.Radius = .2f;
		sphere.Height = sphere.Radius * 2;

		foreach (var child in bones.Children)
		{
			if (child is Pure3D.Chunks.SkeletonJoint)
			{
				Pure3D.Chunks.SkeletonJoint joint = (Pure3D.Chunks.SkeletonJoint)child;
				skeleton.AddBone(joint.Name);
				skeleton.EditorDescription += "Bone: " + joint.Name + "\n";

				Pure3D.Matrix restPose = joint.RestPose;
				 
				Transform3D boneTransform = new Transform3D(new Basis(
					new Vector3(restPose.M11, restPose.M12, restPose.M13),
					new Vector3(restPose.M21, restPose.M22, restPose.M23),
					new Vector3(restPose.M31, restPose.M32, restPose.M33)
				), new Vector3(restPose.M41, restPose.M42, restPose.M43));
				skeleton.SetBoneRest(boneIndex, boneTransform);

				//Node3D boneIndicator = new Node3D();
				MeshInstance3D boneIndicator = new MeshInstance3D();
				boneIndicator.Name = joint.Name;
				boneIndicator.Mesh = sphere;
				boneIndicator.Transform = boneTransform;
				AddChild(boneIndicator);
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