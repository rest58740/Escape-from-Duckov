using System;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000018 RID: 24
	public sealed class FollowerEntity : VersionedMonoBehaviour
	{
		// Token: 0x0600012D RID: 301 RVA: 0x00006EA1 File Offset: 0x000050A1
		public void Start()
		{
			Debug.LogError("The FollowerEntity component requires at least version 1.0 of the 'Entities' package to be installed. You can install it using the Unity package manager.");
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006EAD File Offset: 0x000050AD
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			migrations.IgnoreMigrationAttempt();
		}
	}
}
