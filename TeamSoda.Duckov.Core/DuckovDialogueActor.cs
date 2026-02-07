using System;
using System.Collections.Generic;
using NodeCanvas.DialogueTrees;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class DuckovDialogueActor : MonoBehaviour, IDialogueActor
{
	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00038189 File Offset: 0x00036389
	private static List<DuckovDialogueActor> ActiveActors
	{
		get
		{
			if (DuckovDialogueActor._activeActors == null)
			{
				DuckovDialogueActor._activeActors = new List<DuckovDialogueActor>();
			}
			return DuckovDialogueActor._activeActors;
		}
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x000381A1 File Offset: 0x000363A1
	public static void Register(DuckovDialogueActor actor)
	{
		if (DuckovDialogueActor.ActiveActors.Contains(actor))
		{
			Debug.Log("Actor " + actor.nameKey + " 在重复注册", actor);
			return;
		}
		DuckovDialogueActor.ActiveActors.Add(actor);
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x000381D7 File Offset: 0x000363D7
	public static void Unregister(DuckovDialogueActor actor)
	{
		DuckovDialogueActor.ActiveActors.Remove(actor);
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x000381E8 File Offset: 0x000363E8
	public static DuckovDialogueActor Get(string id)
	{
		return DuckovDialogueActor.ActiveActors.Find((DuckovDialogueActor e) => e.ID == id);
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06000D4F RID: 3407 RVA: 0x00038218 File Offset: 0x00036418
	public string ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06000D50 RID: 3408 RVA: 0x00038220 File Offset: 0x00036420
	public Vector3 Offset
	{
		get
		{
			return this.offset;
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06000D51 RID: 3409 RVA: 0x00038228 File Offset: 0x00036428
	public string NameKey
	{
		get
		{
			return this.nameKey;
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06000D52 RID: 3410 RVA: 0x00038230 File Offset: 0x00036430
	public Texture2D portrait
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06000D53 RID: 3411 RVA: 0x00038233 File Offset: 0x00036433
	public Sprite portraitSprite
	{
		get
		{
			return this._portraitSprite;
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06000D54 RID: 3412 RVA: 0x0003823C File Offset: 0x0003643C
	public Color dialogueColor
	{
		get
		{
			return default(Color);
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06000D55 RID: 3413 RVA: 0x00038254 File Offset: 0x00036454
	public Vector3 dialoguePosition
	{
		get
		{
			return default(Vector3);
		}
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x0003826A File Offset: 0x0003646A
	private void OnEnable()
	{
		DuckovDialogueActor.Register(this);
	}

	// Token: 0x06000D57 RID: 3415 RVA: 0x00038272 File Offset: 0x00036472
	private void OnDisable()
	{
		DuckovDialogueActor.Unregister(this);
	}

	// Token: 0x06000D59 RID: 3417 RVA: 0x00038282 File Offset: 0x00036482
	string IDialogueActor.get_name()
	{
		return base.name;
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x0003828A File Offset: 0x0003648A
	Transform IDialogueActor.get_transform()
	{
		return base.transform;
	}

	// Token: 0x04000B8C RID: 2956
	private static List<DuckovDialogueActor> _activeActors;

	// Token: 0x04000B8D RID: 2957
	[SerializeField]
	private string id;

	// Token: 0x04000B8E RID: 2958
	[SerializeField]
	private Sprite _portraitSprite;

	// Token: 0x04000B8F RID: 2959
	[SerializeField]
	[LocalizationKey("Default")]
	private string nameKey;

	// Token: 0x04000B90 RID: 2960
	[SerializeField]
	private Vector3 offset;
}
