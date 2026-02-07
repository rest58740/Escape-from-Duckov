using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000427 RID: 1063
	public class Dash : ActionTask<AICharacterController>
	{
		// Token: 0x06002689 RID: 9865 RVA: 0x000854E9 File Offset: 0x000836E9
		protected override string OnInit()
		{
			this.dashTimeSpace = Random.Range(this.dashTimeSpaceRange.value.x, this.dashTimeSpaceRange.value.y);
			return null;
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x0600268A RID: 9866 RVA: 0x00085517 File Offset: 0x00083717
		protected override string info
		{
			get
			{
				return string.Format("Dash", Array.Empty<object>());
			}
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x00085528 File Offset: 0x00083728
		protected override void OnExecute()
		{
			if (Time.time - this.lastDashTime < this.dashTimeSpace)
			{
				base.EndAction();
				return;
			}
			this.lastDashTime = Time.time;
			this.dashTimeSpace = Random.Range(this.dashTimeSpaceRange.value.x, this.dashTimeSpaceRange.value.y);
			Vector3 vector = Vector3.forward;
			Dash.DashDirectionModes dashDirectionModes = this.directionMode;
			if (dashDirectionModes != Dash.DashDirectionModes.random)
			{
				if (dashDirectionModes == Dash.DashDirectionModes.targetTransform)
				{
					if (this.targetTransform.value == null)
					{
						base.EndAction();
						return;
					}
					vector = this.targetTransform.value.position - base.agent.transform.position;
					vector.y = 0f;
					vector.Normalize();
					if (this.verticle)
					{
						vector = Vector3.Cross(vector, Vector3.up) * ((Random.Range(0f, 1f) > 0.5f) ? 1f : -1f);
					}
				}
			}
			else
			{
				vector = Random.insideUnitCircle;
				vector.z = vector.y;
				vector.y = 0f;
				vector.Normalize();
			}
			base.agent.CharacterMainControl.SetMoveInput(vector);
			base.agent.CharacterMainControl.Dash();
			base.EndAction(true);
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x00085685 File Offset: 0x00083885
		protected override void OnStop()
		{
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x00085687 File Offset: 0x00083887
		protected override void OnPause()
		{
		}

		// Token: 0x04001A3C RID: 6716
		public Dash.DashDirectionModes directionMode;

		// Token: 0x04001A3D RID: 6717
		[ShowIf("directionMode", 1)]
		public BBParameter<Transform> targetTransform;

		// Token: 0x04001A3E RID: 6718
		[ShowIf("directionMode", 1)]
		public bool verticle;

		// Token: 0x04001A3F RID: 6719
		public BBParameter<Vector2> dashTimeSpaceRange;

		// Token: 0x04001A40 RID: 6720
		private float dashTimeSpace;

		// Token: 0x04001A41 RID: 6721
		private float lastDashTime = -999f;

		// Token: 0x0200069A RID: 1690
		public enum DashDirectionModes
		{
			// Token: 0x04002439 RID: 9273
			random,
			// Token: 0x0400243A RID: 9274
			targetTransform
		}
	}
}
