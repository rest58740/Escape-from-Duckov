using System;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace Duckov.MiniMaps.UI
{
	// Token: 0x0200028A RID: 650
	public class PointOfInterestEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x0004DE12 File Offset: 0x0004C012
		public MonoBehaviour Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0004DE1C File Offset: 0x0004C01C
		internal void Setup(MiniMapDisplay master, MonoBehaviour target, MiniMapDisplayEntry minimapEntry)
		{
			this.rectTransform = (base.transform as RectTransform);
			this.master = master;
			this.target = target;
			this.minimapEntry = minimapEntry;
			this.pointOfInterest = null;
			this.icon.sprite = this.defaultIcon;
			this.icon.color = this.defaultColor;
			this.areaDisplay.color = this.defaultColor;
			Color color = this.defaultColor;
			color.a *= 0.1f;
			this.areaFill.color = color;
			this.caption = target.name;
			this.icon.gameObject.SetActive(true);
			IPointOfInterest pointOfInterest = target as IPointOfInterest;
			if (pointOfInterest == null)
			{
				return;
			}
			this.pointOfInterest = pointOfInterest;
			this.icon.gameObject.SetActive(!this.pointOfInterest.HideIcon);
			this.icon.sprite = ((pointOfInterest.Icon != null) ? pointOfInterest.Icon : this.defaultIcon);
			this.icon.color = pointOfInterest.Color;
			if (this.shadow)
			{
				this.shadow.Color = pointOfInterest.ShadowColor;
				this.shadow.OffsetDistance = pointOfInterest.ShadowDistance;
			}
			string value = this.pointOfInterest.DisplayName;
			this.caption = pointOfInterest.DisplayName;
			if (string.IsNullOrEmpty(value))
			{
				this.displayName.gameObject.SetActive(false);
			}
			else
			{
				this.displayName.gameObject.SetActive(true);
				this.displayName.text = this.pointOfInterest.DisplayName;
			}
			if (pointOfInterest.IsArea)
			{
				this.areaDisplay.gameObject.SetActive(true);
				this.rectTransform.sizeDelta = this.pointOfInterest.AreaRadius * Vector2.one * 2f;
				this.areaDisplay.color = pointOfInterest.Color;
				color = pointOfInterest.Color;
				color.a *= 0.1f;
				this.areaFill.color = color;
				this.areaDisplay.BorderWidth = this.areaLineThickness / this.ParentLocalScale;
			}
			else
			{
				this.icon.enabled = true;
				this.areaDisplay.gameObject.SetActive(false);
			}
			this.RefreshPosition();
			base.gameObject.SetActive(true);
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0004E084 File Offset: 0x0004C284
		private void RefreshPosition()
		{
			this.cachedWorldPosition = this.target.transform.position;
			Vector3 centerOfObjectScene = MiniMapCenter.GetCenterOfObjectScene(this.target);
			Vector3 vector = this.target.transform.position - centerOfObjectScene;
			Vector3 vector2 = new Vector2(vector.x, vector.z);
			Vector3 position = this.minimapEntry.transform.localToWorldMatrix.MultiplyPoint(vector2);
			base.transform.position = position;
			this.UpdateScale();
			this.UpdateRotation();
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x0004E114 File Offset: 0x0004C314
		private float ParentLocalScale
		{
			get
			{
				return base.transform.parent.localScale.x;
			}
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0004E12B File Offset: 0x0004C32B
		private void Update()
		{
			this.UpdateScale();
			this.UpdatePosition();
			this.UpdateRotation();
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0004E140 File Offset: 0x0004C340
		private void UpdateScale()
		{
			float num = (this.pointOfInterest != null) ? this.pointOfInterest.ScaleFactor : 1f;
			this.iconContainer.localScale = Vector3.one * num / this.ParentLocalScale;
			if (this.pointOfInterest != null && this.pointOfInterest.IsArea)
			{
				this.areaDisplay.BorderWidth = this.areaLineThickness / this.ParentLocalScale;
				this.areaDisplay.FalloffDistance = 1f / this.ParentLocalScale;
			}
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x0004E1CD File Offset: 0x0004C3CD
		private void UpdatePosition()
		{
			if (this.cachedWorldPosition != this.target.transform.position)
			{
				this.RefreshPosition();
			}
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0004E1F2 File Offset: 0x0004C3F2
		private void UpdateRotation()
		{
			base.transform.rotation = Quaternion.identity;
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0004E204 File Offset: 0x0004C404
		public void OnPointerClick(PointerEventData eventData)
		{
			this.pointOfInterest.NotifyClicked(eventData);
			if (CheatMode.Active && UIInputManager.Ctrl && UIInputManager.Alt && UIInputManager.Shift)
			{
				if (MiniMapCenter.GetSceneID(this.target) == null)
				{
					return;
				}
				CharacterMainControl.Main.SetPosition(this.target.transform.position);
			}
		}

		// Token: 0x04000F56 RID: 3926
		private RectTransform rectTransform;

		// Token: 0x04000F57 RID: 3927
		private MiniMapDisplay master;

		// Token: 0x04000F58 RID: 3928
		private MonoBehaviour target;

		// Token: 0x04000F59 RID: 3929
		private IPointOfInterest pointOfInterest;

		// Token: 0x04000F5A RID: 3930
		private MiniMapDisplayEntry minimapEntry;

		// Token: 0x04000F5B RID: 3931
		[SerializeField]
		private Transform iconContainer;

		// Token: 0x04000F5C RID: 3932
		[SerializeField]
		private Sprite defaultIcon;

		// Token: 0x04000F5D RID: 3933
		[SerializeField]
		private Color defaultColor = Color.white;

		// Token: 0x04000F5E RID: 3934
		[SerializeField]
		private Image icon;

		// Token: 0x04000F5F RID: 3935
		[SerializeField]
		private TrueShadow shadow;

		// Token: 0x04000F60 RID: 3936
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04000F61 RID: 3937
		[SerializeField]
		private ProceduralImage areaDisplay;

		// Token: 0x04000F62 RID: 3938
		[SerializeField]
		private Image areaFill;

		// Token: 0x04000F63 RID: 3939
		[SerializeField]
		private float areaLineThickness = 1f;

		// Token: 0x04000F64 RID: 3940
		[SerializeField]
		private string caption;

		// Token: 0x04000F65 RID: 3941
		private Vector3 cachedWorldPosition = Vector3.zero;
	}
}
