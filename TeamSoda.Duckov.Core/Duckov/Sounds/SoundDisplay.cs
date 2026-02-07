using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.Sounds
{
	// Token: 0x02000255 RID: 597
	public class SoundDisplay : MonoBehaviour
	{
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x00048120 File Offset: 0x00046320
		public float Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060012CE RID: 4814 RVA: 0x00048128 File Offset: 0x00046328
		public AISound CurrentSount
		{
			get
			{
				return this.sound;
			}
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00048130 File Offset: 0x00046330
		internal void Trigger(AISound sound)
		{
			this.sound = sound;
			base.gameObject.SetActive(true);
			this.velocity = this.triggerVelocity;
			this.value += this.velocity * Time.deltaTime;
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0004816C File Offset: 0x0004636C
		private void Update()
		{
			this.velocity -= this.gravity * Time.deltaTime;
			this.value += this.velocity * Time.deltaTime;
			if (this.value > 1f || this.value < 0f)
			{
				this.velocity = 0f;
			}
			this.value = Mathf.Clamp01(this.value);
			this.image.color = new Color(1f, 1f, 1f, this.value);
		}

		// Token: 0x04000E70 RID: 3696
		[SerializeField]
		private Image image;

		// Token: 0x04000E71 RID: 3697
		[SerializeField]
		private float removeRecordAfterTime = 1f;

		// Token: 0x04000E72 RID: 3698
		[SerializeField]
		private float triggerVelocity = 10f;

		// Token: 0x04000E73 RID: 3699
		[SerializeField]
		private float gravity = 1f;

		// Token: 0x04000E74 RID: 3700
		[SerializeField]
		private float untriggerVelocity = 100f;

		// Token: 0x04000E75 RID: 3701
		private float value;

		// Token: 0x04000E76 RID: 3702
		private float velocity;

		// Token: 0x04000E77 RID: 3703
		private AISound sound;
	}
}
