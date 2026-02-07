using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000042 RID: 66
	public class SwitchMeshCombineManager : MonoBehaviour
	{
		// Token: 0x06000178 RID: 376 RVA: 0x0000D8D4 File Offset: 0x0000BAD4
		private void Start()
		{
			this.t = base.transform;
			this.meshCombiners = base.GetComponentsInChildren<MeshCombiner>(true);
			this.meshCombiners[0].InitMeshCombineJobManager();
			for (int i = 0; i < this.meshCombiners.Length; i++)
			{
				this.meshCombiners[i].CombineAll(true);
			}
			this.gos = new GameObject[this.t.childCount];
			for (int j = 0; j < this.t.childCount; j++)
			{
				this.gos[j] = this.t.GetChild(j).gameObject;
			}
			this.SetGosActive(false);
			this.meshCombiners[0].SwapCombine();
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000D984 File Offset: 0x0000BB84
		private void SetGosActive(bool active)
		{
			for (int i = 0; i < this.gos.Length; i++)
			{
				this.gos[i].SetActive(active);
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000D9B4 File Offset: 0x0000BBB4
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				if (this.meshCombiners[0].combinedActive)
				{
					this.meshCombiners[0].SwapCombine();
				}
				this.SetGosActive(false);
				this.selectIndex = 1;
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				if (!this.meshCombiners[0].combinedActive)
				{
					this.meshCombiners[0].SwapCombine();
				}
				this.SetGosActive(false);
				this.gos[0].SetActive(true);
				this.selectIndex = 2;
				return;
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				if (!this.meshCombiners[0].combinedActive)
				{
					this.meshCombiners[0].SwapCombine();
				}
				this.SetGosActive(false);
				this.gos[1].SetActive(true);
				this.selectIndex = 3;
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000DA78 File Offset: 0x0000BC78
		private void OnGUI()
		{
			if (this.style1 == null)
			{
				this.style1 = new GUIStyle(GUI.skin.label);
				this.style1.fontStyle = FontStyle.Bold;
				this.style2 = new GUIStyle(GUI.skin.label);
				this.style2.fontSize = 14;
				this.style2.fontStyle = FontStyle.Bold;
			}
			GUILayout.BeginArea(new Rect(10f, 10f, 500f, 500f));
			GUILayout.BeginVertical("Box", Array.Empty<GUILayoutOption>());
			GUILayout.Label("Select with Keyboard keys 1,2 and 3.", this.style1, Array.Empty<GUILayoutOption>());
			GUILayout.Space(15f);
			if (this.selectIndex == 1)
			{
				GUI.color = Color.green;
			}
			else
			{
				GUI.color = Color.red;
			}
			GUILayout.Label("1. No Combining", this.style2, Array.Empty<GUILayoutOption>());
			if (this.selectIndex == 2)
			{
				GUI.color = Color.green;
			}
			else
			{
				GUI.color = Color.red;
			}
			GUILayout.Label("2. Normal Combining", this.style2, Array.Empty<GUILayoutOption>());
			if (this.selectIndex == 3)
			{
				GUI.color = Color.green;
			}
			else
			{
				GUI.color = Color.red;
			}
			GUILayout.Label("3. Separate Shadow Combining without backfaces", this.style2, Array.Empty<GUILayoutOption>());
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		// Token: 0x040001AB RID: 427
		private MeshCombiner[] meshCombiners;

		// Token: 0x040001AC RID: 428
		private GameObject[] gos;

		// Token: 0x040001AD RID: 429
		private Transform t;

		// Token: 0x040001AE RID: 430
		private GUIStyle style1;

		// Token: 0x040001AF RID: 431
		private GUIStyle style2;

		// Token: 0x040001B0 RID: 432
		private int selectIndex = 1;
	}
}
