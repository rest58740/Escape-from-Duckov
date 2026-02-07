using System;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000316 RID: 790
	public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerMoveHandler
	{
		// Token: 0x060019E4 RID: 6628 RVA: 0x0005EBCE File Offset: 0x0005CDCE
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.RefreshFadeGroups();
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x0005EBE8 File Offset: 0x0005CDE8
		private void CacheRadius()
		{
			this.cachedRect = this.rectTransform.rect;
			Rect rect = this.cachedRect;
			this.cachedRadius = Mathf.Sqrt(rect.width * rect.width + rect.height * rect.height) / 2f;
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x0005EC3D File Offset: 0x0005CE3D
		private void Update()
		{
			if (this.rectTransform.rect != this.cachedRect)
			{
				this.CacheRadius();
			}
			this.HandleAnimation();
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x0005EC64 File Offset: 0x0005CE64
		private void HandleAnimation()
		{
			Quaternion quaternion = this.cardTransform.rotation;
			if ((this.facingFront && !this.frontFadeGroup.IsShown) || (!this.facingFront && !this.backFadeGroup.IsShown))
			{
				quaternion = Quaternion.RotateTowards(quaternion, Quaternion.Euler(0f, 90f, 0f), this.flipSpeed * Time.deltaTime);
				if (Mathf.Approximately(Quaternion.Angle(quaternion, Quaternion.Euler(0f, 90f, 0f)), 0f))
				{
					quaternion = Quaternion.Euler(0f, -90f, 0f);
					this.RefreshFadeGroups();
				}
			}
			else
			{
				quaternion = Quaternion.RotateTowards(quaternion, this.GetIdealRotation(), this.rotateSpeed * Time.deltaTime);
			}
			this.cardTransform.rotation = quaternion;
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x0005ED3F File Offset: 0x0005CF3F
		private void OnEnable()
		{
			this.CacheRadius();
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x0005ED48 File Offset: 0x0005CF48
		private Quaternion GetIdealRotation()
		{
			if (this.rectTransform.rect != this.cachedRect)
			{
				this.CacheRadius();
			}
			if (this.hovering && !Mathf.Approximately(this.cachedRadius, 0f))
			{
				Vector2 vector;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, this.pointerPosition, null, ref vector);
				Vector2 center = this.rectTransform.rect.center;
				Vector2 vector2 = vector - center;
				float num = Mathf.Max(10f, this.cachedRadius);
				Vector2 vector3 = Vector2.ClampMagnitude(vector2 / num, 1f);
				return Quaternion.Euler(-vector3.y * this.idleAmp, -vector3.x * this.idleAmp, 0f);
			}
			return Quaternion.Euler(Mathf.Sin(Time.time * this.idleFrequency * 3.1415927f * 2f) * this.idleAmp, Mathf.Cos(Time.time * this.idleFrequency * 3.1415927f * 2f) * this.idleAmp, 0f);
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x0005EE5C File Offset: 0x0005D05C
		private void SkipAnimation()
		{
			this.RefreshFadeGroups();
			this.cardTransform.rotation = this.GetIdealRotation();
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x0005EE75 File Offset: 0x0005D075
		public void SetFacing(bool facingFront, bool skipAnimation = false)
		{
			this.facingFront = facingFront;
			if (skipAnimation)
			{
				this.SkipAnimation();
			}
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x0005EE87 File Offset: 0x0005D087
		public void Flip()
		{
			this.SetFacing(!this.facingFront, false);
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x0005EE99 File Offset: 0x0005D099
		private void RefreshFadeGroups()
		{
			if (this.facingFront)
			{
				this.frontFadeGroup.Show();
				this.backFadeGroup.Hide();
				return;
			}
			this.frontFadeGroup.Hide();
			this.backFadeGroup.Show();
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x0005EED0 File Offset: 0x0005D0D0
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.hovering = true;
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x0005EED9 File Offset: 0x0005D0D9
		public void OnPointerExit(PointerEventData eventData)
		{
			this.hovering = false;
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x0005EEE2 File Offset: 0x0005D0E2
		public void OnPointerMove(PointerEventData eventData)
		{
			this.pointerPosition = eventData.position;
		}

		// Token: 0x040012CF RID: 4815
		private RectTransform rectTransform;

		// Token: 0x040012D0 RID: 4816
		[SerializeField]
		private RectTransform cardTransform;

		// Token: 0x040012D1 RID: 4817
		[SerializeField]
		private FadeGroup frontFadeGroup;

		// Token: 0x040012D2 RID: 4818
		[SerializeField]
		private FadeGroup backFadeGroup;

		// Token: 0x040012D3 RID: 4819
		[SerializeField]
		private float idleAmp = 10f;

		// Token: 0x040012D4 RID: 4820
		[SerializeField]
		private float idleFrequency = 0.5f;

		// Token: 0x040012D5 RID: 4821
		[SerializeField]
		private float rotateSpeed = 300f;

		// Token: 0x040012D6 RID: 4822
		[SerializeField]
		private float flipSpeed = 300f;

		// Token: 0x040012D7 RID: 4823
		private bool facingFront;

		// Token: 0x040012D8 RID: 4824
		private bool hovering;

		// Token: 0x040012D9 RID: 4825
		private Vector2 pointerPosition;

		// Token: 0x040012DA RID: 4826
		private Rect cachedRect;

		// Token: 0x040012DB RID: 4827
		private float cachedRadius;
	}
}
