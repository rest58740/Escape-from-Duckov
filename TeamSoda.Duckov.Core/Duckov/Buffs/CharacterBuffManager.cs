using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Duckov.Buffs
{
	// Token: 0x0200041A RID: 1050
	public class CharacterBuffManager : MonoBehaviour
	{
		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002643 RID: 9795 RVA: 0x000848CB File Offset: 0x00082ACB
		public CharacterMainControl Master
		{
			get
			{
				return this.master;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002644 RID: 9796 RVA: 0x000848D3 File Offset: 0x00082AD3
		public ReadOnlyCollection<Buff> Buffs
		{
			get
			{
				if (this._readOnlyBuffsCollection == null)
				{
					this._readOnlyBuffsCollection = new ReadOnlyCollection<Buff>(this.buffs);
				}
				return this._readOnlyBuffsCollection;
			}
		}

		// Token: 0x14000101 RID: 257
		// (add) Token: 0x06002645 RID: 9797 RVA: 0x000848F4 File Offset: 0x00082AF4
		// (remove) Token: 0x06002646 RID: 9798 RVA: 0x0008492C File Offset: 0x00082B2C
		public event Action<CharacterBuffManager, Buff> onAddBuff;

		// Token: 0x14000102 RID: 258
		// (add) Token: 0x06002647 RID: 9799 RVA: 0x00084964 File Offset: 0x00082B64
		// (remove) Token: 0x06002648 RID: 9800 RVA: 0x0008499C File Offset: 0x00082B9C
		public event Action<CharacterBuffManager, Buff> onRemoveBuff;

		// Token: 0x06002649 RID: 9801 RVA: 0x000849D1 File Offset: 0x00082BD1
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<CharacterMainControl>();
			}
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x000849F0 File Offset: 0x00082BF0
		public void AddBuff(Buff buffPrefab, CharacterMainControl fromWho, int overrideWeaponID = 0)
		{
			if (buffPrefab == null)
			{
				return;
			}
			Buff buff = this.buffs.Find((Buff e) => e.ID == buffPrefab.ID);
			if (buff)
			{
				buff.NotifyIncomingBuffWithSameID(buffPrefab);
				return;
			}
			Buff buff2 = Object.Instantiate<Buff>(buffPrefab);
			buff2.Setup(this);
			buff2.fromWho = fromWho;
			if (overrideWeaponID > 0)
			{
				buff2.fromWeaponID = overrideWeaponID;
			}
			this.buffs.Add(buff2);
			Action<CharacterBuffManager, Buff> action = this.onAddBuff;
			if (action == null)
			{
				return;
			}
			action(this, buff2);
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x00084A8C File Offset: 0x00082C8C
		public void RemoveBuff(int buffID, bool removeOneLayer)
		{
			Buff buff = this.buffs.Find((Buff e) => e.ID == buffID);
			if (buff != null)
			{
				this.RemoveBuff(buff, removeOneLayer);
			}
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x00084AD0 File Offset: 0x00082CD0
		public void RemoveBuffsByTag(Buff.BuffExclusiveTags buffTag, bool removeOneLayer)
		{
			if (buffTag == Buff.BuffExclusiveTags.NotExclusive)
			{
				return;
			}
			foreach (Buff buff in this.buffs.FindAll((Buff e) => e.ExclusiveTag == buffTag))
			{
				if (buff != null)
				{
					this.RemoveBuff(buff, removeOneLayer);
				}
			}
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x00084B54 File Offset: 0x00082D54
		public bool HasBuff(int buffID)
		{
			return this.buffs.Find((Buff e) => e.ID == buffID) != null;
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x00084B8C File Offset: 0x00082D8C
		public Buff GetBuffByTag(Buff.BuffExclusiveTags tag)
		{
			if (tag == Buff.BuffExclusiveTags.NotExclusive)
			{
				return null;
			}
			return this.buffs.Find((Buff e) => e.ExclusiveTag == tag);
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x00084BC8 File Offset: 0x00082DC8
		public void RemoveBuff(Buff toRemove, bool oneLayer)
		{
			if (oneLayer && toRemove.CurrentLayers > 1)
			{
				toRemove.CurrentLayers--;
				if (toRemove.CurrentLayers >= 1)
				{
					return;
				}
			}
			if (this.buffs.Remove(toRemove))
			{
				Action<CharacterBuffManager, Buff> action = this.onRemoveBuff;
				if (action != null)
				{
					action(this, toRemove);
				}
				Object.Destroy(toRemove.gameObject);
			}
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x00084C28 File Offset: 0x00082E28
		private void Update()
		{
			bool flag = false;
			foreach (Buff buff in this.buffs)
			{
				if (buff == null)
				{
					flag = true;
				}
				else if (buff.IsOutOfTime)
				{
					buff.NotifyOutOfTime();
					this.outOfTimeBuffsBuffer.Add(buff);
				}
				else
				{
					buff.NotifyUpdate();
				}
			}
			if (this.outOfTimeBuffsBuffer.Count > 0)
			{
				foreach (Buff buff2 in this.outOfTimeBuffsBuffer)
				{
					if (buff2 != null)
					{
						this.RemoveBuff(buff2, false);
					}
				}
				this.outOfTimeBuffsBuffer.Clear();
			}
			if (flag)
			{
				this.buffs.RemoveAll((Buff e) => e == null);
			}
		}

		// Token: 0x04001A1B RID: 6683
		[SerializeField]
		private CharacterMainControl master;

		// Token: 0x04001A1C RID: 6684
		private List<Buff> buffs = new List<Buff>();

		// Token: 0x04001A1D RID: 6685
		private ReadOnlyCollection<Buff> _readOnlyBuffsCollection;

		// Token: 0x04001A20 RID: 6688
		private List<Buff> outOfTimeBuffsBuffer = new List<Buff>();
	}
}
