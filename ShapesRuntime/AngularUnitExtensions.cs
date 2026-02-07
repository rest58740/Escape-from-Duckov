using System;

namespace Shapes
{
	// Token: 0x02000048 RID: 72
	public static class AngularUnitExtensions
	{
		// Token: 0x06000C64 RID: 3172 RVA: 0x00018A61 File Offset: 0x00016C61
		public static string Suffix(this AngularUnit unit)
		{
			return AngularUnitExtensions.angUnitToSuffix[(int)unit];
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00018A6A File Offset: 0x00016C6A
		public static string Name(this AngularUnit unit)
		{
			return AngularUnitExtensions.angUnitNames[(int)unit];
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00018A73 File Offset: 0x00016C73
		public static string NameShort(this AngularUnit unit)
		{
			return AngularUnitExtensions.angUnitNamesShort[(int)unit];
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00018A7C File Offset: 0x00016C7C
		public static float FromRadians(this AngularUnit unit)
		{
			return 1f / unit.ToRadians();
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00018A8A File Offset: 0x00016C8A
		public static float ToRadians(this AngularUnit unit)
		{
			switch (unit)
			{
			case AngularUnit.Radians:
				return 1f;
			case AngularUnit.Degrees:
				return 0.017453292f;
			case AngularUnit.Turns:
				return 6.2831855f;
			default:
				throw new ArgumentOutOfRangeException("unit", unit, null);
			}
		}

		// Token: 0x040001BC RID: 444
		public static string[] angUnitToSuffix = new string[]
		{
			"rad",
			"°",
			"tr"
		};

		// Token: 0x040001BD RID: 445
		public static string[] angUnitNames = new string[]
		{
			"Radians",
			"Degrees",
			"Turns"
		};

		// Token: 0x040001BE RID: 446
		public static string[] angUnitNamesShort = new string[]
		{
			"Rad",
			"Deg",
			"Turns"
		};
	}
}
