using System;
using System.Diagnostics;
using System.Threading;

namespace System
{
	// Token: 0x02000151 RID: 337
	[DebuggerTypeProxy(typeof(LazyDebugView<>))]
	[DebuggerDisplay("ThreadSafetyMode={Mode}, IsValueCreated={IsValueCreated}, IsValueFaulted={IsValueFaulted}, Value={ValueForDebugDisplay}")]
	[Serializable]
	public class Lazy<T>
	{
		// Token: 0x06000CA5 RID: 3237 RVA: 0x00032A81 File Offset: 0x00030C81
		private static T CreateViaDefaultConstructor()
		{
			return (T)((object)LazyHelper.CreateViaDefaultConstructor(typeof(T)));
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00032A97 File Offset: 0x00030C97
		public Lazy() : this(null, LazyThreadSafetyMode.ExecutionAndPublication, true)
		{
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x00032AA2 File Offset: 0x00030CA2
		public Lazy(T value)
		{
			this._value = value;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00032AB1 File Offset: 0x00030CB1
		public Lazy(Func<T> valueFactory) : this(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication, false)
		{
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00032ABC File Offset: 0x00030CBC
		public Lazy(bool isThreadSafe) : this(null, LazyHelper.GetModeFromIsThreadSafe(isThreadSafe), true)
		{
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00032ACC File Offset: 0x00030CCC
		public Lazy(LazyThreadSafetyMode mode) : this(null, mode, true)
		{
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00032AD7 File Offset: 0x00030CD7
		public Lazy(Func<T> valueFactory, bool isThreadSafe) : this(valueFactory, LazyHelper.GetModeFromIsThreadSafe(isThreadSafe), false)
		{
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00032AE7 File Offset: 0x00030CE7
		public Lazy(Func<T> valueFactory, LazyThreadSafetyMode mode) : this(valueFactory, mode, false)
		{
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00032AF2 File Offset: 0x00030CF2
		private Lazy(Func<T> valueFactory, LazyThreadSafetyMode mode, bool useDefaultConstructor)
		{
			if (valueFactory == null && !useDefaultConstructor)
			{
				throw new ArgumentNullException("valueFactory");
			}
			this._factory = valueFactory;
			this._state = LazyHelper.Create(mode, useDefaultConstructor);
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00032B21 File Offset: 0x00030D21
		private void ViaConstructor()
		{
			this._value = Lazy<T>.CreateViaDefaultConstructor();
			this._state = null;
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00032B38 File Offset: 0x00030D38
		private void ViaFactory(LazyThreadSafetyMode mode)
		{
			try
			{
				Func<T> factory = this._factory;
				if (factory == null)
				{
					throw new InvalidOperationException("ValueFactory attempted to access the Value property of this instance.");
				}
				this._factory = null;
				this._value = factory();
				this._state = null;
			}
			catch (Exception exception)
			{
				this._state = new LazyHelper(mode, exception);
				throw;
			}
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00032B9C File Offset: 0x00030D9C
		private void ExecutionAndPublication(LazyHelper executionAndPublication, bool useDefaultConstructor)
		{
			lock (executionAndPublication)
			{
				if (this._state == executionAndPublication)
				{
					if (useDefaultConstructor)
					{
						this.ViaConstructor();
					}
					else
					{
						this.ViaFactory(LazyThreadSafetyMode.ExecutionAndPublication);
					}
				}
			}
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00032BF0 File Offset: 0x00030DF0
		private void PublicationOnly(LazyHelper publicationOnly, T possibleValue)
		{
			if (Interlocked.CompareExchange<LazyHelper>(ref this._state, LazyHelper.PublicationOnlyWaitForOtherThreadToPublish, publicationOnly) == publicationOnly)
			{
				this._factory = null;
				this._value = possibleValue;
				this._state = null;
			}
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00032C1D File Offset: 0x00030E1D
		private void PublicationOnlyViaConstructor(LazyHelper initializer)
		{
			this.PublicationOnly(initializer, Lazy<T>.CreateViaDefaultConstructor());
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x00032C2C File Offset: 0x00030E2C
		private void PublicationOnlyViaFactory(LazyHelper initializer)
		{
			Func<T> factory = this._factory;
			if (factory == null)
			{
				this.PublicationOnlyWaitForOtherThreadToPublish();
				return;
			}
			this.PublicationOnly(initializer, factory());
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x00032C58 File Offset: 0x00030E58
		private void PublicationOnlyWaitForOtherThreadToPublish()
		{
			SpinWait spinWait = default(SpinWait);
			while (this._state != null)
			{
				spinWait.SpinOnce();
			}
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00032C80 File Offset: 0x00030E80
		private T CreateValue()
		{
			LazyHelper state = this._state;
			if (state != null)
			{
				switch (state.State)
				{
				case LazyState.NoneViaConstructor:
					this.ViaConstructor();
					goto IL_84;
				case LazyState.NoneViaFactory:
					this.ViaFactory(LazyThreadSafetyMode.None);
					goto IL_84;
				case LazyState.PublicationOnlyViaConstructor:
					this.PublicationOnlyViaConstructor(state);
					goto IL_84;
				case LazyState.PublicationOnlyViaFactory:
					this.PublicationOnlyViaFactory(state);
					goto IL_84;
				case LazyState.PublicationOnlyWait:
					this.PublicationOnlyWaitForOtherThreadToPublish();
					goto IL_84;
				case LazyState.ExecutionAndPublicationViaConstructor:
					this.ExecutionAndPublication(state, true);
					goto IL_84;
				case LazyState.ExecutionAndPublicationViaFactory:
					this.ExecutionAndPublication(state, false);
					goto IL_84;
				}
				state.ThrowException();
			}
			IL_84:
			return this.Value;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00032D18 File Offset: 0x00030F18
		public override string ToString()
		{
			if (!this.IsValueCreated)
			{
				return "Value is not created.";
			}
			T value = this.Value;
			return value.ToString();
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00032D48 File Offset: 0x00030F48
		internal T ValueForDebugDisplay
		{
			get
			{
				if (!this.IsValueCreated)
				{
					return default(T);
				}
				return this._value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00032D6D File Offset: 0x00030F6D
		internal LazyThreadSafetyMode? Mode
		{
			get
			{
				return LazyHelper.GetMode(this._state);
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00032D7C File Offset: 0x00030F7C
		internal bool IsValueFaulted
		{
			get
			{
				return LazyHelper.GetIsValueFaulted(this._state);
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x00032D8B File Offset: 0x00030F8B
		public bool IsValueCreated
		{
			get
			{
				return this._state == null;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x00032D98 File Offset: 0x00030F98
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public T Value
		{
			get
			{
				if (this._state != null)
				{
					return this.CreateValue();
				}
				return this._value;
			}
		}

		// Token: 0x0400126D RID: 4717
		private volatile LazyHelper _state;

		// Token: 0x0400126E RID: 4718
		private Func<T> _factory;

		// Token: 0x0400126F RID: 4719
		private T _value;
	}
}
