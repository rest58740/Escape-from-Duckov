using System;
using DG.Tweening.Core.Enums;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x02000050 RID: 80
	public class DOTweenSettings : ScriptableObject
	{
		// Token: 0x04000150 RID: 336
		public const string AssetName = "DOTweenSettings";

		// Token: 0x04000151 RID: 337
		public const string AssetFullFilename = "DOTweenSettings.asset";

		// Token: 0x04000152 RID: 338
		public bool useSafeMode = true;

		// Token: 0x04000153 RID: 339
		public DOTweenSettings.SafeModeOptions safeModeOptions = new DOTweenSettings.SafeModeOptions();

		// Token: 0x04000154 RID: 340
		public float timeScale = 1f;

		// Token: 0x04000155 RID: 341
		public bool useSmoothDeltaTime;

		// Token: 0x04000156 RID: 342
		public float maxSmoothUnscaledTime = 0.15f;

		// Token: 0x04000157 RID: 343
		public RewindCallbackMode rewindCallbackMode;

		// Token: 0x04000158 RID: 344
		public bool showUnityEditorReport;

		// Token: 0x04000159 RID: 345
		public LogBehaviour logBehaviour;

		// Token: 0x0400015A RID: 346
		public bool drawGizmos = true;

		// Token: 0x0400015B RID: 347
		public bool defaultRecyclable;

		// Token: 0x0400015C RID: 348
		public AutoPlay defaultAutoPlay = AutoPlay.All;

		// Token: 0x0400015D RID: 349
		public UpdateType defaultUpdateType;

		// Token: 0x0400015E RID: 350
		public bool defaultTimeScaleIndependent;

		// Token: 0x0400015F RID: 351
		public Ease defaultEaseType = Ease.OutQuad;

		// Token: 0x04000160 RID: 352
		public float defaultEaseOvershootOrAmplitude = 1.70158f;

		// Token: 0x04000161 RID: 353
		public float defaultEasePeriod;

		// Token: 0x04000162 RID: 354
		public bool defaultAutoKill = true;

		// Token: 0x04000163 RID: 355
		public LoopType defaultLoopType;

		// Token: 0x04000164 RID: 356
		public bool debugMode;

		// Token: 0x04000165 RID: 357
		public bool debugStoreTargetId = true;

		// Token: 0x04000166 RID: 358
		public bool showPreviewPanel = true;

		// Token: 0x04000167 RID: 359
		public DOTweenSettings.SettingsLocation storeSettingsLocation;

		// Token: 0x04000168 RID: 360
		public DOTweenSettings.ModulesSetup modules = new DOTweenSettings.ModulesSetup();

		// Token: 0x04000169 RID: 361
		public bool createASMDEF;

		// Token: 0x0400016A RID: 362
		public bool showPlayingTweens;

		// Token: 0x0400016B RID: 363
		public bool showPausedTweens;

		// Token: 0x020000C0 RID: 192
		public enum SettingsLocation
		{
			// Token: 0x0400026B RID: 619
			AssetsDirectory,
			// Token: 0x0400026C RID: 620
			DOTweenDirectory,
			// Token: 0x0400026D RID: 621
			DemigiantDirectory
		}

		// Token: 0x020000C1 RID: 193
		[Serializable]
		public class SafeModeOptions
		{
			// Token: 0x0400026E RID: 622
			public SafeModeLogBehaviour logBehaviour = SafeModeLogBehaviour.Warning;

			// Token: 0x0400026F RID: 623
			public NestedTweenFailureBehaviour nestedTweenFailureBehaviour;
		}

		// Token: 0x020000C2 RID: 194
		[Serializable]
		public class ModulesSetup
		{
			// Token: 0x04000270 RID: 624
			public bool showPanel;

			// Token: 0x04000271 RID: 625
			public bool audioEnabled = true;

			// Token: 0x04000272 RID: 626
			public bool physicsEnabled = true;

			// Token: 0x04000273 RID: 627
			public bool physics2DEnabled = true;

			// Token: 0x04000274 RID: 628
			public bool spriteEnabled = true;

			// Token: 0x04000275 RID: 629
			public bool uiEnabled = true;

			// Token: 0x04000276 RID: 630
			public bool textMeshProEnabled;

			// Token: 0x04000277 RID: 631
			public bool tk2DEnabled;

			// Token: 0x04000278 RID: 632
			public bool deAudioEnabled;

			// Token: 0x04000279 RID: 633
			public bool deUnityExtendedEnabled;

			// Token: 0x0400027A RID: 634
			public bool epoOutlineEnabled;
		}
	}
}
