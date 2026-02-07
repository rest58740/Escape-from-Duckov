using System;
using UnityEngine;

namespace ParadoxNotion.Services
{
	// Token: 0x02000083 RID: 131
	public class MonoManager : MonoBehaviour
	{
		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06000569 RID: 1385 RVA: 0x0000FB3C File Offset: 0x0000DD3C
		// (remove) Token: 0x0600056A RID: 1386 RVA: 0x0000FB74 File Offset: 0x0000DD74
		public event Action onUpdate;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x0600056B RID: 1387 RVA: 0x0000FBAC File Offset: 0x0000DDAC
		// (remove) Token: 0x0600056C RID: 1388 RVA: 0x0000FBE4 File Offset: 0x0000DDE4
		public event Action onLateUpdate;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x0600056D RID: 1389 RVA: 0x0000FC1C File Offset: 0x0000DE1C
		// (remove) Token: 0x0600056E RID: 1390 RVA: 0x0000FC54 File Offset: 0x0000DE54
		public event Action onFixedUpdate;

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x0600056F RID: 1391 RVA: 0x0000FC8C File Offset: 0x0000DE8C
		// (remove) Token: 0x06000570 RID: 1392 RVA: 0x0000FCC4 File Offset: 0x0000DEC4
		public event Action onApplicationQuit;

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06000571 RID: 1393 RVA: 0x0000FCFC File Offset: 0x0000DEFC
		// (remove) Token: 0x06000572 RID: 1394 RVA: 0x0000FD34 File Offset: 0x0000DF34
		public event Action<bool> onApplicationPause;

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06000573 RID: 1395 RVA: 0x0000FD6C File Offset: 0x0000DF6C
		// (remove) Token: 0x06000574 RID: 1396 RVA: 0x0000FDA4 File Offset: 0x0000DFA4
		public event Action onGUI;

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0000FDDC File Offset: 0x0000DFDC
		public static MonoManager current
		{
			get
			{
				if (MonoManager._current == null && Threader.applicationIsPlaying && !MonoManager.isQuiting)
				{
					MonoManager._current = Object.FindAnyObjectByType<MonoManager>();
					if (MonoManager._current == null)
					{
						MonoManager._current = new GameObject("_MonoManager").AddComponent<MonoManager>();
					}
				}
				return MonoManager._current;
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0000FE34 File Offset: 0x0000E034
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Purge()
		{
			MonoManager.isQuiting = false;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0000FE3C File Offset: 0x0000E03C
		public static void Create()
		{
			MonoManager._current = MonoManager.current;
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0000FE48 File Offset: 0x0000E048
		public void AddUpdateCall(MonoManager.UpdateMode mode, Action call)
		{
			switch (mode)
			{
			case MonoManager.UpdateMode.NormalUpdate:
				this.onUpdate += call;
				return;
			case MonoManager.UpdateMode.LateUpdate:
				this.onLateUpdate += call;
				return;
			case MonoManager.UpdateMode.FixedUpdate:
				this.onFixedUpdate += call;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0000FE74 File Offset: 0x0000E074
		public void RemoveUpdateCall(MonoManager.UpdateMode mode, Action call)
		{
			switch (mode)
			{
			case MonoManager.UpdateMode.NormalUpdate:
				this.onUpdate -= call;
				return;
			case MonoManager.UpdateMode.LateUpdate:
				this.onLateUpdate -= call;
				return;
			case MonoManager.UpdateMode.FixedUpdate:
				this.onFixedUpdate -= call;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0000FEA0 File Offset: 0x0000E0A0
		protected void Awake()
		{
			if (MonoManager._current != null && MonoManager._current != this)
			{
				Object.DestroyImmediate(base.gameObject);
				return;
			}
			Object.DontDestroyOnLoad(base.gameObject);
			MonoManager._current = this;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0000FED9 File Offset: 0x0000E0D9
		protected void OnApplicationQuit()
		{
			MonoManager.isQuiting = true;
			if (this.onApplicationQuit != null)
			{
				this.onApplicationQuit.Invoke();
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0000FEF4 File Offset: 0x0000E0F4
		protected void OnApplicationPause(bool isPause)
		{
			if (this.onApplicationPause != null)
			{
				this.onApplicationPause.Invoke(isPause);
			}
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0000FF0A File Offset: 0x0000E10A
		protected void Update()
		{
			if (this.onUpdate != null)
			{
				this.onUpdate.Invoke();
			}
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0000FF1F File Offset: 0x0000E11F
		protected void LateUpdate()
		{
			if (this.onLateUpdate != null)
			{
				this.onLateUpdate.Invoke();
			}
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0000FF34 File Offset: 0x0000E134
		protected void FixedUpdate()
		{
			if (this.onFixedUpdate != null)
			{
				this.onFixedUpdate.Invoke();
			}
		}

		// Token: 0x040001B7 RID: 439
		private static bool isQuiting;

		// Token: 0x040001B8 RID: 440
		private static MonoManager _current;

		// Token: 0x02000126 RID: 294
		public enum UpdateMode
		{
			// Token: 0x040002F3 RID: 755
			NormalUpdate,
			// Token: 0x040002F4 RID: 756
			LateUpdate,
			// Token: 0x040002F5 RID: 757
			FixedUpdate
		}
	}
}
