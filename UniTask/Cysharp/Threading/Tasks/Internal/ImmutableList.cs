using System;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x02000122 RID: 290
	internal class ImmutableList<T>
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x0000F8FD File Offset: 0x0000DAFD
		public T[] Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0000F905 File Offset: 0x0000DB05
		private ImmutableList()
		{
			this.data = new T[0];
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0000F919 File Offset: 0x0000DB19
		public ImmutableList(T[] data)
		{
			this.data = data;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0000F928 File Offset: 0x0000DB28
		public ImmutableList<T> Add(T value)
		{
			T[] array = new T[this.data.Length + 1];
			Array.Copy(this.data, array, this.data.Length);
			array[this.data.Length] = value;
			return new ImmutableList<T>(array);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0000F970 File Offset: 0x0000DB70
		public ImmutableList<T> Remove(T value)
		{
			int num = this.IndexOf(value);
			if (num < 0)
			{
				return this;
			}
			int num2 = this.data.Length;
			if (num2 == 1)
			{
				return ImmutableList<T>.Empty;
			}
			T[] destinationArray = new T[num2 - 1];
			Array.Copy(this.data, 0, destinationArray, 0, num);
			Array.Copy(this.data, num + 1, destinationArray, num, num2 - num - 1);
			return new ImmutableList<T>(destinationArray);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0000F9D4 File Offset: 0x0000DBD4
		public int IndexOf(T value)
		{
			for (int i = 0; i < this.data.Length; i++)
			{
				if (object.Equals(this.data[i], value))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x04000159 RID: 345
		public static readonly ImmutableList<T> Empty = new ImmutableList<T>();

		// Token: 0x0400015A RID: 346
		private T[] data;
	}
}
