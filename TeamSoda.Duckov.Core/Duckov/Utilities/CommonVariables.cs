using System;
using Saves;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000413 RID: 1043
	public class CommonVariables : MonoBehaviour
	{
		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x060025C4 RID: 9668 RVA: 0x00083843 File Offset: 0x00081A43
		public CustomDataCollection Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x0008384C File Offset: 0x00081A4C
		private void Awake()
		{
			if (CommonVariables.instance == null)
			{
				CommonVariables.instance = this;
			}
			else
			{
				Debug.LogWarning("检测到多个Common Variables");
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			SavesSystem.OnSetFile += this.OnSetSaveFile;
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x0008389A File Offset: 0x00081A9A
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
			SavesSystem.OnSetFile -= this.OnSetSaveFile;
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x000838BE File Offset: 0x00081ABE
		private void OnSetSaveFile()
		{
			this.Load();
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000838C6 File Offset: 0x00081AC6
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000838CE File Offset: 0x00081ACE
		private void Start()
		{
			this.Load();
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x000838D6 File Offset: 0x00081AD6
		private void Save()
		{
			SavesSystem.Save<CustomDataCollection>("CommonVariables", "Data", this.data);
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x000838ED File Offset: 0x00081AED
		private void Load()
		{
			this.data = SavesSystem.Load<CustomDataCollection>("CommonVariables", "Data");
			if (this.data == null)
			{
				this.data = new CustomDataCollection();
			}
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x00083917 File Offset: 0x00081B17
		public static void SetFloat(string key, float value)
		{
			if (CommonVariables.instance)
			{
				CommonVariables.instance.Data.SetFloat(key, value, true);
			}
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x00083937 File Offset: 0x00081B37
		public static void SetInt(string key, int value)
		{
			if (CommonVariables.instance)
			{
				CommonVariables.instance.Data.SetInt(key, value, true);
			}
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x00083957 File Offset: 0x00081B57
		public static void SetBool(string key, bool value)
		{
			if (CommonVariables.instance)
			{
				CommonVariables.instance.Data.SetBool(key, value, true);
			}
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x00083977 File Offset: 0x00081B77
		public static void SetString(string key, string value)
		{
			if (CommonVariables.instance)
			{
				CommonVariables.instance.Data.SetString(key, value, true);
			}
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x00083997 File Offset: 0x00081B97
		public static float GetFloat(string key, float defaultValue = 0f)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetFloat(key, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000839B8 File Offset: 0x00081BB8
		public static int GetInt(string key, int defaultValue = 0)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetInt(key, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x000839D9 File Offset: 0x00081BD9
		public static bool GetBool(string key, bool defaultValue = false)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetBool(key, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x000839FA File Offset: 0x00081BFA
		public static string GetString(string key, string defaultValue = "")
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetString(key, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x00083A1B File Offset: 0x00081C1B
		public static float GetFloat(int hash, float defaultValue = 0f)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetFloat(hash, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x00083A3C File Offset: 0x00081C3C
		public static int GetInt(int hash, int defaultValue = 0)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetInt(hash, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x00083A5D File Offset: 0x00081C5D
		public static bool GetBool(int hash, bool defaultValue = false)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetBool(hash, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x00083A7E File Offset: 0x00081C7E
		public static string GetString(int hash, string defaultValue = "")
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetString(hash, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x040019BD RID: 6589
		private static CommonVariables instance;

		// Token: 0x040019BE RID: 6590
		[SerializeField]
		private CustomDataCollection data;

		// Token: 0x040019BF RID: 6591
		private const string saves_prefix = "CommonVariables";

		// Token: 0x040019C0 RID: 6592
		private const string saves_key = "Data";
	}
}
