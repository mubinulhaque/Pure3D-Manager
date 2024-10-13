using System;
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

		// Set the root chunk's text to the chunk's name
		root.SetText(0, chunk.ToShortString());

		// Add Name of chunk, where possible
		if (chunk is Named named)
		{
			AddItem(root, $"Name: {named.Name}");
		}

		// Add Version of chunk, where possible
		if (chunk is VersionNamed vNamed)
		{
			AddItem(root, $"Version: {vNamed.Version}");
		}

		// Load the chunk's details based on its type
		switch (chunk)
		{
			#region 2D Chunks
			case Pure3D.Chunks.Image img:
				AddItem(root, $"Resolution: {img.Width} x {img.Height}");
				AddItem(root, $"Bits Per Pixel: {img.Bpp}");
				AddItem(root, $"Palletised: {img.Palettized}");
				AddItem(root, $"Has Alpha Channel: {img.HasAlpha}");
				break;

			case ImageData imgData:
				AddItem(root, $"{imgData.Data.Length} Bytes Long");
				break;
			#endregion

			#region 3D Chunks
			case AnimatedDynamicPhysicsObject adpo:
				AddItem(root, $"Render Order: {adpo.RenderOrder}");
				break;

			case BoundingBox bBox:
				AddItem(root, $"Lower Corner: {bBox.Low}");
				AddItem(root, $"Upper Corner: {bBox.High}");
				break;

			case BoundingSphere bSphere:
				AddItem(root, $"Centre: {bSphere.Centre}");
				AddItem(root, $"Radius: {bSphere.Radius}");
				break;

			case BreakableObject breakable:
				AddItem(root, $"Index: {breakable.Index}");
				AddItem(root, $"Count: {breakable.Count}");
				break;

			case CollisionAABB colAABB:
				TreeItem newItem = AddItem(root, $"No properties for this chunk...");
				newItem.SetTooltipText(0, "No, really");
				break;

			case CollisionAttribute colAttribute:
				AddItem(root, $"Static: {colAttribute.IsStatic}");
				AddItem(root, $"Default Area: {colAttribute.DefaultArea}");
				AddItem(root, $"Can Roll: {colAttribute.CanRoll}");
				AddItem(root, $"Can Slide: {colAttribute.CanSlide}");
				AddItem(root, $"Can Spin: {colAttribute.CanSpin}");
				AddItem(root, $"Can Bounce: {colAttribute.CanBounce}");
				AddItem(root, $"Extra Attribute 1: {colAttribute.ExtraAttribute1}");
				AddItem(root, $"Extra Attribute 2: {colAttribute.ExtraAttribute2}");
				AddItem(root, $"Extra Attribute 3: {colAttribute.ExtraAttribute3}");
				break;

			case CollisionEffect colEffect:
				AddItem(root, $"Type: {colEffect.Classtype}");
				AddItem(root, $"Physics Prop ID: {colEffect.PhysicsProp}");
				AddItem(root, $"Sound: {colEffect.Sound}");
				break;

			case CollisionOBB colOBB:
				AddItem(root, $"Half Extents: {Util.PrintVector3(colOBB.HalfExtents)}");
				break;

			case CollisionObject collisionObject:
				AddItem(root, $"{collisionObject.NumberOfOwners} Owners");
				AddItem(root, $"Material: {collisionObject.Material}");
				AddItem(root, $"{collisionObject.NumberOfOwners} Sub Objects");
				break;

			case CollisionVector colVector:
				AddItem(root, $"Vector: {Util.PrintVector3(colVector.Vector)}");
				break;

			case CollisionVolume colVol:
				AddItem(root, $"{colVol.NumberOfSubVolumes} Sub Volumes");
				AddItem(root, $"Owner Index: {colVol.OwnerIndex}");
				AddItem(root, $"Object Reference Index: {colVol.ObjectReferenceIndex}");
				break;

			case CollisionVolumeOwner colVolOwner:
				AddItem(root, $"{colVolOwner.NumberOfNames} Names");
				break;

			case CollisionVolumeOwnerName:
				// No need for any code
				// since this has only has a Name property
				break;

			case ColourList list:
				root.SetText(
					0,
					"Colour List"
				);

				AddColourList(root, list.Colours);
				break;

			case CompositeDrawableEffectList list:
				root.SetText(
					0,
					"Composite Drawable Effect List"
				);

				AddItem(root, $"{list.NumElements} Effects");
				break;

			case CompositeDrawablePropList list:
				AddItem(root, "Composite Drawable Prop List");
				AddItem(root, $"{list.NumElements} Props");
				break;

			case CompositeDrawableProp cdp:
				AddItem(root, $"Translucent: {cdp.IsTranslucent}");
				AddItem(root, $"Skeleton Joint Index: {cdp.SkeletonJointID}");
				break;

			case CompositeDrawableSkinList list:
				root.SetText(
					0,
					"Composite Drawable Skin List"
				);

				AddItem(root, $"{list.NumElements} Skins");
				break;

			case Entity entity:
				AddItem(root, $"Render Order: {entity.RenderOrder}");
				break;

			case IndexList list:
				root.SetText(
					0,
					"Index List"
				);

				AddItemList(root, "Index", list.Indices);
				break;

			case InstanceList:
				// No code needed
				// Only a Name attribute
				break;

			case Intersect intersect:
				AddItemList(root, "Index", intersect.Indices);
				AddItemList(root, "Position", intersect.Positions);
				AddItemList(root, "Normal", intersect.Normals);
				break;

			case Locator locator:
				AddItem(root, $"Positon: ({locator.Position.X}, {locator.Position.Y}, {locator.Position.Z})");
				break;

			case Locator2 locator2:
				AddItem(root, $"Type: {locator2.LocatorType}");
				AddItem(root, $"Data Size: {locator2.DataSize}");
				AddItemList(AddItem(root, "Data:"), "Data", locator2.Data);
				AddItem(root, $"Position: ({locator2.Position.X}, {locator2.Position.Y}, {locator2.Position.Z})");
				AddItem(root, $"{locator2.NumberOfTriggers} Triggers");
				break;

			case LocatorMatrix locatorMatrix:
				ViewMatrix(root, "Transform:", locatorMatrix.Transform);
				break;

			case MatrixList list:
				root.SetText(
					0,
					"Matrix List"
				);
				for (uint i = 0; i < list.Matrices.Length; i++)
					AddItem(
						root,
						$"Matrix {i + 1}: ({list.Matrices[i][0]}, {list.Matrices[i][1]}, {list.Matrices[i][2]}, {list.Matrices[i][3]})"
					);
				break;

			case MatrixPalette list:
				root.SetText(
					0,
					"Matrix Palette"
				);

				AddItemList(root, "Matrix", list.Matrices);
				break;

			// Normally, everything is in alphabetical order
			// But Skin inherits from Mesh,
			// So it has to come first
			case Pure3D.Chunks.Skin skin:
				AddItem(root, $"Associated Skeleton: {skin.SkeletonName}");
				AddItem(root, $"Version: {skin.Version}");
				AddItem(root, $"{skin.NumPrimGroups} Primitive Groups");
				break;

			case Pure3D.Chunks.Mesh mesh:
				AddItem(root, $"{mesh.NumPrimGroups} Primitive Groups");
				break;

			case NormalList list:
				root.SetText(
					0,
					"Normal List"
				);

				AddItemList(root, "Normal", list.Normals);
				break;

			case PackedNormalList list:
				root.SetText(
					0,
					"Packed Normal List"
				);

				AddItemList(root, "Packed Normal", list.Normals);
				break;

			case ParticleSystem2 pSystem2:
				AddItem(root, $"Factory: {pSystem2.Factory}");
				break;

			case PositionList list:
				root.SetText(
					0,
					"Position List"
				);

				AddItemList(root, "Position", list.Positions);
				break;

			case PrimitiveGroup primGroup:
				root.SetText(
					0,
					"Primitive Group"
				);

				AddItem(root, $"{primGroup.NumVertices} Vertices");
				AddItem(root, $"{primGroup.NumIndices} Indices");
				AddItem(root, $"{primGroup.NumMatrices} Palette Matrices");
				TreeItem primContains = AddItem(root, "Contains:");

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.Colours))
					AddItem(primContains, "Colours");

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.Matrices))
					AddItem(primContains, "Matrices");

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.Normals))
					AddItem(primContains, "Normals");

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.Positions))
					AddItem(primContains, "Positions");

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.UVs))
					AddItem(primContains, "UVs");

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.UVs2))
					AddItem(primContains, "UV2s");

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.UVs3))
					AddItem(primContains, "UV3s");

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.UVs4))
					AddItem(primContains, "UV4s");

				if (primGroup.VertexType.HasFlag(PrimitiveGroup.VertexTypes.Weights))
					AddItem(primContains, "Weights");
				break;

			case Scenegraph:
				break;

			case ScenegraphBranch sgb:
				AddItem(root, $"{sgb.NumberOfChildren} Children");
				break;

			case ScenegraphDrawable sgd:
				AddItem(root, $"Drawable Name: {sgd.Drawable}");
				AddItem(root, $"Translucent: {sgd.IsTranslucent}");
				break;

			case ScenegraphRoot:
				TreeItem scenegraphRoot = AddItem(root, "No properties available for this chunk");
				scenegraphRoot.SetTooltipText(
					0,
					"No, really, this chunk does nothing"
				);
				break;

			case ScenegraphSortOrder sgso:
				AddItem(root, $"Sort Order: {sgso.SortOrder}");
				break;

			case ScenegraphTransform sgt:
				AddItem(root, $"{sgt.NumberOfChildren} Children");
				ViewMatrix(root, "Transform:", sgt.Transform);
				break;

			case Skeleton skel:
				AddItem(root, $"{skel.GetNumJoints()} Joints");
				break;

			case StateProp sp:
				AddItem(root, $"Object Factory: {sp.ObjectFactory}");
				AddItem(root, $"{sp.NumberOfStates} States");
				break;

			case StatePropCallback spc:
				AddItem(root, $"Event: {spc.Event}");
				AddItem(root, $"Frame: {spc.Frame}");
				break;

			case StatePropEvent spe:
				AddItem(root, $"State: {spe.State}");
				AddItem(root, $"Event: {spe.Event}");
				break;

			case StatePropFrameController spfc:
				AddItem(root, $"Cyclic: {spfc.IsCyclic}");
				AddItem(root, $"{spfc.NumberOfCycles} Cycles");
				AddItem(root, $"Hold Frame: {spfc.HoldFrame}");
				AddItem(root, $"Minimum Frame: {spfc.MinFrame}");
				AddItem(root, $"Maximum Frame: {spfc.MaxFrame}");
				AddItem(root, $"Relative Speed: {spfc.RelativeSpeed}");
				break;

			case StatePropState sps:
				AddItem(root, $"Automatic Transition: {sps.AutoTransition}");
				AddItem(root, $"Out State: {sps.OutState}");
				AddItem(root, $"{sps.NumberOfDrawables} Drawables");
				AddItem(root, $"{sps.NumberOfFrameControllers} Frame Controllers");
				AddItem(root, $"{sps.NumberOfEvents} Events");
				AddItem(root, $"{sps.NumberOfCallbacks} Callbacks");
				AddItem(root, $"Out Frame: {sps.OutFrame}");
				break;

			case StatePropVisibility spv:
				AddItem(root, $"Visible: {spv.Visible}");
				break;

			case StaticPhysicsObject:
				// No code needed
				// since this is just VersionNamed
				break;

			case TriggerVolume trigger:
				AddItem(root, $"Is Rect: {trigger.IsRect}");
				AddItem(root, $"Half Extents: {Util.PrintVector3(trigger.HalfExtents)}");
				ViewMatrix(root, "Shape:", trigger.Shape);
				break;

			case UVList list:
				root.SetText(
					0,
					"UV List"
				);

				AddItemList(root, "UV", list.UVs);
				break;

			case WeightList list:
				root.SetText(
					0,
					"Weight List"
				);

				AddItemList(root, "Weight", list.Weights);
				break;
			#endregion

			#region Animation Chunks
			case AnimatedObject animObj:
				AddItem(root, $"Associated Factory: {animObj.Factory}");
				AddItem(root, $"Starting Animation: {animObj.StartAnimation}");
				break;

			case AnimatedObjectWrapper animObjWrapper:
				AddItem(root, $"Alpha: {animObjWrapper.HasAlpha}");
				break;

			case AnimatedObjectAnimation aoa:
				AddItem(root, $"Frame Rate: {aoa.FrameRate}");
				AddItem(root, $"{aoa.NumberOfFrameControllers} Frame Controllers");
				break;

			case AnimatedObjectFactory aof:
				AddItem(root, $"Unknown: {aof.Unknown}");
				AddItem(root, $"{aof.NumberOfAnimations} Animations");
				break;

			case Pure3D.Chunks.Animation anim:
				AddItem(root, $"{anim.NumberOfFrames} Frames");
				AddItem(root, $"Frame Rate: {anim.FrameRate}");
				AddItem(root, $"Looping: {anim.Looping}");
				break;

			case AnimationGroup animGroup:
				AddItem(root, $"Group: {animGroup.GroupId}");
				AddItem(root, $"{animGroup.NumberOfChannels} Channels");
				break;

			case AnimationGroupList agl:
				AddItem(root, $"Version: {agl.Version}");
				AddItem(root, $"{agl.NumberOfGroups} Groups");
				break;

			case AnimationSize animSize:
				AddItem(root, $"Version: {animSize.Version}");
				AddItem(root, $"GameCube: {animSize.GameCube}");
				AddItem(root, $"PC: {animSize.PC}");
				AddItem(root, $"PS2: {animSize.PS2}");
				AddItem(root, $"Xbox: {animSize.Xbox}");
				break;

			case BooleanChannel bc:
				TreeItem bcFrames = ViewAnimationChannelChunk(root, bc);
				AddItem(bcFrames, $"Start State: {bc.Start}");
				AddItemList(bcFrames, "Frame", bc.Values);
				break;

			case ChannelInterpolationMode mode:
				AddItem(root, $"Version: {mode.Version}");
				AddItem(root, $"Mode: {mode.Mode}");
				break;

			case ColourChannel cc:
				TreeItem ccFrames = ViewAnimationChannelChunk(root, cc);
				AddColourList(ccFrames, cc.Values);
				break;

			case CompressedQuaternionChannel cqc:
				TreeItem cqcFrames = ViewAnimationChannelChunk(root, cqc);
				AddItemList(cqcFrames, "Frame", cqc.Frames);
				break;

			case EntityChannel ec:
				TreeItem ecFrames = ViewAnimationChannelChunk(root, ec);
				AddItemList(ecFrames, "Frame", ec.Values);
				break;

			case Float1Channel f1c:
				TreeItem f1cFrames = ViewAnimationChannelChunk(root, f1c);
				AddItemList(f1cFrames, "Frame", f1c.Frames);
				break;

			case IntegerChannel ic:
				TreeItem icFrames = ViewAnimationChannelChunk(root, ic);
				AddItemList(icFrames, "Frame", ic.Frames);
				break;

			case QuaternionChannel qc:
				TreeItem qcFrames = ViewAnimationChannelChunk(root, qc);
				AddItemList(qcFrames, "Frame", qc.Frames);
				break;

			case Vector1Channel v1c:
				TreeItem v1cFrames = ViewAnimationChannelChunk(root, v1c);
				AddItem(v1cFrames, $"Mapping: {v1c.Mapping}");
				AddItem(v1cFrames, $"Constants: ({v1c.Constants.X}, {v1c.Constants.Y}, {v1c.Constants.Z})");
				AddItemList(v1cFrames, "Frame", v1c.Frames);
				break;

			case Vector2Channel v2c:
				TreeItem v2cFrames = ViewAnimationChannelChunk(root, v2c);
				AddItem(v2cFrames, $"Mapping: {v2c.Mapping}");
				AddItem(v2cFrames, $"Constants: ({v2c.Constants.X}, {v2c.Constants.Y}, {v2c.Constants.Z})");
				AddItemList(v2cFrames, "Frame", v2c.Frames);
				break;

			case Vector3Channel v3c:
				TreeItem v3cFrames = ViewAnimationChannelChunk(root, v3c);
				AddItemList(v3cFrames, "Frame", v3c.Frames);
				break;
			#endregion

			#region Shader Chunks
			case Pure3D.Chunks.Shader shader:
				AddItem(root, $"Translucency: {shader.HasTranslucency}");
				AddItem(root, $"Vertex Mask: {shader.VertexMask}");
				AddItem(root, $"Vertex Needs: {shader.VertexNeeds}");
				AddItem(root, $"{shader.GetNumParams()} Parameters");
				break;

			case ShaderColourParam shaderColour:
				AddItem(root, $"Parameter: {shaderColour.Param}");

				Color scColour = Util.GetColour(shaderColour.Colour);
				AddItem(root, "Colour: ({scColour.R}, {scColour.G}, {scColour.B}, {scColour.A})");
				CreateItem(root).SetCustomBgColor(0, scColour);
				break;

			case ShaderFloatParam shaderFloat:
				AddItem(root, $"Parameter: {shaderFloat.Param}");
				AddItem(root, $"Value: {shaderFloat.Value}");
				break;

			case ShaderIntParam shaderInt:
				AddItem(root, $"Parameter: {shaderInt.Param}");
				AddItem(root, $"Value: {shaderInt.Value}");
				break;

			case ShaderTextureParam shaderTex:
				AddItem(root, $"Parameter: {shaderTex.Param}");
				AddItem(root, $"Value: {shaderTex.Value}");
				break;

			case VertexShader vShader:
				AddItem(root, $"Shader Name: {vShader.VertexShaderName}");
				break;
			#endregion

			#region Misc Chunks
			case GameAttribute attribute:
				AddItem(root, $"{attribute.NumberOfParameters} Parameters");
				break;

			case GameAttributeParam aParam:
				AddItem(root, $"Parameter: {aParam.Parameter}");
				AddItem(root, $"Value: {aParam.Value}");
				break;

			case MultiController mController:
				AddItem(root, $"{mController.NumberOfTracks} Tracks");
				AddItem(root, $"Length: {mController.Length}");
				AddItem(root, $"Frame Rate: {mController.FrameRate}");
				break;

			case MultiControllerTrackList mctl:
				root.SetText(
					0,
					"Multi Controller Tracks"
				);

				for (uint i = 0; i < mctl.NumberOfTracks; i++)
				{
					TreeItem mcTracksIndex = AddItem(root, $"Track {i + 1}: {mctl.Names[i]}");
					AddItem(mcTracksIndex, $"Time: {mctl.Starts[i]} - {mctl.Ends[i]}");
					AddItem(mcTracksIndex, $"Scale: {mctl.Scales[i]}");
				}
				break;

			case RenderStatus rs:
				AddItem(root, "Casts Shadow: {rs.CastShadow}");
				break;

			case Root:
				root.SetText(
					0,
					"Root Chunk"
				);
				break;
			#endregion

			case Unknown unknown:
				root.SetText(
					0,
					"Unknown Chunk"
				);

				AddItem(root, $"Decimal Type: {chunk.Type}");
				AddItem(root, $"Hexadecimal Type: 0x{chunk.Type:X}");
				AddItemList(
					AddItem(root, $"{unknown.Data.Length} Bytes Long"),
					"Byte",
					unknown.Data
				);
				break;

			default:
				// If the chunk is not viewable
				// Notify the user
				AddItem(root, "Properties not yet viewable...");
				break;
		}
	}

	/// <summary>
	/// Reads the properties of all base Animation Channels
	/// </summary>
	/// <param name="root">Root chunk of the Pure3D file</param>
	/// <param name="channel">Chunk to view the properties of</param>
	/// <returns>TreeItem to add frames underneath</returns>
	public TreeItem ViewAnimationChannelChunk(TreeItem root, AnimationChannel channel)
	{
		TreeItem channelVers = CreateItem(root);
		channelVers.SetText(
			0,
			$"Version: {channel.Version}"
		);

		TreeItem channelParam = CreateItem(root);
		channelParam.SetText(
			0,
			$"Parameter: {channel.Parameter}"
		);

		TreeItem channelFrames = CreateItem(root);
		channelFrames.SetText(
			0,
			$"{channel.NumberOfFrames} Frames"
		);

		return channelFrames;
	}

	/// <summary>
	/// Details a Pure3D Matrix in a readable format
	/// </summary>
	/// <param name="root">Root chunk of the Details tree</param>
	/// <param name="flavourText">Title of the Matrix</param>
	/// <param name="matrix">Pure3D Matrix to be read</param>
	public void ViewMatrix(TreeItem root, string flavourText, Pure3D.Matrix matrix)
	{

		TreeItem newItem = CreateItem(root);
		newItem.SetText(
			0,
			flavourText
		);

		string[] matrixStrings = Util.PrintMatrix(matrix);
		TreeItem sgtTransform0 = CreateItem(newItem);
		sgtTransform0.SetText(
			0,
			matrixStrings[0]
		);

		TreeItem sgtTransform1 = CreateItem(newItem);
		sgtTransform1.SetText(
			0,
			matrixStrings[1]
		);

		TreeItem sgtTransform2 = CreateItem(newItem);
		sgtTransform2.SetText(
			0,
			matrixStrings[2]
		);

		TreeItem sgtTransform3 = CreateItem(newItem);
		sgtTransform3.SetText(
			0,
			matrixStrings[3]
		);
	}

	/// <summary>
	/// Adds an item to the tree
	/// </summary>
	/// <param name="parent">Parent of the new item</param>
	/// <param name="flavourText">Text for the new item</param>
	/// <returns>Newly created TreeItem</returns>
	public TreeItem AddItem(TreeItem parent, string flavourText)
	{
		TreeItem newItem = CreateItem(parent);
		newItem.SetText(
			0,
			flavourText
		);
		newItem.SetTooltipText(0, "");
		return newItem;
	}

	/// <summary>
	/// Adds an item for every element of an Array
	/// </summary>
	/// <param name="parent">Parent of the new items</param>
	/// <param name="indexText">Text describing what the items are</param>
	/// <param name="list">Array to add items for</param>
	public void AddItemList(TreeItem parent, string indexText, Array list)
	{
		for (uint i = 0; i < list.Length; i++)
		{
			AddItem(
				parent,
				$"{indexText} {i + 1}: {list.GetValue(i)}"
			);
		}
	}

	/// <summary>
	/// Adds an item and a Colour for every element of an Array
	/// </summary>
	/// <param name="parent">Parent of the new items</param>
	/// <param name="list">Array Of unsigned integers to add items for</param>
	public void AddColourList(TreeItem parent, uint[] list)
	{
		for (uint i = 0; i < list.Length; i++)
		{
			Color colour = Util.GetColour(list[i]);
			TreeItem colourItem = CreateItem(parent);
			TreeItem colourValue = CreateItem(parent);
			colourItem.SetText(
				0,
				$"Colour {i + 1}: ({colour.R}, {colour.G}, {colour.B}, {colour.A})"
			);
			colourValue.SetCustomBgColor(0, colour);
		}
	}
}
