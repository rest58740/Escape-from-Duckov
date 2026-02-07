using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000E3 RID: 227
public class OnTriggerEnterEvent : MonoBehaviour
{
	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000753 RID: 1875 RVA: 0x0002121F File Offset: 0x0001F41F
	private bool hideLayerMask
	{
		get
		{
			return this.onlyMainCharacter || this.filterByTeam;
		}
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x00021231 File Offset: 0x0001F431
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x0002123C File Offset: 0x0001F43C
	public void Init()
	{
		Collider component = base.GetComponent<Collider>();
		if (component)
		{
			component.isTrigger = true;
		}
		if (this.filterByTeam)
		{
			this.layerMask = 1 << LayerMask.NameToLayer("Character");
		}
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x00021281 File Offset: 0x0001F481
	private void OnCollisionEnter(Collision collision)
	{
		this.OnEvent(collision.gameObject, true);
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00021290 File Offset: 0x0001F490
	private void OnCollisionExit(Collision collision)
	{
		this.OnEvent(collision.gameObject, false);
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x0002129F File Offset: 0x0001F49F
	private void OnTriggerEnter(Collider other)
	{
		this.OnEvent(other.gameObject, true);
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x000212AE File Offset: 0x0001F4AE
	private void OnTriggerExit(Collider other)
	{
		this.OnEvent(other.gameObject, false);
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x000212C0 File Offset: 0x0001F4C0
	private void OnEvent(GameObject other, bool enter)
	{
		if (this.triggerOnce && this.triggered)
		{
			return;
		}
		if (this.onlyMainCharacter)
		{
			if (CharacterMainControl.Main == null || other != CharacterMainControl.Main.gameObject)
			{
				return;
			}
		}
		else
		{
			if ((1 << other.layer | this.layerMask) != this.layerMask)
			{
				return;
			}
			if (this.filterByTeam)
			{
				CharacterMainControl component = other.GetComponent<CharacterMainControl>();
				if (!component)
				{
					return;
				}
				Teams team = component.Team;
				if (!Team.IsEnemy(this.selfTeam, team))
				{
					return;
				}
			}
		}
		this.triggered = true;
		if (enter)
		{
			UnityEvent doOnTriggerEnter = this.DoOnTriggerEnter;
			if (doOnTriggerEnter == null)
			{
				return;
			}
			doOnTriggerEnter.Invoke();
			return;
		}
		else
		{
			UnityEvent doOnTriggerExit = this.DoOnTriggerExit;
			if (doOnTriggerExit == null)
			{
				return;
			}
			doOnTriggerExit.Invoke();
			return;
		}
	}

	// Token: 0x04000702 RID: 1794
	public bool onlyMainCharacter;

	// Token: 0x04000703 RID: 1795
	public bool filterByTeam;

	// Token: 0x04000704 RID: 1796
	public Teams selfTeam;

	// Token: 0x04000705 RID: 1797
	public LayerMask layerMask;

	// Token: 0x04000706 RID: 1798
	public bool triggerOnce;

	// Token: 0x04000707 RID: 1799
	public UnityEvent DoOnTriggerEnter = new UnityEvent();

	// Token: 0x04000708 RID: 1800
	public UnityEvent DoOnTriggerExit = new UnityEvent();

	// Token: 0x04000709 RID: 1801
	private bool triggered;

	// Token: 0x0400070A RID: 1802
	private bool mainCharacterIn;
}
