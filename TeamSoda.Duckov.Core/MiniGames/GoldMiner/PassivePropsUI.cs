using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Duckov.MiniGames.GoldMiner.UI;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002B7 RID: 695
	public class PassivePropsUI : MiniGameBehaviour
	{
		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060016E0 RID: 5856 RVA: 0x00054D84 File Offset: 0x00052F84
		private PrefabPool<PassivePropDisplay> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<PassivePropDisplay>(this.entryTemplate, null, new Action<PassivePropDisplay>(this.OnGetEntry), new Action<PassivePropDisplay>(this.OnReleaseEntry), null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x00054DD3 File Offset: 0x00052FD3
		private void OnReleaseEntry(PassivePropDisplay display)
		{
			this.navGroup.Remove(display.NavEntry);
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00054DE6 File Offset: 0x00052FE6
		private void OnGetEntry(PassivePropDisplay display)
		{
			this.navGroup.Add(display.NavEntry);
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x00054DFC File Offset: 0x00052FFC
		private void Awake()
		{
			GoldMiner goldMiner = this.master;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = this.master;
			goldMiner2.onArtifactChange = (Action<GoldMiner>)Delegate.Combine(goldMiner2.onArtifactChange, new Action<GoldMiner>(this.OnArtifactChanged));
			GoldMiner goldMiner3 = this.master;
			goldMiner3.onEarlyLevelPlayTick = (Action<GoldMiner>)Delegate.Combine(goldMiner3.onEarlyLevelPlayTick, new Action<GoldMiner>(this.OnEarlyTick));
			NavGroup.OnNavGroupChanged = (Action)Delegate.Combine(NavGroup.OnNavGroupChanged, new Action(this.OnNavGroupChanged));
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00054E9E File Offset: 0x0005309E
		private void OnDestroy()
		{
			NavGroup.OnNavGroupChanged = (Action)Delegate.Remove(NavGroup.OnNavGroupChanged, new Action(this.OnNavGroupChanged));
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x00054EC0 File Offset: 0x000530C0
		private void OnNavGroupChanged()
		{
			this.changeLock = true;
			if (this.navGroup.active && this.Pool.ActiveEntries.Count <= 0)
			{
				this.upNavGroup.SetAsActiveNavGroup();
			}
			this.RefreshDescription();
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x00054EFC File Offset: 0x000530FC
		private void OnEarlyTick(GoldMiner miner)
		{
			this.RefreshDescription();
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00054F10 File Offset: 0x00053110
		private void SetCoord([TupleElementNames(new string[]
		{
			"x",
			"y"
		})] ValueTuple<int, int> coord)
		{
			int navIndex = this.CoordToIndex(coord);
			this.navGroup.NavIndex = navIndex;
			this.RefreshDescription();
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00054F38 File Offset: 0x00053138
		private void RefreshDescription()
		{
			if (!this.navGroup.active)
			{
				this.HideDescription();
				return;
			}
			if (this.Pool.ActiveEntries.Count <= 0)
			{
				this.HideDescription();
				return;
			}
			NavEntry selectedEntry = this.navGroup.GetSelectedEntry();
			if (selectedEntry == null)
			{
				this.HideDescription();
				return;
			}
			if (!selectedEntry.VCT.IsHovering)
			{
				this.HideDescription();
				return;
			}
			PassivePropDisplay component = selectedEntry.GetComponent<PassivePropDisplay>();
			if (component == null)
			{
				this.HideDescription();
				return;
			}
			this.SetupAndShowDescription(component);
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00054FC1 File Offset: 0x000531C1
		private void HideDescription()
		{
			this.descriptionContainer.gameObject.SetActive(false);
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x00054FD4 File Offset: 0x000531D4
		private void SetupAndShowDescription(PassivePropDisplay ppd)
		{
			this.descriptionContainer.gameObject.SetActive(true);
			string description = ppd.Target.Description;
			this.descriptionText.text = description;
			this.descriptionContainer.position = ppd.rectTransform.TransformPoint(ppd.rectTransform.rect.max);
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00055038 File Offset: 0x00053238
		private int CoordToIndex([TupleElementNames(new string[]
		{
			"x",
			"y"
		})] ValueTuple<int, int> coord)
		{
			int count = this.navGroup.entries.Count;
			if (count <= 0)
			{
				return 0;
			}
			int constraintCount = this.gridLayout.constraintCount;
			int num = count / constraintCount;
			if (coord.Item2 > num)
			{
				coord.Item2 = num;
			}
			int num2 = constraintCount;
			if (coord.Item2 == num)
			{
				num2 = count % constraintCount;
			}
			if (coord.Item1 < 0)
			{
				coord.Item1 = num2 - 1;
			}
			coord.Item1 %= num2;
			if (coord.Item2 < 0)
			{
				coord.Item2 = num;
			}
			coord.Item2 %= num + 1;
			return constraintCount * coord.Item2 + coord.Item1;
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x000550DC File Offset: 0x000532DC
		[return: TupleElementNames(new string[]
		{
			"x",
			"y"
		})]
		private ValueTuple<int, int> IndexToCoord(int index)
		{
			int constraintCount = this.gridLayout.constraintCount;
			int item = index / constraintCount;
			return new ValueTuple<int, int>(index % constraintCount, item);
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x00055102 File Offset: 0x00053302
		private void OnLevelBegin(GoldMiner miner)
		{
			this.Refresh();
			this.RefreshDescription();
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x00055110 File Offset: 0x00053310
		private void OnArtifactChanged(GoldMiner miner)
		{
			this.Refresh();
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00055118 File Offset: 0x00053318
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			if (this.master == null)
			{
				return;
			}
			GoldMinerRunData run = this.master.run;
			if (run == null)
			{
				return;
			}
			foreach (IGrouping<string, GoldMinerArtifact> source in from e in run.artifacts
			where e != null
			group e by e.ID)
			{
				GoldMinerArtifact target = source.ElementAt(0);
				this.Pool.Get(null).Setup(target, source.Count<GoldMinerArtifact>());
			}
		}

		// Token: 0x040010F8 RID: 4344
		[SerializeField]
		private GoldMiner master;

		// Token: 0x040010F9 RID: 4345
		[SerializeField]
		private RectTransform descriptionContainer;

		// Token: 0x040010FA RID: 4346
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x040010FB RID: 4347
		[SerializeField]
		private PassivePropDisplay entryTemplate;

		// Token: 0x040010FC RID: 4348
		[SerializeField]
		private NavGroup navGroup;

		// Token: 0x040010FD RID: 4349
		[SerializeField]
		private NavGroup upNavGroup;

		// Token: 0x040010FE RID: 4350
		[SerializeField]
		private GridLayoutGroup gridLayout;

		// Token: 0x040010FF RID: 4351
		private PrefabPool<PassivePropDisplay> _pool;

		// Token: 0x04001100 RID: 4352
		private bool changeLock;
	}
}
