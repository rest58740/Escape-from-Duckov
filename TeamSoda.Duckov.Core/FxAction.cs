using System;
using ItemStatsSystem;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class FxAction : EffectAction
{
	// Token: 0x17000103 RID: 259
	// (get) Token: 0x060004DB RID: 1243 RVA: 0x00016453 File Offset: 0x00014653
	private CharacterMainControl MainControl
	{
		get
		{
			if (this._mainControl == null)
			{
				Effect master = base.Master;
				CharacterMainControl mainControl;
				if (master == null)
				{
					mainControl = null;
				}
				else
				{
					Item item = master.Item;
					mainControl = ((item != null) ? item.GetCharacterMainControl() : null);
				}
				this._mainControl = mainControl;
			}
			return this._mainControl;
		}
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00016490 File Offset: 0x00014690
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl || !this.MainControl.characterModel)
		{
			return;
		}
		Transform transform = this.MainControl.transform;
		switch (this.socket)
		{
		case FxAction.Sockets.root:
			break;
		case FxAction.Sockets.helmat:
			transform = this.MainControl.characterModel.HelmatSocket;
			break;
		case FxAction.Sockets.armor:
			transform = this.MainControl.characterModel.ArmorSocket;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (!transform)
		{
			return;
		}
		if (!this.fxPfb)
		{
			return;
		}
		Object.Instantiate<GameObject>(this.fxPfb, transform.position, quaternion.identity);
	}

	// Token: 0x04000419 RID: 1049
	public FxAction.Sockets socket = FxAction.Sockets.helmat;

	// Token: 0x0400041A RID: 1050
	public GameObject fxPfb;

	// Token: 0x0400041B RID: 1051
	private CharacterMainControl _mainControl;

	// Token: 0x0200045A RID: 1114
	public enum Sockets
	{
		// Token: 0x04001B3E RID: 6974
		root,
		// Token: 0x04001B3F RID: 6975
		helmat,
		// Token: 0x04001B40 RID: 6976
		armor
	}
}
