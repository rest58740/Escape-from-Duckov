using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000055 RID: 85
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[DontApplyToListElements]
	[Conditional("UNITY_EDITOR")]
	public class PropertySpaceAttribute : Attribute
	{
		// Token: 0x06000126 RID: 294 RVA: 0x000034D1 File Offset: 0x000016D1
		public PropertySpaceAttribute()
		{
			this.SpaceBefore = 8f;
			this.SpaceAfter = 0f;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000034EF File Offset: 0x000016EF
		public PropertySpaceAttribute(float spaceBefore)
		{
			this.SpaceBefore = spaceBefore;
			this.SpaceAfter = 0f;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00003509 File Offset: 0x00001709
		public PropertySpaceAttribute(float spaceBefore, float spaceAfter)
		{
			this.SpaceBefore = spaceBefore;
			this.SpaceAfter = spaceAfter;
		}

		// Token: 0x040000F1 RID: 241
		public float SpaceBefore;

		// Token: 0x040000F2 RID: 242
		public float SpaceAfter;
	}
}
