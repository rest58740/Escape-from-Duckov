using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000011 RID: 17
	[ExecuteInEditMode]
	public class FindLodGroups : MonoBehaviour
	{
		// Token: 0x0600002A RID: 42 RVA: 0x000033BA File Offset: 0x000015BA
		private void Start()
		{
			this.FindLods();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000033C2 File Offset: 0x000015C2
		private void Update()
		{
			if (this.find)
			{
				this.find = false;
				this.FindLods();
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000033DC File Offset: 0x000015DC
		private void FindLods()
		{
			LODGroup[] componentsInChildren = base.GetComponentsInChildren<LODGroup>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Debug.Log(componentsInChildren[i].name);
			}
			Debug.Log("---------------------------------------------");
			Debug.Log("LODGroups found " + componentsInChildren.Length.ToString());
			Debug.Log("---------------------------------------------");
		}

		// Token: 0x04000032 RID: 50
		public bool find;
	}
}
