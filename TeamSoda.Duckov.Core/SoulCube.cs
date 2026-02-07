using System;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class SoulCube : MonoBehaviour
{
	// Token: 0x0600060F RID: 1551 RVA: 0x0001B2D4 File Offset: 0x000194D4
	public void Init(SoulCollector collectorTarget)
	{
		this.target = collectorTarget;
		this.direction = Random.insideUnitSphere + Vector3.up;
		this.direction.Normalize();
		this.spawnSpeed = Random.Range(this.speedRange.x, this.speedRange.y);
		this.roatePart.transform.localRotation = Quaternion.Euler(Random.insideUnitSphere * 360f);
		this.rotateAxis = Random.insideUnitSphere;
		this.rotateSpeed = Random.Range(this.rotateSpeedRange.x, this.rotateSpeedRange.y);
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0001B37C File Offset: 0x0001957C
	private void Update()
	{
		this.roatePart.Rotate(this.rotateSpeed * this.rotateAxis * Time.deltaTime);
		if (this.target == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.stateTimer += Time.deltaTime;
		SoulCube.States states = this.currentState;
		if (states != SoulCube.States.spawn)
		{
			if (states != SoulCube.States.goToTarget)
			{
				return;
			}
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.target.transform.position, this.toTargetSpeed * Time.deltaTime);
			if (Vector3.Distance(base.transform.position, this.target.transform.position) < 0.3f)
			{
				this.AddCube();
			}
		}
		else
		{
			this.velocity = this.spawnSpeed * this.direction * this.spawnSpeedCurve.Evaluate(Mathf.Clamp01(this.stateTimer / this.spawnTime));
			base.transform.position += this.velocity * Time.deltaTime;
			if (this.stateTimer > this.spawnTime)
			{
				this.currentState = SoulCube.States.goToTarget;
				return;
			}
		}
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0001B4C3 File Offset: 0x000196C3
	private void AddCube()
	{
		if (this.target)
		{
			this.target.AddCube();
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000599 RID: 1433
	private SoulCube.States currentState;

	// Token: 0x0400059A RID: 1434
	private SoulCollector target;

	// Token: 0x0400059B RID: 1435
	private Vector3 direction;

	// Token: 0x0400059C RID: 1436
	private float stateTimer;

	// Token: 0x0400059D RID: 1437
	public Vector2 speedRange;

	// Token: 0x0400059E RID: 1438
	private float spawnSpeed;

	// Token: 0x0400059F RID: 1439
	public float spawnTime;

	// Token: 0x040005A0 RID: 1440
	public float toTargetSpeed;

	// Token: 0x040005A1 RID: 1441
	public AnimationCurve spawnSpeedCurve;

	// Token: 0x040005A2 RID: 1442
	private Vector3 velocity;

	// Token: 0x040005A3 RID: 1443
	public Transform roatePart;

	// Token: 0x040005A4 RID: 1444
	public Vector2 rotateSpeedRange = new Vector2(300f, 1000f);

	// Token: 0x040005A5 RID: 1445
	private float rotateSpeed;

	// Token: 0x040005A6 RID: 1446
	private Vector3 rotateAxis;

	// Token: 0x0200047C RID: 1148
	private enum States
	{
		// Token: 0x04001BF0 RID: 7152
		spawn,
		// Token: 0x04001BF1 RID: 7153
		goToTarget
	}
}
