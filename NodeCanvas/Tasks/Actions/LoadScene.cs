using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine.SceneManagement;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000063 RID: 99
	[Category("Application")]
	public class LoadScene : ActionTask
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00008957 File Offset: 0x00006B57
		protected override string info
		{
			get
			{
				return string.Format("Load Scene {0}", this.sceneName);
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00008969 File Offset: 0x00006B69
		protected override void OnExecute()
		{
			SceneManager.LoadScene(this.sceneName.value, this.mode.value);
			base.EndAction();
		}

		// Token: 0x04000134 RID: 308
		[RequiredField]
		public BBParameter<string> sceneName;

		// Token: 0x04000135 RID: 309
		public BBParameter<LoadSceneMode> mode;
	}
}
