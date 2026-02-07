using System;
using LeTai.TrueShadow.PluginInterfaces;
using UnityEngine;

namespace LeTai.TrueShadow
{
	// Token: 0x02000008 RID: 8
	[ExecuteAlways]
	[RequireComponent(typeof(TrueShadow))]
	public class DisableShadowCache : MonoBehaviour, ITrueShadowCustomHashProvider
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002946 File Offset: 0x00000B46
		private void OnEnable()
		{
			this.shadow = base.GetComponent<TrueShadow>();
			this.shadow.CustomHash = this.shadow.GetInstanceID();
			this.shadow.SetTextureDirty();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002975 File Offset: 0x00000B75
		private void OnDisable()
		{
			this.shadow.CustomHash = 0;
			this.shadow.SetTextureDirty();
		}

		// Token: 0x04000017 RID: 23
		private TrueShadow shadow;
	}
}
