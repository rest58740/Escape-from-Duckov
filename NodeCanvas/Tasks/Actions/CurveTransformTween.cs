using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000A9 RID: 169
	[Name("Curve Tween", 0)]
	[Category("Movement/Direct")]
	public class CurveTransformTween : ActionTask<Transform>
	{
		// Token: 0x060002C9 RID: 713 RVA: 0x0000AC34 File Offset: 0x00008E34
		protected override void OnExecute()
		{
			if (this.ponging)
			{
				this.final = this.original;
			}
			if (this.transformMode == CurveTransformTween.TransformMode.Position)
			{
				this.original = base.agent.localPosition;
			}
			if (this.transformMode == CurveTransformTween.TransformMode.Rotation)
			{
				this.original = base.agent.localEulerAngles;
			}
			if (this.transformMode == CurveTransformTween.TransformMode.Scale)
			{
				this.original = base.agent.localScale;
			}
			if (!this.ponging)
			{
				this.final = this.targetPosition.value + ((this.mode == CurveTransformTween.TweenMode.Additive) ? this.original : Vector3.zero);
			}
			this.ponging = (this.playMode == CurveTransformTween.PlayMode.PingPong);
			if ((this.original - this.final).magnitude < 0.1f)
			{
				base.EndAction();
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000AD0C File Offset: 0x00008F0C
		protected override void OnUpdate()
		{
			Vector3 vector = Vector3.Lerp(this.original, this.final, this.curve.value.Evaluate(base.elapsedTime / this.time.value));
			if (this.transformMode == CurveTransformTween.TransformMode.Position)
			{
				base.agent.localPosition = vector;
			}
			if (this.transformMode == CurveTransformTween.TransformMode.Rotation)
			{
				base.agent.localEulerAngles = vector;
			}
			if (this.transformMode == CurveTransformTween.TransformMode.Scale)
			{
				base.agent.localScale = vector;
			}
			if (base.elapsedTime >= this.time.value)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x040001E2 RID: 482
		public CurveTransformTween.TransformMode transformMode;

		// Token: 0x040001E3 RID: 483
		public CurveTransformTween.TweenMode mode;

		// Token: 0x040001E4 RID: 484
		public CurveTransformTween.PlayMode playMode;

		// Token: 0x040001E5 RID: 485
		public BBParameter<Vector3> targetPosition;

		// Token: 0x040001E6 RID: 486
		public BBParameter<AnimationCurve> curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x040001E7 RID: 487
		public BBParameter<float> time = 0.5f;

		// Token: 0x040001E8 RID: 488
		private Vector3 original;

		// Token: 0x040001E9 RID: 489
		private Vector3 final;

		// Token: 0x040001EA RID: 490
		private bool ponging;

		// Token: 0x0200013D RID: 317
		public enum TransformMode
		{
			// Token: 0x0400038F RID: 911
			Position,
			// Token: 0x04000390 RID: 912
			Rotation,
			// Token: 0x04000391 RID: 913
			Scale
		}

		// Token: 0x0200013E RID: 318
		public enum TweenMode
		{
			// Token: 0x04000393 RID: 915
			Absolute,
			// Token: 0x04000394 RID: 916
			Additive
		}

		// Token: 0x0200013F RID: 319
		public enum PlayMode
		{
			// Token: 0x04000396 RID: 918
			Normal,
			// Token: 0x04000397 RID: 919
			PingPong
		}
	}
}
