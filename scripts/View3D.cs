using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class View3D : SubViewport
{
	[Signal]
	public delegate void EnableExportingEventHandler(Node3D rootNode);

	public override void _Ready()
	{
		// Handle window resizing
		DisplayServer.WindowSetMinSize(new Vector2I(720, 720));
	}

	// Loads a Pure3D skeleton as a Skeleton3D
	public void LoadSkeleton(Pure3D.Chunks.Skeleton bones)
	{
		// If this child is a Pure3D Skeleton
		// Add a new Skeleton3D
		Skeleton3D skeleton = new Skeleton3D();
		skeleton.Name = bones.Name;
		AddChild(skeleton);
		EmitSignal(SignalName.EnableExporting, skeleton);

		// Define a placeholder mesh
		SphereMesh sphere = new SphereMesh();
		sphere.Radius = .05f;
		sphere.Height = sphere.Radius * 2;

		// Iterate through the skeleton's bones and add them to the Godot Skeleton
		foreach (var child in bones.Children)
		{
			// For every bone in the Pure3D Skeleton
			if (child is Pure3D.Chunks.SkeletonJoint)
			{
				// Add bone to Skeleton3D
				Pure3D.Chunks.SkeletonJoint joint = (Pure3D.Chunks.SkeletonJoint)child;
				int boneIndex = skeleton.AddBone(joint.Name);
				if (boneIndex != joint.SkeletonParent) skeleton.SetBoneParent(boneIndex, ((int)joint.SkeletonParent));

				// Set the bone's rest pose
				Pure3D.Matrix restPose = joint.RestPose;
				 
				Transform3D boneTransform = new Transform3D(new Basis(
					new Vector3(restPose.M11, restPose.M12, restPose.M13),
					new Vector3(restPose.M21, restPose.M22, restPose.M23),
					new Vector3(restPose.M31, restPose.M32, restPose.M33)
				), new Vector3(restPose.M41, restPose.M42, restPose.M43));
				skeleton.SetBoneRest(boneIndex, boneTransform);

				// Add a primitive sphere to the bone to indicate its position
				BoneAttachment3D attachment = new BoneAttachment3D();
				attachment.Name = joint.Name;
				attachment.BoneName = joint.Name;
				attachment.BoneIdx = boneIndex;
				attachment.SetUseExternalSkeleton(true);
				attachment.SetExternalSkeleton("../" + skeleton.Name);
				AddChild(attachment);
				
				MeshInstance3D indicator = new MeshInstance3D();
				indicator.Mesh = sphere;
				attachment.AddChild(indicator);

				// Set the skeleton to its rest pose
				skeleton.ResetBonePoses();
			}
		}
	}
}