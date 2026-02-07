using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000247 RID: 583
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[ComVisible(true)]
	[Serializable]
	public class Object
	{
		// Token: 0x06001AE0 RID: 6880 RVA: 0x0002842A File Offset: 0x0002662A
		public virtual bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x0006456C File Offset: 0x0006276C
		public static bool Equals(object objA, object objB)
		{
			return objA == objB || (objA != null && objB != null && objA.Equals(objB));
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public Object()
		{
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected virtual void Finalize()
		{
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x00064583 File Offset: 0x00062783
		public virtual int GetHashCode()
		{
			return object.InternalGetHashCode(this);
		}

		// Token: 0x06001AE5 RID: 6885
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Type GetType();

		// Token: 0x06001AE6 RID: 6886
		[MethodImpl(MethodImplOptions.InternalCall)]
		protected extern object MemberwiseClone();

		// Token: 0x06001AE7 RID: 6887 RVA: 0x0006458B File Offset: 0x0006278B
		public virtual string ToString()
		{
			return this.GetType().ToString();
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x0002842A File Offset: 0x0002662A
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool ReferenceEquals(object objA, object objB)
		{
			return objA == objB;
		}

		// Token: 0x06001AE9 RID: 6889
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int InternalGetHashCode(object o);

		// Token: 0x06001AEA RID: 6890 RVA: 0x00004BF9 File Offset: 0x00002DF9
		private void FieldGetter(string typeName, string fieldName, ref object val)
		{
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x00004BF9 File Offset: 0x00002DF9
		private void FieldSetter(string typeName, string fieldName, object val)
		{
		}
	}
}
