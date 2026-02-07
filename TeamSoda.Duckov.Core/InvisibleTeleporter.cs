using System;
using Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020000A9 RID: 169
[Obsolete]
public class InvisibleTeleporter : MonoBehaviour, IDrawGizmos
{
	// Token: 0x1700012B RID: 299
	// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001A391 File Offset: 0x00018591
	private bool UsePosition
	{
		get
		{
			return this.target == null;
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0001A3A0 File Offset: 0x000185A0
	private Vector3 TargetWorldPosition
	{
		get
		{
			if (this.target != null)
			{
				return this.target.transform.position;
			}
			Space space = this.space;
			if (space == null)
			{
				return this.position;
			}
			if (space != 1)
			{
				return default(Vector3);
			}
			return base.transform.TransformPoint(this.position);
		}
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0001A400 File Offset: 0x00018600
	public void Teleport()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		GameCamera instance = GameCamera.Instance;
		Vector3 vector = instance.transform.position - main.transform.position;
		main.SetPosition(this.TargetWorldPosition);
		Vector3 vector2 = main.transform.position + vector;
		instance.transform.position = vector2;
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0001A467 File Offset: 0x00018667
	private void LateUpdate()
	{
		if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
		{
			this.Teleport();
		}
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0001A488 File Offset: 0x00018688
	public void DrawGizmos()
	{
		if (!GizmoContext.InActiveSelection(this))
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			Draw.Arrow(base.transform.position, this.TargetWorldPosition);
			return;
		}
		Draw.Arrow(main.transform.position, this.TargetWorldPosition);
	}

	// Token: 0x0400055E RID: 1374
	[SerializeField]
	private Transform target;

	// Token: 0x0400055F RID: 1375
	[SerializeField]
	private Vector3 position;

	// Token: 0x04000560 RID: 1376
	[SerializeField]
	private Space space;
}
