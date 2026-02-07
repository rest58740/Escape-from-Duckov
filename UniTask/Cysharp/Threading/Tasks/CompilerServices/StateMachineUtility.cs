using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cysharp.Threading.Tasks.CompilerServices
{
	// Token: 0x02000129 RID: 297
	internal static class StateMachineUtility
	{
		// Token: 0x060006C6 RID: 1734 RVA: 0x0000FCD2 File Offset: 0x0000DED2
		public static int GetState(IAsyncStateMachine stateMachine)
		{
			return (int)stateMachine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).First((FieldInfo x) => x.Name.EndsWith("__state")).GetValue(stateMachine);
		}
	}
}
