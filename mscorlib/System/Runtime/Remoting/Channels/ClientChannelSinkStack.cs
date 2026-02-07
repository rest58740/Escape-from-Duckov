using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005AB RID: 1451
	[ComVisible(true)]
	public class ClientChannelSinkStack : IClientChannelSinkStack, IClientResponseChannelSinkStack
	{
		// Token: 0x06003849 RID: 14409 RVA: 0x0000259F File Offset: 0x0000079F
		public ClientChannelSinkStack()
		{
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x000CA1D8 File Offset: 0x000C83D8
		public ClientChannelSinkStack(IMessageSink replySink)
		{
			this._replySink = replySink;
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x000CA1E8 File Offset: 0x000C83E8
		[SecurityCritical]
		public void AsyncProcessResponse(ITransportHeaders headers, Stream stream)
		{
			if (this._sinkStack == null)
			{
				throw new RemotingException("The current sink stack is empty");
			}
			ChanelSinkStackEntry sinkStack = this._sinkStack;
			this._sinkStack = this._sinkStack.Next;
			((IClientChannelSink)sinkStack.Sink).AsyncProcessResponse(this, sinkStack.State, headers, stream);
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x000CA239 File Offset: 0x000C8439
		[SecurityCritical]
		public void DispatchException(Exception e)
		{
			this.DispatchReplyMessage(new ReturnMessage(e, null));
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x000CA248 File Offset: 0x000C8448
		[SecurityCritical]
		public void DispatchReplyMessage(IMessage msg)
		{
			if (this._replySink != null)
			{
				this._replySink.SyncProcessMessage(msg);
			}
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x000CA260 File Offset: 0x000C8460
		[SecurityCritical]
		public object Pop(IClientChannelSink sink)
		{
			while (this._sinkStack != null)
			{
				ChanelSinkStackEntry sinkStack = this._sinkStack;
				this._sinkStack = this._sinkStack.Next;
				if (sinkStack.Sink == sink)
				{
					return sinkStack.State;
				}
			}
			throw new RemotingException("The current sink stack is empty, or the specified sink was never pushed onto the current stack");
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x000CA2A9 File Offset: 0x000C84A9
		[SecurityCritical]
		public void Push(IClientChannelSink sink, object state)
		{
			this._sinkStack = new ChanelSinkStackEntry(sink, state, this._sinkStack);
		}

		// Token: 0x040025DB RID: 9691
		private IMessageSink _replySink;

		// Token: 0x040025DC RID: 9692
		private ChanelSinkStackEntry _sinkStack;
	}
}
