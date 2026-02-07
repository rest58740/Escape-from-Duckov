using System;

namespace Sirenix.Utilities
{
	// Token: 0x02000012 RID: 18
	public static class ArrayUtilities
	{
		// Token: 0x060000EE RID: 238 RVA: 0x00007748 File Offset: 0x00005948
		public static T[] CreateNewArrayWithAddedElement<T>(T[] array, T value)
		{
			if (array == null)
			{
				return new T[]
				{
					value
				};
			}
			T[] array2 = new T[array.Length + 1];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i];
			}
			array2[array2.Length - 1] = value;
			return array2;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000779C File Offset: 0x0000599C
		public static T[] CreateNewArrayWithInsertedElement<T>(T[] array, int index, T value)
		{
			if (array == null)
			{
				if (index == 0)
				{
					return new T[]
					{
						value
					};
				}
				throw new ArgumentNullException();
			}
			else
			{
				if (index < 0 || index > array.Length)
				{
					throw new ArgumentOutOfRangeException();
				}
				T[] array2 = new T[array.Length + 1];
				for (int i = 0; i < array2.Length; i++)
				{
					if (i < index)
					{
						array2[i] = array[i];
					}
					else if (i > index)
					{
						array2[i] = array[i - 1];
					}
					else
					{
						array2[i] = value;
					}
				}
				return array2;
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007820 File Offset: 0x00005A20
		public static T[] CreateNewArrayWithRemovedElement<T>(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException();
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException();
			}
			T[] array2 = new T[array.Length - 1];
			for (int i = 0; i < array.Length; i++)
			{
				if (i < index)
				{
					array2[i] = array[i];
				}
				else if (i > index)
				{
					array2[i - 1] = array[i];
				}
			}
			return array2;
		}
	}
}
