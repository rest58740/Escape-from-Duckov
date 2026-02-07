using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A7 RID: 167
public class FowSmoke : MonoBehaviour
{
	// Token: 0x060005CD RID: 1485 RVA: 0x0001A260 File Offset: 0x00018460
	private void Start()
	{
		this.UpdateSmoke().Forget();
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x0001A27C File Offset: 0x0001847C
	private UniTaskVoid UpdateSmoke()
	{
		FowSmoke.<UpdateSmoke>d__11 <UpdateSmoke>d__;
		<UpdateSmoke>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<UpdateSmoke>d__.<>4__this = this;
		<UpdateSmoke>d__.<>1__state = -1;
		<UpdateSmoke>d__.<>t__builder.Start<FowSmoke.<UpdateSmoke>d__11>(ref <UpdateSmoke>d__);
		return <UpdateSmoke>d__.<>t__builder.Task;
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0001A2BF File Offset: 0x000184BF
	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
	}

	// Token: 0x04000550 RID: 1360
	[SerializeField]
	private int res = 8;

	// Token: 0x04000551 RID: 1361
	[SerializeField]
	private float radius;

	// Token: 0x04000552 RID: 1362
	[SerializeField]
	private float height;

	// Token: 0x04000553 RID: 1363
	[SerializeField]
	private float thickness;

	// Token: 0x04000554 RID: 1364
	public Transform colParent;

	// Token: 0x04000555 RID: 1365
	public ParticleSystem[] particles;

	// Token: 0x04000556 RID: 1366
	public float startTime;

	// Token: 0x04000557 RID: 1367
	public float lifeTime;

	// Token: 0x04000558 RID: 1368
	public float particleFadeTime = 3f;

	// Token: 0x04000559 RID: 1369
	public UnityEvent beforeFadeOutEvent;
}
