using System;
using UnityEngine;

// Token: 0x02000147 RID: 327
public abstract class ShapeProvider : MonoBehaviour
{
	// Token: 0x06000A93 RID: 2707
	public abstract PipeRenderer.OrientedPoint[] GenerateShape();
}
