using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Duckov.Tasks
{
	// Token: 0x0200038D RID: 909
	public class PlayTimelineTask : MonoBehaviour, ITaskBehaviour
	{
		// Token: 0x06001F91 RID: 8081 RVA: 0x0006F9FC File Offset: 0x0006DBFC
		private void Awake()
		{
			this.timeline.stopped += this.OnTimelineStopped;
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x0006FA15 File Offset: 0x0006DC15
		private void OnDestroy()
		{
			if (this.timeline != null)
			{
				this.timeline.stopped -= this.OnTimelineStopped;
			}
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x0006FA3C File Offset: 0x0006DC3C
		private void OnTimelineStopped(PlayableDirector director)
		{
			this.running = false;
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x0006FA45 File Offset: 0x0006DC45
		public void Begin()
		{
			this.running = true;
			this.timeline.Play();
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x0006FA59 File Offset: 0x0006DC59
		public bool IsComplete()
		{
			return this.timeline.time > this.timeline.duration - 0.009999999776482582 || this.timeline.state != 1;
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x0006FA90 File Offset: 0x0006DC90
		public bool IsPending()
		{
			return this.timeline.time <= this.timeline.duration - 0.009999999776482582 && this.timeline.state == 1;
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x0006FAC4 File Offset: 0x0006DCC4
		public void Skip()
		{
			this.timeline.Stop();
		}

		// Token: 0x04001598 RID: 5528
		[SerializeField]
		private PlayableDirector timeline;

		// Token: 0x04001599 RID: 5529
		private bool running;
	}
}
