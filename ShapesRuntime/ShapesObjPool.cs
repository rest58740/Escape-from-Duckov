using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200007B RID: 123
	public abstract class ShapesObjPool<T, P> : MonoBehaviour where T : Component where P : ShapesObjPool<T, P>
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x0001C7C8 File Offset: 0x0001A9C8
		private int ElementCount
		{
			get
			{
				return this.elementsPassive.Count + this.elementsActive.Count;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x0001C7E1 File Offset: 0x0001A9E1
		public T ImmediateModeElement
		{
			get
			{
				return this.GetElement(-1);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x0001C7EA File Offset: 0x0001A9EA
		public static int InstanceElementCount
		{
			get
			{
				if (!ShapesObjPool<T, P>.InstanceExists)
				{
					return 0;
				}
				return ShapesObjPool<T, P>.Instance.ElementCount;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x0001C804 File Offset: 0x0001AA04
		public static int InstanceElementCountActive
		{
			get
			{
				if (!ShapesObjPool<T, P>.InstanceExists)
				{
					return 0;
				}
				return ShapesObjPool<T, P>.Instance.elementsActive.Count;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0001C823 File Offset: 0x0001AA23
		public static bool InstanceExists
		{
			get
			{
				return ShapesObjPool<T, P>.instance != null;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000D2B RID: 3371
		public abstract string PoolTypeName { get; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x0001C835 File Offset: 0x0001AA35
		public static P Instance
		{
			get
			{
				if (ShapesObjPool<T, P>.instance == null)
				{
					ShapesObjPool<T, P>.instance = Object.FindAnyObjectByType<P>();
					if (ShapesObjPool<T, P>.instance == null)
					{
						ShapesObjPool<T, P>.instance = ShapesObjPool<T, P>.CreatePool();
					}
				}
				return ShapesObjPool<T, P>.instance;
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0001C874 File Offset: 0x0001AA74
		private static P CreatePool()
		{
			GameObject gameObject = new GameObject();
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(gameObject);
			}
			P result = gameObject.AddComponent<P>();
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			return result;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0001C8A4 File Offset: 0x0001AAA4
		private void ClearData()
		{
			for (int i = base.transform.childCount - 1; i >= 0; i--)
			{
				base.transform.GetChild(i).gameObject.DestroyBranched();
			}
			this.elementsPassive.Clear();
			this.elementsActive.Clear();
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0001C8F5 File Offset: 0x0001AAF5
		private void OnEnable()
		{
			base.gameObject.name = "Shapes " + this.PoolTypeName + " Pool";
			this.ClearData();
			ShapesObjPool<T, P>.instance = (P)((object)this);
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0001C928 File Offset: 0x0001AB28
		private void OnDisable()
		{
			this.ClearData();
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0001C930 File Offset: 0x0001AB30
		public T GetElement(int id)
		{
			T result;
			if (!this.elementsActive.TryGetValue(id, ref result))
			{
				result = this.AllocateElement(id);
			}
			return result;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0001C958 File Offset: 0x0001AB58
		public T AllocateElement(int id)
		{
			T t = default(T);
			while (t == null && this.elementsPassive.Count > 0)
			{
				t = this.elementsPassive.Pop();
			}
			if (t == null)
			{
				t = this.CreateElement(id);
			}
			this.elementsActive.Add(id, t);
			return t;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0001C9BC File Offset: 0x0001ABBC
		public void ReleaseElement(int id)
		{
			T t;
			if (this.elementsActive.TryGetValue(id, ref t))
			{
				this.elementsActive.Remove(id);
				this.elementsPassive.Push(t);
			}
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0001C9F4 File Offset: 0x0001ABF4
		private T CreateElement(int id)
		{
			int elementCount = this.ElementCount;
			if (elementCount > 1000)
			{
				Debug.LogError(string.Format("Text element allocation cap of {0} reached. You are probably leaking and not properly disposing {1} elements", 1000, this.PoolTypeName.ToLower()));
				return default(T);
			}
			if (elementCount > 500)
			{
				Debug.LogWarning(string.Format("Allocating more than {0} {1} elements. You are probably leaking and not properly disposing text objects", 500, this.PoolTypeName.ToLower()));
			}
			GameObject gameObject = new GameObject((id == -1) ? ("Immediate Mode " + this.PoolTypeName) : id.ToString());
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			T t = gameObject.AddComponent<T>();
			this.OnCreatedNewComponent(t);
			return t;
		}

		// Token: 0x06000D35 RID: 3381
		public abstract void OnCreatedNewComponent(T comp);

		// Token: 0x04000307 RID: 775
		private const int ALLOCATION_COUNT_WARNING = 500;

		// Token: 0x04000308 RID: 776
		private const int ALLOCATION_COUNT_CAP = 1000;

		// Token: 0x04000309 RID: 777
		private Stack<T> elementsPassive = new Stack<T>();

		// Token: 0x0400030A RID: 778
		private Dictionary<int, T> elementsActive = new Dictionary<int, T>();

		// Token: 0x0400030B RID: 779
		private static P instance;
	}
}
