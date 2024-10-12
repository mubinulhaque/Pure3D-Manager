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
			TreeItem unknownName = CreateItem(root);
			unknownName.SetText(
				0,
				$"Name: {named.Name}"
			);
		}

		// Add Version of chunk, where possible
		if (chunk is VersionNamed vNamed)
		{
			TreeItem unknownName = CreateItem(root);
			unknownName.SetText(
				0,
				$"Version: {vNamed.Version}"
			);
		}

		// Load the chunk's details based on its type
		switch (chunk)
		{
			#region 2D Chunks
			case Pure3D.Chunks.Image img:
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
				TreeItem imgDataLen = CreateItem(root);
				imgDataLen.SetText(
					0,
					$"{imgData.Data.Length} Bytes Long"
				);
				break;
			#endregion

			#region 3D Chunks
			case AnimatedDynamicPhysicsObject adpo:
				TreeItem adpoRenderOrder = CreateItem(root);
				adpoRenderOrder.SetText(
					0,
					$"Render Order: {adpo.RenderOrder}"
				);
				break;

			case BoundingBox bBox:
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

			case CollisionAABB colAABB:
				TreeItem colAABBNothing = CreateItem(root);
				colAABBNothing.SetText(
					0,
					$"Nothing: {colAABB.Nothing}"
				);
				colAABBNothing.SetTooltipText(
					0,
					"No, really, this does nothing"
				);
				break;

			case CollisionAttribute colAttribute:
				TreeItem colAttributeStatic = CreateItem(root);
				colAttributeStatic.SetText(
					0,
					$"Static: {colAttribute.IsStatic}"
				);

				TreeItem colAttributeArea = CreateItem(root);
				colAttributeArea.SetText(
					0,
					$"Default Area: {colAttribute.DefaultArea}"
				);

				TreeItem colAttributeRoll = CreateItem(root);
				colAttributeRoll.SetText(
					0,
					$"Can Roll: {colAttribute.CanRoll}"
				);

				TreeItem colAttributeSlide = CreateItem(root);
				colAttributeSlide.SetText(
					0,
					$"Can Slide: {colAttribute.CanSlide}"
				);

				TreeItem colAttributeSpin = CreateItem(root);
				colAttributeSpin.SetText(
					0,
					$"Can Spin: {colAttribute.CanSpin}"
				);

				TreeItem colAttributeBounce = CreateItem(root);
				colAttributeBounce.SetText(
					0,
					$"Can Bounce: {colAttribute.CanBounce}"
				);

				TreeItem colAttribute1 = CreateItem(root);
				colAttribute1.SetText(
					0,
					$"Extra Attribute 1: {colAttribute.ExtraAttribute1}"
				);

				TreeItem colAttribute2 = CreateItem(root);
				colAttribute2.SetText(
					0,
					$"Extra Attribute 2: {colAttribute.ExtraAttribute2}"
				);

				TreeItem colAttribute3 = CreateItem(root);
				colAttribute3.SetText(
					0,
					$"Extra Attribute 3: {colAttribute.ExtraAttribute3}"
				);
				break;

			case CollisionEffect colEffect:
				TreeItem colEffectType = CreateItem(root);
				colEffectType.SetText(
					0,
					$"Type: {colEffect.Classtype}"
				);

				TreeItem colEffectProp = CreateItem(root);
				colEffectProp.SetText(
					0,
					$"Physics Prop ID: {colEffect.PhysicsProp}"
				);

				TreeItem colEffectSound = CreateItem(root);
				colEffectSound.SetText(
					0,
					$"Sound: {colEffect.Sound}"
				);
				break;

			case CollisionOBB colOBB:
				TreeItem colOBBExtents = CreateItem(root);
				colOBBExtents.SetText(
					0,
					$"Half Extents: {Util.PrintVector3(colOBB.HalfExtents)}"
				);
				break;

			case CollisionObject collisionObject:
				TreeItem collisionObjectOwners = CreateItem(root);
				collisionObjectOwners.SetText(
					0,
					$"{collisionObject.NumberOfOwners} Owners"
				);

				TreeItem collisionObjectMat = CreateItem(root);
				collisionObjectMat.SetText(
					0,
					$"Material: {collisionObject.Material}"
				);

				TreeItem collisionObjectSubObjects = CreateItem(root);
				collisionObjectSubObjects.SetText(
					0,
					$"{collisionObject.NumberOfOwners} Sub Objects"
				);
				break;

			case CollisionVector colVector:
				TreeItem colVectorItem = CreateItem(root);
				colVectorItem.SetText(
					0,
					$"Vector: {Util.PrintVector3(colVector.Vector)}"
				);
				break;

			case CollisionVolume colVol:
				TreeItem colVolSubVolumes = CreateItem(root);
				colVolSubVolumes.SetText(
					0,
					$"{colVol.NumberOfSubVolumes} Sub Volumes"
				);

				TreeItem colVolOwnerIndex = CreateItem(root);
				colVolOwnerIndex.SetText(
					0,
					$"Owner Index: {colVol.OwnerIndex}"
				);

				TreeItem colVolObject = CreateItem(root);
				colVolObject.SetText(
					0,
					$"Object Reference Index: {colVol.ObjectReferenceIndex}"
				);
				break;

			case CollisionVolumeOwner colVolOwner:
				TreeItem colVolOwnerNames = CreateItem(root);
				colVolOwnerNames.SetText(
					0,
					$"{colVolOwner.NumberOfNames} Names"
				);
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

			case CompositeDrawableProp cdp:
				TreeItem cdpTranslucent = CreateItem(root);
				cdpTranslucent.SetText(
					0,
					$"Translucent: {cdp.IsTranslucent}"
				);

				TreeItem cdpJoint = CreateItem(root);
				cdpJoint.SetText(
					0,
					$"Skeleton Joint Index: {cdp.SkeletonJointID}"
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

			case InstanceList:
				// No code needed
				// Only a Name attribute
				break;

			case Intersect intersect:
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

			case ParticleSystem2 pSystem2:
				TreeItem pSystem2Factory = CreateItem(root);
				pSystem2Factory.SetText(
					0,
					$"Factory: {pSystem2.Factory}"
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

			case Scenegraph:
				break;

			case ScenegraphBranch sgb:
				TreeItem sgbChildren = CreateItem(root);
				sgbChildren.SetText(
					0,
					$"{sgb.NumberOfChildren} Children"
				);
				break;

			case ScenegraphDrawable sDrawable:
				TreeItem sdDrawable = CreateItem(root);
				sdDrawable.SetText(
					0,
					$"Drawable Name: {sDrawable.Drawable}"
				);

				TreeItem sdTranslucent = CreateItem(root);
				sdTranslucent.SetText(
					0,
					$"Translucent: {sDrawable.IsTranslucent}"
				);
				break;

			case ScenegraphRoot:
				TreeItem scenegraphRoot = CreateItem(root);
				scenegraphRoot.SetText(
					0,
					"No properties available for this chunk"
				);
				scenegraphRoot.SetTooltipText(
					0,
					"No, really, this chunk does nothing"
				);
				break;

			case ScenegraphTransform sgt:
				TreeItem sgtChildren = CreateItem(root);
				sgtChildren.SetText(
					0,
					$"{sgt.NumberOfChildren} Children"
				);

				TreeItem sgtTransform = CreateItem(root);
				sgtTransform.SetText(
					0,
					"Transform:"
				);

				string[] sgtMatrix = Util.PrintMatrix(sgt.Transform);
				TreeItem sgtTransform0 = CreateItem(sgtTransform);
				sgtTransform0.SetText(
					0,
					sgtMatrix[0]
				);

				TreeItem sgtTransform1 = CreateItem(sgtTransform);
				sgtTransform1.SetText(
					0,
					sgtMatrix[1]
				);

				TreeItem sgtTransform2 = CreateItem(sgtTransform);
				sgtTransform2.SetText(
					0,
					sgtMatrix[2]
				);

				TreeItem sgtTransform3 = CreateItem(sgtTransform);
				sgtTransform3.SetText(
					0,
					sgtMatrix[3]
				);
				break;

			case Skeleton skel:
				TreeItem skelJoints = CreateItem(root);
				skelJoints.SetText(
					0,
					$"{skel.GetNumJoints()} Joints"
				);
				break;

			case StateProp sp:
				TreeItem spFactory = CreateItem(root);
				spFactory.SetText(
					0,
					$"Object Factory: {sp.ObjectFactory}"
				);

				TreeItem spdStates = CreateItem(root);
				spdStates.SetText(
					0,
					$"{sp.NumberOfStates} States"
				);
				break;

			case StatePropCallback spc:
				TreeItem spcEvent = CreateItem(root);
				spcEvent.SetText(
					0,
					$"Event: {spc.Event}"
				);

				TreeItem spcFrame = CreateItem(root);
				spcFrame.SetText(
					0,
					$"Frame: {spc.Frame}"
				);
				break;

			case StatePropEvent spe:
				TreeItem speState = CreateItem(root);
				speState.SetText(
					0,
					$"State: {spe.State}"
				);

				TreeItem speEvent = CreateItem(root);
				speEvent.SetText(
					0,
					$"Event: {spe.Event}"
				);
				break;

			case StatePropFrameController spfc:
				TreeItem spfcCyclic = CreateItem(root);
				spfcCyclic.SetText(
					0,
					$"Cyclic: {spfc.IsCyclic}"
				);

				TreeItem spfcCycles = CreateItem(root);
				spfcCycles.SetText(
					0,
					$"{spfc.NumberOfCycles} Cycles"
				);

				TreeItem spfcHoldFrame = CreateItem(root);
				spfcHoldFrame.SetText(
					0,
					$"Hold Frame: {spfc.HoldFrame}"
				);

				TreeItem spfcMinFrame = CreateItem(root);
				spfcMinFrame.SetText(
					0,
					$"Minimum Frame: {spfc.MinFrame}"
				);

				TreeItem spfcMaxFrame = CreateItem(root);
				spfcMaxFrame.SetText(
					0,
					$"Maximum Frame: {spfc.MaxFrame}"
				);

				TreeItem spfcSpeed = CreateItem(root);
				spfcSpeed.SetText(
					0,
					$"Relative Speed: {spfc.RelativeSpeed}"
				);
				break;

			case StatePropState sps:
				TreeItem spsTrans = CreateItem(root);
				spsTrans.SetText(
					0,
					$"Automatic Transition: {sps.AutoTransition}"
				);

				TreeItem spsState = CreateItem(root);
				spsState.SetText(
					0,
					$"Out State: {sps.OutState}"
				);

				TreeItem spsDrawables = CreateItem(root);
				spsDrawables.SetText(
					0,
					$"{sps.NumberOfDrawables} Drawables"
				);

				TreeItem spsControllers = CreateItem(root);
				spsControllers.SetText(
					0,
					$"{sps.NumberOfFrameControllers} Frame Controllers"
				);

				TreeItem spsEvents = CreateItem(root);
				spsEvents.SetText(
					0,
					$"{sps.NumberOfEvents} Events"
				);

				TreeItem spsCallbacks = CreateItem(root);
				spsCallbacks.SetText(
					0,
					$"{sps.NumberOfCallbacks} Callbacks"
				);

				TreeItem spsFrame = CreateItem(root);
				spsFrame.SetText(
					0,
					$"Out Frame: {sps.OutFrame}"
				);
				break;

			case StatePropVisibility spv:
				TreeItem spvVisible = CreateItem(root);
				spvVisible.SetText(
					0,
					$"Visible: {spv.Visible}"
				);
				break;

			case StaticPhysicsObject:
				// No code needed, since this is just VersionNamed
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
			case AnimatedObject animObj:
				TreeItem animObjFactory = CreateItem(root);
				animObjFactory.SetText(
					0,
					$"Associated Factory: {animObj.Factory}"
				);

				TreeItem animObjStart = CreateItem(root);
				animObjStart.SetText(
					0,
					$"Starting Animation: {animObj.StartAnimation}"
				);
				break;

			case AnimatedObjectWrapper animObjWrapper:
				TreeItem animObjWrapperAlpha = CreateItem(root);
				animObjWrapperAlpha.SetText(
					0,
					$"Alpha: {animObjWrapper.HasAlpha}"
				);
				break;

			case AnimatedObjectAnimation aoa:
				TreeItem aoaFrameRate = CreateItem(root);
				aoaFrameRate.SetText(
					0,
					$"Frame Rate: {aoa.FrameRate}"
				);

				TreeItem aoaFrameControllers = CreateItem(root);
				aoaFrameControllers.SetText(
					0,
					$"{aoa.NumberOfFrameControllers} Frame Controllers"
				);
				break;

			case AnimatedObjectFactory aof:
				TreeItem aofUnknown = CreateItem(root);
				aofUnknown.SetText(
					0,
					$"Unknown: {aof.Unknown}"
				);

				TreeItem aofAnims = CreateItem(root);
				aofAnims.SetText(
					0,
					$"{aof.NumberOfAnimations} Animations"
				);
				break;

			case Pure3D.Chunks.Animation anim:
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

			case BooleanChannel bc:
				TreeItem bcFrames = ViewAnimationChannelChunk(root, bc);

				TreeItem bcStart = CreateItem(bcFrames);
				bcStart.SetText(
					0,
					$"Start State: {bc.Start}"
				);

				for (uint i = 0; i < bc.Frames.Length; i++)
				{
					TreeItem member = CreateItem(bcFrames);
					member.SetText(
						0,
						$"Frame {bc.Frames[i]}: {bc.Values[i]}"
					);
				}
				break;

			case ChannelInterpolationMode mode:
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

			case ColourChannel cc:
				TreeItem ccFrames = ViewAnimationChannelChunk(root, cc);

				for (uint i = 0; i < cc.Values.Length; i++)
				{
					Color colour = cc.Values[i];

					TreeItem colourItem = CreateItem(ccFrames);
					TreeItem colourValue = CreateItem(ccFrames);
					colourItem.SetText(
						0,
						$"Colour {i + 1}: ({colour.R}, {colour.G}, {colour.B}, {colour.A})"
					);
					colourValue.SetCustomBgColor(0, colour);
				}
				break;

			case CompressedQuaternionChannel cqc:
				TreeItem cqcFrames = ViewAnimationChannelChunk(root, cqc);

				for (uint i = 0; i < cqc.Frames.Length; i++)
				{
					TreeItem member = CreateItem(cqcFrames);
					Pure3D.Quaternion value = cqc.Values[i];
					member.SetText(
						0,
						$"Frame {cqc.Frames[i] + 1}: ({value.X}, {value.Y}, {value.Z}, {value.W})"
					);
				}
				break;

			case EntityChannel ec:
				TreeItem ecFrames = ViewAnimationChannelChunk(root, ec);

				for (uint i = 0; i < ec.Frames.Length; i++)
				{
					TreeItem member = CreateItem(ecFrames);
					member.SetText(
						0,
						$"Frame {ec.Frames[i] + 1}: {ec.Values[i]}"
					);
				}
				break;

			case Float1Channel f1c:
				TreeItem f1cFrames = ViewAnimationChannelChunk(root, f1c);

				for (uint i = 0; i < f1c.Frames.Length; i++)
				{
					TreeItem member = CreateItem(f1cFrames);
					member.SetText(
						0,
						$"Frame {f1c.Frames[i] + 1}: {f1c.Values[i]}"
					);
				}
				break;

			case IntegerChannel ic:
				TreeItem icFrames = ViewAnimationChannelChunk(root, ic);

				for (uint i = 0; i < ic.Frames.Length; i++)
				{
					TreeItem member = CreateItem(icFrames);
					member.SetText(
						0,
						$"Frame {ic.Frames[i] + 1}: {ic.Values[i]}"
					);
				}
				break;

			case QuaternionChannel qc:
				TreeItem qcFrames = ViewAnimationChannelChunk(root, qc);

				for (uint i = 0; i < qc.Frames.Length; i++)
				{
					TreeItem member = CreateItem(qcFrames);
					member.SetText(
						0,
						$"Frame {qc.Frames[i] + 1}: {qc.Values[i]}"
					);
				}
				break;

			case Vector1Channel v1c:
				TreeItem v1cFrames = ViewAnimationChannelChunk(root, v1c);

				TreeItem v1cMapping = CreateItem(v1cFrames);
				v1cMapping.SetText(
					0,
					$"Mapping: {v1c.Mapping}"
				);

				TreeItem v1cConstants = CreateItem(v1cFrames);
				v1cConstants.SetText(
					0,
					$"Constants: ({v1c.Constants.X}, {v1c.Constants.Y}, {v1c.Constants.Z})"
				);

				for (uint i = 0; i < v1c.Frames.Length; i++)
				{
					TreeItem member = CreateItem(v1cFrames);
					member.SetText(
						0,
						$"Frame {v1c.Frames[i] + 1}: {v1c.Values[i]}"
					);
				}
				break;

			case Vector2Channel v2c:
				TreeItem v2cFrames = ViewAnimationChannelChunk(root, v2c);

				TreeItem v2cMapping = CreateItem(v2cFrames);
				v2cMapping.SetText(
					0,
					$"Mapping: {v2c.Mapping}"
				);

				TreeItem v2cConstants = CreateItem(v2cFrames);
				v2cConstants.SetText(
					0,
					$"Constants: ({v2c.Constants.X}, {v2c.Constants.Y}, {v2c.Constants.Z})"
				);

				for (uint i = 0; i < v2c.Frames.Length; i++)
				{
					TreeItem member = CreateItem(v2cFrames);
					Pure3D.Vector2 value = v2c.Values[i];
					member.SetText(
						0,
						$"Frame {v2c.Frames[i] + 1}: ({value.X}, {value.Y})"
					);
				}
				break;

			case Vector3Channel v3c:
				TreeItem v3cFrames = ViewAnimationChannelChunk(root, v3c);

				for (uint i = 0; i < v3c.Frames.Length; i++)
				{
					TreeItem member = CreateItem(v3cFrames);
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
				TreeItem vsName = CreateItem(root);
				vsName.SetText(
					0,
					$"Shader Name: {vShader.VertexShaderName}"
				);
				break;
			#endregion

			#region Misc Chunks
			case GameAttribute attribute:
				TreeItem attributeParams = CreateItem(root);
				attributeParams.SetText(
					0,
					$"{attribute.NumberOfParameters} Parameters"
				);
				break;

			case GameAttributeParam aParam:
				TreeItem aParamName = CreateItem(root);
				aParamName.SetText(
					0,
					$"Parameter: {aParam.Parameter}"
				);

				TreeItem aParamValue = CreateItem(root);
				aParamValue.SetText(
					0,
					$"Value: {aParam.Value}"
				);
				break;

			case MultiController mController:
				TreeItem mControllerTracks = CreateItem(root);
				mControllerTracks.SetText(
					0,
					$"{mController.NumberOfTracks} Tracks"
				);

				TreeItem mControllerLength = CreateItem(root);
				mControllerLength.SetText(
					0,
					$"Length: {mController.Length}"
				);

				TreeItem mControllerFrameRate = CreateItem(root);
				mControllerFrameRate.SetText(
					0,
					$"Frame Rate: {mController.FrameRate}"
				);
				break;

			case MultiControllerTrackList mctl:
				root.SetText(
					0,
					"Multi Controller Tracks"
				);

				for (uint i = 0; i < mctl.NumberOfTracks; i++)
				{
					TreeItem mcTracksIndex = CreateItem(root);
					mcTracksIndex.SetText(
						0,
						$"Track {i + 1}: {mctl.Names[i]}"
					);

					TreeItem mcTracksTime = CreateItem(mcTracksIndex);
					mcTracksTime.SetText(
						0,
						$"Time: {mctl.Starts[i]} - {mctl.Ends[i]}"
					);

					TreeItem mcTracksScale = CreateItem(mcTracksIndex);
					mcTracksScale.SetText(
						0,
						$"Scale: {mctl.Scales[i]}"
					);
				}
				break;

			case RenderStatus rs:
				TreeItem rsCastShadow = CreateItem(root);
				rsCastShadow.SetText(
					0,
					$"Casts Shadow: {rs.CastShadow}"
				);
				break;

			case Root rootChunk:
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

				TreeItem unknownLength = CreateItem(root);
				unknownLength.SetText(
					0,
					$"{unknown.Data.Length} Bytes Long"
				);

				for (uint i = 0; i < unknown.Data.Length; i++)
				{
					TreeItem unknownByte = CreateItem(unknownLength);
					unknownByte.SetText(
						0,
						$"Byte {i + 1}: {unknown.Data[i]} (0x{unknown.Data[i]:X})"
					);
				}
				break;

			default:
				// If the chunk is not viewable
				// Notify the user
				TreeItem unnamedWarning = CreateItem(root);
				unnamedWarning.SetText(
					0,
					"Properties not yet viewable..."
				);
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
}
