using System;
using Unity.Collections;
using UnityEngine.Animations;

namespace Animancer
{
	// Token: 0x0200003D RID: 61
	public class AnimatedBool : AnimatedProperty<AnimatedBool.Job, bool>
	{
		// Token: 0x06000414 RID: 1044 RVA: 0x0000B5E6 File Offset: 0x000097E6
		public AnimatedBool(IAnimancerComponent animancer, int propertyCount, NativeArrayOptions options = NativeArrayOptions.ClearMemory) : base(animancer, propertyCount, options)
		{
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000B5F1 File Offset: 0x000097F1
		public AnimatedBool(IAnimancerComponent animancer, string propertyName) : base(animancer, propertyName)
		{
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000B5FB File Offset: 0x000097FB
		public AnimatedBool(IAnimancerComponent animancer, params string[] propertyNames) : base(animancer, propertyNames)
		{
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000B608 File Offset: 0x00009808
		protected override void CreateJob()
		{
			this._Job = new AnimatedBool.Job
			{
				properties = this._Properties,
				values = this._Values
			};
		}

		// Token: 0x02000093 RID: 147
		public struct Job : IAnimationJob
		{
			// Token: 0x06000665 RID: 1637 RVA: 0x000113DD File Offset: 0x0000F5DD
			public void ProcessRootMotion(AnimationStream stream)
			{
			}

			// Token: 0x06000666 RID: 1638 RVA: 0x000113E0 File Offset: 0x0000F5E0
			public void ProcessAnimation(AnimationStream stream)
			{
				for (int i = this.properties.Length - 1; i >= 0; i--)
				{
					this.values[i] = this.properties[i].GetBool(stream);
				}
			}

			// Token: 0x0400014D RID: 333
			public NativeArray<PropertyStreamHandle> properties;

			// Token: 0x0400014E RID: 334
			public NativeArray<bool> values;
		}
	}
}
