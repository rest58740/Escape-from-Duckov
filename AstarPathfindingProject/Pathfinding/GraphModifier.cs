using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200006E RID: 110
	[ExecuteInEditMode]
	public abstract class GraphModifier : VersionedMonoBehaviour
	{
		// Token: 0x060003AF RID: 943 RVA: 0x00012C10 File Offset: 0x00010E10
		protected static List<T> GetModifiersOfType<T>() where T : GraphModifier
		{
			GraphModifier graphModifier = GraphModifier.root;
			List<T> list = new List<T>();
			while (graphModifier != null)
			{
				T t = graphModifier as T;
				if (t != null)
				{
					list.Add(t);
				}
				graphModifier = graphModifier.next;
			}
			return list;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00012C60 File Offset: 0x00010E60
		public static void FindAllModifiers()
		{
			GraphModifier[] array = UnityCompatibility.FindObjectsByTypeSorted<GraphModifier>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].enabled && array[i].next == null)
				{
					array[i].enabled = false;
					array[i].enabled = true;
				}
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00012CB0 File Offset: 0x00010EB0
		public static void TriggerEvent(GraphModifier.EventType type)
		{
			if (!Application.isPlaying)
			{
				GraphModifier.FindAllModifiers();
			}
			try
			{
				GraphModifier graphModifier = GraphModifier.root;
				if (type <= GraphModifier.EventType.PostUpdate)
				{
					switch (type)
					{
					case GraphModifier.EventType.PostScan:
						while (graphModifier != null)
						{
							graphModifier.OnPostScan();
							graphModifier = graphModifier.next;
						}
						break;
					case GraphModifier.EventType.PreScan:
						while (graphModifier != null)
						{
							graphModifier.OnPreScan();
							graphModifier = graphModifier.next;
						}
						break;
					case (GraphModifier.EventType)3:
						break;
					case GraphModifier.EventType.LatePostScan:
						while (graphModifier != null)
						{
							graphModifier.OnLatePostScan();
							graphModifier = graphModifier.next;
						}
						break;
					default:
						if (type != GraphModifier.EventType.PreUpdate)
						{
							if (type == GraphModifier.EventType.PostUpdate)
							{
								while (graphModifier != null)
								{
									graphModifier.OnGraphsPostUpdate();
									graphModifier = graphModifier.next;
								}
							}
						}
						else
						{
							while (graphModifier != null)
							{
								graphModifier.OnGraphsPreUpdate();
								graphModifier = graphModifier.next;
							}
						}
						break;
					}
				}
				else if (type != GraphModifier.EventType.PostCacheLoad)
				{
					if (type != GraphModifier.EventType.PostUpdateBeforeAreaRecalculation)
					{
						if (type == GraphModifier.EventType.PostGraphLoad)
						{
							while (graphModifier != null)
							{
								graphModifier.OnPostGraphLoad();
								graphModifier = graphModifier.next;
							}
						}
					}
					else
					{
						while (graphModifier != null)
						{
							graphModifier.OnGraphsPostUpdateBeforeAreaRecalculation();
							graphModifier = graphModifier.next;
						}
					}
				}
				else
				{
					while (graphModifier != null)
					{
						graphModifier.OnPostCacheLoad();
						graphModifier = graphModifier.next;
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00012E0C File Offset: 0x0001100C
		protected virtual void OnEnable()
		{
			this.RemoveFromLinkedList();
			this.AddToLinkedList();
			this.ConfigureUniqueID();
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00012E20 File Offset: 0x00011020
		protected virtual void OnDisable()
		{
			this.RemoveFromLinkedList();
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00012E28 File Offset: 0x00011028
		protected override void Awake()
		{
			base.Awake();
			this.ConfigureUniqueID();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00012E38 File Offset: 0x00011038
		private void ConfigureUniqueID()
		{
			GraphModifier x;
			if (GraphModifier.usedIDs.TryGetValue(this.uniqueID, out x) && x != this)
			{
				this.Reset();
			}
			GraphModifier.usedIDs[this.uniqueID] = this;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00012E79 File Offset: 0x00011079
		private void AddToLinkedList()
		{
			if (GraphModifier.root == null)
			{
				GraphModifier.root = this;
				return;
			}
			this.next = GraphModifier.root;
			GraphModifier.root.prev = this;
			GraphModifier.root = this;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00012EAC File Offset: 0x000110AC
		private void RemoveFromLinkedList()
		{
			if (GraphModifier.root == this)
			{
				GraphModifier.root = this.next;
				if (GraphModifier.root != null)
				{
					GraphModifier.root.prev = null;
				}
			}
			else
			{
				if (this.prev != null)
				{
					this.prev.next = this.next;
				}
				if (this.next != null)
				{
					this.next.prev = this.prev;
				}
			}
			this.prev = null;
			this.next = null;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00012F37 File Offset: 0x00011137
		protected virtual void OnDestroy()
		{
			GraphModifier.usedIDs.Remove(this.uniqueID);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void OnPostScan()
		{
		}

		// Token: 0x060003BA RID: 954 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void OnPreScan()
		{
		}

		// Token: 0x060003BB RID: 955 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void OnLatePostScan()
		{
		}

		// Token: 0x060003BC RID: 956 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void OnPostCacheLoad()
		{
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void OnPostGraphLoad()
		{
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void OnGraphsPreUpdate()
		{
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void OnGraphsPostUpdate()
		{
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void OnGraphsPostUpdateBeforeAreaRecalculation()
		{
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00012F4C File Offset: 0x0001114C
		protected override void Reset()
		{
			base.Reset();
			ulong num = (ulong)((long)UnityEngine.Random.Range(0, int.MaxValue));
			ulong num2 = (ulong)((ulong)((long)UnityEngine.Random.Range(0, int.MaxValue)) << 32);
			this.uniqueID = (num | num2);
			GraphModifier.usedIDs[this.uniqueID] = this;
		}

		// Token: 0x04000277 RID: 631
		private static GraphModifier root;

		// Token: 0x04000278 RID: 632
		private GraphModifier prev;

		// Token: 0x04000279 RID: 633
		private GraphModifier next;

		// Token: 0x0400027A RID: 634
		[SerializeField]
		[HideInInspector]
		protected ulong uniqueID;

		// Token: 0x0400027B RID: 635
		protected static Dictionary<ulong, GraphModifier> usedIDs = new Dictionary<ulong, GraphModifier>();

		// Token: 0x0200006F RID: 111
		public enum EventType
		{
			// Token: 0x0400027D RID: 637
			PostScan = 1,
			// Token: 0x0400027E RID: 638
			PreScan,
			// Token: 0x0400027F RID: 639
			LatePostScan = 4,
			// Token: 0x04000280 RID: 640
			PreUpdate = 8,
			// Token: 0x04000281 RID: 641
			PostUpdate = 16,
			// Token: 0x04000282 RID: 642
			PostCacheLoad = 32,
			// Token: 0x04000283 RID: 643
			PostUpdateBeforeAreaRecalculation = 64,
			// Token: 0x04000284 RID: 644
			PostGraphLoad = 128
		}
	}
}
