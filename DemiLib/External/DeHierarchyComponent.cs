using System;
using System.Collections.Generic;
using UnityEngine;

namespace DG.DemiLib.External
{
	// Token: 0x0200000E RID: 14
	public class DeHierarchyComponent : MonoBehaviour
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002C74 File Offset: 0x00000E74
		public List<int> MissingItemsIndexes()
		{
			List<int> list = null;
			for (int i = this.customizedItems.Count - 1; i > -1; i--)
			{
				if (this.customizedItems[i].gameObject == null)
				{
					if (list == null)
					{
						list = new List<int>();
					}
					list.Add(i);
				}
			}
			return list;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002CC8 File Offset: 0x00000EC8
		public void StoreItemColor(GameObject go, DeHierarchyComponent.HColor hColor)
		{
			for (int i = 0; i < this.customizedItems.Count; i++)
			{
				if (!(this.customizedItems[i].gameObject != go))
				{
					this.customizedItems[i].hColor = hColor;
					return;
				}
			}
			this.customizedItems.Add(new DeHierarchyComponent.CustomizedItem(go, hColor));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002D2C File Offset: 0x00000F2C
		public void StoreItemIcon(GameObject go, DeHierarchyComponent.IcoType icoType)
		{
			for (int i = 0; i < this.customizedItems.Count; i++)
			{
				if (!(this.customizedItems[i].gameObject != go))
				{
					this.customizedItems[i].icoType = icoType;
					return;
				}
			}
			this.customizedItems.Add(new DeHierarchyComponent.CustomizedItem(go, icoType));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002D90 File Offset: 0x00000F90
		public void StoreItemSeparator(GameObject go, DeHierarchyComponent.SeparatorType? separatorType, DeHierarchyComponent.HColor? separatorHColor)
		{
			for (int i = 0; i < this.customizedItems.Count; i++)
			{
				if (!(this.customizedItems[i].gameObject != go))
				{
					if (separatorType != null)
					{
						this.customizedItems[i].separatorType = separatorType.Value;
					}
					if (separatorHColor != null)
					{
						this.customizedItems[i].separatorHColor = separatorHColor.Value;
					}
					return;
				}
			}
			this.customizedItems.Add(new DeHierarchyComponent.CustomizedItem(go, (separatorType == null) ? DeHierarchyComponent.SeparatorType.None : separatorType.Value, (separatorHColor == null) ? DeHierarchyComponent.HColor.None : separatorHColor.Value));
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002E48 File Offset: 0x00001048
		public bool RemoveItemData(GameObject go)
		{
			int num = -1;
			for (int i = 0; i < this.customizedItems.Count; i++)
			{
				if (this.customizedItems[i].gameObject == go)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				this.customizedItems.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002EA0 File Offset: 0x000010A0
		public bool ResetSeparator(GameObject go)
		{
			for (int i = 0; i < this.customizedItems.Count; i++)
			{
				if (!(this.customizedItems[i].gameObject != go))
				{
					this.customizedItems[i].separatorType = DeHierarchyComponent.SeparatorType.None;
					this.customizedItems[i].separatorHColor = DeHierarchyComponent.HColor.None;
					if (this.customizedItems[i].hColor == DeHierarchyComponent.HColor.None)
					{
						this.customizedItems.RemoveAt(i);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002F24 File Offset: 0x00001124
		public DeHierarchyComponent.CustomizedItem GetItem(GameObject go)
		{
			for (int i = 0; i < this.customizedItems.Count; i++)
			{
				if (this.customizedItems[i].gameObject == go)
				{
					return this.customizedItems[i];
				}
			}
			return null;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002F70 File Offset: 0x00001170
		public static Color GetColor(DeHierarchyComponent.HColor color)
		{
			switch (color)
			{
			case DeHierarchyComponent.HColor.Blue:
				return new Color(0.21f, 0.62f, 1f);
			case DeHierarchyComponent.HColor.Green:
				return new Color(0.05060553f, 0.8602941f, 0.2237113f, 1f);
			case DeHierarchyComponent.HColor.Orange:
				return new Color(1f, 0.44f, 0f);
			case DeHierarchyComponent.HColor.Purple:
				return new Color(0.64f, 0.27f, 1f);
			case DeHierarchyComponent.HColor.Red:
				return new Color(0.82f, 0f, 0f);
			case DeHierarchyComponent.HColor.Yellow:
				return new Color(0.99f, 0.84f, 0.12f);
			case DeHierarchyComponent.HColor.BrightGrey:
				return new Color(0.6470588f, 0.6470588f, 0.6470588f, 1f);
			case DeHierarchyComponent.HColor.DarkGrey:
				return new Color(0.3308824f, 0.3308824f, 0.3308824f, 1f);
			case DeHierarchyComponent.HColor.Black:
				return Color.black;
			case DeHierarchyComponent.HColor.White:
				return Color.white;
			case DeHierarchyComponent.HColor.Pink:
				return new Color(1f, 0.21f, 0.82f);
			default:
				return Color.white;
			}
		}

		// Token: 0x04000039 RID: 57
		public List<DeHierarchyComponent.CustomizedItem> customizedItems = new List<DeHierarchyComponent.CustomizedItem>();

		// Token: 0x02000013 RID: 19
		public enum HColor
		{
			// Token: 0x04000041 RID: 65
			None,
			// Token: 0x04000042 RID: 66
			Blue,
			// Token: 0x04000043 RID: 67
			Green,
			// Token: 0x04000044 RID: 68
			Orange,
			// Token: 0x04000045 RID: 69
			Purple,
			// Token: 0x04000046 RID: 70
			Red,
			// Token: 0x04000047 RID: 71
			Yellow,
			// Token: 0x04000048 RID: 72
			BrightGrey,
			// Token: 0x04000049 RID: 73
			DarkGrey,
			// Token: 0x0400004A RID: 74
			Black,
			// Token: 0x0400004B RID: 75
			White,
			// Token: 0x0400004C RID: 76
			Pink
		}

		// Token: 0x02000014 RID: 20
		public enum IcoType
		{
			// Token: 0x0400004E RID: 78
			Dot,
			// Token: 0x0400004F RID: 79
			Star,
			// Token: 0x04000050 RID: 80
			Cog,
			// Token: 0x04000051 RID: 81
			Comment,
			// Token: 0x04000052 RID: 82
			UI,
			// Token: 0x04000053 RID: 83
			Play,
			// Token: 0x04000054 RID: 84
			Heart,
			// Token: 0x04000055 RID: 85
			Skull,
			// Token: 0x04000056 RID: 86
			Camera,
			// Token: 0x04000057 RID: 87
			Light
		}

		// Token: 0x02000015 RID: 21
		public enum SeparatorType
		{
			// Token: 0x04000059 RID: 89
			None,
			// Token: 0x0400005A RID: 90
			Top,
			// Token: 0x0400005B RID: 91
			Bottom
		}

		// Token: 0x02000016 RID: 22
		[Serializable]
		public class CustomizedItem
		{
			// Token: 0x0600002D RID: 45 RVA: 0x000030CB File Offset: 0x000012CB
			public CustomizedItem(GameObject gameObject, DeHierarchyComponent.HColor hColor)
			{
				this.gameObject = gameObject;
				this.hColor = hColor;
			}

			// Token: 0x0600002E RID: 46 RVA: 0x000030F0 File Offset: 0x000012F0
			public CustomizedItem(GameObject gameObject, DeHierarchyComponent.IcoType icoType)
			{
				this.gameObject = gameObject;
				this.icoType = icoType;
			}

			// Token: 0x0600002F RID: 47 RVA: 0x00003115 File Offset: 0x00001315
			public CustomizedItem(GameObject gameObject, DeHierarchyComponent.SeparatorType separatorType, DeHierarchyComponent.HColor separatorHColor)
			{
				this.gameObject = gameObject;
				this.separatorType = separatorType;
				this.separatorHColor = separatorHColor;
			}

			// Token: 0x06000030 RID: 48 RVA: 0x00003141 File Offset: 0x00001341
			public Color GetColor()
			{
				return DeHierarchyComponent.GetColor(this.hColor);
			}

			// Token: 0x06000031 RID: 49 RVA: 0x0000314E File Offset: 0x0000134E
			public Color GetSeparatorColor()
			{
				return DeHierarchyComponent.GetColor(this.separatorHColor);
			}

			// Token: 0x0400005C RID: 92
			public GameObject gameObject;

			// Token: 0x0400005D RID: 93
			public DeHierarchyComponent.HColor hColor = DeHierarchyComponent.HColor.BrightGrey;

			// Token: 0x0400005E RID: 94
			public DeHierarchyComponent.IcoType icoType;

			// Token: 0x0400005F RID: 95
			public DeHierarchyComponent.SeparatorType separatorType;

			// Token: 0x04000060 RID: 96
			public DeHierarchyComponent.HColor separatorHColor = DeHierarchyComponent.HColor.Black;
		}
	}
}
