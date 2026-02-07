using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200073D RID: 1853
	[ComVisible(true)]
	public sealed class ExtensibleClassFactory
	{
		// Token: 0x0600411E RID: 16670 RVA: 0x0000259F File Offset: 0x0000079F
		private ExtensibleClassFactory()
		{
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x000E1D7F File Offset: 0x000DFF7F
		internal static ObjectCreationDelegate GetObjectCreationCallback(Type t)
		{
			return ExtensibleClassFactory.hashtable[t] as ObjectCreationDelegate;
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x000E1D94 File Offset: 0x000DFF94
		public static void RegisterObjectCreationCallback(ObjectCreationDelegate callback)
		{
			int i = 1;
			StackTrace stackTrace = new StackTrace(false);
			while (i < stackTrace.FrameCount)
			{
				MethodBase method = stackTrace.GetFrame(i).GetMethod();
				if (method.MemberType == MemberTypes.Constructor && method.IsStatic)
				{
					ExtensibleClassFactory.hashtable.Add(method.DeclaringType, callback);
					return;
				}
				i++;
			}
			throw new InvalidOperationException("RegisterObjectCreationCallback must be called from .cctor of class derived from ComImport type.");
		}

		// Token: 0x04002BBE RID: 11198
		private static readonly Hashtable hashtable = new Hashtable();
	}
}
