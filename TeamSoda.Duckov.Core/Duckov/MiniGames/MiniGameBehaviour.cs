using System;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x02000292 RID: 658
	public class MiniGameBehaviour : MonoBehaviour
	{
		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x0600154B RID: 5451 RVA: 0x0004F6A0 File Offset: 0x0004D8A0
		public MiniGame Game
		{
			get
			{
				return this.game;
			}
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0004F6A8 File Offset: 0x0004D8A8
		public void SetGame(MiniGame game = null)
		{
			if (game == null)
			{
				this.game = base.GetComponentInParent<MiniGame>();
				return;
			}
			this.game = game;
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0004F6C7 File Offset: 0x0004D8C7
		private void OnUpdateLogic(MiniGame game, float deltaTime)
		{
			if (this == null)
			{
				return;
			}
			if (!base.enabled)
			{
				return;
			}
			if (game == null)
			{
				return;
			}
			if (game != this.game)
			{
				return;
			}
			this.OnUpdate(deltaTime);
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0004F6FC File Offset: 0x0004D8FC
		protected virtual void OnEnable()
		{
			MiniGame.onUpdateLogic = (Action<MiniGame, float>)Delegate.Combine(MiniGame.onUpdateLogic, new Action<MiniGame, float>(this.OnUpdateLogic));
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0004F71E File Offset: 0x0004D91E
		protected virtual void OnDisable()
		{
			MiniGame.onUpdateLogic = (Action<MiniGame, float>)Delegate.Remove(MiniGame.onUpdateLogic, new Action<MiniGame, float>(this.OnUpdateLogic));
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0004F740 File Offset: 0x0004D940
		private void OnDestroy()
		{
			MiniGame.onUpdateLogic = (Action<MiniGame, float>)Delegate.Remove(MiniGame.onUpdateLogic, new Action<MiniGame, float>(this.OnUpdateLogic));
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0004F762 File Offset: 0x0004D962
		protected virtual void Start()
		{
			if (this.game == null)
			{
				this.SetGame(null);
			}
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x0004F779 File Offset: 0x0004D979
		protected virtual void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x04000F9F RID: 3999
		[SerializeField]
		private MiniGame game;
	}
}
