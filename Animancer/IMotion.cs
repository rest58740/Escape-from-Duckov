using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200002F RID: 47
	public interface IMotion
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000346 RID: 838
		float AverageAngularSpeed { get; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000347 RID: 839
		Vector3 AverageVelocity { get; }
	}
}
