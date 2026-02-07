using System;
using LeTai.TrueShadow.PluginInterfaces;
using UnityEngine;

namespace LeTai.TrueShadow
{
	// Token: 0x0200000B RID: 11
	[ExecuteAlways]
	public class ShadowMaterial : MonoBehaviour, ITrueShadowRendererMaterialProvider
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600004A RID: 74 RVA: 0x00003064 File Offset: 0x00001264
		// (remove) Token: 0x0600004B RID: 75 RVA: 0x0000309C File Offset: 0x0000129C
		public event Action materialReplaced;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600004C RID: 76 RVA: 0x000030D4 File Offset: 0x000012D4
		// (remove) Token: 0x0600004D RID: 77 RVA: 0x0000310C File Offset: 0x0000130C
		public event Action materialModified;

		// Token: 0x0600004E RID: 78 RVA: 0x00003141 File Offset: 0x00001341
		public Material GetTrueShadowRendererMaterial()
		{
			if (!base.isActiveAndEnabled)
			{
				return null;
			}
			return this.material;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003154 File Offset: 0x00001354
		private void OnEnable()
		{
			TrueShadow component = base.GetComponent<TrueShadow>();
			if (component)
			{
				component.RefreshPlugins();
			}
			Action action = this.materialReplaced;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003188 File Offset: 0x00001388
		private void OnDisable()
		{
			TrueShadow component = base.GetComponent<TrueShadow>();
			if (component)
			{
				component.RefreshPlugins();
			}
			Action action = this.materialReplaced;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000031BA File Offset: 0x000013BA
		private void OnValidate()
		{
			Action action = this.materialReplaced;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000031CC File Offset: 0x000013CC
		public void OnMaterialModified()
		{
			Action action = this.materialModified;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0400003A RID: 58
		public Material material;
	}
}
