using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200000C RID: 12
	[ExecuteAlways]
	public class ShapeGroup : MonoBehaviour
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000568B File Offset: 0x0000388B
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00005693 File Offset: 0x00003893
		internal bool IsEnabled { get; private set; }

		// Token: 0x0600019C RID: 412 RVA: 0x0000569C File Offset: 0x0000389C
		private void OnEnable()
		{
			ShapeGroup.shapeGroupsInScene++;
			this.IsEnabled = true;
			this.UpdateChildShapes();
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000056B7 File Offset: 0x000038B7
		private void OnDisable()
		{
			ShapeGroup.shapeGroupsInScene--;
			this.IsEnabled = false;
			this.UpdateChildShapes();
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600019E RID: 414 RVA: 0x000056D2 File Offset: 0x000038D2
		// (set) Token: 0x0600019F RID: 415 RVA: 0x000056DA File Offset: 0x000038DA
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
				this.UpdateChildShapes();
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000056E9 File Offset: 0x000038E9
		private void OnValidate()
		{
			this.UpdateChildShapes();
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000056F4 File Offset: 0x000038F4
		private void UpdateChildShapes()
		{
			ShapeRenderer[] componentsInChildren = base.GetComponentsInChildren<ShapeRenderer>();
			if (componentsInChildren != null)
			{
				ShapeRenderer[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].UpdateAllMaterialProperties();
				}
			}
		}

		// Token: 0x04000052 RID: 82
		public static int shapeGroupsInScene;

		// Token: 0x04000053 RID: 83
		[ShapesColorField(true)]
		[SerializeField]
		private Color color = Color.white;
	}
}
