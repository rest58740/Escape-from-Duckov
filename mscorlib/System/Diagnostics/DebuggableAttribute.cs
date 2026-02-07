using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020009B8 RID: 2488
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, AllowMultiple = false)]
	public sealed class DebuggableAttribute : Attribute
	{
		// Token: 0x0600599D RID: 22941 RVA: 0x00132F9A File Offset: 0x0013119A
		public DebuggableAttribute(bool isJITTrackingEnabled, bool isJITOptimizerDisabled)
		{
			this.m_debuggingModes = DebuggableAttribute.DebuggingModes.None;
			if (isJITTrackingEnabled)
			{
				this.m_debuggingModes |= DebuggableAttribute.DebuggingModes.Default;
			}
			if (isJITOptimizerDisabled)
			{
				this.m_debuggingModes |= DebuggableAttribute.DebuggingModes.DisableOptimizations;
			}
		}

		// Token: 0x0600599E RID: 22942 RVA: 0x00132FCF File Offset: 0x001311CF
		public DebuggableAttribute(DebuggableAttribute.DebuggingModes modes)
		{
			this.m_debuggingModes = modes;
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x0600599F RID: 22943 RVA: 0x00132FDE File Offset: 0x001311DE
		public bool IsJITTrackingEnabled
		{
			get
			{
				return (this.m_debuggingModes & DebuggableAttribute.DebuggingModes.Default) > DebuggableAttribute.DebuggingModes.None;
			}
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x060059A0 RID: 22944 RVA: 0x00132FEB File Offset: 0x001311EB
		public bool IsJITOptimizerDisabled
		{
			get
			{
				return (this.m_debuggingModes & DebuggableAttribute.DebuggingModes.DisableOptimizations) > DebuggableAttribute.DebuggingModes.None;
			}
		}

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x060059A1 RID: 22945 RVA: 0x00132FFC File Offset: 0x001311FC
		public DebuggableAttribute.DebuggingModes DebuggingFlags
		{
			get
			{
				return this.m_debuggingModes;
			}
		}

		// Token: 0x04003774 RID: 14196
		private DebuggableAttribute.DebuggingModes m_debuggingModes;

		// Token: 0x020009B9 RID: 2489
		[Flags]
		[ComVisible(true)]
		public enum DebuggingModes
		{
			// Token: 0x04003776 RID: 14198
			None = 0,
			// Token: 0x04003777 RID: 14199
			Default = 1,
			// Token: 0x04003778 RID: 14200
			DisableOptimizations = 256,
			// Token: 0x04003779 RID: 14201
			IgnoreSymbolStoreSequencePoints = 2,
			// Token: 0x0400377A RID: 14202
			EnableEditAndContinue = 4
		}
	}
}
