using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x0200067E RID: 1662
	[ComVisible(true)]
	[SecurityCritical]
	public sealed class InternalST
	{
		// Token: 0x06003DD9 RID: 15833 RVA: 0x0000259F File Offset: 0x0000079F
		private InternalST()
		{
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[Conditional("_LOGGING")]
		public static void InfoSoap(params object[] messages)
		{
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x000D59C8 File Offset: 0x000D3BC8
		public static bool SoapCheckEnabled()
		{
			return BCLDebug.CheckEnabled("Soap");
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x000D59D4 File Offset: 0x000D3BD4
		[Conditional("SER_LOGGING")]
		public static void Soap(params object[] messages)
		{
			if (!(messages[0] is string))
			{
				messages[0] = messages[0].GetType().Name + " ";
				return;
			}
			int num = 0;
			object obj = messages[0];
			messages[num] = ((obj != null) ? obj.ToString() : null) + " ";
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[Conditional("_DEBUG")]
		public static void SoapAssert(bool condition, string message)
		{
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x000D5A22 File Offset: 0x000D3C22
		public static void SerializationSetValue(FieldInfo fi, object target, object value)
		{
			if (fi == null)
			{
				throw new ArgumentNullException("fi");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			FormatterServices.SerializationSetValue(fi, target, value);
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x000D5A5C File Offset: 0x000D3C5C
		public static Assembly LoadAssemblyFromString(string assemblyString)
		{
			return FormatterServices.LoadAssemblyFromString(assemblyString);
		}
	}
}
