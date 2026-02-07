using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.MiniGames
{
	// Token: 0x0200028C RID: 652
	public class VirtualCursor : MiniGameBehaviour
	{
		// Token: 0x060014F8 RID: 5368 RVA: 0x0004E2A0 File Offset: 0x0004C4A0
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = (base.transform as RectTransform);
			}
			if (this.moveArea == null)
			{
				this.moveArea = (this.rectTransform.parent as RectTransform);
			}
			if (this.canvas == null)
			{
				this.canvas = base.GetComponentInParent<Canvas>();
			}
			this.canvasRectTransform = (this.canvas.transform as RectTransform);
			if (this.raycaster == null)
			{
				this.raycaster = base.GetComponentInParent<GraphicRaycaster>();
			}
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x0004E33C File Offset: 0x0004C53C
		private void Update()
		{
			if (base.Game == null)
			{
				return;
			}
			if (base.Game.Console && !base.Game.Console.Interacting)
			{
				return;
			}
			Vector2 mouseDelta = UIInputManager.MouseDelta;
			Vector3 vector = this.rectTransform.localPosition + mouseDelta * this.sensitivity;
			Rect rect = this.moveArea.rect;
			vector.x = Mathf.Clamp(vector.x, rect.min.x, rect.max.x);
			vector.y = Mathf.Clamp(vector.y, rect.min.y, rect.max.y);
			this.rectTransform.localPosition = vector;
			List<RaycastResult> list = new List<RaycastResult>();
			this.Raycast(list);
			RaycastResult raycastResult = VirtualCursor.FindFirstRaycast(list);
			if (raycastResult.gameObject != VirtualCursor.raycastGO)
			{
				VirtualCursorTarget virtualCursorTarget = VirtualCursor.target;
				VirtualCursorTarget virtualCursorTarget2;
				if (raycastResult.gameObject != null)
				{
					virtualCursorTarget2 = raycastResult.gameObject.GetComponent<VirtualCursorTarget>();
				}
				else
				{
					virtualCursorTarget2 = null;
				}
				if (virtualCursorTarget2 != virtualCursorTarget)
				{
					VirtualCursor.target = virtualCursorTarget2;
					this.OnChange(virtualCursorTarget2, virtualCursorTarget);
				}
			}
			if (UIInputManager.WasClickedThisFrame && VirtualCursor.target != null)
			{
				VirtualCursor.target.OnClick();
			}
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0004E49C File Offset: 0x0004C69C
		private void OnChange(VirtualCursorTarget newTarget, VirtualCursorTarget oldTarget)
		{
			if (newTarget != null)
			{
				newTarget.OnCursorEnter();
			}
			if (oldTarget != null)
			{
				oldTarget.OnCursorExit();
			}
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0004E4BC File Offset: 0x0004C6BC
		private void Raycast(List<RaycastResult> resultAppendList)
		{
			if (this.canvas == null)
			{
				return;
			}
			IList<Graphic> raycastableGraphicsForCanvas = GraphicRegistry.GetRaycastableGraphicsForCanvas(this.canvas);
			VirtualCursor.s_canvasGraphics.Clear();
			if (raycastableGraphicsForCanvas == null || raycastableGraphicsForCanvas.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < raycastableGraphicsForCanvas.Count; i++)
			{
				VirtualCursor.s_canvasGraphics.Add(raycastableGraphicsForCanvas[i]);
			}
			Camera eventCamera = this.raycaster.eventCamera;
			Vector3 vector = eventCamera.WorldToScreenPoint(base.transform.position);
			vector.z = 0f;
			this.eventPositionWatch = vector;
			this.m_RaycastResults.Clear();
			VirtualCursor.Raycast(this.canvas, eventCamera, vector, raycastableGraphicsForCanvas, this.m_RaycastResults);
			int count = this.m_RaycastResults.Count;
			for (int j = 0; j < count; j++)
			{
				GameObject gameObject = this.m_RaycastResults[j].gameObject;
				float distance = 0f;
				Vector3 forward = gameObject.transform.forward;
				RaycastResult raycastResult = default(RaycastResult);
				raycastResult.gameObject = gameObject;
				raycastResult.module = this.raycaster;
				raycastResult.distance = distance;
				raycastResult.screenPosition = vector;
				raycastResult.displayIndex = 0;
				raycastResult.index = (float)resultAppendList.Count;
				raycastResult.depth = this.m_RaycastResults[j].depth;
				raycastResult.sortingLayer = this.canvas.sortingLayerID;
				raycastResult.sortingOrder = this.canvas.sortingOrder;
				raycastResult.worldPosition = vector;
				raycastResult.worldNormal = -forward;
				RaycastResult item = raycastResult;
				resultAppendList.Add(item);
			}
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x0004E66C File Offset: 0x0004C86C
		private static void Raycast(Canvas canvas, Camera eventCamera, Vector2 pointerPosition, IList<Graphic> foundGraphics, List<Graphic> results)
		{
			int count = foundGraphics.Count;
			for (int i = 0; i < count; i++)
			{
				Graphic graphic = foundGraphics[i];
				if (graphic.raycastTarget && !graphic.canvasRenderer.cull && graphic.depth != -1 && RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, pointerPosition, eventCamera, graphic.raycastPadding) && (!(eventCamera != null) || eventCamera.WorldToScreenPoint(graphic.rectTransform.position).z <= eventCamera.farClipPlane) && graphic.Raycast(pointerPosition, eventCamera))
				{
					VirtualCursor.s_SortedGraphics.Add(graphic);
				}
			}
			VirtualCursor.s_SortedGraphics.Sort((Graphic g1, Graphic g2) => g2.depth.CompareTo(g1.depth));
			count = VirtualCursor.s_SortedGraphics.Count;
			for (int j = 0; j < count; j++)
			{
				results.Add(VirtualCursor.s_SortedGraphics[j]);
			}
			VirtualCursor.s_SortedGraphics.Clear();
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0004E760 File Offset: 0x0004C960
		private static RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
		{
			int count = candidates.Count;
			for (int i = 0; i < count; i++)
			{
				if (!(candidates[i].gameObject == null))
				{
					return candidates[i];
				}
			}
			return default(RaycastResult);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0004E7A8 File Offset: 0x0004C9A8
		internal static bool IsHovering(VirtualCursorTarget virtualCursorTarget)
		{
			return virtualCursorTarget == VirtualCursor.target;
		}

		// Token: 0x04000F66 RID: 3942
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x04000F67 RID: 3943
		[SerializeField]
		private RectTransform moveArea;

		// Token: 0x04000F68 RID: 3944
		[SerializeField]
		private Canvas canvas;

		// Token: 0x04000F69 RID: 3945
		private RectTransform canvasRectTransform;

		// Token: 0x04000F6A RID: 3946
		[SerializeField]
		private GraphicRaycaster raycaster;

		// Token: 0x04000F6B RID: 3947
		[SerializeField]
		private float sensitivity = 0.5f;

		// Token: 0x04000F6C RID: 3948
		private static GameObject raycastGO;

		// Token: 0x04000F6D RID: 3949
		private static VirtualCursorTarget target;

		// Token: 0x04000F6E RID: 3950
		[NonSerialized]
		private List<Graphic> m_RaycastResults = new List<Graphic>();

		// Token: 0x04000F6F RID: 3951
		private Vector3 eventPositionWatch;

		// Token: 0x04000F70 RID: 3952
		[NonSerialized]
		private static List<Graphic> s_canvasGraphics = new List<Graphic>();

		// Token: 0x04000F71 RID: 3953
		[NonSerialized]
		private static readonly List<Graphic> s_SortedGraphics = new List<Graphic>();
	}
}
