using System;
using UnityEngine;

namespace HorizonBasedAmbientOcclusion
{
	// Token: 0x02000004 RID: 4
	public class RotateObject : MonoBehaviour
	{
		// Token: 0x06000007 RID: 7 RVA: 0x0000212C File Offset: 0x0000032C
		private void Start()
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000212E File Offset: 0x0000032E
		private void Update()
		{
			base.transform.Rotate(Vector3.up * Time.deltaTime * 15f, Space.World);
		}
	}
}
