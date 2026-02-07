using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200091C RID: 2332
	[ComVisible(true)]
	public class DynamicILInfo
	{
		// Token: 0x06004F63 RID: 20323 RVA: 0x0000259F File Offset: 0x0000079F
		internal DynamicILInfo()
		{
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x000F9A6A File Offset: 0x000F7C6A
		internal DynamicILInfo(DynamicMethod method)
		{
			this.method = method;
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06004F65 RID: 20325 RVA: 0x000F9A79 File Offset: 0x000F7C79
		public DynamicMethod DynamicMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public int GetTokenFor(byte[] signature)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x000F9A81 File Offset: 0x000F7C81
		public int GetTokenFor(DynamicMethod method)
		{
			return this.method.GetILGenerator().TokenGenerator.GetToken(method, false);
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x000F9A9A File Offset: 0x000F7C9A
		public int GetTokenFor(RuntimeFieldHandle field)
		{
			return this.method.GetILGenerator().TokenGenerator.GetToken(FieldInfo.GetFieldFromHandle(field), false);
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x000F9AB8 File Offset: 0x000F7CB8
		public int GetTokenFor(RuntimeMethodHandle method)
		{
			MethodBase methodFromHandle = MethodBase.GetMethodFromHandle(method);
			return this.method.GetILGenerator().TokenGenerator.GetToken(methodFromHandle, false);
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x000F9AE4 File Offset: 0x000F7CE4
		public int GetTokenFor(RuntimeTypeHandle type)
		{
			Type typeFromHandle = Type.GetTypeFromHandle(type);
			return this.method.GetILGenerator().TokenGenerator.GetToken(typeFromHandle, false);
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x000F9B0F File Offset: 0x000F7D0F
		public int GetTokenFor(string literal)
		{
			return this.method.GetILGenerator().TokenGenerator.GetToken(literal);
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public int GetTokenFor(RuntimeMethodHandle method, RuntimeTypeHandle contextType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public int GetTokenFor(RuntimeFieldHandle field, RuntimeTypeHandle contextType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x000F9B27 File Offset: 0x000F7D27
		public void SetCode(byte[] code, int maxStackSize)
		{
			if (code == null)
			{
				throw new ArgumentNullException("code");
			}
			this.method.GetILGenerator().SetCode(code, maxStackSize);
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x000F9B49 File Offset: 0x000F7D49
		[CLSCompliant(false)]
		public unsafe void SetCode(byte* code, int codeSize, int maxStackSize)
		{
			if (code == null)
			{
				throw new ArgumentNullException("code");
			}
			this.method.GetILGenerator().SetCode(code, codeSize, maxStackSize);
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public void SetExceptions(byte[] exceptions)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x000479FC File Offset: 0x00045BFC
		[CLSCompliant(false)]
		[MonoTODO]
		public unsafe void SetExceptions(byte* exceptions, int exceptionsSize)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public void SetLocalSignature(byte[] localSignature)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x000F9B70 File Offset: 0x000F7D70
		[CLSCompliant(false)]
		public unsafe void SetLocalSignature(byte* localSignature, int signatureSize)
		{
			byte[] array = new byte[signatureSize];
			for (int i = 0; i < signatureSize; i++)
			{
				array[i] = localSignature[i];
			}
		}

		// Token: 0x04003135 RID: 12597
		private DynamicMethod method;
	}
}
