using System;
using System.Runtime.CompilerServices;
using Microsoft.Win32.SafeHandles;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006D0 RID: 1744
	public abstract class SafeBuffer : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06004010 RID: 16400 RVA: 0x000E05DC File Offset: 0x000DE7DC
		protected SafeBuffer(bool ownsHandle) : base(ownsHandle)
		{
			this._numBytes = SafeBuffer.Uninitialized;
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x000E05F0 File Offset: 0x000DE7F0
		[CLSCompliant(false)]
		public void Initialize(ulong numBytes)
		{
			if (IntPtr.Size == 4 && numBytes > (ulong)-1)
			{
				throw new ArgumentOutOfRangeException("numBytes", "The number of bytes cannot exceed the virtual address space on a 32 bit machine.");
			}
			if (numBytes >= (ulong)SafeBuffer.Uninitialized)
			{
				throw new ArgumentOutOfRangeException("numBytes", "The length of the buffer must be less than the maximum UIntPtr value for your platform.");
			}
			this._numBytes = (UIntPtr)numBytes;
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x000E0644 File Offset: 0x000DE844
		[CLSCompliant(false)]
		public void Initialize(uint numElements, uint sizeOfEachElement)
		{
			if (IntPtr.Size == 4 && numElements * sizeOfEachElement > 4294967295U)
			{
				throw new ArgumentOutOfRangeException("numBytes", "The number of bytes cannot exceed the virtual address space on a 32 bit machine.");
			}
			if ((ulong)(numElements * sizeOfEachElement) >= (ulong)SafeBuffer.Uninitialized)
			{
				throw new ArgumentOutOfRangeException("numElements", "The length of the buffer must be less than the maximum UIntPtr value for your platform.");
			}
			this._numBytes = (UIntPtr)(checked(numElements * sizeOfEachElement));
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x000E069D File Offset: 0x000DE89D
		[CLSCompliant(false)]
		public void Initialize<T>(uint numElements) where T : struct
		{
			this.Initialize(numElements, SafeBuffer.AlignedSizeOf<T>());
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x000E06AC File Offset: 0x000DE8AC
		[CLSCompliant(false)]
		public unsafe void AcquirePointer(ref byte* pointer)
		{
			if (this._numBytes == SafeBuffer.Uninitialized)
			{
				throw SafeBuffer.NotInitialized();
			}
			pointer = (IntPtr)((UIntPtr)0);
			bool flag = false;
			base.DangerousAddRef(ref flag);
			pointer = (void*)this.handle;
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x000E06EC File Offset: 0x000DE8EC
		public void ReleasePointer()
		{
			if (this._numBytes == SafeBuffer.Uninitialized)
			{
				throw SafeBuffer.NotInitialized();
			}
			base.DangerousRelease();
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x000E070C File Offset: 0x000DE90C
		[CLSCompliant(false)]
		public unsafe T Read<T>(ulong byteOffset) where T : struct
		{
			if (this._numBytes == SafeBuffer.Uninitialized)
			{
				throw SafeBuffer.NotInitialized();
			}
			uint num = SafeBuffer.SizeOf<T>();
			byte* ptr = (byte*)((void*)this.handle) + byteOffset;
			this.SpaceCheck(ptr, (ulong)num);
			T result = default(T);
			bool flag = false;
			try
			{
				base.DangerousAddRef(ref flag);
				try
				{
					fixed (byte* ptr2 = Unsafe.As<T, byte>(ref result))
					{
						Buffer.Memmove(ptr2, ptr, num);
					}
				}
				finally
				{
					byte* ptr2 = null;
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x000E07A4 File Offset: 0x000DE9A4
		[CLSCompliant(false)]
		public unsafe void ReadArray<T>(ulong byteOffset, T[] array, int index, int count) where T : struct
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", "Buffer cannot be null.");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (array.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (this._numBytes == SafeBuffer.Uninitialized)
			{
				throw SafeBuffer.NotInitialized();
			}
			uint num = SafeBuffer.SizeOf<T>();
			uint num2 = SafeBuffer.AlignedSizeOf<T>();
			byte* ptr = (byte*)((void*)this.handle) + byteOffset;
			this.SpaceCheck(ptr, checked(unchecked((ulong)num2) * (ulong)(unchecked((long)count))));
			bool flag = false;
			try
			{
				base.DangerousAddRef(ref flag);
				if (count > 0)
				{
					try
					{
						fixed (byte* ptr2 = Unsafe.As<T, byte>(ref array[index]))
						{
							byte* ptr3 = ptr2;
							for (int i = 0; i < count; i++)
							{
								Buffer.Memmove(ptr3 + (ulong)num * (ulong)((long)i), ptr + (ulong)num2 * (ulong)((long)i), num);
							}
						}
					}
					finally
					{
						byte* ptr2 = null;
					}
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x000E08C0 File Offset: 0x000DEAC0
		[CLSCompliant(false)]
		public unsafe void Write<T>(ulong byteOffset, T value) where T : struct
		{
			if (this._numBytes == SafeBuffer.Uninitialized)
			{
				throw SafeBuffer.NotInitialized();
			}
			uint num = SafeBuffer.SizeOf<T>();
			byte* ptr = (byte*)((void*)this.handle) + byteOffset;
			this.SpaceCheck(ptr, (ulong)num);
			bool flag = false;
			try
			{
				base.DangerousAddRef(ref flag);
				try
				{
					fixed (byte* ptr2 = Unsafe.As<T, byte>(ref value))
					{
						byte* src = ptr2;
						Buffer.Memmove(ptr, src, num);
					}
				}
				finally
				{
					byte* ptr2 = null;
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x000E0950 File Offset: 0x000DEB50
		[CLSCompliant(false)]
		public unsafe void WriteArray<T>(ulong byteOffset, T[] array, int index, int count) where T : struct
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", "Buffer cannot be null.");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (array.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (this._numBytes == SafeBuffer.Uninitialized)
			{
				throw SafeBuffer.NotInitialized();
			}
			uint num = SafeBuffer.SizeOf<T>();
			uint num2 = SafeBuffer.AlignedSizeOf<T>();
			byte* ptr = (byte*)((void*)this.handle) + byteOffset;
			this.SpaceCheck(ptr, checked(unchecked((ulong)num2) * (ulong)(unchecked((long)count))));
			bool flag = false;
			try
			{
				base.DangerousAddRef(ref flag);
				if (count > 0)
				{
					try
					{
						fixed (byte* ptr2 = Unsafe.As<T, byte>(ref array[index]))
						{
							byte* ptr3 = ptr2;
							for (int i = 0; i < count; i++)
							{
								Buffer.Memmove(ptr + (ulong)num2 * (ulong)((long)i), ptr3 + (ulong)num * (ulong)((long)i), num);
							}
						}
					}
					finally
					{
						byte* ptr2 = null;
					}
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x0600401A RID: 16410 RVA: 0x000E0A6C File Offset: 0x000DEC6C
		[CLSCompliant(false)]
		public ulong ByteLength
		{
			get
			{
				if (this._numBytes == SafeBuffer.Uninitialized)
				{
					throw SafeBuffer.NotInitialized();
				}
				return (ulong)this._numBytes;
			}
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x000E0A91 File Offset: 0x000DEC91
		private unsafe void SpaceCheck(byte* ptr, ulong sizeInBytes)
		{
			if ((ulong)this._numBytes < sizeInBytes)
			{
				SafeBuffer.NotEnoughRoom();
			}
			if ((long)((byte*)ptr - (byte*)((void*)this.handle)) > (long)((ulong)this._numBytes - sizeInBytes))
			{
				SafeBuffer.NotEnoughRoom();
			}
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x000E0ACA File Offset: 0x000DECCA
		private static void NotEnoughRoom()
		{
			throw new ArgumentException("Not enough space available in the buffer.");
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x000E0AD6 File Offset: 0x000DECD6
		private static InvalidOperationException NotInitialized()
		{
			return new InvalidOperationException("You must call Initialize on this object instance before using it.");
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x000E0AE4 File Offset: 0x000DECE4
		internal static uint AlignedSizeOf<T>() where T : struct
		{
			uint num = SafeBuffer.SizeOf<T>();
			if (num == 1U || num == 2U)
			{
				return num;
			}
			return (uint)((ulong)(num + 3U) & 18446744073709551612UL);
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x000E0B0A File Offset: 0x000DED0A
		internal static uint SizeOf<T>() where T : struct
		{
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				throw new ArgumentException("The specified Type must be a struct containing no references.");
			}
			return (uint)Unsafe.SizeOf<T>();
		}

		// Token: 0x04002A15 RID: 10773
		private static readonly UIntPtr Uninitialized = (UIntPtr.Size == 4) ? ((UIntPtr)uint.MaxValue) : ((UIntPtr)ulong.MaxValue);

		// Token: 0x04002A16 RID: 10774
		private UIntPtr _numBytes;
	}
}
