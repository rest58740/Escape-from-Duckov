using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace MiniExcelLibs
{
	// Token: 0x0200001B RID: 27
	public class Property : Member
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00003DCC File Offset: 0x00001FCC
		public Property(PropertyInfo property)
		{
			this.Name = property.Name;
			this.Info = property;
			if (property.CanRead)
			{
				this.CanRead = true;
				this.m_geter = new MemberGetter(property);
			}
			if (property.CanWrite)
			{
				this.CanWrite = true;
				this.m_seter = new MemberSetter(property);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00003E28 File Offset: 0x00002028
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00003E30 File Offset: 0x00002030
		public bool CanRead { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003E39 File Offset: 0x00002039
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00003E41 File Offset: 0x00002041
		public bool CanWrite { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003E4A File Offset: 0x0000204A
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00003E52 File Offset: 0x00002052
		public PropertyInfo Info { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00003E5B File Offset: 0x0000205B
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00003E63 File Offset: 0x00002063
		public string Name { get; protected set; }

		// Token: 0x060000C4 RID: 196 RVA: 0x00003E6C File Offset: 0x0000206C
		public static Property[] GetProperties(Type type)
		{
			return Property.m_cached.GetOrAdd(type, (Type t) => (from p in t.GetProperties()
			select new Property(p)).ToArray<Property>());
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003E98 File Offset: 0x00002098
		public object GetValue(object instance)
		{
			if (this.m_geter == null)
			{
				throw new NotSupportedException();
			}
			return this.m_geter.Invoke(instance);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003EB4 File Offset: 0x000020B4
		public void SetValue(object instance, object value)
		{
			if (this.m_seter == null)
			{
				throw new NotSupportedException(this.Name + " can't set value");
			}
			this.m_seter.Invoke(instance, value);
		}

		// Token: 0x04000023 RID: 35
		private static readonly ConcurrentDictionary<Type, Property[]> m_cached = new ConcurrentDictionary<Type, Property[]>();

		// Token: 0x04000024 RID: 36
		private readonly MemberGetter m_geter;

		// Token: 0x04000025 RID: 37
		private readonly MemberSetter m_seter;
	}
}
