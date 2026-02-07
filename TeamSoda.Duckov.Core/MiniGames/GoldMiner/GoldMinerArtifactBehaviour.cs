using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029B RID: 667
	public abstract class GoldMinerArtifactBehaviour : MiniGameBehaviour
	{
		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x000515EA File Offset: 0x0004F7EA
		protected GoldMinerRunData Run
		{
			get
			{
				if (this.master == null)
				{
					return null;
				}
				if (this.master.Master == null)
				{
					return null;
				}
				return this.master.Master.run;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060015D7 RID: 5591 RVA: 0x00051621 File Offset: 0x0004F821
		protected GoldMiner GoldMiner
		{
			get
			{
				if (this.master == null)
				{
					return null;
				}
				return this.master.Master;
			}
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x00051640 File Offset: 0x0004F840
		private void Awake()
		{
			if (!this.master)
			{
				this.master = base.GetComponent<GoldMinerArtifact>();
			}
			GoldMinerArtifact goldMinerArtifact = this.master;
			goldMinerArtifact.OnAttached = (Action<GoldMinerArtifact>)Delegate.Combine(goldMinerArtifact.OnAttached, new Action<GoldMinerArtifact>(this.OnAttached));
			GoldMinerArtifact goldMinerArtifact2 = this.master;
			goldMinerArtifact2.OnDetached = (Action<GoldMinerArtifact>)Delegate.Combine(goldMinerArtifact2.OnDetached, new Action<GoldMinerArtifact>(this.OnDetached));
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x000516B6 File Offset: 0x0004F8B6
		protected virtual void OnAttached(GoldMinerArtifact artifact)
		{
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x000516B8 File Offset: 0x0004F8B8
		protected virtual void OnDetached(GoldMinerArtifact artifact)
		{
		}

		// Token: 0x04001009 RID: 4105
		[SerializeField]
		protected GoldMinerArtifact master;
	}
}
