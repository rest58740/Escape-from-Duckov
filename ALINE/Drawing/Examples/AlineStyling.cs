using System;
using Unity.Mathematics;
using UnityEngine;

namespace Drawing.Examples
{
	// Token: 0x02000068 RID: 104
	public class AlineStyling : MonoBehaviour
	{
		// Token: 0x060003E1 RID: 993 RVA: 0x00013338 File Offset: 0x00011538
		private unsafe void Update()
		{
			CommandBuilder commandBuilder = *Draw.ingame;
			using (commandBuilder.InScreenSpace(Camera.main))
			{
				using (commandBuilder.WithMatrix(Matrix4x4.TRS(new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f), Quaternion.identity, new Vector3((float)Screen.width, (float)Screen.width, 1f))))
				{
					for (int i = 0; i < 4; i++)
					{
						using (commandBuilder.WithLineWidth((float)(i * i + 1), true))
						{
							float x = 0.7853982f * (float)(i + 1) + Time.time * (float)i;
							Vector3 vector = new Vector3(-0.3f + (float)i * 0.2f, 0f, 0f);
							float num = 0.075f;
							commandBuilder.Line(vector + new Vector3(math.cos(x) * num, math.sin(x) * num, 0f), vector, this.gizmoColor);
							commandBuilder.Line(vector, vector + new Vector3(num, 0f, 0f), this.gizmoColor);
							commandBuilder.xy.Circle(vector, num, this.gizmoColor2);
						}
					}
				}
			}
		}

		// Token: 0x040001A0 RID: 416
		public Color gizmoColor = new Color(1f, 0.34509805f, 0.33333334f);

		// Token: 0x040001A1 RID: 417
		public Color gizmoColor2 = new Color(0.30980393f, 0.8f, 0.92941177f);
	}
}
