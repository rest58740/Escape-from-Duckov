using System;
using Unity.Collections;
using UnityEngine.Animations;

namespace Animancer
{
	// Token: 0x0200003E RID: 62
	public class AnimatedFloat : AnimatedProperty<AnimatedFloat.Job, float>
	{
		// Token: 0x06000418 RID: 1048 RVA: 0x0000B63E File Offset: 0x0000983E
		public AnimatedFloat(IAnimancerComponent animancer, int propertyCount, NativeArrayOptions options = NativeArrayOptions.ClearMemory) : base(animancer, propertyCount, options)
		{
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000B649 File Offset: 0x00009849
		public AnimatedFloat(IAnimancerComponent animancer, string propertyName) : base(animancer, propertyName)
		{
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000B653 File Offset: 0x00009853
		public AnimatedFloat(IAnimancerComponent animancer, params string[] propertyNames) : base(animancer, propertyNames)
		{
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000B660 File Offset: 0x00009860
		protected override void CreateJob()
		{
			this._Job = new AnimatedFloat.Job
			{
				properties = this._Properties,
				values = this._Values
			};
		}

		// Token: 0x02000094 RID: 148
		public struct Job : IAnimationJob
		{
			// Token: 0x06000667 RID: 1639 RVA: 0x00011426 File Offset: 0x0000F626
			public void ProcessRootMotion(AnimationStream stream)
			{
			}

			// Token: 0x06000668 RID: 1640 RVA: 0x00011428 File Offset: 0x0000F628
			public void ProcessAnimation(AnimationStream stream)
			{
				for (int i = this.properties.Length - 1; i >= 0; i--)
				{
					this.values[i] = this.properties[i].GetFloat(stream);
				}
			}

			// Token: 0x0400014F RID: 335
			public NativeArray<PropertyStreamHandle> properties;

			// Token: 0x04000150 RID: 336
			public NativeArray<float> values;
		}
	}
}
