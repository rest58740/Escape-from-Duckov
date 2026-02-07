using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class SpawnPaperBoxAction : EffectAction
{
	// Token: 0x17000106 RID: 262
	// (get) Token: 0x060004E9 RID: 1257 RVA: 0x000167C3 File Offset: 0x000149C3
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

	// Token: 0x060004EA RID: 1258 RVA: 0x00016800 File Offset: 0x00014A00
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl || !this.MainControl.characterModel)
		{
			return;
		}
		Transform transform = this.MainControl.transform;
		switch (this.socket)
		{
		case SpawnPaperBoxAction.Sockets.root:
			break;
		case SpawnPaperBoxAction.Sockets.helmat:
			transform = this.MainControl.characterModel.HelmatSocket;
			break;
		case SpawnPaperBoxAction.Sockets.armor:
			transform = this.MainControl.characterModel.ArmorSocket;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (!transform)
		{
			return;
		}
		if (!this.paperBoxPrefab)
		{
			return;
		}
		this.instance = Object.Instantiate<PaperBox>(this.paperBoxPrefab, transform);
		this.instance.character = this.MainControl;
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x000168BA File Offset: 0x00014ABA
	private void OnDestroy()
	{
		if (this.instance)
		{
			Object.Destroy(this.instance.gameObject);
		}
	}

	// Token: 0x04000429 RID: 1065
	public SpawnPaperBoxAction.Sockets socket = SpawnPaperBoxAction.Sockets.helmat;

	// Token: 0x0400042A RID: 1066
	public PaperBox paperBoxPrefab;

	// Token: 0x0400042B RID: 1067
	private PaperBox instance;

	// Token: 0x0400042C RID: 1068
	private CharacterMainControl _mainControl;

	// Token: 0x0200045B RID: 1115
	public enum Sockets
	{
		// Token: 0x04001B42 RID: 6978
		root,
		// Token: 0x04001B43 RID: 6979
		helmat,
		// Token: 0x04001B44 RID: 6980
		armor
	}
}
