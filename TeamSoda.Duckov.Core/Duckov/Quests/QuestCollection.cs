using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200034B RID: 843
	[CreateAssetMenu(menuName = "Quest Collection")]
	public class QuestCollection : ScriptableObject, IList<Quest>, ICollection<Quest>, IEnumerable<Quest>, IEnumerable, ISelfValidator
	{
		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x00069503 File Offset: 0x00067703
		public static QuestCollection Instance
		{
			get
			{
				return GameplayDataSettings.QuestCollection;
			}
		}

		// Token: 0x17000553 RID: 1363
		public Quest this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				this.list[index] = value;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001CE6 RID: 7398 RVA: 0x00069527 File Offset: 0x00067727
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x00069534 File Offset: 0x00067734
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x00069537 File Offset: 0x00067737
		public void Add(Quest item)
		{
			this.list.Add(item);
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x00069545 File Offset: 0x00067745
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x00069552 File Offset: 0x00067752
		public bool Contains(Quest item)
		{
			return this.list.Contains(item);
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x00069560 File Offset: 0x00067760
		public void CopyTo(Quest[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x0006956F File Offset: 0x0006776F
		public IEnumerator<Quest> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x00069581 File Offset: 0x00067781
		public int IndexOf(Quest item)
		{
			return this.list.IndexOf(item);
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x0006958F File Offset: 0x0006778F
		public void Insert(int index, Quest item)
		{
			this.list.Insert(index, item);
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x0006959E File Offset: 0x0006779E
		public bool Remove(Quest item)
		{
			return this.list.Remove(item);
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000695AC File Offset: 0x000677AC
		public void RemoveAt(int index)
		{
			this.list.RemoveAt(index);
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x000695BA File Offset: 0x000677BA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x000695C2 File Offset: 0x000677C2
		public void Collect()
		{
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x000695C4 File Offset: 0x000677C4
		public void Validate(SelfValidationResult result)
		{
			this.list.GroupBy(delegate(Quest e)
			{
				if (e == null)
				{
					return -1;
				}
				return e.ID;
			});
			if (this.list.GroupBy(delegate(Quest e)
			{
				if (e == null)
				{
					return -1;
				}
				return e.ID;
			}).Any((IGrouping<int, Quest> g) => g.Count<Quest>() > 1))
			{
				SelfValidationResultItemExtensions.WithFix(result.AddError("存在冲突的QuestID。"), "自动重新分配ID", new Action(this.AutoFixID), true);
			}
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x00069670 File Offset: 0x00067870
		private void AutoFixID()
		{
			int num = this.list.Max((Quest e) => e.ID) + 1;
			foreach (IEnumerable<Quest> enumerable in from e in this.list
			group e by e.ID into g
			where g.Count<Quest>() > 1
			select g)
			{
				int num2 = 0;
				foreach (Quest quest in enumerable)
				{
					if (!(quest == null) && num2++ != 0)
					{
						quest.ID = num++;
					}
				}
			}
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x0006977C File Offset: 0x0006797C
		public Quest Get(int id)
		{
			return this.list.FirstOrDefault((Quest q) => q != null && q.ID == id);
		}

		// Token: 0x0400145B RID: 5211
		[SerializeField]
		private List<Quest> list;
	}
}
