using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace EPOOutline.Demo
{
	// Token: 0x02000006 RID: 6
	public class Chicken : MonoBehaviour
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000023E4 File Offset: 0x000005E4
		private void Awake()
		{
			this.agent = base.GetComponent<NavMeshAgent>();
			this.outlinable = base.GetComponent<Outlinable>();
			this.animator = base.GetComponent<Animator>();
			if (!this.alwaysActive)
			{
				this.outlinable.enabled = false;
			}
			this.agent.avoidancePriority = Chicken.priority++;
			if (this.updateChicken)
			{
				base.StartCoroutine(this.UpdateChicken());
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002456 File Offset: 0x00000656
		private void OnTriggerEnter(Collider other)
		{
			if (this.alwaysActive)
			{
				return;
			}
			if (!other.GetComponent<Character>())
			{
				return;
			}
			this.enteredCount++;
			this.outlinable.enabled = true;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000248C File Offset: 0x0000068C
		private void OnTriggerExit(Collider other)
		{
			if (this.alwaysActive)
			{
				return;
			}
			if (!other.GetComponent<Character>())
			{
				return;
			}
			int num = this.enteredCount - 1;
			this.enteredCount = num;
			if (num != 0)
			{
				return;
			}
			this.outlinable.enabled = false;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000024D0 File Offset: 0x000006D0
		private IEnumerator UpdateChicken()
		{
			NavMeshPath path = new NavMeshPath();
			for (;;)
			{
				this.animator.CrossFade("Walk In Place", 0.1f);
				Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
				Vector3 b = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y) * this.searchRadius;
				NavMeshHit navMeshHit;
				if (!NavMesh.SamplePosition(base.transform.position + b, out navMeshHit, this.searchRadius, -1))
				{
					yield return null;
				}
				else
				{
					Debug.DrawLine(base.transform.position, navMeshHit.position, Color.yellow, 3f);
					if (!NavMesh.CalculatePath(base.transform.position, navMeshHit.position, -1, path))
					{
						yield return null;
					}
					else
					{
						this.agent.destination = navMeshHit.position;
						while (this.agent.pathStatus != NavMeshPathStatus.PathComplete)
						{
							yield return null;
						}
						float timeToWait = this.agent.remainingDistance / this.agent.speed * 1.5f;
						while (this.agent.remainingDistance > this.agent.stoppingDistance && timeToWait > 0f)
						{
							timeToWait -= Time.deltaTime;
							yield return null;
						}
						this.animator.CrossFade("Eat", 0.1f);
						yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 5f));
						yield return null;
					}
				}
			}
			yield break;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000024DF File Offset: 0x000006DF
		private void OnDrawGizmos()
		{
			Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
			Gizmos.DrawSphere(base.transform.position, this.searchRadius);
		}

		// Token: 0x04000013 RID: 19
		[SerializeField]
		private bool alwaysActive;

		// Token: 0x04000014 RID: 20
		[SerializeField]
		private bool updateChicken = true;

		// Token: 0x04000015 RID: 21
		[SerializeField]
		private float searchRadius = 5f;

		// Token: 0x04000016 RID: 22
		private Outlinable outlinable;

		// Token: 0x04000017 RID: 23
		private NavMeshAgent agent;

		// Token: 0x04000018 RID: 24
		private Animator animator;

		// Token: 0x04000019 RID: 25
		private int enteredCount;

		// Token: 0x0400001A RID: 26
		private static int priority;
	}
}
