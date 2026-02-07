using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using Unity;

namespace System.Reflection
{
	// Token: 0x020008B1 RID: 2225
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public abstract class Module : ICustomAttributeProvider, ISerializable, _Module
	{
		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06004974 RID: 18804 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual Assembly Assembly
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06004975 RID: 18805 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual string FullyQualifiedName
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06004976 RID: 18806 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual string Name
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06004977 RID: 18807 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual int MDStreamVersion
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06004978 RID: 18808 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual Guid ModuleVersionId
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06004979 RID: 18809 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual string ScopeName
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x0600497A RID: 18810 RVA: 0x000EEEBE File Offset: 0x000ED0BE
		public ModuleHandle ModuleHandle
		{
			get
			{
				return this.GetModuleHandleImpl();
			}
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x000EEEC6 File Offset: 0x000ED0C6
		internal virtual ModuleHandle GetModuleHandleImpl()
		{
			return ModuleHandle.EmptyHandle;
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool IsResource()
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool IsDefined(Type attributeType, bool inherit)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x0600497F RID: 18815 RVA: 0x000EEECD File Offset: 0x000ED0CD
		public virtual IEnumerable<CustomAttributeData> CustomAttributes
		{
			get
			{
				return this.GetCustomAttributesData();
			}
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual IList<CustomAttributeData> GetCustomAttributesData()
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual object[] GetCustomAttributes(bool inherit)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x000EEED5 File Offset: 0x000ED0D5
		public MethodInfo GetMethod(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.GetMethodImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, null, null);
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x000EEEF2 File Offset: 0x000ED0F2
		public MethodInfo GetMethod(string name, Type[] types)
		{
			return this.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, types, null);
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x000EEF04 File Offset: 0x000ED104
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x0004722A File Offset: 0x0004542A
		protected virtual MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x000EEF63 File Offset: 0x000ED163
		public MethodInfo[] GetMethods()
		{
			return this.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual MethodInfo[] GetMethods(BindingFlags bindingFlags)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x000EEF6D File Offset: 0x000ED16D
		public FieldInfo GetField(string name)
		{
			return this.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x000EEF78 File Offset: 0x000ED178
		public FieldInfo[] GetFields()
		{
			return this.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual FieldInfo[] GetFields(BindingFlags bindingFlags)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual Type[] GetTypes()
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x000EEF82 File Offset: 0x000ED182
		public virtual Type GetType(string className)
		{
			return this.GetType(className, false, false);
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x000EEF8D File Offset: 0x000ED18D
		public virtual Type GetType(string className, bool ignoreCase)
		{
			return this.GetType(className, false, ignoreCase);
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual Type GetType(string className, bool throwOnError, bool ignoreCase)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x000EEF98 File Offset: 0x000ED198
		public virtual Type[] FindTypes(TypeFilter filter, object filterCriteria)
		{
			Type[] types = this.GetTypes();
			int num = 0;
			for (int i = 0; i < types.Length; i++)
			{
				if (filter != null && !filter(types[i], filterCriteria))
				{
					types[i] = null;
				}
				else
				{
					num++;
				}
			}
			if (num == types.Length)
			{
				return types;
			}
			Type[] array = new Type[num];
			num = 0;
			for (int j = 0; j < types.Length; j++)
			{
				if (types[j] != null)
				{
					array[num++] = types[j];
				}
			}
			return array;
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06004992 RID: 18834 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual int MetadataToken
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x000EF010 File Offset: 0x000ED210
		public FieldInfo ResolveField(int metadataToken)
		{
			return this.ResolveField(metadataToken, null, null);
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x000EF01B File Offset: 0x000ED21B
		public MemberInfo ResolveMember(int metadataToken)
		{
			return this.ResolveMember(metadataToken, null, null);
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x000EF026 File Offset: 0x000ED226
		public MethodBase ResolveMethod(int metadataToken)
		{
			return this.ResolveMethod(metadataToken, null, null);
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual byte[] ResolveSignature(int metadataToken)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual string ResolveString(int metadataToken)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x000EF031 File Offset: 0x000ED231
		public Type ResolveType(int metadataToken)
		{
			return this.ResolveType(metadataToken, null, null);
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x0004722A File Offset: 0x0004542A
		[SecurityCritical]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x00097E36 File Offset: 0x00096036
		public override bool Equals(object o)
		{
			return base.Equals(o);
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x000930F4 File Offset: 0x000912F4
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x0006456C File Offset: 0x0006276C
		public static bool operator ==(Module left, Module right)
		{
			return left == right || (left != null && right != null && left.Equals(right));
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x000EF03C File Offset: 0x000ED23C
		public static bool operator !=(Module left, Module right)
		{
			return !(left == right);
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x000EF048 File Offset: 0x000ED248
		public override string ToString()
		{
			return this.ScopeName;
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x000EF050 File Offset: 0x000ED250
		private static bool FilterTypeNameImpl(Type cls, object filterCriteria)
		{
			if (filterCriteria == null || !(filterCriteria is string))
			{
				throw new InvalidFilterCriteriaException("A String must be provided for the filter criteria.");
			}
			string text = (string)filterCriteria;
			if (text.Length > 0 && text[text.Length - 1] == '*')
			{
				text = text.Substring(0, text.Length - 1);
				return cls.Name.StartsWith(text, StringComparison.Ordinal);
			}
			return cls.Name.Equals(text);
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x000EF0C0 File Offset: 0x000ED2C0
		private static bool FilterTypeNameIgnoreCaseImpl(Type cls, object filterCriteria)
		{
			if (filterCriteria == null || !(filterCriteria is string))
			{
				throw new InvalidFilterCriteriaException("A String must be provided for the filter criteria.");
			}
			string text = (string)filterCriteria;
			if (text.Length > 0 && text[text.Length - 1] == '*')
			{
				text = text.Substring(0, text.Length - 1);
				string name = cls.Name;
				return name.Length >= text.Length && string.Compare(name, 0, text, 0, text.Length, StringComparison.OrdinalIgnoreCase) == 0;
			}
			return string.Compare(text, cls.Name, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x060049A5 RID: 18853 RVA: 0x000EF150 File Offset: 0x000ED350
		internal Guid MvId
		{
			get
			{
				return this.GetModuleVersionId();
			}
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x000EF150 File Offset: 0x000ED350
		internal static Guid Mono_GetGuid(Module module)
		{
			return module.GetModuleVersionId();
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual Guid GetModuleVersionId()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual X509Certificate GetSignerCertificate()
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x000173AD File Offset: 0x000155AD
		void _Module.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x060049AB RID: 18859 RVA: 0x000173AD File Offset: 0x000155AD
		void _Module.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x060049AC RID: 18860 RVA: 0x000173AD File Offset: 0x000155AD
		void _Module.GetTypeInfoCount(out uint pcTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x060049AD RID: 18861 RVA: 0x000173AD File Offset: 0x000155AD
		void _Module.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04002EF0 RID: 12016
		public static readonly TypeFilter FilterTypeName = new TypeFilter(Module.FilterTypeNameImpl);

		// Token: 0x04002EF1 RID: 12017
		public static readonly TypeFilter FilterTypeNameIgnoreCase = new TypeFilter(Module.FilterTypeNameIgnoreCaseImpl);

		// Token: 0x04002EF2 RID: 12018
		private const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
	}
}
