using System;
using System.Collections.Generic;
using UnityEngine;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x02000014 RID: 20
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[HelpURL("https://github.com/SymmetryBreakStudio/TastyGrassShader/wiki/Quick-Start")]
	[RequireComponent(typeof(Terrain))]
	[AddComponentMenu("Symmetry Break Studio/Tasty Grass Shader/Tasty Grass Shader For Terrain")]
	public class TgsForUnityTerrain : MonoBehaviour
	{
		// Token: 0x06000031 RID: 49 RVA: 0x0000294E File Offset: 0x00000B4E
		private void Update()
		{
			if (!Application.isPlaying)
			{
				this.MarkMaterialDirty();
				this.OnPropertiesMayChanged();
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002963 File Offset: 0x00000B63
		private void OnEnable()
		{
			TerrainCallbacks.heightmapChanged += this.TerrainCallbacksOnheightmapChanged;
			TerrainCallbacks.textureChanged += this.TerrainCallbacksOntextureChanged;
			this.InstancesUpdateGrass();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002990 File Offset: 0x00000B90
		private void OnDisable()
		{
			TerrainCallbacks.heightmapChanged -= this.TerrainCallbacksOnheightmapChanged;
			TerrainCallbacks.textureChanged -= this.TerrainCallbacksOntextureChanged;
			foreach (TgsTerrainLayer tgsTerrainLayer in this.layers)
			{
				tgsTerrainLayer.Release();
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002A04 File Offset: 0x00000C04
		private void OnDrawGizmosSelected()
		{
			if (this.showChunksBounds)
			{
				foreach (TgsTerrainLayer tgsTerrainLayer in this.layers)
				{
					tgsTerrainLayer.DrawGizmos();
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002A5C File Offset: 0x00000C5C
		public void OnPropertiesMayChanged()
		{
			foreach (TgsTerrainLayer tgsTerrainLayer in this.layers)
			{
				tgsTerrainLayer.CheckForSettingsChange();
			}
			this.InstancesUpdateGrass();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public void MarkGeometryDirty()
		{
			foreach (TgsTerrainLayer tgsTerrainLayer in this.layers)
			{
				tgsTerrainLayer.MarkGeometryDirty();
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002B04 File Offset: 0x00000D04
		public void MarkMaterialDirty()
		{
			foreach (TgsTerrainLayer tgsTerrainLayer in this.layers)
			{
				tgsTerrainLayer.MarkMaterialDirty();
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002B54 File Offset: 0x00000D54
		public int GetChunkCount()
		{
			int num = 0;
			foreach (TgsTerrainLayer tgsTerrainLayer in this.layers)
			{
				num += tgsTerrainLayer.GetChunkCount();
			}
			return num;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002BAC File Offset: 0x00000DAC
		public int GetGrassMemoryBufferByteSize()
		{
			int num = 0;
			foreach (TgsTerrainLayer tgsTerrainLayer in this.layers)
			{
				num += tgsTerrainLayer.GetGrassMemoryBufferByteSize();
			}
			return num;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002C04 File Offset: 0x00000E04
		public int GetPlacementMemoryBufferByteSize()
		{
			int num = 0;
			foreach (TgsTerrainLayer tgsTerrainLayer in this.layers)
			{
				num += tgsTerrainLayer.GetPlacementMemoryBufferByteSize();
			}
			return num;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002C5C File Offset: 0x00000E5C
		public TgsTerrainLayer GetLayerByIndex(int index)
		{
			return this.layers[index];
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002C6A File Offset: 0x00000E6A
		public int GetLayerCount()
		{
			return this.layers.Count;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002C78 File Offset: 0x00000E78
		public int AddNewLayer()
		{
			TgsTerrainLayer tgsTerrainLayer = new TgsTerrainLayer();
			this.layers.Add(tgsTerrainLayer);
			return this.layers.Count - 1;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002CA4 File Offset: 0x00000EA4
		public void RemoveLayerAt(int index)
		{
			this.layers[index].Release();
			this.layers.RemoveAt(index);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002CC4 File Offset: 0x00000EC4
		private void TerrainCallbacksOntextureChanged(Terrain terrain, string texturename, RectInt texelregion, bool synched)
		{
			if (terrain != base.GetComponent<Terrain>())
			{
				return;
			}
			if (!synched)
			{
				return;
			}
			float num = (float)terrain.terrainData.heightmapResolution / (float)terrain.terrainData.alphamapResolution;
			texelregion = new RectInt((int)((float)texelregion.min.x * num), (int)((float)texelregion.min.y * num), (int)((float)texelregion.max.x * num), (int)((float)texelregion.max.y * num));
			Debug.Log("TerrainCallbacksOntextureChanged");
			this._changedTerrainRegions.Add(texelregion);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002D68 File Offset: 0x00000F68
		private void TerrainCallbacksOnheightmapChanged(Terrain terrain, RectInt heightregion, bool synched)
		{
			if (terrain != base.GetComponent<Terrain>())
			{
				return;
			}
			if (!synched)
			{
				return;
			}
			Debug.Log("TerrainCallbacksOnheightmapChanged");
			this._changedTerrainRegions.Add(heightregion);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002D94 File Offset: 0x00000F94
		public void InstancesUpdateGrass()
		{
			Transform transform = base.transform;
			Terrain component = base.GetComponent<Terrain>();
			TerrainData terrainData = component.terrainData;
			Texture2D[] alphamapTextures = terrainData.alphamapTextures;
			TerrainLayer[] terrainLayers = terrainData.terrainLayers;
			int layer = base.gameObject.layer;
			foreach (TgsTerrainLayer tgsTerrainLayer in this.layers)
			{
				tgsTerrainLayer.CheckForChange(transform, component, alphamapTextures, terrainLayers, this._changedTerrainRegions, layer);
				tgsTerrainLayer.SetWindSettings(this.windSettings);
			}
			this._changedTerrainRegions.Clear();
		}

		// Token: 0x0400003D RID: 61
		[SerializeField]
		private List<TgsTerrainLayer> layers = new List<TgsTerrainLayer>();

		// Token: 0x0400003E RID: 62
		[Tooltip("Wind setting used for this object.")]
		public TgsWindSettings windSettings;

		// Token: 0x0400003F RID: 63
		[Header("Debugging")]
		[Tooltip("Shows the bounds of all terrain chunks in editor.")]
		public bool showChunksBounds;

		// Token: 0x04000040 RID: 64
		private readonly List<RectInt> _changedTerrainRegions = new List<RectInt>();

		// Token: 0x04000041 RID: 65
		private bool _passedFirstFrame;
	}
}
