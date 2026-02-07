using System;
using System.Reflection;
using System.Security;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000707 RID: 1799
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	[ComVisible(true)]
	public sealed class StructLayoutAttribute : Attribute
	{
		// Token: 0x060040A3 RID: 16547 RVA: 0x000E13FC File Offset: 0x000DF5FC
		[SecurityCritical]
		internal static StructLayoutAttribute GetCustomAttribute(RuntimeType type)
		{
			if (!StructLayoutAttribute.IsDefined(type))
			{
				return null;
			}
			int num = 0;
			int size = 0;
			LayoutKind layoutKind = LayoutKind.Auto;
			TypeAttributes typeAttributes = type.Attributes & TypeAttributes.LayoutMask;
			if (typeAttributes != TypeAttributes.NotPublic)
			{
				if (typeAttributes != TypeAttributes.SequentialLayout)
				{
					if (typeAttributes == TypeAttributes.ExplicitLayout)
					{
						layoutKind = LayoutKind.Explicit;
					}
				}
				else
				{
					layoutKind = LayoutKind.Sequential;
				}
			}
			else
			{
				layoutKind = LayoutKind.Auto;
			}
			CharSet charSet = CharSet.None;
			typeAttributes = (type.Attributes & TypeAttributes.StringFormatMask);
			if (typeAttributes != TypeAttributes.NotPublic)
			{
				if (typeAttributes != TypeAttributes.UnicodeClass)
				{
					if (typeAttributes == TypeAttributes.AutoClass)
					{
						charSet = CharSet.Auto;
					}
				}
				else
				{
					charSet = CharSet.Unicode;
				}
			}
			else
			{
				charSet = CharSet.Ansi;
			}
			type.GetPacking(out num, out size);
			if (num == 0)
			{
				num = 8;
			}
			return new StructLayoutAttribute(layoutKind, num, size, charSet);
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x000E1487 File Offset: 0x000DF687
		internal static bool IsDefined(RuntimeType type)
		{
			return !type.IsInterface && !type.HasElementType && !type.IsGenericParameter;
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x000E14A4 File Offset: 0x000DF6A4
		internal StructLayoutAttribute(LayoutKind layoutKind, int pack, int size, CharSet charSet)
		{
			this._val = layoutKind;
			this.Pack = pack;
			this.Size = size;
			this.CharSet = charSet;
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x000E14C9 File Offset: 0x000DF6C9
		public StructLayoutAttribute(LayoutKind layoutKind)
		{
			this._val = layoutKind;
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x000E14C9 File Offset: 0x000DF6C9
		public StructLayoutAttribute(short layoutKind)
		{
			this._val = (LayoutKind)layoutKind;
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x060040A8 RID: 16552 RVA: 0x000E14D8 File Offset: 0x000DF6D8
		public LayoutKind Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002ADA RID: 10970
		private const int DEFAULT_PACKING_SIZE = 8;

		// Token: 0x04002ADB RID: 10971
		internal LayoutKind _val;

		// Token: 0x04002ADC RID: 10972
		public int Pack;

		// Token: 0x04002ADD RID: 10973
		public int Size;

		// Token: 0x04002ADE RID: 10974
		public CharSet CharSet;
	}
}
