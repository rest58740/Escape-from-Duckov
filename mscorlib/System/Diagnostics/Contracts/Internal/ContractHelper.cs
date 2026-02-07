using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace System.Diagnostics.Contracts.Internal
{
	// Token: 0x020009D3 RID: 2515
	[Obsolete("Use the ContractHelper class in the System.Runtime.CompilerServices namespace instead.")]
	public static class ContractHelper
	{
		// Token: 0x06005A3A RID: 23098 RVA: 0x00134261 File Offset: 0x00132461
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DebuggerNonUserCode]
		public static string RaiseContractFailedEvent(ContractFailureKind failureKind, string userMessage, string conditionText, Exception innerException)
		{
			return ContractHelper.RaiseContractFailedEvent(failureKind, userMessage, conditionText, innerException);
		}

		// Token: 0x06005A3B RID: 23099 RVA: 0x0013426C File Offset: 0x0013246C
		[DebuggerNonUserCode]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void TriggerFailure(ContractFailureKind kind, string displayMessage, string userMessage, string conditionText, Exception innerException)
		{
			ContractHelper.TriggerFailure(kind, displayMessage, userMessage, conditionText, innerException);
		}
	}
}
