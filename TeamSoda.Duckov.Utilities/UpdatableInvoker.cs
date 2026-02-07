using System;
using System.Collections.Generic;
using Duckov.Utilities.Updatables;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000009 RID: 9
	public class UpdatableInvoker : MonoBehaviour
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002D3F File Offset: 0x00000F3F
		public static UpdatableInvoker Instance
		{
			get
			{
				if (UpdatableInvoker.instance == null)
				{
					UpdatableInvoker.CreateInstance();
				}
				return UpdatableInvoker.instance;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002D58 File Offset: 0x00000F58
		private static void CreateInstance()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			GameObject gameObject = new GameObject("UpdateInvoker");
			UpdatableInvoker.instance = gameObject.AddComponent<UpdatableInvoker>();
			Object.DontDestroyOnLoad(gameObject);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002D7C File Offset: 0x00000F7C
		private void Awake()
		{
			if (UpdatableInvoker.instance == null)
			{
				UpdatableInvoker.instance = this;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002D91 File Offset: 0x00000F91
		private void Update()
		{
			if (UpdatableInvoker.instance != this)
			{
				return;
			}
			UpdatableInvoker.DoUpdate();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002DA8 File Offset: 0x00000FA8
		private static void DoUpdate()
		{
			if (UpdatableInvoker.incomingObjects.Count > 0)
			{
				UpdatableInvoker.ActivateIncomingObjects();
			}
			bool flag = false;
			for (int i = 0; i < UpdatableInvoker.activeObjects.Count; i++)
			{
				object obj = UpdatableInvoker.activeObjects[i];
				if (obj == null)
				{
					flag = true;
				}
				else
				{
					(obj as IUpdatable).OnUpdate();
				}
			}
			if (flag)
			{
				UpdatableInvoker.activeObjects.RemoveAll((object e) => e == null);
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002E2C File Offset: 0x0000102C
		private static void ActivateIncomingObjects()
		{
			foreach (object item in UpdatableInvoker.incomingObjects)
			{
				UpdatableInvoker.activeObjects.Add(item);
			}
			UpdatableInvoker.incomingObjects.Clear();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002E8C File Offset: 0x0000108C
		public static void Register(IUpdatable updatable)
		{
			UpdatableInvoker.incomingObjects.Add(updatable);
			if (UpdatableInvoker.Instance == null)
			{
				UpdatableInvoker.CreateInstance();
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002EAC File Offset: 0x000010AC
		public static bool Unregister(IUpdatable updatable)
		{
			bool flag = UpdatableInvoker.incomingObjects.Remove(updatable);
			bool flag2 = UpdatableInvoker.activeObjects.Remove(updatable);
			return flag || flag2;
		}

		// Token: 0x04000019 RID: 25
		private static UpdatableInvoker instance;

		// Token: 0x0400001A RID: 26
		private static List<object> incomingObjects = new List<object>();

		// Token: 0x0400001B RID: 27
		private static List<object> activeObjects = new List<object>();
	}
}
