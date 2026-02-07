using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200000A RID: 10
	public class IMColorPickerInteraction : MonoBehaviour
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00003484 File Offset: 0x00001684
		private void Update()
		{
			if (Camera.main != null)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				this.RaycastInteract(ray, Input.GetMouseButtonDown(0), Input.GetMouseButton(0), Input.GetMouseButtonUp(0));
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000034C7 File Offset: 0x000016C7
		private void OnDisable()
		{
			this.currentInteraction = IMColorPickerInteraction.ColorPickerElement.None;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000034D0 File Offset: 0x000016D0
		public void RaycastInteract(Ray ray, bool onPress, bool whileHeld, bool onRelease)
		{
			if (onPress || whileHeld)
			{
				ray.origin = base.transform.InverseTransformPoint(ray.origin);
				ray.direction = base.transform.InverseTransformDirection(ray.direction);
				float distance;
				if (new Plane(Vector3.back, 0f).Raycast(ray, out distance))
				{
					Vector2 pt = ray.GetPoint(distance);
					if (onPress)
					{
						this.currentInteraction = this.GetPickerElementAt(pt);
					}
					if (whileHeld && this.currentInteraction != IMColorPickerInteraction.ColorPickerElement.None)
					{
						this.UpdatePickerColor(pt);
					}
				}
			}
			if (onRelease)
			{
				this.currentInteraction = IMColorPickerInteraction.ColorPickerElement.None;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000356C File Offset: 0x0000176C
		private void UpdatePickerColor(Vector2 pt)
		{
			if (this.currentInteraction == IMColorPickerInteraction.ColorPickerElement.HueStrip)
			{
				this.picker.hue = IMColorPickerRenderer.VectorToHue(pt);
				return;
			}
			if (this.currentInteraction == IMColorPickerInteraction.ColorPickerElement.Rectangle)
			{
				Vector2 vector = ShapesMath.InverseLerp(this.picker.QuadRect, pt);
				this.picker.saturation = Mathf.Clamp01(vector.x);
				this.picker.value = Mathf.Clamp01(vector.y);
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000035DC File Offset: 0x000017DC
		private IMColorPickerInteraction.ColorPickerElement GetPickerElementAt(Vector2 pt)
		{
			if (this.HueStripContains(pt))
			{
				return IMColorPickerInteraction.ColorPickerElement.HueStrip;
			}
			if (this.picker.QuadRect.Contains(pt))
			{
				return IMColorPickerInteraction.ColorPickerElement.Rectangle;
			}
			return IMColorPickerInteraction.ColorPickerElement.None;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003610 File Offset: 0x00001810
		private bool HueStripContains(Vector2 pt)
		{
			float magnitude = pt.magnitude;
			return magnitude >= this.picker.HueStripRadiusInner && magnitude <= this.picker.HueStripRadiusOuter;
		}

		// Token: 0x0400004F RID: 79
		public IMColorPickerRenderer picker;

		// Token: 0x04000050 RID: 80
		private IMColorPickerInteraction.ColorPickerElement currentInteraction;

		// Token: 0x02000013 RID: 19
		private enum ColorPickerElement
		{
			// Token: 0x0400007C RID: 124
			None,
			// Token: 0x0400007D RID: 125
			HueStrip,
			// Token: 0x0400007E RID: 126
			Rectangle
		}
	}
}
