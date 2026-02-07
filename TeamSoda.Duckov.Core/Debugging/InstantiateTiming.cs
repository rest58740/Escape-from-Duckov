using System;
using UnityEngine;

namespace Debugging
{
	// Token: 0x02000229 RID: 553
	public class InstantiateTiming : MonoBehaviour
	{
		// Token: 0x060010D4 RID: 4308 RVA: 0x00041D97 File Offset: 0x0003FF97
		public void InstantiatePrefab()
		{
			Debug.Log("Start Instantiate");
			Object.Instantiate<GameObject>(this.prefab);
			Debug.Log("Instantiated");
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00041DB9 File Offset: 0x0003FFB9
		private void Awake()
		{
			Debug.Log("Awake");
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00041DC5 File Offset: 0x0003FFC5
		private void Start()
		{
			Debug.Log("Start");
		}

		// Token: 0x04000D7D RID: 3453
		public GameObject prefab;
	}
}
