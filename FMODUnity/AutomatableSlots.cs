using System;
using UnityEngine.Serialization;

namespace FMODUnity
{
	// Token: 0x020000FE RID: 254
	[Serializable]
	public struct AutomatableSlots
	{
		// Token: 0x0600067B RID: 1659 RVA: 0x000072D4 File Offset: 0x000054D4
		public float GetValue(int index)
		{
			switch (index)
			{
			case 0:
				return this.Slot00;
			case 1:
				return this.Slot01;
			case 2:
				return this.Slot02;
			case 3:
				return this.Slot03;
			case 4:
				return this.Slot04;
			case 5:
				return this.Slot05;
			case 6:
				return this.Slot06;
			case 7:
				return this.Slot07;
			case 8:
				return this.Slot08;
			case 9:
				return this.Slot09;
			case 10:
				return this.Slot10;
			case 11:
				return this.Slot11;
			case 12:
				return this.Slot12;
			case 13:
				return this.Slot13;
			case 14:
				return this.Slot14;
			case 15:
				return this.Slot15;
			default:
				throw new ArgumentException(string.Format("Invalid slot index: {0}", index));
			}
		}

		// Token: 0x04000552 RID: 1362
		public const int Count = 16;

		// Token: 0x04000553 RID: 1363
		[FormerlySerializedAs("slot00")]
		public float Slot00;

		// Token: 0x04000554 RID: 1364
		[FormerlySerializedAs("slot01")]
		public float Slot01;

		// Token: 0x04000555 RID: 1365
		[FormerlySerializedAs("slot02")]
		public float Slot02;

		// Token: 0x04000556 RID: 1366
		[FormerlySerializedAs("slot03")]
		public float Slot03;

		// Token: 0x04000557 RID: 1367
		[FormerlySerializedAs("slot04")]
		public float Slot04;

		// Token: 0x04000558 RID: 1368
		[FormerlySerializedAs("slot05")]
		public float Slot05;

		// Token: 0x04000559 RID: 1369
		[FormerlySerializedAs("slot06")]
		public float Slot06;

		// Token: 0x0400055A RID: 1370
		[FormerlySerializedAs("slot07")]
		public float Slot07;

		// Token: 0x0400055B RID: 1371
		[FormerlySerializedAs("slot08")]
		public float Slot08;

		// Token: 0x0400055C RID: 1372
		[FormerlySerializedAs("slot09")]
		public float Slot09;

		// Token: 0x0400055D RID: 1373
		[FormerlySerializedAs("slot10")]
		public float Slot10;

		// Token: 0x0400055E RID: 1374
		[FormerlySerializedAs("slot11")]
		public float Slot11;

		// Token: 0x0400055F RID: 1375
		[FormerlySerializedAs("slot12")]
		public float Slot12;

		// Token: 0x04000560 RID: 1376
		[FormerlySerializedAs("slot13")]
		public float Slot13;

		// Token: 0x04000561 RID: 1377
		[FormerlySerializedAs("slot14")]
		public float Slot14;

		// Token: 0x04000562 RID: 1378
		[FormerlySerializedAs("slot15")]
		public float Slot15;
	}
}
