using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200023B RID: 571
	public class CursorManager : MonoBehaviour
	{
		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x00046514 File Offset: 0x00044714
		// (set) Token: 0x060011FB RID: 4603 RVA: 0x0004651B File Offset: 0x0004471B
		public static CursorManager Instance { get; private set; }

		// Token: 0x060011FC RID: 4604 RVA: 0x00046523 File Offset: 0x00044723
		public static void Register(ICursorDataProvider dataProvider)
		{
			CursorManager.cursorDataStack.Add(dataProvider);
			CursorManager.ApplyStackData();
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00046535 File Offset: 0x00044735
		public static bool Unregister(ICursorDataProvider dataProvider)
		{
			if (CursorManager.cursorDataStack.Count < 1)
			{
				return false;
			}
			if (!CursorManager.cursorDataStack.Contains(dataProvider))
			{
				return false;
			}
			bool result = CursorManager.cursorDataStack.Remove(dataProvider);
			CursorManager.ApplyStackData();
			return result;
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00046568 File Offset: 0x00044768
		private static void ApplyStackData()
		{
			if (CursorManager.Instance == null)
			{
				return;
			}
			if (CursorManager.cursorDataStack.Count <= 0)
			{
				CursorManager.Instance.MSetDefaultCursor();
				return;
			}
			ICursorDataProvider cursorDataProvider = CursorManager.cursorDataStack[CursorManager.cursorDataStack.Count - 1];
			if (cursorDataProvider == null)
			{
				CursorManager.Instance.MSetDefaultCursor();
			}
			CursorManager.Instance.MSetCursor(cursorDataProvider.GetCursorData());
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x000465CF File Offset: 0x000447CF
		private void Awake()
		{
			CursorManager.Instance = this;
			this.MSetCursor(this.defaultCursor);
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x000465E4 File Offset: 0x000447E4
		private void Update()
		{
			if (this.currentCursor == null)
			{
				return;
			}
			if (this.currentCursor.textures.Length < 2)
			{
				return;
			}
			this.fpsBuffer += Time.unscaledDeltaTime * this.currentCursor.fps;
			if (this.fpsBuffer > 1f)
			{
				this.fpsBuffer = 0f;
				this.frame++;
				this.RefreshCursor();
			}
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x00046655 File Offset: 0x00044855
		private void RefreshCursor()
		{
			if (this.currentCursor == null)
			{
				return;
			}
			this.currentCursor.Apply(this.frame);
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x00046671 File Offset: 0x00044871
		public void MSetDefaultCursor()
		{
			this.MSetCursor(this.defaultCursor);
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0004667F File Offset: 0x0004487F
		public void MSetCursor(CursorData data)
		{
			this.currentCursor = data;
			this.frame = 12;
			this.RefreshCursor();
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x00046698 File Offset: 0x00044898
		private void OnDestroy()
		{
			Cursor.SetCursor(null, default(Vector2), 0);
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x000466B5 File Offset: 0x000448B5
		internal static void NotifyRefresh()
		{
			CursorManager.ApplyStackData();
		}

		// Token: 0x04000DFC RID: 3580
		[SerializeField]
		private CursorData defaultCursor;

		// Token: 0x04000DFD RID: 3581
		public CursorData currentCursor;

		// Token: 0x04000DFE RID: 3582
		private static List<ICursorDataProvider> cursorDataStack = new List<ICursorDataProvider>();

		// Token: 0x04000DFF RID: 3583
		private int frame;

		// Token: 0x04000E00 RID: 3584
		private float fpsBuffer;
	}
}
