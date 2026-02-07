using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pathfinding.Ionic.Crc;
using Pathfinding.Ionic.Zlib;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000038 RID: 56
	public class ZipOutputStream : Stream
	{
		// Token: 0x0600027E RID: 638 RVA: 0x0000F6F4 File Offset: 0x0000D8F4
		public ZipOutputStream(Stream stream) : this(stream, false)
		{
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000F700 File Offset: 0x0000D900
		public ZipOutputStream(string fileName)
		{
			this._alternateEncoding = Encoding.UTF8;
			this._maxBufferPairs = 16;
			base..ctor();
			Stream stream = File.Open(fileName, 2, 3, 0);
			this._Init(stream, false, fileName);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000F73C File Offset: 0x0000D93C
		public ZipOutputStream(Stream stream, bool leaveOpen)
		{
			this._alternateEncoding = Encoding.UTF8;
			this._maxBufferPairs = 16;
			base..ctor();
			this._Init(stream, leaveOpen, null);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000F76C File Offset: 0x0000D96C
		private void _Init(Stream stream, bool leaveOpen, string name)
		{
			this._outputStream = ((!stream.CanRead) ? new CountingStream(stream) : stream);
			this.CompressionLevel = CompressionLevel.Default;
			this.CompressionMethod = CompressionMethod.Deflate;
			this._encryption = EncryptionAlgorithm.None;
			this._entriesWritten = new Dictionary<string, ZipEntry>(StringComparer.Ordinal);
			this._zip64 = Zip64Option.Default;
			this._leaveUnderlyingStreamOpen = leaveOpen;
			this.Strategy = CompressionStrategy.Default;
			this._name = (name ?? "(stream)");
			this.ParallelDeflateThreshold = -1L;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000F7EC File Offset: 0x0000D9EC
		public override string ToString()
		{
			return string.Format("ZipOutputStream::{0}(leaveOpen({1})))", this._name, this._leaveUnderlyingStreamOpen);
		}

		// Token: 0x17000091 RID: 145
		// (set) Token: 0x06000283 RID: 643 RVA: 0x0000F80C File Offset: 0x0000DA0C
		public string Password
		{
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._password = value;
				if (this._password == null)
				{
					this._encryption = EncryptionAlgorithm.None;
				}
				else if (this._encryption == EncryptionAlgorithm.None)
				{
					this._encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000F868 File Offset: 0x0000DA68
		// (set) Token: 0x06000285 RID: 645 RVA: 0x0000F870 File Offset: 0x0000DA70
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._encryption;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				if (value == EncryptionAlgorithm.Unsupported)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._encryption = value;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000F8B0 File Offset: 0x0000DAB0
		// (set) Token: 0x06000287 RID: 647 RVA: 0x0000F8B8 File Offset: 0x0000DAB8
		public int CodecBufferSize { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000F8C4 File Offset: 0x0000DAC4
		// (set) Token: 0x06000289 RID: 649 RVA: 0x0000F8CC File Offset: 0x0000DACC
		public CompressionStrategy Strategy { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000F8D8 File Offset: 0x0000DAD8
		// (set) Token: 0x0600028B RID: 651 RVA: 0x0000F8E0 File Offset: 0x0000DAE0
		public ZipEntryTimestamp Timestamp
		{
			get
			{
				return this._timestamp;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._timestamp = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000F914 File Offset: 0x0000DB14
		// (set) Token: 0x0600028D RID: 653 RVA: 0x0000F91C File Offset: 0x0000DB1C
		public CompressionLevel CompressionLevel { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000F928 File Offset: 0x0000DB28
		// (set) Token: 0x0600028F RID: 655 RVA: 0x0000F930 File Offset: 0x0000DB30
		public CompressionMethod CompressionMethod { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000F93C File Offset: 0x0000DB3C
		// (set) Token: 0x06000291 RID: 657 RVA: 0x0000F944 File Offset: 0x0000DB44
		public string Comment
		{
			get
			{
				return this._comment;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._comment = value;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000F978 File Offset: 0x0000DB78
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0000F980 File Offset: 0x0000DB80
		public Zip64Option EnableZip64
		{
			get
			{
				return this._zip64;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._zip64 = value;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000F9B4 File Offset: 0x0000DBB4
		public bool OutputUsedZip64
		{
			get
			{
				return this._anyEntriesUsedZip64 || this._directoryNeededZip64;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000F9CC File Offset: 0x0000DBCC
		// (set) Token: 0x06000296 RID: 662 RVA: 0x0000F9D8 File Offset: 0x0000DBD8
		public bool IgnoreCase
		{
			get
			{
				return !this._DontIgnoreCase;
			}
			set
			{
				this._DontIgnoreCase = !value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000F9E4 File Offset: 0x0000DBE4
		// (set) Token: 0x06000298 RID: 664 RVA: 0x0000FA04 File Offset: 0x0000DC04
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete. It will be removed in a future version of the library. Use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this._alternateEncoding == Encoding.UTF8 && this.AlternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this._alternateEncoding = Encoding.UTF8;
					this._alternateEncodingUsage = ZipOption.AsNecessary;
				}
				else
				{
					this._alternateEncoding = ZipOutputStream.DefaultEncoding;
					this._alternateEncodingUsage = ZipOption.Default;
				}
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000FA38 File Offset: 0x0000DC38
		// (set) Token: 0x0600029A RID: 666 RVA: 0x0000FA50 File Offset: 0x0000DC50
		[Obsolete("use AlternateEncoding and AlternateEncodingUsage instead.")]
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				if (this._alternateEncodingUsage == ZipOption.AsNecessary)
				{
					return this._alternateEncoding;
				}
				return null;
			}
			set
			{
				this._alternateEncoding = value;
				this._alternateEncodingUsage = ZipOption.AsNecessary;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000FA60 File Offset: 0x0000DC60
		// (set) Token: 0x0600029C RID: 668 RVA: 0x0000FA68 File Offset: 0x0000DC68
		public Encoding AlternateEncoding
		{
			get
			{
				return this._alternateEncoding;
			}
			set
			{
				this._alternateEncoding = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000FA74 File Offset: 0x0000DC74
		// (set) Token: 0x0600029E RID: 670 RVA: 0x0000FA7C File Offset: 0x0000DC7C
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				return this._alternateEncodingUsage;
			}
			set
			{
				this._alternateEncodingUsage = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000FA88 File Offset: 0x0000DC88
		public static Encoding DefaultEncoding
		{
			get
			{
				return Encoding.UTF8;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000FACC File Offset: 0x0000DCCC
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x0000FA90 File Offset: 0x0000DC90
		public long ParallelDeflateThreshold
		{
			get
			{
				return this._ParallelDeflateThreshold;
			}
			set
			{
				if (value != 0L && value != -1L && value < 65536L)
				{
					throw new ArgumentOutOfRangeException("value must be greater than 64k, or 0, or -1");
				}
				this._ParallelDeflateThreshold = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000FAD4 File Offset: 0x0000DCD4
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x0000FADC File Offset: 0x0000DCDC
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateMaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000FAFC File Offset: 0x0000DCFC
		private void InsureUniqueEntry(ZipEntry ze1)
		{
			if (this._entriesWritten.ContainsKey(ze1.FileName))
			{
				this._exceptionPending = true;
				throw new ArgumentException(string.Format("The entry '{0}' already exists in the zip archive.", ze1.FileName));
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000FB3C File Offset: 0x0000DD3C
		internal Stream OutputStream
		{
			get
			{
				return this._outputStream;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000FB44 File Offset: 0x0000DD44
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000FB4C File Offset: 0x0000DD4C
		public bool ContainsEntry(string name)
		{
			return this._entriesWritten.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000FB60 File Offset: 0x0000DD60
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			if (buffer == null)
			{
				this._exceptionPending = true;
				throw new ArgumentNullException("buffer");
			}
			if (this._currentEntry == null)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("You must call PutNextEntry() before calling Write().");
			}
			if (this._currentEntry.IsDirectory)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("You cannot Write() data for an entry that is a directory.");
			}
			if (this._needToWriteEntryHeader)
			{
				this._InitiateCurrentEntry(false);
			}
			if (count != 0)
			{
				this._entryOutputStream.Write(buffer, offset, count);
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000FC08 File Offset: 0x0000DE08
		public ZipEntry PutNextEntry(string entryName)
		{
			if (string.IsNullOrEmpty(entryName))
			{
				throw new ArgumentNullException("entryName");
			}
			if (this._disposed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			this._FinishCurrentEntry();
			this._currentEntry = ZipEntry.CreateForZipOutputStream(entryName);
			this._currentEntry._container = new ZipContainer(this);
			ZipEntry currentEntry = this._currentEntry;
			currentEntry._BitField |= 8;
			this._currentEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			this._currentEntry.CompressionLevel = this.CompressionLevel;
			this._currentEntry.CompressionMethod = this.CompressionMethod;
			this._currentEntry.Password = this._password;
			this._currentEntry.Encryption = this.Encryption;
			this._currentEntry.AlternateEncoding = this.AlternateEncoding;
			this._currentEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
			if (entryName.EndsWith("/"))
			{
				this._currentEntry.MarkAsDirectory();
			}
			this._currentEntry.EmitTimesInWindowsFormatWhenSaving = ((this._timestamp & ZipEntryTimestamp.Windows) != ZipEntryTimestamp.None);
			this._currentEntry.EmitTimesInUnixFormatWhenSaving = ((this._timestamp & ZipEntryTimestamp.Unix) != ZipEntryTimestamp.None);
			this.InsureUniqueEntry(this._currentEntry);
			this._needToWriteEntryHeader = true;
			return this._currentEntry;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000FD68 File Offset: 0x0000DF68
		private void _InitiateCurrentEntry(bool finishing)
		{
			this._entriesWritten.Add(this._currentEntry.FileName, this._currentEntry);
			this._entryCount++;
			if (this._entryCount > 65534 && this._zip64 == Zip64Option.Default)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Too many entries. Consider setting ZipOutputStream.EnableZip64.");
			}
			this._currentEntry.WriteHeader(this._outputStream, (!finishing) ? 0 : 99);
			this._currentEntry.StoreRelativeOffset();
			if (!this._currentEntry.IsDirectory)
			{
				this._currentEntry.WriteSecurityMetadata(this._outputStream);
				this._currentEntry.PrepOutputStream(this._outputStream, (!finishing) ? -1L : 0L, out this._outputCounter, out this._encryptor, out this._deflater, out this._entryOutputStream);
			}
			this._needToWriteEntryHeader = false;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000FE58 File Offset: 0x0000E058
		private void _FinishCurrentEntry()
		{
			if (this._currentEntry != null)
			{
				if (this._needToWriteEntryHeader)
				{
					this._InitiateCurrentEntry(true);
				}
				this._currentEntry.FinishOutputStream(this._outputStream, this._outputCounter, this._encryptor, this._deflater, this._entryOutputStream);
				this._currentEntry.PostProcessOutput(this._outputStream);
				if (this._currentEntry.OutputUsedZip64 != null)
				{
					this._anyEntriesUsedZip64 |= this._currentEntry.OutputUsedZip64.Value;
				}
				this._outputCounter = null;
				this._encryptor = (this._deflater = null);
				this._entryOutputStream = null;
			}
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000FF14 File Offset: 0x0000E114
		protected override void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing && !this._exceptionPending)
			{
				this._FinishCurrentEntry();
				this._directoryNeededZip64 = ZipOutput.WriteCentralDirectoryStructure(this._outputStream, this._entriesWritten.Values, 1U, this._zip64, this.Comment, new ZipContainer(this));
				CountingStream countingStream = this._outputStream as CountingStream;
				Stream stream;
				if (countingStream != null)
				{
					stream = countingStream.WrappedStream;
					countingStream.Dispose();
				}
				else
				{
					stream = this._outputStream;
				}
				if (!this._leaveUnderlyingStreamOpen)
				{
					stream.Dispose();
				}
				this._outputStream = null;
			}
			this._disposed = true;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000FFC0 File Offset: 0x0000E1C0
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000FFC4 File Offset: 0x0000E1C4
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000FFC8 File Offset: 0x0000E1C8
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000FFCC File Offset: 0x0000E1CC
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000FFD4 File Offset: 0x0000E1D4
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000FFE4 File Offset: 0x0000E1E4
		public override long Position
		{
			get
			{
				return this._outputStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000FFEC File Offset: 0x0000E1EC
		public override void Flush()
		{
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000FFF0 File Offset: 0x0000E1F0
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Read");
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000FFFC File Offset: 0x0000E1FC
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Seek");
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00010008 File Offset: 0x0000E208
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400013E RID: 318
		private EncryptionAlgorithm _encryption;

		// Token: 0x0400013F RID: 319
		private ZipEntryTimestamp _timestamp;

		// Token: 0x04000140 RID: 320
		internal string _password;

		// Token: 0x04000141 RID: 321
		private string _comment;

		// Token: 0x04000142 RID: 322
		private Stream _outputStream;

		// Token: 0x04000143 RID: 323
		private ZipEntry _currentEntry;

		// Token: 0x04000144 RID: 324
		internal Zip64Option _zip64;

		// Token: 0x04000145 RID: 325
		private Dictionary<string, ZipEntry> _entriesWritten;

		// Token: 0x04000146 RID: 326
		private int _entryCount;

		// Token: 0x04000147 RID: 327
		private ZipOption _alternateEncodingUsage;

		// Token: 0x04000148 RID: 328
		private Encoding _alternateEncoding;

		// Token: 0x04000149 RID: 329
		private bool _leaveUnderlyingStreamOpen;

		// Token: 0x0400014A RID: 330
		private bool _disposed;

		// Token: 0x0400014B RID: 331
		private bool _exceptionPending;

		// Token: 0x0400014C RID: 332
		private bool _anyEntriesUsedZip64;

		// Token: 0x0400014D RID: 333
		private bool _directoryNeededZip64;

		// Token: 0x0400014E RID: 334
		private CountingStream _outputCounter;

		// Token: 0x0400014F RID: 335
		private Stream _encryptor;

		// Token: 0x04000150 RID: 336
		private Stream _deflater;

		// Token: 0x04000151 RID: 337
		private CrcCalculatorStream _entryOutputStream;

		// Token: 0x04000152 RID: 338
		private bool _needToWriteEntryHeader;

		// Token: 0x04000153 RID: 339
		private string _name;

		// Token: 0x04000154 RID: 340
		private bool _DontIgnoreCase;

		// Token: 0x04000155 RID: 341
		internal ParallelDeflateOutputStream ParallelDeflater;

		// Token: 0x04000156 RID: 342
		private long _ParallelDeflateThreshold;

		// Token: 0x04000157 RID: 343
		private int _maxBufferPairs;
	}
}
