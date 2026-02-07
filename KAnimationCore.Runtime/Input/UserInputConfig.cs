using System;
using System.Collections.Generic;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Input
{
	// Token: 0x02000013 RID: 19
	[CreateAssetMenu(fileName = "NewInputConfig", menuName = "KINEMATION/Input Config")]
	public class UserInputConfig : ScriptableObject
	{
		// Token: 0x04000029 RID: 41
		public List<IntProperty> intProperties = new List<IntProperty>();

		// Token: 0x0400002A RID: 42
		public List<FloatProperty> floatProperties = new List<FloatProperty>();

		// Token: 0x0400002B RID: 43
		public List<BoolProperty> boolProperties = new List<BoolProperty>();

		// Token: 0x0400002C RID: 44
		public List<VectorProperty> vectorProperties = new List<VectorProperty>();
	}
}
