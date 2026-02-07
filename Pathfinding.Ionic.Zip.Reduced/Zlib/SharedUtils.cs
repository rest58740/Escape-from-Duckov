using System;
using System.IO;
using System.Text;

namespace Pathfinding.Ionic.Zlib
{
	// Token: 0x02000060 RID: 96
	internal class SharedUtils
	{
		// Token: 0x0600042E RID: 1070 RVA: 0x0001D848 File Offset: 0x0001BA48
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001D850 File Offset: 0x0001BA50
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (byte)array[i];
			}
			return num;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001D8A0 File Offset: 0x0001BAA0
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0001D8B0 File Offset: 0x0001BAB0
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
