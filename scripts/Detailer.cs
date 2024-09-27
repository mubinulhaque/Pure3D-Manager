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
			// 3D chunks
			case Skeleton skel:
				root.SetText(
					0,
					$"{skel.Name} (Skeleton)"
				);

				TreeItem skelVers = CreateItem(root);
				skelVers.SetText(
					0,
					$"Version: {skel.Version}"
				);

				TreeItem skelJoints = CreateItem(root);
				skelJoints.SetText(
					0,
					$"{skel.GetNumJoints()} Joints"
				);
				break;

			case Pure3D.Chunks.Skin skin:
				root.SetText(
					0,
					$"{skin.Name} (Skin)"
				);

				TreeItem skinSkel = CreateItem(root);
				skinSkel.SetText(
					0,
					$"Associated Skeleton: {skin.SkeletonName}"
				);

				TreeItem skinVers = CreateItem(root);
				skinVers.SetText(
					0,
					$"Version: {skin.Version}"
				);

				TreeItem skinPrims = CreateItem(root);
				skinPrims.SetText(
					0,
					$"{skin.NumPrimGroups} Primitive Groups"
				);
				break;

			case Pure3D.Chunks.Mesh mesh:
				root.SetText(
					0,
					$"{mesh.Name} (Mesh)"
				);

				TreeItem meshVers = CreateItem(root);
				meshVers.SetText(
					0,
					$"Version: {mesh.Version}"
				);

				TreeItem meshPrims = CreateItem(root);
				meshPrims.SetText(
					0,
					$"{mesh.NumPrimGroups} Primitive Groups"
				);
				break;

			// 2D chunks
			case Pure3D.Chunks.Image img:
				root.SetText(
					0,
					$"{img.Name} (Image)"
				);

				TreeItem imgVers = CreateItem(root);
				imgVers.SetText(
					0,
					$"Version: {img.Version}"
				);

				TreeItem imgRes = CreateItem(root);
				imgRes.SetText(
					0,
					$"Resolution: {img.Width} x {img.Height}"
				);

				TreeItem imgBpp = CreateItem(root);
				imgBpp.SetText(
					0,
					$"Bits Per Pixel: {img.Bpp}"
				);

				TreeItem imgPlt = CreateItem(root);
				imgPlt.SetText(
					0,
					$"Palletised: {img.Palettized}"
				);

				TreeItem imgAlpha = CreateItem(root);
				imgAlpha.SetText(
					0,
					$"Has Alpha Channel: {img.HasAlpha}"
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
					$"{imgData.Data.Length} Bytes Long"
				);
				break;

			// Miscellaneous Chunks
			case Root rootChunk:
				root.SetText(
					0,
					"Root Chunk"
				);
				break;

			case AnimationGroupList list:
				root.SetText(
					0,
					"Animation Group List"
				);

				TreeItem aglVers = CreateItem(root);
				aglVers.SetText(
					0,
					$"Version: {list.Version}"
				);

				TreeItem aglGroups = CreateItem(root);
				aglGroups.SetText(
					0,
					$"{list.Version} Groups"
				);
				break;

			case ColourList list:
				root.SetText(
					0,
					"Colour List"
				);

				for (uint i = 0; i < list.Colours.Length; i++)
				{
					Color colour = new(list.Colours[i]);

					TreeItem colourItem = CreateItem(root);
					colourItem.SetText(
						0,
						$"Colour {i + 1}: ({colour.R}, {colour.G}, {colour.B}, {colour.A})"
					);
					colourItem.SetCustomBgColor(0, colour);
				}
				break;

			case CompositeDrawableEffectList list:
				root.SetText(
					0,
					"Composite Drawable Effect List"
				);

				TreeItem cdelCount = CreateItem(root);
				cdelCount.SetText(
					0,
					$"{list.NumElements} Elements"
				);
				break;

			case CompositeDrawablePropList list:
				root.SetText(
					0,
					"Composite Drawable Prop List"
				);

				TreeItem cdplCount = CreateItem(root);
				cdplCount.SetText(
					0,
					$"{list.NumElements} Elements"
				);
				break;

			case CompositeDrawableSkinList list:
				root.SetText(
					0,
					"Composite Drawable Skin List"
				);

				TreeItem cdslCount = CreateItem(root);
				cdslCount.SetText(
					0,
					$"{list.NumElements} Elements"
				);
				break;

			case IndexList list:
				root.SetText(
					0,
					"Index List"
				);

				for (uint i = 0; i < list.Indices.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Index {i + 1}: {list.Indices[i]}"
					);
				}
				break;

			case MatrixList list:
				root.SetText(
					0,
					"Matrix List"
				);

				for (uint i = 0; i < list.Matrices.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Matrix {i + 1}: ({list.Matrices[i][0]}, {list.Matrices[i][1]}, {list.Matrices[i][2]}, {list.Matrices[i][3]})"
					);
				}
				break;

			case MatrixPalette list:
				root.SetText(
					0,
					"Matrix Palette"
				);

				for (uint i = 0; i < list.Matrices.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Matrix {i + 1}: {list.Matrices[i]}"
					);
				}
				break;

			case NormalList list:
				root.SetText(
					0,
					"Normal List"
				);

				for (uint i = 0; i < list.Normals.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Normal {i + 1}: ({list.Normals[i].X}, {list.Normals[i].Y}, {list.Normals[i].Z})"
					);
				}
				break;

			case PackedNormalList list:
				root.SetText(
					0,
					"Packed Normal List"
				);

				for (uint i = 0; i < list.Normals.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Normal {i + 1}: {list.Normals[i]}"
					);
				}
				break;

			case PositionList list:
				root.SetText(
					0,
					"Position List"
				);

				for (uint i = 0; i < list.Positions.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Position {i + 1}: ({list.Positions[i].X}, {list.Positions[i].Y}, {list.Positions[i].Z})"
					);
				}
				break;

			case UVList list:
				root.SetText(
					0,
					"UV List"
				);

				for (uint i = 0; i < list.UVs.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"UV {i + 1}: ({list.UVs[i].X}, {list.UVs[i].Y})"
					);
				}
				break;

			case WeightList list:
				root.SetText(
					0,
					"Position List"
				);

				for (uint i = 0; i < list.Weights.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Weight {i + 1}: ({list.Weights[i].X}, {list.Weights[i].Y}, {list.Weights[i].Z})"
					);
				}
				break;

			// Unknown chunks
			case Unknown:
				root.SetText(
					0,
					$"Unknown Chunk"
				);

				TreeItem unknownType = CreateItem(root);
				unknownType.SetText(
					0,
					$"Decimal Type: {chunk.Type}"
				);

				TreeItem unknownHexType = CreateItem(root);
				unknownHexType.SetText(
					0,
					$"Hexadecimal Type: 0x{chunk.Type:X}"
				);
				break;

			case Named named:
				// If the chunk is not viewable but is named
				// Notify the user
				root.SetText(
					0,
					$"{named.Name} (Named Chunk)"
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
					$"{chunk.ToShortString()} (Chunk)"
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
