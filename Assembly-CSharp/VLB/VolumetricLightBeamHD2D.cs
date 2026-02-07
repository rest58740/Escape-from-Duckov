using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200002B RID: 43
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[SelectionBase]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-hd/")]
	[AddComponentMenu("VLB/HD/Volumetric Light Beam HD (2D)")]
	public class VolumetricLightBeamHD2D : VolumetricLightBeamHD
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000055B0 File Offset: 0x000037B0
		// (set) Token: 0x06000116 RID: 278 RVA: 0x000055B8 File Offset: 0x000037B8
		public int sortingLayerID
		{
			get
			{
				return this.m_SortingLayerID;
			}
			set
			{
				this.m_SortingLayerID = value;
				if (this.m_BeamGeom)
				{
					this.m_BeamGeom.sortingLayerID = value;
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000117 RID: 279 RVA: 0x000055DA File Offset: 0x000037DA
		// (set) Token: 0x06000118 RID: 280 RVA: 0x000055E7 File Offset: 0x000037E7
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

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000119 RID: 281 RVA: 0x000055F5 File Offset: 0x000037F5
		// (set) Token: 0x0600011A RID: 282 RVA: 0x000055FD File Offset: 0x000037FD
		public int sortingOrder
		{
			get
			{
				return this.m_SortingOrder;
			}
			set
			{
				this.m_SortingOrder = value;
				if (this.m_BeamGeom)
				{
					this.m_BeamGeom.sortingOrder = value;
				}
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000561F File Offset: 0x0000381F
		public override Dimensions GetDimensions()
		{
			return Dimensions.Dim2D;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005622 File Offset: 0x00003822
		public override bool DoesSupportSorting2D()
		{
			return true;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005625 File Offset: 0x00003825
		public override int GetSortingLayerID()
		{
			return this.sortingLayerID;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000562D File Offset: 0x0000382D
		public override int GetSortingOrder()
		{
			return this.sortingOrder;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005638 File Offset: 0x00003838
		public override void CopyPropsFrom(VolumetricLightBeamAbstractBase beamSrc, BeamProps beamProps)
		{
			base.CopyPropsFrom(beamSrc, beamProps);
			if (beamSrc is VolumetricLightBeamSD)
			{
				VolumetricLightBeamSD volumetricLightBeamSD = beamSrc as VolumetricLightBeamSD;
				if (beamProps.HasFlag(BeamProps.Props2D))
				{
					this.sortingLayerID = volumetricLightBeamSD.sortingLayerID;
					this.sortingOrder = volumetricLightBeamSD.sortingOrder;
					return;
				}
			}
			else if (beamSrc is VolumetricLightBeamHD2D)
			{
				VolumetricLightBeamHD2D volumetricLightBeamHD2D = beamSrc as VolumetricLightBeamHD2D;
				if (beamProps.HasFlag(BeamProps.Props2D))
				{
					this.sortingLayerID = volumetricLightBeamHD2D.sortingLayerID;
					this.sortingOrder = volumetricLightBeamHD2D.sortingOrder;
				}
			}
		}

		// Token: 0x040000F9 RID: 249
		[SerializeField]
		private int m_SortingLayerID;

		// Token: 0x040000FA RID: 250
		[SerializeField]
		private int m_SortingOrder;
	}
}
