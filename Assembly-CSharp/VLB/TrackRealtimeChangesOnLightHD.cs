using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000028 RID: 40
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Light), typeof(VolumetricLightBeamHD))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-trackrealtimechanges-hd/")]
	[AddComponentMenu("VLB/HD/Track Realtime Changes On Light")]
	public class TrackRealtimeChangesOnLightHD : MonoBehaviour
	{
		// Token: 0x06000096 RID: 150 RVA: 0x000046B5 File Offset: 0x000028B5
		private void Awake()
		{
			this.m_Master = base.GetComponent<VolumetricLightBeamHD>();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000046C3 File Offset: 0x000028C3
		private void Update()
		{
			if (this.m_Master.enabled)
			{
				this.m_Master.AssignPropertiesFromAttachedSpotLight();
			}
		}

		// Token: 0x040000D0 RID: 208
		public const string ClassName = "TrackRealtimeChangesOnLightHD";

		// Token: 0x040000D1 RID: 209
		private VolumetricLightBeamHD m_Master;
	}
}
