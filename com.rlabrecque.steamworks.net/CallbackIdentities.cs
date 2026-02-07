using System;

namespace Steamworks
{
	// Token: 0x0200018A RID: 394
	internal class CallbackIdentities
	{
		// Token: 0x06000900 RID: 2304 RVA: 0x0000D570 File Offset: 0x0000B770
		public static int GetCallbackIdentity(Type callbackStruct)
		{
			object[] customAttributes = callbackStruct.GetCustomAttributes(typeof(CallbackIdentityAttribute), false);
			int num = 0;
			if (num >= customAttributes.Length)
			{
				throw new Exception("Callback number not found for struct " + ((callbackStruct != null) ? callbackStruct.ToString() : null));
			}
			return ((CallbackIdentityAttribute)customAttributes[num]).Identity;
		}
	}
}
