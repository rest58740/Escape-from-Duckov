using System;
using Duckov;
using UnityEngine;

// Token: 0x0200012A RID: 298
public class Radar : MonoBehaviour
{
	// Token: 0x1700020D RID: 525
	// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0002B86A File Offset: 0x00029A6A
	private Vector3 CenterPosition
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0002B877 File Offset: 0x00029A77
	// (set) Token: 0x060009E9 RID: 2537 RVA: 0x0002B87F File Offset: 0x00029A7F
	public Vector3 TargetPosition { get; set; }

	// Token: 0x060009EA RID: 2538 RVA: 0x0002B888 File Offset: 0x00029A88
	private Transform GetTarget()
	{
		if (this.overrideTarget != null)
		{
			return this.overrideTarget;
		}
		return Radar.Target;
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x060009EB RID: 2539 RVA: 0x0002B8A4 File Offset: 0x00029AA4
	// (set) Token: 0x060009EC RID: 2540 RVA: 0x0002B8AB File Offset: 0x00029AAB
	public static Transform Target
	{
		get
		{
			return Radar._sTarget;
		}
		set
		{
			Radar._sTarget = value;
			Action<Transform> onSetTarget = Radar.OnSetTarget;
			if (onSetTarget == null)
			{
				return;
			}
			onSetTarget(Radar._sTarget);
		}
	}

	// Token: 0x1400004A RID: 74
	// (add) Token: 0x060009ED RID: 2541 RVA: 0x0002B8C8 File Offset: 0x00029AC8
	// (remove) Token: 0x060009EE RID: 2542 RVA: 0x0002B8FC File Offset: 0x00029AFC
	public static event Action<Transform> OnSetTarget;

	// Token: 0x060009EF RID: 2543 RVA: 0x0002B92F File Offset: 0x00029B2F
	private Vector3 GetDelta()
	{
		return this.TargetPosition - this.CenterPosition;
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x0002B944 File Offset: 0x00029B44
	private float CalculateFrequency()
	{
		float magnitude = this.GetDelta().magnitude;
		if (Mathf.Abs(magnitude) < this.nearestDist)
		{
			return this.maxFreq;
		}
		return this.maxFreq / (magnitude + 1f - this.nearestDist);
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x0002B98A File Offset: 0x00029B8A
	private void OnEnable()
	{
		this.buffer = 1f;
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x0002B998 File Offset: 0x00029B98
	private void Update()
	{
		Transform target = this.GetTarget();
		if (target != null)
		{
			this.TargetPosition = target.position;
		}
		float num = this.CalculateFrequency();
		this.buffer += num * Time.deltaTime;
		if (this.buffer > 1f)
		{
			this.buffer = 0f;
			this.Beep();
		}
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x0002B9FA File Offset: 0x00029BFA
	private void Beep()
	{
		AudioManager.Post("SFX/Special/Radar/Beep", base.gameObject);
	}

	// Token: 0x040008CF RID: 2255
	[Min(0.1f)]
	[SerializeField]
	private float nearestDist = 0.5f;

	// Token: 0x040008D0 RID: 2256
	[Min(1f)]
	[SerializeField]
	private float maxFreq = 10f;

	// Token: 0x040008D2 RID: 2258
	public Transform overrideTarget;

	// Token: 0x040008D3 RID: 2259
	private static Transform _sTarget;

	// Token: 0x040008D5 RID: 2261
	private float buffer;
}
