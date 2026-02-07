using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x02000219 RID: 537
	public class Menu : MonoBehaviour
	{
		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x00040337 File Offset: 0x0003E537
		// (set) Token: 0x0600102E RID: 4142 RVA: 0x0004033F File Offset: 0x0003E53F
		public bool Focused
		{
			get
			{
				return this.focused;
			}
			set
			{
				this.SetFocused(value);
			}
		}

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x0600102F RID: 4143 RVA: 0x00040348 File Offset: 0x0003E548
		// (remove) Token: 0x06001030 RID: 4144 RVA: 0x00040380 File Offset: 0x0003E580
		public event Action<Menu, MenuItem> onSelectionChanged;

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06001031 RID: 4145 RVA: 0x000403B8 File Offset: 0x0003E5B8
		// (remove) Token: 0x06001032 RID: 4146 RVA: 0x000403F0 File Offset: 0x0003E5F0
		public event Action<Menu, MenuItem> onConfirmed;

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06001033 RID: 4147 RVA: 0x00040428 File Offset: 0x0003E628
		// (remove) Token: 0x06001034 RID: 4148 RVA: 0x00040460 File Offset: 0x0003E660
		public event Action<Menu, MenuItem> onCanceled;

		// Token: 0x06001035 RID: 4149 RVA: 0x00040495 File Offset: 0x0003E695
		private void SetFocused(bool value)
		{
			this.focused = value;
			if (this.focused && this.cursor == null)
			{
				this.SelectDefault();
			}
			MenuItem menuItem = this.cursor;
			if (menuItem == null)
			{
				return;
			}
			menuItem.NotifyMasterFocusStatusChanged();
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x000404CA File Offset: 0x0003E6CA
		public MenuItem GetSelected()
		{
			return this.cursor;
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x000404D4 File Offset: 0x0003E6D4
		public T GetSelected<T>() where T : Component
		{
			if (this.cursor == null)
			{
				return default(T);
			}
			return this.cursor.GetComponent<T>();
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x00040504 File Offset: 0x0003E704
		public void Select(MenuItem toSelect)
		{
			if (toSelect.transform.parent != base.transform)
			{
				Debug.LogError("正在尝试选中不属于此菜单的项目。已取消。");
				return;
			}
			if (!this.items.Contains(toSelect))
			{
				this.items.Add(toSelect);
			}
			if (!toSelect.Selectable)
			{
				return;
			}
			if (this.cursor != null)
			{
				this.DeselectCurrent();
			}
			this.cursor = toSelect;
			this.cursor.NotifySelected();
			this.OnSelectionChanged();
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00040584 File Offset: 0x0003E784
		public void SelectDefault()
		{
			MenuItem[] componentsInChildren = base.GetComponentsInChildren<MenuItem>(false);
			if (componentsInChildren == null)
			{
				return;
			}
			foreach (MenuItem menuItem in componentsInChildren)
			{
				if (!(menuItem == null) && menuItem.Selectable)
				{
					this.Select(menuItem);
				}
			}
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x000405C7 File Offset: 0x0003E7C7
		public void Confirm()
		{
			if (this.cursor != null)
			{
				this.cursor.NotifyConfirmed();
			}
			Action<Menu, MenuItem> action = this.onConfirmed;
			if (action == null)
			{
				return;
			}
			action(this, this.cursor);
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x000405F9 File Offset: 0x0003E7F9
		public void Cancel()
		{
			if (this.cursor != null)
			{
				this.cursor.NotifyCanceled();
			}
			Action<Menu, MenuItem> action = this.onCanceled;
			if (action == null)
			{
				return;
			}
			action(this, this.cursor);
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0004062B File Offset: 0x0003E82B
		private void DeselectCurrent()
		{
			this.cursor.NotifyDeselected();
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x00040638 File Offset: 0x0003E838
		private void OnSelectionChanged()
		{
			Action<Menu, MenuItem> action = this.onSelectionChanged;
			if (action == null)
			{
				return;
			}
			action(this, this.cursor);
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x00040654 File Offset: 0x0003E854
		public void Navigate(Vector2 direction)
		{
			if (this.cursor == null)
			{
				this.SelectDefault();
			}
			if (this.cursor == null)
			{
				return;
			}
			if (Mathf.Approximately(direction.sqrMagnitude, 0f))
			{
				return;
			}
			MenuItem menuItem = this.FindClosestEntryInDirection(this.cursor, direction);
			if (menuItem == null)
			{
				return;
			}
			this.Select(menuItem);
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x000406B8 File Offset: 0x0003E8B8
		private MenuItem FindClosestEntryInDirection(MenuItem cursor, Vector2 direction)
		{
			if (cursor == null)
			{
				return null;
			}
			direction = direction.normalized;
			float num = Mathf.Cos(0.7853982f);
			Menu.<>c__DisplayClass26_0 CS$<>8__locals1;
			CS$<>8__locals1.bestMatch = null;
			CS$<>8__locals1.bestSqrDist = float.MaxValue;
			CS$<>8__locals1.bestDot = num;
			foreach (MenuItem cur in this.items)
			{
				Menu.<>c__DisplayClass26_1 CS$<>8__locals2;
				CS$<>8__locals2.cur = cur;
				if (!(CS$<>8__locals2.cur == null) && !(CS$<>8__locals2.cur == cursor) && CS$<>8__locals2.cur.Selectable)
				{
					Vector3 vector = CS$<>8__locals2.cur.transform.localPosition - cursor.transform.localPosition;
					Vector3 normalized = vector.normalized;
					Menu.<>c__DisplayClass26_2 CS$<>8__locals3;
					CS$<>8__locals3.dot = Vector3.Dot(normalized, direction);
					if (CS$<>8__locals3.dot >= num)
					{
						CS$<>8__locals3.sqrDist = vector.magnitude;
						if (CS$<>8__locals3.sqrDist <= CS$<>8__locals1.bestSqrDist)
						{
							if (CS$<>8__locals3.sqrDist < CS$<>8__locals1.bestSqrDist)
							{
								Menu.<FindClosestEntryInDirection>g__SetBestAsCur|26_0(ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
							}
							else if (CS$<>8__locals3.sqrDist == CS$<>8__locals1.bestSqrDist && CS$<>8__locals3.dot > CS$<>8__locals1.bestDot)
							{
								Menu.<FindClosestEntryInDirection>g__SetBestAsCur|26_0(ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
							}
						}
					}
				}
			}
			return CS$<>8__locals1.bestMatch;
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x00040830 File Offset: 0x0003EA30
		internal void Register(MenuItem menuItem)
		{
			this.items.Add(menuItem);
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0004083F File Offset: 0x0003EA3F
		internal void Unegister(MenuItem menuItem)
		{
			this.items.Remove(menuItem);
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00040861 File Offset: 0x0003EA61
		[CompilerGenerated]
		internal static void <FindClosestEntryInDirection>g__SetBestAsCur|26_0(ref Menu.<>c__DisplayClass26_0 A_0, ref Menu.<>c__DisplayClass26_1 A_1, ref Menu.<>c__DisplayClass26_2 A_2)
		{
			A_0.bestMatch = A_1.cur;
			A_0.bestSqrDist = A_2.sqrDist;
			A_0.bestDot = A_2.dot;
		}

		// Token: 0x04000D20 RID: 3360
		[SerializeField]
		private bool focused;

		// Token: 0x04000D21 RID: 3361
		[SerializeField]
		private MenuItem cursor;

		// Token: 0x04000D22 RID: 3362
		[SerializeField]
		private LayoutGroup layout;

		// Token: 0x04000D26 RID: 3366
		private HashSet<MenuItem> items = new HashSet<MenuItem>();
	}
}
