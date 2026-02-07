using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000049 RID: 73
	[Serializable]
	public class DirectionalClipTransition : ClipTransition, ICopyable<DirectionalClipTransition>
	{
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0000D099 File Offset: 0x0000B299
		public ref DirectionalAnimationSet AnimationSet
		{
			get
			{
				return ref this._AnimationSet;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0000D0A1 File Offset: 0x0000B2A1
		public override UnityEngine.Object MainObject
		{
			get
			{
				return this._AnimationSet;
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000D0A9 File Offset: 0x0000B2A9
		public void SetDirection(Vector2 direction)
		{
			base.Clip = this._AnimationSet.GetClip(direction);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000D0BD File Offset: 0x0000B2BD
		public void SetDirection(int direction)
		{
			base.Clip = this._AnimationSet.GetClip(direction);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000D0D1 File Offset: 0x0000B2D1
		public void SetDirection(DirectionalAnimationSet.Direction direction)
		{
			base.Clip = this._AnimationSet.GetClip(direction);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000D0E5 File Offset: 0x0000B2E5
		public void SetDirection(DirectionalAnimationSet8.Direction direction)
		{
			base.Clip = this._AnimationSet.GetClip((int)direction);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000D0F9 File Offset: 0x0000B2F9
		public override void GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			base.GatherAnimationClips(clips);
			clips.GatherFromSource(this._AnimationSet);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000D10E File Offset: 0x0000B30E
		public virtual void CopyFrom(DirectionalClipTransition copyFrom)
		{
			base.CopyFrom(copyFrom);
			if (copyFrom == null)
			{
				this._AnimationSet = null;
				return;
			}
			this._AnimationSet = copyFrom._AnimationSet;
		}

		// Token: 0x040000C5 RID: 197
		[SerializeField]
		[Tooltip("The animations which used to determine the Clip")]
		private DirectionalAnimationSet _AnimationSet;
	}
}
