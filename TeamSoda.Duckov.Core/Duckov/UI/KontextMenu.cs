using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003E3 RID: 995
	public class KontextMenu : MonoBehaviour
	{
		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002453 RID: 9299 RVA: 0x0007F874 File Offset: 0x0007DA74
		private Transform ContentRoot
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002454 RID: 9300 RVA: 0x0007F87C File Offset: 0x0007DA7C
		private PrefabPool<KontextMenuEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<KontextMenuEntry>(this.entryPrefab, this.ContentRoot, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x0007F8BA File Offset: 0x0007DABA
		private void Awake()
		{
			if (KontextMenu.instance == null)
			{
				KontextMenu.instance = this;
			}
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x0007F8E0 File Offset: 0x0007DAE0
		private void OnDestroy()
		{
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x0007F8E4 File Offset: 0x0007DAE4
		private void Update()
		{
			if (this.watchRectTransform)
			{
				if ((this.cachedTransformPosition - this.watchRectTransform.position).magnitude > this.positionMoveCloseThreshold)
				{
					KontextMenu.Hide(null);
					return;
				}
			}
			else if (this.isWatchingRectTransform)
			{
				KontextMenu.Hide(null);
			}
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x0007F93C File Offset: 0x0007DB3C
		public void InstanceShow(object target, RectTransform targetRectTransform, params KontextMenuDataEntry[] entries)
		{
			this.target = target;
			this.watchRectTransform = targetRectTransform;
			this.isWatchingRectTransform = true;
			this.cachedTransformPosition = this.watchRectTransform.position;
			Vector3[] array = new Vector3[4];
			targetRectTransform.GetWorldCorners(array);
			float num = Mathf.Min(new float[]
			{
				array[0].x,
				array[1].x,
				array[2].x,
				array[3].x
			});
			float num2 = Mathf.Max(new float[]
			{
				array[0].x,
				array[1].x,
				array[2].x,
				array[3].x
			});
			float num3 = Mathf.Min(new float[]
			{
				array[0].y,
				array[1].y,
				array[2].y,
				array[3].y
			});
			float num4 = Mathf.Max(new float[]
			{
				array[0].y,
				array[1].y,
				array[2].y,
				array[3].y
			});
			float num5 = num;
			float num6 = (float)Screen.width - num2;
			float num7 = num3;
			float num8 = (float)Screen.height - num4;
			float num9 = (num5 > num6) ? num : num2;
			float num10 = (num7 > num8) ? num3 : num4;
			Vector2 vector;
			vector..ctor(num9, num10);
			if (entries.Length < 1)
			{
				this.InstanceHide();
				return;
			}
			Vector2 vector2 = new Vector2(vector.x / (float)Screen.width, vector.y / (float)Screen.height);
			float num11 = (float)((vector2.x < 0.5f) ? 0 : 1);
			float num12 = (float)((vector2.y < 0.5f) ? 0 : 1);
			this.rectTransform.pivot = new Vector2(num11, num12);
			base.gameObject.SetActive(true);
			this.fadeGroup.SkipHide();
			this.Setup(entries);
			this.fadeGroup.Show();
			base.transform.position = vector;
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x0007FB80 File Offset: 0x0007DD80
		public void InstanceShow(object target, Vector2 screenPoint, params KontextMenuDataEntry[] entries)
		{
			this.target = target;
			this.watchRectTransform = null;
			this.isWatchingRectTransform = false;
			if (entries.Length < 1)
			{
				this.InstanceHide();
				return;
			}
			Vector2 vector = new Vector2(screenPoint.x / (float)Screen.width, screenPoint.y / (float)Screen.height);
			float num = (float)((vector.x < 0.5f) ? 0 : 1);
			float num2 = (float)((vector.y < 0.5f) ? 0 : 1);
			this.rectTransform.pivot = new Vector2(num, num2);
			base.gameObject.SetActive(true);
			this.fadeGroup.SkipHide();
			this.Setup(entries);
			this.fadeGroup.Show();
			base.transform.position = screenPoint;
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x0007FC40 File Offset: 0x0007DE40
		private void Clear()
		{
			this.EntryPool.ReleaseAll();
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.ContentRoot.childCount; i++)
			{
				Transform child = this.ContentRoot.GetChild(i);
				if (child.gameObject.activeSelf)
				{
					list.Add(child.gameObject);
				}
			}
			foreach (GameObject gameObject in list)
			{
				Object.Destroy(gameObject);
			}
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0007FCD8 File Offset: 0x0007DED8
		private void Setup(IEnumerable<KontextMenuDataEntry> entries)
		{
			this.Clear();
			int num = 0;
			foreach (KontextMenuDataEntry kontextMenuDataEntry in entries)
			{
				if (kontextMenuDataEntry != null)
				{
					KontextMenuEntry kontextMenuEntry = this.EntryPool.Get(this.ContentRoot);
					num++;
					kontextMenuEntry.Setup(this, num, kontextMenuDataEntry);
					kontextMenuEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x0007FD4C File Offset: 0x0007DF4C
		public void InstanceHide()
		{
			this.target = null;
			this.watchRectTransform = null;
			this.fadeGroup.Hide();
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x0007FD67 File Offset: 0x0007DF67
		public static void Show(object target, RectTransform watchRectTransform, params KontextMenuDataEntry[] entries)
		{
			if (KontextMenu.instance == null)
			{
				return;
			}
			KontextMenu.instance.InstanceShow(target, watchRectTransform, entries);
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x0007FD84 File Offset: 0x0007DF84
		public static void Show(object target, Vector2 position, params KontextMenuDataEntry[] entries)
		{
			if (KontextMenu.instance == null)
			{
				return;
			}
			KontextMenu.instance.InstanceShow(target, position, entries);
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x0007FDA1 File Offset: 0x0007DFA1
		public static void Hide(object target)
		{
			if (KontextMenu.instance == null)
			{
				return;
			}
			if (target != null && target != KontextMenu.instance.target)
			{
				return;
			}
			if (KontextMenu.instance.fadeGroup.IsHidingInProgress)
			{
				return;
			}
			KontextMenu.instance.InstanceHide();
		}

		// Token: 0x040018B2 RID: 6322
		private static KontextMenu instance;

		// Token: 0x040018B3 RID: 6323
		private RectTransform rectTransform;

		// Token: 0x040018B4 RID: 6324
		[SerializeField]
		private KontextMenuEntry entryPrefab;

		// Token: 0x040018B5 RID: 6325
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040018B6 RID: 6326
		[SerializeField]
		private float positionMoveCloseThreshold = 10f;

		// Token: 0x040018B7 RID: 6327
		private object target;

		// Token: 0x040018B8 RID: 6328
		private bool isWatchingRectTransform;

		// Token: 0x040018B9 RID: 6329
		private RectTransform watchRectTransform;

		// Token: 0x040018BA RID: 6330
		private Vector3 cachedTransformPosition;

		// Token: 0x040018BB RID: 6331
		private PrefabPool<KontextMenuEntry> _entryPool;
	}
}
