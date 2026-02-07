using System;

namespace NodeCanvas.Framework
{
	// Token: 0x02000012 RID: 18
	public interface IInvokable : IGraphElement
	{
		// Token: 0x0600013D RID: 317
		string GetInvocationID();

		// Token: 0x0600013E RID: 318
		object Invoke(params object[] args);

		// Token: 0x0600013F RID: 319
		void InvokeAsync(Action<object> callback, params object[] args);
	}
}
