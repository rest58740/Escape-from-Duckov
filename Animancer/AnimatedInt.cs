using System;
using Unity.Collections;
using UnityEngine.Animations;

namespace Animancer
{
	// Token: 0x0200003F RID: 63
	public class AnimatedInt : AnimatedProperty<AnimatedInt.Job, int>
	{
		// Token: 0x0600041C RID: 1052 RVA: 0x0000B696 File Offset: 0x00009896
		public AnimatedInt(IAnimancerComponent animancer, int propertyCount, NativeArrayOptions options = NativeArrayOptions.ClearMemory) : base(animancer, propertyCount, options)
		{
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000B6A1 File Offset: 0x000098A1
		public AnimatedInt(IAnimancerComponent animancer, string propertyName) : base(animancer, propertyName)
		{
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000B6AB File Offset: 0x000098AB
		public AnimatedInt(IAnimancerComponent animancer, params string[] propertyNames) : base(animancer, propertyNames)
		{
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000B6B8 File Offset: 0x000098B8
		protected override void CreateJob()
		{
			this._Job = new AnimatedInt.Job
			{
				properties = this._Properties,
				values = this._Values
			};
		}

		// Token: 0x02000095 RID: 149
		public struct Job : IAnimationJob
		{
			// Token: 0x06000669 RID: 1641 RVA: 0x0001146E File Offset: 0x0000F66E
			public void ProcessRootMotion(AnimationStream stream)
			{
			}

			// Token: 0x0600066A RID: 1642 RVA: 0x00011470 File Offset: 0x0000F670
			public void ProcessAnimation(AnimationStream stream)
			{
				for (int i = this.properties.Length - 1; i >= 0; i--)
				{
					this.values[i] = this.properties[i].GetInt(stream);
				}
			}

			// Token: 0x04000151 RID: 337
			public NativeArray<PropertyStreamHandle> properties;

			// Token: 0x04000152 RID: 338
			public NativeArray<int> values;
		}
	}
}
