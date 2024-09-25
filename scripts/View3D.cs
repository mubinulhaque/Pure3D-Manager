using Godot;
using Pure3D;

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
				if (boneIndex != joint.SkeletonParent) skeleton.SetBoneParent(boneIndex, (int)joint.SkeletonParent);

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
				// Create a Surface Tool for generating the mesh
				var st = new SurfaceTool();

				// Set the Tool's Primitive Type
				switch (prim.PrimitiveType)
				{
					case Pure3D.Chunks.PrimitiveGroup.PrimitiveTypes.TriangleList:
						st.Begin(Mesh.PrimitiveType.Triangles);
						break;

					case Pure3D.Chunks.PrimitiveGroup.PrimitiveTypes.TriangleStrip:
						st.Begin(Mesh.PrimitiveType.TriangleStrip);
						break;

					case Pure3D.Chunks.PrimitiveGroup.PrimitiveTypes.LineList:
						st.Begin(Mesh.PrimitiveType.Lines);
						break;

					case Pure3D.Chunks.PrimitiveGroup.PrimitiveTypes.LineStrip:
						st.Begin(Mesh.PrimitiveType.LineStrip);
						break;
				}


			}
		}
	}
}