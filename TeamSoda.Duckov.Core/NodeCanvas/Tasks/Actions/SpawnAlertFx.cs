using System;
using Duckov.Utilities;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000435 RID: 1077
	public class SpawnAlertFx : ActionTask<AICharacterController>
	{
		// Token: 0x060026E2 RID: 9954 RVA: 0x00086745 File Offset: 0x00084945
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060026E3 RID: 9955 RVA: 0x00086748 File Offset: 0x00084948
		protected override string info
		{
			get
			{
				return string.Format("AlertFx", Array.Empty<object>());
			}
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x0008675C File Offset: 0x0008495C
		protected override void OnExecute()
		{
			if (!base.agent || !base.agent.CharacterMainControl)
			{
				base.EndAction(true);
			}
			Transform rightHandSocket = base.agent.CharacterMainControl.RightHandSocket;
			if (!rightHandSocket)
			{
				base.EndAction(true);
			}
			Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.AlertFxPrefab, rightHandSocket).transform.localPosition = Vector3.zero;
			if (this.alertTime.value <= 0f)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000867E7 File Offset: 0x000849E7
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= this.alertTime.value)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x00086803 File Offset: 0x00084A03
		protected override void OnStop()
		{
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x00086805 File Offset: 0x00084A05
		protected override void OnPause()
		{
		}

		// Token: 0x04001A78 RID: 6776
		public BBParameter<float> alertTime = 0.2f;
	}
}
