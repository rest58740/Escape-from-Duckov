using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.UI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000113 RID: 275
public class CountDownArea : MonoBehaviour
{
	// Token: 0x17000204 RID: 516
	// (get) Token: 0x0600098A RID: 2442 RVA: 0x0002AC3A File Offset: 0x00028E3A
	public float RequiredExtrationTime
	{
		get
		{
			return this.requiredExtrationTime;
		}
	}

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x0600098B RID: 2443 RVA: 0x0002AC42 File Offset: 0x00028E42
	private float TimeSinceCountDownBegan
	{
		get
		{
			return Time.time - this.timeWhenCountDownBegan;
		}
	}

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x0600098C RID: 2444 RVA: 0x0002AC50 File Offset: 0x00028E50
	public float RemainingTime
	{
		get
		{
			return Mathf.Clamp(this.RequiredExtrationTime - this.TimeSinceCountDownBegan, 0f, this.RequiredExtrationTime);
		}
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x0600098D RID: 2445 RVA: 0x0002AC6F File Offset: 0x00028E6F
	public float Progress
	{
		get
		{
			if (this.requiredExtrationTime <= 0f)
			{
				return 1f;
			}
			return this.TimeSinceCountDownBegan / this.RequiredExtrationTime;
		}
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x0002AC94 File Offset: 0x00028E94
	private void OnTriggerEnter(Collider other)
	{
		if (!base.enabled)
		{
			return;
		}
		CharacterMainControl component = other.GetComponent<CharacterMainControl>();
		if (component == null)
		{
			return;
		}
		if (component.IsMainCharacter())
		{
			this.hoveringMainCharacters.Add(component);
			this.OnHoveringMainCharactersChanged();
		}
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x0002ACD8 File Offset: 0x00028ED8
	private void OnTriggerExit(Collider other)
	{
		if (!base.enabled)
		{
			return;
		}
		CharacterMainControl component = other.GetComponent<CharacterMainControl>();
		if (component == null)
		{
			return;
		}
		if (component.IsMainCharacter())
		{
			this.hoveringMainCharacters.Remove(component);
			this.OnHoveringMainCharactersChanged();
		}
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x0002AD1A File Offset: 0x00028F1A
	private void OnHoveringMainCharactersChanged()
	{
		if (!this.countingDown && this.hoveringMainCharacters.Count > 0)
		{
			this.BeginCountDown();
			return;
		}
		if (this.countingDown && this.hoveringMainCharacters.Count < 1)
		{
			this.AbortCountDown();
		}
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x0002AD55 File Offset: 0x00028F55
	private void BeginCountDown()
	{
		this.countingDown = true;
		this.timeWhenCountDownBegan = Time.time;
		UnityEvent<CountDownArea> unityEvent = this.onCountDownStarted;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(this);
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x0002AD7A File Offset: 0x00028F7A
	private void AbortCountDown()
	{
		this.countingDown = false;
		this.timeWhenCountDownBegan = float.MaxValue;
		UnityEvent<CountDownArea> unityEvent = this.onCountDownStopped;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(this);
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0002ADA0 File Offset: 0x00028FA0
	private void UpdateCountDown()
	{
		if (this.hoveringMainCharacters.All((CharacterMainControl e) => e.Health.IsDead))
		{
			this.AbortCountDown();
		}
		if (this.TimeSinceCountDownBegan >= this.RequiredExtrationTime)
		{
			this.OnCountdownSucceed();
		}
		int num = (int)(this.RemainingTime + Time.deltaTime);
		if ((int)this.RemainingTime != num)
		{
			UnityEvent unityEvent = this.onTickSecond;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x0002AE1B File Offset: 0x0002901B
	private void OnCountdownSucceed()
	{
		UnityEvent<CountDownArea> unityEvent = this.onCountDownStopped;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		UnityEvent unityEvent2 = this.onCountDownSucceed;
		if (unityEvent2 != null)
		{
			unityEvent2.Invoke();
		}
		this.countingDown = false;
		if (this.disableWhenSucceed)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x0002AE56 File Offset: 0x00029056
	private void Update()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.countingDown && View.ActiveView == null)
		{
			this.UpdateCountDown();
		}
	}

	// Token: 0x0400089A RID: 2202
	[SerializeField]
	private float requiredExtrationTime = 5f;

	// Token: 0x0400089B RID: 2203
	[SerializeField]
	private bool disableWhenSucceed = true;

	// Token: 0x0400089C RID: 2204
	public UnityEvent onCountDownSucceed;

	// Token: 0x0400089D RID: 2205
	public UnityEvent onTickSecond;

	// Token: 0x0400089E RID: 2206
	public UnityEvent<CountDownArea> onCountDownStarted;

	// Token: 0x0400089F RID: 2207
	public UnityEvent<CountDownArea> onCountDownStopped;

	// Token: 0x040008A0 RID: 2208
	private bool countingDown;

	// Token: 0x040008A1 RID: 2209
	private float timeWhenCountDownBegan = float.MaxValue;

	// Token: 0x040008A2 RID: 2210
	private HashSet<CharacterMainControl> hoveringMainCharacters = new HashSet<CharacterMainControl>();
}
