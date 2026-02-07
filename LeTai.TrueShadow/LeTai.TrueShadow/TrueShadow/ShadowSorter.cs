using System;
using System.Collections.Generic;
using UnityEngine;

namespace LeTai.TrueShadow
{
	// Token: 0x02000011 RID: 17
	[ExecuteAlways]
	public class ShadowSorter : MonoBehaviour
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00004B20 File Offset: 0x00002D20
		public static ShadowSorter Instance
		{
			get
			{
				if (!ShadowSorter.instance)
				{
					ShadowSorter[] array = Shims.FindObjectsOfType<ShadowSorter>(false);
					for (int i = array.Length - 1; i > 0; i--)
					{
						UnityEngine.Object.Destroy(array[i]);
					}
					ShadowSorter.instance = ((array.Length != 0) ? array[0] : null);
					if (!ShadowSorter.instance)
					{
						ShadowSorter.instance = new GameObject("ShadowSorter")
						{
							hideFlags = HideFlags.HideAndDontSave
						}.AddComponent<ShadowSorter>();
					}
				}
				return ShadowSorter.instance;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004B94 File Offset: 0x00002D94
		public void Register(TrueShadow shadow)
		{
			this.shadows.AddUnique(shadow);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004BA3 File Offset: 0x00002DA3
		public void UnRegister(TrueShadow shadow)
		{
			this.shadows.Remove(shadow);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004BB4 File Offset: 0x00002DB4
		private void LateUpdate()
		{
			if (!this)
			{
				return;
			}
			for (int i = 0; i < this.shadows.Count; i++)
			{
				TrueShadow trueShadow = this.shadows[i];
				if (trueShadow && trueShadow.isActiveAndEnabled)
				{
					trueShadow.CheckHierarchyDirtied();
					if (trueShadow.HierachyDirty)
					{
						this.AddSortEntry(trueShadow);
					}
				}
			}
			this.Sort();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004C18 File Offset: 0x00002E18
		private void AddSortEntry(TrueShadow shadow)
		{
			ShadowSorter.SortEntry sortEntry = new ShadowSorter.SortEntry(shadow);
			ShadowSorter.SortGroup item = new ShadowSorter.SortGroup(sortEntry);
			int num = this.sortGroups.IndexOf(item);
			if (num > -1)
			{
				this.sortGroups[num].Add(sortEntry);
				return;
			}
			this.sortGroups.Add(item);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004C68 File Offset: 0x00002E68
		public void Sort()
		{
			for (int i = 0; i < this.sortGroups.Count; i++)
			{
				ShadowSorter.SortGroup sortGroup = this.sortGroups[i];
				if (sortGroup.parentTransform)
				{
					foreach (ShadowSorter.SortEntry sortEntry in sortGroup.sortEntries)
					{
						sortEntry.rendererTransform.SetParent(sortGroup.parentTransform, false);
						int siblingIndex = sortEntry.rendererTransform.GetSiblingIndex();
						int siblingIndex2 = sortEntry.shadowTransform.GetSiblingIndex();
						if (siblingIndex > siblingIndex2)
						{
							sortEntry.rendererTransform.SetSiblingIndex(siblingIndex2);
						}
						else
						{
							sortEntry.rendererTransform.SetSiblingIndex(siblingIndex2 - 1);
						}
						sortEntry.shadow.UnSetHierachyDirty();
					}
					foreach (ShadowSorter.SortEntry sortEntry2 in sortGroup.sortEntries)
					{
						sortEntry2.shadow.ForgetSiblingIndexChanges();
					}
				}
			}
			this.sortGroups.Clear();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004D98 File Offset: 0x00002F98
		private void OnApplicationQuit()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04000066 RID: 102
		private static ShadowSorter instance;

		// Token: 0x04000067 RID: 103
		private readonly IndexedSet<TrueShadow> shadows = new IndexedSet<TrueShadow>();

		// Token: 0x04000068 RID: 104
		private readonly IndexedSet<ShadowSorter.SortGroup> sortGroups = new IndexedSet<ShadowSorter.SortGroup>();

		// Token: 0x02000031 RID: 49
		private readonly struct SortEntry : IComparable<ShadowSorter.SortEntry>
		{
			// Token: 0x0600014E RID: 334 RVA: 0x00006D0D File Offset: 0x00004F0D
			public SortEntry(TrueShadow shadow)
			{
				this.shadow = shadow;
				this.shadowTransform = shadow.transform;
				this.rendererTransform = shadow.shadowRenderer.transform;
			}

			// Token: 0x0600014F RID: 335 RVA: 0x00006D34 File Offset: 0x00004F34
			public int CompareTo(ShadowSorter.SortEntry other)
			{
				return other.shadowTransform.GetSiblingIndex().CompareTo(this.shadowTransform.GetSiblingIndex());
			}

			// Token: 0x040000D3 RID: 211
			public readonly TrueShadow shadow;

			// Token: 0x040000D4 RID: 212
			public readonly Transform shadowTransform;

			// Token: 0x040000D5 RID: 213
			public readonly Transform rendererTransform;
		}

		// Token: 0x02000032 RID: 50
		private readonly struct SortGroup
		{
			// Token: 0x06000150 RID: 336 RVA: 0x00006D5F File Offset: 0x00004F5F
			public SortGroup(ShadowSorter.SortEntry firstEntry)
			{
				this.sortEntries = new List<ShadowSorter.SortEntry>
				{
					firstEntry
				};
				this.parentTransform = firstEntry.shadowTransform.parent;
			}

			// Token: 0x06000151 RID: 337 RVA: 0x00006D84 File Offset: 0x00004F84
			public void Add(ShadowSorter.SortEntry pair)
			{
				if (pair.shadowTransform.parent != this.parentTransform)
				{
					return;
				}
				int num = this.sortEntries.BinarySearch(pair);
				if (num < 0)
				{
					this.sortEntries.Insert(~num, pair);
				}
			}

			// Token: 0x06000152 RID: 338 RVA: 0x00006DC9 File Offset: 0x00004FC9
			public override int GetHashCode()
			{
				return this.parentTransform.GetHashCode();
			}

			// Token: 0x06000153 RID: 339 RVA: 0x00006DD8 File Offset: 0x00004FD8
			public override bool Equals(object obj)
			{
				if (obj is ShadowSorter.SortGroup)
				{
					ShadowSorter.SortGroup sortGroup = (ShadowSorter.SortGroup)obj;
					return sortGroup.parentTransform == this.parentTransform;
				}
				return false;
			}

			// Token: 0x040000D6 RID: 214
			public readonly Transform parentTransform;

			// Token: 0x040000D7 RID: 215
			public readonly List<ShadowSorter.SortEntry> sortEntries;
		}
	}
}
