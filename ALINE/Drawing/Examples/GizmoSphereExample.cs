using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Drawing.Examples
{
	// Token: 0x0200005F RID: 95
	public class GizmoSphereExample : MonoBehaviourGizmos
	{
		// Token: 0x060003C4 RID: 964 RVA: 0x00012AD0 File Offset: 0x00010CD0
		public override void DrawGizmos()
		{
			using (Draw.InLocalSpace(base.transform))
			{
				Draw.WireSphere(Vector3.zero, 0.5f, this.gizmoColor);
				foreach (GizmoSphereExample.Contact contact in this.contactForces.Values)
				{
					Draw.Circle(contact.lastPoint, contact.lastNormal, 0.1f * contact.impulse, this.gizmoColor2);
					Draw.SolidCircle(contact.lastPoint, contact.lastNormal, 0.1f * contact.impulse, this.gizmoColor2);
				}
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00012BBC File Offset: 0x00010DBC
		private void FixedUpdate()
		{
			foreach (Collider key in this.contactForces.Keys.ToList<Collider>())
			{
				GizmoSphereExample.Contact contact = this.contactForces[key];
				if (contact.impulse > 0.1f)
				{
					contact.impulse = Mathf.Lerp(contact.impulse, 0f, 10f * Time.fixedDeltaTime);
					contact.smoothImpulse = Mathf.Lerp(contact.impulse, contact.smoothImpulse, 20f * Time.fixedDeltaTime);
					this.contactForces[key] = contact;
				}
				else
				{
					this.contactForces.Remove(key);
				}
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00012C94 File Offset: 0x00010E94
		private void OnCollisionStay(Collision collision)
		{
			ContactPoint[] contacts = collision.contacts;
			int num = 0;
			if (num >= contacts.Length)
			{
				return;
			}
			ContactPoint contactPoint = contacts[num];
			if (!this.contactForces.ContainsKey(collision.collider))
			{
				this.contactForces.Add(collision.collider, new GizmoSphereExample.Contact
				{
					impulse = 2f
				});
			}
			GizmoSphereExample.Contact contact = this.contactForces[collision.collider];
			contact.impulse = Mathf.Max(contact.impulse, 1f);
			contact.lastPoint = base.transform.InverseTransformPoint(contactPoint.point);
			contact.lastNormal = base.transform.InverseTransformVector(contactPoint.normal);
			this.contactForces[collision.collider] = contact;
		}

		// Token: 0x04000187 RID: 391
		public Color gizmoColor = new Color(1f, 0.34509805f, 0.33333334f);

		// Token: 0x04000188 RID: 392
		public Color gizmoColor2 = new Color(0.30980393f, 0.8f, 0.92941177f);

		// Token: 0x04000189 RID: 393
		private Dictionary<Collider, GizmoSphereExample.Contact> contactForces = new Dictionary<Collider, GizmoSphereExample.Contact>();

		// Token: 0x02000060 RID: 96
		private struct Contact
		{
			// Token: 0x0400018A RID: 394
			public float impulse;

			// Token: 0x0400018B RID: 395
			public float smoothImpulse;

			// Token: 0x0400018C RID: 396
			public Vector3 lastPoint;

			// Token: 0x0400018D RID: 397
			public Vector3 lastNormal;
		}
	}
}
