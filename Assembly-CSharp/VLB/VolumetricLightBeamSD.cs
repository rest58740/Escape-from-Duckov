using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x0200003D RID: 61
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[SelectionBase]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-sd/")]
	[AddComponentMenu("VLB/SD/Volumetric Light Beam SD")]
	public class VolumetricLightBeamSD : VolumetricLightBeamAbstractBase
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00008BB5 File Offset: 0x00006DB5
		public ColorMode usedColorMode
		{
			get
			{
				if (Config.Instance.featureEnabledColorGradient == FeatureEnabledColorGradient.Off)
				{
					return ColorMode.Flat;
				}
				return this.colorMode;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00008BCB File Offset: 0x00006DCB
		private bool useColorFromAttachedLightSpot
		{
			get
			{
				return this.colorFromLight && base.lightSpotAttached != null;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00008BE3 File Offset: 0x00006DE3
		private bool useColorTemperatureFromAttachedLightSpot
		{
			get
			{
				return this.useColorFromAttachedLightSpot && base.lightSpotAttached.useColorTemperature && Config.Instance.useLightColorTemperature;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00008C06 File Offset: 0x00006E06
		// (set) Token: 0x060001FB RID: 507 RVA: 0x00008C0E File Offset: 0x00006E0E
		[Obsolete("Use 'intensityGlobal' or 'intensityInside' instead")]
		public float alphaInside
		{
			get
			{
				return this.intensityInside;
			}
			set
			{
				this.intensityInside = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00008C17 File Offset: 0x00006E17
		// (set) Token: 0x060001FD RID: 509 RVA: 0x00008C1F File Offset: 0x00006E1F
		[Obsolete("Use 'intensityGlobal' or 'intensityOutside' instead")]
		public float alphaOutside
		{
			get
			{
				return this.intensityOutside;
			}
			set
			{
				this.intensityOutside = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00008C28 File Offset: 0x00006E28
		// (set) Token: 0x060001FF RID: 511 RVA: 0x00008C30 File Offset: 0x00006E30
		public float intensityGlobal
		{
			get
			{
				return this.intensityOutside;
			}
			set
			{
				this.intensityInside = value;
				this.intensityOutside = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00008C40 File Offset: 0x00006E40
		public bool useIntensityFromAttachedLightSpot
		{
			get
			{
				return this.intensityFromLight && base.lightSpotAttached != null;
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00008C58 File Offset: 0x00006E58
		public void GetInsideAndOutsideIntensity(out float inside, out float outside)
		{
			if (this.intensityModeAdvanced)
			{
				inside = this.intensityInside;
				outside = this.intensityOutside;
				return;
			}
			inside = (outside = this.intensityOutside);
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00008C8B File Offset: 0x00006E8B
		public bool useSpotAngleFromAttachedLightSpot
		{
			get
			{
				return this.spotAngleFromLight && base.lightSpotAttached != null;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00008CA3 File Offset: 0x00006EA3
		public float coneAngle
		{
			get
			{
				return Mathf.Atan2(this.coneRadiusEnd - this.coneRadiusStart, this.maxGeometryDistance) * 57.29578f * 2f;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00008CC9 File Offset: 0x00006EC9
		// (set) Token: 0x06000205 RID: 517 RVA: 0x00008CDC File Offset: 0x00006EDC
		public float coneRadiusEnd
		{
			get
			{
				return Utils.ComputeConeRadiusEnd(this.maxGeometryDistance, this.spotAngle);
			}
			set
			{
				this.spotAngle = Utils.ComputeSpotAngle(this.maxGeometryDistance, value);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00008CF0 File Offset: 0x00006EF0
		public float coneVolume
		{
			get
			{
				float num = this.coneRadiusStart;
				float coneRadiusEnd = this.coneRadiusEnd;
				return 1.0471976f * (num * num + num * coneRadiusEnd + coneRadiusEnd * coneRadiusEnd) * this.fallOffEnd;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00008D24 File Offset: 0x00006F24
		public float coneApexOffsetZ
		{
			get
			{
				float num = this.coneRadiusStart / this.coneRadiusEnd;
				if (num != 1f)
				{
					return this.maxGeometryDistance * num / (1f - num);
				}
				return float.MaxValue;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00008D5D File Offset: 0x00006F5D
		public Vector3 coneApexPositionLocal
		{
			get
			{
				return new Vector3(0f, 0f, -this.coneApexOffsetZ);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00008D78 File Offset: 0x00006F78
		public Vector3 coneApexPositionGlobal
		{
			get
			{
				return base.transform.localToWorldMatrix.MultiplyPoint(this.coneApexPositionLocal);
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00008D9E File Offset: 0x00006F9E
		public override bool IsScalable()
		{
			return true;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00008DA1 File Offset: 0x00006FA1
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00008DBD File Offset: 0x00006FBD
		public int geomSides
		{
			get
			{
				if (this.geomMeshType != MeshType.Custom)
				{
					return Config.Instance.sharedMeshSides;
				}
				return this.geomCustomSides;
			}
			set
			{
				this.geomCustomSides = value;
				Debug.LogWarningFormat("The setter VLB.{0}.geomSides is OBSOLETE and has been renamed to geomCustomSides.", new object[]
				{
					"VolumetricLightBeamSD"
				});
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00008DDE File Offset: 0x00006FDE
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00008DFA File Offset: 0x00006FFA
		public int geomSegments
		{
			get
			{
				if (this.geomMeshType != MeshType.Custom)
				{
					return Config.Instance.sharedMeshSegments;
				}
				return this.geomCustomSegments;
			}
			set
			{
				this.geomCustomSegments = value;
				Debug.LogWarningFormat("The setter VLB.{0}.geomSegments is OBSOLETE and has been renamed to geomCustomSegments.", new object[]
				{
					"VolumetricLightBeamSD"
				});
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00008E1C File Offset: 0x0000701C
		public Vector3 skewingLocalForwardDirectionNormalized
		{
			get
			{
				if (Mathf.Approximately(this.skewingLocalForwardDirection.z, 0f))
				{
					Debug.LogErrorFormat("Beam {0} has a skewingLocalForwardDirection with a null Z, which is forbidden", new object[]
					{
						base.name
					});
					return Vector3.forward;
				}
				return this.skewingLocalForwardDirection.normalized;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00008E6A File Offset: 0x0000706A
		public bool canHaveMeshSkewing
		{
			get
			{
				return this.geomMeshType == MeshType.Custom;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00008E75 File Offset: 0x00007075
		public bool hasMeshSkewing
		{
			get
			{
				return Config.Instance.featureEnabledMeshSkewing && this.canHaveMeshSkewing && !Mathf.Approximately(Vector3.Dot(this.skewingLocalForwardDirectionNormalized, Vector3.forward), 1f);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000212 RID: 530 RVA: 0x00008EAE File Offset: 0x000070AE
		public Vector4 additionalClippingPlane
		{
			get
			{
				if (!(this.clippingPlaneTransform == null))
				{
					return Utils.PlaneEquation(this.clippingPlaneTransform.forward, this.clippingPlaneTransform.position);
				}
				return Vector4.zero;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00008EDF File Offset: 0x000070DF
		public float attenuationLerpLinearQuad
		{
			get
			{
				if (this.attenuationEquation == AttenuationEquation.Linear)
				{
					return 0f;
				}
				if (this.attenuationEquation == AttenuationEquation.Quadratic)
				{
					return 1f;
				}
				return this.attenuationCustomBlending;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00008F04 File Offset: 0x00007104
		// (set) Token: 0x06000215 RID: 533 RVA: 0x00008F0C File Offset: 0x0000710C
		[Obsolete("Use 'fallOffStart' instead")]
		public float fadeStart
		{
			get
			{
				return this.fallOffStart;
			}
			set
			{
				this.fallOffStart = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00008F15 File Offset: 0x00007115
		// (set) Token: 0x06000217 RID: 535 RVA: 0x00008F1D File Offset: 0x0000711D
		[Obsolete("Use 'fallOffEnd' instead")]
		public float fadeEnd
		{
			get
			{
				return this.fallOffEnd;
			}
			set
			{
				this.fallOffEnd = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00008F26 File Offset: 0x00007126
		// (set) Token: 0x06000219 RID: 537 RVA: 0x00008F2E File Offset: 0x0000712E
		[Obsolete("Use 'fallOffEndFromLight' instead")]
		public bool fadeEndFromLight
		{
			get
			{
				return this.fallOffEndFromLight;
			}
			set
			{
				this.fallOffEndFromLight = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00008F37 File Offset: 0x00007137
		public bool useFallOffEndFromAttachedLightSpot
		{
			get
			{
				return this.fallOffEndFromLight && base.lightSpotAttached != null;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00008F4F File Offset: 0x0000714F
		public float maxGeometryDistance
		{
			get
			{
				return this.fallOffEnd + Mathf.Max(Mathf.Abs(this.tiltFactor.x), Mathf.Abs(this.tiltFactor.y));
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00008F7D File Offset: 0x0000717D
		public bool isNoiseEnabled
		{
			get
			{
				return this.noiseMode > NoiseMode.Disabled;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00008F88 File Offset: 0x00007188
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00008F90 File Offset: 0x00007190
		[Obsolete("Use 'noiseMode' instead")]
		public bool noiseEnabled
		{
			get
			{
				return this.isNoiseEnabled;
			}
			set
			{
				this.noiseMode = (value ? NoiseMode.WorldSpace : NoiseMode.Disabled);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00008F9F File Offset: 0x0000719F
		// (set) Token: 0x06000220 RID: 544 RVA: 0x00008FA7 File Offset: 0x000071A7
		public float fadeOutBegin
		{
			get
			{
				return this._FadeOutBegin;
			}
			set
			{
				this.SetFadeOutValue(ref this._FadeOutBegin, value);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00008FB6 File Offset: 0x000071B6
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00008FBE File Offset: 0x000071BE
		public float fadeOutEnd
		{
			get
			{
				return this._FadeOutEnd;
			}
			set
			{
				this.SetFadeOutValue(ref this._FadeOutEnd, value);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00008FCD File Offset: 0x000071CD
		public bool isFadeOutEnabled
		{
			get
			{
				return this._FadeOutBegin >= 0f && this._FadeOutEnd >= 0f;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00008FEE File Offset: 0x000071EE
		public bool isTilted
		{
			get
			{
				return !this.tiltFactor.Approximately(Vector2.zero, 1E-05f);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00009008 File Offset: 0x00007208
		// (set) Token: 0x06000226 RID: 550 RVA: 0x00009010 File Offset: 0x00007210
		public int sortingLayerID
		{
			get
			{
				return this._SortingLayerID;
			}
			set
			{
				this._SortingLayerID = value;
				if (this.m_BeamGeom)
				{
					this.m_BeamGeom.sortingLayerID = value;
				}
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00009032 File Offset: 0x00007232
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0000903F File Offset: 0x0000723F
		public string sortingLayerName
		{
			get
			{
				return SortingLayer.IDToName(this.sortingLayerID);
			}
			set
			{
				this.sortingLayerID = SortingLayer.NameToID(value);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000904D File Offset: 0x0000724D
		// (set) Token: 0x0600022A RID: 554 RVA: 0x00009055 File Offset: 0x00007255
		public int sortingOrder
		{
			get
			{
				return this._SortingOrder;
			}
			set
			{
				this._SortingOrder = value;
				if (this.m_BeamGeom)
				{
					this.m_BeamGeom.sortingOrder = value;
				}
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00009077 File Offset: 0x00007277
		// (set) Token: 0x0600022C RID: 556 RVA: 0x0000907F File Offset: 0x0000727F
		public bool trackChangesDuringPlaytime
		{
			get
			{
				return this._TrackChangesDuringPlaytime;
			}
			set
			{
				this._TrackChangesDuringPlaytime = value;
				this.StartPlaytimeUpdateIfNeeded();
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000908E File Offset: 0x0000728E
		public bool isCurrentlyTrackingChanges
		{
			get
			{
				return this.m_CoPlaytimeUpdate != null;
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00009099 File Offset: 0x00007299
		public override BeamGeometryAbstractBase GetBeamGeometry()
		{
			return this.m_BeamGeom;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000090A1 File Offset: 0x000072A1
		protected override void SetBeamGeometryNull()
		{
			this.m_BeamGeom = null;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000230 RID: 560 RVA: 0x000090AA File Offset: 0x000072AA
		public int blendingModeAsInt
		{
			get
			{
				return Mathf.Clamp((int)this.blendingMode, 0, Enum.GetValues(typeof(BlendingMode)).Length);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000231 RID: 561 RVA: 0x000090CC File Offset: 0x000072CC
		public Quaternion beamInternalLocalRotation
		{
			get
			{
				if (this.dimensions != Dimensions.Dim3D)
				{
					return Quaternion.LookRotation(Vector3.right, Vector3.up);
				}
				return Quaternion.identity;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000232 RID: 562 RVA: 0x000090EB File Offset: 0x000072EB
		public Vector3 beamLocalForward
		{
			get
			{
				if (this.dimensions != Dimensions.Dim3D)
				{
					return Vector3.right;
				}
				return Vector3.forward;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00009100 File Offset: 0x00007300
		public Vector3 beamGlobalForward
		{
			get
			{
				return base.transform.TransformDirection(this.beamLocalForward);
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00009114 File Offset: 0x00007314
		public override Vector3 GetLossyScale()
		{
			if (this.dimensions != Dimensions.Dim3D)
			{
				return new Vector3(base.transform.lossyScale.z, base.transform.lossyScale.y, base.transform.lossyScale.x);
			}
			return base.transform.lossyScale;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000916C File Offset: 0x0000736C
		public float raycastDistance
		{
			get
			{
				if (!this.hasMeshSkewing)
				{
					return this.maxGeometryDistance;
				}
				float z = this.skewingLocalForwardDirectionNormalized.z;
				if (!Mathf.Approximately(z, 0f))
				{
					return this.maxGeometryDistance / z;
				}
				return this.maxGeometryDistance;
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000091B0 File Offset: 0x000073B0
		private Vector3 ComputeRaycastGlobalVector(Vector3 localVec)
		{
			return base.transform.rotation * this.beamInternalLocalRotation * localVec;
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000237 RID: 567 RVA: 0x000091CE File Offset: 0x000073CE
		public Vector3 raycastGlobalForward
		{
			get
			{
				return this.ComputeRaycastGlobalVector(this.hasMeshSkewing ? this.skewingLocalForwardDirectionNormalized : Vector3.forward);
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000238 RID: 568 RVA: 0x000091EB File Offset: 0x000073EB
		public Vector3 raycastGlobalUp
		{
			get
			{
				return this.ComputeRaycastGlobalVector(Vector3.up);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000239 RID: 569 RVA: 0x000091F8 File Offset: 0x000073F8
		public Vector3 raycastGlobalRight
		{
			get
			{
				return this.ComputeRaycastGlobalVector(Vector3.right);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00009205 File Offset: 0x00007405
		// (set) Token: 0x0600023B RID: 571 RVA: 0x0000921B File Offset: 0x0000741B
		public MaterialManager.SD.DynamicOcclusion _INTERNAL_DynamicOcclusionMode
		{
			get
			{
				if (!Config.Instance.featureEnabledDynamicOcclusion)
				{
					return MaterialManager.SD.DynamicOcclusion.Off;
				}
				return this.m_INTERNAL_DynamicOcclusionMode;
			}
			set
			{
				this.m_INTERNAL_DynamicOcclusionMode = value;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00009224 File Offset: 0x00007424
		public MaterialManager.SD.DynamicOcclusion _INTERNAL_DynamicOcclusionMode_Runtime
		{
			get
			{
				if (!this.m_INTERNAL_DynamicOcclusionMode_Runtime)
				{
					return MaterialManager.SD.DynamicOcclusion.Off;
				}
				return this._INTERNAL_DynamicOcclusionMode;
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00009236 File Offset: 0x00007436
		public void _INTERNAL_SetDynamicOcclusionCallback(string shaderKeyword, MaterialModifier.Callback cb)
		{
			this.m_INTERNAL_DynamicOcclusionMode_Runtime = (cb != null);
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.SetDynamicOcclusionCallback(shaderKeyword, cb);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600023E RID: 574 RVA: 0x0000925C File Offset: 0x0000745C
		// (remove) Token: 0x0600023F RID: 575 RVA: 0x00009294 File Offset: 0x00007494
		public event VolumetricLightBeamSD.OnWillCameraRenderCB onWillCameraRenderThisBeam;

		// Token: 0x06000240 RID: 576 RVA: 0x000092C9 File Offset: 0x000074C9
		public void _INTERNAL_OnWillCameraRenderThisBeam(Camera cam)
		{
			if (this.onWillCameraRenderThisBeam != null)
			{
				this.onWillCameraRenderThisBeam(cam);
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000092DF File Offset: 0x000074DF
		public void RegisterOnBeamGeometryInitializedCallback(VolumetricLightBeamSD.OnBeamGeometryInitialized cb)
		{
			this.m_OnBeamGeometryInitialized = (VolumetricLightBeamSD.OnBeamGeometryInitialized)Delegate.Combine(this.m_OnBeamGeometryInitialized, cb);
			if (this.m_BeamGeom)
			{
				this.CallOnBeamGeometryInitializedCallback();
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000930B File Offset: 0x0000750B
		private void CallOnBeamGeometryInitializedCallback()
		{
			if (this.m_OnBeamGeometryInitialized != null)
			{
				this.m_OnBeamGeometryInitialized();
				this.m_OnBeamGeometryInitialized = null;
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00009328 File Offset: 0x00007528
		private void SetFadeOutValue(ref float propToChange, float value)
		{
			bool isFadeOutEnabled = this.isFadeOutEnabled;
			propToChange = value;
			if (this.isFadeOutEnabled != isFadeOutEnabled)
			{
				this.OnFadeOutStateChanged();
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000934E File Offset: 0x0000754E
		private void OnFadeOutStateChanged()
		{
			if (this.isFadeOutEnabled && this.m_BeamGeom)
			{
				this.m_BeamGeom.RestartFadeOutCoroutine();
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00009370 File Offset: 0x00007570
		// (set) Token: 0x06000246 RID: 582 RVA: 0x00009378 File Offset: 0x00007578
		public uint _INTERNAL_InstancedMaterialGroupID { get; protected set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00009384 File Offset: 0x00007584
		public string meshStats
		{
			get
			{
				Mesh mesh = this.m_BeamGeom ? this.m_BeamGeom.coneMesh : null;
				if (mesh)
				{
					return string.Format("Cone angle: {0:0.0} degrees\nMesh: {1} vertices, {2} triangles", this.coneAngle, mesh.vertexCount, mesh.triangles.Length / 3);
				}
				return "no mesh available";
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000248 RID: 584 RVA: 0x000093EA File Offset: 0x000075EA
		public int meshVerticesCount
		{
			get
			{
				if (!this.m_BeamGeom || !this.m_BeamGeom.coneMesh)
				{
					return 0;
				}
				return this.m_BeamGeom.coneMesh.vertexCount;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000941D File Offset: 0x0000761D
		public int meshTrianglesCount
		{
			get
			{
				if (!this.m_BeamGeom || !this.m_BeamGeom.coneMesh)
				{
					return 0;
				}
				return this.m_BeamGeom.coneMesh.triangles.Length / 3;
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00009454 File Offset: 0x00007654
		public float GetInsideBeamFactor(Vector3 posWS)
		{
			return this.GetInsideBeamFactorFromObjectSpacePos(base.transform.InverseTransformPoint(posWS));
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00009468 File Offset: 0x00007668
		public float GetInsideBeamFactorFromObjectSpacePos(Vector3 posOS)
		{
			if (this.dimensions == Dimensions.Dim2D)
			{
				posOS = new Vector3(posOS.z, posOS.y, posOS.x);
			}
			if (posOS.z < 0f)
			{
				return -1f;
			}
			Vector2 a = posOS.xy();
			if (this.hasMeshSkewing)
			{
				Vector3 skewingLocalForwardDirectionNormalized = this.skewingLocalForwardDirectionNormalized;
				a -= skewingLocalForwardDirectionNormalized.xy() * (posOS.z / skewingLocalForwardDirectionNormalized.z);
			}
			Vector2 normalized = new Vector2(a.magnitude, posOS.z + this.coneApexOffsetZ).normalized;
			return Mathf.Clamp((Mathf.Abs(Mathf.Sin(this.coneAngle * 0.017453292f / 2f)) - Mathf.Abs(normalized.x)) / 0.1f, -1f, 1f);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000953E File Offset: 0x0000773E
		[Obsolete("Use 'GenerateGeometry()' instead")]
		public void Generate()
		{
			this.GenerateGeometry();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00009548 File Offset: 0x00007748
		public override void GenerateGeometry()
		{
			this.HandleBackwardCompatibility(this.pluginVersion, 20200);
			this.pluginVersion = 20200;
			this.ValidateProperties();
			if (this.m_BeamGeom == null)
			{
				this.m_BeamGeom = Utils.NewWithComponent<BeamGeometrySD>("Beam Geometry");
				this.m_BeamGeom.Initialize(this);
				this.CallOnBeamGeometryInitializedCallback();
			}
			this.m_BeamGeom.RegenerateMesh(base.enabled);
			base.GenerateGeometry();
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000095BE File Offset: 0x000077BE
		public virtual void UpdateAfterManualPropertyChange()
		{
			this.ValidateProperties();
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.UpdateMaterialAndBounds();
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000095DE File Offset: 0x000077DE
		private void Start()
		{
			base.InitLightSpotAttachedCached();
			this.GenerateGeometry();
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000095EC File Offset: 0x000077EC
		private void OnEnable()
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.OnMasterEnable();
			}
			this.StartPlaytimeUpdateIfNeeded();
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000960C File Offset: 0x0000780C
		private void OnDisable()
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.OnMasterDisable();
			}
			this.m_CoPlaytimeUpdate = null;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000962D File Offset: 0x0000782D
		private void StartPlaytimeUpdateIfNeeded()
		{
			if (Application.isPlaying && this.trackChangesDuringPlaytime && this.m_CoPlaytimeUpdate == null)
			{
				this.m_CoPlaytimeUpdate = base.StartCoroutine(this.CoPlaytimeUpdate());
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00009658 File Offset: 0x00007858
		private IEnumerator CoPlaytimeUpdate()
		{
			while (this.trackChangesDuringPlaytime && base.enabled)
			{
				this.UpdateAfterManualPropertyChange();
				yield return null;
			}
			this.m_CoPlaytimeUpdate = null;
			yield break;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00009668 File Offset: 0x00007868
		private void AssignPropertiesFromAttachedSpotLight()
		{
			Light lightSpotAttached = base.lightSpotAttached;
			if (lightSpotAttached)
			{
				if (this.intensityFromLight)
				{
					this.intensityModeAdvanced = false;
					this.intensityGlobal = SpotLightHelper.GetIntensity(lightSpotAttached) * this.intensityMultiplier;
				}
				if (this.fallOffEndFromLight)
				{
					this.fallOffEnd = SpotLightHelper.GetFallOffEnd(lightSpotAttached) * this.fallOffEndMultiplier;
				}
				if (this.spotAngleFromLight)
				{
					this.spotAngle = Mathf.Clamp(SpotLightHelper.GetSpotAngle(lightSpotAttached) * this.spotAngleMultiplier, 0.1f, 179.9f);
				}
				if (this.colorFromLight)
				{
					this.colorMode = ColorMode.Flat;
					if (this.useColorTemperatureFromAttachedLightSpot)
					{
						Color b = Mathf.CorrelatedColorTemperatureToRGB(lightSpotAttached.colorTemperature);
						this.color = (lightSpotAttached.color.linear * b).gamma;
						return;
					}
					this.color = lightSpotAttached.color;
				}
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00009740 File Offset: 0x00007940
		private void ClampProperties()
		{
			this.intensityInside = Mathf.Max(this.intensityInside, 0f);
			this.intensityOutside = Mathf.Max(this.intensityOutside, 0f);
			this.intensityMultiplier = Mathf.Max(this.intensityMultiplier, 0f);
			this.attenuationCustomBlending = Mathf.Clamp(this.attenuationCustomBlending, 0f, 1f);
			this.fallOffEnd = Mathf.Max(0.01f, this.fallOffEnd);
			this.fallOffStart = Mathf.Clamp(this.fallOffStart, 0f, this.fallOffEnd - 0.01f);
			this.fallOffEndMultiplier = Mathf.Max(this.fallOffEndMultiplier, 0f);
			this.spotAngle = Mathf.Clamp(this.spotAngle, 0.1f, 179.9f);
			this.spotAngleMultiplier = Mathf.Max(this.spotAngleMultiplier, 0f);
			this.coneRadiusStart = Mathf.Max(this.coneRadiusStart, 0f);
			this.depthBlendDistance = Mathf.Max(this.depthBlendDistance, 0f);
			this.cameraClippingDistance = Mathf.Max(this.cameraClippingDistance, 0f);
			this.geomCustomSides = Mathf.Clamp(this.geomCustomSides, 3, 256);
			this.geomCustomSegments = Mathf.Clamp(this.geomCustomSegments, 0, 64);
			this.fresnelPow = Mathf.Max(0f, this.fresnelPow);
			this.glareBehind = Mathf.Clamp(this.glareBehind, 0f, 1f);
			this.glareFrontal = Mathf.Clamp(this.glareFrontal, 0f, 1f);
			this.noiseIntensity = Mathf.Clamp(this.noiseIntensity, 0f, 1f);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x000098FD File Offset: 0x00007AFD
		private void ValidateProperties()
		{
			this.AssignPropertiesFromAttachedSpotLight();
			this.ClampProperties();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000990C File Offset: 0x00007B0C
		private void HandleBackwardCompatibility(int serializedVersion, int newVersion)
		{
			if (serializedVersion == -1)
			{
				return;
			}
			if (serializedVersion == newVersion)
			{
				return;
			}
			if (serializedVersion < 1301)
			{
				this.attenuationEquation = AttenuationEquation.Linear;
			}
			if (serializedVersion < 1501)
			{
				this.geomMeshType = MeshType.Custom;
				this.geomCustomSegments = 5;
			}
			if (serializedVersion < 1610)
			{
				this.intensityFromLight = false;
				this.intensityModeAdvanced = !Mathf.Approximately(this.intensityInside, this.intensityOutside);
			}
			if (serializedVersion < 1910 && !this.intensityModeAdvanced && !Mathf.Approximately(this.intensityInside, this.intensityOutside))
			{
				this.intensityInside = this.intensityOutside;
			}
			Utils.MarkCurrentSceneDirty();
		}

		// Token: 0x04000145 RID: 325
		public new const string ClassName = "VolumetricLightBeamSD";

		// Token: 0x04000146 RID: 326
		public bool colorFromLight = true;

		// Token: 0x04000147 RID: 327
		public ColorMode colorMode;

		// Token: 0x04000148 RID: 328
		[ColorUsage(false, true)]
		[FormerlySerializedAs("colorValue")]
		public Color color = Consts.Beam.FlatColor;

		// Token: 0x04000149 RID: 329
		public Gradient colorGradient;

		// Token: 0x0400014A RID: 330
		public bool intensityFromLight = true;

		// Token: 0x0400014B RID: 331
		public bool intensityModeAdvanced;

		// Token: 0x0400014C RID: 332
		[FormerlySerializedAs("alphaInside")]
		[Min(0f)]
		public float intensityInside = 1f;

		// Token: 0x0400014D RID: 333
		[FormerlySerializedAs("alphaOutside")]
		[FormerlySerializedAs("alpha")]
		[Min(0f)]
		public float intensityOutside = 1f;

		// Token: 0x0400014E RID: 334
		[Min(0f)]
		public float intensityMultiplier = 1f;

		// Token: 0x0400014F RID: 335
		[Range(0f, 1f)]
		public float hdrpExposureWeight;

		// Token: 0x04000150 RID: 336
		public BlendingMode blendingMode;

		// Token: 0x04000151 RID: 337
		[FormerlySerializedAs("angleFromLight")]
		public bool spotAngleFromLight = true;

		// Token: 0x04000152 RID: 338
		[Range(0.1f, 179.9f)]
		public float spotAngle = 35f;

		// Token: 0x04000153 RID: 339
		[Min(0f)]
		public float spotAngleMultiplier = 1f;

		// Token: 0x04000154 RID: 340
		[FormerlySerializedAs("radiusStart")]
		public float coneRadiusStart = 0.1f;

		// Token: 0x04000155 RID: 341
		public ShaderAccuracy shaderAccuracy;

		// Token: 0x04000156 RID: 342
		public MeshType geomMeshType;

		// Token: 0x04000157 RID: 343
		[FormerlySerializedAs("geomSides")]
		public int geomCustomSides = 18;

		// Token: 0x04000158 RID: 344
		public int geomCustomSegments = 5;

		// Token: 0x04000159 RID: 345
		public Vector3 skewingLocalForwardDirection = Consts.Beam.SD.SkewingLocalForwardDirectionDefault;

		// Token: 0x0400015A RID: 346
		public Transform clippingPlaneTransform;

		// Token: 0x0400015B RID: 347
		public bool geomCap;

		// Token: 0x0400015C RID: 348
		public AttenuationEquation attenuationEquation = AttenuationEquation.Quadratic;

		// Token: 0x0400015D RID: 349
		[Range(0f, 1f)]
		public float attenuationCustomBlending = 0.5f;

		// Token: 0x0400015E RID: 350
		[FormerlySerializedAs("fadeStart")]
		public float fallOffStart;

		// Token: 0x0400015F RID: 351
		[FormerlySerializedAs("fadeEnd")]
		public float fallOffEnd = 3f;

		// Token: 0x04000160 RID: 352
		[FormerlySerializedAs("fadeEndFromLight")]
		public bool fallOffEndFromLight = true;

		// Token: 0x04000161 RID: 353
		[Min(0f)]
		public float fallOffEndMultiplier = 1f;

		// Token: 0x04000162 RID: 354
		public float depthBlendDistance = 2f;

		// Token: 0x04000163 RID: 355
		public float cameraClippingDistance = 0.5f;

		// Token: 0x04000164 RID: 356
		[Range(0f, 1f)]
		public float glareFrontal = 0.5f;

		// Token: 0x04000165 RID: 357
		[Range(0f, 1f)]
		public float glareBehind = 0.5f;

		// Token: 0x04000166 RID: 358
		[FormerlySerializedAs("fresnelPowOutside")]
		public float fresnelPow = 8f;

		// Token: 0x04000167 RID: 359
		public NoiseMode noiseMode;

		// Token: 0x04000168 RID: 360
		[Range(0f, 1f)]
		public float noiseIntensity = 0.5f;

		// Token: 0x04000169 RID: 361
		public bool noiseScaleUseGlobal = true;

		// Token: 0x0400016A RID: 362
		[Range(0.01f, 2f)]
		public float noiseScaleLocal = 0.5f;

		// Token: 0x0400016B RID: 363
		public bool noiseVelocityUseGlobal = true;

		// Token: 0x0400016C RID: 364
		public Vector3 noiseVelocityLocal = Consts.Beam.NoiseVelocityDefault;

		// Token: 0x0400016D RID: 365
		public Dimensions dimensions;

		// Token: 0x0400016E RID: 366
		public Vector2 tiltFactor = Consts.Beam.SD.TiltDefault;

		// Token: 0x0400016F RID: 367
		private MaterialManager.SD.DynamicOcclusion m_INTERNAL_DynamicOcclusionMode;

		// Token: 0x04000170 RID: 368
		private bool m_INTERNAL_DynamicOcclusionMode_Runtime;

		// Token: 0x04000172 RID: 370
		private VolumetricLightBeamSD.OnBeamGeometryInitialized m_OnBeamGeometryInitialized;

		// Token: 0x04000173 RID: 371
		[FormerlySerializedAs("trackChangesDuringPlaytime")]
		[SerializeField]
		private bool _TrackChangesDuringPlaytime;

		// Token: 0x04000174 RID: 372
		[SerializeField]
		private int _SortingLayerID;

		// Token: 0x04000175 RID: 373
		[SerializeField]
		private int _SortingOrder;

		// Token: 0x04000176 RID: 374
		[FormerlySerializedAs("fadeOutBegin")]
		[SerializeField]
		private float _FadeOutBegin = -150f;

		// Token: 0x04000177 RID: 375
		[FormerlySerializedAs("fadeOutEnd")]
		[SerializeField]
		private float _FadeOutEnd = -200f;

		// Token: 0x04000179 RID: 377
		private BeamGeometrySD m_BeamGeom;

		// Token: 0x0400017A RID: 378
		private Coroutine m_CoPlaytimeUpdate;

		// Token: 0x020000BE RID: 190
		// (Invoke) Token: 0x060004E6 RID: 1254
		public delegate void OnWillCameraRenderCB(Camera cam);

		// Token: 0x020000BF RID: 191
		// (Invoke) Token: 0x060004EA RID: 1258
		public delegate void OnBeamGeometryInitialized();
	}
}
