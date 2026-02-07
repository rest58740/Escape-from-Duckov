using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200003C RID: 60
	public class MCS_FPSCounter : MonoBehaviour
	{
		// Token: 0x06000146 RID: 326 RVA: 0x0000C130 File Offset: 0x0000A330
		private void Awake()
		{
			MCS_FPSCounter.instance = this;
			this.gradient.colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(new Color(1f, 0f, 0f, 1f), 0f),
				new GradientColorKey(new Color(1f, 1f, 0f, 1f), 0.5f),
				new GradientColorKey(new Color(0f, 1f, 0f, 1f), 1f)
			};
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000C1D2 File Offset: 0x0000A3D2
		private void OnDestroy()
		{
			if (MCS_FPSCounter.instance == this)
			{
				MCS_FPSCounter.instance = null;
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000C1E8 File Offset: 0x0000A3E8
		private void OnGUI()
		{
			if (this.displayType == MCS_FPSCounter.GUIType.DisplayNothing)
			{
				return;
			}
			if (this.displayType == MCS_FPSCounter.GUIType.DisplayRunning)
			{
				if ((float)Screen.width != this.screenSize.x || (float)Screen.height != this.screenSize.y)
				{
					this.screenSize.x = (float)Screen.width;
					this.screenSize.y = (float)Screen.height;
					this.SetRectsRun();
				}
				GUI.Label(this.rectsRun[0], this.currentFPSText, this.bigStyleShadow);
				GUI.Label(this.rectsRun[1], this.avgFPSText, this.smallStyleShadow);
				GUI.Label(this.rectsRun[2], this.minFPSText, this.smallStyleShadow);
				GUI.Label(this.rectsRun[3], this.maxFSPText, this.smallStyleShadow);
				GUI.Label(this.rectsRun[4], "Avg:", this.smallStyleShadow);
				GUI.Label(this.rectsRun[5], "Min:", this.smallStyleShadow);
				GUI.Label(this.rectsRun[6], "Max:", this.smallStyleShadow);
				GUI.Label(this.rectsRun[7], this.currentFPSText, this.bigStyle);
				GUI.Label(this.rectsRun[8], this.avgFPSText, this.smallStyle);
				GUI.Label(this.rectsRun[9], this.minFPSText, this.smallStyle);
				GUI.Label(this.rectsRun[10], this.maxFSPText, this.smallStyle);
				GUI.Label(this.rectsRun[11], "Avg:", this.smallStyleLabel);
				GUI.Label(this.rectsRun[12], "Min:", this.smallStyleLabel);
				GUI.Label(this.rectsRun[13], "Max:", this.smallStyleLabel);
				return;
			}
			if ((float)Screen.width != this.screenSize.x || (float)Screen.height != this.screenSize.y)
			{
				this.screenSize.x = (float)Screen.width;
				this.screenSize.y = (float)Screen.height;
				this.SetRectsResult();
			}
			if (this.showLogoOnResultsScreen)
			{
				GUI.DrawTexture(this.rectsResult[8], this.logo);
			}
			GUI.Label(this.rectsResult[0], this.resultHeaderGUI, this.headerStyle);
			GUI.DrawTexture(this.rectsResult[1], Texture2D.whiteTexture);
			GUI.Label(this.rectsResult[2], this.reslutLabelAvgGUI, this.smallStyle);
			GUI.Label(this.rectsResult[4], "MINIMUM FPS:", this.smallStyleLabel);
			GUI.Label(this.rectsResult[6], "MAXIMUM FPS:", this.smallStyleLabel);
			GUI.Label(this.rectsResult[3], this.avgTextGUI, this.bigStyle);
			GUI.Label(this.rectsResult[5], this.minFPSText, this.smallStyle);
			GUI.Label(this.rectsResult[7], this.maxFSPText, this.smallStyle);
			GUI.Label(this.rectsResult[9], this.instructions, this.smallStyleLabel);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000C558 File Offset: 0x0000A758
		private void SetRectsRun()
		{
			this.columnRight = (float)Screen.width - 34f;
			this.columnLeft = this.columnRight - 34f;
			float num = 0f;
			this.rectsRun[0].Set((float)Screen.width - 48f + 1f, num + 4f + 2f, 40f, 22f);
			this.rectsRun[1].Set(this.columnRight + 1f, num + 30f + 2f, 26f, 22f);
			this.rectsRun[2].Set(this.columnRight + 1f, num + 44f + 2f, 26f, 22f);
			this.rectsRun[3].Set(this.columnRight + 1f, num + 58f + 2f, 26f, 22f);
			this.rectsRun[4].Set(this.columnLeft + 1f, num + 30f + 2f, 26f, 22f);
			this.rectsRun[5].Set(this.columnLeft + 1f, num + 44f + 2f, 26f, 22f);
			this.rectsRun[6].Set(this.columnLeft + 1f, num + 58f + 2f, 26f, 22f);
			this.rectsRun[7].Set((float)Screen.width - 53f, num + 4f, 45f, 22f);
			this.rectsRun[8].Set(this.columnRight, num + 30f, 26f, 22f);
			this.rectsRun[9].Set(this.columnRight, num + 44f, 26f, 22f);
			this.rectsRun[10].Set(this.columnRight, num + 58f, 26f, 22f);
			this.rectsRun[11].Set(this.columnLeft, num + 30f, 26f, 22f);
			this.rectsRun[12].Set(this.columnLeft, num + 44f, 26f, 22f);
			this.rectsRun[13].Set(this.columnLeft, num + 58f, 26f, 22f);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000C824 File Offset: 0x0000AA24
		private void SetRectsResult()
		{
			float num = 256f;
			this.rectsResult[8].Set((float)(Screen.width / 2 - this.logo.width / 2), (float)(Screen.height / 2) - num, (float)this.logo.width, (float)this.logo.height);
			Vector2 vector = this.headerStyle.CalcSize(this.resultHeaderGUI);
			this.rectsResult[0].Set((float)(Screen.width / 2) - vector.x / 2f, (float)(Screen.height / 2) - (num - 256f), vector.x, vector.y);
			vector.x += 10f;
			this.rectsResult[1].Set((float)(Screen.width / 2) - vector.x / 2f, (float)(Screen.height / 2) - (num - 256f - 30f), vector.x, 1f);
			this.rectsResult[2].Set((float)(Screen.width / 2 - 200), (float)(Screen.height / 2) - (num - 256f - 30f - 30f), 200f, 24f);
			this.rectsResult[4].Set((float)(Screen.width / 2 - 200), (float)(Screen.height / 2) - (num - 256f - 30f - 30f - 20f), 200f, 24f);
			this.rectsResult[6].Set((float)(Screen.width / 2 - 200), (float)(Screen.height / 2) - (num - 256f - 30f - 30f - 20f - 20f), 200f, 24f);
			this.rectsResult[3].Set((float)(Screen.width / 2), (float)(Screen.height / 2) - (num - 256f - 30f - 18f), 65f, 24f);
			this.rectsResult[5].Set((float)(Screen.width / 2), (float)(Screen.height / 2) - (num - 256f - 30f - 30f - 20f), 65f, 24f);
			this.rectsResult[7].Set((float)(Screen.width / 2), (float)(Screen.height / 2) - (num - 256f - 30f - 30f - 20f - 20f), 65f, 24f);
			vector = this.smallStyleLabel.CalcSize(this.instructions);
			this.rectsResult[9].Set((float)(Screen.width / 2) - vector.x / 2f, (float)(Screen.height / 2) - (num - 256f - 30f - 30f - 20f - 20f - 40f), vector.x, vector.y);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000CB50 File Offset: 0x0000AD50
		private void Start()
		{
			this.headerStyle.normal.textColor = this.label;
			this.headerStyle.fontSize = 24;
			this.headerStyle.font = this.fontResult;
			this.headerStyle.alignment = TextAnchor.UpperCenter;
			this.bigStyle.alignment = TextAnchor.UpperRight;
			this.bigStyle.font = this.fontRun;
			this.bigStyle.fontSize = 24;
			this.bigStyle.normal.textColor = Color.green;
			this.bigStyleShadow = new GUIStyle(this.bigStyle);
			this.bigStyleShadow.normal.textColor = this.fontShadow;
			this.smallStyle.alignment = TextAnchor.UpperRight;
			this.smallStyle.font = this.fontRun;
			this.smallStyle.fontSize = 12;
			this.smallStyle.normal.textColor = Color.white;
			this.smallStyleShadow = new GUIStyle(this.smallStyle);
			this.smallStyleShadow.normal.textColor = this.fontShadow;
			this.smallStyleLabel = new GUIStyle(this.smallStyle);
			this.smallStyleLabel.normal.textColor = this.label;
			base.Invoke("Reset", 0.5f);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000CCA0 File Offset: 0x0000AEA0
		private void Update()
		{
			if (this.displayType != this.oldDisplayType)
			{
				if (this.displayType == MCS_FPSCounter.GUIType.DisplayResults)
				{
					this.SetRectsResult();
					this.colorAvg = this.EvaluateGradient(this.averageFPS);
					this.bigStyle.normal.textColor = this.colorAvg;
					this.avgTextGUI.text = this.avgFPSText;
				}
				else if (this.displayType == MCS_FPSCounter.GUIType.DisplayRunning)
				{
					this.Reset();
					this.SetRectsRun();
				}
				this.oldDisplayType = this.displayType;
			}
			if (Input.GetKeyDown(this.showHideButton) && this.acceptInput && this.displayType != MCS_FPSCounter.GUIType.DisplayResults)
			{
				if (this.displayType == MCS_FPSCounter.GUIType.DisplayNothing)
				{
					this.displayType = MCS_FPSCounter.GUIType.DisplayRunning;
				}
				else
				{
					this.displayType = MCS_FPSCounter.GUIType.DisplayNothing;
				}
			}
			if (this.displayType == MCS_FPSCounter.GUIType.DisplayNothing)
			{
				return;
			}
			if (this.displayType == MCS_FPSCounter.GUIType.DisplayRunning)
			{
				this.GetFPS();
			}
			if (this.reset)
			{
				this.reset = false;
				this.Reset();
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000CD89 File Offset: 0x0000AF89
		public void StartBenchmark()
		{
			this.Reset();
			this.SetRectsRun();
			this.displayType = MCS_FPSCounter.GUIType.DisplayRunning;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000CD9E File Offset: 0x0000AF9E
		public void StopBenchmark()
		{
			this.SetRectsResult();
			this.displayType = MCS_FPSCounter.GUIType.DisplayResults;
			this.colorAvg = this.EvaluateGradient(this.averageFPS);
			this.bigStyle.normal.textColor = this.colorAvg;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000CDD8 File Offset: 0x0000AFD8
		private void GetFPS()
		{
			this.tempFrameCount++;
			this.totalFrameCount++;
			if ((double)Time.realtimeSinceStartup - this.tStampTemp > (double)this.interval)
			{
				this.currentFPS = (float)((double)this.tempFrameCount / ((double)Time.realtimeSinceStartup - this.tStampTemp));
				this.averageFPS = (float)((double)this.totalFrameCount / ((double)Time.realtimeSinceStartup - this.tStamp));
				if (this.currentFPS < this.minimumFPS)
				{
					this.minimumFPS = this.currentFPS;
				}
				if (this.currentFPS > this.maximumFPS)
				{
					this.maximumFPS = this.currentFPS;
				}
				this.tStampTemp = (double)Time.realtimeSinceStartup;
				this.tempFrameCount = 0;
				this.currentFPSText = "FPS " + this.currentFPS.ToString("0.0");
				this.avgFPSText = this.averageFPS.ToString("0.0");
				this.minFPSText = this.minimumFPS.ToString("0.0");
				this.maxFSPText = this.maximumFPS.ToString("0.0");
				this.colorCurrent = this.EvaluateGradient(this.currentFPS);
				this.bigStyle.normal.textColor = this.colorCurrent;
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000CF24 File Offset: 0x0000B124
		public void Reset()
		{
			this.tStamp = (double)Time.realtimeSinceStartup;
			this.tStampTemp = (double)Time.realtimeSinceStartup;
			this.currentFPS = 0f;
			this.averageFPS = 0f;
			this.minimumFPS = 999.9f;
			this.maximumFPS = 0f;
			this.tempFrameCount = 0;
			this.totalFrameCount = 0;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000CF83 File Offset: 0x0000B183
		private Color EvaluateGradient(float f)
		{
			return this.gradient.Evaluate(Mathf.Clamp01((f - this.gradientRange.x) / (this.gradientRange.y - this.gradientRange.x)));
		}

		// Token: 0x04000161 RID: 353
		public static MCS_FPSCounter instance;

		// Token: 0x04000162 RID: 354
		[Header("___ Settings ___________________________________________________________________________________________________________")]
		public float interval = 0.25f;

		// Token: 0x04000163 RID: 355
		public MCS_FPSCounter.GUIType displayType;

		// Token: 0x04000164 RID: 356
		public Vector2 gradientRange = new Vector2(15f, 60f);

		// Token: 0x04000165 RID: 357
		public Font fontRun;

		// Token: 0x04000166 RID: 358
		public Font fontResult;

		// Token: 0x04000167 RID: 359
		public Texture logo;

		// Token: 0x04000168 RID: 360
		public bool showLogoOnResultsScreen = true;

		// Token: 0x04000169 RID: 361
		public KeyCode showHideButton = KeyCode.Backspace;

		// Token: 0x0400016A RID: 362
		public bool acceptInput = true;

		// Token: 0x0400016B RID: 363
		public bool reset;

		// Token: 0x0400016C RID: 364
		[Header("___ Results ___________________________________________________________________________________________________________")]
		public float currentFPS;

		// Token: 0x0400016D RID: 365
		public float averageFPS;

		// Token: 0x0400016E RID: 366
		public float minimumFPS;

		// Token: 0x0400016F RID: 367
		public float maximumFPS;

		// Token: 0x04000170 RID: 368
		private int totalFrameCount;

		// Token: 0x04000171 RID: 369
		private int tempFrameCount;

		// Token: 0x04000172 RID: 370
		private double tStamp;

		// Token: 0x04000173 RID: 371
		private double tStampTemp;

		// Token: 0x04000174 RID: 372
		private string currentFPSText;

		// Token: 0x04000175 RID: 373
		private string avgFPSText;

		// Token: 0x04000176 RID: 374
		private string minFPSText;

		// Token: 0x04000177 RID: 375
		private string maxFSPText;

		// Token: 0x04000178 RID: 376
		private GUIStyle bigStyle = new GUIStyle();

		// Token: 0x04000179 RID: 377
		private GUIStyle bigStyleShadow;

		// Token: 0x0400017A RID: 378
		private GUIStyle smallStyle = new GUIStyle();

		// Token: 0x0400017B RID: 379
		private GUIStyle smallStyleShadow;

		// Token: 0x0400017C RID: 380
		private GUIStyle smallStyleLabel;

		// Token: 0x0400017D RID: 381
		private GUIStyle headerStyle = new GUIStyle();

		// Token: 0x0400017E RID: 382
		private Rect[] rectsRun = new Rect[14];

		// Token: 0x0400017F RID: 383
		private Rect[] rectsResult = new Rect[10];

		// Token: 0x04000180 RID: 384
		private Gradient gradient = new Gradient();

		// Token: 0x04000181 RID: 385
		private const float line1 = 4f;

		// Token: 0x04000182 RID: 386
		private const float line2 = 30f;

		// Token: 0x04000183 RID: 387
		private const float line3 = 44f;

		// Token: 0x04000184 RID: 388
		private const float line4 = 58f;

		// Token: 0x04000185 RID: 389
		private const float labelWidth = 26f;

		// Token: 0x04000186 RID: 390
		private const float paddingH = 8f;

		// Token: 0x04000187 RID: 391
		private const float lineHeight = 22f;

		// Token: 0x04000188 RID: 392
		private float columnRight;

		// Token: 0x04000189 RID: 393
		private float columnLeft;

		// Token: 0x0400018A RID: 394
		private Color fontShadow = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x0400018B RID: 395
		private Color label = new Color(0.8f, 0.8f, 0.8f, 1f);

		// Token: 0x0400018C RID: 396
		private Color colorCurrent;

		// Token: 0x0400018D RID: 397
		private Color colorAvg;

		// Token: 0x0400018E RID: 398
		private const string resultHeader = "BENCHMARK RESULTS";

		// Token: 0x0400018F RID: 399
		private const string resultLabelAvg = "AVERAGE FPS:";

		// Token: 0x04000190 RID: 400
		private const string resultLabelMin = "MINIMUM FPS:";

		// Token: 0x04000191 RID: 401
		private const string resultLabelMax = "MAXIMUM FPS:";

		// Token: 0x04000192 RID: 402
		private GUIContent resultHeaderGUI = new GUIContent("BENCHMARK RESULTS");

		// Token: 0x04000193 RID: 403
		private GUIContent reslutLabelAvgGUI = new GUIContent("AVERAGE FPS:");

		// Token: 0x04000194 RID: 404
		private GUIContent avgTextGUI = new GUIContent();

		// Token: 0x04000195 RID: 405
		private GUIContent instructions = new GUIContent("PRESS SPACEBAR TO RERUN BENCHMARK | PRESS ESCAPE TO RETURN TO MENU");

		// Token: 0x04000196 RID: 406
		private const string runLabelAvg = "Avg:";

		// Token: 0x04000197 RID: 407
		private const string runLabelMin = "Min:";

		// Token: 0x04000198 RID: 408
		private const string runLabelMax = "Max:";

		// Token: 0x04000199 RID: 409
		private Vector2 screenSize = new Vector2(0f, 0f);

		// Token: 0x0400019A RID: 410
		private MCS_FPSCounter.GUIType oldDisplayType = MCS_FPSCounter.GUIType.DisplayNothing;

		// Token: 0x0200006F RID: 111
		public enum GUIType
		{
			// Token: 0x040002B5 RID: 693
			DisplayRunning,
			// Token: 0x040002B6 RID: 694
			DisplayResults,
			// Token: 0x040002B7 RID: 695
			DisplayNothing
		}
	}
}
