using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class BoundaryGenerator : MonoBehaviour
{
	// Token: 0x06000A7F RID: 2687 RVA: 0x0002D50C File Offset: 0x0002B70C
	public void UpdateColliderParameters()
	{
		if (this.colliderObjects == null || this.colliderObjects.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < this.colliderObjects.Count; i++)
		{
			if (i < this.points.Count - 1)
			{
				BoxCollider boxCollider = this.colliderObjects[i];
				if (!(boxCollider == null))
				{
					boxCollider.gameObject.layer = base.gameObject.layer;
					Vector3 vector = base.transform.TransformPoint(this.points[i]);
					Vector3 vector2 = base.transform.TransformPoint(this.points[i + 1]);
					float y = Mathf.Min(vector.y, vector2.y);
					vector.y = y;
					vector2.y = y;
					Vector3 normalized = (vector2 - vector).normalized;
					float num = Vector3.Distance(vector, vector2);
					boxCollider.size = new Vector3(this.colliderThickness, this.colliderHeight, num);
					boxCollider.transform.forward = normalized;
					boxCollider.transform.position = (vector + vector2) / 2f + Vector3.up * 0.5f * this.colliderHeight + Vector3.up * this.yOffset + boxCollider.transform.right * 0.5f * this.colliderThickness * (this.inverseFaceDirection ? -1f : 1f);
					if (this.provideContects)
					{
						boxCollider.providesContacts = true;
					}
				}
			}
		}
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0002D6C4 File Offset: 0x0002B8C4
	private void DestroyAllChildren()
	{
		while (base.transform.childCount > 0)
		{
			Transform child = base.transform.GetChild(0);
			child.SetParent(null);
			if (Application.isPlaying)
			{
				Object.Destroy(child.gameObject);
			}
			else
			{
				Object.DestroyImmediate(child.gameObject);
			}
		}
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0002D714 File Offset: 0x0002B914
	public void UpdateColliders()
	{
		this.DestroyAllChildren();
		if (this.colliderObjects == null)
		{
			this.colliderObjects = new List<BoxCollider>();
		}
		this.colliderObjects.Clear();
		for (int i = 0; i < this.points.Count - 1; i++)
		{
			BoxCollider item = new GameObject(string.Format("Collider_{0}", i))
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<BoxCollider>();
			this.colliderObjects.Add(item);
		}
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0002D798 File Offset: 0x0002B998
	public void SetYtoZero()
	{
		for (int i = 0; i < this.points.Count; i++)
		{
			this.points[i] = new Vector3(this.points[i].x, 0f, this.points[i].z);
		}
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0002D7F3 File Offset: 0x0002B9F3
	public void OnPointsUpdated(bool OnValidate = false)
	{
		if (this.points == null)
		{
			this.points = new List<Vector3>();
		}
		if (base.transform.childCount != this.points.Count - 1 && !OnValidate)
		{
			this.UpdateColliders();
		}
		this.UpdateColliderParameters();
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0002D831 File Offset: 0x0002BA31
	public void RemoveAllPoints()
	{
		this.points.Clear();
		this.OnPointsUpdated(false);
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0002D845 File Offset: 0x0002BA45
	public void RespawnColliders()
	{
		this.DestroyAllChildren();
		this.colliderObjects.Clear();
		this.OnPointsUpdated(false);
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x0002D85F File Offset: 0x0002BA5F
	private void OnValidate()
	{
		if (Application.isPlaying)
		{
			return;
		}
		this.OnPointsUpdated(true);
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0002D870 File Offset: 0x0002BA70
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		if (this.colliderObjects == null)
		{
			return;
		}
		if (this.colliderObjects.Count > 0)
		{
			foreach (Vector3 vector in this.points)
			{
				Gizmos.DrawCube(base.transform.TransformPoint(vector), Vector3.one * 0.15f);
			}
		}
	}

	// Token: 0x0400093E RID: 2366
	public List<Vector3> points;

	// Token: 0x0400093F RID: 2367
	[HideInInspector]
	public int lastSelectedPointIndex = -1;

	// Token: 0x04000940 RID: 2368
	public float colliderHeight = 1f;

	// Token: 0x04000941 RID: 2369
	public float yOffset;

	// Token: 0x04000942 RID: 2370
	public float colliderThickness = 0.1f;

	// Token: 0x04000943 RID: 2371
	public bool inverseFaceDirection;

	// Token: 0x04000944 RID: 2372
	public bool provideContects;

	// Token: 0x04000945 RID: 2373
	[SerializeField]
	[HideInInspector]
	private List<BoxCollider> colliderObjects;
}
