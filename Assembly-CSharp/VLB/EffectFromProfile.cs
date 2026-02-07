using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200000F RID: 15
	[HelpURL("http://saladgamer.com/vlb-doc/comp-effect-from-profile/")]
	[AddComponentMenu("VLB/Common/Effect From Profile")]
	public class EffectFromProfile : MonoBehaviour
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000038C2 File Offset: 0x00001AC2
		// (set) Token: 0x06000064 RID: 100 RVA: 0x000038CA File Offset: 0x00001ACA
		public EffectAbstractBase effectProfile
		{
			get
			{
				return this.m_EffectProfile;
			}
			set
			{
				this.m_EffectProfile = value;
				this.InitInstanceFromProfile();
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000038D9 File Offset: 0x00001AD9
		public void InitInstanceFromProfile()
		{
			if (this.m_EffectInstance)
			{
				if (this.m_EffectProfile)
				{
					this.m_EffectInstance.InitFrom(this.m_EffectProfile);
					return;
				}
				this.m_EffectInstance.enabled = false;
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003914 File Offset: 0x00001B14
		private void OnEnable()
		{
			if (this.m_EffectInstance)
			{
				this.m_EffectInstance.enabled = true;
				return;
			}
			if (this.m_EffectProfile)
			{
				this.m_EffectInstance = (base.gameObject.AddComponent(this.m_EffectProfile.GetType()) as EffectAbstractBase);
				this.InitInstanceFromProfile();
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000396F File Offset: 0x00001B6F
		private void OnDisable()
		{
			if (this.m_EffectInstance)
			{
				this.m_EffectInstance.enabled = false;
			}
		}

		// Token: 0x04000052 RID: 82
		public const string ClassName = "EffectFromProfile";

		// Token: 0x04000053 RID: 83
		[SerializeField]
		private EffectAbstractBase m_EffectProfile;

		// Token: 0x04000054 RID: 84
		private EffectAbstractBase m_EffectInstance;
	}
}
