using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000242 RID: 578
	[CreateAssetMenu(menuName = "Settings/MetaData")]
	public class GameMetaData : ScriptableObject
	{
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06001248 RID: 4680 RVA: 0x000470CC File Offset: 0x000452CC
		public VersionData Version
		{
			get
			{
				if (GameMetaData.Instance == null)
				{
					return default(VersionData);
				}
				return GameMetaData.Instance.versionData.versionData;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06001249 RID: 4681 RVA: 0x000470FF File Offset: 0x000452FF
		public bool IsDemo
		{
			get
			{
				return this.isDemo;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600124A RID: 4682 RVA: 0x00047107 File Offset: 0x00045307
		public bool IsTestVersion
		{
			get
			{
				return this.isTestVersion;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x0600124B RID: 4683 RVA: 0x00047114 File Offset: 0x00045314
		public static GameMetaData Instance
		{
			get
			{
				if (GameMetaData._instance == null)
				{
					GameMetaData._instance = Resources.Load<GameMetaData>("MetaData");
				}
				return GameMetaData._instance;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x0600124C RID: 4684 RVA: 0x00047137 File Offset: 0x00045337
		public static bool BloodFxOn
		{
			get
			{
				return GameMetaData.Instance.bloodFxOn;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x0600124D RID: 4685 RVA: 0x00047143 File Offset: 0x00045343
		// (set) Token: 0x0600124E RID: 4686 RVA: 0x0004714B File Offset: 0x0004534B
		public Platform Platform
		{
			get
			{
				return this.platform;
			}
			set
			{
				this.platform = value;
			}
		}

		// Token: 0x04000E15 RID: 3605
		[SerializeField]
		private GameVersionData versionData;

		// Token: 0x04000E16 RID: 3606
		[SerializeField]
		private bool isTestVersion;

		// Token: 0x04000E17 RID: 3607
		[SerializeField]
		private bool isDemo;

		// Token: 0x04000E18 RID: 3608
		[SerializeField]
		private Platform platform;

		// Token: 0x04000E19 RID: 3609
		[SerializeField]
		private bool bloodFxOn = true;

		// Token: 0x04000E1A RID: 3610
		private static GameMetaData _instance;
	}
}
