using System;
using System.ComponentModel;

namespace Sirenix.Serialization
{
	// Token: 0x0200005C RID: 92
	[AttributeUsage(4)]
	[Obsolete("Use a RegisterFormatterAttribute applied to the containing assembly instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class CustomGenericFormatterAttribute : CustomFormatterAttribute
	{
		// Token: 0x06000332 RID: 818 RVA: 0x00016F60 File Offset: 0x00015160
		public CustomGenericFormatterAttribute(Type serializedGenericTypeDefinition, int priority = 0) : base(priority)
		{
			if (serializedGenericTypeDefinition == null)
			{
				throw new ArgumentNullException();
			}
			if (!serializedGenericTypeDefinition.IsGenericTypeDefinition)
			{
				throw new ArgumentException("The type " + serializedGenericTypeDefinition.Name + " is not a generic type definition.");
			}
			this.SerializedGenericTypeDefinition = serializedGenericTypeDefinition;
		}

		// Token: 0x04000103 RID: 259
		public readonly Type SerializedGenericTypeDefinition;
	}
}
