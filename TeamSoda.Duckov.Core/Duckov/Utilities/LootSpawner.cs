using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000416 RID: 1046
	[RequireComponent(typeof(Points))]
	public class LootSpawner : MonoBehaviour
	{
		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x060025FD RID: 9725 RVA: 0x00083E43 File Offset: 0x00082043
		public bool RandomFromPool
		{
			get
			{
				return this.randomGenrate && this.randomFromPool;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x060025FE RID: 9726 RVA: 0x00083E55 File Offset: 0x00082055
		public bool RandomButNotFromPool
		{
			get
			{
				return this.randomGenrate && !this.randomFromPool;
			}
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x00083E6A File Offset: 0x0008206A
		public void CalculateChances()
		{
			this.tags.RefreshPercent();
			this.qualities.RefreshPercent();
			this.randomPool.RefreshPercent();
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x00083E90 File Offset: 0x00082090
		private void Start()
		{
			if (this.points == null)
			{
				this.points = base.GetComponent<Points>();
			}
			bool flag = false;
			int key = this.GetKey();
			object obj;
			if (MultiSceneCore.Instance.inLevelData.TryGetValue(key, out obj) && obj is bool)
			{
				bool flag2 = (bool)obj;
				flag = flag2;
			}
			if (!flag)
			{
				if (Random.Range(0f, 1f) <= this.spawnChance)
				{
					this.Setup().Forget();
				}
				MultiSceneCore.Instance.inLevelData.Add(key, true);
			}
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x00083F24 File Offset: 0x00082124
		private int GetKey()
		{
			Transform parent = base.transform.parent;
			string text = base.transform.GetSiblingIndex().ToString();
			while (parent != null)
			{
				text = string.Format("{0}/{1}", parent.GetSiblingIndex(), text);
				parent = parent.parent;
			}
			text = string.Format("{0}/{1}", base.gameObject.scene.buildIndex, text);
			return text.GetHashCode();
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x00083FA4 File Offset: 0x000821A4
		public UniTask Setup()
		{
			LootSpawner.<Setup>d__20 <Setup>d__;
			<Setup>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Setup>d__.<>4__this = this;
			<Setup>d__.<>1__state = -1;
			<Setup>d__.<>t__builder.Start<LootSpawner.<Setup>d__20>(ref <Setup>d__);
			return <Setup>d__.<>t__builder.Task;
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x00083FE7 File Offset: 0x000821E7
		public static int[] Search(ItemFilter filter)
		{
			return ItemAssetsCollection.Search(filter);
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x00083FEF File Offset: 0x000821EF
		private void OnValidate()
		{
			if (this.points == null)
			{
				this.points = base.GetComponent<Points>();
			}
		}

		// Token: 0x040019E4 RID: 6628
		[Range(0f, 1f)]
		public float spawnChance = 1f;

		// Token: 0x040019E5 RID: 6629
		public bool randomGenrate = true;

		// Token: 0x040019E6 RID: 6630
		public bool randomFromPool;

		// Token: 0x040019E7 RID: 6631
		[SerializeField]
		private Vector2Int randomCount = new Vector2Int(1, 1);

		// Token: 0x040019E8 RID: 6632
		[SerializeField]
		private RandomContainer<Tag> tags;

		// Token: 0x040019E9 RID: 6633
		[SerializeField]
		private List<Tag> excludeTags;

		// Token: 0x040019EA RID: 6634
		[SerializeField]
		private RandomContainer<int> qualities;

		// Token: 0x040019EB RID: 6635
		[SerializeField]
		private RandomContainer<LootSpawner.Entry> randomPool;

		// Token: 0x040019EC RID: 6636
		[ItemTypeID]
		[SerializeField]
		private List<int> fixedItems;

		// Token: 0x040019ED RID: 6637
		[SerializeField]
		private Points points;

		// Token: 0x040019EE RID: 6638
		private bool loading;

		// Token: 0x040019EF RID: 6639
		[SerializeField]
		[ItemTypeID]
		private List<int> typeIds;

		// Token: 0x0200068B RID: 1675
		[Serializable]
		private struct Entry
		{
			// Token: 0x04002406 RID: 9222
			[ItemTypeID]
			[SerializeField]
			public int itemTypeID;
		}
	}
}
