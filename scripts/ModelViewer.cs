using Godot;
using Pure3D.Chunks;
using System;
using System.Collections.Generic;

public partial class ModelViewer : Node3D
{
	[Export]
	public String ModelPath { get; set; } = "";
	[Export]
	public Label label;
	
	public override void _Ready()
	{
		var file = new Pure3D.File();
		file.Load(ModelPath);
		_LoadChunk(file.RootChunk, 0);
	}
	
	public void _LoadChunk(Pure3D.Chunk chunk, int indent)
	{
		GD.Print(new String('\t', indent), chunk.ToString());

		foreach (var child in chunk.Children)
		{
			if (child is Pure3D.Chunks.Skeleton)
			{
				// If this child is a Pure3D Skeleton
				// Add a new Skeleton3D
				Skeleton3D newNode = new Skeleton3D();
				AddChild(newNode);

				// Set the name of the Skeleton3D
				var skeleton = (Pure3D.Chunks.Skeleton)child;
				newNode.Name = skeleton.Name;

				// Load the Skeleton's Joints
				_LoadSkeleton(skeleton.Children, newNode);
			} else {
				_LoadChunk(child, indent + 1);
			}
		}
	}

	public void _LoadSkeleton(List<Pure3D.Chunk> children, Skeleton3D skeleton)
	{
		int boneIndex = 0;

		foreach (var child in children)
		{
			if (child is SkeletonJoint) {
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
				skeleton.AddChild(boneIndicator);
			} else {
				GD.Print(child);
			}

			boneIndex++;
		}
	}
}
