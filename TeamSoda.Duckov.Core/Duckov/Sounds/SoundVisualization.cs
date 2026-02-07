using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Sounds
{
	// Token: 0x02000256 RID: 598
	public class SoundVisualization : MonoBehaviour
	{
		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060012D2 RID: 4818 RVA: 0x0004823C File Offset: 0x0004643C
		private PrefabPool<SoundDisplay> DisplayPool
		{
			get
			{
				if (this._displayPool == null)
				{
					this._displayPool = new PrefabPool<SoundDisplay>(this.displayTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._displayPool;
			}
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00048275 File Offset: 0x00046475
		private void Awake()
		{
			AIMainBrain.OnPlayerHearSound += this.OnHeardSound;
			if (this.layoutCenter == null)
			{
				this.layoutCenter = (base.transform as RectTransform);
			}
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x000482A7 File Offset: 0x000464A7
		private void OnDestroy()
		{
			AIMainBrain.OnPlayerHearSound -= this.OnHeardSound;
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x000482BC File Offset: 0x000464BC
		private void Update()
		{
			using (IEnumerator<SoundDisplay> enumerator = this.DisplayPool.ActiveEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SoundDisplay soundDisplay = enumerator.Current;
					if (soundDisplay.Value <= 0f)
					{
						this.releaseBuffer.Enqueue(soundDisplay);
					}
					else
					{
						this.RefreshEntryPosition(soundDisplay);
					}
				}
				goto IL_71;
			}
			IL_50:
			SoundDisplay soundDisplay2 = this.releaseBuffer.Dequeue();
			if (!(soundDisplay2 == null))
			{
				this.DisplayPool.Release(soundDisplay2);
			}
			IL_71:
			if (this.releaseBuffer.Count <= 0)
			{
				return;
			}
			goto IL_50;
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00048358 File Offset: 0x00046558
		private void OnHeardSound(AISound sound)
		{
			this.Trigger(sound);
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00048364 File Offset: 0x00046564
		private void Trigger(AISound sound)
		{
			if (GameCamera.Instance == null)
			{
				return;
			}
			SoundDisplay soundDisplay = null;
			if (sound.fromCharacter != null)
			{
				foreach (SoundDisplay soundDisplay2 in this.DisplayPool.ActiveEntries)
				{
					AISound currentSount = soundDisplay2.CurrentSount;
					if (!(currentSount.fromCharacter != sound.fromCharacter) && currentSount.soundType == sound.soundType && Vector3.Distance(currentSount.pos, sound.pos) < this.retriggerDistanceThreshold)
					{
						soundDisplay = soundDisplay2;
					}
				}
			}
			if (soundDisplay == null)
			{
				soundDisplay = this.DisplayPool.Get(null);
			}
			this.RefreshEntryPosition(soundDisplay);
			soundDisplay.Trigger(sound);
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00048434 File Offset: 0x00046634
		private void RefreshEntryPosition(SoundDisplay e)
		{
			Vector3 pos = e.CurrentSount.pos;
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(GameCamera.Instance.renderCamera, pos);
			Vector2 vector2;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.layoutCenter, vector, null, ref vector2);
			Vector2 normalized = vector2.normalized;
			e.transform.localPosition = normalized * this.displayOffset;
			e.transform.rotation = Quaternion.FromToRotation(Vector2.up, normalized);
		}

		// Token: 0x04000E78 RID: 3704
		[SerializeField]
		private RectTransform layoutCenter;

		// Token: 0x04000E79 RID: 3705
		[SerializeField]
		private SoundDisplay displayTemplate;

		// Token: 0x04000E7A RID: 3706
		[SerializeField]
		private float retriggerDistanceThreshold = 1f;

		// Token: 0x04000E7B RID: 3707
		[SerializeField]
		private float displayOffset = 400f;

		// Token: 0x04000E7C RID: 3708
		private PrefabPool<SoundDisplay> _displayPool;

		// Token: 0x04000E7D RID: 3709
		private Queue<SoundDisplay> releaseBuffer = new Queue<SoundDisplay>();
	}
}
