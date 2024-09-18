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
		// If this child is a Pure3D Skeleton
		// Add a new Skeleton3D
		Skeleton3D skeleton = new Skeleton3D();
		skeleton.Name = bones.Name;
		AddChild(skeleton);

		// Define a placeholder mesh
		SphereMesh sphere = new SphereMesh();
		sphere.Radius = .05f;
		sphere.Height = sphere.Radius * 2;

		// Iterate through the skeleton's bones and add them to the Godot Skeleton
		foreach (var child in bones.Children)
		{
			if (child is Pure3D.Chunks.SkeletonJoint)
			{
				Pure3D.Chunks.SkeletonJoint joint = (Pure3D.Chunks.SkeletonJoint)child;
				int boneIndex = skeleton.AddBone(joint.Name);
				GD.Print(boneIndex);
				if (boneIndex != joint.SkeletonParent) skeleton.SetBoneParent(boneIndex, ((int)joint.SkeletonParent));

				Pure3D.Matrix restPose = joint.RestPose;
				 
				Transform3D boneTransform = new Transform3D(new Basis(
					new Vector3(restPose.M11, restPose.M12, restPose.M13),
					new Vector3(restPose.M21, restPose.M22, restPose.M23),
					new Vector3(restPose.M31, restPose.M32, restPose.M33)
				), new Vector3(restPose.M41, restPose.M42, restPose.M43));
				skeleton.SetBoneRest(boneIndex, boneTransform);

				BoneAttachment3D attachment = new BoneAttachment3D();
				attachment.Name = joint.Name;
				attachment.BoneName = joint.Name;
				attachment.BoneIdx = boneIndex;
				skeleton.AddChild(attachment);
				
				MeshInstance3D indicator = new MeshInstance3D();
				indicator.Mesh = sphere;
				attachment.AddChild(indicator);

				skeleton.ResetBonePoses();
			} else
			{
				GD.Print(child);
			}
		}
	}

	private void _OnSizeChanged()
	{
		Size = GetTree().Root.Size / 2;
	}
}