using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000029 RID: 41
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightBeamHD))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-cookie-hd/")]
	[AddComponentMenu("VLB/HD/Volumetric Cookie HD")]
	public class VolumetricCookieHD : MonoBehaviour
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000046E5 File Offset: 0x000028E5
		// (set) Token: 0x0600009A RID: 154 RVA: 0x000046ED File Offset: 0x000028ED
		public float contribution
		{
			get
			{
				return this.m_Contribution;
			}
			set
			{
				if (this.m_Contribution != value)
				{
					this.m_Contribution = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00004705 File Offset: 0x00002905
		// (set) Token: 0x0600009C RID: 156 RVA: 0x0000470D File Offset: 0x0000290D
		public Texture cookieTexture
		{
			get
			{
				return this.m_CookieTexture;
			}
			set
			{
				if (this.m_CookieTexture != value)
				{
					this.m_CookieTexture = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000472A File Offset: 0x0000292A
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00004732 File Offset: 0x00002932
		public CookieChannel channel
		{
			get
			{
				return this.m_Channel;
			}
			set
			{
				if (this.m_Channel != value)
				{
					this.m_Channel = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600009F RID: 159 RVA: 0x0000474A File Offset: 0x0000294A
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00004752 File Offset: 0x00002952
		public bool negative
		{
			get
			{
				return this.m_Negative;
			}
			set
			{
				if (this.m_Negative != value)
				{
					this.m_Negative = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x0000476A File Offset: 0x0000296A
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00004772 File Offset: 0x00002972
		public Vector2 translation
		{
			get
			{
				return this.m_Translation;
			}
			set
			{
				if (this.m_Translation != value)
				{
					this.m_Translation = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000478F File Offset: 0x0000298F
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00004797 File Offset: 0x00002997
		public float rotation
		{
			get
			{
				return this.m_Rotation;
			}
			set
			{
				if (this.m_Rotation != value)
				{
					this.m_Rotation = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000047AF File Offset: 0x000029AF
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x000047B7 File Offset: 0x000029B7
		public Vector2 scale
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				if (this.m_Scale != value)
				{
					this.m_Scale = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000047D4 File Offset: 0x000029D4
		private void SetDirty()
		{
			if (this.m_Master)
			{
				this.m_Master.SetPropertyDirty(DirtyProps.CookieProps);
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000047F4 File Offset: 0x000029F4
		public static void ApplyMaterialProperties(VolumetricCookieHD instance, BeamGeometryHD geom)
		{
			if (instance && instance.enabled && instance.cookieTexture != null)
			{
				geom.SetMaterialProp(ShaderProperties.HD.CookieTexture, instance.cookieTexture);
				geom.SetMaterialProp(ShaderProperties.HD.CookieProperties, new Vector4(instance.negative ? instance.contribution : (-instance.contribution), (float)instance.channel, Mathf.Cos(instance.rotation * 0.017453292f), Mathf.Sin(instance.rotation * 0.017453292f)));
				geom.SetMaterialProp(ShaderProperties.HD.CookiePosAndScale, new Vector4(instance.translation.x, instance.translation.y, instance.scale.x, instance.scale.y));
				return;
			}
			geom.SetMaterialProp(ShaderProperties.HD.CookieTexture, BeamGeometryHD.InvalidTexture.Null);
			geom.SetMaterialProp(ShaderProperties.HD.CookieProperties, Vector4.zero);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000048E2 File Offset: 0x00002AE2
		private void Awake()
		{
			this.m_Master = base.GetComponent<VolumetricLightBeamHD>();
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000048F0 File Offset: 0x00002AF0
		private void OnEnable()
		{
			this.SetDirty();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000048F8 File Offset: 0x00002AF8
		private void OnDisable()
		{
			this.SetDirty();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004900 File Offset: 0x00002B00
		private void OnDidApplyAnimationProperties()
		{
			this.SetDirty();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004908 File Offset: 0x00002B08
		private void Start()
		{
			if (Application.isPlaying)
			{
				this.SetDirty();
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004917 File Offset: 0x00002B17
		private void OnDestroy()
		{
			if (Application.isPlaying)
			{
				this.SetDirty();
			}
		}

		// Token: 0x040000D2 RID: 210
		public const string ClassName = "VolumetricCookieHD";

		// Token: 0x040000D3 RID: 211
		[SerializeField]
		private float m_Contribution = 1f;

		// Token: 0x040000D4 RID: 212
		[SerializeField]
		private Texture m_CookieTexture;

		// Token: 0x040000D5 RID: 213
		[SerializeField]
		private CookieChannel m_Channel = CookieChannel.Alpha;

		// Token: 0x040000D6 RID: 214
		[SerializeField]
		private bool m_Negative;

		// Token: 0x040000D7 RID: 215
		[SerializeField]
		private Vector2 m_Translation = Consts.Cookie.TranslationDefault;

		// Token: 0x040000D8 RID: 216
		[SerializeField]
		private float m_Rotation;

		// Token: 0x040000D9 RID: 217
		[SerializeField]
		private Vector2 m_Scale = Consts.Cookie.ScaleDefault;

		// Token: 0x040000DA RID: 218
		private VolumetricLightBeamHD m_Master;
	}
}
