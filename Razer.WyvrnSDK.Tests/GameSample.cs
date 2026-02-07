using System;
using System.Collections;
using UnityEngine;
using WyvrnSDK;

// Token: 0x02000002 RID: 2
public class GameSample : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public IEnumerator Start()
	{
		if (!WyvrnAPI.IsWyvrnSDKAvailable())
		{
			this._mResult = 6023;
			yield break;
		}
		APPINFOTYPE appinfotype = default(APPINFOTYPE);
		appinfotype.Title = "Game Sample: Application";
		appinfotype.Description = "A Unity sample application using Razer Wyvrn SDK";
		appinfotype.Author_Name = "Razer";
		appinfotype.Author_Contact = "https://wyvrn.com";
		appinfotype.Category = 1U;
		this._mResult = WyvrnAPI.CoreInitSDK(ref appinfotype);
		int mResult = this._mResult;
		if (mResult != 0)
		{
			if (mResult != 6023)
			{
				if (mResult != 6033)
				{
					Debug.Log(string.Format("Failed to initialize Wyvrn! {0}", RazerErrors.GetResultString(this._mResult)));
				}
				else
				{
					Debug.Log(string.Format("Wyvrn DLL has an invalid signature! {0}", RazerErrors.GetResultString(this._mResult)));
				}
			}
			else
			{
				Debug.Log(string.Format("Wyvrn DLL is not found! {0}", RazerErrors.GetResultString(this._mResult)));
			}
		}
		else
		{
			this._mInitialized = true;
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}

	// Token: 0x06000002 RID: 2 RVA: 0x0000205F File Offset: 0x0000025F
	public void OnApplicationQuit()
	{
		if (this._mResult == 0)
		{
			bool flag = WyvrnAPI.CoreUnInit() != 0;
			WyvrnAPI.CoreUnInit();
			if (flag)
			{
				Debug.LogError("Failed to uninitialize Wyvrn!");
			}
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002080 File Offset: 0x00000280
	private string GetEffectName(int index)
	{
		return string.Format("Effect{0}", index);
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002094 File Offset: 0x00000294
	private void ExecuteItem(int index)
	{
		switch (index)
		{
		case 1:
			this.ShowEffect1();
			return;
		case 2:
			this.ShowEffect2();
			return;
		case 3:
			this.ShowEffect3();
			return;
		case 4:
			this.ShowEffect4();
			return;
		case 5:
			this.ShowEffect5();
			return;
		case 6:
			this.ShowEffect6();
			return;
		case 7:
			this.ShowEffect7();
			return;
		case 8:
			this.ShowEffect8();
			return;
		case 9:
			this.ShowEffect9();
			return;
		case 10:
			this.ShowEffect10();
			return;
		case 11:
			this.ShowEffect11();
			return;
		case 12:
			this.ShowEffect12();
			return;
		case 13:
			this.ShowEffect13();
			return;
		case 14:
			this.ShowEffect14();
			return;
		case 15:
			this.ShowEffect15();
			return;
		default:
			return;
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002150 File Offset: 0x00000350
	public void OnGUI()
	{
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.Width((float)Screen.width)
		});
		GUILayout.FlexibleSpace();
		if (!this._mInitialized)
		{
			GUILayout.BeginVertical(new GUILayoutOption[]
			{
				GUILayout.Height((float)Screen.height)
			});
			GUILayout.FlexibleSpace();
			GUILayout.Label("Sample has not yet initialized!", Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			return;
		}
		int mResult = this._mResult;
		if (mResult != 0)
		{
			if (mResult != 6023)
			{
				if (mResult != 6033)
				{
					GUILayout.BeginVertical(new GUILayoutOption[]
					{
						GUILayout.Height((float)Screen.height)
					});
					GUILayout.FlexibleSpace();
					GUILayout.Label(string.Format("Failed to initialize Wyvrn! {0}", RazerErrors.GetResultString(this._mResult)), Array.Empty<GUILayoutOption>());
					GUILayout.FlexibleSpace();
					GUILayout.EndVertical();
				}
				else
				{
					GUILayout.BeginVertical(new GUILayoutOption[]
					{
						GUILayout.Height((float)Screen.height)
					});
					GUILayout.FlexibleSpace();
					GUILayout.Label("Wyvrn DLL has an invalid signature!", Array.Empty<GUILayoutOption>());
					GUILayout.FlexibleSpace();
					GUILayout.EndVertical();
				}
			}
			else
			{
				GUILayout.BeginVertical(new GUILayoutOption[]
				{
					GUILayout.Height((float)Screen.height)
				});
				GUILayout.FlexibleSpace();
				GUILayout.Label("Wyvrn DLL is not found!", Array.Empty<GUILayoutOption>());
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
			}
		}
		else
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width((float)Screen.width),
				GUILayout.Height((float)Screen.height)
			});
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width((float)Screen.width)
			});
			GUILayout.FlexibleSpace();
			GUILayout.Label("UNITY GAME WYVRN SAMPLE APP", Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width((float)Screen.width)
			});
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			for (int i = 0; i <= 7; i++)
			{
				if (GUILayout.Button(this.GetEffectName(i + 1), new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				}))
				{
					this.ExecuteItem(i + 1);
				}
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			for (int j = 8; j < 15; j++)
			{
				if (GUILayout.Button(this.GetEffectName(j + 1), new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				}))
				{
					this.ExecuteItem(j + 1);
				}
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002408 File Offset: 0x00000608
	private void ShowEffect1()
	{
		WyvrnAPI.CoreSetEventName("Effect1");
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002415 File Offset: 0x00000615
	private void ShowEffect1ChromaLink()
	{
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002417 File Offset: 0x00000617
	private void ShowEffect2()
	{
		WyvrnAPI.CoreSetEventName("Effect2");
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002424 File Offset: 0x00000624
	private void ShowEffect3()
	{
		WyvrnAPI.CoreSetEventName("Effect3");
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002431 File Offset: 0x00000631
	private void ShowEffect4()
	{
		WyvrnAPI.CoreSetEventName("Effect4");
	}

	// Token: 0x0600000B RID: 11 RVA: 0x0000243E File Offset: 0x0000063E
	private void ShowEffect5()
	{
		WyvrnAPI.CoreSetEventName("Effect5");
	}

	// Token: 0x0600000C RID: 12 RVA: 0x0000244B File Offset: 0x0000064B
	private void ShowEffect6()
	{
		WyvrnAPI.CoreSetEventName("Effect6");
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002458 File Offset: 0x00000658
	private void ShowEffect7()
	{
		WyvrnAPI.CoreSetEventName("Effect7");
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002465 File Offset: 0x00000665
	private void ShowEffect8()
	{
		WyvrnAPI.CoreSetEventName("Effect8");
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002472 File Offset: 0x00000672
	private void ShowEffect9()
	{
		WyvrnAPI.CoreSetEventName("Effect9");
	}

	// Token: 0x06000010 RID: 16 RVA: 0x0000247F File Offset: 0x0000067F
	private void ShowEffect10()
	{
		WyvrnAPI.CoreSetEventName("Effect10");
	}

	// Token: 0x06000011 RID: 17 RVA: 0x0000248C File Offset: 0x0000068C
	private void ShowEffect11()
	{
		WyvrnAPI.CoreSetEventName("Effect11");
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002499 File Offset: 0x00000699
	private void ShowEffect12()
	{
		WyvrnAPI.CoreSetEventName("Effect12");
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000024A6 File Offset: 0x000006A6
	private void ShowEffect13()
	{
		WyvrnAPI.CoreSetEventName("Effect13");
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000024B3 File Offset: 0x000006B3
	private void ShowEffect14()
	{
		WyvrnAPI.CoreSetEventName("Effect14");
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000024C0 File Offset: 0x000006C0
	private void ShowEffect15()
	{
		WyvrnAPI.CoreSetEventName("Effect15");
	}

	// Token: 0x04000001 RID: 1
	private const int MAX_EFFECTS = 15;

	// Token: 0x04000002 RID: 2
	private bool _mInitialized;

	// Token: 0x04000003 RID: 3
	private int _mResult;
}
