using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x02000010 RID: 16
	[ExecuteAlways]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[HelpURL("https://github.com/SymmetryBreakStudio/TastyGrassShader/wiki/Quick-Start")]
	[AddComponentMenu("Symmetry Break Studio/Tasty Grass Shader/Tasty Grass Shader For Mesh")]
	public class TgsForMesh : MonoBehaviour
	{
		// Token: 0x0600001A RID: 26 RVA: 0x000022E8 File Offset: 0x000004E8
		private void Update()
		{
			if (!Application.isPlaying || this.UpdateOnNextTick)
			{
				this.MarkMaterialDirty();
				this.MarkGeometryDirty();
				if (!Application.isPlaying)
				{
					this.sharedMeshReference = base.GetComponent<MeshFilter>().sharedMesh;
				}
				this.OnPropertiesMayChanged();
				this.UpdateOnNextTick = false;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002335 File Offset: 0x00000535
		private void OnEnable()
		{
			this.MarkGeometryDirty();
			this.OnPropertiesMayChanged();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002344 File Offset: 0x00000544
		private void OnDisable()
		{
			foreach (TgsMeshLayer tgsMeshLayer in this.layers)
			{
				tgsMeshLayer.Release();
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002394 File Offset: 0x00000594
		private void OnDrawGizmosSelected()
		{
			this._isSelected = true;
			foreach (TgsMeshLayer tgsMeshLayer in this.layers)
			{
				if (tgsMeshLayer.Instance != null)
				{
					Gizmos.color = Color.red;
					Gizmos.DrawWireCube(tgsMeshLayer.Instance.tightBounds.center, tgsMeshLayer.Instance.tightBounds.size);
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002424 File Offset: 0x00000624
		private void OnTransformParentChanged()
		{
			this.MarkGeometryDirty();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000242C File Offset: 0x0000062C
		private void OnValidate()
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			this.sharedMeshReference = component.sharedMesh;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000244C File Offset: 0x0000064C
		public TgsMeshLayer GetLayerByIndex(int index)
		{
			if (index >= this.layers.Count)
			{
				return null;
			}
			return this.layers[index];
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000246A File Offset: 0x0000066A
		public int GetLayerCount()
		{
			return this.layers.Count;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002478 File Offset: 0x00000678
		public int AddNewLayer()
		{
			TgsMeshLayer tgsMeshLayer = new TgsMeshLayer();
			this.layers.Add(tgsMeshLayer);
			return this.layers.Count - 1;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000024A4 File Offset: 0x000006A4
		public void RemoveLayerAt(int index)
		{
			this.layers[index].Release();
			this.layers.RemoveAt(index);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000024C4 File Offset: 0x000006C4
		public static TgsForMesh.GrassMeshError CheckForErrorsMeshFilter(TgsForMesh tgsForMesh, MeshFilter meshFilter)
		{
			if (!meshFilter)
			{
				return TgsForMesh.GrassMeshError.MissingMeshFilter;
			}
			if (!meshFilter.sharedMesh)
			{
				return TgsForMesh.GrassMeshError.MissingMesh;
			}
			if (!meshFilter.sharedMesh.isReadable)
			{
				return TgsForMesh.GrassMeshError.MeshNoReadWrite;
			}
			bool flag = false;
			using (List<TgsMeshLayer>.Enumerator enumerator = tgsForMesh.layers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.distribution != TgsMeshLayer.DensityColorChannelMask.Fill)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag && !meshFilter.sharedMesh.HasVertexAttribute(VertexAttribute.Color))
			{
				return TgsForMesh.GrassMeshError.MissingVertexColor;
			}
			return TgsForMesh.GrassMeshError.None;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002558 File Offset: 0x00000758
		public TgsForMesh.GrassMeshError CheckForErrors()
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			return TgsForMesh.CheckForErrorsMeshFilter(this, component);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002574 File Offset: 0x00000774
		public void MarkGeometryDirty()
		{
			foreach (TgsMeshLayer tgsMeshLayer in this.layers)
			{
				tgsMeshLayer.MarkGeometryDirty();
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000025C4 File Offset: 0x000007C4
		public void MarkMaterialDirty()
		{
			foreach (TgsMeshLayer tgsMeshLayer in this.layers)
			{
				tgsMeshLayer.MarkMaterialDirty();
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002614 File Offset: 0x00000814
		public void OnPropertiesMayChanged()
		{
			MeshRenderer component = base.GetComponent<MeshRenderer>();
			Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
			if (localToWorldMatrix != this._previousLocalToWorld)
			{
				this.MarkGeometryDirty();
			}
			int layer = base.gameObject.layer;
			this._previousLocalToWorld = localToWorldMatrix;
			foreach (TgsMeshLayer tgsMeshLayer in this.layers)
			{
				if (this._isSelected)
				{
					tgsMeshLayer.MarkGeometryDirty();
				}
				tgsMeshLayer.CheckForChange(localToWorldMatrix, this.sharedMeshReference, component.bounds, layer);
				tgsMeshLayer.Instance.UsedWindSettings = this.windSettings;
			}
			this._isSelected = false;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000026D8 File Offset: 0x000008D8
		public int GetMemoryBufferByteSize()
		{
			int num = 0;
			foreach (TgsMeshLayer tgsMeshLayer in this.layers)
			{
				if (tgsMeshLayer.Instance != null)
				{
					num += tgsMeshLayer.Instance.GetGrassBufferMemoryByteSize();
				}
			}
			return num;
		}

		// Token: 0x04000027 RID: 39
		[SerializeField]
		private List<TgsMeshLayer> layers = new List<TgsMeshLayer>();

		// Token: 0x04000028 RID: 40
		[Tooltip("Wind setting used for this object.")]
		public TgsWindSettings windSettings;

		// Token: 0x04000029 RID: 41
		public Mesh sharedMeshReference;

		// Token: 0x0400002A RID: 42
		private bool _isSelected;

		// Token: 0x0400002B RID: 43
		private Matrix4x4 _previousLocalToWorld;

		// Token: 0x0400002C RID: 44
		[HideInInspector]
		[NonSerialized]
		public bool UpdateOnNextTick;

		// Token: 0x02000011 RID: 17
		public enum GrassMeshError
		{
			// Token: 0x0400002E RID: 46
			None,
			// Token: 0x0400002F RID: 47
			MissingMeshFilter,
			// Token: 0x04000030 RID: 48
			MeshNoReadWrite,
			// Token: 0x04000031 RID: 49
			MissingVertexColor,
			// Token: 0x04000032 RID: 50
			MissingMesh
		}
	}
}
