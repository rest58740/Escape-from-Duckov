using System;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection.Emit
{
	// Token: 0x02000921 RID: 2337
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_EventBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class EventBuilder : _EventBuilder
	{
		// Token: 0x06004FEB RID: 20459 RVA: 0x000479FC File Offset: 0x00045BFC
		void _EventBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x000479FC File Offset: 0x00045BFC
		void _EventBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x000479FC File Offset: 0x00045BFC
		void _EventBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x000479FC File Offset: 0x00045BFC
		void _EventBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x000FA636 File Offset: 0x000F8836
		internal EventBuilder(TypeBuilder tb, string eventName, EventAttributes eventAttrs, Type eventType)
		{
			this.name = eventName;
			this.attrs = eventAttrs;
			this.type = eventType;
			this.typeb = tb;
			this.table_idx = this.get_next_table_index(this, 20, 1);
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x000FA66B File Offset: 0x000F886B
		internal int get_next_table_index(object obj, int table, int count)
		{
			return this.typeb.get_next_table_index(obj, table, count);
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x000FA67C File Offset: 0x000F887C
		public void AddOtherMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.RejectIfCreated();
			if (this.other_methods != null)
			{
				MethodBuilder[] array = new MethodBuilder[this.other_methods.Length + 1];
				this.other_methods.CopyTo(array, 0);
				this.other_methods = array;
			}
			else
			{
				this.other_methods = new MethodBuilder[1];
			}
			this.other_methods[this.other_methods.Length - 1] = mdBuilder;
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x000FA6EF File Offset: 0x000F88EF
		public EventToken GetEventToken()
		{
			return new EventToken(335544320 | this.table_idx);
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x000FA702 File Offset: 0x000F8902
		public void SetAddOnMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.RejectIfCreated();
			this.add_method = mdBuilder;
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x000FA725 File Offset: 0x000F8925
		public void SetRaiseMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.RejectIfCreated();
			this.raise_method = mdBuilder;
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x000FA748 File Offset: 0x000F8948
		public void SetRemoveOnMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.RejectIfCreated();
			this.remove_method = mdBuilder;
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x000FA76C File Offset: 0x000F896C
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			this.RejectIfCreated();
			if (customBuilder.Ctor.ReflectedType.FullName == "System.Runtime.CompilerServices.SpecialNameAttribute")
			{
				this.attrs |= EventAttributes.SpecialName;
				return;
			}
			if (this.cattrs != null)
			{
				CustomAttributeBuilder[] array = new CustomAttributeBuilder[this.cattrs.Length + 1];
				this.cattrs.CopyTo(array, 0);
				array[this.cattrs.Length] = customBuilder;
				this.cattrs = array;
				return;
			}
			this.cattrs = new CustomAttributeBuilder[1];
			this.cattrs[0] = customBuilder;
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x000FA809 File Offset: 0x000F8A09
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			this.SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x000FA83A File Offset: 0x000F8A3A
		private void RejectIfCreated()
		{
			if (this.typeb.is_created)
			{
				throw new InvalidOperationException("Type definition of the method is complete.");
			}
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x000173AD File Offset: 0x000155AD
		internal EventBuilder()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0400314E RID: 12622
		internal string name;

		// Token: 0x0400314F RID: 12623
		private Type type;

		// Token: 0x04003150 RID: 12624
		private TypeBuilder typeb;

		// Token: 0x04003151 RID: 12625
		private CustomAttributeBuilder[] cattrs;

		// Token: 0x04003152 RID: 12626
		internal MethodBuilder add_method;

		// Token: 0x04003153 RID: 12627
		internal MethodBuilder remove_method;

		// Token: 0x04003154 RID: 12628
		internal MethodBuilder raise_method;

		// Token: 0x04003155 RID: 12629
		internal MethodBuilder[] other_methods;

		// Token: 0x04003156 RID: 12630
		internal EventAttributes attrs;

		// Token: 0x04003157 RID: 12631
		private int table_idx;
	}
}
