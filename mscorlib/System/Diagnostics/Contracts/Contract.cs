using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace System.Diagnostics.Contracts
{
	// Token: 0x020009CF RID: 2511
	public static class Contract
	{
		// Token: 0x06005A0F RID: 23055 RVA: 0x00133DCD File Offset: 0x00131FCD
		[Conditional("DEBUG")]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[Conditional("CONTRACTS_FULL")]
		public static void Assume(bool condition)
		{
			if (!condition)
			{
				Contract.ReportFailure(ContractFailureKind.Assume, null, null, null);
			}
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x00133DDB File Offset: 0x00131FDB
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[Conditional("CONTRACTS_FULL")]
		[Conditional("DEBUG")]
		public static void Assume(bool condition, string userMessage)
		{
			if (!condition)
			{
				Contract.ReportFailure(ContractFailureKind.Assume, userMessage, null, null);
			}
		}

		// Token: 0x06005A11 RID: 23057 RVA: 0x00133DE9 File Offset: 0x00131FE9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[Conditional("CONTRACTS_FULL")]
		[Conditional("DEBUG")]
		public static void Assert(bool condition)
		{
			if (!condition)
			{
				Contract.ReportFailure(ContractFailureKind.Assert, null, null, null);
			}
		}

		// Token: 0x06005A12 RID: 23058 RVA: 0x00133DF7 File Offset: 0x00131FF7
		[Conditional("CONTRACTS_FULL")]
		[Conditional("DEBUG")]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void Assert(bool condition, string userMessage)
		{
			if (!condition)
			{
				Contract.ReportFailure(ContractFailureKind.Assert, userMessage, null, null);
			}
		}

		// Token: 0x06005A13 RID: 23059 RVA: 0x00133E05 File Offset: 0x00132005
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[Conditional("CONTRACTS_FULL")]
		public static void Requires(bool condition)
		{
			Contract.AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires");
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x00133E05 File Offset: 0x00132005
		[Conditional("CONTRACTS_FULL")]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void Requires(bool condition, string userMessage)
		{
			Contract.AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires");
		}

		// Token: 0x06005A15 RID: 23061 RVA: 0x00133E12 File Offset: 0x00132012
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void Requires<TException>(bool condition) where TException : Exception
		{
			Contract.AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires<TException>");
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x00133E12 File Offset: 0x00132012
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void Requires<TException>(bool condition, string userMessage) where TException : Exception
		{
			Contract.AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires<TException>");
		}

		// Token: 0x06005A17 RID: 23063 RVA: 0x00133E1F File Offset: 0x0013201F
		[Conditional("CONTRACTS_FULL")]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void Ensures(bool condition)
		{
			Contract.AssertMustUseRewriter(ContractFailureKind.Postcondition, "Ensures");
		}

		// Token: 0x06005A18 RID: 23064 RVA: 0x00133E1F File Offset: 0x0013201F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[Conditional("CONTRACTS_FULL")]
		public static void Ensures(bool condition, string userMessage)
		{
			Contract.AssertMustUseRewriter(ContractFailureKind.Postcondition, "Ensures");
		}

		// Token: 0x06005A19 RID: 23065 RVA: 0x00133E2C File Offset: 0x0013202C
		[Conditional("CONTRACTS_FULL")]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void EnsuresOnThrow<TException>(bool condition) where TException : Exception
		{
			Contract.AssertMustUseRewriter(ContractFailureKind.PostconditionOnException, "EnsuresOnThrow");
		}

		// Token: 0x06005A1A RID: 23066 RVA: 0x00133E2C File Offset: 0x0013202C
		[Conditional("CONTRACTS_FULL")]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void EnsuresOnThrow<TException>(bool condition, string userMessage) where TException : Exception
		{
			Contract.AssertMustUseRewriter(ContractFailureKind.PostconditionOnException, "EnsuresOnThrow");
		}

		// Token: 0x06005A1B RID: 23067 RVA: 0x00133E3C File Offset: 0x0013203C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static T Result<T>()
		{
			return default(T);
		}

		// Token: 0x06005A1C RID: 23068 RVA: 0x00133E52 File Offset: 0x00132052
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static T ValueAtReturn<T>(out T value)
		{
			value = default(T);
			return value;
		}

		// Token: 0x06005A1D RID: 23069 RVA: 0x00133E64 File Offset: 0x00132064
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static T OldValue<T>(T value)
		{
			return default(T);
		}

		// Token: 0x06005A1E RID: 23070 RVA: 0x00133E7A File Offset: 0x0013207A
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[Conditional("CONTRACTS_FULL")]
		public static void Invariant(bool condition)
		{
			Contract.AssertMustUseRewriter(ContractFailureKind.Invariant, "Invariant");
		}

		// Token: 0x06005A1F RID: 23071 RVA: 0x00133E7A File Offset: 0x0013207A
		[Conditional("CONTRACTS_FULL")]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void Invariant(bool condition, string userMessage)
		{
			Contract.AssertMustUseRewriter(ContractFailureKind.Invariant, "Invariant");
		}

		// Token: 0x06005A20 RID: 23072 RVA: 0x00133E88 File Offset: 0x00132088
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static bool ForAll(int fromInclusive, int toExclusive, Predicate<int> predicate)
		{
			if (fromInclusive > toExclusive)
			{
				throw new ArgumentException("fromInclusive must be less than or equal to toExclusive.");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			for (int i = fromInclusive; i < toExclusive; i++)
			{
				if (!predicate(i))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005A21 RID: 23073 RVA: 0x00133ECC File Offset: 0x001320CC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static bool ForAll<T>(IEnumerable<T> collection, Predicate<T> predicate)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			foreach (T obj in collection)
			{
				if (!predicate(obj))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005A22 RID: 23074 RVA: 0x00133F3C File Offset: 0x0013213C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static bool Exists(int fromInclusive, int toExclusive, Predicate<int> predicate)
		{
			if (fromInclusive > toExclusive)
			{
				throw new ArgumentException("fromInclusive must be less than or equal to toExclusive.");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			for (int i = fromInclusive; i < toExclusive; i++)
			{
				if (predicate(i))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005A23 RID: 23075 RVA: 0x00133F80 File Offset: 0x00132180
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static bool Exists<T>(IEnumerable<T> collection, Predicate<T> predicate)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			foreach (T obj in collection)
			{
				if (predicate(obj))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[Conditional("CONTRACTS_FULL")]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void EndContractBlock()
		{
		}

		// Token: 0x06005A25 RID: 23077 RVA: 0x00133FF0 File Offset: 0x001321F0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DebuggerNonUserCode]
		private static void ReportFailure(ContractFailureKind failureKind, string userMessage, string conditionText, Exception innerException)
		{
			if (failureKind < ContractFailureKind.Precondition || failureKind > ContractFailureKind.Assume)
			{
				throw new ArgumentException(Environment.GetResourceString("Illegal enum value: {0}.", new object[]
				{
					failureKind
				}), "failureKind");
			}
			string text = ContractHelper.RaiseContractFailedEvent(failureKind, userMessage, conditionText, innerException);
			if (text == null)
			{
				return;
			}
			ContractHelper.TriggerFailure(failureKind, text, userMessage, conditionText, innerException);
		}

		// Token: 0x06005A26 RID: 23078 RVA: 0x00134044 File Offset: 0x00132244
		[SecuritySafeCritical]
		private static void AssertMustUseRewriter(ContractFailureKind kind, string contractKind)
		{
			if (Contract._assertingMustUseRewriter)
			{
				System.Diagnostics.Assert.Fail("Asserting that we must use the rewriter went reentrant.", "Didn't rewrite this mscorlib?");
			}
			Contract._assertingMustUseRewriter = true;
			Assembly assembly = typeof(Contract).Assembly;
			StackTrace stackTrace = new StackTrace();
			Assembly assembly2 = null;
			for (int i = 0; i < stackTrace.FrameCount; i++)
			{
				Assembly assembly3 = stackTrace.GetFrame(i).GetMethod().DeclaringType.Assembly;
				if (assembly3 != assembly)
				{
					assembly2 = assembly3;
					break;
				}
			}
			if (assembly2 == null)
			{
				assembly2 = assembly;
			}
			string name = assembly2.GetName().Name;
			ContractHelper.TriggerFailure(kind, Environment.GetResourceString("An assembly (probably \"{1}\") must be rewritten using the code contracts binary rewriter (CCRewrite) because it is calling Contract.{0} and the CONTRACTS_FULL symbol is defined.  Remove any explicit definitions of the CONTRACTS_FULL symbol from your project and rebuild.  CCRewrite can be downloaded from http://go.microsoft.com/fwlink/?LinkID=169180. \\r\\nAfter the rewriter is installed, it can be enabled in Visual Studio from the project's Properties page on the Code Contracts pane.  Ensure that \"Perform Runtime Contract Checking\" is enabled, which will define CONTRACTS_FULL.", new object[]
			{
				contractKind,
				name
			}), null, null, null);
			Contract._assertingMustUseRewriter = false;
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06005A27 RID: 23079 RVA: 0x00134100 File Offset: 0x00132300
		// (remove) Token: 0x06005A28 RID: 23080 RVA: 0x00134108 File Offset: 0x00132308
		public static event EventHandler<ContractFailedEventArgs> ContractFailed
		{
			[SecurityCritical]
			add
			{
				ContractHelper.InternalContractFailed += value;
			}
			[SecurityCritical]
			remove
			{
				ContractHelper.InternalContractFailed -= value;
			}
		}

		// Token: 0x040037AB RID: 14251
		[ThreadStatic]
		private static bool _assertingMustUseRewriter;
	}
}
