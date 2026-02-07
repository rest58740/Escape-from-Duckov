using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.MiniGames.Utilities
{
	// Token: 0x02000295 RID: 661
	public class ControllerPickupAnimation : MonoBehaviour
	{
		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x0005007C File Offset: 0x0004E27C
		private AnimationCurve pickupRotCurve
		{
			get
			{
				return this.pickupCurve;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x00050084 File Offset: 0x0004E284
		private AnimationCurve pickupPosCurve
		{
			get
			{
				return this.pickupCurve;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001575 RID: 5493 RVA: 0x0005008C File Offset: 0x0004E28C
		private AnimationCurve putDownCurve
		{
			get
			{
				return this.pickupCurve;
			}
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x00050094 File Offset: 0x0004E294
		public UniTask PickUp(Transform endTransform)
		{
			ControllerPickupAnimation.<PickUp>d__11 <PickUp>d__;
			<PickUp>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<PickUp>d__.<>4__this = this;
			<PickUp>d__.endTransform = endTransform;
			<PickUp>d__.<>1__state = -1;
			<PickUp>d__.<>t__builder.Start<ControllerPickupAnimation.<PickUp>d__11>(ref <PickUp>d__);
			return <PickUp>d__.<>t__builder.Task;
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x000500E0 File Offset: 0x0004E2E0
		public UniTask PutDown()
		{
			ControllerPickupAnimation.<PutDown>d__12 <PutDown>d__;
			<PutDown>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<PutDown>d__.<>4__this = this;
			<PutDown>d__.<>1__state = -1;
			<PutDown>d__.<>t__builder.Start<ControllerPickupAnimation.<PutDown>d__12>(ref <PutDown>d__);
			return <PutDown>d__.<>t__builder.Task;
		}

		// Token: 0x04000FBC RID: 4028
		[SerializeField]
		private Transform restTransform;

		// Token: 0x04000FBD RID: 4029
		[SerializeField]
		private Transform controllerTransform;

		// Token: 0x04000FBE RID: 4030
		[SerializeField]
		private float transitionTime = 1f;

		// Token: 0x04000FBF RID: 4031
		[SerializeField]
		private AnimationCurve pickupCurve;

		// Token: 0x04000FC0 RID: 4032
		private int activeToken;
	}
}
