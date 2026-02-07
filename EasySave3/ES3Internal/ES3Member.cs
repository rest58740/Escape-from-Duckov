using System;

namespace ES3Internal
{
	// Token: 0x020000E2 RID: 226
	public class ES3Member
	{
		// Token: 0x060004D4 RID: 1236 RVA: 0x0001DEE1 File Offset: 0x0001C0E1
		public ES3Member(string name, Type type, bool isProperty)
		{
			this.name = name;
			this.type = type;
			this.isProperty = isProperty;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001DEFE File Offset: 0x0001C0FE
		public ES3Member(ES3Reflection.ES3ReflectedMember reflectedMember)
		{
			this.reflectedMember = reflectedMember;
			this.name = reflectedMember.Name;
			this.type = reflectedMember.MemberType;
			this.isProperty = reflectedMember.isProperty;
			this.useReflection = true;
		}

		// Token: 0x0400014A RID: 330
		public string name;

		// Token: 0x0400014B RID: 331
		public Type type;

		// Token: 0x0400014C RID: 332
		public bool isProperty;

		// Token: 0x0400014D RID: 333
		public ES3Reflection.ES3ReflectedMember reflectedMember;

		// Token: 0x0400014E RID: 334
		public bool useReflection;
	}
}
