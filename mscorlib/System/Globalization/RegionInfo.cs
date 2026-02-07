using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Globalization
{
	// Token: 0x020009AD RID: 2477
	[ComVisible(true)]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class RegionInfo
	{
		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x06005973 RID: 22899 RVA: 0x00132C44 File Offset: 0x00130E44
		public static RegionInfo CurrentRegion
		{
			get
			{
				RegionInfo regionInfo = RegionInfo.currentRegion;
				if (regionInfo == null)
				{
					CultureInfo currentCulture = CultureInfo.CurrentCulture;
					if (currentCulture != null)
					{
						regionInfo = new RegionInfo(currentCulture);
					}
					if (Interlocked.CompareExchange<RegionInfo>(ref RegionInfo.currentRegion, regionInfo, null) != null)
					{
						regionInfo = RegionInfo.currentRegion;
					}
				}
				return regionInfo;
			}
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x00132C7F File Offset: 0x00130E7F
		public RegionInfo(int culture)
		{
			if (!this.GetByTerritory(CultureInfo.GetCultureInfo(culture)))
			{
				throw new ArgumentException(string.Format("Region ID {0} (0x{0:X4}) is not a supported region.", culture), "culture");
			}
		}

		// Token: 0x06005975 RID: 22901 RVA: 0x00132CB0 File Offset: 0x00130EB0
		public RegionInfo(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			if (this.construct_internal_region_from_name(name.ToUpperInvariant()))
			{
				return;
			}
			if (!this.GetByTerritory(CultureInfo.GetCultureInfo(name)))
			{
				throw new ArgumentException(string.Format("Region name {0} is not supported.", name), "name");
			}
		}

		// Token: 0x06005976 RID: 22902 RVA: 0x00132D00 File Offset: 0x00130F00
		private RegionInfo(CultureInfo ci)
		{
			if (ci.LCID == 127)
			{
				this.regionId = 244;
				this.iso2Name = "IV";
				this.iso3Name = "ivc";
				this.win3Name = "IVC";
				this.nativeName = (this.englishName = "Invariant Country");
				this.currencySymbol = "¤";
				this.isoCurrencySymbol = "XDR";
				this.currencyEnglishName = (this.currencyNativeName = "International Monetary Fund");
				return;
			}
			if (ci.Territory == null)
			{
				throw new NotImplementedException("Neutral region info");
			}
			this.construct_internal_region_from_name(ci.Territory.ToUpperInvariant());
		}

		// Token: 0x06005977 RID: 22903 RVA: 0x00132DAD File Offset: 0x00130FAD
		private bool GetByTerritory(CultureInfo ci)
		{
			if (ci == null)
			{
				throw new Exception("INTERNAL ERROR: should not happen.");
			}
			return !ci.IsNeutralCulture && ci.Territory != null && this.construct_internal_region_from_name(ci.Territory.ToUpperInvariant());
		}

		// Token: 0x06005978 RID: 22904
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool construct_internal_region_from_name(string name);

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x06005979 RID: 22905 RVA: 0x00132DE0 File Offset: 0x00130FE0
		[ComVisible(false)]
		public virtual string CurrencyEnglishName
		{
			get
			{
				return this.currencyEnglishName;
			}
		}

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x0600597A RID: 22906 RVA: 0x00132DE8 File Offset: 0x00130FE8
		public virtual string CurrencySymbol
		{
			get
			{
				return this.currencySymbol;
			}
		}

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x0600597B RID: 22907 RVA: 0x00132DF0 File Offset: 0x00130FF0
		[MonoTODO("DisplayName currently only returns the EnglishName")]
		public virtual string DisplayName
		{
			get
			{
				return this.englishName;
			}
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x0600597C RID: 22908 RVA: 0x00132DF0 File Offset: 0x00130FF0
		public virtual string EnglishName
		{
			get
			{
				return this.englishName;
			}
		}

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x0600597D RID: 22909 RVA: 0x00132DF8 File Offset: 0x00130FF8
		[ComVisible(false)]
		public virtual int GeoId
		{
			get
			{
				return this.regionId;
			}
		}

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x0600597E RID: 22910 RVA: 0x00132E00 File Offset: 0x00131000
		public virtual bool IsMetric
		{
			get
			{
				string a = this.iso2Name;
				return !(a == "US") && !(a == "UK");
			}
		}

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x0600597F RID: 22911 RVA: 0x00132E31 File Offset: 0x00131031
		public virtual string ISOCurrencySymbol
		{
			get
			{
				return this.isoCurrencySymbol;
			}
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x06005980 RID: 22912 RVA: 0x00132E39 File Offset: 0x00131039
		[ComVisible(false)]
		public virtual string NativeName
		{
			get
			{
				return this.nativeName;
			}
		}

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06005981 RID: 22913 RVA: 0x00132E41 File Offset: 0x00131041
		[ComVisible(false)]
		public virtual string CurrencyNativeName
		{
			get
			{
				return this.currencyNativeName;
			}
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06005982 RID: 22914 RVA: 0x00132E49 File Offset: 0x00131049
		public virtual string Name
		{
			get
			{
				return this.iso2Name;
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06005983 RID: 22915 RVA: 0x00132E51 File Offset: 0x00131051
		public virtual string ThreeLetterISORegionName
		{
			get
			{
				return this.iso3Name;
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x06005984 RID: 22916 RVA: 0x00132E59 File Offset: 0x00131059
		public virtual string ThreeLetterWindowsRegionName
		{
			get
			{
				return this.win3Name;
			}
		}

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x06005985 RID: 22917 RVA: 0x00132E49 File Offset: 0x00131049
		public virtual string TwoLetterISORegionName
		{
			get
			{
				return this.iso2Name;
			}
		}

		// Token: 0x06005986 RID: 22918 RVA: 0x00132E64 File Offset: 0x00131064
		public override bool Equals(object value)
		{
			RegionInfo regionInfo = value as RegionInfo;
			return regionInfo != null && this.Name == regionInfo.Name;
		}

		// Token: 0x06005987 RID: 22919 RVA: 0x00132E8E File Offset: 0x0013108E
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		// Token: 0x06005988 RID: 22920 RVA: 0x00132E9B File Offset: 0x0013109B
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06005989 RID: 22921 RVA: 0x00132EA3 File Offset: 0x001310A3
		internal static void ClearCachedData()
		{
			RegionInfo.currentRegion = null;
		}

		// Token: 0x04003761 RID: 14177
		private static RegionInfo currentRegion;

		// Token: 0x04003762 RID: 14178
		private int regionId;

		// Token: 0x04003763 RID: 14179
		private string iso2Name;

		// Token: 0x04003764 RID: 14180
		private string iso3Name;

		// Token: 0x04003765 RID: 14181
		private string win3Name;

		// Token: 0x04003766 RID: 14182
		private string englishName;

		// Token: 0x04003767 RID: 14183
		private string nativeName;

		// Token: 0x04003768 RID: 14184
		private string currencySymbol;

		// Token: 0x04003769 RID: 14185
		private string isoCurrencySymbol;

		// Token: 0x0400376A RID: 14186
		private string currencyEnglishName;

		// Token: 0x0400376B RID: 14187
		private string currencyNativeName;
	}
}
