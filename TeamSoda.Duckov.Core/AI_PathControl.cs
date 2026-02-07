using System;
using Pathfinding;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class AI_PathControl : MonoBehaviour
{
	// Token: 0x17000064 RID: 100
	// (get) Token: 0x060001E5 RID: 485 RVA: 0x00009583 File Offset: 0x00007783
	public bool ReachedEndOfPath
	{
		get
		{
			return this.reachedEndOfPath;
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000958B File Offset: 0x0000778B
	public bool Moving
	{
		get
		{
			return this.moving;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x060001E7 RID: 487 RVA: 0x00009593 File Offset: 0x00007793
	public bool WaitingForPathResult
	{
		get
		{
			return this.waitingForPathResult;
		}
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000959B File Offset: 0x0000779B
	public void Start()
	{
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0000959D File Offset: 0x0000779D
	public void MoveToPos(Vector3 pos)
	{
		this.reachedEndOfPath = false;
		this.path = null;
		this.seeker.StartPath(base.transform.position, pos, new OnPathDelegate(this.OnPathComplete));
		this.waitingForPathResult = true;
	}

	// Token: 0x060001EA RID: 490 RVA: 0x000095D8 File Offset: 0x000077D8
	public void OnPathComplete(Path p)
	{
		if (!p.error)
		{
			this.path = p;
			this.currentWaypoint = 0;
			this.moving = true;
		}
		this.waitingForPathResult = false;
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00009600 File Offset: 0x00007800
	public void Update()
	{
		this.moving = (this.path != null);
		if (this.path == null)
		{
			return;
		}
		this.reachedEndOfPath = false;
		float num;
		for (;;)
		{
			num = Vector3.Distance(base.transform.position, this.path.vectorPath[this.currentWaypoint]);
			if (num >= this.nextWaypointDistance)
			{
				goto IL_80;
			}
			if (this.currentWaypoint + 1 >= this.path.vectorPath.Count)
			{
				break;
			}
			this.currentWaypoint++;
		}
		this.reachedEndOfPath = true;
		IL_80:
		Vector3 normalized = (this.path.vectorPath[this.currentWaypoint] - base.transform.position).normalized;
		if (this.reachedEndOfPath)
		{
			float num2 = Mathf.Sqrt(num / this.nextWaypointDistance);
			this.controller.SetMoveInput(normalized * num2);
			if (num < this.stopDistance)
			{
				this.path = null;
				this.controller.SetMoveInput(Vector2.zero);
				return;
			}
		}
		else
		{
			this.controller.SetMoveInput(normalized);
		}
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00009716 File Offset: 0x00007916
	public void StopMove()
	{
		this.path = null;
		this.controller.SetMoveInput(Vector3.zero);
		this.waitingForPathResult = false;
	}

	// Token: 0x0400019D RID: 413
	public Seeker seeker;

	// Token: 0x0400019E RID: 414
	public CharacterMainControl controller;

	// Token: 0x0400019F RID: 415
	public Path path;

	// Token: 0x040001A0 RID: 416
	public float nextWaypointDistance = 3f;

	// Token: 0x040001A1 RID: 417
	private int currentWaypoint;

	// Token: 0x040001A2 RID: 418
	private bool reachedEndOfPath;

	// Token: 0x040001A3 RID: 419
	public float stopDistance = 0.2f;

	// Token: 0x040001A4 RID: 420
	private bool moving;

	// Token: 0x040001A5 RID: 421
	private bool waitingForPathResult;
}
