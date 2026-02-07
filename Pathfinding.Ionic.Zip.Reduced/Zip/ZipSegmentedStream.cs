using System;
using System.IO;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x0200003A RID: 58
	internal class ZipSegmentedStream : Stream
	{
		// Token: 0x060002C9 RID: 713 RVA: 0x00010364 File Offset: 0x0000E564
		private ZipSegmentedStream()
		{
			this._exceptionPending = false;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00010374 File Offset: 0x0000E574
		public static ZipSegmentedStream ForReading(string name, uint initialDiskNumber, uint maxDiskNumber)
		{
			ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream
			{
				rwMode = ZipSegmentedStream.RwMode.ReadOnly,
				CurrentSegment = initialDiskNumber,
				_maxDiskNumber = maxDiskNumber,
				_baseName = name
			};
			zipSegmentedStream._SetReadStream();
			return zipSegmentedStream;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x000103AC File Offset: 0x0000E5AC
		public static ZipSegmentedStream ForWriting(string name, int maxSegmentSize)
		{
			ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream
			{
				rwMode = ZipSegmentedStream.RwMode.Write,
				CurrentSegment = 0U,
				_baseName = name,
				_maxSegmentSize = maxSegmentSize,
				_baseDir = Path.GetDirectoryName(name)
			};
			if (zipSegmentedStream._baseDir == string.Empty)
			{
				zipSegmentedStream._baseDir = ".";
			}
			zipSegmentedStream._SetWriteStream(0U);
			return zipSegmentedStream;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00010414 File Offset: 0x0000E614
		public static Stream ForUpdate(string name, uint diskNumber)
		{
			if (diskNumber >= 99U)
			{
				throw new ArgumentOutOfRangeException("diskNumber");
			}
			string text = string.Format("{0}.z{1:D2}", Path.Combine(Path.GetDirectoryName(name), Path.GetFileNameWithoutExtension(name)), diskNumber + 1U);
			return File.Open(text, 3, 3, 0);
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002CD RID: 717 RVA: 0x00010464 File Offset: 0x0000E664
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0001046C File Offset: 0x0000E66C
		public bool ContiguousWrite { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002CF RID: 719 RVA: 0x00010478 File Offset: 0x0000E678
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x00010480 File Offset: 0x0000E680
		public uint CurrentSegment
		{
			get
			{
				return this._currentDiskNumber;
			}
			private set
			{
				this._currentDiskNumber = value;
				this._currentName = null;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x00010490 File Offset: 0x0000E690
		public string CurrentName
		{
			get
			{
				if (this._currentName == null)
				{
					this._currentName = this._NameForSegment(this.CurrentSegment);
				}
				return this._currentName;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x000104B8 File Offset: 0x0000E6B8
		public string CurrentTempName
		{
			get
			{
				return this._currentTempName;
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x000104C0 File Offset: 0x0000E6C0
		private string _NameForSegment(uint diskNumber)
		{
			if (diskNumber >= 99U)
			{
				this._exceptionPending = true;
				throw new OverflowException("The number of zip segments would exceed 99.");
			}
			return string.Format("{0}.z{1:D2}", Path.Combine(Path.GetDirectoryName(this._baseName), Path.GetFileNameWithoutExtension(this._baseName)), diskNumber + 1U);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00010514 File Offset: 0x0000E714
		public uint ComputeSegment(int length)
		{
			if (this._innerStream.Position + (long)length > (long)this._maxSegmentSize)
			{
				return this.CurrentSegment + 1U;
			}
			return this.CurrentSegment;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00010540 File Offset: 0x0000E740
		public override string ToString()
		{
			return string.Format("{0}[{1}][{2}], pos=0x{3:X})", new object[]
			{
				"ZipSegmentedStream",
				this.CurrentName,
				this.rwMode.ToString(),
				this.Position
			});
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00010590 File Offset: 0x0000E790
		private void _SetReadStream()
		{
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
			}
			if (this.CurrentSegment + 1U == this._maxDiskNumber)
			{
				this._currentName = this._baseName;
			}
			this._innerStream = File.OpenRead(this.CurrentName);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000105E4 File Offset: 0x0000E7E4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.ReadOnly)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Stream Error: Cannot Read.");
			}
			int num = this._innerStream.Read(buffer, offset, count);
			int num2 = num;
			while (num2 != count)
			{
				if (this._innerStream.Position != this._innerStream.Length)
				{
					this._exceptionPending = true;
					throw new ZipException(string.Format("Read error in file {0}", this.CurrentName));
				}
				if (this.CurrentSegment + 1U == this._maxDiskNumber)
				{
					return num;
				}
				this.CurrentSegment += 1U;
				this._SetReadStream();
				offset += num2;
				count -= num2;
				num2 = this._innerStream.Read(buffer, offset, count);
				num += num2;
			}
			return num;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x000106AC File Offset: 0x0000E8AC
		private void _SetWriteStream(uint increment)
		{
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
				if (File.Exists(this.CurrentName))
				{
					File.Delete(this.CurrentName);
				}
				File.Move(this._currentTempName, this.CurrentName);
			}
			if (increment > 0U)
			{
				this.CurrentSegment += increment;
			}
			SharedUtilities.CreateAndOpenUniqueTempFile(this._baseDir, out this._innerStream, out this._currentTempName);
			if (this.CurrentSegment == 0U)
			{
				this._innerStream.Write(BitConverter.GetBytes(134695760), 0, 4);
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0001074C File Offset: 0x0000E94C
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Stream Error: Cannot Write.");
			}
			if (this.ContiguousWrite)
			{
				if (this._innerStream.Position + (long)count > (long)this._maxSegmentSize)
				{
					this._SetWriteStream(1U);
				}
			}
			else
			{
				while (this._innerStream.Position + (long)count > (long)this._maxSegmentSize)
				{
					int num = this._maxSegmentSize - (int)this._innerStream.Position;
					this._innerStream.Write(buffer, offset, num);
					this._SetWriteStream(1U);
					count -= num;
					offset += num;
				}
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00010808 File Offset: 0x0000EA08
		public long TruncateBackward(uint diskNumber, long offset)
		{
			if (diskNumber >= 99U)
			{
				throw new ArgumentOutOfRangeException("diskNumber");
			}
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new ZipException("bad state.");
			}
			if (diskNumber == this.CurrentSegment)
			{
				return this._innerStream.Seek(offset, 0);
			}
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
				if (File.Exists(this._currentTempName))
				{
					File.Delete(this._currentTempName);
				}
			}
			for (uint num = this.CurrentSegment - 1U; num > diskNumber; num -= 1U)
			{
				string text = this._NameForSegment(num);
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
			this.CurrentSegment = diskNumber;
			for (int i = 0; i < 3; i++)
			{
				try
				{
					this._currentTempName = SharedUtilities.InternalGetTempFileName();
					File.Move(this.CurrentName, this._currentTempName);
					break;
				}
				catch (IOException)
				{
					if (i == 2)
					{
						throw;
					}
				}
			}
			this._innerStream = new FileStream(this._currentTempName, 3);
			return this._innerStream.Seek(offset, 0);
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002DB RID: 731 RVA: 0x00010954 File Offset: 0x0000EB54
		public override bool CanRead
		{
			get
			{
				return this.rwMode == ZipSegmentedStream.RwMode.ReadOnly && this._innerStream != null && this._innerStream.CanRead;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0001097C File Offset: 0x0000EB7C
		public override bool CanSeek
		{
			get
			{
				return this._innerStream != null && this._innerStream.CanSeek;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00010998 File Offset: 0x0000EB98
		public override bool CanWrite
		{
			get
			{
				return this.rwMode == ZipSegmentedStream.RwMode.Write && this._innerStream != null && this._innerStream.CanWrite;
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000109C0 File Offset: 0x0000EBC0
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002DF RID: 735 RVA: 0x000109D0 File Offset: 0x0000EBD0
		public override long Length
		{
			get
			{
				return this._innerStream.Length;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x000109E0 File Offset: 0x0000EBE0
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x000109F0 File Offset: 0x0000EBF0
		public override long Position
		{
			get
			{
				return this._innerStream.Position;
			}
			set
			{
				this._innerStream.Position = value;
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00010A00 File Offset: 0x0000EC00
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._innerStream.Seek(offset, origin);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00010A1C File Offset: 0x0000EC1C
		public override void SetLength(long value)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException();
			}
			this._innerStream.SetLength(value);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00010A44 File Offset: 0x0000EC44
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this._innerStream != null)
				{
					this._innerStream.Dispose();
					if (this.rwMode != ZipSegmentedStream.RwMode.Write || this._exceptionPending)
					{
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0400015F RID: 351
		private ZipSegmentedStream.RwMode rwMode;

		// Token: 0x04000160 RID: 352
		private bool _exceptionPending;

		// Token: 0x04000161 RID: 353
		private string _baseName;

		// Token: 0x04000162 RID: 354
		private string _baseDir;

		// Token: 0x04000163 RID: 355
		private string _currentName;

		// Token: 0x04000164 RID: 356
		private string _currentTempName;

		// Token: 0x04000165 RID: 357
		private uint _currentDiskNumber;

		// Token: 0x04000166 RID: 358
		private uint _maxDiskNumber;

		// Token: 0x04000167 RID: 359
		private int _maxSegmentSize;

		// Token: 0x04000168 RID: 360
		private Stream _innerStream;

		// Token: 0x0200003B RID: 59
		private enum RwMode
		{
			// Token: 0x0400016B RID: 363
			None,
			// Token: 0x0400016C RID: 364
			ReadOnly,
			// Token: 0x0400016D RID: 365
			Write
		}
	}
}
