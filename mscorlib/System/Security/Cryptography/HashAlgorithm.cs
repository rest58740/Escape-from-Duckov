using System;
using System.Buffers;
using System.IO;

namespace System.Security.Cryptography
{
	// Token: 0x02000471 RID: 1137
	public abstract class HashAlgorithm : IDisposable, ICryptoTransform
	{
		// Token: 0x06002E02 RID: 11778 RVA: 0x000A5A14 File Offset: 0x000A3C14
		public static HashAlgorithm Create()
		{
			return CryptoConfigForwarder.CreateDefaultHashAlgorithm();
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x000A5A1B File Offset: 0x000A3C1B
		public static HashAlgorithm Create(string hashName)
		{
			return (HashAlgorithm)CryptoConfigForwarder.CreateFromName(hashName);
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06002E04 RID: 11780 RVA: 0x000A5A28 File Offset: 0x000A3C28
		public virtual int HashSize
		{
			get
			{
				return this.HashSizeValue;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06002E05 RID: 11781 RVA: 0x000A5A30 File Offset: 0x000A3C30
		public virtual byte[] Hash
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException(null);
				}
				if (this.State != 0)
				{
					throw new CryptographicUnexpectedOperationException("Hash must be finalized before the hash value is retrieved.");
				}
				byte[] hashValue = this.HashValue;
				return (byte[])((hashValue != null) ? hashValue.Clone() : null);
			}
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000A5A6B File Offset: 0x000A3C6B
		public byte[] ComputeHash(byte[] buffer)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(null);
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.HashCore(buffer, 0, buffer.Length);
			return this.CaptureHashCodeAndReinitialize();
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000A5A9C File Offset: 0x000A3C9C
		public bool TryComputeHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(null);
			}
			if (destination.Length < this.HashSizeValue / 8)
			{
				bytesWritten = 0;
				return false;
			}
			this.HashCore(source);
			if (!this.TryHashFinal(destination, out bytesWritten))
			{
				throw new InvalidOperationException("The algorithm's implementation is incorrect.");
			}
			this.HashValue = null;
			this.Initialize();
			return true;
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000A5AF8 File Offset: 0x000A3CF8
		public byte[] ComputeHash(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0 || count > buffer.Length)
			{
				throw new ArgumentException("Argument {0} should be larger than {1}.");
			}
			if (buffer.Length - count < offset)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (this._disposed)
			{
				throw new ObjectDisposedException(null);
			}
			this.HashCore(buffer, offset, count);
			return this.CaptureHashCodeAndReinitialize();
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000A5B70 File Offset: 0x000A3D70
		public byte[] ComputeHash(Stream inputStream)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(null);
			}
			byte[] array = ArrayPool<byte>.Shared.Rent(4096);
			byte[] result;
			try
			{
				int cbSize;
				while ((cbSize = inputStream.Read(array, 0, array.Length)) > 0)
				{
					this.HashCore(array, 0, cbSize);
				}
				result = this.CaptureHashCodeAndReinitialize();
			}
			finally
			{
				CryptographicOperations.ZeroMemory(array);
				ArrayPool<byte>.Shared.Return(array, false);
			}
			return result;
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x000A5BEC File Offset: 0x000A3DEC
		private byte[] CaptureHashCodeAndReinitialize()
		{
			this.HashValue = this.HashFinal();
			byte[] result = (byte[])this.HashValue.Clone();
			this.Initialize();
			return result;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x000A5C10 File Offset: 0x000A3E10
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000A5C1F File Offset: 0x000A3E1F
		public void Clear()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000A5C27 File Offset: 0x000A3E27
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._disposed = true;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06002E0E RID: 11790 RVA: 0x000040F7 File Offset: 0x000022F7
		public virtual int InputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06002E0F RID: 11791 RVA: 0x000040F7 File Offset: 0x000022F7
		public virtual int OutputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06002E10 RID: 11792 RVA: 0x000040F7 File Offset: 0x000022F7
		public virtual bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06002E11 RID: 11793 RVA: 0x000040F7 File Offset: 0x000022F7
		public virtual bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000A5C33 File Offset: 0x000A3E33
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			this.ValidateTransformBlock(inputBuffer, inputOffset, inputCount);
			this.State = 1;
			this.HashCore(inputBuffer, inputOffset, inputCount);
			if (outputBuffer != null && (inputBuffer != outputBuffer || inputOffset != outputOffset))
			{
				Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
			}
			return inputCount;
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x000A5C6C File Offset: 0x000A3E6C
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			this.ValidateTransformBlock(inputBuffer, inputOffset, inputCount);
			this.HashCore(inputBuffer, inputOffset, inputCount);
			this.HashValue = this.CaptureHashCodeAndReinitialize();
			byte[] array;
			if (inputCount != 0)
			{
				array = new byte[inputCount];
				Buffer.BlockCopy(inputBuffer, inputOffset, array, 0, inputCount);
			}
			else
			{
				array = Array.Empty<byte>();
			}
			this.State = 0;
			return array;
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000A5CBC File Offset: 0x000A3EBC
		private void ValidateTransformBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset", "Non-negative number required.");
			}
			if (inputCount < 0 || inputCount > inputBuffer.Length)
			{
				throw new ArgumentException("Argument {0} should be larger than {1}.");
			}
			if (inputBuffer.Length - inputCount < inputOffset)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (this._disposed)
			{
				throw new ObjectDisposedException(null);
			}
		}

		// Token: 0x06002E15 RID: 11797
		protected abstract void HashCore(byte[] array, int ibStart, int cbSize);

		// Token: 0x06002E16 RID: 11798
		protected abstract byte[] HashFinal();

		// Token: 0x06002E17 RID: 11799
		public abstract void Initialize();

		// Token: 0x06002E18 RID: 11800 RVA: 0x000A5D24 File Offset: 0x000A3F24
		protected virtual void HashCore(ReadOnlySpan<byte> source)
		{
			byte[] array = ArrayPool<byte>.Shared.Rent(source.Length);
			try
			{
				source.CopyTo(array);
				this.HashCore(array, 0, source.Length);
			}
			finally
			{
				Array.Clear(array, 0, source.Length);
				ArrayPool<byte>.Shared.Return(array, false);
			}
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x000A5D8C File Offset: 0x000A3F8C
		protected virtual bool TryHashFinal(Span<byte> destination, out int bytesWritten)
		{
			int num = this.HashSizeValue / 8;
			if (destination.Length < num)
			{
				bytesWritten = 0;
				return false;
			}
			byte[] array = this.HashFinal();
			if (array.Length == num)
			{
				new ReadOnlySpan<byte>(array).CopyTo(destination);
				bytesWritten = array.Length;
				return true;
			}
			throw new InvalidOperationException("The algorithm's implementation is incorrect.");
		}

		// Token: 0x04002114 RID: 8468
		private bool _disposed;

		// Token: 0x04002115 RID: 8469
		protected int HashSizeValue;

		// Token: 0x04002116 RID: 8470
		protected internal byte[] HashValue;

		// Token: 0x04002117 RID: 8471
		protected int State;
	}
}
