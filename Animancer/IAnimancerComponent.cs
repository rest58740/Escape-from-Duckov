using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000028 RID: 40
	public interface IAnimancerComponent
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000336 RID: 822
		bool enabled { get; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000337 RID: 823
		GameObject gameObject { get; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000338 RID: 824
		// (set) Token: 0x06000339 RID: 825
		Animator Animator { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600033A RID: 826
		AnimancerPlayable Playable { get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600033B RID: 827
		bool IsPlayableInitialized { get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600033C RID: 828
		bool ResetOnDisable { get; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600033D RID: 829
		// (set) Token: 0x0600033E RID: 830
		AnimatorUpdateMode UpdateMode { get; set; }

		// Token: 0x0600033F RID: 831
		object GetKey(AnimationClip clip);
	}
}
