using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security;

namespace System.Collections
{
	// Token: 0x02000A0F RID: 2575
	[Serializable]
	public sealed class Comparer : IComparer, ISerializable
	{
		// Token: 0x06005B6E RID: 23406 RVA: 0x00134A76 File Offset: 0x00132C76
		public Comparer(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			this._compareInfo = culture.CompareInfo;
		}

		// Token: 0x06005B6F RID: 23407 RVA: 0x00134A98 File Offset: 0x00132C98
		private Comparer(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this._compareInfo = (CompareInfo)info.GetValue("CompareInfo", typeof(CompareInfo));
		}

		// Token: 0x06005B70 RID: 23408 RVA: 0x00134ACE File Offset: 0x00132CCE
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("CompareInfo", this._compareInfo);
		}

		// Token: 0x06005B71 RID: 23409 RVA: 0x00134AF0 File Offset: 0x00132CF0
		public int Compare(object a, object b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return this._compareInfo.Compare(text, text2);
			}
			IComparable comparable = a as IComparable;
			if (comparable != null)
			{
				return comparable.CompareTo(b);
			}
			IComparable comparable2 = b as IComparable;
			if (comparable2 != null)
			{
				return -comparable2.CompareTo(a);
			}
			throw new ArgumentException("At least one object must implement IComparable.");
		}

		// Token: 0x04003864 RID: 14436
		private CompareInfo _compareInfo;

		// Token: 0x04003865 RID: 14437
		public static readonly Comparer Default = new Comparer(CultureInfo.CurrentCulture);

		// Token: 0x04003866 RID: 14438
		public static readonly Comparer DefaultInvariant = new Comparer(CultureInfo.InvariantCulture);
	}
}
