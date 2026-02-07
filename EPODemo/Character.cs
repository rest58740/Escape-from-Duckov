using System;
using UnityEngine;
using UnityEngine.AI;

namespace EPOOutline.Demo
{
	// Token: 0x02000005 RID: 5
	public class Character : MonoBehaviour
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002262 File Offset: 0x00000462
		private void Start()
		{
			this.initialWalkVolume = this.walkSource.volume;
			this.mainCamera = Camera.main;
			this.agent.updateRotation = false;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000228C File Offset: 0x0000048C
		private void Update()
		{
			Vector3 forward = this.mainCamera.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			Vector3 right = this.mainCamera.transform.right;
			right.y = 0f;
			right.Normalize();
			Vector3 forward2 = forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal");
			if (forward2.magnitude > 0.1f)
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(forward2), Time.deltaTime * this.agent.angularSpeed);
			}
			this.agent.velocity = forward2.normalized * this.agent.speed;
			this.walkSource.volume = this.initialWalkVolume * (this.agent.velocity.magnitude / this.agent.speed);
			this.characterAnimator.SetBool("IsRunning", forward2.magnitude > 0.1f);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023B8 File Offset: 0x000005B8
		private void OnTriggerEnter(Collider other)
		{
			ICollectable component = other.GetComponent<ICollectable>();
			if (component == null)
			{
				return;
			}
			component.Collect(base.gameObject);
		}

		// Token: 0x0400000E RID: 14
		[SerializeField]
		private AudioSource walkSource;

		// Token: 0x0400000F RID: 15
		[SerializeField]
		private NavMeshAgent agent;

		// Token: 0x04000010 RID: 16
		[SerializeField]
		private Animator characterAnimator;

		// Token: 0x04000011 RID: 17
		private float initialWalkVolume;

		// Token: 0x04000012 RID: 18
		private Camera mainCamera;
	}
}
