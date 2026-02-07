using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000A2 RID: 162
	public class fsCyclicReferenceManager
	{
		// Token: 0x06000602 RID: 1538 RVA: 0x0001151E File Offset: 0x0000F71E
		public fsCyclicReferenceManager()
		{
			this._objectIds = new Dictionary<object, int>(fsCyclicReferenceManager.ObjectReferenceEqualityComparator.Instance);
			this._marked = new Dictionary<int, object>();
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00011541 File Offset: 0x0000F741
		public void Clear()
		{
			this._depth = 0;
			this._nextId = 0;
			this._objectIds.Clear();
			this._marked.Clear();
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00011567 File Offset: 0x0000F767
		public bool Enter()
		{
			this._depth++;
			return this._depth == 1;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00011580 File Offset: 0x0000F780
		public bool Exit()
		{
			this._depth--;
			if (this._depth == 0)
			{
				this._nextId = 0;
				this._objectIds.Clear();
				this._marked.Clear();
			}
			if (this._depth < 0)
			{
				this._depth = 0;
				throw new InvalidOperationException("Internal Error - Mismatched Enter/Exit");
			}
			return this._depth == 0;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x000115E4 File Offset: 0x0000F7E4
		public object GetReferenceObject(int id)
		{
			object result = null;
			if (!this._marked.TryGetValue(id, ref result))
			{
				throw new InvalidOperationException("Internal Deserialization Error - Object definition has not been encountered for object with id=" + id.ToString() + "; have you reordered or modified the serialized data? If this is an issue with an unmodified Full Json implementation and unmodified serialization data, please report an issue with an included test case.");
			}
			return result;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00011620 File Offset: 0x0000F820
		public void AddReferenceWithId(int id, object reference)
		{
			this._marked[id] = reference;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00011630 File Offset: 0x0000F830
		public int GetReferenceId(object item)
		{
			int num;
			if (!this._objectIds.TryGetValue(item, ref num))
			{
				int nextId = this._nextId;
				this._nextId = nextId + 1;
				num = nextId;
				this._objectIds[item] = num;
			}
			return num;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001166D File Offset: 0x0000F86D
		public bool IsReference(object item)
		{
			return this._marked.ContainsKey(this.GetReferenceId(item));
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00011684 File Offset: 0x0000F884
		public void MarkSerialized(object item)
		{
			int referenceId = this.GetReferenceId(item);
			if (this._marked.ContainsKey(referenceId))
			{
				throw new InvalidOperationException("Internal Error - " + ((item != null) ? item.ToString() : null) + " has already been marked as serialized");
			}
			this._marked[referenceId] = item;
		}

		// Token: 0x040001DB RID: 475
		private Dictionary<object, int> _objectIds;

		// Token: 0x040001DC RID: 476
		private int _nextId;

		// Token: 0x040001DD RID: 477
		private Dictionary<int, object> _marked;

		// Token: 0x040001DE RID: 478
		private int _depth;

		// Token: 0x02000131 RID: 305
		private class ObjectReferenceEqualityComparator : IEqualityComparer<object>
		{
			// Token: 0x0600085C RID: 2140 RVA: 0x0001860E File Offset: 0x0001680E
			bool IEqualityComparer<object>.Equals(object x, object y)
			{
				return x == y;
			}

			// Token: 0x0600085D RID: 2141 RVA: 0x00018614 File Offset: 0x00016814
			int IEqualityComparer<object>.GetHashCode(object obj)
			{
				return RuntimeHelpers.GetHashCode(obj);
			}

			// Token: 0x0400030D RID: 781
			public static readonly IEqualityComparer<object> Instance = new fsCyclicReferenceManager.ObjectReferenceEqualityComparator();
		}
	}
}
