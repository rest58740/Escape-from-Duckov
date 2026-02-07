using System;
using UnityEngine;

namespace ECM2
{
	// Token: 0x02000012 RID: 18
	public static class CollisionDetection
	{
		// Token: 0x06000263 RID: 611 RVA: 0x0000A268 File Offset: 0x00008468
		public static int Raycast(Vector3 origin, Vector3 direction, float distance, int layerMask, QueryTriggerInteraction queryTriggerInteraction, out RaycastHit closestHit, RaycastHit[] hits, IColliderFilter colliderFilter)
		{
			closestHit = default(RaycastHit);
			int num = Physics.RaycastNonAlloc(origin, direction, CollisionDetection.HitsBuffer, distance, layerMask, queryTriggerInteraction);
			if (num == 0)
			{
				return 0;
			}
			int result = 0;
			float num2 = float.PositiveInfinity;
			Array.Clear(hits, 0, hits.Length);
			for (int i = 0; i < num; i++)
			{
				if (CollisionDetection.HitsBuffer[i].distance > 0f && (colliderFilter == null || !colliderFilter.Filter(CollisionDetection.HitsBuffer[i].collider)))
				{
					if (CollisionDetection.HitsBuffer[i].distance < num2)
					{
						closestHit = CollisionDetection.HitsBuffer[i];
						num2 = closestHit.distance;
					}
					hits[result++] = CollisionDetection.HitsBuffer[i];
				}
			}
			return result;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000A32C File Offset: 0x0000852C
		public static int SphereCast(Vector3 origin, float radius, Vector3 direction, float distance, int layerMask, QueryTriggerInteraction queryTriggerInteraction, out RaycastHit closestHit, RaycastHit[] hits, IColliderFilter colliderFilter, float backStepDistance)
		{
			closestHit = default(RaycastHit);
			Vector3 origin2 = origin - direction * backStepDistance;
			float maxDistance = distance + backStepDistance;
			int num = Physics.SphereCastNonAlloc(origin2, radius, direction, CollisionDetection.HitsBuffer, maxDistance, layerMask, queryTriggerInteraction);
			if (num == 0)
			{
				return 0;
			}
			int result = 0;
			float num2 = float.PositiveInfinity;
			Array.Clear(hits, 0, hits.Length);
			for (int i = 0; i < num; i++)
			{
				if (CollisionDetection.HitsBuffer[i].distance > 0f && (colliderFilter == null || !colliderFilter.Filter(CollisionDetection.HitsBuffer[i].collider)))
				{
					RaycastHit[] hitsBuffer = CollisionDetection.HitsBuffer;
					int num3 = i;
					hitsBuffer[num3].distance = hitsBuffer[num3].distance - backStepDistance;
					if (CollisionDetection.HitsBuffer[i].distance < num2)
					{
						closestHit = CollisionDetection.HitsBuffer[i];
						num2 = closestHit.distance;
					}
					hits[result++] = CollisionDetection.HitsBuffer[i];
				}
			}
			return result;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000A430 File Offset: 0x00008630
		public static int CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float distance, int layerMask, QueryTriggerInteraction queryTriggerInteraction, out RaycastHit closestHit, RaycastHit[] hits, IColliderFilter colliderFilter, float backStepDistance)
		{
			closestHit = default(RaycastHit);
			Vector3 point3 = point1 - direction * backStepDistance;
			Vector3 point4 = point2 - direction * backStepDistance;
			float maxDistance = distance + backStepDistance;
			int num = Physics.CapsuleCastNonAlloc(point3, point4, radius, direction, CollisionDetection.HitsBuffer, maxDistance, layerMask, queryTriggerInteraction);
			if (num == 0)
			{
				return 0;
			}
			int result = 0;
			float num2 = float.PositiveInfinity;
			Array.Clear(hits, 0, hits.Length);
			for (int i = 0; i < num; i++)
			{
				if (CollisionDetection.HitsBuffer[i].distance > 0f && (colliderFilter == null || !colliderFilter.Filter(CollisionDetection.HitsBuffer[i].collider)))
				{
					RaycastHit[] hitsBuffer = CollisionDetection.HitsBuffer;
					int num3 = i;
					hitsBuffer[num3].distance = hitsBuffer[num3].distance - backStepDistance;
					if (CollisionDetection.HitsBuffer[i].distance < num2)
					{
						closestHit = CollisionDetection.HitsBuffer[i];
						num2 = closestHit.distance;
					}
					hits[result++] = CollisionDetection.HitsBuffer[i];
				}
			}
			return result;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000A548 File Offset: 0x00008748
		public static int OverlapCapsule(Vector3 point1, Vector3 point2, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction, Collider[] results, IColliderFilter colliderFilter)
		{
			int num = Physics.OverlapCapsuleNonAlloc(point1, point2, radius, results, layerMask, queryTriggerInteraction);
			if (num == 0)
			{
				return 0;
			}
			int num2 = num;
			for (int i = 0; i < num; i++)
			{
				Collider otherCollider = results[i];
				if ((colliderFilter == null || colliderFilter.Filter(otherCollider)) && i < --num2)
				{
					results[i] = results[num2];
				}
			}
			return num2;
		}

		// Token: 0x040000E0 RID: 224
		private const int kMaxHits = 8;

		// Token: 0x040000E1 RID: 225
		private static readonly RaycastHit[] HitsBuffer = new RaycastHit[8];
	}
}
