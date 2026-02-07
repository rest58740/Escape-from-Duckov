using System;
using UnityEngine.Animations;

namespace Animancer
{
	// Token: 0x0200003C RID: 60
	public abstract class AnimancerJob<T> where T : struct, IAnimationJob
	{
		// Token: 0x06000411 RID: 1041 RVA: 0x0000B5B7 File Offset: 0x000097B7
		protected void CreatePlayable(AnimancerPlayable animancer)
		{
			this._Playable = animancer.InsertOutputJob<T>(this._Job);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000B5CB File Offset: 0x000097CB
		public virtual void Destroy()
		{
			AnimancerUtilities.RemovePlayable(this._Playable, true);
		}

		// Token: 0x040000A8 RID: 168
		protected T _Job;

		// Token: 0x040000A9 RID: 169
		protected AnimationScriptPlayable _Playable;
	}
}
