using System;
using UnityEngine;

namespace Drawing.Examples
{
	// Token: 0x0200005E RID: 94
	public class GizmoCharacterExample : MonoBehaviourGizmos
	{
		// Token: 0x060003BE RID: 958 RVA: 0x000127E8 File Offset: 0x000109E8
		private void Start()
		{
			this.seed = UnityEngine.Random.value * 1000f;
			this.startPosition = base.transform.position;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0001280C File Offset: 0x00010A0C
		private Vector3 GetSmoothRandomVelocity(float time, Vector3 position)
		{
			float num = time * this.movementNoiseScale + this.seed;
			float x = 2f * Mathf.PerlinNoise(num, num + 5341.2314f) - 1f;
			float z = 2f * Mathf.PerlinNoise(num + 92.9842f, -num + 231.85146f) - 1f;
			Vector3 vector = new Vector3(x, 0f, z);
			vector += (this.startPosition - position) * this.startPointAttractionStrength;
			vector.y = 0f;
			return vector;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000128A0 File Offset: 0x00010AA0
		private void PlotFuturePath(float time, Vector3 position)
		{
			float num = 0.05f;
			for (int i = 0; i < this.futurePathPlotSteps; i++)
			{
				Vector3 smoothRandomVelocity = this.GetSmoothRandomVelocity(time + (float)i * num, position);
				int num2 = i - this.plotStartStep;
				if (num2 >= 0 && num2 % this.plotEveryNSteps == 0)
				{
					Draw.Arrowhead(position, smoothRandomVelocity, 0.1f, this.gizmoColor);
				}
				position += smoothRandomVelocity.normalized * num;
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0001291C File Offset: 0x00010B1C
		private void Update()
		{
			this.PlotFuturePath(Time.time, base.transform.position);
			Vector3 smoothRandomVelocity = this.GetSmoothRandomVelocity(Time.time, base.transform.position);
			base.transform.rotation = Quaternion.LookRotation(smoothRandomVelocity);
			base.transform.position += base.transform.forward * Time.deltaTime;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00012994 File Offset: 0x00010B94
		public override void DrawGizmos()
		{
			using (Draw.InLocalSpace(base.transform))
			{
				Draw.WireCylinder(Vector3.zero, Vector3.up, 2f, 0.5f, this.gizmoColor);
				Draw.ArrowheadArc(Vector3.zero, Vector3.forward, 0.55f, this.gizmoColor);
				Draw.Label2D(Vector3.zero, base.gameObject.name, 14f, LabelAlignment.TopCenter.withPixelOffset(0f, -20f), this.gizmoColor2);
			}
		}

		// Token: 0x0400017E RID: 382
		public Color gizmoColor = new Color(1f, 0.34509805f, 0.33333334f);

		// Token: 0x0400017F RID: 383
		public Color gizmoColor2 = new Color(0.30980393f, 0.8f, 0.92941177f);

		// Token: 0x04000180 RID: 384
		public float movementNoiseScale = 0.2f;

		// Token: 0x04000181 RID: 385
		public float startPointAttractionStrength = 0.05f;

		// Token: 0x04000182 RID: 386
		public int futurePathPlotSteps = 100;

		// Token: 0x04000183 RID: 387
		public int plotStartStep = 10;

		// Token: 0x04000184 RID: 388
		public int plotEveryNSteps = 10;

		// Token: 0x04000185 RID: 389
		private float seed;

		// Token: 0x04000186 RID: 390
		private Vector3 startPosition;
	}
}
