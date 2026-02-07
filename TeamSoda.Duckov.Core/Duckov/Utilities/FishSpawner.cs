using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Weathers;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000415 RID: 1045
	public class FishSpawner : MonoBehaviour
	{
		// Token: 0x060025F5 RID: 9717 RVA: 0x00083C1A File Offset: 0x00081E1A
		public void CalculateChances()
		{
			this.tags.RefreshPercent();
			this.qualities.RefreshPercent();
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x00083C32 File Offset: 0x00081E32
		private void Awake()
		{
			this.excludeTagsReal = new List<Tag>();
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x00083C3F File Offset: 0x00081E3F
		private void Start()
		{
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x00083C44 File Offset: 0x00081E44
		public UniTask<Item> Spawn(int baitID, float luck)
		{
			FishSpawner.<Spawn>d__14 <Spawn>d__;
			<Spawn>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<Spawn>d__.<>4__this = this;
			<Spawn>d__.baitID = baitID;
			<Spawn>d__.luck = luck;
			<Spawn>d__.<>1__state = -1;
			<Spawn>d__.<>t__builder.Start<FishSpawner.<Spawn>d__14>(ref <Spawn>d__);
			return <Spawn>d__.<>t__builder.Task;
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x00083C97 File Offset: 0x00081E97
		public static int[] Search(ItemFilter filter)
		{
			return ItemAssetsCollection.Search(filter);
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x00083CA0 File Offset: 0x00081EA0
		private void CalculateTags(bool atNight, Weather weather)
		{
			this.excludeTagsReal.Clear();
			this.excludeTagsReal.AddRange(this.excludeTags);
			if (atNight)
			{
				this.excludeTagsReal.Add(this.Fish_OnlyDay);
			}
			else
			{
				this.excludeTagsReal.Add(this.Fish_OnlyNight);
			}
			this.excludeTagsReal.Add(this.Fish_OnlySunDay);
			this.excludeTagsReal.Add(this.Fish_OnlyRainDay);
			this.excludeTagsReal.Add(this.Fish_OnlyStorm);
			switch (weather)
			{
			case Weather.Sunny:
				this.excludeTagsReal.Remove(this.Fish_OnlySunDay);
				return;
			case Weather.Cloudy:
				break;
			case Weather.Rainy:
				this.excludeTagsReal.Remove(this.Fish_OnlyRainDay);
				return;
			case Weather.Stormy_I:
				this.excludeTagsReal.Remove(this.Fish_OnlyStorm);
				return;
			case Weather.Stormy_II:
				this.excludeTagsReal.Remove(this.Fish_OnlyStorm);
				break;
			default:
				if (weather != Weather.Snow)
				{
					return;
				}
				this.excludeTagsReal.Remove(this.Fish_OnlyRainDay);
				return;
			}
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x00083DA4 File Offset: 0x00081FA4
		private bool CheckFishDayNightAndWeather(int fishID, bool atNight, Weather currentWeather)
		{
			ItemMetaData metaData = ItemAssetsCollection.GetMetaData(fishID);
			return (!metaData.tags.Contains(this.Fish_OnlyNight) || atNight) && (!metaData.tags.Contains(this.Fish_OnlyDay) || !atNight) && (!metaData.tags.Contains(this.Fish_OnlyRainDay) || currentWeather == Weather.Rainy || currentWeather == Weather.Snow) && (!metaData.tags.Contains(this.Fish_OnlySunDay) || currentWeather == Weather.Sunny) && (!metaData.tags.Contains(this.Fish_OnlyStorm) || currentWeather == Weather.Stormy_I || currentWeather == Weather.Stormy_II);
		}

		// Token: 0x040019DA RID: 6618
		[SerializeField]
		private List<FishSpawner.SpecialPair> specialPairs;

		// Token: 0x040019DB RID: 6619
		[SerializeField]
		private RandomContainer<Tag> tags;

		// Token: 0x040019DC RID: 6620
		[SerializeField]
		private List<Tag> excludeTags;

		// Token: 0x040019DD RID: 6621
		[SerializeField]
		private RandomContainer<int> qualities;

		// Token: 0x040019DE RID: 6622
		private List<Tag> excludeTagsReal;

		// Token: 0x040019DF RID: 6623
		[SerializeField]
		private Tag Fish_OnlyDay;

		// Token: 0x040019E0 RID: 6624
		[SerializeField]
		private Tag Fish_OnlyNight;

		// Token: 0x040019E1 RID: 6625
		[SerializeField]
		private Tag Fish_OnlySunDay;

		// Token: 0x040019E2 RID: 6626
		[SerializeField]
		private Tag Fish_OnlyRainDay;

		// Token: 0x040019E3 RID: 6627
		[SerializeField]
		private Tag Fish_OnlyStorm;

		// Token: 0x02000689 RID: 1673
		[Serializable]
		private struct SpecialPair
		{
			// Token: 0x040023FD RID: 9213
			[ItemTypeID]
			public int baitID;

			// Token: 0x040023FE RID: 9214
			[ItemTypeID]
			public int fishID;

			// Token: 0x040023FF RID: 9215
			[Range(0f, 1f)]
			public float chance;
		}
	}
}
