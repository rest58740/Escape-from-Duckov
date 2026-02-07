using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000003 RID: 3
public class Demo : MonoBehaviour
{
	// Token: 0x06000005 RID: 5 RVA: 0x00002074 File Offset: 0x00000274
	private void Start()
	{
		this.animal_parent = GameObject.Find("Animals").transform;
		Transform transform = GameObject.Find("Canvas").transform;
		this.dropdownAnimal = transform.Find("Animal").Find("Dropdown").GetComponent<Dropdown>();
		this.dropdownAnimation = transform.Find("Animation").Find("Dropdown").GetComponent<Dropdown>();
		this.dropdownShapekey = transform.Find("Shapekey").Find("Dropdown").GetComponent<Dropdown>();
		int childCount = this.animal_parent.childCount;
		this.animals = new GameObject[childCount];
		List<string> list = new List<string>();
		for (int i = 0; i < childCount; i++)
		{
			this.animals[i] = this.animal_parent.GetChild(i).gameObject;
			string name = this.animal_parent.GetChild(i).name;
			list.Add(name);
			if (i == 0)
			{
				this.animals[i].SetActive(true);
			}
			else
			{
				this.animals[i].SetActive(false);
			}
		}
		this.dropdownAnimal.AddOptions(list);
		this.dropdownAnimation.AddOptions(this.animationList);
		this.dropdownShapekey.AddOptions(this.shapekeyList);
		this.dropdownShapekey.value = 1;
		this.ChangeShapekey();
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000021C8 File Offset: 0x000003C8
	private void Update()
	{
		if (Input.GetKeyDown("up"))
		{
			this.PrevAnimal();
			return;
		}
		if (Input.GetKeyDown("down"))
		{
			this.NextAnimal();
			return;
		}
		if (Input.GetKeyDown("right") && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
		{
			this.NextShapekey();
			return;
		}
		if (Input.GetKeyDown("left") && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
		{
			this.PrevShapekey();
			return;
		}
		if (Input.GetKeyDown("right"))
		{
			this.NextAnimation();
			return;
		}
		if (Input.GetKeyDown("left"))
		{
			this.PrevAnimation();
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002278 File Offset: 0x00000478
	public void NextAnimal()
	{
		if (this.dropdownAnimal.value >= this.dropdownAnimal.options.Count - 1)
		{
			this.dropdownAnimal.value = 0;
		}
		else
		{
			Dropdown dropdown = this.dropdownAnimal;
			int value = dropdown.value;
			dropdown.value = value + 1;
		}
		this.ChangeAnimal();
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000022D0 File Offset: 0x000004D0
	public void PrevAnimal()
	{
		if (this.dropdownAnimal.value <= 0)
		{
			this.dropdownAnimal.value = this.dropdownAnimal.options.Count - 1;
		}
		else
		{
			Dropdown dropdown = this.dropdownAnimal;
			int value = dropdown.value;
			dropdown.value = value - 1;
		}
		this.ChangeAnimal();
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002328 File Offset: 0x00000528
	public void ChangeAnimal()
	{
		this.animals[this.animalIndex].SetActive(false);
		this.animals[this.dropdownAnimal.value].SetActive(true);
		this.animalIndex = this.dropdownAnimal.value;
		this.ChangeAnimation();
		this.ChangeShapekey();
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002380 File Offset: 0x00000580
	public void NextAnimation()
	{
		if (this.dropdownAnimation.value >= this.dropdownAnimation.options.Count - 1)
		{
			this.dropdownAnimation.value = 0;
		}
		else
		{
			Dropdown dropdown = this.dropdownAnimation;
			int value = dropdown.value;
			dropdown.value = value + 1;
		}
		this.ChangeAnimation();
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000023D8 File Offset: 0x000005D8
	public void PrevAnimation()
	{
		if (this.dropdownAnimation.value <= 0)
		{
			this.dropdownAnimation.value = this.dropdownAnimation.options.Count - 1;
		}
		else
		{
			Dropdown dropdown = this.dropdownAnimation;
			int value = dropdown.value;
			dropdown.value = value - 1;
		}
		this.ChangeAnimation();
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002430 File Offset: 0x00000630
	public void ChangeAnimation()
	{
		Animator component = this.animals[this.dropdownAnimal.value].GetComponent<Animator>();
		if (component != null)
		{
			int value = this.dropdownAnimation.value;
			if (value == 15)
			{
				if (component.HasState(0, Animator.StringToHash("Spin")))
				{
					component.Play("Spin");
					return;
				}
				if (component.HasState(0, Animator.StringToHash("Splash")))
				{
					component.Play("Splash");
					return;
				}
			}
			else
			{
				component.Play(this.dropdownAnimation.options[value].text);
			}
		}
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000024CC File Offset: 0x000006CC
	public void NextShapekey()
	{
		if (this.dropdownShapekey.value >= this.dropdownShapekey.options.Count - 1)
		{
			this.dropdownShapekey.value = 0;
		}
		else
		{
			Dropdown dropdown = this.dropdownShapekey;
			int value = dropdown.value;
			dropdown.value = value + 1;
		}
		this.ChangeShapekey();
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002524 File Offset: 0x00000724
	public void PrevShapekey()
	{
		if (this.dropdownShapekey.value <= 0)
		{
			this.dropdownShapekey.value = this.dropdownShapekey.options.Count - 1;
		}
		else
		{
			Dropdown dropdown = this.dropdownShapekey;
			int value = dropdown.value;
			dropdown.value = value - 1;
		}
		this.ChangeShapekey();
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000257C File Offset: 0x0000077C
	public void ChangeShapekey()
	{
		Animator component = this.animals[this.dropdownAnimal.value].GetComponent<Animator>();
		if (component != null)
		{
			component.Play(this.dropdownShapekey.options[this.dropdownShapekey.value].text);
		}
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000025D0 File Offset: 0x000007D0
	public void GoToWebsite(string URL)
	{
		Application.OpenURL(URL);
	}

	// Token: 0x04000005 RID: 5
	private GameObject[] animals;

	// Token: 0x04000006 RID: 6
	private int animalIndex;

	// Token: 0x04000007 RID: 7
	private List<string> animationList = new List<string>
	{
		"Attack",
		"Bounce",
		"Clicked",
		"Death",
		"Eat",
		"Fear",
		"Fly",
		"Hit",
		"Idle_A",
		"Idle_B",
		"Idle_C",
		"Jump",
		"Roll",
		"Run",
		"Sit",
		"Spin/Splash",
		"Swim",
		"Walk"
	};

	// Token: 0x04000008 RID: 8
	private List<string> shapekeyList = new List<string>
	{
		"Eyes_Annoyed",
		"Eyes_Blink",
		"Eyes_Cry",
		"Eyes_Dead",
		"Eyes_Excited",
		"Eyes_Happy",
		"Eyes_LookDown",
		"Eyes_LookIn",
		"Eyes_LookOut",
		"Eyes_LookUp",
		"Eyes_Rabid",
		"Eyes_Sad",
		"Eyes_Shrink",
		"Eyes_Sleep",
		"Eyes_Spin",
		"Eyes_Squint",
		"Eyes_Trauma",
		"Sweat_L",
		"Sweat_R",
		"Teardrop_L",
		"Teardrop_R"
	};

	// Token: 0x04000009 RID: 9
	[Space(10f)]
	private Transform animal_parent;

	// Token: 0x0400000A RID: 10
	private Dropdown dropdownAnimal;

	// Token: 0x0400000B RID: 11
	private Dropdown dropdownAnimation;

	// Token: 0x0400000C RID: 12
	private Dropdown dropdownShapekey;
}
