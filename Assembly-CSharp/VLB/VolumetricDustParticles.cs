using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000047 RID: 71
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightBeamAbstractBase))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-dustparticles/")]
	[AddComponentMenu("VLB/Common/Volumetric Dust Particles")]
	public class VolumetricDustParticles : MonoBehaviour
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000ACDF File Offset: 0x00008EDF
		// (set) Token: 0x060002AF RID: 687 RVA: 0x0000ACE7 File Offset: 0x00008EE7
		public bool isCulled { get; private set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000ACF0 File Offset: 0x00008EF0
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x0000ACF8 File Offset: 0x00008EF8
		public float alphaAdditionalRuntime
		{
			get
			{
				return this.m_AlphaAdditionalRuntime;
			}
			set
			{
				if (this.m_AlphaAdditionalRuntime != value)
				{
					this.m_AlphaAdditionalRuntime = value;
					this.m_RuntimePropertiesDirty = true;
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000AD11 File Offset: 0x00008F11
		public bool particlesAreInstantiated
		{
			get
			{
				return this.m_Particles;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000AD1E File Offset: 0x00008F1E
		public int particlesCurrentCount
		{
			get
			{
				if (!this.m_Particles)
				{
					return 0;
				}
				return this.m_Particles.particleCount;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000AD3C File Offset: 0x00008F3C
		public int particlesMaxCount
		{
			get
			{
				if (!this.m_Particles)
				{
					return 0;
				}
				return this.m_Particles.main.maxParticles;
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000AD6B File Offset: 0x00008F6B
		public ParticleSystemRenderer FindRenderer()
		{
			if (this.m_Renderer)
			{
				return this.m_Renderer;
			}
			return this.m_Particles.GetComponent<ParticleSystemRenderer>();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000AD8C File Offset: 0x00008F8C
		private void Start()
		{
			this.isCulled = false;
			this.m_Master = base.GetComponent<VolumetricLightBeamAbstractBase>();
			this.HandleBackwardCompatibility(this.m_Master._INTERNAL_pluginVersion, 20200);
			this.InstantiateParticleSystem();
			this.SetActiveAndPlay();
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000ADC4 File Offset: 0x00008FC4
		private void InstantiateParticleSystem()
		{
			base.gameObject.ForeachComponentsInDirectChildrenOnly(delegate(ParticleSystem ps)
			{
				UnityEngine.Object.DestroyImmediate(ps.gameObject);
			}, true);
			this.m_Particles = Config.Instance.NewVolumetricDustParticles();
			if (this.m_Particles)
			{
				this.m_Particles.transform.SetParent(base.transform, false);
				this.m_Renderer = this.m_Particles.GetComponent<ParticleSystemRenderer>();
				this.m_Material = new Material(this.m_Renderer.sharedMaterial);
				this.m_Renderer.material = this.m_Material;
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000AE68 File Offset: 0x00009068
		private void OnEnable()
		{
			this.SetActiveAndPlay();
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000AE70 File Offset: 0x00009070
		private void SetActive(bool active)
		{
			if (this.m_Particles)
			{
				this.m_Particles.gameObject.SetActive(active);
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000AE90 File Offset: 0x00009090
		private void SetActiveAndPlay()
		{
			this.SetActive(true);
			this.Play();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000AE9F File Offset: 0x0000909F
		private void Play()
		{
			if (this.m_Particles)
			{
				this.SetParticleProperties();
				this.m_Particles.Simulate(0f);
				this.m_Particles.Play(true);
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000AED0 File Offset: 0x000090D0
		private void OnDisable()
		{
			this.SetActive(false);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000AEDC File Offset: 0x000090DC
		private void OnDestroy()
		{
			if (this.m_Particles)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Particles.gameObject);
				this.m_Particles = null;
			}
			if (this.m_Material)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Material);
				this.m_Material = null;
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000AF2C File Offset: 0x0000912C
		private void Update()
		{
			this.UpdateCulling();
			if (UtilsBeamProps.CanChangeDuringPlaytime(this.m_Master))
			{
				this.SetParticleProperties();
			}
			if (this.m_RuntimePropertiesDirty && this.m_Material != null)
			{
				this.m_Material.SetColor(ShaderProperties.ParticlesTintColor, new Color(1f, 1f, 1f, this.alphaAdditionalRuntime));
				this.m_RuntimePropertiesDirty = false;
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000AF9C File Offset: 0x0000919C
		private void SetParticleProperties()
		{
			if (this.m_Particles && this.m_Particles.gameObject.activeSelf)
			{
				this.m_Particles.transform.localRotation = UtilsBeamProps.GetInternalLocalRotation(this.m_Master);
				this.m_Particles.transform.localScale = (this.m_Master.IsScalable() ? Vector3.one : Vector3.one.Divide(this.m_Master.GetLossyScale()));
				float num = UtilsBeamProps.GetFallOffEnd(this.m_Master) * (this.spawnDistanceRange.maxValue - this.spawnDistanceRange.minValue);
				float num2 = num * this.density;
				int maxParticles = (int)(num2 * 4f);
				ParticleSystem.MainModule main = this.m_Particles.main;
				ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
				startLifetime.mode = ParticleSystemCurveMode.TwoConstants;
				startLifetime.constantMin = 4f;
				startLifetime.constantMax = 6f;
				main.startLifetime = startLifetime;
				ParticleSystem.MinMaxCurve startSize = main.startSize;
				startSize.mode = ParticleSystemCurveMode.TwoConstants;
				startSize.constantMin = this.size * 0.9f;
				startSize.constantMax = this.size * 1.1f;
				main.startSize = startSize;
				ParticleSystem.MinMaxGradient startColor = main.startColor;
				if (UtilsBeamProps.GetColorMode(this.m_Master) == ColorMode.Flat)
				{
					startColor.mode = ParticleSystemGradientMode.Color;
					Color colorFlat = UtilsBeamProps.GetColorFlat(this.m_Master);
					colorFlat.a *= this.alpha;
					startColor.color = colorFlat;
				}
				else
				{
					startColor.mode = ParticleSystemGradientMode.Gradient;
					Gradient colorGradient = UtilsBeamProps.GetColorGradient(this.m_Master);
					GradientColorKey[] colorKeys = colorGradient.colorKeys;
					GradientAlphaKey[] alphaKeys = colorGradient.alphaKeys;
					for (int i = 0; i < alphaKeys.Length; i++)
					{
						GradientAlphaKey[] array = alphaKeys;
						int num3 = i;
						array[num3].alpha = array[num3].alpha * this.alpha;
					}
					this.m_GradientCached.SetKeys(colorKeys, alphaKeys);
					startColor.gradient = this.m_GradientCached;
				}
				main.startColor = startColor;
				ParticleSystem.MinMaxCurve startSpeed = main.startSpeed;
				startSpeed.constant = ((this.direction == ParticlesDirection.Random) ? Mathf.Abs(this.velocity.z) : 0f);
				main.startSpeed = startSpeed;
				ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = this.m_Particles.velocityOverLifetime;
				velocityOverLifetime.enabled = (this.direction > ParticlesDirection.Random);
				velocityOverLifetime.space = ((this.direction == ParticlesDirection.LocalSpace) ? ParticleSystemSimulationSpace.Local : ParticleSystemSimulationSpace.World);
				velocityOverLifetime.xMultiplier = this.velocity.x;
				velocityOverLifetime.yMultiplier = this.velocity.y;
				velocityOverLifetime.zMultiplier = this.velocity.z;
				main.maxParticles = maxParticles;
				float thickness = UtilsBeamProps.GetThickness(this.m_Master);
				float fallOffEnd = UtilsBeamProps.GetFallOffEnd(this.m_Master);
				ParticleSystem.ShapeModule shape = this.m_Particles.shape;
				shape.shapeType = ParticleSystemShapeType.ConeVolume;
				float num4 = UtilsBeamProps.GetConeAngle(this.m_Master) * Mathf.Lerp(0.7f, 1f, thickness);
				shape.angle = num4 * 0.5f;
				float a = UtilsBeamProps.GetConeRadiusStart(this.m_Master) * Mathf.Lerp(0.3f, 1f, thickness);
				float b = Utils.ComputeConeRadiusEnd(fallOffEnd, num4);
				shape.radius = Mathf.Lerp(a, b, this.spawnDistanceRange.minValue);
				shape.length = num;
				float z = fallOffEnd * this.spawnDistanceRange.minValue;
				shape.position = new Vector3(0f, 0f, z);
				shape.arc = 360f;
				shape.randomDirectionAmount = ((this.direction == ParticlesDirection.Random) ? 1f : 0f);
				ParticleSystem.EmissionModule emission = this.m_Particles.emission;
				ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
				rateOverTime.constant = num2;
				emission.rateOverTime = rateOverTime;
				if (this.m_Renderer)
				{
					this.m_Renderer.sortingLayerID = UtilsBeamProps.GetSortingLayerID(this.m_Master);
					this.m_Renderer.sortingOrder = UtilsBeamProps.GetSortingOrder(this.m_Master);
				}
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000B38C File Offset: 0x0000958C
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
			if (serializedVersion < 1880)
			{
				if (this.direction == ParticlesDirection.Random)
				{
					this.direction = ParticlesDirection.LocalSpace;
				}
				else
				{
					this.direction = ParticlesDirection.Random;
				}
				this.velocity = new Vector3(0f, 0f, this.speed);
			}
			if (serializedVersion < 1940)
			{
				this.spawnDistanceRange = new MinMaxRangeFloat(this.spawnMinDistance, this.spawnMaxDistance);
			}
			Utils.MarkCurrentSceneDirty();
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000B404 File Offset: 0x00009604
		private void UpdateCulling()
		{
			if (this.m_Particles)
			{
				bool flag = true;
				bool fadeOutEnabled = UtilsBeamProps.GetFadeOutEnabled(this.m_Master);
				if ((this.cullingEnabled || fadeOutEnabled) && this.m_Master.hasGeometry)
				{
					if (Config.Instance.fadeOutCameraTransform)
					{
						float num = this.cullingMaxDistance;
						if (fadeOutEnabled)
						{
							num = Mathf.Min(num, UtilsBeamProps.GetFadeOutEnd(this.m_Master));
						}
						float num2 = num * num;
						flag = (this.m_Master.bounds.SqrDistance(Config.Instance.fadeOutCameraTransform.position) <= num2);
					}
					else
					{
						Debug.LogErrorFormat(base.gameObject, "Fail to retrieve the camera with tag '{0}' (specified in VLB Config's 'fadeOutCameraTag') for the {1} Culling feature.", new object[]
						{
							Config.Instance.fadeOutCameraTag,
							"VolumetricDustParticles"
						});
					}
				}
				if (this.m_Particles.gameObject.activeSelf != flag)
				{
					this.SetActive(flag);
					this.isCulled = !flag;
				}
				if (flag && !this.m_Particles.isPlaying)
				{
					this.m_Particles.Play();
				}
			}
		}

		// Token: 0x0400019F RID: 415
		public const string ClassName = "VolumetricDustParticles";

		// Token: 0x040001A0 RID: 416
		[Range(0f, 1f)]
		public float alpha = 0.5f;

		// Token: 0x040001A1 RID: 417
		[Range(0.0001f, 0.1f)]
		public float size = 0.01f;

		// Token: 0x040001A2 RID: 418
		public ParticlesDirection direction;

		// Token: 0x040001A3 RID: 419
		public Vector3 velocity = Consts.DustParticles.VelocityDefault;

		// Token: 0x040001A4 RID: 420
		[Obsolete("Use 'velocity' instead")]
		public float speed = 0.03f;

		// Token: 0x040001A5 RID: 421
		public float density = 5f;

		// Token: 0x040001A6 RID: 422
		[MinMaxRange(0f, 1f)]
		public MinMaxRangeFloat spawnDistanceRange = Consts.DustParticles.SpawnDistanceRangeDefault;

		// Token: 0x040001A7 RID: 423
		[Obsolete("Use 'spawnDistanceRange' instead")]
		public float spawnMinDistance;

		// Token: 0x040001A8 RID: 424
		[Obsolete("Use 'spawnDistanceRange' instead")]
		public float spawnMaxDistance = 0.7f;

		// Token: 0x040001A9 RID: 425
		public bool cullingEnabled;

		// Token: 0x040001AA RID: 426
		public float cullingMaxDistance = 10f;

		// Token: 0x040001AC RID: 428
		[SerializeField]
		private float m_AlphaAdditionalRuntime = 1f;

		// Token: 0x040001AD RID: 429
		private ParticleSystem m_Particles;

		// Token: 0x040001AE RID: 430
		private ParticleSystemRenderer m_Renderer;

		// Token: 0x040001AF RID: 431
		private Material m_Material;

		// Token: 0x040001B0 RID: 432
		private Gradient m_GradientCached = new Gradient();

		// Token: 0x040001B1 RID: 433
		private bool m_RuntimePropertiesDirty = true;

		// Token: 0x040001B2 RID: 434
		private VolumetricLightBeamAbstractBase m_Master;
	}
}
