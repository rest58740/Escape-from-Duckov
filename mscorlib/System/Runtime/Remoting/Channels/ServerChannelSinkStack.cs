using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005C5 RID: 1477
	[ComVisible(true)]
	public class ServerChannelSinkStack : IServerChannelSinkStack, IServerResponseChannelSinkStack
	{
		// Token: 0x060038A1 RID: 14497 RVA: 0x000CA759 File Offset: 0x000C8959
		[SecurityCritical]
		public Stream GetResponseStream(IMessage msg, ITransportHeaders headers)
		{
			if (this._sinkStack == null)
			{
				throw new RemotingException("The sink stack is empty");
			}
			return ((IServerChannelSink)this._sinkStack.Sink).GetResponseStream(this, this._sinkStack.State, msg, headers);
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x000CA794 File Offset: 0x000C8994
		[SecurityCritical]
		public object Pop(IServerChannelSink sink)
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

		// Token: 0x060038A3 RID: 14499 RVA: 0x000CA7DD File Offset: 0x000C89DD
		[SecurityCritical]
		public void Push(IServerChannelSink sink, object state)
		{
			this._sinkStack = new ChanelSinkStackEntry(sink, state, this._sinkStack);
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x000479FC File Offset: 0x00045BFC
		[SecurityCritical]
		[MonoTODO]
		public void ServerCallback(IAsyncResult ar)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x000479FC File Offset: 0x00045BFC
		[SecurityCritical]
		[MonoTODO]
		public void Store(IServerChannelSink sink, object state)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		[SecurityCritical]
		public void StoreAndDispatch(IServerChannelSink sink, object state)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x000CA7F4 File Offset: 0x000C89F4
		[SecurityCritical]
		public void AsyncProcessResponse(IMessage msg, ITransportHeaders headers, Stream stream)
		{
			if (this._sinkStack == null)
			{
				throw new RemotingException("The current sink stack is empty");
			}
			ChanelSinkStackEntry sinkStack = this._sinkStack;
			this._sinkStack = this._sinkStack.Next;
			((IServerChannelSink)sinkStack.Sink).AsyncProcessResponse(this, sinkStack.State, msg, headers, stream);
		}

		// Token: 0x040025E9 RID: 9705
		private ChanelSinkStackEntry _sinkStack;
	}
}
