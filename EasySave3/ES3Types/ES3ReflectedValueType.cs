using System;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200005C RID: 92
	[Preserve]
	internal class ES3ReflectedValueType : ES3Type
	{
		// Token: 0x060002CD RID: 717 RVA: 0x0000A64A File Offset: 0x0000884A
		public ES3ReflectedValueType(Type type) : base(type)
		{
			this.isReflectedType = true;
			base.GetMembers(true);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000A661 File Offset: 0x00008861
		public override void Write(object obj, ES3Writer writer)
		{
			base.WriteProperties(obj, writer);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000A66C File Offset: 0x0000886C
		public override object Read<T>(ES3Reader reader)
		{
			object obj = ES3Reflection.CreateInstance(this.type);
			if (obj == null)
			{
				string str = "Cannot create an instance of ";
				Type type = this.type;
				throw new NotSupportedException(str + ((type != null) ? type.ToString() : null) + ". However, you may be able to add support for it using a custom ES3Type file. For more information see: http://docs.moodkie.com/easy-save-3/es3-guides/controlling-serialization-using-es3types/");
			}
			return base.ReadProperties(reader, obj);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000A6B7 File Offset: 0x000088B7
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			throw new NotSupportedException("Cannot perform self-assigning load on a value type.");
		}
	}
}
