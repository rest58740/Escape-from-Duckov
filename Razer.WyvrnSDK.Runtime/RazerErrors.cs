using System;

namespace WyvrnSDK
{
	// Token: 0x02000003 RID: 3
	public class RazerErrors
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
		public static string GetResultString(int result)
		{
			if (result <= 1062)
			{
				if (result <= 5)
				{
					if (result <= -1)
					{
						if (result == -2147467259)
						{
							return "RZRESULT_FAILED";
						}
						if (result == -1)
						{
							return "RZRESULT_INVALID";
						}
					}
					else
					{
						if (result == 0)
						{
							return "RZRESULT_SUCCESS";
						}
						if (result == 5)
						{
							return "RZRESULT_ACCESS_DENIED";
						}
					}
				}
				else if (result <= 50)
				{
					if (result == 6)
					{
						return "RZRESULT_INVALID_HANDLE";
					}
					if (result == 50)
					{
						return "RZRESULT_NOT_SUPPORTED";
					}
				}
				else
				{
					if (result == 87)
					{
						return "RZRESULT_INVALID_PARAMETER";
					}
					if (result == 259)
					{
						return "RZRESULT_NO_MORE_ITEMS";
					}
					if (result == 1062)
					{
						return "RZRESULT_SERVICE_NOT_ACTIVE";
					}
				}
			}
			else if (result <= 1247)
			{
				if (result <= 1167)
				{
					if (result == 1152)
					{
						return "RZRESULT_SINGLE_INSTANCE_APP";
					}
					if (result == 1167)
					{
						return "RZRESULT_DEVICE_NOT_CONNECTED";
					}
				}
				else
				{
					if (result == 1168)
					{
						return "RZRESULT_NOT_FOUND";
					}
					if (result == 1235)
					{
						return "RZRESULT_REQUEST_ABORTED";
					}
					if (result == 1247)
					{
						return "RZRESULT_ALREADY_INITIALIZED";
					}
				}
			}
			else if (result <= 4319)
			{
				if (result == 4309)
				{
					return "RZRESULT_RESOURCE_DISABLED";
				}
				if (result == 4319)
				{
					return "RZRESULT_DEVICE_NOT_AVAILABLE";
				}
			}
			else
			{
				if (result == 5023)
				{
					return "RZRESULT_NOT_VALID_STATE";
				}
				if (result == 6023)
				{
					return "RZRESULT_DLL_NOT_FOUND";
				}
				if (result == 6033)
				{
					return "RZRESULT_DLL_INVALID_SIGNATURE";
				}
			}
			return result.ToString();
		}

		// Token: 0x04000001 RID: 1
		public const int RZRESULT_INVALID = -1;

		// Token: 0x04000002 RID: 2
		public const int RZRESULT_SUCCESS = 0;

		// Token: 0x04000003 RID: 3
		public const int RZRESULT_ACCESS_DENIED = 5;

		// Token: 0x04000004 RID: 4
		public const int RZRESULT_INVALID_HANDLE = 6;

		// Token: 0x04000005 RID: 5
		public const int RZRESULT_NOT_SUPPORTED = 50;

		// Token: 0x04000006 RID: 6
		public const int RZRESULT_INVALID_PARAMETER = 87;

		// Token: 0x04000007 RID: 7
		public const int RZRESULT_SERVICE_NOT_ACTIVE = 1062;

		// Token: 0x04000008 RID: 8
		public const int RZRESULT_SINGLE_INSTANCE_APP = 1152;

		// Token: 0x04000009 RID: 9
		public const int RZRESULT_DEVICE_NOT_CONNECTED = 1167;

		// Token: 0x0400000A RID: 10
		public const int RZRESULT_NOT_FOUND = 1168;

		// Token: 0x0400000B RID: 11
		public const int RZRESULT_REQUEST_ABORTED = 1235;

		// Token: 0x0400000C RID: 12
		public const int RZRESULT_ALREADY_INITIALIZED = 1247;

		// Token: 0x0400000D RID: 13
		public const int RZRESULT_RESOURCE_DISABLED = 4309;

		// Token: 0x0400000E RID: 14
		public const int RZRESULT_DEVICE_NOT_AVAILABLE = 4319;

		// Token: 0x0400000F RID: 15
		public const int RZRESULT_NOT_VALID_STATE = 5023;

		// Token: 0x04000010 RID: 16
		public const int RZRESULT_NO_MORE_ITEMS = 259;

		// Token: 0x04000011 RID: 17
		public const int RZRESULT_DLL_NOT_FOUND = 6023;

		// Token: 0x04000012 RID: 18
		public const int RZRESULT_DLL_INVALID_SIGNATURE = 6033;

		// Token: 0x04000013 RID: 19
		public const int RZRESULT_FAILED = -2147467259;
	}
}
