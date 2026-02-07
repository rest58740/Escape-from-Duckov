using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace Animancer
{
	// Token: 0x02000040 RID: 64
	public abstract class AnimatedProperty<TJob, TValue> : AnimancerJob<TJob>, IDisposable where TJob : struct, IAnimationJob where TValue : struct
	{
		// Token: 0x06000420 RID: 1056 RVA: 0x0000B6F0 File Offset: 0x000098F0
		public AnimatedProperty(IAnimancerComponent animancer, int propertyCount, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
		{
			this._Properties = new NativeArray<PropertyStreamHandle>(propertyCount, Allocator.Persistent, options);
			this._Values = new NativeArray<TValue>(propertyCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.CreateJob();
			AnimancerPlayable playable = animancer.Playable;
			base.CreatePlayable(playable);
			playable.Disposables.Add(this);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000B740 File Offset: 0x00009940
		public AnimatedProperty(IAnimancerComponent animancer, string propertyName) : this(animancer, 1, NativeArrayOptions.UninitializedMemory)
		{
			Animator animator = animancer.Animator;
			this._Properties[0] = animator.BindStreamProperty(animator.transform, typeof(Animator), propertyName);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000B780 File Offset: 0x00009980
		public AnimatedProperty(IAnimancerComponent animancer, params string[] propertyNames) : this(animancer, propertyNames.Length, NativeArrayOptions.UninitializedMemory)
		{
			int num = propertyNames.Length;
			Animator animator = animancer.Animator;
			Transform transform = animator.transform;
			for (int i = 0; i < num; i++)
			{
				this.InitializeProperty(animator, i, transform, typeof(Animator), propertyNames[i]);
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000B7CC File Offset: 0x000099CC
		public void InitializeProperty(Animator animator, int index, string name)
		{
			this.InitializeProperty(animator, index, animator.transform, typeof(Animator), name);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000B7E7 File Offset: 0x000099E7
		public void InitializeProperty(Animator animator, int index, Transform transform, Type type, string name)
		{
			this._Properties[index] = animator.BindStreamProperty(transform, type, name);
		}

		// Token: 0x06000425 RID: 1061
		protected abstract void CreateJob();

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0000B800 File Offset: 0x00009A00
		public TValue Value
		{
			get
			{
				return this[0];
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000B809 File Offset: 0x00009A09
		public static implicit operator TValue(AnimatedProperty<TJob, TValue> properties)
		{
			return properties[0];
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000B812 File Offset: 0x00009A12
		public TValue GetValue(int index)
		{
			return this._Values[index];
		}

		// Token: 0x17000105 RID: 261
		public TValue this[int index]
		{
			get
			{
				return this._Values[index];
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000B82E File Offset: 0x00009A2E
		public void GetValues(ref TValue[] values)
		{
			AnimancerUtilities.SetLength<TValue>(ref values, this._Values.Length);
			this._Values.CopyTo(values);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000B850 File Offset: 0x00009A50
		public TValue[] GetValues()
		{
			TValue[] array = new TValue[this._Values.Length];
			this._Values.CopyTo(array);
			return array;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000B87B File Offset: 0x00009A7B
		void IDisposable.Dispose()
		{
			this.Dispose();
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000B883 File Offset: 0x00009A83
		protected virtual void Dispose()
		{
			if (this._Properties.IsCreated)
			{
				this._Properties.Dispose();
				this._Values.Dispose();
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000B8A8 File Offset: 0x00009AA8
		public override void Destroy()
		{
			this.Dispose();
			base.Destroy();
		}

		// Token: 0x040000AA RID: 170
		protected NativeArray<PropertyStreamHandle> _Properties;

		// Token: 0x040000AB RID: 171
		protected NativeArray<TValue> _Values;
	}
}
