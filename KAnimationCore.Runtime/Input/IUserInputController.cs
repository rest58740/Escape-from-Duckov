using System;

namespace KINEMATION.KAnimationCore.Runtime.Input
{
	// Token: 0x02000012 RID: 18
	[Obsolete("use `UserInputController` instead.")]
	public interface IUserInputController
	{
		// Token: 0x0600001A RID: 26
		void Initialize();

		// Token: 0x0600001B RID: 27
		int GetPropertyIndex(string propertyName);

		// Token: 0x0600001C RID: 28
		void SetValue(string propertyName, object value);

		// Token: 0x0600001D RID: 29
		T GetValue<T>(string propertyName);

		// Token: 0x0600001E RID: 30
		void SetValue(int propertyIndex, object value);

		// Token: 0x0600001F RID: 31
		T GetValue<T>(int propertyIndex);
	}
}
