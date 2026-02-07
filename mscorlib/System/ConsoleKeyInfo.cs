using System;

namespace System
{
	// Token: 0x020001C0 RID: 448
	[Serializable]
	public readonly struct ConsoleKeyInfo
	{
		// Token: 0x06001339 RID: 4921 RVA: 0x0004D88C File Offset: 0x0004BA8C
		public ConsoleKeyInfo(char keyChar, ConsoleKey key, bool shift, bool alt, bool control)
		{
			if (key < (ConsoleKey)0 || key > (ConsoleKey)255)
			{
				throw new ArgumentOutOfRangeException("key", "Console key values must be between 0 and 255 inclusive.");
			}
			this._keyChar = keyChar;
			this._key = key;
			this._mods = (ConsoleModifiers)0;
			if (shift)
			{
				this._mods |= ConsoleModifiers.Shift;
			}
			if (alt)
			{
				this._mods |= ConsoleModifiers.Alt;
			}
			if (control)
			{
				this._mods |= ConsoleModifiers.Control;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600133A RID: 4922 RVA: 0x0004D8FF File Offset: 0x0004BAFF
		public char KeyChar
		{
			get
			{
				return this._keyChar;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600133B RID: 4923 RVA: 0x0004D907 File Offset: 0x0004BB07
		public ConsoleKey Key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600133C RID: 4924 RVA: 0x0004D90F File Offset: 0x0004BB0F
		public ConsoleModifiers Modifiers
		{
			get
			{
				return this._mods;
			}
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x0004D917 File Offset: 0x0004BB17
		public override bool Equals(object value)
		{
			return value is ConsoleKeyInfo && this.Equals((ConsoleKeyInfo)value);
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x0004D92F File Offset: 0x0004BB2F
		public bool Equals(ConsoleKeyInfo obj)
		{
			return obj._keyChar == this._keyChar && obj._key == this._key && obj._mods == this._mods;
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0004D95D File Offset: 0x0004BB5D
		public static bool operator ==(ConsoleKeyInfo a, ConsoleKeyInfo b)
		{
			return a.Equals(b);
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0004D967 File Offset: 0x0004BB67
		public static bool operator !=(ConsoleKeyInfo a, ConsoleKeyInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0004D973 File Offset: 0x0004BB73
		public override int GetHashCode()
		{
			return (int)((ConsoleKey)this._keyChar | (int)this._key << 16 | (ConsoleKey)((int)this._mods << 24));
		}

		// Token: 0x04001439 RID: 5177
		private readonly char _keyChar;

		// Token: 0x0400143A RID: 5178
		private readonly ConsoleKey _key;

		// Token: 0x0400143B RID: 5179
		private readonly ConsoleModifiers _mods;
	}
}
