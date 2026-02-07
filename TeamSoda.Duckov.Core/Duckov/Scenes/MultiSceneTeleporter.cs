using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.Scenes
{
	// Token: 0x02000345 RID: 837
	public class MultiSceneTeleporter : InteractableBase
	{
		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001C89 RID: 7305 RVA: 0x000682E9 File Offset: 0x000664E9
		protected override bool ShowUnityEvents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001C8A RID: 7306 RVA: 0x000682EC File Offset: 0x000664EC
		[SerializeField]
		public MultiSceneLocation Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001C8B RID: 7307 RVA: 0x000682F4 File Offset: 0x000664F4
		private static float TimeSinceTeleportFinished
		{
			get
			{
				return Time.time - MultiSceneTeleporter.timeWhenTeleportFinished;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001C8C RID: 7308 RVA: 0x00068301 File Offset: 0x00066501
		private static bool Teleportable
		{
			get
			{
				return MultiSceneTeleporter.TimeSinceTeleportFinished > 1f;
			}
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x0006830F File Offset: 0x0006650F
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x00068318 File Offset: 0x00066518
		private void OnDrawGizmosSelected()
		{
			Transform locationTransform = this.target.LocationTransform;
			if (locationTransform)
			{
				Gizmos.DrawLine(base.transform.position, locationTransform.position);
				Gizmos.DrawWireSphere(locationTransform.position, 0.25f);
			}
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x0006835F File Offset: 0x0006655F
		public void DoTeleport()
		{
			if (!MultiSceneTeleporter.Teleportable)
			{
				Debug.Log("not Teleportable");
				return;
			}
			this.TeleportTask().Forget();
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x0006837E File Offset: 0x0006657E
		protected override bool IsInteractable()
		{
			return MultiSceneTeleporter.Teleportable;
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x00068388 File Offset: 0x00066588
		private UniTask TeleportTask()
		{
			MultiSceneTeleporter.<TeleportTask>d__16 <TeleportTask>d__;
			<TeleportTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<TeleportTask>d__.<>4__this = this;
			<TeleportTask>d__.<>1__state = -1;
			<TeleportTask>d__.<>t__builder.Start<MultiSceneTeleporter.<TeleportTask>d__16>(ref <TeleportTask>d__);
			return <TeleportTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x000683CB File Offset: 0x000665CB
		protected override void OnInteractStart(CharacterMainControl interactCharacter)
		{
			this.coolTime = 2f;
			this.finishWhenTimeOut = true;
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x000683DF File Offset: 0x000665DF
		protected override void OnInteractFinished()
		{
			this.DoTeleport();
			base.StopInteract();
		}

		// Token: 0x04001439 RID: 5177
		[SerializeField]
		private MultiSceneLocation target;

		// Token: 0x0400143A RID: 5178
		[SerializeField]
		private bool teleportOnTriggerEnter;

		// Token: 0x0400143B RID: 5179
		private const float CoolDown = 1f;

		// Token: 0x0400143C RID: 5180
		private static float timeWhenTeleportFinished;
	}
}
