using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000B9 RID: 185
[RequireComponent(typeof(Rigidbody))]
public class Zone : MonoBehaviour
{
	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001B740 File Offset: 0x00019940
	public HashSet<Health> Healths
	{
		get
		{
			return this.healths;
		}
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x0001B748 File Offset: 0x00019948
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.healths = new HashSet<Health>();
		this.rb.isKinematic = true;
		this.rb.useGravity = false;
		this.sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
		if (this.setActiveByDistance)
		{
			SetActiveByPlayerDistance.Register(base.gameObject, this.sceneBuildIndex);
		}
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0001B7B0 File Offset: 0x000199B0
	private void OnDestroy()
	{
		if (this.setActiveByDistance)
		{
			SetActiveByPlayerDistance.Unregister(base.gameObject, this.sceneBuildIndex);
		}
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0001B7CC File Offset: 0x000199CC
	private void OnTriggerEnter(Collider other)
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (other.gameObject.layer != LayerMask.NameToLayer("Character"))
		{
			return;
		}
		Health component = other.GetComponent<Health>();
		if (component == null)
		{
			return;
		}
		if (this.onlyPlayerTeam && component.team != Teams.player)
		{
			return;
		}
		if (!this.healths.Contains(component))
		{
			this.healths.Add(component);
		}
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0001B838 File Offset: 0x00019A38
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Character"))
		{
			return;
		}
		Health component = other.GetComponent<Health>();
		if (component == null)
		{
			return;
		}
		if (this.onlyPlayerTeam && component.team != Teams.player)
		{
			return;
		}
		if (this.healths.Contains(component))
		{
			this.healths.Remove(component);
		}
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0001B89A File Offset: 0x00019A9A
	private void OnDisable()
	{
		this.healths.Clear();
	}

	// Token: 0x040005B4 RID: 1460
	public bool onlyPlayerTeam;

	// Token: 0x040005B5 RID: 1461
	private HashSet<Health> healths;

	// Token: 0x040005B6 RID: 1462
	public bool setActiveByDistance = true;

	// Token: 0x040005B7 RID: 1463
	private Rigidbody rb;

	// Token: 0x040005B8 RID: 1464
	private int sceneBuildIndex = -1;
}
