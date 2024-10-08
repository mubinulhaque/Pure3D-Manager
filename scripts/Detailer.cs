using Godot;
using Pure3D;
using Pure3D.Chunks;

public partial class Detailer : Tree
{
	/// <summary>
	/// Displays the properties of a Pure3D chunk
	/// </summary>
	/// <param name="chunk">Pure3D chunk</param>
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
			#region 2D Chunks
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
			#endregion

			#region 3D Chunks
			case BoundingBox bBox:
				root.SetText(
					0,
					$"Bounding Box"
				);

				TreeItem bbLow = CreateItem(root);
				bbLow.SetText(
					0,
					$"Lower Corner: {bBox.Low}"
				);

				TreeItem bbHigh = CreateItem(root);
				bbHigh.SetText(
					0,
					$"Upper Corner: {bBox.High}"
				);
				break;

			case BoundingSphere bSphere:
				root.SetText(
					0,
					$"Bounding Sphere"
				);

				TreeItem bSphereCentre = CreateItem(root);
				bSphereCentre.SetText(
					0,
					$"Centre: {bSphere.Centre}"
				);

				TreeItem bSphereRadius = CreateItem(root);
				bSphereRadius.SetText(
					0,
					$"Radius: {bSphere.Radius}"
				);
				break;

			case BreakableObject breakable:
				root.SetText(
					0,
					$"Breakable Object"
				);

				TreeItem breakableIndex = CreateItem(root);
				breakableIndex.SetText(
					0,
					$"Index: {breakable.Index}"
				);

				TreeItem breakableCount = CreateItem(root);
				breakableCount.SetText(
					0,
					$"Count: {breakable.Count}"
				);
				break;

			case ColourList list:
				root.SetText(
					0,
					"Colour List"
				);

				for (uint i = 0; i < list.Colours.Length; i++)
				{
					Color colour = Util.GetColour(list.Colours[i]);

					TreeItem colourItem = CreateItem(root);
					TreeItem colourValue = CreateItem(root);
					colourItem.SetText(
						0,
						$"Colour {i + 1}: ({colour.R}, {colour.G}, {colour.B}, {colour.A})"
					);
					colourValue.SetCustomBgColor(0, colour);
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
					$"{list.NumElements} Effects"
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
					$"{list.NumElements} Props"
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
					$"{list.NumElements} Skins"
				);
				break;

			case Entity entity:
				root.SetText(
					0,
					$"{entity.Name} ({entity.GetChunkName()})"
				);

				TreeItem entityVers = CreateItem(root);
				entityVers.SetText(
					0,
					$"Version: {entity.Version}"
				);

				TreeItem entityOrder = CreateItem(root);
				entityOrder.SetText(
					0,
					$"Render Order: {entity.RenderOrder}"
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

			case Intersect intersect:
				root.SetText(
					0,
					"Intersect"
				);

				for (uint i = 0; i < intersect.Indices.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Index {i + 1}: {intersect.Indices[i]}"
					);
				}

				for (uint i = 0; i < intersect.Positions.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Position {i + 1}: {intersect.Positions[i]}"
					);
				}

				for (uint i = 0; i < intersect.Normals.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Normal {i + 1}: {intersect.Normals[i]}"
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

			// Normally, everything is in alphabetical order
			// But Skin inherits from Mesh,
			// So it has to come first
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

			case PrimitiveGroup primGroup:
				root.SetText(
					0,
					"Primitive Group"
				);

				TreeItem primVerts = CreateItem(root);
				primVerts.SetText(
					0,
					$"{primGroup.NumVertices} Vertices"
				);

				TreeItem primIndices = CreateItem(root);
				primIndices.SetText(
					0,
					$"{primGroup.NumIndices} Indices"
				);

				TreeItem primMats = CreateItem(root);
				primMats.SetText(
					0,
					$"{primGroup.NumMatrices} Palette Matrices"
				);

				TreeItem primContains = CreateItem(root);
				primContains.SetText(
					0,
					"Contains:"
				);

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.UVs))
				{
					TreeItem primFlag = CreateItem(primContains);
					primFlag.SetText(
						0,
						"UVs"
					);
				}

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.UVs2))
				{
					TreeItem primFlag = CreateItem(primContains);
					primFlag.SetText(
						0,
						"UV2s"
					);
				}

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.UVs3))
				{
					TreeItem primFlag = CreateItem(primContains);
					primFlag.SetText(
						0,
						"UV3s"
					);
				}

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.UVs4))
				{
					TreeItem primFlag = CreateItem(primContains);
					primFlag.SetText(
						0,
						"UV4s"
					);
				}

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.Normals))
				{
					TreeItem primFlag = CreateItem(primContains);
					primFlag.SetText(
						0,
						"Normals"
					);
				}

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.Colours))
				{
					TreeItem primFlag = CreateItem(primContains);
					primFlag.SetText(
						0,
						"Colours"
					);
				}

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.Matrices))
				{
					TreeItem primFlag = CreateItem(primContains);
					primFlag.SetText(
						0,
						"Matrices"
					);
				}

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.Weights))
				{
					TreeItem primFlag = CreateItem(primContains);
					primFlag.SetText(
						0,
						"Weights"
					);
				}

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.Positions))
				{
					TreeItem primFlag = CreateItem(primContains);
					primFlag.SetText(
						0,
						"Positions"
					);
				}
				break;

			case Skeleton skel:
				break;

			case StaticPhysicsObject spo:
				root.SetText(
					0,
					$"{spo.Name} (Static Physics Object)"
				);

				TreeItem spoData = CreateItem(root);
				spoData.SetText(
					0,
					$"Data:"
				);

				for (uint i = 0; i < spo.Data.Length; i++)
				{
					TreeItem member = CreateItem(spoData);
					member.SetText(
						0,
						$"Byte {i + 1}: ({spo.Data[i]})"
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
					"Weight List"
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
			#endregion

			#region Animation Chunks
			case Pure3D.Chunks.Animation anim:
				root.SetText(
					0,
					"Animation"
				);

				TreeItem animName = CreateItem(root);
				animName.SetText(
					0,
					$"Name: {anim.Name}"
				);

				TreeItem animVers = CreateItem(root);
				animVers.SetText(
					0,
					$"Version: {anim.Version}"
				);

				TreeItem animFrames = CreateItem(root);
				animFrames.SetText(
					0,
					$"{anim.NumberOfFrames} Frames"
				);

				TreeItem animFrameRate = CreateItem(root);
				animFrameRate.SetText(
					0,
					$"Frame Rate: {anim.FrameRate}"
				);

				TreeItem animLooping = CreateItem(root);
				animLooping.SetText(
					0,
					$"Looping: {anim.Looping}"
				);
				break;

			case AnimationGroup animGroup:
				root.SetText(
					0,
					"Animation Group"
				);

				TreeItem animGroupName = CreateItem(root);
				animGroupName.SetText(
					0,
					$"Name: {animGroup.Name}"
				);

				TreeItem animGroupVers = CreateItem(root);
				animGroupVers.SetText(
					0,
					$"Version: {animGroup.Version}"
				);

				TreeItem animGroupId = CreateItem(root);
				animGroupId.SetText(
					0,
					$"Group: {animGroup.GroupId}"
				);

				TreeItem animGroupChannels = CreateItem(root);
				animGroupChannels.SetText(
					0,
					$"{animGroup.NumberOfChannels} Channels"
				);
				break;

			case AnimationGroupList agl:
				root.SetText(
					0,
					"Animation Group List"
				);

				TreeItem aglVers = CreateItem(root);
				aglVers.SetText(
					0,
					$"Version: {agl.Version}"
				);

				TreeItem aglGroups = CreateItem(root);
				aglGroups.SetText(
					0,
					$"{agl.NumberOfGroups} Groups"
				);
				break;

			case AnimationSize animSize:
				root.SetText(
					0,
					"Animation Size"
				);

				TreeItem aglSizeVers = CreateItem(root);
				aglSizeVers.SetText(
					0,
					$"Version: {animSize.Version}"
				);

				TreeItem animSizeGC = CreateItem(root);
				animSizeGC.SetText(
					0,
					$"GameCube: {animSize.GameCube}"
				);

				TreeItem animSizePC = CreateItem(root);
				animSizePC.SetText(
					0,
					$"PC: {animSize.PC}"
				);

				TreeItem animSizePS2 = CreateItem(root);
				animSizePS2.SetText(
					0,
					$"PS2: {animSize.PS2}"
				);

				TreeItem animSizeXbox = CreateItem(root);
				animSizeXbox.SetText(
					0,
					$"Xbox: {animSize.Xbox}"
				);
				break;

			case ChannelInterpolationMode mode:
				root.SetText(
					0,
					"Channel Interpolation Mode"
				);

				TreeItem channelVers = CreateItem(root);
				channelVers.SetText(
					0,
					$"Version: {mode.Version}"
				);

				TreeItem channelMode = CreateItem(root);
				channelMode.SetText(
					0,
					$"Mode: {mode.Mode}"
				);
				break;

			case CompressedQuaternionChannel cqc:
				root.SetText(
					0,
					"Compressed Quaternion Channel"
				);

				TreeItem cqcVers = CreateItem(root);
				cqcVers.SetText(
					0,
					$"Version: {cqc.Version}"
				);

				TreeItem cqcParam = CreateItem(root);
				cqcParam.SetText(
					0,
					$"Parameter: {cqc.Parameter}"
				);

				TreeItem cqcFrames = CreateItem(root);
				cqcFrames.SetText(
					0,
					$"{cqc.NumberOfFrames} Frames"
				);

				for (uint i = 0; i < cqc.Frames.Length; i++)
				{
					TreeItem member = CreateItem(root);
					Pure3D.Quaternion value = cqc.Values[i];
					member.SetText(
						0,
						$"Frame {cqc.Frames[i] + 1}: ({value.X}, {value.Y}, {value.Z}, {value.W})"
					);
				}
				break;

			case EntityChannel ec:
				root.SetText(
					0,
					"Entity Channel"
				);

				TreeItem ecVers = CreateItem(root);
				ecVers.SetText(
					0,
					$"Version: {ec.Version}"
				);

				TreeItem ecParam = CreateItem(root);
				ecParam.SetText(
					0,
					$"Parameter: {ec.Param}"
				);

				TreeItem ecFrames = CreateItem(root);
				ecFrames.SetText(
					0,
					$"{ec.NumberOfFrames} Frames"
				);

				for (uint i = 0; i < ec.Frames.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Frame {ec.Frames[i] + 1}: {ec.Values[i]}"
					);
				}
				break;

			case ParticleAnimation pAnim:
				root.SetText(
					0,
					"Entity Channel"
				);

				TreeItem pAnimType = CreateItem(root);
				pAnimType.SetText(
					0,
					$"Type: {pAnim.Type}"
				);

				for (uint i = 0; i < pAnim.Data.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Byte {i + 1}: {pAnim.Data[i]}"
					);
				}
				break;

			case QuaternionChannel qc:
				root.SetText(
					0,
					"Quaternion Channel"
				);

				TreeItem qcVers = CreateItem(root);
				qcVers.SetText(
					0,
					$"Version: {qc.Version}"
				);

				TreeItem qcParam = CreateItem(root);
				qcParam.SetText(
					0,
					$"Parameter: {qc.Parameter}"
				);

				TreeItem qcFrames = CreateItem(root);
				qcFrames.SetText(
					0,
					$"{qc.NumberOfFrames} Frames"
				);

				for (uint i = 0; i < qc.Frames.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Frame {qc.Frames[i] + 1}: {qc.Values[i]}"
					);
				}
				break;

			case Vector1Channel v1c:
				root.SetText(
					0,
					"Vector 1 Channel"
				);

				TreeItem v1cVers = CreateItem(root);
				v1cVers.SetText(
					0,
					$"Version: {v1c.Version}"
				);

				TreeItem v1cParam = CreateItem(root);
				v1cParam.SetText(
					0,
					$"Parameter: {v1c.Parameter}"
				);

				TreeItem v1cMapping = CreateItem(root);
				v1cMapping.SetText(
					0,
					$"Mapping: {v1c.Mapping}"
				);

				TreeItem v1cFrames = CreateItem(root);
				v1cFrames.SetText(
					0,
					$"{v1c.NumberOfFrames} Frames"
				);

				TreeItem v1cConstants = CreateItem(root);
				v1cConstants.SetText(
					0,
					$"Constants: ({v1c.Constants.X}, {v1c.Constants.Y}, {v1c.Constants.Z})"
				);

				for (uint i = 0; i < v1c.Frames.Length; i++)
				{
					TreeItem member = CreateItem(root);
					member.SetText(
						0,
						$"Frame {v1c.Frames[i] + 1}: {v1c.Values[i]}"
					);
				}
				break;

			case Vector2Channel v2c:
				root.SetText(
					0,
					"Vector 2 Channel"
				);

				TreeItem v2cVers = CreateItem(root);
				v2cVers.SetText(
					0,
					$"Version: {v2c.Version}"
				);

				TreeItem v2cParam = CreateItem(root);
				v2cParam.SetText(
					0,
					$"Parameter: {v2c.Parameter}"
				);

				TreeItem v2cMapping = CreateItem(root);
				v2cMapping.SetText(
					0,
					$"Mapping: {v2c.Mapping}"
				);

				TreeItem v2cFrames = CreateItem(root);
				v2cFrames.SetText(
					0,
					$"{v2c.NumberOfFrames} Frames"
				);

				TreeItem v2cConstants = CreateItem(root);
				v2cConstants.SetText(
					0,
					$"Constants: ({v2c.Constants.X}, {v2c.Constants.Y}, {v2c.Constants.Z})"
				);

				for (uint i = 0; i < v2c.Frames.Length; i++)
				{
					TreeItem member = CreateItem(root);
					Pure3D.Vector2 value = v2c.Values[i];
					member.SetText(
						0,
						$"Frame {v2c.Frames[i] + 1}: ({value.X}, {value.Y})"
					);
				}
				break;

			case Vector3Channel v3c:
				root.SetText(
					0,
					"Vector 3 Channel"
				);

				TreeItem v3cVers = CreateItem(root);
				v3cVers.SetText(
					0,
					$"Version: {v3c.Version}"
				);

				TreeItem v3cParam = CreateItem(root);
				v3cParam.SetText(
					0,
					$"Parameter: {v3c.Parameter}"
				);

				TreeItem v3cFrames = CreateItem(root);
				v3cFrames.SetText(
					0,
					$"{v3c.NumberOfFrames} Frames"
				);

				for (uint i = 0; i < v3c.Frames.Length; i++)
				{
					TreeItem member = CreateItem(root);
					Pure3D.Vector3 value = v3c.Values[i];
					member.SetText(
						0,
						$"Frame {v3c.Frames[i] + 1}: ({value.X}, {value.Y}, {value.Z})"
					);
				}
				break;
			#endregion

			#region Shader Chunks
			case Pure3D.Chunks.Shader shader:
				root.SetText(
					0,
					"Shader"
				);

				TreeItem sName = CreateItem(root);
				sName.SetText(
					0,
					$"Name: {shader.Name}"
				);

				TreeItem pddiName = CreateItem(root);
				pddiName.SetText(
					0,
					$"Shader Name: {shader.PddiShaderName}"
				);

				TreeItem sVers = CreateItem(root);
				sVers.SetText(
					0,
					$"Version: {shader.Version}"
				);

				TreeItem sTrans = CreateItem(root);
				sTrans.SetText(
					0,
					$"Translucency: {shader.HasTranslucency}"
				);

				TreeItem sMask = CreateItem(root);
				sMask.SetText(
					0,
					$"Vertex Mask: {shader.VertexMask}"
				);

				TreeItem sNeeds = CreateItem(root);
				sNeeds.SetText(
					0,
					$"Vertex Needs: {shader.VertexNeeds}"
				);

				TreeItem sParams = CreateItem(root);
				sParams.SetText(
					0,
					$"{shader.GetNumParams()} Parameters"
				);
				break;

			case ShaderColourParam shaderColour:
				root.SetText(
					0,
					"Shader Colour Parameter"
				);

				TreeItem scParam = CreateItem(root);
				scParam.SetText(
					0,
					$"Parameter: {shaderColour.Param}"
				);

				Color scColour = Util.GetColour(shaderColour.Colour);
				TreeItem scValue = CreateItem(root);
				scValue.SetText(
					0,
					$"Colour: ({scColour.R}, {scColour.G}, {scColour.B}, {scColour.A})"
				);

				TreeItem scColourItem = CreateItem(root);
				scColourItem.SetCustomBgColor(
					0,
					scColour
				);
				break;

			case ShaderFloatParam shaderFloat:
				root.SetText(
					0,
					"Shader Float Parameter"
				);

				TreeItem sfParam = CreateItem(root);
				sfParam.SetText(
					0,
					$"Parameter: {shaderFloat.Param}"
				);

				TreeItem sfValue = CreateItem(root);
				sfValue.SetText(
					0,
					$"Value: {shaderFloat.Value}"
				);
				break;

			case ShaderIntParam shaderInt:
				root.SetText(
					0,
					"Shader Integer Parameter"
				);

				TreeItem siParam = CreateItem(root);
				siParam.SetText(
					0,
					$"Parameter: {shaderInt.Param}"
				);

				TreeItem siValue = CreateItem(root);
				siValue.SetText(
					0,
					$"Value: {shaderInt.Value}"
				);
				break;

			case ShaderTextureParam shaderTex:
				root.SetText(
					0,
					"Shader Texture Parameter"
				);

				TreeItem stParam = CreateItem(root);
				stParam.SetText(
					0,
					$"Parameter: {shaderTex.Param}"
				);

				TreeItem stValue = CreateItem(root);
				stValue.SetText(
					0,
					$"Value: {shaderTex.Value}"
				);
				break;

			case VertexShader vShader:
				root.SetText(
					0,
					"Vertex Shader"
				);

				TreeItem vsName = CreateItem(root);
				vsName.SetText(
					0,
					$"Shader Name: {vShader.VertexShaderName}"
				);
				break;
			#endregion

			#region Misc Chunks
			case Root rootChunk:
				root.SetText(
					0,
					"Root Chunk"
				);
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

			case RenderStatus rs:
				root.SetText(
					0,
					$"Render Status"
				);

				TreeItem rsCastShadow = CreateItem(root);
				rsCastShadow.SetText(
					0,
					$"Casts Shadow: {rs.CastShadow}"
				);
				break;
			#endregion

			#region Unknown Chunks
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
			#endregion

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
