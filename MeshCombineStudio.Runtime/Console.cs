using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MeshCombineStudio
{
	// Token: 0x0200002F RID: 47
	public class Console : MonoBehaviour
	{
		// Token: 0x060000FB RID: 251 RVA: 0x00009443 File Offset: 0x00007643
		private void Awake()
		{
			Console.instance = this;
			this.FindMeshCombiners();
			this.window = default(Rect);
			this.inputText = string.Empty;
			this.ReportStartup();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00009470 File Offset: 0x00007670
		private void ReportStartup()
		{
			Console.Log("---------------------------------", 0, null, null);
			Console.Log("*** MeshCombineStudio Console ***", 0, null, null);
			Console.Log("---------------------------------", 0, null, null);
			Console.Log("", 0, null, null);
			this.ReportMeshCombiners(false);
			Console.Log("combine automatic " + (this.combineAutomatic ? "on" : "off"), 0, null, null);
			if (this.meshCombiners != null && this.meshCombiners.Length != 0)
			{
				this.SelectMeshCombiner(this.meshCombiners[0].name);
			}
			Console.Log("", 0, null, null);
			Console.Log("Type '?' to show commands", 0, null, null);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000951D File Offset: 0x0000771D
		private void FindMeshCombiners()
		{
			this.meshCombiners = UnityEngine.Object.FindObjectsOfType<MeshCombiner>();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000952C File Offset: 0x0000772C
		private void ReportMeshCombiners(bool reportSelected = true)
		{
			for (int i = 0; i < this.meshCombiners.Length; i++)
			{
				this.ReportMeshCombiner(this.meshCombiners[i], true);
			}
			if (this.selectedMeshCombiner != null)
			{
				Console.Log("Selected MCS -> " + this.selectedMeshCombiner.name, 0, null, null);
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00009588 File Offset: 0x00007788
		private void ReportMeshCombiner(MeshCombiner meshCombiner, bool foundText = false)
		{
			Console.Log(string.Concat(new string[]
			{
				foundText ? "Found MCS -> " : "",
				meshCombiner.name,
				" (",
				meshCombiner.combined ? "*color-green#Combined" : "*color-blue#Uncombined",
				") -> Cell Size ",
				meshCombiner.cellSize.ToString(),
				meshCombiner.searchOptions.useMaxBoundsFactor ? (" | Max Bounds Factor " + meshCombiner.searchOptions.maxBoundsFactor.ToString()) : "",
				meshCombiner.searchOptions.useVertexInputLimit ? (" | Vertex Input Limit " + (meshCombiner.searchOptions.useVertexInputLimit ? meshCombiner.searchOptions.vertexInputLimit : 65534).ToString()) : ""
			}), 0, null, meshCombiner);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00009674 File Offset: 0x00007874
		public int SelectMeshCombiner(string name)
		{
			if (this.meshCombiners == null && this.meshCombiners.Length == 0)
			{
				return 0;
			}
			for (int i = 0; i < this.meshCombiners.Length; i++)
			{
				MeshCombiner meshCombiner = this.meshCombiners[i];
				if (meshCombiner.name == name)
				{
					Console.Log(string.Concat(new string[]
					{
						"Selected MCS -> ",
						meshCombiner.name,
						" (",
						meshCombiner.combined ? "*color-green#Combined" : "*color-blue#Uncombined",
						")"
					}), 0, null, meshCombiner);
					this.selectedMeshCombiner = meshCombiner;
					return 2;
				}
			}
			return 0;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00009713 File Offset: 0x00007913
		private void OnEnable()
		{
			Application.logMessageReceived += this.HandleLog;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00009726 File Offset: 0x00007926
		private void OnDisable()
		{
			Application.logMessageReceived -= this.HandleLog;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00009739 File Offset: 0x00007939
		private void OnDestroy()
		{
			Console.instance = null;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00009741 File Offset: 0x00007941
		public static void Log(string logString, int commandType = 0, GameObject go = null, MeshCombiner meshCombiner = null)
		{
			Console.instance.logs.Add(new Console.LogEntry(logString, "", LogType.Log, false, commandType, go, meshCombiner));
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00009764 File Offset: 0x00007964
		private void HandleLog(string logString, string stackTrace, LogType logType)
		{
			if (this.logActive)
			{
				this.logs.Add(new Console.LogEntry(logString, stackTrace, logType, true, 0, null, null));
				if (this.showOnError && (logType == LogType.Error || logType == LogType.Warning))
				{
					this.SetConsoleActive(true);
					this.showLast = true;
					this.showUnityLog = true;
				}
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000097B4 File Offset: 0x000079B4
		private void Update()
		{
			if (Input.GetKeyDown(this.consoleKey))
			{
				this.SetConsoleActive(!this.showConsole);
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000097D2 File Offset: 0x000079D2
		private void SetConsoleActive(bool active)
		{
			this.showConsole = active;
			if (this.showConsole)
			{
				this.setFocus = true;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000097EC File Offset: 0x000079EC
		private void ExecuteCommand(string cmd)
		{
			this.logs.Add(new Console.LogEntry(cmd, "", LogType.Log, false, 1, null, null));
			Console.LogEntry logEntry = this.logs[this.logs.Count - 1];
			if (cmd == "?")
			{
				Console.Log("'F1' to show/hide console", 0, null, null);
				Console.Log("'dir', 'dirAll', 'dirSort', 'cd', 'show', 'showAll', 'hide', 'hideAll'", 0, null, null);
				Console.Log("'components', 'lines', 'clear', 'gc collect'", 0, null, null);
				Console.Log("'select (MeshCombineStudio name)', ", 0, null, null);
				Console.Log("'report MeshCombineStudio'", 0, null, null);
				Console.Log("'combine', 'uncombine', 'combine automatic on/off'", 0, null, null);
				Console.Log("'max bounds factor (float)/off'", 0, null, null);
				Console.Log("'vertex input limit (float)/off'", 0, null, null);
				Console.Log("'vertex input limit lod (float)/off'", 0, null, null);
				Console.Log("'cell size (float)'", 0, null, null);
				logEntry.commandType = 2;
				return;
			}
			if (cmd == "gc collect")
			{
				GC.Collect();
				logEntry.commandType = 2;
				return;
			}
			if (cmd == "dir")
			{
				this.Dir();
				logEntry.commandType = 2;
				return;
			}
			if (cmd == "components")
			{
				this.Components(logEntry);
				return;
			}
			if (cmd.Contains("lines "))
			{
				int.TryParse(cmd.Replace("lines ", ""), out this.lines);
				this.lines = Mathf.Clamp(this.lines, 5, 50);
				logEntry.commandType = 2;
				return;
			}
			if (cmd == "cd..")
			{
				this.CD(logEntry, "..");
				return;
			}
			if (cmd == "cd\\")
			{
				this.CD(logEntry, "\\");
				return;
			}
			if (cmd.Contains("cd "))
			{
				this.CD(logEntry, cmd.Replace("cd ", ""));
				return;
			}
			if (cmd.Contains("show "))
			{
				Transform transform = Methods.Find<Transform>(this.selectGO, cmd.Replace("show ", ""));
				if (transform != null)
				{
					transform.gameObject.SetActive(true);
					logEntry.commandType = 2;
					return;
				}
			}
			else if (cmd == "show")
			{
				if (this.selectGO != null)
				{
					this.selectGO.SetActive(true);
					logEntry.commandType = 2;
					return;
				}
			}
			else
			{
				if (cmd.Contains("showAll "))
				{
					this.SetActiveContains(cmd.Replace("showAll ", ""), true);
					logEntry.commandType = 2;
					return;
				}
				if (cmd.Contains("hide "))
				{
					GameObject gameObject = GameObject.Find(cmd.Replace("hide ", ""));
					if (gameObject != null)
					{
						gameObject.SetActive(false);
						logEntry.commandType = 2;
						return;
					}
				}
				else
				{
					if (cmd.Contains("hideAll "))
					{
						this.SetActiveContains(cmd.Replace("hideAll ", ""), false);
						logEntry.commandType = 2;
						return;
					}
					if (cmd == "hide")
					{
						if (this.selectGO != null)
						{
							this.selectGO.SetActive(false);
							logEntry.commandType = 2;
							return;
						}
					}
					else
					{
						if (cmd.Contains("clear"))
						{
							this.Clear(logEntry, cmd.Replace("clear ", ""));
							return;
						}
						if (cmd.Contains("dir "))
						{
							this.DirContains(cmd.Replace("dir ", ""));
							logEntry.commandType = 2;
							return;
						}
						if (cmd == "dirAll")
						{
							this.DirAll();
							logEntry.commandType = 2;
							return;
						}
						if (cmd.Contains("dirSort "))
						{
							this.DirSort(cmd.Replace("dirSort ", ""));
							logEntry.commandType = 2;
							return;
						}
						if (cmd == "dirSort")
						{
							this.DirSort();
							logEntry.commandType = 2;
							return;
						}
						if (cmd.Contains("cell size "))
						{
							int num;
							int.TryParse(cmd.Replace("cell size ", ""), out num);
							if (num < 4)
							{
								Console.Log("cell size should be bigger than 4", 0, null, null);
								return;
							}
							if (this.selectedMeshCombiner != null)
							{
								this.selectedMeshCombiner.cellSize = num;
								this.selectedMeshCombiner.AddObjectsAutomatically(true);
								if (this.combineAutomatic)
								{
									this.selectedMeshCombiner.CombineAll(true);
								}
								this.ReportMeshCombiner(this.selectedMeshCombiner, false);
								logEntry.commandType = 2;
								return;
							}
						}
						else
						{
							if (cmd == "report MeshCombineStudio")
							{
								this.ReportMeshCombiners(true);
								logEntry.commandType = 2;
								return;
							}
							if (cmd == "combine")
							{
								if (this.selectedMeshCombiner != null)
								{
									this.selectedMeshCombiner.octreeContainsObjects = false;
									this.selectedMeshCombiner.CombineAll(true);
									this.ReportMeshCombiner(this.selectedMeshCombiner, false);
									logEntry.commandType = 2;
									return;
								}
							}
							else if (cmd == "uncombine")
							{
								if (this.selectedMeshCombiner != null)
								{
									this.selectedMeshCombiner.DestroyCombinedObjects();
									this.ReportMeshCombiner(this.selectedMeshCombiner, false);
									logEntry.commandType = 2;
									return;
								}
							}
							else
							{
								if (cmd == "combine automatic off")
								{
									this.combineAutomatic = false;
									logEntry.commandType = 2;
									return;
								}
								if (cmd == "combine automatic on")
								{
									this.combineAutomatic = true;
									logEntry.commandType = 2;
									return;
								}
								if (cmd.Contains("select "))
								{
									if (this.SelectMeshCombiner(cmd.Replace("select ", "")) == 2)
									{
										this.ReportMeshCombiner(this.selectedMeshCombiner, false);
										logEntry.commandType = 2;
										return;
									}
								}
								else if (cmd == "max bounds factor off")
								{
									if (this.selectedMeshCombiner != null)
									{
										this.selectedMeshCombiner.searchOptions.useMaxBoundsFactor = false;
										this.selectedMeshCombiner.AddObjectsAutomatically(true);
										if (this.combineAutomatic)
										{
											this.selectedMeshCombiner.CombineAll(true);
										}
										this.ReportMeshCombiner(this.selectedMeshCombiner, false);
										logEntry.commandType = 2;
										return;
									}
								}
								else if (cmd.Contains("max bounds factor "))
								{
									float num2;
									float.TryParse(cmd.Replace("max bounds factor ", ""), out num2);
									if (num2 < 1f)
									{
										Console.Log("max bounds factor needs to be bigger than 1", 0, null, null);
										return;
									}
									if (this.selectedMeshCombiner != null)
									{
										this.selectedMeshCombiner.searchOptions.useMaxBoundsFactor = true;
										this.selectedMeshCombiner.searchOptions.maxBoundsFactor = num2;
										this.selectedMeshCombiner.AddObjectsAutomatically(true);
										if (this.combineAutomatic)
										{
											this.selectedMeshCombiner.CombineAll(true);
										}
										this.ReportMeshCombiner(this.selectedMeshCombiner, false);
										logEntry.commandType = 2;
										return;
									}
								}
								else if (cmd == "vertex input limit off")
								{
									if (this.selectedMeshCombiner != null)
									{
										this.selectedMeshCombiner.searchOptions.useVertexInputLimit = false;
										this.selectedMeshCombiner.AddObjectsAutomatically(true);
										if (this.combineAutomatic)
										{
											this.selectedMeshCombiner.CombineAll(true);
										}
										this.ReportMeshCombiner(this.selectedMeshCombiner, false);
										logEntry.commandType = 2;
										return;
									}
								}
								else if (cmd.Contains("vertex input limit "))
								{
									int num3;
									int.TryParse(cmd.Replace("vertex input limit ", ""), out num3);
									if (num3 < 1)
									{
										Console.Log("vertex input limit needs to be bigger than 1", 0, null, null);
										return;
									}
									if (this.selectedMeshCombiner != null)
									{
										this.selectedMeshCombiner.searchOptions.useVertexInputLimit = true;
										this.selectedMeshCombiner.searchOptions.vertexInputLimit = num3;
										this.selectedMeshCombiner.AddObjectsAutomatically(true);
										if (this.combineAutomatic)
										{
											this.selectedMeshCombiner.CombineAll(true);
										}
										this.ReportMeshCombiner(this.selectedMeshCombiner, false);
										logEntry.commandType = 2;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00009F58 File Offset: 0x00008158
		private void DirSort()
		{
			GameObject[] gos = Methods.Search<GameObject>(this.selectGO);
			this.SortLog(gos, true);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00009F7C File Offset: 0x0000817C
		private void DirSort(string name)
		{
			GameObject[] array = Methods.Search<GameObject>(null);
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < array.Length; i++)
			{
				if (Methods.Contains(array[i].name, name))
				{
					list.Add(array[i]);
				}
			}
			this.SortLog(list.ToArray(), false);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00009FCC File Offset: 0x000081CC
		public void SortLog(GameObject[] gos, bool showMeshInfo = false)
		{
			List<GameObject> list = new List<GameObject>();
			List<int> list2 = new List<int>();
			int num = 0;
			int num2 = 0;
			foreach (GameObject gameObject in gos)
			{
				this.GetMeshInfo(gameObject, ref num2);
				string name = gameObject.name;
				int num3 = -1;
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].name == name)
					{
						num3 = j;
						break;
					}
				}
				if (num3 == -1)
				{
					list.Add(gameObject);
					list2.Add(1);
					num++;
				}
				else
				{
					List<int> list3 = list2;
					int index = num3;
					int num4 = list3[index];
					list3[index] = num4 + 1;
					num++;
				}
			}
			int num5 = 0;
			for (int k = 0; k < list.Count; k++)
			{
				Console.Log(string.Concat(new string[]
				{
					list[k].name,
					" -> ",
					list2[k].ToString(),
					" ",
					this.GetMeshInfo(list[k], ref num5)
				}), 0, null, null);
			}
			Console.Log(string.Concat(new string[]
			{
				"Total amount ",
				num.ToString(),
				" Total items ",
				list.Count.ToString(),
				" Total shared meshes ",
				num2.ToString()
			}), 0, null, null);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000A14C File Offset: 0x0000834C
		private string GetMeshInfo(GameObject go, ref int meshCount)
		{
			MeshFilter component = go.GetComponent<MeshFilter>();
			if (component != null)
			{
				Mesh sharedMesh = component.sharedMesh;
				if (sharedMesh != null)
				{
					meshCount++;
					return string.Concat(new string[]
					{
						"(vertices ",
						sharedMesh.vertexCount.ToString(),
						", combine ",
						Mathf.FloorToInt((float)(65000 / sharedMesh.vertexCount)).ToString(),
						")"
					});
				}
			}
			return "";
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000A1D8 File Offset: 0x000083D8
		private void TimeStep(string cmd)
		{
			float fixedDeltaTime;
			float.TryParse(cmd, out fixedDeltaTime);
			Time.fixedDeltaTime = fixedDeltaTime;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000A1F4 File Offset: 0x000083F4
		private void TimeScale(string cmd)
		{
			float timeScale;
			float.TryParse(cmd, out timeScale);
			Time.timeScale = timeScale;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000A210 File Offset: 0x00008410
		private void Clear(Console.LogEntry log, string cmd)
		{
			if (cmd == "clear")
			{
				this.logs.Clear();
				log.commandType = 2;
				return;
			}
			if (cmd == "input")
			{
				for (int i = 0; i < this.logs.Count; i++)
				{
					if (!this.logs[i].unityLog)
					{
						this.logs.RemoveAt(i--);
					}
				}
				log.commandType = 2;
				return;
			}
			if (cmd == "unity")
			{
				for (int j = 0; j < this.logs.Count; j++)
				{
					if (this.logs[j].unityLog)
					{
						this.logs.RemoveAt(j--);
					}
				}
				log.commandType = 2;
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000A2D8 File Offset: 0x000084D8
		private void DirAll()
		{
			GameObject[] array = Methods.Search<GameObject>(this.selectGO);
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				Console.Log(string.Concat(new string[]
				{
					this.GetPath(array[i]),
					"\\",
					array[i].transform.childCount.ToString(),
					" ",
					this.GetMeshInfo(array[i], ref num)
				}), 0, array[i], null);
			}
			Console.Log(array.Length.ToString() + " (meshes " + num.ToString() + ")\\..", 0, null, null);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000A384 File Offset: 0x00008584
		private void Dir()
		{
			int num = 0;
			if (this.selectGO == null)
			{
				GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
				for (int i = 0; i < rootGameObjects.Length; i++)
				{
					Console.Log(string.Concat(new string[]
					{
						rootGameObjects[i].name,
						"\\",
						rootGameObjects[i].transform.childCount.ToString(),
						" ",
						this.GetMeshInfo(rootGameObjects[i], ref num)
					}), 0, rootGameObjects[i], null);
				}
				Console.Log(rootGameObjects.Length.ToString() + " (meshes " + num.ToString() + ")\\..", 0, null, null);
				return;
			}
			this.ShowPath(true);
			Transform transform = this.selectGO.transform;
			for (int j = 0; j < transform.childCount; j++)
			{
				Transform child = transform.GetChild(j);
				Console.Log(string.Concat(new string[]
				{
					child.name,
					"\\",
					child.childCount.ToString(),
					" ",
					this.GetMeshInfo(child.gameObject, ref num)
				}), 0, child.gameObject, null);
			}
			Console.Log(transform.childCount.ToString() + " (meshes " + num.ToString() + ")\\..", 0, null, null);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000A500 File Offset: 0x00008700
		private void Components(Console.LogEntry log)
		{
			if (this.selectGO == null)
			{
				log.commandType = 1;
				return;
			}
			Component[] components = this.selectGO.GetComponents<Component>();
			this.ShowPath(true);
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i] != null)
				{
					Console.Log(components[i].GetType().Name, 0, null, null);
				}
			}
			log.commandType = 2;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000A56C File Offset: 0x0000876C
		private void ShowPath(bool showLines = true)
		{
			string path = this.GetPath(this.selectGO);
			if (path != "")
			{
				Console.Log(path, 0, null, null);
			}
			else
			{
				Console.Log("Root\\", 0, null, null);
			}
			if (showLines)
			{
				Console.Log("---------------------------------", 0, null, null);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000A5BC File Offset: 0x000087BC
		private string GetPath(GameObject go)
		{
			if (go != null)
			{
				string text = go.name;
				Transform transform = go.transform;
				while (transform.parent != null)
				{
					text = text.Insert(0, transform.parent.name + "\\");
					transform = transform.parent;
				}
				return text;
			}
			return "";
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000A61C File Offset: 0x0000881C
		private void CD(Console.LogEntry log, string name)
		{
			if (name == "..")
			{
				if (this.selectGO != null)
				{
					if (this.selectGO.transform.parent != null)
					{
						this.selectGO = this.selectGO.transform.parent.gameObject;
					}
					else
					{
						this.selectGO = null;
					}
					log.commandType = 2;
					this.ShowPath(false);
					return;
				}
			}
			else if (name == "\\")
			{
				this.selectGO = null;
				log.commandType = 2;
				return;
			}
			Transform transform = Methods.Find<Transform>(this.selectGO, name);
			if (transform != null)
			{
				this.selectGO = transform.gameObject;
				this.ShowPath(false);
				log.commandType = 2;
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000A6DC File Offset: 0x000088DC
		public void SetActiveContains(string textContains, bool active)
		{
			GameObject[] array = Methods.Search<GameObject>(this.selectGO);
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (Methods.Contains(array[i].name, textContains) && (array[i].transform.parent.name.IndexOf("GUI") == 0 || array[i].transform.parent.parent == null || array[i].transform.parent.parent.name.IndexOf("GUI") == 0))
				{
					array[i].SetActive(active);
					num++;
				}
			}
			Console.Log("Total amount set to " + active.ToString() + " : " + num.ToString(), 0, null, null);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000A7A8 File Offset: 0x000089A8
		public void DirContains(string textContains)
		{
			GameObject[] array = Methods.Search<GameObject>(this.selectGO);
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (Methods.Contains(array[i].name, textContains))
				{
					Console.Log(array[i].name, 0, array[i], null);
					num++;
				}
			}
			Console.Log("Total amount: " + num.ToString(), 0, null, null);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000A810 File Offset: 0x00008A10
		private void OnGUI()
		{
			if (!this.showConsole)
			{
				return;
			}
			this.window.x = 225f;
			this.window.y = 5f;
			this.window.yMax = (float)(this.lines * 20 + 30);
			this.window.xMax = (float)Screen.width - this.window.x;
			GUI.Box(this.window, "Console");
			this.inputRect.x = this.window.x + 5f;
			this.inputRect.y = this.window.yMax - 25f;
			this.inputRect.xMax = this.window.xMax - 10f;
			this.inputRect.yMax = this.window.yMax - 5f;
			if (this.showInputLog)
			{
				if (GUI.GetNameOfFocusedControl() == "ConsoleInput" && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
				{
					this.showLast = true;
					this.ExecuteCommand(this.inputText);
					this.inputText = string.Empty;
				}
				GUI.SetNextControlName("ConsoleInput");
				GUI.changed = false;
				this.inputText = GUI.TextField(this.inputRect, this.inputText);
				if (GUI.changed && this.inputText.Contains("`"))
				{
					this.inputText = this.inputText.Replace("`", "");
					this.SetConsoleActive(!this.showConsole);
				}
				if (this.setFocus)
				{
					this.setFocus = false;
					GUI.FocusControl("ConsoleInput");
				}
			}
			if (this.showInputLog)
			{
				GUI.color = Color.green;
			}
			else
			{
				GUI.color = Color.grey;
			}
			if (GUI.Button(new Rect(this.window.xMin + 5f, this.window.yMin + 5f, 75f, 20f), "Input Log"))
			{
				this.showInputLog = !this.showInputLog;
			}
			if (this.showUnityLog)
			{
				GUI.color = Color.green;
			}
			else
			{
				GUI.color = Color.grey;
			}
			if (GUI.Button(new Rect(this.window.xMin + 85f, this.window.yMin + 5f, 75f, 20f), "Unity Log"))
			{
				this.showUnityLog = !this.showUnityLog;
			}
			GUI.color = Color.white;
			if (!this.showInputLog && !this.showUnityLog)
			{
				this.showInputLog = true;
			}
			this.logRect.x = this.window.x + 5f;
			this.logRect.y = this.window.y + 25f;
			this.logRect.xMax = this.window.xMax - 20f;
			this.logRect.yMax = this.logRect.y + 20f;
			this.vScrollRect.x = this.window.xMax - 15f;
			this.vScrollRect.y = this.logRect.y;
			this.vScrollRect.xMax = this.window.xMax - 5f;
			this.vScrollRect.yMax = this.window.yMax - 45f;
			float num = Mathf.Ceil(this.vScrollRect.height / 20f);
			if (this.showLast && Event.current.type != EventType.Repaint)
			{
				this.scrollPos = (float)this.logs.Count;
			}
			GUI.changed = false;
			this.scrollPos = GUI.VerticalScrollbar(this.vScrollRect, this.scrollPos, (num > (float)(this.logs.Count - 1)) ? ((float)(this.logs.Count - 1)) : (num - 1f), 0f, (float)(this.logs.Count - 1));
			if (GUI.changed)
			{
				this.showLast = false;
			}
			int num2 = (int)this.scrollPos;
			if (num2 < 0)
			{
				num2 = 0;
			}
			int num3 = num2 + (int)num;
			if (num3 > this.logs.Count)
			{
				num3 = this.logs.Count;
			}
			int num4 = num3 - num2;
			int num5 = num2;
			int num6 = 0;
			while (num6 != num4 && num5 < this.logs.Count)
			{
				Console.LogEntry logEntry = this.logs[num5];
				if ((logEntry.unityLog && this.showUnityLog) || (!logEntry.unityLog && this.showInputLog))
				{
					if (logEntry.logType == LogType.Warning)
					{
						this.AnimateColor(Color.yellow, logEntry, 0.75f);
					}
					else if (logEntry.logType == LogType.Error)
					{
						this.AnimateColor(Color.red, logEntry, 0.75f);
					}
					else if (logEntry.logType == LogType.Exception)
					{
						this.AnimateColor(Color.magenta, logEntry, 0.75f);
					}
					else if (logEntry.unityLog)
					{
						this.AnimateColor(Color.white, logEntry, 0.75f);
					}
					else if (logEntry.commandType == 1)
					{
						GUI.color = new Color(0f, 0.5f, 0f);
					}
					else if (logEntry.commandType == 2)
					{
						GUI.color = Color.green;
					}
					else if (logEntry.go != null)
					{
						GUI.color = (logEntry.go.activeSelf ? Color.white : (Color.white * 0.7f));
					}
					string text = logEntry.logString;
					if (text.Contains("*color-"))
					{
						if (text.Contains("*color-green#"))
						{
							text = text.Replace("*color-green#", "");
							GUI.color = Color.green;
						}
						else if (text.Contains("*color-blue#"))
						{
							text = text.Replace("*color-blue#", "");
							GUI.color = Color.blue;
						}
					}
					GUI.Label(this.logRect, num5.ToString() + ") ");
					this.logRect.xMin = this.logRect.xMin + 55f;
					GUI.Label(this.logRect, text + ((logEntry.stackTrace != "") ? (" (" + logEntry.stackTrace + ")") : ""));
					this.logRect.xMin = this.logRect.xMin - 55f;
					GUI.color = Color.white;
					this.logRect.y = this.logRect.y + 20f;
					num6++;
				}
				num5++;
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000AEEA File Offset: 0x000090EA
		private void AnimateColor(Color col, Console.LogEntry log, float multi)
		{
			GUI.color = Color.Lerp(col * multi, col, Mathf.Abs(Mathf.Sin(Time.time)));
		}

		// Token: 0x0400011C RID: 284
		public static Console instance;

		// Token: 0x0400011D RID: 285
		public KeyCode consoleKey = KeyCode.F1;

		// Token: 0x0400011E RID: 286
		public bool logActive = true;

		// Token: 0x0400011F RID: 287
		public bool showConsole;

		// Token: 0x04000120 RID: 288
		public bool showOnError;

		// Token: 0x04000121 RID: 289
		public bool combineAutomatic = true;

		// Token: 0x04000122 RID: 290
		private bool showLast = true;

		// Token: 0x04000123 RID: 291
		private bool setFocus;

		// Token: 0x04000124 RID: 292
		private GameObject selectGO;

		// Token: 0x04000125 RID: 293
		public List<Console.LogEntry> logs = new List<Console.LogEntry>();

		// Token: 0x04000126 RID: 294
		private Rect window;

		// Token: 0x04000127 RID: 295
		private Rect inputRect;

		// Token: 0x04000128 RID: 296
		private Rect logRect;

		// Token: 0x04000129 RID: 297
		private Rect vScrollRect;

		// Token: 0x0400012A RID: 298
		private string inputText;

		// Token: 0x0400012B RID: 299
		private float scrollPos;

		// Token: 0x0400012C RID: 300
		private int lines = 20;

		// Token: 0x0400012D RID: 301
		private bool showUnityLog = true;

		// Token: 0x0400012E RID: 302
		private bool showInputLog = true;

		// Token: 0x0400012F RID: 303
		private MeshCombiner[] meshCombiners;

		// Token: 0x04000130 RID: 304
		private MeshCombiner selectedMeshCombiner;

		// Token: 0x0200006E RID: 110
		public class LogEntry
		{
			// Token: 0x060001E7 RID: 487 RVA: 0x00011702 File Offset: 0x0000F902
			public LogEntry(string logString, string stackTrace, LogType logType, bool unityLog = false, int commandType = 0, GameObject go = null, MeshCombiner meshCombiner = null)
			{
				this.logString = logString;
				this.stackTrace = stackTrace;
				this.logType = logType;
				this.unityLog = unityLog;
				this.commandType = commandType;
				this.go = go;
				this.meshCombiner = meshCombiner;
			}

			// Token: 0x040002AC RID: 684
			public string logString;

			// Token: 0x040002AD RID: 685
			public string stackTrace;

			// Token: 0x040002AE RID: 686
			public LogType logType;

			// Token: 0x040002AF RID: 687
			public int commandType;

			// Token: 0x040002B0 RID: 688
			public bool unityLog;

			// Token: 0x040002B1 RID: 689
			public float tStamp;

			// Token: 0x040002B2 RID: 690
			public GameObject go;

			// Token: 0x040002B3 RID: 691
			public MeshCombiner meshCombiner;
		}
	}
}
