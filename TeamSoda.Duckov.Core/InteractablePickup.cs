using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class InteractablePickup : InteractableBase
{
	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000737 RID: 1847 RVA: 0x00020C5D File Offset: 0x0001EE5D
	public DuckovItemAgent ItemAgent
	{
		get
		{
			return this.itemAgent;
		}
	}

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x06000738 RID: 1848 RVA: 0x00020C68 File Offset: 0x0001EE68
	// (remove) Token: 0x06000739 RID: 1849 RVA: 0x00020C9C File Offset: 0x0001EE9C
	public static event Action<InteractablePickup, CharacterMainControl> OnPickupSuccess;

	// Token: 0x0600073A RID: 1850 RVA: 0x00020CCF File Offset: 0x0001EECF
	protected override bool IsInteractable()
	{
		return true;
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x00020CD4 File Offset: 0x0001EED4
	public void OnInit()
	{
		if (this.itemAgent && this.itemAgent.Item && this.sprite)
		{
			this.sprite.sprite = this.itemAgent.Item.Icon;
		}
		this.overrideInteractName = true;
		base.InteractName = this.itemAgent.Item.DisplayNameRaw;
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x00020D48 File Offset: 0x0001EF48
	protected override void OnInteractStart(CharacterMainControl character)
	{
		bool flag = character.PickupItem(this.itemAgent.Item);
		try
		{
			if (flag)
			{
				Action<InteractablePickup, CharacterMainControl> onPickupSuccess = InteractablePickup.OnPickupSuccess;
				if (onPickupSuccess != null)
				{
					onPickupSuccess(this, character);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
		base.StopInteract();
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00020D9C File Offset: 0x0001EF9C
	public void Throw(Vector3 direction, float randomAngle)
	{
		this.throwStartPoint = base.transform.position;
		if (!this.rb)
		{
			this.rb = base.gameObject.AddComponent<Rigidbody>();
		}
		this.rb.collisionDetectionMode = 1;
		this.rb.constraints = 112;
		if (direction.magnitude < 0.1f)
		{
			direction = Vector3.zero;
		}
		else
		{
			direction.y = 0f;
			direction.Normalize();
			direction = Quaternion.Euler(0f, Random.Range(-randomAngle, randomAngle) * 0.5f, 0f) * direction;
			direction *= Random.Range(0.5f, 1f) * 3f;
			direction.y = 2.5f;
		}
		this.rb.velocity = direction;
		this.DestroyRigidbody().Forget();
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x00020E83 File Offset: 0x0001F083
	protected override void OnDestroy()
	{
		this.destroied = true;
		base.OnDestroy();
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x00020E94 File Offset: 0x0001F094
	private UniTaskVoid DestroyRigidbody()
	{
		InteractablePickup.<DestroyRigidbody>d__15 <DestroyRigidbody>d__;
		<DestroyRigidbody>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<DestroyRigidbody>d__.<>4__this = this;
		<DestroyRigidbody>d__.<>1__state = -1;
		<DestroyRigidbody>d__.<>t__builder.Start<InteractablePickup.<DestroyRigidbody>d__15>(ref <DestroyRigidbody>d__);
		return <DestroyRigidbody>d__.<>t__builder.Task;
	}

	// Token: 0x040006ED RID: 1773
	[SerializeField]
	private DuckovItemAgent itemAgent;

	// Token: 0x040006EE RID: 1774
	public SpriteRenderer sprite;

	// Token: 0x040006EF RID: 1775
	private Rigidbody rb;

	// Token: 0x040006F0 RID: 1776
	private Vector3 throwStartPoint;

	// Token: 0x040006F1 RID: 1777
	private bool destroied;
}
