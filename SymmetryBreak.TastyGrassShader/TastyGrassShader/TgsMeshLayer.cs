using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x02000012 RID: 18
	[Serializable]
	public class TgsMeshLayer
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00002753 File Offset: 0x00000953
		internal TgsMeshLayer()
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002768 File Offset: 0x00000968
		public void CheckForChange(Matrix4x4 localToWorldMatrix, Mesh mesh, Bounds worldSpaceBounds, int unityLayer)
		{
			if (this.Instance == null)
			{
				this.Instance = new TgsInstance();
				this.Instance.MarkGeometryDirty();
				this.Instance.MarkMaterialDirty();
			}
			this.Instance.Hide = this.hide;
			this.Instance.UnityLayer = unityLayer;
			default(Hash128).Append<TgsMeshLayer.DensityColorChannelMask>(ref this.distribution);
			if (this.Instance.isGeometryDirty || this.settings.HasChanged())
			{
				TgsInstance.TgsInstanceRecipe bakeParameters = TgsInstance.TgsInstanceRecipe.BakeFromMesh(localToWorldMatrix, this.settings, mesh, worldSpaceBounds);
				if (this.distribution != TgsMeshLayer.DensityColorChannelMask.Fill)
				{
					Vector4 v = new Vector4((this.distribution == TgsMeshLayer.DensityColorChannelMask.Red) ? 1f : 0f, (this.distribution == TgsMeshLayer.DensityColorChannelMask.Green) ? 1f : 0f, (this.distribution == TgsMeshLayer.DensityColorChannelMask.Blue) ? 1f : 0f, (this.distribution == TgsMeshLayer.DensityColorChannelMask.Alpha) ? 1f : 0f);
					bakeParameters.SetupDistributionByVertexColor(v);
				}
				this.Instance.SetBakeParameters(bakeParameters);
				this.Instance.MarkGeometryDirty();
				this.Instance.MarkMaterialDirty();
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000288F File Offset: 0x00000A8F
		public void MarkGeometryDirty()
		{
			TgsInstance instance = this.Instance;
			if (instance == null)
			{
				return;
			}
			instance.MarkGeometryDirty();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000028A1 File Offset: 0x00000AA1
		public void MarkMaterialDirty()
		{
			TgsInstance instance = this.Instance;
			if (instance == null)
			{
				return;
			}
			instance.MarkMaterialDirty();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000028B3 File Offset: 0x00000AB3
		public void Release()
		{
			TgsInstance instance = this.Instance;
			if (instance != null)
			{
				instance.Release();
			}
			this.Instance = null;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000028D0 File Offset: 0x00000AD0
		public string GetEditorName(int index)
		{
			return string.Format("#{0} - {1} ({2}) {3}", new object[]
			{
				index,
				(this.settings.preset != null) ? this.settings.preset.name : "NO PRESET DEFINED",
				this.distribution.ToString(),
				this.hide ? "(Hidden)" : ""
			});
		}

		// Token: 0x04000033 RID: 51
		[HideInInspector]
		public bool hide;

		// Token: 0x04000034 RID: 52
		[FormerlySerializedAs("quickSettings")]
		public TgsPreset.Settings settings = TgsPreset.Settings.GetDefault();

		// Token: 0x04000035 RID: 53
		public TgsMeshLayer.DensityColorChannelMask distribution;

		// Token: 0x04000036 RID: 54
		internal TgsInstance Instance;

		// Token: 0x02000013 RID: 19
		public enum DensityColorChannelMask
		{
			// Token: 0x04000038 RID: 56
			Fill,
			// Token: 0x04000039 RID: 57
			Red,
			// Token: 0x0400003A RID: 58
			Green,
			// Token: 0x0400003B RID: 59
			Blue,
			// Token: 0x0400003C RID: 60
			Alpha
		}
	}
}
