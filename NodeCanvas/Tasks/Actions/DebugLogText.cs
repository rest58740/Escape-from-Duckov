using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000CA RID: 202
	[Name("Debug Log", 0)]
	[Category("✫ Utility")]
	[Description("Display a UI label on the agent's position if seconds to run is not 0 and also logs the message, which can also be mapped to any variable.")]
	public class DebugLogText : ActionTask<Transform>
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000374 RID: 884 RVA: 0x0000DAAC File Offset: 0x0000BCAC
		protected override string info
		{
			get
			{
				return "Log " + this.log.ToString() + ((this.secondsToRun > 0f) ? (" for " + this.secondsToRun.ToString() + " sec.") : "");
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000DAFC File Offset: 0x0000BCFC
		protected override void OnExecute()
		{
			if (this.verboseMode == DebugLogText.VerboseMode.LogAndDisplayLabel || this.verboseMode == DebugLogText.VerboseMode.LogOnly)
			{
				string.Format("(<b>{0}</b>) {1}", base.agent.gameObject.name, this.log.value);
				DebugLogText.LogMode logMode = this.logMode;
				DebugLogText.LogMode logMode2 = this.logMode;
				DebugLogText.LogMode logMode3 = this.logMode;
			}
			if ((this.verboseMode == DebugLogText.VerboseMode.LogAndDisplayLabel || this.verboseMode == DebugLogText.VerboseMode.DisplayLabelOnly) && this.secondsToRun > 0f)
			{
				MonoManager.current.onGUI += new Action(this.OnGUI);
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000DB8D File Offset: 0x0000BD8D
		protected override void OnStop()
		{
			if ((this.verboseMode == DebugLogText.VerboseMode.LogAndDisplayLabel || this.verboseMode == DebugLogText.VerboseMode.DisplayLabelOnly) && this.secondsToRun > 0f)
			{
				MonoManager.current.onGUI -= new Action(this.OnGUI);
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000DBC3 File Offset: 0x0000BDC3
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= this.secondsToRun)
			{
				base.EndAction(this.finishStatus == CompactStatus.Success);
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000DBE8 File Offset: 0x0000BDE8
		private void OnGUI()
		{
			if (Camera.main == null)
			{
				return;
			}
			Vector3 vector = Camera.main.WorldToScreenPoint(base.agent.position + new Vector3(0f, this.labelYOffset, 0f));
			Vector2 vector2 = GUI.skin.label.CalcSize(new GUIContent(this.log.value));
			Rect position = new Rect(vector.x - vector2.x / 2f, (float)Screen.height - vector.y, vector2.x + 10f, vector2.y);
			GUI.color = Color.white.WithAlpha(0.5f);
			GUI.DrawTexture(position, Texture2D.whiteTexture);
			GUI.color = new Color(0.2f, 0.2f, 0.2f);
			position.x += 4f;
			GUI.Label(position, this.log.value);
			GUI.color = Color.white;
		}

		// Token: 0x0400025B RID: 603
		[RequiredField]
		public BBParameter<string> log = "Hello World";

		// Token: 0x0400025C RID: 604
		public float labelYOffset;

		// Token: 0x0400025D RID: 605
		public float secondsToRun = 1f;

		// Token: 0x0400025E RID: 606
		public DebugLogText.VerboseMode verboseMode;

		// Token: 0x0400025F RID: 607
		public DebugLogText.LogMode logMode;

		// Token: 0x04000260 RID: 608
		public CompactStatus finishStatus = CompactStatus.Success;

		// Token: 0x02000145 RID: 325
		public enum LogMode
		{
			// Token: 0x040003AA RID: 938
			Log,
			// Token: 0x040003AB RID: 939
			Warning,
			// Token: 0x040003AC RID: 940
			Error
		}

		// Token: 0x02000146 RID: 326
		public enum VerboseMode
		{
			// Token: 0x040003AE RID: 942
			LogAndDisplayLabel,
			// Token: 0x040003AF RID: 943
			LogOnly,
			// Token: 0x040003B0 RID: 944
			DisplayLabelOnly
		}
	}
}
