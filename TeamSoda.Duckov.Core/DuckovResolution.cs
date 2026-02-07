using System;
using UnityEngine;

// Token: 0x020001D6 RID: 470
[Serializable]
public struct DuckovResolution
{
	// Token: 0x06000E26 RID: 3622 RVA: 0x0003AA04 File Offset: 0x00038C04
	public override bool Equals(object obj)
	{
		if (obj is DuckovResolution)
		{
			DuckovResolution duckovResolution = (DuckovResolution)obj;
			if (duckovResolution.height == this.height && duckovResolution.width == this.width)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x0003AA3F File Offset: 0x00038C3F
	public override string ToString()
	{
		return string.Format("{0} x {1}", this.width, this.height);
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x0003AA61 File Offset: 0x00038C61
	public DuckovResolution(Resolution res)
	{
		this.height = res.height;
		this.width = res.width;
	}

	// Token: 0x06000E29 RID: 3625 RVA: 0x0003AA7D File Offset: 0x00038C7D
	public DuckovResolution(int _width, int _height)
	{
		this.height = _height;
		this.width = _width;
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x0003AA90 File Offset: 0x00038C90
	public bool CheckRotioFit(DuckovResolution newRes, DuckovResolution defaultRes)
	{
		float num = (float)newRes.height / (float)newRes.width;
		return Mathf.Abs((float)defaultRes.height - num * (float)defaultRes.width) <= 2f;
	}

	// Token: 0x04000C0A RID: 3082
	public int width;

	// Token: 0x04000C0B RID: 3083
	public int height;
}
