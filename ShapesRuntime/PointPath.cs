using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000038 RID: 56
	public class PointPath<T> : DisposableMesh
	{
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0001779B File Offset: 0x0001599B
		public int Count
		{
			get
			{
				return this.path.Count;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000BCB RID: 3019 RVA: 0x000177A8 File Offset: 0x000159A8
		public T LastPoint
		{
			get
			{
				return this.path[this.path.Count - 1];
			}
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x000177C2 File Offset: 0x000159C2
		protected void OnSetFirstDataPoint()
		{
			this.hasData = true;
			this.meshDirty = true;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x000177D2 File Offset: 0x000159D2
		public void ClearAllPoints()
		{
			this.path.Clear();
			this.hasData = false;
		}

		// Token: 0x170001BC RID: 444
		public T this[int i]
		{
			get
			{
				return this.path[i];
			}
			set
			{
				this.path[i] = value;
				this.meshDirty = true;
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0001780A File Offset: 0x00015A0A
		public void SetPoint(int index, T point)
		{
			this.path[index] = point;
			this.meshDirty = true;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00017820 File Offset: 0x00015A20
		public void RemovePointAt(int index)
		{
			int count = this.path.Count;
			if (index < 0 || index >= count)
			{
				throw new IndexOutOfRangeException();
			}
			this.path.RemoveAt(index);
			this.meshDirty = true;
			if (count == 1)
			{
				this.hasData = false;
			}
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00017865 File Offset: 0x00015A65
		public void AddPoint(T p)
		{
			if (!this.hasData)
			{
				this.OnSetFirstDataPoint();
			}
			this.path.Add(p);
			this.hasData = true;
			this.meshDirty = true;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0001788F File Offset: 0x00015A8F
		public void AddPoints(params T[] pts)
		{
			this.AddPoints(pts);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00017898 File Offset: 0x00015A98
		public void AddPoints(IEnumerable<T> ptsToAdd)
		{
			int count = this.path.Count;
			this.path.AddRange(ptsToAdd);
			if (this.path.Count - count > 0)
			{
				if (!this.hasData)
				{
					this.OnSetFirstDataPoint();
				}
				this.hasData = true;
				this.meshDirty = true;
			}
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x000178E9 File Offset: 0x00015AE9
		protected bool CheckCanAddContinuePoint([CallerMemberName] string callerName = null)
		{
			if (!this.hasData)
			{
				Debug.LogWarning(callerName + " requires adding a point before calling it, to determine starting point");
				return true;
			}
			return false;
		}

		// Token: 0x04000198 RID: 408
		protected List<T> path = new List<T>();
	}
}
