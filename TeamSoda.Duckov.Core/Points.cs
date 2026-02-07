using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class Points : MonoBehaviour
{
	// Token: 0x06000ABF RID: 2751 RVA: 0x0002FBC0 File Offset: 0x0002DDC0
	public void SetYtoZero()
	{
		for (int i = 0; i < this.points.Count; i++)
		{
			this.points[i] = new Vector3(this.points[i].x, 0f, this.points[i].z);
		}
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x0002FC1B File Offset: 0x0002DE1B
	public void RemoveAllPoints()
	{
		this.points = new List<Vector3>();
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x0002FC28 File Offset: 0x0002DE28
	public List<Vector3> GetRandomPoints(int count)
	{
		List<Vector3> list = new List<Vector3>();
		list.AddRange(this.points);
		List<Vector3> list2 = new List<Vector3>();
		while (list2.Count < count && list.Count > 0)
		{
			int index = Random.Range(0, list.Count);
			Vector3 item = this.PointToWorld(list[index]);
			list2.Add(item);
			list.RemoveAt(index);
		}
		return list2;
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x0002FC8C File Offset: 0x0002DE8C
	public Vector3 GetRandomPoint()
	{
		int index = Random.Range(0, this.points.Count);
		return this.GetPoint(index);
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x0002FCB4 File Offset: 0x0002DEB4
	public Vector3 GetPoint(int index)
	{
		if (index >= this.points.Count)
		{
			return Vector3.zero;
		}
		Vector3 point = this.points[index];
		return this.PointToWorld(point);
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x0002FCE9 File Offset: 0x0002DEE9
	private Vector3 PointToWorld(Vector3 point)
	{
		if (!this.worldSpace)
		{
			point = base.transform.TransformPoint(point);
		}
		return point;
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x0002FD04 File Offset: 0x0002DF04
	public void SendPointsChangedMessage()
	{
		IOnPointsChanged component = base.GetComponent<IOnPointsChanged>();
		if (component != null)
		{
			component.OnPointsChanged();
		}
	}

	// Token: 0x04000978 RID: 2424
	public List<Vector3> points;

	// Token: 0x04000979 RID: 2425
	[HideInInspector]
	public int lastSelectedPointIndex = -1;

	// Token: 0x0400097A RID: 2426
	public bool worldSpace = true;

	// Token: 0x0400097B RID: 2427
	public bool syncToSelectedPoint;
}
