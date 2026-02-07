using System;
using System.Collections.Generic;
using UnityEngine;

namespace EPOOutline
{
	// Token: 0x02000011 RID: 17
	[ExecuteAlways]
	public class Outlinable : MonoBehaviour
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003307 File Offset: 0x00001507
		// (set) Token: 0x06000045 RID: 69 RVA: 0x0000330F File Offset: 0x0000150F
		public RenderStyle RenderStyle
		{
			get
			{
				return this.renderStyle;
			}
			set
			{
				this.renderStyle = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003318 File Offset: 0x00001518
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00003320 File Offset: 0x00001520
		public ComplexMaskingMode ComplexMaskingMode
		{
			get
			{
				return this.complexMaskingMode;
			}
			set
			{
				this.complexMaskingMode = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003329 File Offset: 0x00001529
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00003331 File Offset: 0x00001531
		public OutlinableDrawingMode DrawingMode
		{
			get
			{
				return this.drawingMode;
			}
			set
			{
				this.drawingMode = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004A RID: 74 RVA: 0x0000333A File Offset: 0x0000153A
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00003342 File Offset: 0x00001542
		public int OutlineLayer
		{
			get
			{
				return this.outlineLayer;
			}
			set
			{
				this.outlineLayer = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600004C RID: 76 RVA: 0x0000334B File Offset: 0x0000154B
		public IReadOnlyList<OutlineTarget> OutlineTargets
		{
			get
			{
				return this.outlineTargets;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003353 File Offset: 0x00001553
		public Outlinable.OutlineProperties OutlineParameters
		{
			get
			{
				return this.outlineParameters;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004E RID: 78 RVA: 0x0000335B File Offset: 0x0000155B
		public Outlinable.OutlineProperties FrontParameters
		{
			get
			{
				return this.frontParameters;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003363 File Offset: 0x00001563
		public Outlinable.OutlineProperties BackParameters
		{
			get
			{
				return this.backParameters;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000050 RID: 80 RVA: 0x0000336C File Offset: 0x0000156C
		internal bool NeedsFillMask
		{
			get
			{
				return (this.drawingMode & OutlinableDrawingMode.Normal) != (OutlinableDrawingMode)0 && this.renderStyle == RenderStyle.FrontBack && (this.frontParameters.Enabled || this.backParameters.Enabled) && (this.frontParameters.FillPass.Material != null || this.backParameters.FillPass.Material != null);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000033DC File Offset: 0x000015DC
		public void AddRenderer(Renderer rendererToAdd, OutlineTargetProvider targetProvider = null)
		{
			int submeshCount = RendererUtility.GetSubmeshCount(rendererToAdd);
			for (int i = 0; i < submeshCount; i++)
			{
				OutlineTarget target = (targetProvider == null) ? new OutlineTarget(rendererToAdd, i) : targetProvider(rendererToAdd, i);
				this.AddTarget(target);
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003418 File Offset: 0x00001618
		[Obsolete("It's obsolete and will be removed. Use AddTarget instead")]
		public void TryAddTarget(OutlineTarget target)
		{
			this.AddTarget(target);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003421 File Offset: 0x00001621
		public void AddTarget(OutlineTarget target)
		{
			this.outlineTargets.Add(target);
			this.ValidateTargets();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003438 File Offset: 0x00001638
		public void RemoveTarget(OutlineTarget target)
		{
			this.outlineTargets.Remove(target);
			if (target.renderer != null)
			{
				TargetStateListener component = target.renderer.GetComponent<TargetStateListener>();
				if (component == null)
				{
					return;
				}
				component.RemoveCallback(this, new Action(this.UpdateVisibility));
			}
		}

		// Token: 0x1700000D RID: 13
		public OutlineTarget this[int index]
		{
			get
			{
				return this.outlineTargets[index];
			}
			set
			{
				this.outlineTargets[index] = value;
				this.ValidateTargets();
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000034AC File Offset: 0x000016AC
		public int OutlineTargetsCount
		{
			get
			{
				return this.outlineTargets.Count;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000034B9 File Offset: 0x000016B9
		private void Reset()
		{
			this.AddAllChildRenderersToRenderingList(RenderersAddingMode.MeshRenderer | RenderersAddingMode.SkinnedMeshRenderer | RenderersAddingMode.SpriteRenderer);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000034C2 File Offset: 0x000016C2
		private void OnValidate()
		{
			this.outlineLayer = Mathf.Clamp(this.outlineLayer, 0, 63);
			this.shouldValidateTargets = true;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000034E0 File Offset: 0x000016E0
		private void SubscribeToVisibilityChange(GameObject go)
		{
			TargetStateListener targetStateListener = go.GetComponent<TargetStateListener>();
			if (targetStateListener == null)
			{
				targetStateListener = go.AddComponent<TargetStateListener>();
			}
			targetStateListener.RemoveCallback(this, new Action(this.UpdateVisibility));
			targetStateListener.AddCallback(this, new Action(this.UpdateVisibility));
			targetStateListener.ForceUpdate();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003530 File Offset: 0x00001730
		private void UpdateVisibility()
		{
			if (!base.enabled)
			{
				Outlinable.outlinables.Remove(this);
				return;
			}
			this.outlineTargets.RemoveAll((OutlineTarget x) => x.renderer == null);
			for (int i = 0; i < this.OutlineTargets.Count; i++)
			{
				OutlineTarget outlineTarget = this.OutlineTargets[i];
				outlineTarget.IsVisible = outlineTarget.renderer.isVisible;
			}
			this.outlineTargets.RemoveAll((OutlineTarget x) => x.renderer == null);
			using (List<OutlineTarget>.Enumerator enumerator = this.outlineTargets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsVisible)
					{
						Outlinable.outlinables.Add(this);
						return;
					}
				}
			}
			Outlinable.outlinables.Remove(this);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000363C File Offset: 0x0000183C
		private void OnEnable()
		{
			this.UpdateVisibility();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003644 File Offset: 0x00001844
		private void OnDisable()
		{
			Outlinable.outlinables.Remove(this);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003652 File Offset: 0x00001852
		private void Awake()
		{
			this.ValidateTargets();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000365C File Offset: 0x0000185C
		private void ValidateTargets()
		{
			this.outlineTargets.RemoveAll((OutlineTarget x) => x.renderer == null);
			foreach (OutlineTarget outlineTarget in this.outlineTargets)
			{
				this.SubscribeToVisibilityChange(outlineTarget.renderer.gameObject);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000036E4 File Offset: 0x000018E4
		private void OnDestroy()
		{
			Outlinable.outlinables.Remove(this);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000036F4 File Offset: 0x000018F4
		public static void GetAllActiveOutlinables(List<Outlinable> outlinablesList)
		{
			outlinablesList.Clear();
			foreach (Outlinable item in Outlinable.outlinables)
			{
				outlinablesList.Add(item);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000374C File Offset: 0x0000194C
		public void AddAllChildRenderersToRenderingList(RenderersAddingMode renderersAddingMode = RenderersAddingMode.All)
		{
			this.outlineTargets.Clear();
			foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
			{
				if (this.MatchingMode(renderer, renderersAddingMode))
				{
					int submeshCount = RendererUtility.GetSubmeshCount(renderer);
					for (int j = 0; j < submeshCount; j++)
					{
						this.AddTarget(new OutlineTarget(renderer, j));
					}
				}
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000037AD File Offset: 0x000019AD
		private void Update()
		{
			if (!this.shouldValidateTargets)
			{
				return;
			}
			this.shouldValidateTargets = false;
			this.ValidateTargets();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000037C8 File Offset: 0x000019C8
		private bool MatchingMode(Renderer rendererToMatch, RenderersAddingMode mode)
		{
			return (!(rendererToMatch is MeshRenderer) && !(rendererToMatch is SkinnedMeshRenderer) && !(rendererToMatch is SpriteRenderer) && (mode & RenderersAddingMode.Others) != RenderersAddingMode.None) || (rendererToMatch is MeshRenderer && (mode & RenderersAddingMode.MeshRenderer) != RenderersAddingMode.None) || (rendererToMatch is SpriteRenderer && (mode & RenderersAddingMode.SpriteRenderer) != RenderersAddingMode.None) || (rendererToMatch is SkinnedMeshRenderer && (mode & RenderersAddingMode.SkinnedMeshRenderer) > RenderersAddingMode.None);
		}

		// Token: 0x0400003C RID: 60
		private static HashSet<Outlinable> outlinables = new HashSet<Outlinable>();

		// Token: 0x0400003D RID: 61
		[SerializeField]
		private ComplexMaskingMode complexMaskingMode;

		// Token: 0x0400003E RID: 62
		[SerializeField]
		private OutlinableDrawingMode drawingMode = OutlinableDrawingMode.Normal;

		// Token: 0x0400003F RID: 63
		[SerializeField]
		private int outlineLayer;

		// Token: 0x04000040 RID: 64
		[SerializeField]
		private List<OutlineTarget> outlineTargets = new List<OutlineTarget>();

		// Token: 0x04000041 RID: 65
		[SerializeField]
		private RenderStyle renderStyle = RenderStyle.Single;

		// Token: 0x04000042 RID: 66
		[SerializeField]
		private Outlinable.OutlineProperties outlineParameters = new Outlinable.OutlineProperties();

		// Token: 0x04000043 RID: 67
		[SerializeField]
		private Outlinable.OutlineProperties backParameters = new Outlinable.OutlineProperties();

		// Token: 0x04000044 RID: 68
		[SerializeField]
		private Outlinable.OutlineProperties frontParameters = new Outlinable.OutlineProperties();

		// Token: 0x04000045 RID: 69
		private bool shouldValidateTargets;

		// Token: 0x0200002B RID: 43
		[Serializable]
		public class OutlineProperties
		{
			// Token: 0x1700002D RID: 45
			// (get) Token: 0x060000F6 RID: 246 RVA: 0x00006EB5 File Offset: 0x000050B5
			// (set) Token: 0x060000F7 RID: 247 RVA: 0x00006EBD File Offset: 0x000050BD
			public bool Enabled
			{
				get
				{
					return this.enabled;
				}
				set
				{
					this.enabled = value;
				}
			}

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x060000F8 RID: 248 RVA: 0x00006EC6 File Offset: 0x000050C6
			// (set) Token: 0x060000F9 RID: 249 RVA: 0x00006ECE File Offset: 0x000050CE
			public Color Color
			{
				get
				{
					return this.color;
				}
				set
				{
					this.color = value;
				}
			}

			// Token: 0x1700002F RID: 47
			// (get) Token: 0x060000FA RID: 250 RVA: 0x00006ED7 File Offset: 0x000050D7
			// (set) Token: 0x060000FB RID: 251 RVA: 0x00006EDF File Offset: 0x000050DF
			public float DilateShift
			{
				get
				{
					return this.dilateShift;
				}
				set
				{
					this.dilateShift = value;
				}
			}

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x060000FC RID: 252 RVA: 0x00006EE8 File Offset: 0x000050E8
			// (set) Token: 0x060000FD RID: 253 RVA: 0x00006EF0 File Offset: 0x000050F0
			public float BlurShift
			{
				get
				{
					return this.blurShift;
				}
				set
				{
					this.blurShift = value;
				}
			}

			// Token: 0x17000031 RID: 49
			// (get) Token: 0x060000FE RID: 254 RVA: 0x00006EF9 File Offset: 0x000050F9
			public SerializedPass FillPass
			{
				get
				{
					return this.fillPass;
				}
			}

			// Token: 0x040000DF RID: 223
			[SerializeField]
			private bool enabled = true;

			// Token: 0x040000E0 RID: 224
			[SerializeField]
			private Color color = Color.yellow;

			// Token: 0x040000E1 RID: 225
			[SerializeField]
			[Range(0f, 1f)]
			private float dilateShift = 1f;

			// Token: 0x040000E2 RID: 226
			[SerializeField]
			[Range(0f, 1f)]
			private float blurShift = 1f;

			// Token: 0x040000E3 RID: 227
			[SerializeField]
			[SerializedPassInfo("Fill style", "Hidden/EPO/Fill/")]
			private SerializedPass fillPass = new SerializedPass();
		}
	}
}
