using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x02000243 RID: 579
	[ComVisible(true)]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public abstract class MulticastDelegate : Delegate
	{
		// Token: 0x06001A4E RID: 6734 RVA: 0x00061088 File Offset: 0x0005F288
		protected MulticastDelegate(object target, string method) : base(target, method)
		{
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x00061092 File Offset: 0x0005F292
		protected MulticastDelegate(Type target, string method) : base(target, method)
		{
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x0006109C File Offset: 0x0005F29C
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x000610A8 File Offset: 0x0005F2A8
		protected sealed override object DynamicInvokeImpl(object[] args)
		{
			if (this.delegates == null)
			{
				return base.DynamicInvokeImpl(args);
			}
			int num = 0;
			int num2 = this.delegates.Length;
			object result;
			do
			{
				result = this.delegates[num].DynamicInvoke(args);
			}
			while (++num < num2);
			return result;
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06001A52 RID: 6738 RVA: 0x000610E8 File Offset: 0x0005F2E8
		internal bool HasSingleTarget
		{
			get
			{
				return this.delegates == null;
			}
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x000610F4 File Offset: 0x0005F2F4
		public sealed override bool Equals(object obj)
		{
			if (!base.Equals(obj))
			{
				return false;
			}
			MulticastDelegate multicastDelegate = obj as MulticastDelegate;
			if (multicastDelegate == null)
			{
				return false;
			}
			if (this.delegates == null && multicastDelegate.delegates == null)
			{
				return true;
			}
			if (this.delegates == null ^ multicastDelegate.delegates == null)
			{
				return false;
			}
			if (this.delegates.Length != multicastDelegate.delegates.Length)
			{
				return false;
			}
			for (int i = 0; i < this.delegates.Length; i++)
			{
				if (!this.delegates[i].Equals(multicastDelegate.delegates[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x00061182 File Offset: 0x0005F382
		public sealed override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x0006118A File Offset: 0x0005F38A
		protected override MethodInfo GetMethodImpl()
		{
			if (this.delegates != null)
			{
				return this.delegates[this.delegates.Length - 1].Method;
			}
			return base.GetMethodImpl();
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x000611B1 File Offset: 0x0005F3B1
		public sealed override Delegate[] GetInvocationList()
		{
			if (this.delegates != null)
			{
				return (Delegate[])this.delegates.Clone();
			}
			return new Delegate[]
			{
				this
			};
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x000611D8 File Offset: 0x0005F3D8
		protected sealed override Delegate CombineImpl(Delegate follow)
		{
			if (follow == null)
			{
				return this;
			}
			MulticastDelegate multicastDelegate = (MulticastDelegate)follow;
			MulticastDelegate multicastDelegate2 = Delegate.AllocDelegateLike_internal(this);
			if (this.delegates == null && multicastDelegate.delegates == null)
			{
				multicastDelegate2.delegates = new Delegate[]
				{
					this,
					multicastDelegate
				};
			}
			else if (this.delegates == null)
			{
				multicastDelegate2.delegates = new Delegate[1 + multicastDelegate.delegates.Length];
				multicastDelegate2.delegates[0] = this;
				Array.Copy(multicastDelegate.delegates, 0, multicastDelegate2.delegates, 1, multicastDelegate.delegates.Length);
			}
			else if (multicastDelegate.delegates == null)
			{
				multicastDelegate2.delegates = new Delegate[this.delegates.Length + 1];
				Array.Copy(this.delegates, 0, multicastDelegate2.delegates, 0, this.delegates.Length);
				multicastDelegate2.delegates[multicastDelegate2.delegates.Length - 1] = multicastDelegate;
			}
			else
			{
				multicastDelegate2.delegates = new Delegate[this.delegates.Length + multicastDelegate.delegates.Length];
				Array.Copy(this.delegates, 0, multicastDelegate2.delegates, 0, this.delegates.Length);
				Array.Copy(multicastDelegate.delegates, 0, multicastDelegate2.delegates, this.delegates.Length, multicastDelegate.delegates.Length);
			}
			return multicastDelegate2;
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x00061310 File Offset: 0x0005F510
		private int LastIndexOf(Delegate[] haystack, Delegate[] needle)
		{
			if (haystack.Length < needle.Length)
			{
				return -1;
			}
			if (haystack.Length == needle.Length)
			{
				for (int i = 0; i < haystack.Length; i++)
				{
					if (!haystack[i].Equals(needle[i]))
					{
						return -1;
					}
				}
				return 0;
			}
			int num;
			for (int j = haystack.Length - needle.Length; j >= 0; j -= num + 1)
			{
				num = 0;
				while (needle[num].Equals(haystack[j]))
				{
					if (num == needle.Length - 1)
					{
						return j - num;
					}
					j++;
					num++;
				}
			}
			return -1;
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00061388 File Offset: 0x0005F588
		protected sealed override Delegate RemoveImpl(Delegate value)
		{
			if (value == null)
			{
				return this;
			}
			MulticastDelegate multicastDelegate = (MulticastDelegate)value;
			if (this.delegates == null && multicastDelegate.delegates == null)
			{
				if (!this.Equals(multicastDelegate))
				{
					return this;
				}
				return null;
			}
			else
			{
				if (this.delegates == null)
				{
					foreach (Delegate obj in multicastDelegate.delegates)
					{
						if (this.Equals(obj))
						{
							return null;
						}
					}
					return this;
				}
				if (multicastDelegate.delegates == null)
				{
					int num = Array.LastIndexOf<Delegate>(this.delegates, multicastDelegate);
					if (num == -1)
					{
						return this;
					}
					if (this.delegates.Length <= 1)
					{
						throw new InvalidOperationException();
					}
					if (this.delegates.Length == 2)
					{
						return this.delegates[(num == 0) ? 1 : 0];
					}
					MulticastDelegate multicastDelegate2 = Delegate.AllocDelegateLike_internal(this);
					multicastDelegate2.delegates = new Delegate[this.delegates.Length - 1];
					Array.Copy(this.delegates, multicastDelegate2.delegates, num);
					Array.Copy(this.delegates, num + 1, multicastDelegate2.delegates, num, this.delegates.Length - num - 1);
					return multicastDelegate2;
				}
				else
				{
					if (this.delegates.Equals(multicastDelegate.delegates))
					{
						return null;
					}
					int num2 = this.LastIndexOf(this.delegates, multicastDelegate.delegates);
					if (num2 == -1)
					{
						return this;
					}
					MulticastDelegate multicastDelegate3 = Delegate.AllocDelegateLike_internal(this);
					multicastDelegate3.delegates = new Delegate[this.delegates.Length - multicastDelegate.delegates.Length];
					Array.Copy(this.delegates, multicastDelegate3.delegates, num2);
					Array.Copy(this.delegates, num2 + multicastDelegate.delegates.Length, multicastDelegate3.delegates, num2, this.delegates.Length - num2 - multicastDelegate.delegates.Length);
					return multicastDelegate3;
				}
			}
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x00061530 File Offset: 0x0005F730
		public static bool operator ==(MulticastDelegate d1, MulticastDelegate d2)
		{
			if (d1 == null)
			{
				return d2 == null;
			}
			return d1.Equals(d2);
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x00061541 File Offset: 0x0005F741
		public static bool operator !=(MulticastDelegate d1, MulticastDelegate d2)
		{
			if (d1 == null)
			{
				return d2 != null;
			}
			return !d1.Equals(d2);
		}

		// Token: 0x04001732 RID: 5938
		private Delegate[] delegates;
	}
}
