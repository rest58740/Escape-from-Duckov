using System;
using UnityEngine;
using VLB;

namespace VLB_Samples
{
	// Token: 0x0200004A RID: 74
	[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(MeshRenderer))]
	public class CheckIfInsideBeam : MonoBehaviour
	{
		// Token: 0x060002D7 RID: 727 RVA: 0x0000B800 File Offset: 0x00009A00
		private void Start()
		{
			this.m_Collider = base.GetComponent<Collider>();
			MeshRenderer component = base.GetComponent<MeshRenderer>();
			if (component)
			{
				this.m_Material = component.material;
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000B834 File Offset: 0x00009A34
		private void Update()
		{
			if (this.m_Material)
			{
				this.m_Material.SetColor("_Color", this.isInsideBeam ? Color.green : Color.red);
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000B867 File Offset: 0x00009A67
		private void FixedUpdate()
		{
			this.isInsideBeam = false;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000B870 File Offset: 0x00009A70
		private void OnTriggerStay(Collider trigger)
		{
			DynamicOcclusionRaycasting component = trigger.GetComponent<DynamicOcclusionRaycasting>();
			if (component)
			{
				this.isInsideBeam = !component.IsColliderHiddenByDynamicOccluder(this.m_Collider);
				return;
			}
			this.isInsideBeam = true;
		}

		// Token: 0x040001B8 RID: 440
		private bool isInsideBeam;

		// Token: 0x040001B9 RID: 441
		private Material m_Material;

		// Token: 0x040001BA RID: 442
		private Collider m_Collider;
	}
}
