using System;
using System.Collections.Generic;
using System.Numerics;
using Godot;
using Pure3D;
using Pure3D.Chunks;

public partial class View3D : SubViewport
{
	public override void _Ready()
	{
		// Handle window resizing
		DisplayServer.WindowSetMinSize(new Vector2I(720, 720));
	}

	// Loads a Pure3D skeleton as a Skeleton3D
	public Node3D LoadSkeleton(Pure3D.Chunks.Skeleton bones)
	{
		// If this child is a Pure3D Skeleton
		// Add a parent Node
		Node3D parentNode = new Node3D();
		parentNode.Name = bones.Name;
		AddChild(parentNode);

		// Add a new Skeleton3D
		Skeleton3D skeleton = new Skeleton3D();
		skeleton.Name = bones.Name + "_skeleton";
		parentNode.AddChild(skeleton);

		// Define a placeholder mesh
		SphereMesh sphere = new SphereMesh();
		sphere.Radius = .05f;
		sphere.Height = sphere.Radius * 2;

		// Iterate through the skeleton's bones and add them to the Godot Skeleton
		foreach (var child in bones.Children)
		{
			// For every bone in the Pure3D Skeleton
			if (child is Pure3D.Chunks.SkeletonJoint joint)
			{
				// Add bone to Skeleton3D
				int boneIndex = skeleton.AddBone(joint.Name);
				if (boneIndex != joint.SkeletonParent)
					skeleton.SetBoneParent(boneIndex, (int)joint.SkeletonParent);

				// Set the bone's rest pose
				Pure3D.Matrix restPose = joint.RestPose;

				Transform3D boneTransform = new Transform3D(new Basis(
					new Godot.Vector3(restPose.M11, restPose.M12, restPose.M13),
					new Godot.Vector3(restPose.M21, restPose.M22, restPose.M23),
					new Godot.Vector3(restPose.M31, restPose.M32, restPose.M33)
				), new Godot.Vector3(restPose.M41, restPose.M42, restPose.M43));
				skeleton.SetBoneRest(boneIndex, boneTransform);

				// Add a primitive sphere to the bone to indicate its position
				BoneAttachment3D attachment = new BoneAttachment3D();
				attachment.Name = joint.Name;
				attachment.BoneName = joint.Name;
				attachment.BoneIdx = boneIndex;
				attachment.SetUseExternalSkeleton(true);
				attachment.SetExternalSkeleton("../" + skeleton.Name);
				parentNode.AddChild(attachment);

				MeshInstance3D indicator = new MeshInstance3D();
				indicator.Mesh = sphere;
				attachment.AddChild(indicator);

				// Set the skeleton to its rest pose
				skeleton.ResetBonePoses();
			}
		}

		// Return the parent node after making it invisible
		parentNode.Visible = false;
		return parentNode;
	}

	public void LoadMesh(Pure3D.Chunks.Mesh mesh)
	{
		GD.Print("Loading mesh " + mesh.Name);

		// Iterate through the Mesh's children
		foreach (Pure3D.Chunk chunk in mesh.Children)
		{
			if (chunk is Pure3D.Chunks.PrimitiveGroup prim)
			{
				// If the child is a Primitive Group
				// Create a Surface Tool for generating a mesh
				// and an ArrayMesh to store the mesh
				var st = new SurfaceTool();
				ArrayMesh newMesh;

				// Set the Tool's Primitive Type
				switch (prim.PrimitiveType)
				{
					case PrimitiveGroup.PrimitiveTypes.TriangleList:
						st.Begin(Godot.Mesh.PrimitiveType.Triangles);
						break;

					case PrimitiveGroup.PrimitiveTypes.TriangleStrip:
						st.Begin(Godot.Mesh.PrimitiveType.TriangleStrip);
						break;

					case PrimitiveGroup.PrimitiveTypes.LineList:
						st.Begin(Godot.Mesh.PrimitiveType.Lines);
						break;

					case PrimitiveGroup.PrimitiveTypes.LineStrip:
						st.Begin(Godot.Mesh.PrimitiveType.LineStrip);
						break;
				}

				// Get normals
				var normals = (NormalList)prim.Children.Find(x => x is NormalList);
				var colours = (ColourList)prim.Children.Find(x => x is ColourList);
				var uvs = (UVList)prim.Children.Find(x => x is UVList);
				var verts = (PositionList)prim.Children.Find(x => x is PositionList);

				for (uint i = 0; i < prim.NumVertices; i++)
				{
					// Set the normal of the next vertex
					if (normals != null)
					{
						Pure3D.Vector3 normal = normals.Normals[i];
						st.SetNormal(new Godot.Vector3(normal.X, normal.Y, normal.Z));
					}

					// Set the colour of the next vertex
					if (colours != null)
					{
						uint colour = colours.Colours[i];
						st.SetColor(new Color(colour));
					}

					// Set the UV of the next vertex
					if (uvs != null)
					{
						Pure3D.Vector2 uv = uvs.UVs[i];
						st.SetUV(new Godot.Vector2(uv.X, uv.Y));
					}

					// Add the next vertex
					Pure3D.Vector3 vert = verts.Positions[i];
					st.AddVertex(new Godot.Vector3(vert.X, vert.Y, vert.Z));
				}

				// Add tangents to the Mesh
				if (normals != null && uvs != null) st.GenerateTangents();

				// Add indices
				var indices = (IndexList)prim.Children.Find(x => x is IndexList);

				if (indices != null)
					foreach (uint index in indices.Indices)
					{
						st.SetSmoothGroup(index);
						st.AddIndex((int)index);
					}

				// Finish generating the new Mesh and add it to the scene
				newMesh = st.Commit();
				MeshInstance3D newNode = new();
				newNode.Name = prim.ShaderName;
				newNode.Mesh = newMesh;
				AddChild(newNode);
			}
		}
	}
}