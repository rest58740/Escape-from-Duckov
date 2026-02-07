using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;
using UnityEngine.UI;

namespace UI_Spline_Renderer.Example
{
	// Token: 0x02000051 RID: 81
	public class DraggableSplinePointExample : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
	{
		// Token: 0x0600030D RID: 781 RVA: 0x0000CA68 File Offset: 0x0000AC68
		public void OnDrag(PointerEventData eventData)
		{
			Vector3 v = base.transform.parent.InverseTransformPoint(eventData.position);
			BezierKnot value = new BezierKnot(v);
			this.uiSplineRenderer.splineContainer[this.splineIndex].SetKnot(this.knotIndex, value, BezierTangent.Out);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000CAC4 File Offset: 0x0000ACC4
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.uiSplineRenderer.raycastTarget = false;
			this.myImage.raycastTarget = false;
			this.uiSplineRenderer.color = Color.white;
			if (!this.isConnected)
			{
				this._originalKnot = this.uiSplineRenderer.splineContainer[this.splineIndex][this.knotIndex];
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000CB28 File Offset: 0x0000AD28
		public void OnEndDrag(PointerEventData eventData)
		{
			foreach (GameObject gameObject in eventData.hovered)
			{
				if (gameObject.GetComponent<DragPortExample>())
				{
					this.Connect(gameObject.transform);
					this.uiSplineRenderer.raycastTarget = true;
					this.myImage.raycastTarget = true;
					return;
				}
			}
			this.Disconnect();
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000CBB0 File Offset: 0x0000ADB0
		private void Connect(Transform t)
		{
			Vector3 v = base.transform.parent.InverseTransformPoint(t.position);
			BezierKnot value = new BezierKnot(v);
			this.uiSplineRenderer.splineContainer[this.splineIndex].SetKnot(this.knotIndex, value, BezierTangent.Out);
			this.uiSplineRenderer.color = this.connectedColor;
			this.isConnected = true;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000CC1C File Offset: 0x0000AE1C
		private void Disconnect()
		{
			this.uiSplineRenderer.color = Color.white;
			this.uiSplineRenderer.splineContainer[this.splineIndex].SetKnot(this.knotIndex, this._originalKnot, BezierTangent.Out);
			this.isConnected = false;
			this.uiSplineRenderer.raycastTarget = true;
			this.myImage.raycastTarget = true;
		}

		// Token: 0x040001D6 RID: 470
		public UISplineRenderer uiSplineRenderer;

		// Token: 0x040001D7 RID: 471
		public Image myImage;

		// Token: 0x040001D8 RID: 472
		public int splineIndex;

		// Token: 0x040001D9 RID: 473
		public int knotIndex;

		// Token: 0x040001DA RID: 474
		public Color connectedColor;

		// Token: 0x040001DB RID: 475
		public bool isConnected;

		// Token: 0x040001DC RID: 476
		private BezierKnot _originalKnot;
	}
}
