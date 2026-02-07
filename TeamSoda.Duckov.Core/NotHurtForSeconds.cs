using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200008B RID: 139
[MenuPath("Health/一段时间没受伤")]
public class NotHurtForSeconds : EffectFilter
{
	// Token: 0x1700010D RID: 269
	// (get) Token: 0x060004FC RID: 1276 RVA: 0x00016AA3 File Offset: 0x00014CA3
	public override string DisplayName
	{
		get
		{
			return this.time.ToString() + "秒内没受伤";
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x060004FD RID: 1277 RVA: 0x00016ABA File Offset: 0x00014CBA
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

	// Token: 0x060004FE RID: 1278 RVA: 0x00016AF4 File Offset: 0x00014CF4
	protected override bool OnEvaluate(EffectTriggerEventContext context)
	{
		if (!this.binded && this.MainControl)
		{
			this.MainControl.Health.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnHurt));
			this.binded = true;
		}
		return Time.time - this.lastHurtTime > this.time;
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x00016B52 File Offset: 0x00014D52
	private void OnDestroy()
	{
		if (this.MainControl)
		{
			this.MainControl.Health.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnHurt));
		}
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00016B82 File Offset: 0x00014D82
	private void OnHurt(DamageInfo dmgInfo)
	{
		this.lastHurtTime = Time.time;
	}

	// Token: 0x04000435 RID: 1077
	public float time;

	// Token: 0x04000436 RID: 1078
	private float lastHurtTime = -9999f;

	// Token: 0x04000437 RID: 1079
	private bool binded;

	// Token: 0x04000438 RID: 1080
	private CharacterMainControl _mainControl;
}
