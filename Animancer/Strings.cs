using System;

namespace Animancer
{
	// Token: 0x02000027 RID: 39
	public static class Strings
	{
		// Token: 0x04000089 RID: 137
		public const string ProductName = "Animancer";

		// Token: 0x0400008A RID: 138
		public const string MenuPrefix = "Animancer/";

		// Token: 0x0400008B RID: 139
		public const string CreateMenuPrefix = "Assets/Create/Animancer/";

		// Token: 0x0400008C RID: 140
		public const string ExamplesMenuPrefix = "Animancer/Examples/";

		// Token: 0x0400008D RID: 141
		public const string AnimancerToolsMenuPath = "Window/Animation/Animancer Tools";

		// Token: 0x0400008E RID: 142
		public const int AssetMenuOrder = 410;

		// Token: 0x0400008F RID: 143
		public const string UnityEditor = "UNITY_EDITOR";

		// Token: 0x04000090 RID: 144
		public const string Assertions = "UNITY_ASSERTIONS";

		// Token: 0x04000091 RID: 145
		public const string Indent = "    ";

		// Token: 0x04000092 RID: 146
		public const string ProOnlyTag = "";

		// Token: 0x04000093 RID: 147
		public const string MustBeFinite = "must not be NaN or Infinity";

		// Token: 0x0200008E RID: 142
		public static class DocsURLs
		{
			// Token: 0x04000140 RID: 320
			public const string Documentation = "https://kybernetik.com.au/animancer";

			// Token: 0x04000141 RID: 321
			public const string APIDocumentation = "https://kybernetik.com.au/animancer/api/Animancer";

			// Token: 0x04000142 RID: 322
			public const string ExampleAPIDocumentation = "https://kybernetik.com.au/animancer/api/Animancer.Examples.";

			// Token: 0x04000143 RID: 323
			public const string DeveloperEmail = "animancer@kybernetik.com.au";

			// Token: 0x04000144 RID: 324
			public const string LatestVersion = "https://kybernetik.com.au/animancer/latest-version.txt";

			// Token: 0x04000145 RID: 325
			public const string OptionalWarning = "https://kybernetik.com.au/animancer/api/Animancer/OptionalWarning";
		}

		// Token: 0x0200008F RID: 143
		public static class Tooltips
		{
			// Token: 0x04000146 RID: 326
			public const string MiddleClickReset = "\n• Middle Click = reset to default value";

			// Token: 0x04000147 RID: 327
			public const string FadeDuration = "The amount of time the transition will take, e.g:\n• 0s = Instant\n• 0.25s = quarter of a second (Default)\n• 0.25x = quarter of the animation length\n• x = Normalized, s = Seconds, f = Frame\n• Middle Click = reset to default value";

			// Token: 0x04000148 RID: 328
			public const string Speed = "How fast the animation will play, e.g:\n• 0x = paused\n• 1x = normal speed\n• -2x = double speed backwards";

			// Token: 0x04000149 RID: 329
			public const string OptionalSpeed = "How fast the animation will play, e.g:\n• 0x = paused\n• 1x = normal speed\n• -2x = double speed backwards\n• Disabled = keep previous speed\n• Middle Click = reset to default value";

			// Token: 0x0400014A RID: 330
			public const string NormalizedStartTime = "• Enabled = use FadeMode.FromStart and always restart at this time.\n• Disabled = use FadeMode.FixedSpeed and continue from the current time if already playing.\n• x = Normalized, s = Seconds, f = Frame";

			// Token: 0x0400014B RID: 331
			public const string EndTime = "The time when the End Callback will be triggered.\n• x = Normalized, s = Seconds, f = Frame\n\nDisabling the toggle automates the value:\n• Speed >= 0 ends at 1x\n• Speed < 0 ends at 0x";

			// Token: 0x0400014C RID: 332
			public const string CallbackTime = "The time when the Event Callback will be triggered.\n• x = Normalized, s = Seconds, f = Frame";
		}
	}
}
