using Godot;
using Pure3D;
using Pure3D.Chunks;

public partial class Detailer : Tree
{
	public void ViewChunk(Chunk chunk)
	{
		// Empty the Tree
		if (GetRoot() != null) GetRoot().Free();

		// Add the chunk's properties to the Detail Tree
		TreeItem root = CreateItem();
		root.DisableFolding = true;

		// Load the chunk's details based on its type
		switch (chunk)
		{
			case Root rootChunk:
				root.SetText(
					0,
					"Root Chunk"
				);
				break;

			case Skeleton skel:
				root.SetText(
					0,
					skel.Name + " (Skeleton)"
				);

				TreeItem skelVers = CreateItem(root);
				skelVers.SetText(
					0,
					"Version: " + skel.Version
				);

				TreeItem skelJoints = CreateItem(root);
				skelJoints.SetText(
					0,
					skel.GetNumJoints() + " Joints"
				);
				break;

			case Pure3D.Chunks.Skin skin:
				root.SetText(
					0,
					skin.Name + " (Skin)"
				);

				TreeItem skinSkel = CreateItem(root);
				skinSkel.SetText(
					0,
					"Associated Skeleton: " + skin.SkeletonName
				);

				TreeItem skinVers = CreateItem(root);
				skinVers.SetText(
					0,
					"Version: " + skin.Version
				);

				TreeItem skinPrims = CreateItem(root);
				skinPrims.SetText(
					0,
					skin.NumPrimGroups + " Primitive Groups"
				);
				break;

			case Pure3D.Chunks.Mesh mesh:
				root.SetText(
					0,
					mesh.Name + " (Mesh)"
				);

				TreeItem meshVers = CreateItem(root);
				meshVers.SetText(
					0,
					"Version: " + mesh.Version
				);

				TreeItem meshPrims = CreateItem(root);
				meshPrims.SetText(
					0,
					mesh.NumPrimGroups + " Primitive Groups"
				);
				break;

			case Pure3D.Chunks.Image img:
				root.SetText(
					0,
					img.Name + " (Image)"
				);

				TreeItem imgVers = CreateItem(root);
				imgVers.SetText(
					0,
					"Version: " + img.Version
				);

				TreeItem imgRes = CreateItem(root);
				imgRes.SetText(
					0,
					"Resolution: " + img.Width + " x " + img.Height
				);

				TreeItem imgBpp = CreateItem(root);
				imgBpp.SetText(
					0,
					"Bits Per Pixel: " + img.Bpp
				);

				TreeItem imgPlt = CreateItem(root);
				imgPlt.SetText(
					0,
					"Palletised: " + img.Palettized
				);

				TreeItem imgAlpha = CreateItem(root);
				imgAlpha.SetText(
					0,
					"Has Alpha Channel: " + img.HasAlpha
				);
				break;

			case ImageData imgData:
				root.SetText(
					0,
					"Image Data"
				);

				TreeItem imgDataLen = CreateItem(root);
				imgDataLen.SetText(
					0,
					imgData.Data.Length + " Bytes Long"
				);
				break;

			case Unknown:
				root.SetText(
					0,
					"Chunk " + chunk.Type
				);

				TreeItem unknownWarning = CreateItem(root);
				unknownWarning.SetText(
					0,
					"Chunk not yet known..."
				);
				break;

			case Named named:
				// If the chunk is not viewable but is named
				// Notify the user
				root.SetText(
					0,
					named.Name + " (Named Chunk)"
				);

				TreeItem namedWarning = CreateItem(root);
				namedWarning.SetText(
					0,
					"Properties not yet viewable..."
				);
				break;

			default:
				// If the chunk is not viewable
				// Notify the user
				root.SetText(
					0,
					chunk.ToShortString() + " (Chunk)"
				);

				TreeItem unnamedWarning = CreateItem(root);
				unnamedWarning.SetText(
					0,
					"Properties not yet viewable..."
				);
				break;
		}
	}
}
