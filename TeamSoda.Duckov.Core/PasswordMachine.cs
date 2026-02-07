using System;
using System.Collections.Generic;
using Duckov;
using Duckov.Quests;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class PasswordMachine : MonoBehaviour
{
	// Token: 0x1700012D RID: 301
	// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0001A68C File Offset: 0x0001888C
	public int maxNum
	{
		get
		{
			return this.rightCode.Count;
		}
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0001A699 File Offset: 0x00018899
	private void Start()
	{
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0001A69B File Offset: 0x0001889B
	private void Update()
	{
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0001A6A0 File Offset: 0x000188A0
	private bool CheckConditions()
	{
		if (this.conditions.Count == 0)
		{
			return true;
		}
		for (int i = 0; i < this.conditions.Count; i++)
		{
			if (!(this.conditions[i] == null) && !this.conditions[i].Evaluate())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0001A6FC File Offset: 0x000188FC
	public void InputNum(int num)
	{
		if (this.nums.Count < this.maxNum)
		{
			this.nums.Add(num);
		}
		AudioManager.Post(string.Format("SFX/Special/Phone/phone_{0}", num), base.gameObject);
		AudioManager.Post("SFX/Special/Phone/phone_key_dial", base.gameObject);
		this.dialogueBubbleProxy.Pop(this.CurrentNums(), this.popSpeed);
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x0001A76C File Offset: 0x0001896C
	public void DeleteNum()
	{
		if (this.nums.Count > 0)
		{
			this.nums.RemoveAt(this.nums.Count - 1);
			AudioManager.Post("SFX/Special/Phone/phone_hash", base.gameObject);
			AudioManager.Post("SFX/Special/Phone/phone_key_dial", base.gameObject);
		}
		this.dialogueBubbleProxy.Pop(this.CurrentNums(), this.popSpeed);
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0001A7D8 File Offset: 0x000189D8
	public void Confirm()
	{
		if (this.rightCode.Count != this.nums.Count)
		{
			this.nums.Clear();
			this.dialogueBubbleProxy.Pop(LocalizationManager.ToPlainText(this.wrongPassWorldKey), this.popSpeed);
			AudioManager.Post("SFX/Special/Phone/phone_busy", base.gameObject);
			return;
		}
		for (int i = 0; i < this.rightCode.Count; i++)
		{
			if (this.rightCode[i] != this.nums[i])
			{
				this.nums.Clear();
				this.dialogueBubbleProxy.Pop(LocalizationManager.ToPlainText(this.wrongPassWorldKey), this.popSpeed);
				AudioManager.Post("SFX/Special/Phone/phone_busy", base.gameObject);
				return;
			}
		}
		if (!this.CheckConditions())
		{
			this.nums.Clear();
			this.dialogueBubbleProxy.Pop(LocalizationManager.ToPlainText(this.wrongTimeKey), this.popSpeed);
			AudioManager.Post("SFX/Special/Phone/phone_busy", base.gameObject);
			return;
		}
		this.nums.Clear();
		this.dialogueBubbleProxy.Pop(LocalizationManager.ToPlainText(this.rightKey), this.popSpeed);
		this.activeObject.SetActive(true);
		this.interactBoxCollider.enabled = false;
		AudioManager.Post("SFX/Special/Phone/phone_ringing", base.gameObject);
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0001A930 File Offset: 0x00018B30
	private string CurrentNums()
	{
		string text = "";
		for (int i = 0; i < this.nums.Count; i++)
		{
			text += this.nums[i].ToString();
		}
		for (int j = 0; j < this.maxNum - this.nums.Count; j++)
		{
			text += "_";
		}
		return text;
	}

	// Token: 0x0400056B RID: 1387
	public List<Condition> conditions;

	// Token: 0x0400056C RID: 1388
	[Tooltip("从下到上")]
	public List<int> rightCode;

	// Token: 0x0400056D RID: 1389
	private List<int> nums = new List<int>();

	// Token: 0x0400056E RID: 1390
	[LocalizationKey("Default")]
	[SerializeField]
	private string wrongTimeKey = "Passworld_WrongTime";

	// Token: 0x0400056F RID: 1391
	[LocalizationKey("Default")]
	[SerializeField]
	private string wrongPassWorldKey = "Passworld_WrongNumber";

	// Token: 0x04000570 RID: 1392
	[LocalizationKey("Default")]
	[SerializeField]
	private string rightKey = "Passworld_Right";

	// Token: 0x04000571 RID: 1393
	[SerializeField]
	private DialogueBubbleProxy dialogueBubbleProxy;

	// Token: 0x04000572 RID: 1394
	[SerializeField]
	private GameObject activeObject;

	// Token: 0x04000573 RID: 1395
	[SerializeField]
	private BoxCollider interactBoxCollider;

	// Token: 0x04000574 RID: 1396
	private float popSpeed = 30f;
}
