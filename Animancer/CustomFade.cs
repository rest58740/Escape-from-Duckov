using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000041 RID: 65
	public abstract class CustomFade : Key, IUpdatable, Key.IListItem
	{
		// Token: 0x0600042F RID: 1071 RVA: 0x0000B8B8 File Offset: 0x00009AB8
		protected void Apply(AnimancerState state)
		{
			this.Apply(state);
			IPlayableWrapper parent = state.Parent;
			for (int i = parent.ChildCount - 1; i >= 0; i--)
			{
				AnimancerNode child = parent.GetChild(i);
				if (child != state && child.FadeSpeed != 0f)
				{
					child.FadeSpeed = 0f;
					this.FadeOutNodes.Add(new CustomFade.NodeWeight(child));
				}
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000B91C File Offset: 0x00009B1C
		protected void Apply(AnimancerNode node)
		{
			this._Time = 0f;
			this._Target = new CustomFade.NodeWeight(node);
			this._FadeSpeed = node.FadeSpeed;
			this._Layer = node.Layer;
			this._CommandCount = this._Layer.CommandCount;
			node.FadeSpeed = 0f;
			this.FadeOutNodes.Clear();
			node.Root.RequirePreUpdate(this);
		}

		// Token: 0x06000431 RID: 1073
		protected abstract float CalculateWeight(float progress);

		// Token: 0x06000432 RID: 1074
		protected abstract void Release();

		// Token: 0x06000433 RID: 1075 RVA: 0x0000B98C File Offset: 0x00009B8C
		void IUpdatable.Update()
		{
			if (!this._Target.Node.IsValid() || this._Layer != this._Target.Node.Layer || this._CommandCount != this._Layer.CommandCount)
			{
				this.FadeOutNodes.Clear();
				this._Layer.Root.CancelPreUpdate(this);
				this.Release();
				return;
			}
			this._Time += AnimancerPlayable.DeltaTime * this._Layer.Speed * this._FadeSpeed;
			if (this._Time < 1f)
			{
				float num = this.CalculateWeight(this._Time);
				this._Target.Node.SetWeight(Mathf.LerpUnclamped(this._Target.StartingWeight, this._Target.Node.TargetWeight, num));
				this._Target.Node.ApplyWeight();
				num = 1f - num;
				for (int i = this.FadeOutNodes.Count - 1; i >= 0; i--)
				{
					CustomFade.NodeWeight nodeWeight = this.FadeOutNodes[i];
					nodeWeight.Node.SetWeight(nodeWeight.StartingWeight * num);
					nodeWeight.Node.ApplyWeight();
				}
				return;
			}
			this._Time = 1f;
			CustomFade.ForceFinishFade(this._Target.Node);
			for (int j = this.FadeOutNodes.Count - 1; j >= 0; j--)
			{
				CustomFade.ForceFinishFade(this.FadeOutNodes[j].Node);
			}
			this.FadeOutNodes.Clear();
			this._Layer.Root.CancelPreUpdate(this);
			this.Release();
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000BB34 File Offset: 0x00009D34
		private static void ForceFinishFade(AnimancerNode node)
		{
			float targetWeight = node.TargetWeight;
			node.SetWeight(targetWeight);
			node.ApplyWeight();
			if (targetWeight == 0f)
			{
				node.Stop();
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000BB63 File Offset: 0x00009D63
		public static void Apply(AnimancerComponent animancer, AnimationCurve curve)
		{
			CustomFade.Apply(animancer.States.Current, curve);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000BB76 File Offset: 0x00009D76
		public static void Apply(AnimancerPlayable animancer, AnimationCurve curve)
		{
			CustomFade.Apply(animancer.States.Current, curve);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000BB89 File Offset: 0x00009D89
		public static void Apply(AnimancerState state, AnimationCurve curve)
		{
			CustomFade.Curve.Acquire(curve).Apply(state);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000BB97 File Offset: 0x00009D97
		public static void Apply(AnimancerNode node, AnimationCurve curve)
		{
			CustomFade.Curve.Acquire(curve).Apply(node);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000BBA5 File Offset: 0x00009DA5
		public static void Apply(AnimancerComponent animancer, Func<float, float> calculateWeight)
		{
			CustomFade.Apply(animancer.States.Current, calculateWeight);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000BBB8 File Offset: 0x00009DB8
		public static void Apply(AnimancerPlayable animancer, Func<float, float> calculateWeight)
		{
			CustomFade.Apply(animancer.States.Current, calculateWeight);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000BBCB File Offset: 0x00009DCB
		public static void Apply(AnimancerState state, Func<float, float> calculateWeight)
		{
			CustomFade.Delegate.Acquire(calculateWeight).Apply(state);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000BBD9 File Offset: 0x00009DD9
		public static void Apply(AnimancerNode node, Func<float, float> calculateWeight)
		{
			CustomFade.Delegate.Acquire(calculateWeight).Apply(node);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000BBE7 File Offset: 0x00009DE7
		public static void Apply(AnimancerComponent animancer, Easing.Function function)
		{
			CustomFade.Apply(animancer.States.Current, function);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000BBFA File Offset: 0x00009DFA
		public static void Apply(AnimancerPlayable animancer, Easing.Function function)
		{
			CustomFade.Apply(animancer.States.Current, function);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000BC0D File Offset: 0x00009E0D
		public static void Apply(AnimancerState state, Easing.Function function)
		{
			CustomFade.Delegate.Acquire(function.GetDelegate()).Apply(state);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000BC20 File Offset: 0x00009E20
		public static void Apply(AnimancerNode node, Easing.Function function)
		{
			CustomFade.Delegate.Acquire(function.GetDelegate()).Apply(node);
		}

		// Token: 0x040000AC RID: 172
		private float _Time;

		// Token: 0x040000AD RID: 173
		private float _FadeSpeed;

		// Token: 0x040000AE RID: 174
		private CustomFade.NodeWeight _Target;

		// Token: 0x040000AF RID: 175
		private AnimancerLayer _Layer;

		// Token: 0x040000B0 RID: 176
		private int _CommandCount;

		// Token: 0x040000B1 RID: 177
		private readonly List<CustomFade.NodeWeight> FadeOutNodes = new List<CustomFade.NodeWeight>();

		// Token: 0x02000096 RID: 150
		private readonly struct NodeWeight
		{
			// Token: 0x0600066B RID: 1643 RVA: 0x000114B6 File Offset: 0x0000F6B6
			public NodeWeight(AnimancerNode node)
			{
				this.Node = node;
				this.StartingWeight = node.Weight;
			}

			// Token: 0x04000153 RID: 339
			public readonly AnimancerNode Node;

			// Token: 0x04000154 RID: 340
			public readonly float StartingWeight;
		}

		// Token: 0x02000097 RID: 151
		private class Curve : CustomFade
		{
			// Token: 0x0600066C RID: 1644 RVA: 0x000114CB File Offset: 0x0000F6CB
			public static CustomFade.Curve Acquire(AnimationCurve curve)
			{
				if (curve == null)
				{
					return null;
				}
				CustomFade.Curve curve2 = ObjectPool<CustomFade.Curve>.Acquire();
				curve2._Curve = curve;
				return curve2;
			}

			// Token: 0x0600066D RID: 1645 RVA: 0x000114DE File Offset: 0x0000F6DE
			protected override float CalculateWeight(float progress)
			{
				return this._Curve.Evaluate(progress);
			}

			// Token: 0x0600066E RID: 1646 RVA: 0x000114EC File Offset: 0x0000F6EC
			protected override void Release()
			{
				ObjectPool<CustomFade.Curve>.Release(this);
			}

			// Token: 0x04000155 RID: 341
			private AnimationCurve _Curve;
		}

		// Token: 0x02000098 RID: 152
		private class Delegate : CustomFade
		{
			// Token: 0x06000670 RID: 1648 RVA: 0x000114FC File Offset: 0x0000F6FC
			public static CustomFade.Delegate Acquire(Func<float, float> calculateWeight)
			{
				if (calculateWeight == null)
				{
					return null;
				}
				CustomFade.Delegate @delegate = ObjectPool<CustomFade.Delegate>.Acquire();
				@delegate._CalculateWeight = calculateWeight;
				return @delegate;
			}

			// Token: 0x06000671 RID: 1649 RVA: 0x0001150F File Offset: 0x0000F70F
			protected override float CalculateWeight(float progress)
			{
				return this._CalculateWeight(progress);
			}

			// Token: 0x06000672 RID: 1650 RVA: 0x0001151D File Offset: 0x0000F71D
			protected override void Release()
			{
				ObjectPool<CustomFade.Delegate>.Release(this);
			}

			// Token: 0x04000156 RID: 342
			private Func<float, float> _CalculateWeight;
		}
	}
}
