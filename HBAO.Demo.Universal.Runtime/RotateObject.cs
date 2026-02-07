using System;
using UnityEngine;

namespace HorizonBasedAmbientOcclusion.Universal
{
	// Token: 0x02000004 RID: 4
	public class RotateObject : MonoBehaviour
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000021C0 File Offset: 0x000003C0
		private void Start()
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021C2 File Offset: 0x000003C2
		private void Update()
		{
			base.transform.Rotate(Vector3.up * Time.deltaTime * 15f, Space.World);
		}
	}
}
