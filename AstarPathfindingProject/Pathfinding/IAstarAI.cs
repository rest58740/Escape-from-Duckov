using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000019 RID: 25
	public interface IAstarAI
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000130 RID: 304
		// (set) Token: 0x06000131 RID: 305
		float radius { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000132 RID: 306
		// (set) Token: 0x06000133 RID: 307
		float height { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000134 RID: 308
		Vector3 position { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000135 RID: 309
		// (set) Token: 0x06000136 RID: 310
		Quaternion rotation { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000137 RID: 311
		// (set) Token: 0x06000138 RID: 312
		float maxSpeed { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000139 RID: 313
		Vector3 velocity { get; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600013A RID: 314
		Vector3 desiredVelocity { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600013B RID: 315
		// (set) Token: 0x0600013C RID: 316
		Vector3 desiredVelocityWithoutLocalAvoidance { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600013D RID: 317
		float remainingDistance { get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600013E RID: 318
		bool reachedDestination { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600013F RID: 319
		bool reachedEndOfPath { get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000140 RID: 320
		Vector3 endOfPath { get; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000141 RID: 321
		// (set) Token: 0x06000142 RID: 322
		Vector3 destination { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000143 RID: 323
		// (set) Token: 0x06000144 RID: 324
		bool canSearch { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000145 RID: 325
		// (set) Token: 0x06000146 RID: 326
		bool canMove { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000147 RID: 327
		bool hasPath { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000148 RID: 328
		bool pathPending { get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000149 RID: 329
		// (set) Token: 0x0600014A RID: 330
		bool updatePosition { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600014B RID: 331
		// (set) Token: 0x0600014C RID: 332
		bool updateRotation { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600014D RID: 333
		// (set) Token: 0x0600014E RID: 334
		bool isStopped { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600014F RID: 335
		Vector3 steeringTarget { get; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000150 RID: 336
		// (set) Token: 0x06000151 RID: 337
		Action onSearchPath { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000152 RID: 338
		NativeMovementPlane movementPlane { get; }

		// Token: 0x06000153 RID: 339
		void GetRemainingPath(List<Vector3> buffer, out bool stale);

		// Token: 0x06000154 RID: 340
		void GetRemainingPath(List<Vector3> buffer, List<PathPartWithLinkInfo> partsBuffer, out bool stale);

		// Token: 0x06000155 RID: 341
		void SearchPath();

		// Token: 0x06000156 RID: 342
		void SetPath(Path path, bool updateDestinationFromPath = true);

		// Token: 0x06000157 RID: 343
		void Teleport(Vector3 newPosition, bool clearPath = true);

		// Token: 0x06000158 RID: 344
		void Move(Vector3 deltaPosition);

		// Token: 0x06000159 RID: 345
		void MovementUpdate(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation);

		// Token: 0x0600015A RID: 346
		void FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation);
	}
}
