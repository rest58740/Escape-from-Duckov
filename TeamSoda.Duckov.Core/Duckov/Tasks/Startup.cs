using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Saves;
using UnityEngine;

namespace Duckov.Tasks
{
	// Token: 0x02000388 RID: 904
	public class Startup : MonoBehaviour
	{
		// Token: 0x06001F73 RID: 8051 RVA: 0x0006F4DA File Offset: 0x0006D6DA
		private void Awake()
		{
			this.MoveOldSaves();
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x0006F4E4 File Offset: 0x0006D6E4
		private void MoveOldSaves()
		{
			string fullPathToSavesFolder = SavesSystem.GetFullPathToSavesFolder();
			if (!Directory.Exists(fullPathToSavesFolder))
			{
				Directory.CreateDirectory(fullPathToSavesFolder);
			}
			for (int i = 1; i <= 3; i++)
			{
				string saveFileName = SavesSystem.GetSaveFileName(i);
				string text = Path.Combine(Application.persistentDataPath, saveFileName);
				string text2 = Path.Combine(fullPathToSavesFolder, saveFileName);
				if (File.Exists(text) && !File.Exists(text2))
				{
					Debug.Log("Transporting:\n" + text + "\n->\n" + text2);
					SavesSystem.UpgradeSaveFileAssemblyInfo(text);
					File.Move(text, text2);
				}
			}
			string path = "Options.ES3";
			string text3 = Path.Combine(Application.persistentDataPath, path);
			string text4 = Path.Combine(fullPathToSavesFolder, path);
			if (File.Exists(text3) && !File.Exists(text4))
			{
				Debug.Log("Transporting:\n" + text3 + "\n->\n" + text4);
				SavesSystem.UpgradeSaveFileAssemblyInfo(text3);
				File.Move(text3, text4);
			}
			string globalSaveDataFileName = SavesSystem.GlobalSaveDataFileName;
			string text5 = Path.Combine(Application.persistentDataPath, globalSaveDataFileName);
			string text6 = Path.Combine(fullPathToSavesFolder, globalSaveDataFileName);
			if (!File.Exists(text5))
			{
				text5 = Path.Combine(Application.persistentDataPath, "Global.csv");
			}
			if (File.Exists(text5) && !File.Exists(text6))
			{
				Debug.Log("Transporting:\n" + text5 + "\n->\n" + text6);
				SavesSystem.UpgradeSaveFileAssemblyInfo(text5);
				File.Move(text5, text6);
			}
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x0006F630 File Offset: 0x0006D830
		private void Start()
		{
			this.StartupFlow().Forget();
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x0006F640 File Offset: 0x0006D840
		private UniTask StartupFlow()
		{
			Startup.<StartupFlow>d__5 <StartupFlow>d__;
			<StartupFlow>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<StartupFlow>d__.<>4__this = this;
			<StartupFlow>d__.<>1__state = -1;
			<StartupFlow>d__.<>t__builder.Start<Startup.<StartupFlow>d__5>(ref <StartupFlow>d__);
			return <StartupFlow>d__.<>t__builder.Task;
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0006F684 File Offset: 0x0006D884
		private bool EvaluateWaitList()
		{
			foreach (MonoBehaviour monoBehaviour in this.waitForTasks)
			{
				if (!(monoBehaviour == null))
				{
					ITaskBehaviour taskBehaviour = monoBehaviour as ITaskBehaviour;
					if (taskBehaviour != null && !taskBehaviour.IsComplete())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0400157A RID: 5498
		public List<MonoBehaviour> beforeSequence = new List<MonoBehaviour>();

		// Token: 0x0400157B RID: 5499
		public List<MonoBehaviour> waitForTasks = new List<MonoBehaviour>();
	}
}
