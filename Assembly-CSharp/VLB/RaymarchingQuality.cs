using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public class RaymarchingQuality
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000460E File Offset: 0x0000280E
		public int uniqueID
		{
			get
			{
				return this._UniqueID;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00004616 File Offset: 0x00002816
		public bool hasValidUniqueID
		{
			get
			{
				return this._UniqueID >= 0;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00004624 File Offset: 0x00002824
		public static RaymarchingQuality defaultInstance
		{
			get
			{
				return RaymarchingQuality.ms_DefaultInstance;
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000462B File Offset: 0x0000282B
		private RaymarchingQuality(int uniqueID)
		{
			this._UniqueID = uniqueID;
			this.name = "New quality";
			this.stepCount = 10;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000464D File Offset: 0x0000284D
		public static RaymarchingQuality New()
		{
			return new RaymarchingQuality(UnityEngine.Random.Range(4, int.MaxValue));
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000465F File Offset: 0x0000285F
		public static RaymarchingQuality New(string name, int forcedUniqueID, int stepCount)
		{
			return new RaymarchingQuality(forcedUniqueID)
			{
				name = name,
				stepCount = stepCount
			};
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004678 File Offset: 0x00002878
		private static bool HasRaymarchingQualityWithSameUniqueID(RaymarchingQuality[] values, int id)
		{
			foreach (RaymarchingQuality raymarchingQuality in values)
			{
				if (raymarchingQuality != null && raymarchingQuality.uniqueID == id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040000CB RID: 203
		public string name;

		// Token: 0x040000CC RID: 204
		public int stepCount;

		// Token: 0x040000CD RID: 205
		[SerializeField]
		private int _UniqueID;

		// Token: 0x040000CE RID: 206
		private static RaymarchingQuality ms_DefaultInstance = new RaymarchingQuality(-1);

		// Token: 0x040000CF RID: 207
		private const int kRandomUniqueIdMinRange = 4;
	}
}
