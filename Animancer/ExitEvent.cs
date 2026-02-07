using System;

namespace Animancer
{
	// Token: 0x0200004A RID: 74
	public class ExitEvent : Key, IUpdatable, Key.IListItem
	{
		// Token: 0x060004A3 RID: 1187 RVA: 0x0000D138 File Offset: 0x0000B338
		public static void Register(AnimancerNode node, Action callback)
		{
			ExitEvent exitEvent = ObjectPool.Acquire<ExitEvent>();
			exitEvent._Callback = callback;
			exitEvent._Node = node;
			node.Root.RequirePostUpdate(exitEvent);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000D168 File Offset: 0x0000B368
		public static bool Unregister(AnimancerPlayable animancer)
		{
			for (int i = animancer.PostUpdatableCount - 1; i >= 0; i--)
			{
				ExitEvent exitEvent = animancer.GetPostUpdatable(i) as ExitEvent;
				if (exitEvent != null)
				{
					animancer.CancelPostUpdate(exitEvent);
					exitEvent.Release();
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0000D1A8 File Offset: 0x0000B3A8
		public static bool Unregister(AnimancerNode node)
		{
			AnimancerPlayable root = node.Root;
			for (int i = root.PostUpdatableCount - 1; i >= 0; i--)
			{
				ExitEvent exitEvent = root.GetPostUpdatable(i) as ExitEvent;
				if (exitEvent != null && exitEvent._Node == node)
				{
					root.CancelPostUpdate(exitEvent);
					exitEvent.Release();
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000D1F8 File Offset: 0x0000B3F8
		void IUpdatable.Update()
		{
			if (this._Node.IsValid() && this._Node.EffectiveWeight > 0f)
			{
				return;
			}
			this._Callback();
			this._Node.Root.CancelPostUpdate(this);
			this.Release();
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000D247 File Offset: 0x0000B447
		private void Release()
		{
			this._Callback = null;
			this._Node = null;
			ObjectPool.Release<ExitEvent>(this);
		}

		// Token: 0x040000C6 RID: 198
		private Action _Callback;

		// Token: 0x040000C7 RID: 199
		private AnimancerNode _Node;
	}
}
