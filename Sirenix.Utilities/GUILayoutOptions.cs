using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x02000021 RID: 33
	public static class GUILayoutOptions
	{
		// Token: 0x0600015E RID: 350 RVA: 0x0000B6B8 File Offset: 0x000098B8
		static GUILayoutOptions()
		{
			GUILayoutOptions.GUILayoutOptionsInstanceCache = new GUILayoutOptions.GUILayoutOptionsInstance[30];
			GUILayoutOptions.GUILayoutOptionsInstanceCache[0] = new GUILayoutOptions.GUILayoutOptionsInstance();
			for (int i = 1; i < 30; i++)
			{
				GUILayoutOptions.GUILayoutOptionsInstanceCache[i] = new GUILayoutOptions.GUILayoutOptionsInstance();
				GUILayoutOptions.GUILayoutOptionsInstanceCache[i].Parent = GUILayoutOptions.GUILayoutOptionsInstanceCache[i - 1];
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000B728 File Offset: 0x00009928
		public static GUILayoutOptions.GUILayoutOptionsInstance Width(float width)
		{
			GUILayoutOptions.CurrentCacheIndex = 0;
			GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
			guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.Width, width);
			return guilayoutOptionsInstance;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000B758 File Offset: 0x00009958
		public static GUILayoutOptions.GUILayoutOptionsInstance Height(float height)
		{
			GUILayoutOptions.CurrentCacheIndex = 0;
			GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
			guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.Height, height);
			return guilayoutOptionsInstance;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000B788 File Offset: 0x00009988
		public static GUILayoutOptions.GUILayoutOptionsInstance MaxHeight(float height)
		{
			GUILayoutOptions.CurrentCacheIndex = 0;
			GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
			guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.MaxHeight, height);
			return guilayoutOptionsInstance;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000B7B8 File Offset: 0x000099B8
		public static GUILayoutOptions.GUILayoutOptionsInstance MaxWidth(float width)
		{
			GUILayoutOptions.CurrentCacheIndex = 0;
			GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
			guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.MaxWidth, width);
			return guilayoutOptionsInstance;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000B7E8 File Offset: 0x000099E8
		public static GUILayoutOptions.GUILayoutOptionsInstance MinWidth(float width)
		{
			GUILayoutOptions.CurrentCacheIndex = 0;
			GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
			guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.MinWidth, width);
			return guilayoutOptionsInstance;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000B818 File Offset: 0x00009A18
		public static GUILayoutOptions.GUILayoutOptionsInstance MinHeight(float height)
		{
			GUILayoutOptions.CurrentCacheIndex = 0;
			GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
			guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.MinHeight, height);
			return guilayoutOptionsInstance;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000B848 File Offset: 0x00009A48
		public static GUILayoutOptions.GUILayoutOptionsInstance ExpandHeight(bool expand = true)
		{
			GUILayoutOptions.CurrentCacheIndex = 0;
			GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
			guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.ExpandHeight, expand);
			return guilayoutOptionsInstance;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000B878 File Offset: 0x00009A78
		public static GUILayoutOptions.GUILayoutOptionsInstance ExpandWidth(bool expand = true)
		{
			GUILayoutOptions.CurrentCacheIndex = 0;
			GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
			guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.ExpandWidth, expand);
			return guilayoutOptionsInstance;
		}

		// Token: 0x04000051 RID: 81
		private static int CurrentCacheIndex = 0;

		// Token: 0x04000052 RID: 82
		private static readonly GUILayoutOptions.GUILayoutOptionsInstance[] GUILayoutOptionsInstanceCache;

		// Token: 0x04000053 RID: 83
		private static readonly Dictionary<GUILayoutOptions.GUILayoutOptionsInstance, GUILayoutOption[]> GUILayoutOptionsCache = new Dictionary<GUILayoutOptions.GUILayoutOptionsInstance, GUILayoutOption[]>();

		// Token: 0x04000054 RID: 84
		public static readonly GUILayoutOption[] EmptyGUIOptions = new GUILayoutOption[0];

		// Token: 0x02000085 RID: 133
		internal enum GUILayoutOptionType
		{
			// Token: 0x040001C9 RID: 457
			Width,
			// Token: 0x040001CA RID: 458
			Height,
			// Token: 0x040001CB RID: 459
			MinWidth,
			// Token: 0x040001CC RID: 460
			MaxHeight,
			// Token: 0x040001CD RID: 461
			MaxWidth,
			// Token: 0x040001CE RID: 462
			MinHeight,
			// Token: 0x040001CF RID: 463
			ExpandHeight,
			// Token: 0x040001D0 RID: 464
			ExpandWidth
		}

		// Token: 0x02000086 RID: 134
		public sealed class GUILayoutOptionsInstance : IEquatable<GUILayoutOptions.GUILayoutOptionsInstance>
		{
			// Token: 0x060003B8 RID: 952 RVA: 0x0001195C File Offset: 0x0000FB5C
			private GUILayoutOption[] GetCachedOptions()
			{
				GUILayoutOption[] result;
				if (!GUILayoutOptions.GUILayoutOptionsCache.TryGetValue(this, ref result))
				{
					result = (GUILayoutOptions.GUILayoutOptionsCache[this.Clone()] = this.CreateOptionsArary());
				}
				return result;
			}

			// Token: 0x060003B9 RID: 953 RVA: 0x00011993 File Offset: 0x0000FB93
			public static implicit operator GUILayoutOption[](GUILayoutOptions.GUILayoutOptionsInstance options)
			{
				return options.GetCachedOptions();
			}

			// Token: 0x060003BA RID: 954 RVA: 0x0001199C File Offset: 0x0000FB9C
			private GUILayoutOption[] CreateOptionsArary()
			{
				List<GUILayoutOption> list = new List<GUILayoutOption>();
				for (GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = this; guilayoutOptionsInstance != null; guilayoutOptionsInstance = guilayoutOptionsInstance.Parent)
				{
					switch (guilayoutOptionsInstance.GUILayoutOptionType)
					{
					case GUILayoutOptions.GUILayoutOptionType.Width:
						list.Add(GUILayout.Width(guilayoutOptionsInstance.value));
						break;
					case GUILayoutOptions.GUILayoutOptionType.Height:
						list.Add(GUILayout.Height(guilayoutOptionsInstance.value));
						break;
					case GUILayoutOptions.GUILayoutOptionType.MinWidth:
						list.Add(GUILayout.MinWidth(guilayoutOptionsInstance.value));
						break;
					case GUILayoutOptions.GUILayoutOptionType.MaxHeight:
						list.Add(GUILayout.MaxHeight(guilayoutOptionsInstance.value));
						break;
					case GUILayoutOptions.GUILayoutOptionType.MaxWidth:
						list.Add(GUILayout.MaxWidth(guilayoutOptionsInstance.value));
						break;
					case GUILayoutOptions.GUILayoutOptionType.MinHeight:
						list.Add(GUILayout.MinHeight(guilayoutOptionsInstance.value));
						break;
					case GUILayoutOptions.GUILayoutOptionType.ExpandHeight:
						list.Add(GUILayout.ExpandHeight(guilayoutOptionsInstance.value > 0.2f));
						break;
					case GUILayoutOptions.GUILayoutOptionType.ExpandWidth:
						list.Add(GUILayout.ExpandWidth(guilayoutOptionsInstance.value > 0.2f));
						break;
					}
				}
				return list.ToArray();
			}

			// Token: 0x060003BB RID: 955 RVA: 0x00011AA4 File Offset: 0x0000FCA4
			private GUILayoutOptions.GUILayoutOptionsInstance Clone()
			{
				GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = new GUILayoutOptions.GUILayoutOptionsInstance
				{
					value = this.value,
					GUILayoutOptionType = this.GUILayoutOptionType
				};
				GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance2 = guilayoutOptionsInstance;
				GUILayoutOptions.GUILayoutOptionsInstance parent = this.Parent;
				while (parent != null)
				{
					guilayoutOptionsInstance2.Parent = new GUILayoutOptions.GUILayoutOptionsInstance
					{
						value = parent.value,
						GUILayoutOptionType = parent.GUILayoutOptionType
					};
					parent = parent.Parent;
					guilayoutOptionsInstance2 = guilayoutOptionsInstance2.Parent;
				}
				return guilayoutOptionsInstance;
			}

			// Token: 0x060003BC RID: 956 RVA: 0x0000CF61 File Offset: 0x0000B161
			internal GUILayoutOptionsInstance()
			{
			}

			// Token: 0x060003BD RID: 957 RVA: 0x00011B14 File Offset: 0x0000FD14
			public GUILayoutOptions.GUILayoutOptionsInstance Width(float width)
			{
				GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
				guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.Width, width);
				return guilayoutOptionsInstance;
			}

			// Token: 0x060003BE RID: 958 RVA: 0x00011B40 File Offset: 0x0000FD40
			public GUILayoutOptions.GUILayoutOptionsInstance Height(float height)
			{
				GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
				guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.Height, height);
				return guilayoutOptionsInstance;
			}

			// Token: 0x060003BF RID: 959 RVA: 0x00011B6C File Offset: 0x0000FD6C
			public GUILayoutOptions.GUILayoutOptionsInstance MaxHeight(float height)
			{
				GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
				guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.MaxHeight, height);
				return guilayoutOptionsInstance;
			}

			// Token: 0x060003C0 RID: 960 RVA: 0x00011B98 File Offset: 0x0000FD98
			public GUILayoutOptions.GUILayoutOptionsInstance MaxWidth(float width)
			{
				GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
				guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.MaxWidth, width);
				return guilayoutOptionsInstance;
			}

			// Token: 0x060003C1 RID: 961 RVA: 0x00011BC4 File Offset: 0x0000FDC4
			public GUILayoutOptions.GUILayoutOptionsInstance MinHeight(float height)
			{
				GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
				guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.MinHeight, height);
				return guilayoutOptionsInstance;
			}

			// Token: 0x060003C2 RID: 962 RVA: 0x00011BF0 File Offset: 0x0000FDF0
			public GUILayoutOptions.GUILayoutOptionsInstance MinWidth(float width)
			{
				GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
				guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.MinWidth, width);
				return guilayoutOptionsInstance;
			}

			// Token: 0x060003C3 RID: 963 RVA: 0x00011C1C File Offset: 0x0000FE1C
			public GUILayoutOptions.GUILayoutOptionsInstance ExpandHeight(bool expand = true)
			{
				GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
				guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.ExpandHeight, expand);
				return guilayoutOptionsInstance;
			}

			// Token: 0x060003C4 RID: 964 RVA: 0x00011C48 File Offset: 0x0000FE48
			public GUILayoutOptions.GUILayoutOptionsInstance ExpandWidth(bool expand = true)
			{
				GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = GUILayoutOptions.GUILayoutOptionsInstanceCache[GUILayoutOptions.CurrentCacheIndex++];
				guilayoutOptionsInstance.SetValue(GUILayoutOptions.GUILayoutOptionType.ExpandWidth, expand);
				return guilayoutOptionsInstance;
			}

			// Token: 0x060003C5 RID: 965 RVA: 0x00011C72 File Offset: 0x0000FE72
			internal void SetValue(GUILayoutOptions.GUILayoutOptionType type, float value)
			{
				this.GUILayoutOptionType = type;
				this.value = value;
			}

			// Token: 0x060003C6 RID: 966 RVA: 0x00011C82 File Offset: 0x0000FE82
			internal void SetValue(GUILayoutOptions.GUILayoutOptionType type, bool value)
			{
				this.GUILayoutOptionType = type;
				this.value = (value > false);
			}

			// Token: 0x060003C7 RID: 967 RVA: 0x00011C98 File Offset: 0x0000FE98
			public bool Equals(GUILayoutOptions.GUILayoutOptionsInstance other)
			{
				GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = this;
				GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance2 = other;
				while (guilayoutOptionsInstance != null && guilayoutOptionsInstance2 != null)
				{
					if (guilayoutOptionsInstance.GUILayoutOptionType != guilayoutOptionsInstance2.GUILayoutOptionType || guilayoutOptionsInstance.value != guilayoutOptionsInstance2.value)
					{
						return false;
					}
					guilayoutOptionsInstance = guilayoutOptionsInstance.Parent;
					guilayoutOptionsInstance2 = guilayoutOptionsInstance2.Parent;
				}
				return guilayoutOptionsInstance2 == null && guilayoutOptionsInstance == null;
			}

			// Token: 0x060003C8 RID: 968 RVA: 0x00011CE8 File Offset: 0x0000FEE8
			public override int GetHashCode()
			{
				int num = 0;
				int num2 = 17;
				for (GUILayoutOptions.GUILayoutOptionsInstance guilayoutOptionsInstance = this; guilayoutOptionsInstance != null; guilayoutOptionsInstance = guilayoutOptionsInstance.Parent)
				{
					num2 = num2 * 29 + this.GUILayoutOptionType.GetHashCode() + this.value.GetHashCode() * 17 + num++;
				}
				return num2;
			}

			// Token: 0x040001D1 RID: 465
			private float value;

			// Token: 0x040001D2 RID: 466
			internal GUILayoutOptions.GUILayoutOptionsInstance Parent;

			// Token: 0x040001D3 RID: 467
			internal GUILayoutOptions.GUILayoutOptionType GUILayoutOptionType;
		}
	}
}
