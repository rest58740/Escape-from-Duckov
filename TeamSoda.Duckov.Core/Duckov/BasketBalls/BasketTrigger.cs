using System;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.BasketBalls
{
	// Token: 0x02000327 RID: 807
	public class BasketTrigger : MonoBehaviour
	{
		// Token: 0x06001ABB RID: 6843 RVA: 0x000610F0 File Offset: 0x0005F2F0
		private void OnTriggerEnter(Collider other)
		{
			Debug.Log("ONTRIGGERENTER:" + other.name);
			BasketBall component = other.GetComponent<BasketBall>();
			if (component == null)
			{
				return;
			}
			this.onGoal.Invoke(component);
		}

		// Token: 0x0400134C RID: 4940
		public UnityEvent<BasketBall> onGoal;
	}
}
