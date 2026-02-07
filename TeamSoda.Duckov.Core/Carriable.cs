using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000A0 RID: 160
public class Carriable : MonoBehaviour
{
	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000570 RID: 1392 RVA: 0x000186F9 File Offset: 0x000168F9
	private Inventory inventory
	{
		get
		{
			if (this.lootbox == null)
			{
				return null;
			}
			return this.lootbox.Inventory;
		}
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x00018716 File Offset: 0x00016916
	public float GetWeight()
	{
		if (this.inventory)
		{
			return this.inventory.CachedWeight + this.selfWeight;
		}
		return this.selfWeight;
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x00018740 File Offset: 0x00016940
	public void Take(CA_Carry _carrier)
	{
		if (!_carrier)
		{
			return;
		}
		if (this.carrier)
		{
			this.carrier.StopAction();
		}
		this.droping = false;
		this.carrier = _carrier;
		if (this.inventory)
		{
			this.inventory.RecalculateWeight();
		}
		this.rb.transform.SetParent(this.carrier.characterController.modelRoot);
		this.rb.velocity = Vector3.zero;
		this.rb.transform.position = this.carrier.characterController.modelRoot.TransformPoint(this.carrier.carryPoint);
		this.rb.transform.localRotation = Quaternion.identity;
		this.SetRigidbodyActive(false);
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x00018814 File Offset: 0x00016A14
	private void SetRigidbodyActive(bool active)
	{
		if (active)
		{
			this.rb.isKinematic = false;
			this.rb.interpolation = 1;
			if (this.lootbox && this.lootbox.interactCollider)
			{
				this.lootbox.interactCollider.isTrigger = false;
				return;
			}
		}
		else
		{
			this.rb.isKinematic = true;
			this.rb.interpolation = 0;
			if (this.lootbox && this.lootbox.interactCollider)
			{
				this.lootbox.interactCollider.isTrigger = true;
			}
		}
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x000188B8 File Offset: 0x00016AB8
	public void Drop()
	{
		if (this.carrier.Running)
		{
			this.carrier.StopAction();
		}
		this.carrier = null;
		MultiSceneCore.MoveToActiveWithScene(this.rb.gameObject, SceneManager.GetActiveScene().buildIndex);
		this.DropTask().Forget();
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x00018910 File Offset: 0x00016B10
	public void OnCarriableUpdate(float deltaTime)
	{
		if (!this.carrier)
		{
			return;
		}
		Vector3 position = this.carrier.characterController.modelRoot.TransformPoint(this.carrier.carryPoint);
		if (this.carrier.characterController.RightHandSocket)
		{
			position.y = this.carrier.characterController.RightHandSocket.transform.position.y + this.carrier.carryPoint.y;
		}
		this.rb.transform.position = position;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x000189AC File Offset: 0x00016BAC
	private UniTaskVoid DropTask()
	{
		Carriable.<DropTask>d__14 <DropTask>d__;
		<DropTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<DropTask>d__.<>4__this = this;
		<DropTask>d__.<>1__state = -1;
		<DropTask>d__.<>t__builder.Start<Carriable.<DropTask>d__14>(ref <DropTask>d__);
		return <DropTask>d__.<>t__builder.Task;
	}

	// Token: 0x040004EE RID: 1262
	private CA_Carry carrier;

	// Token: 0x040004EF RID: 1263
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x040004F0 RID: 1264
	[SerializeField]
	private float selfWeight;

	// Token: 0x040004F1 RID: 1265
	public InteractableLootbox lootbox;

	// Token: 0x040004F2 RID: 1266
	private bool droping;

	// Token: 0x040004F3 RID: 1267
	private float startDropTime = -1f;

	// Token: 0x040004F4 RID: 1268
	private bool carring;
}
