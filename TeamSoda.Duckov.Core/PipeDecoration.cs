using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000149 RID: 329
[RequireComponent(typeof(PipeRenderer))]
public class PipeDecoration : MonoBehaviour
{
	// Token: 0x06000A96 RID: 2710 RVA: 0x0002DB50 File Offset: 0x0002BD50
	private void OnDrawGizmosSelected()
	{
		if (this.pipeRenderer == null)
		{
			this.pipeRenderer = base.GetComponent<PipeRenderer>();
		}
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0002DB6C File Offset: 0x0002BD6C
	private void Refresh()
	{
		if (this.pipeRenderer.splineInUse == null || this.pipeRenderer.splineInUse.Length < 1)
		{
			return;
		}
		for (int i = 0; i < this.decorations.Count; i++)
		{
			PipeDecoration.GameObjectOffset gameObjectOffset = this.decorations[i];
			Quaternion localRotation;
			Vector3 positionByOffset = this.pipeRenderer.GetPositionByOffset(gameObjectOffset.offset, out localRotation);
			Vector3 position = this.pipeRenderer.transform.localToWorldMatrix.MultiplyPoint(positionByOffset);
			if (!(gameObjectOffset.gameObject == null))
			{
				gameObjectOffset.gameObject.transform.position = position;
				gameObjectOffset.gameObject.transform.localRotation = localRotation;
				gameObjectOffset.gameObject.transform.Rotate(this.rotate);
				gameObjectOffset.gameObject.transform.localScale = this.scale * this.uniformScale;
			}
		}
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0002DC58 File Offset: 0x0002BE58
	public void OnValidate()
	{
		if (this.pipeRenderer == null)
		{
			this.pipeRenderer = base.GetComponent<PipeRenderer>();
		}
		this.Refresh();
	}

	// Token: 0x0400094B RID: 2379
	public PipeRenderer pipeRenderer;

	// Token: 0x0400094C RID: 2380
	[HideInInspector]
	public List<PipeDecoration.GameObjectOffset> decorations = new List<PipeDecoration.GameObjectOffset>();

	// Token: 0x0400094D RID: 2381
	public Vector3 rotate;

	// Token: 0x0400094E RID: 2382
	public Vector3 scale = Vector3.one;

	// Token: 0x0400094F RID: 2383
	public float uniformScale = 1f;

	// Token: 0x020004C3 RID: 1219
	[Serializable]
	public class GameObjectOffset
	{
		// Token: 0x04001D10 RID: 7440
		public GameObject gameObject;

		// Token: 0x04001D11 RID: 7441
		public float offset;
	}
}
