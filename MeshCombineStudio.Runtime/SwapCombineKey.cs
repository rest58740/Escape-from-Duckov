using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200001F RID: 31
	public class SwapCombineKey : MonoBehaviour
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00007EA4 File Offset: 0x000060A4
		private void Awake()
		{
			SwapCombineKey.instance = this;
			this.meshCombiner = base.GetComponent<MeshCombiner>();
			this.meshCombinerList.Add(this.meshCombiner);
			QualitySettings.vSyncCount = 0;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00007ECF File Offset: 0x000060CF
		private void OnDestroy()
		{
			SwapCombineKey.instance = null;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00007ED8 File Offset: 0x000060D8
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				for (int i = 0; i < this.meshCombinerList.Count; i++)
				{
					if (this.meshCombinerList[i].meshCombineJobs.Count > 0)
					{
						return;
					}
				}
				for (int j = 0; j < this.meshCombinerList.Count; j++)
				{
					this.meshCombinerList[j].SwapCombine();
				}
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00007F48 File Offset: 0x00006148
		private void OnGUI()
		{
			if (this.textStyle == null)
			{
				this.textStyle = new GUIStyle("label");
				this.textStyle.fontStyle = FontStyle.Bold;
				this.textStyle.fontSize = 16;
			}
			this.textStyle.normal.textColor = ((this.meshCombiner.combinedActive && this.meshCombiner.combined) ? Color.green : Color.red);
			int num = 0;
			GUI.Box(new Rect(5f, 30f, 310f, (float)(40 + this.meshCombinerList.Count * 22)), GUIContent.none);
			for (int i = 0; i < this.meshCombinerList.Count; i++)
			{
				MeshCombiner meshCombiner = this.meshCombinerList[i];
				if (meshCombiner.meshCombineJobs.Count > num)
				{
					num = meshCombiner.meshCombineJobs.Count;
				}
				if (meshCombiner.combinedActive && meshCombiner.combined)
				{
					GUI.Label(new Rect(10f, (float)(30 + i * 22), 300f, 30f), meshCombiner.gameObject.name + " is Enabled.", this.textStyle);
				}
				else
				{
					GUI.Label(new Rect(10f, (float)(30 + i * 22), 300f, 30f), meshCombiner.gameObject.name + " is Disabled.", this.textStyle);
				}
			}
			if (num > 0)
			{
				GUI.Label(new Rect(10f, (float)(45 + this.meshCombinerList.Count * 22), 250f, 30f), "Combining => Jobs Left " + num.ToString(), this.textStyle);
				return;
			}
			GUI.Label(new Rect(10f, (float)(45 + this.meshCombinerList.Count * 22), 200f, 30f), "Toggle with 'Tab' key.", this.textStyle);
		}

		// Token: 0x04000105 RID: 261
		public static SwapCombineKey instance;

		// Token: 0x04000106 RID: 262
		public List<MeshCombiner> meshCombinerList = new List<MeshCombiner>();

		// Token: 0x04000107 RID: 263
		private MeshCombiner meshCombiner;

		// Token: 0x04000108 RID: 264
		private GUIStyle textStyle;
	}
}
