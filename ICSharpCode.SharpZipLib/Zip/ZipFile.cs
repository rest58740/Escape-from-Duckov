using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Encryption;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000023 RID: 35
	public class ZipFile : IDisposable, IEnumerable
	{
		// Token: 0x0600013C RID: 316 RVA: 0x00009B38 File Offset: 0x00007D38
		public ZipFile(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.name_ = name;
			this.baseStream_ = File.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.isStreamOwner = true;
			try
			{
				this.ReadEntries();
			}
			catch
			{
				this.DisposeInternal(true);
				throw;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00009BCC File Offset: 0x00007DCC
		public ZipFile(FileStream file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			if (!file.CanSeek)
			{
				throw new ArgumentException("Stream is not seekable", "file");
			}
			this.baseStream_ = file;
			this.name_ = file.Name;
			this.isStreamOwner = true;
			try
			{
				this.ReadEntries();
			}
			catch
			{
				this.DisposeInternal(true);
				throw;
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00009C78 File Offset: 0x00007E78
		public ZipFile(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanSeek)
			{
				throw new ArgumentException("Stream is not seekable", "stream");
			}
			this.baseStream_ = stream;
			this.isStreamOwner = true;
			if (this.baseStream_.Length > 0L)
			{
				try
				{
					this.ReadEntries();
				}
				catch
				{
					this.DisposeInternal(true);
					throw;
				}
			}
			else
			{
				this.entries_ = new ZipEntry[0];
				this.isNewArchive_ = true;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00009D44 File Offset: 0x00007F44
		internal ZipFile()
		{
			this.entries_ = new ZipEntry[0];
			this.isNewArchive_ = true;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00009D88 File Offset: 0x00007F88
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00009D90 File Offset: 0x00007F90
		private void OnKeysRequired(string fileName)
		{
			if (this.KeysRequired != null)
			{
				KeysRequiredEventArgs keysRequiredEventArgs = new KeysRequiredEventArgs(fileName, this.key);
				this.KeysRequired(this, keysRequiredEventArgs);
				this.key = keysRequiredEventArgs.Key;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00009DD0 File Offset: 0x00007FD0
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00009DD8 File Offset: 0x00007FD8
		private byte[] Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00009DE4 File Offset: 0x00007FE4
		public string Password
		{
			set
			{
				if (value == null || value.Length == 0)
				{
					this.key = null;
				}
				else
				{
					this.rawPassword_ = value;
					this.key = PkzipClassic.GenerateKeys(ZipConstants.ConvertToArray(value));
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00009E1C File Offset: 0x0000801C
		private bool HaveKeys
		{
			get
			{
				return this.key != null;
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00009E2C File Offset: 0x0000802C
		~ZipFile()
		{
			this.Dispose(false);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00009E68 File Offset: 0x00008068
		public void Close()
		{
			this.DisposeInternal(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00009E78 File Offset: 0x00008078
		public static ZipFile Create(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			FileStream fileStream = File.Create(fileName);
			return new ZipFile
			{
				name_ = fileName,
				baseStream_ = fileStream,
				isStreamOwner = true
			};
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00009EBC File Offset: 0x000080BC
		public static ZipFile Create(Stream outStream)
		{
			if (outStream == null)
			{
				throw new ArgumentNullException("outStream");
			}
			if (!outStream.CanWrite)
			{
				throw new ArgumentException("Stream is not writeable", "outStream");
			}
			if (!outStream.CanSeek)
			{
				throw new ArgumentException("Stream is not seekable", "outStream");
			}
			return new ZipFile
			{
				baseStream_ = outStream
			};
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00009F20 File Offset: 0x00008120
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00009F28 File Offset: 0x00008128
		public bool IsStreamOwner
		{
			get
			{
				return this.isStreamOwner;
			}
			set
			{
				this.isStreamOwner = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00009F34 File Offset: 0x00008134
		public bool IsEmbeddedArchive
		{
			get
			{
				return this.offsetOfFirstEntry > 0L;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00009F40 File Offset: 0x00008140
		public bool IsNewArchive
		{
			get
			{
				return this.isNewArchive_;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00009F48 File Offset: 0x00008148
		public string ZipFileComment
		{
			get
			{
				return this.comment_;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00009F50 File Offset: 0x00008150
		public string Name
		{
			get
			{
				return this.name_;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00009F58 File Offset: 0x00008158
		[Obsolete("Use the Count property instead")]
		public int Size
		{
			get
			{
				return this.entries_.Length;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00009F64 File Offset: 0x00008164
		public long Count
		{
			get
			{
				return (long)this.entries_.Length;
			}
		}

		// Token: 0x1700004C RID: 76
		[IndexerName("EntryByIndex")]
		public ZipEntry this[int index]
		{
			get
			{
				return (ZipEntry)this.entries_[index].Clone();
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00009F84 File Offset: 0x00008184
		public IEnumerator GetEnumerator()
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			return new ZipFile.ZipEntryEnumerator(this.entries_);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00009FA8 File Offset: 0x000081A8
		public int FindEntry(string name, bool ignoreCase)
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			for (int i = 0; i < this.entries_.Length; i++)
			{
				if (string.Compare(name, this.entries_[i].Name, ignoreCase, CultureInfo.InvariantCulture) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000A008 File Offset: 0x00008208
		public ZipEntry GetEntry(string name)
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			int num = this.FindEntry(name, true);
			return (num < 0) ? null : ((ZipEntry)this.entries_[num].Clone());
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000A054 File Offset: 0x00008254
		public Stream GetInputStream(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			long num = entry.ZipFileIndex;
			if (num < 0L || num >= (long)this.entries_.Length || this.entries_[(int)(checked((IntPtr)num))].Name != entry.Name)
			{
				num = (long)this.FindEntry(entry.Name, true);
				if (num < 0L)
				{
					throw new ZipException("Entry cannot be found");
				}
			}
			return this.GetInputStream(num);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000A0F0 File Offset: 0x000082F0
		public Stream GetInputStream(long entryIndex)
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			checked
			{
				long start = this.LocateEntry(this.entries_[(int)((IntPtr)entryIndex)]);
				CompressionMethod compressionMethod = this.entries_[(int)((IntPtr)entryIndex)].CompressionMethod;
				Stream stream = new ZipFile.PartialInputStream(this, start, this.entries_[(int)((IntPtr)entryIndex)].CompressedSize);
				if (this.entries_[(int)((IntPtr)entryIndex)].IsCrypted)
				{
					stream = this.CreateAndInitDecryptionStream(stream, this.entries_[(int)((IntPtr)entryIndex)]);
					if (stream == null)
					{
						throw new ZipException("Unable to decrypt this entry");
					}
				}
				CompressionMethod compressionMethod2 = compressionMethod;
				if (compressionMethod2 != CompressionMethod.Stored)
				{
					if (compressionMethod2 != CompressionMethod.Deflated)
					{
						throw new ZipException("Unsupported compression method " + compressionMethod);
					}
					stream = new InflaterInputStream(stream, new Inflater(true));
				}
				return stream;
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000A1C0 File Offset: 0x000083C0
		public bool TestArchive(bool testData)
		{
			return this.TestArchive(testData, TestStrategy.FindFirstError, null);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000A1CC File Offset: 0x000083CC
		public bool TestArchive(bool testData, TestStrategy strategy, ZipTestResultHandler resultHandler)
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			TestStatus testStatus = new TestStatus(this);
			if (resultHandler != null)
			{
				resultHandler(testStatus, null);
			}
			ZipFile.HeaderTest tests = (!testData) ? ZipFile.HeaderTest.Header : (ZipFile.HeaderTest.Extract | ZipFile.HeaderTest.Header);
			bool flag = true;
			try
			{
				int num = 0;
				while (flag && (long)num < this.Count)
				{
					if (resultHandler != null)
					{
						testStatus.SetEntry(this[num]);
						testStatus.SetOperation(TestOperation.EntryHeader);
						resultHandler(testStatus, null);
					}
					try
					{
						this.TestLocalHeader(this[num], tests);
					}
					catch (ZipException ex)
					{
						testStatus.AddError();
						if (resultHandler != null)
						{
							resultHandler(testStatus, string.Format("Exception during test - '{0}'", ex.Message));
						}
						if (strategy == TestStrategy.FindFirstError)
						{
							flag = false;
						}
					}
					if (flag && testData && this[num].IsFile)
					{
						if (resultHandler != null)
						{
							testStatus.SetOperation(TestOperation.EntryData);
							resultHandler(testStatus, null);
						}
						Crc32 crc = new Crc32();
						using (Stream inputStream = this.GetInputStream(this[num]))
						{
							byte[] array = new byte[4096];
							long num2 = 0L;
							int num3;
							while ((num3 = inputStream.Read(array, 0, array.Length)) > 0)
							{
								crc.Update(array, 0, num3);
								if (resultHandler != null)
								{
									num2 += (long)num3;
									testStatus.SetBytesTested(num2);
									resultHandler(testStatus, null);
								}
							}
						}
						if (this[num].Crc != crc.Value)
						{
							testStatus.AddError();
							if (resultHandler != null)
							{
								resultHandler(testStatus, "CRC mismatch");
							}
							if (strategy == TestStrategy.FindFirstError)
							{
								flag = false;
							}
						}
						if ((this[num].Flags & 8) != 0)
						{
							ZipHelperStream zipHelperStream = new ZipHelperStream(this.baseStream_);
							DescriptorData descriptorData = new DescriptorData();
							zipHelperStream.ReadDataDescriptor(this[num].LocalHeaderRequiresZip64, descriptorData);
							if (this[num].Crc != descriptorData.Crc)
							{
								testStatus.AddError();
							}
							if (this[num].CompressedSize != descriptorData.CompressedSize)
							{
								testStatus.AddError();
							}
							if (this[num].Size != descriptorData.Size)
							{
								testStatus.AddError();
							}
						}
					}
					if (resultHandler != null)
					{
						testStatus.SetOperation(TestOperation.EntryComplete);
						resultHandler(testStatus, null);
					}
					num++;
				}
				if (resultHandler != null)
				{
					testStatus.SetOperation(TestOperation.MiscellaneousTests);
					resultHandler(testStatus, null);
				}
			}
			catch (Exception ex2)
			{
				testStatus.AddError();
				if (resultHandler != null)
				{
					resultHandler(testStatus, string.Format("Exception during test - '{0}'", ex2.Message));
				}
			}
			if (resultHandler != null)
			{
				testStatus.SetOperation(TestOperation.Complete);
				testStatus.SetEntry(null);
				resultHandler(testStatus, null);
			}
			return testStatus.ErrorCount == 0;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000A4E8 File Offset: 0x000086E8
		private long TestLocalHeader(ZipEntry entry, ZipFile.HeaderTest tests)
		{
			Stream obj = this.baseStream_;
			long result;
			lock (obj)
			{
				bool flag = (tests & ZipFile.HeaderTest.Header) != (ZipFile.HeaderTest)0;
				bool flag2 = (tests & ZipFile.HeaderTest.Extract) != (ZipFile.HeaderTest)0;
				this.baseStream_.Seek(this.offsetOfFirstEntry + entry.Offset, SeekOrigin.Begin);
				if (this.ReadLEUint() != 67324752U)
				{
					throw new ZipException(string.Format("Wrong local header signature @{0:X}", this.offsetOfFirstEntry + entry.Offset));
				}
				short num = (short)this.ReadLEUshort();
				short num2 = (short)this.ReadLEUshort();
				short num3 = (short)this.ReadLEUshort();
				short num4 = (short)this.ReadLEUshort();
				short num5 = (short)this.ReadLEUshort();
				uint num6 = this.ReadLEUint();
				long num7 = (long)((ulong)this.ReadLEUint());
				long num8 = (long)((ulong)this.ReadLEUint());
				int num9 = (int)this.ReadLEUshort();
				int num10 = (int)this.ReadLEUshort();
				byte[] array = new byte[num9];
				StreamUtils.ReadFully(this.baseStream_, array);
				byte[] array2 = new byte[num10];
				StreamUtils.ReadFully(this.baseStream_, array2);
				ZipExtraData zipExtraData = new ZipExtraData(array2);
				if (zipExtraData.Find(1))
				{
					num8 = zipExtraData.ReadLong();
					num7 = zipExtraData.ReadLong();
					if ((num2 & 8) != 0)
					{
						if (num8 != -1L && num8 != entry.Size)
						{
							throw new ZipException("Size invalid for descriptor");
						}
						if (num7 != -1L && num7 != entry.CompressedSize)
						{
							throw new ZipException("Compressed size invalid for descriptor");
						}
					}
				}
				else if (num >= 45 && ((uint)num8 == 4294967295U || (uint)num7 == 4294967295U))
				{
					throw new ZipException("Required Zip64 extended information missing");
				}
				if (flag2 && entry.IsFile)
				{
					if (!entry.IsCompressionMethodSupported())
					{
						throw new ZipException("Compression method not supported");
					}
					if (num > 51 || (num > 20 && num < 45))
					{
						throw new ZipException(string.Format("Version required to extract this entry not supported ({0})", num));
					}
					if ((num2 & 12384) != 0)
					{
						throw new ZipException("The library does not support the zip version required to extract this entry");
					}
				}
				if (flag)
				{
					if (num <= 63 && num != 10 && num != 11 && num != 20 && num != 21 && num != 25 && num != 27 && num != 45 && num != 46 && num != 50 && num != 51 && num != 52 && num != 61 && num != 62 && num != 63)
					{
						throw new ZipException(string.Format("Version required to extract this entry is invalid ({0})", num));
					}
					if (((int)num2 & 49168) != 0)
					{
						throw new ZipException("Reserved bit flags cannot be set.");
					}
					if ((num2 & 1) != 0 && num < 20)
					{
						throw new ZipException(string.Format("Version required to extract this entry is too low for encryption ({0})", num));
					}
					if ((num2 & 64) != 0)
					{
						if ((num2 & 1) == 0)
						{
							throw new ZipException("Strong encryption flag set but encryption flag is not set");
						}
						if (num < 50)
						{
							throw new ZipException(string.Format("Version required to extract this entry is too low for encryption ({0})", num));
						}
					}
					if ((num2 & 32) != 0 && num < 27)
					{
						throw new ZipException(string.Format("Patched data requires higher version than ({0})", num));
					}
					if ((int)num2 != entry.Flags)
					{
						throw new ZipException("Central header/local header flags mismatch");
					}
					if (entry.CompressionMethod != (CompressionMethod)num3)
					{
						throw new ZipException("Central header/local header compression method mismatch");
					}
					if (entry.Version != (int)num)
					{
						throw new ZipException("Extract version mismatch");
					}
					if ((num2 & 64) != 0 && num < 62)
					{
						throw new ZipException("Strong encryption flag set but version not high enough");
					}
					if ((num2 & 8192) != 0 && (num4 != 0 || num5 != 0))
					{
						throw new ZipException("Header masked set but date/time values non-zero");
					}
					if ((num2 & 8) == 0 && num6 != (uint)entry.Crc)
					{
						throw new ZipException("Central header/local header crc mismatch");
					}
					if (num8 == 0L && num7 == 0L && num6 != 0U)
					{
						throw new ZipException("Invalid CRC for empty entry");
					}
					if (entry.Name.Length > num9)
					{
						throw new ZipException("File name length mismatch");
					}
					string text = ZipConstants.ConvertToStringExt((int)num2, array);
					if (text != entry.Name)
					{
						throw new ZipException("Central header and local header file name mismatch");
					}
					if (entry.IsDirectory)
					{
						if (num8 > 0L)
						{
							throw new ZipException("Directory cannot have size");
						}
						if (entry.IsCrypted)
						{
							if (num7 > 14L)
							{
								throw new ZipException("Directory compressed size invalid");
							}
						}
						else if (num7 > 2L)
						{
							throw new ZipException("Directory compressed size invalid");
						}
					}
					if (!ZipNameTransform.IsValidName(text, true))
					{
						throw new ZipException("Name is invalid");
					}
				}
				if ((num2 & 8) == 0 || num8 > 0L || num7 > 0L)
				{
					if (num8 != entry.Size)
					{
						throw new ZipException(string.Format("Size mismatch between central header({0}) and local header({1})", entry.Size, num8));
					}
					if (num7 != entry.CompressedSize && num7 != (long)((ulong)-1) && num7 != -1L)
					{
						throw new ZipException(string.Format("Compressed size mismatch between central header({0}) and local header({1})", entry.CompressedSize, num7));
					}
				}
				int num11 = num9 + num10;
				result = this.offsetOfFirstEntry + entry.Offset + 30L + (long)num11;
			}
			return result;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000AA80 File Offset: 0x00008C80
		// (set) Token: 0x0600015C RID: 348 RVA: 0x0000AA90 File Offset: 0x00008C90
		public INameTransform NameTransform
		{
			get
			{
				return this.updateEntryFactory_.NameTransform;
			}
			set
			{
				this.updateEntryFactory_.NameTransform = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600015D RID: 349 RVA: 0x0000AAA0 File Offset: 0x00008CA0
		// (set) Token: 0x0600015E RID: 350 RVA: 0x0000AAA8 File Offset: 0x00008CA8
		public IEntryFactory EntryFactory
		{
			get
			{
				return this.updateEntryFactory_;
			}
			set
			{
				if (value == null)
				{
					this.updateEntryFactory_ = new ZipEntryFactory();
				}
				else
				{
					this.updateEntryFactory_ = value;
				}
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000AAC8 File Offset: 0x00008CC8
		// (set) Token: 0x06000160 RID: 352 RVA: 0x0000AAD0 File Offset: 0x00008CD0
		public int BufferSize
		{
			get
			{
				return this.bufferSize_;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("value", "cannot be below 1024");
				}
				if (this.bufferSize_ != value)
				{
					this.bufferSize_ = value;
					this.copyBuffer_ = null;
				}
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000AB08 File Offset: 0x00008D08
		public bool IsUpdating
		{
			get
			{
				return this.updates_ != null;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000AB18 File Offset: 0x00008D18
		// (set) Token: 0x06000163 RID: 355 RVA: 0x0000AB20 File Offset: 0x00008D20
		public UseZip64 UseZip64
		{
			get
			{
				return this.useZip64_;
			}
			set
			{
				this.useZip64_ = value;
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000AB2C File Offset: 0x00008D2C
		public void BeginUpdate(IArchiveStorage archiveStorage, IDynamicDataSource dataSource)
		{
			if (archiveStorage == null)
			{
				throw new ArgumentNullException("archiveStorage");
			}
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			if (this.IsEmbeddedArchive)
			{
				throw new ZipException("Cannot update embedded/SFX archives");
			}
			this.archiveStorage_ = archiveStorage;
			this.updateDataSource_ = dataSource;
			this.updateIndex_ = new Hashtable();
			this.updates_ = new ArrayList(this.entries_.Length);
			foreach (ZipEntry zipEntry in this.entries_)
			{
				int num = this.updates_.Add(new ZipFile.ZipUpdate(zipEntry));
				this.updateIndex_.Add(zipEntry.Name, num);
			}
			this.updates_.Sort(new ZipFile.UpdateComparer());
			int num2 = 0;
			foreach (object obj in this.updates_)
			{
				ZipFile.ZipUpdate zipUpdate = (ZipFile.ZipUpdate)obj;
				if (num2 == this.updates_.Count - 1)
				{
					break;
				}
				zipUpdate.OffsetBasedSize = ((ZipFile.ZipUpdate)this.updates_[num2 + 1]).Entry.Offset - zipUpdate.Entry.Offset;
				num2++;
			}
			this.updateCount_ = (long)this.updates_.Count;
			this.contentsEdited_ = false;
			this.commentEdited_ = false;
			this.newComment_ = null;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000ACEC File Offset: 0x00008EEC
		public void BeginUpdate(IArchiveStorage archiveStorage)
		{
			this.BeginUpdate(archiveStorage, new DynamicDiskDataSource());
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000ACFC File Offset: 0x00008EFC
		public void BeginUpdate()
		{
			if (this.Name == null)
			{
				this.BeginUpdate(new MemoryArchiveStorage(), new DynamicDiskDataSource());
			}
			else
			{
				this.BeginUpdate(new DiskArchiveStorage(this), new DynamicDiskDataSource());
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000AD30 File Offset: 0x00008F30
		public void CommitUpdate()
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			this.CheckUpdating();
			try
			{
				this.updateIndex_.Clear();
				this.updateIndex_ = null;
				if (this.contentsEdited_)
				{
					this.RunUpdates();
				}
				else if (this.commentEdited_)
				{
					this.UpdateCommentOnly();
				}
				else if (this.entries_.Length == 0)
				{
					byte[] comment = (this.newComment_ == null) ? ZipConstants.ConvertToArray(this.comment_) : this.newComment_.RawComment;
					using (ZipHelperStream zipHelperStream = new ZipHelperStream(this.baseStream_))
					{
						zipHelperStream.WriteEndOfCentralDirectory(0L, 0L, 0L, comment);
					}
				}
			}
			finally
			{
				this.PostUpdateCleanup();
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000AE38 File Offset: 0x00009038
		public void AbortUpdate()
		{
			this.PostUpdateCleanup();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000AE40 File Offset: 0x00009040
		public void SetComment(string comment)
		{
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			this.CheckUpdating();
			this.newComment_ = new ZipFile.ZipString(comment);
			if (this.newComment_.RawLength > 65535)
			{
				this.newComment_ = null;
				throw new ZipException("Comment length exceeds maximum - 65535");
			}
			this.commentEdited_ = true;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000AEA4 File Offset: 0x000090A4
		private void AddUpdate(ZipFile.ZipUpdate update)
		{
			this.contentsEdited_ = true;
			int num = this.FindExistingUpdate(update.Entry.Name);
			if (num >= 0)
			{
				if (this.updates_[num] == null)
				{
					this.updateCount_ += 1L;
				}
				this.updates_[num] = update;
			}
			else
			{
				num = this.updates_.Add(update);
				this.updateCount_ += 1L;
				this.updateIndex_.Add(update.Entry.Name, num);
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000AF3C File Offset: 0x0000913C
		public void Add(string fileName, CompressionMethod compressionMethod, bool useUnicodeText)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (this.isDisposed_)
			{
				throw new ObjectDisposedException("ZipFile");
			}
			if (!ZipEntry.IsCompressionMethodSupported(compressionMethod))
			{
				throw new ArgumentOutOfRangeException("compressionMethod");
			}
			this.CheckUpdating();
			this.contentsEdited_ = true;
			ZipEntry zipEntry = this.EntryFactory.MakeFileEntry(fileName);
			zipEntry.IsUnicodeText = useUnicodeText;
			zipEntry.CompressionMethod = compressionMethod;
			this.AddUpdate(new ZipFile.ZipUpdate(fileName, zipEntry));
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000AFBC File Offset: 0x000091BC
		public void Add(string fileName, CompressionMethod compressionMethod)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (!ZipEntry.IsCompressionMethodSupported(compressionMethod))
			{
				throw new ArgumentOutOfRangeException("compressionMethod");
			}
			this.CheckUpdating();
			this.contentsEdited_ = true;
			ZipEntry zipEntry = this.EntryFactory.MakeFileEntry(fileName);
			zipEntry.CompressionMethod = compressionMethod;
			this.AddUpdate(new ZipFile.ZipUpdate(fileName, zipEntry));
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000B020 File Offset: 0x00009220
		public void Add(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			this.CheckUpdating();
			this.AddUpdate(new ZipFile.ZipUpdate(fileName, this.EntryFactory.MakeFileEntry(fileName)));
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000B05C File Offset: 0x0000925C
		public void Add(string fileName, string entryName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (entryName == null)
			{
				throw new ArgumentNullException("entryName");
			}
			this.CheckUpdating();
			this.AddUpdate(new ZipFile.ZipUpdate(fileName, this.EntryFactory.MakeFileEntry(fileName, entryName, true)));
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000B0AC File Offset: 0x000092AC
		public void Add(string fileName, ZipEntry entry)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.CheckUpdating();
			this.AddUpdate(new ZipFile.ZipUpdate(fileName, entry));
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000B0E4 File Offset: 0x000092E4
		public void Add(IStaticDataSource dataSource, string entryName)
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			if (entryName == null)
			{
				throw new ArgumentNullException("entryName");
			}
			this.CheckUpdating();
			this.AddUpdate(new ZipFile.ZipUpdate(dataSource, this.EntryFactory.MakeFileEntry(entryName, false)));
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000B134 File Offset: 0x00009334
		public void Add(IStaticDataSource dataSource, string entryName, CompressionMethod compressionMethod)
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			if (entryName == null)
			{
				throw new ArgumentNullException("entryName");
			}
			this.CheckUpdating();
			ZipEntry zipEntry = this.EntryFactory.MakeFileEntry(entryName, false);
			zipEntry.CompressionMethod = compressionMethod;
			this.AddUpdate(new ZipFile.ZipUpdate(dataSource, zipEntry));
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000B18C File Offset: 0x0000938C
		public void Add(IStaticDataSource dataSource, string entryName, CompressionMethod compressionMethod, bool useUnicodeText)
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			if (entryName == null)
			{
				throw new ArgumentNullException("entryName");
			}
			this.CheckUpdating();
			ZipEntry zipEntry = this.EntryFactory.MakeFileEntry(entryName, false);
			zipEntry.IsUnicodeText = useUnicodeText;
			zipEntry.CompressionMethod = compressionMethod;
			this.AddUpdate(new ZipFile.ZipUpdate(dataSource, zipEntry));
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000B1EC File Offset: 0x000093EC
		public void Add(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.CheckUpdating();
			if (entry.Size != 0L || entry.CompressedSize != 0L)
			{
				throw new ZipException("Entry cannot have any data");
			}
			this.AddUpdate(new ZipFile.ZipUpdate(ZipFile.UpdateCommand.Add, entry));
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000B240 File Offset: 0x00009440
		public void AddDirectory(string directoryName)
		{
			if (directoryName == null)
			{
				throw new ArgumentNullException("directoryName");
			}
			this.CheckUpdating();
			ZipEntry entry = this.EntryFactory.MakeDirectoryEntry(directoryName);
			this.AddUpdate(new ZipFile.ZipUpdate(ZipFile.UpdateCommand.Add, entry));
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000B280 File Offset: 0x00009480
		public bool Delete(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			this.CheckUpdating();
			int num = this.FindExistingUpdate(fileName);
			if (num >= 0 && this.updates_[num] != null)
			{
				bool result = true;
				this.contentsEdited_ = true;
				this.updates_[num] = null;
				this.updateCount_ -= 1L;
				return result;
			}
			throw new ZipException("Cannot find entry to delete");
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000B2FC File Offset: 0x000094FC
		public void Delete(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.CheckUpdating();
			int num = this.FindExistingUpdate(entry);
			if (num >= 0)
			{
				this.contentsEdited_ = true;
				this.updates_[num] = null;
				this.updateCount_ -= 1L;
				return;
			}
			throw new ZipException("Cannot find entry to delete");
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000B364 File Offset: 0x00009564
		private void WriteLEShort(int value)
		{
			this.baseStream_.WriteByte((byte)(value & 255));
			this.baseStream_.WriteByte((byte)(value >> 8 & 255));
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000B39C File Offset: 0x0000959C
		private void WriteLEUshort(ushort value)
		{
			this.baseStream_.WriteByte((byte)(value & 255));
			this.baseStream_.WriteByte((byte)(value >> 8));
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000B3CC File Offset: 0x000095CC
		private void WriteLEInt(int value)
		{
			this.WriteLEShort(value & 65535);
			this.WriteLEShort(value >> 16);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000B3E8 File Offset: 0x000095E8
		private void WriteLEUint(uint value)
		{
			this.WriteLEUshort((ushort)(value & 65535U));
			this.WriteLEUshort((ushort)(value >> 16));
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000B404 File Offset: 0x00009604
		private void WriteLeLong(long value)
		{
			this.WriteLEInt((int)(value & (long)((ulong)-1)));
			this.WriteLEInt((int)(value >> 32));
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000B41C File Offset: 0x0000961C
		private void WriteLEUlong(ulong value)
		{
			this.WriteLEUint((uint)(value & (ulong)-1));
			this.WriteLEUint((uint)(value >> 32));
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000B434 File Offset: 0x00009634
		private void WriteLocalEntryHeader(ZipFile.ZipUpdate update)
		{
			ZipEntry outEntry = update.OutEntry;
			outEntry.Offset = this.baseStream_.Position;
			if (update.Command != ZipFile.UpdateCommand.Copy)
			{
				if (outEntry.CompressionMethod == CompressionMethod.Deflated)
				{
					if (outEntry.Size == 0L)
					{
						outEntry.CompressedSize = outEntry.Size;
						outEntry.Crc = 0L;
						outEntry.CompressionMethod = CompressionMethod.Stored;
					}
				}
				else if (outEntry.CompressionMethod == CompressionMethod.Stored)
				{
					outEntry.Flags &= -9;
				}
				if (this.HaveKeys)
				{
					outEntry.IsCrypted = true;
					if (outEntry.Crc < 0L)
					{
						outEntry.Flags |= 8;
					}
				}
				else
				{
					outEntry.IsCrypted = false;
				}
				switch (this.useZip64_)
				{
				case UseZip64.On:
					outEntry.ForceZip64();
					break;
				case UseZip64.Dynamic:
					if (outEntry.Size < 0L)
					{
						outEntry.ForceZip64();
					}
					break;
				}
			}
			this.WriteLEInt(67324752);
			this.WriteLEShort(outEntry.Version);
			this.WriteLEShort(outEntry.Flags);
			this.WriteLEShort((int)((byte)outEntry.CompressionMethod));
			this.WriteLEInt((int)outEntry.DosTime);
			if (!outEntry.HasCrc)
			{
				update.CrcPatchOffset = this.baseStream_.Position;
				this.WriteLEInt(0);
			}
			else
			{
				this.WriteLEInt((int)outEntry.Crc);
			}
			if (outEntry.LocalHeaderRequiresZip64)
			{
				this.WriteLEInt(-1);
				this.WriteLEInt(-1);
			}
			else
			{
				if (outEntry.CompressedSize < 0L || outEntry.Size < 0L)
				{
					update.SizePatchOffset = this.baseStream_.Position;
				}
				this.WriteLEInt((int)outEntry.CompressedSize);
				this.WriteLEInt((int)outEntry.Size);
			}
			byte[] array = ZipConstants.ConvertToArray(outEntry.Flags, outEntry.Name);
			if (array.Length > 65535)
			{
				throw new ZipException("Entry name too long.");
			}
			ZipExtraData zipExtraData = new ZipExtraData(outEntry.ExtraData);
			if (outEntry.LocalHeaderRequiresZip64)
			{
				zipExtraData.StartNewEntry();
				zipExtraData.AddLeLong(outEntry.Size);
				zipExtraData.AddLeLong(outEntry.CompressedSize);
				zipExtraData.AddNewEntry(1);
			}
			else
			{
				zipExtraData.Delete(1);
			}
			outEntry.ExtraData = zipExtraData.GetEntryData();
			this.WriteLEShort(array.Length);
			this.WriteLEShort(outEntry.ExtraData.Length);
			if (array.Length > 0)
			{
				this.baseStream_.Write(array, 0, array.Length);
			}
			if (outEntry.LocalHeaderRequiresZip64)
			{
				if (!zipExtraData.Find(1))
				{
					throw new ZipException("Internal error cannot find extra data");
				}
				update.SizePatchOffset = this.baseStream_.Position + (long)zipExtraData.CurrentReadIndex;
			}
			if (outEntry.ExtraData.Length > 0)
			{
				this.baseStream_.Write(outEntry.ExtraData, 0, outEntry.ExtraData.Length);
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000B71C File Offset: 0x0000991C
		private int WriteCentralDirectoryHeader(ZipEntry entry)
		{
			if (entry.CompressedSize < 0L)
			{
				throw new ZipException("Attempt to write central directory entry with unknown csize");
			}
			if (entry.Size < 0L)
			{
				throw new ZipException("Attempt to write central directory entry with unknown size");
			}
			if (entry.Crc < 0L)
			{
				throw new ZipException("Attempt to write central directory entry with unknown crc");
			}
			this.WriteLEInt(33639248);
			this.WriteLEShort(51);
			this.WriteLEShort(entry.Version);
			this.WriteLEShort(entry.Flags);
			this.WriteLEShort((int)((byte)entry.CompressionMethod));
			this.WriteLEInt((int)entry.DosTime);
			this.WriteLEInt((int)entry.Crc);
			if (entry.IsZip64Forced() || entry.CompressedSize >= (long)((ulong)-1))
			{
				this.WriteLEInt(-1);
			}
			else
			{
				this.WriteLEInt((int)(entry.CompressedSize & (long)((ulong)-1)));
			}
			if (entry.IsZip64Forced() || entry.Size >= (long)((ulong)-1))
			{
				this.WriteLEInt(-1);
			}
			else
			{
				this.WriteLEInt((int)entry.Size);
			}
			byte[] array = ZipConstants.ConvertToArray(entry.Flags, entry.Name);
			if (array.Length > 65535)
			{
				throw new ZipException("Entry name is too long.");
			}
			this.WriteLEShort(array.Length);
			ZipExtraData zipExtraData = new ZipExtraData(entry.ExtraData);
			if (entry.CentralHeaderRequiresZip64)
			{
				zipExtraData.StartNewEntry();
				if (entry.Size >= (long)((ulong)-1) || this.useZip64_ == UseZip64.On)
				{
					zipExtraData.AddLeLong(entry.Size);
				}
				if (entry.CompressedSize >= (long)((ulong)-1) || this.useZip64_ == UseZip64.On)
				{
					zipExtraData.AddLeLong(entry.CompressedSize);
				}
				if (entry.Offset >= (long)((ulong)-1))
				{
					zipExtraData.AddLeLong(entry.Offset);
				}
				zipExtraData.AddNewEntry(1);
			}
			else
			{
				zipExtraData.Delete(1);
			}
			byte[] entryData = zipExtraData.GetEntryData();
			this.WriteLEShort(entryData.Length);
			this.WriteLEShort((entry.Comment == null) ? 0 : entry.Comment.Length);
			this.WriteLEShort(0);
			this.WriteLEShort(0);
			if (entry.ExternalFileAttributes != -1)
			{
				this.WriteLEInt(entry.ExternalFileAttributes);
			}
			else if (entry.IsDirectory)
			{
				this.WriteLEUint(16U);
			}
			else
			{
				this.WriteLEUint(0U);
			}
			if (entry.Offset >= (long)((ulong)-1))
			{
				this.WriteLEUint(uint.MaxValue);
			}
			else
			{
				this.WriteLEUint((uint)((int)entry.Offset));
			}
			if (array.Length > 0)
			{
				this.baseStream_.Write(array, 0, array.Length);
			}
			if (entryData.Length > 0)
			{
				this.baseStream_.Write(entryData, 0, entryData.Length);
			}
			byte[] array2 = (entry.Comment == null) ? new byte[0] : Encoding.ASCII.GetBytes(entry.Comment);
			if (array2.Length > 0)
			{
				this.baseStream_.Write(array2, 0, array2.Length);
			}
			return 46 + array.Length + entryData.Length + array2.Length;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000BA14 File Offset: 0x00009C14
		private void PostUpdateCleanup()
		{
			this.updateDataSource_ = null;
			this.updates_ = null;
			this.updateIndex_ = null;
			if (this.archiveStorage_ != null)
			{
				this.archiveStorage_.Dispose();
				this.archiveStorage_ = null;
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000BA54 File Offset: 0x00009C54
		private string GetTransformedFileName(string name)
		{
			INameTransform nameTransform = this.NameTransform;
			return (nameTransform == null) ? name : nameTransform.TransformFile(name);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000BA7C File Offset: 0x00009C7C
		private string GetTransformedDirectoryName(string name)
		{
			INameTransform nameTransform = this.NameTransform;
			return (nameTransform == null) ? name : nameTransform.TransformDirectory(name);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000BAA4 File Offset: 0x00009CA4
		private byte[] GetBuffer()
		{
			if (this.copyBuffer_ == null)
			{
				this.copyBuffer_ = new byte[this.bufferSize_];
			}
			return this.copyBuffer_;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000BAD4 File Offset: 0x00009CD4
		private void CopyDescriptorBytes(ZipFile.ZipUpdate update, Stream dest, Stream source)
		{
			int i = this.GetDescriptorSize(update);
			if (i > 0)
			{
				byte[] buffer = this.GetBuffer();
				while (i > 0)
				{
					int count = Math.Min(buffer.Length, i);
					int num = source.Read(buffer, 0, count);
					if (num <= 0)
					{
						throw new ZipException("Unxpected end of stream");
					}
					dest.Write(buffer, 0, num);
					i -= num;
				}
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000BB3C File Offset: 0x00009D3C
		private void CopyBytes(ZipFile.ZipUpdate update, Stream destination, Stream source, long bytesToCopy, bool updateCrc)
		{
			if (destination == source)
			{
				throw new InvalidOperationException("Destination and source are the same");
			}
			Crc32 crc = new Crc32();
			byte[] buffer = this.GetBuffer();
			long num = bytesToCopy;
			long num2 = 0L;
			int num4;
			do
			{
				int num3 = buffer.Length;
				if (bytesToCopy < (long)num3)
				{
					num3 = (int)bytesToCopy;
				}
				num4 = source.Read(buffer, 0, num3);
				if (num4 > 0)
				{
					if (updateCrc)
					{
						crc.Update(buffer, 0, num4);
					}
					destination.Write(buffer, 0, num4);
					bytesToCopy -= (long)num4;
					num2 += (long)num4;
				}
			}
			while (num4 > 0 && bytesToCopy > 0L);
			if (num2 != num)
			{
				throw new ZipException(string.Format("Failed to copy bytes expected {0} read {1}", num, num2));
			}
			if (updateCrc)
			{
				update.OutEntry.Crc = crc.Value;
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000BC0C File Offset: 0x00009E0C
		private int GetDescriptorSize(ZipFile.ZipUpdate update)
		{
			int result = 0;
			if ((update.Entry.Flags & 8) != 0)
			{
				result = 12;
				if (update.Entry.LocalHeaderRequiresZip64)
				{
					result = 20;
				}
			}
			return result;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000BC44 File Offset: 0x00009E44
		private void CopyDescriptorBytesDirect(ZipFile.ZipUpdate update, Stream stream, ref long destinationPosition, long sourcePosition)
		{
			int i = this.GetDescriptorSize(update);
			while (i > 0)
			{
				int count = i;
				byte[] buffer = this.GetBuffer();
				stream.Position = sourcePosition;
				int num = stream.Read(buffer, 0, count);
				if (num <= 0)
				{
					throw new ZipException("Unxpected end of stream");
				}
				stream.Position = destinationPosition;
				stream.Write(buffer, 0, num);
				i -= num;
				destinationPosition += (long)num;
				sourcePosition += (long)num;
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000BCBC File Offset: 0x00009EBC
		private void CopyEntryDataDirect(ZipFile.ZipUpdate update, Stream stream, bool updateCrc, ref long destinationPosition, ref long sourcePosition)
		{
			long num = update.Entry.CompressedSize;
			Crc32 crc = new Crc32();
			byte[] buffer = this.GetBuffer();
			long num2 = num;
			long num3 = 0L;
			int num5;
			do
			{
				int num4 = buffer.Length;
				if (num < (long)num4)
				{
					num4 = (int)num;
				}
				stream.Position = sourcePosition;
				num5 = stream.Read(buffer, 0, num4);
				if (num5 > 0)
				{
					if (updateCrc)
					{
						crc.Update(buffer, 0, num5);
					}
					stream.Position = destinationPosition;
					stream.Write(buffer, 0, num5);
					destinationPosition += (long)num5;
					sourcePosition += (long)num5;
					num -= (long)num5;
					num3 += (long)num5;
				}
			}
			while (num5 > 0 && num > 0L);
			if (num3 != num2)
			{
				throw new ZipException(string.Format("Failed to copy bytes expected {0} read {1}", num2, num3));
			}
			if (updateCrc)
			{
				update.OutEntry.Crc = crc.Value;
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000BDA8 File Offset: 0x00009FA8
		private int FindExistingUpdate(ZipEntry entry)
		{
			int result = -1;
			string transformedFileName = this.GetTransformedFileName(entry.Name);
			if (this.updateIndex_.ContainsKey(transformedFileName))
			{
				result = (int)this.updateIndex_[transformedFileName];
			}
			return result;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000BDE8 File Offset: 0x00009FE8
		private int FindExistingUpdate(string fileName)
		{
			int result = -1;
			string transformedFileName = this.GetTransformedFileName(fileName);
			if (this.updateIndex_.ContainsKey(transformedFileName))
			{
				result = (int)this.updateIndex_[transformedFileName];
			}
			return result;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000BE24 File Offset: 0x0000A024
		private Stream GetOutputStream(ZipEntry entry)
		{
			Stream stream = this.baseStream_;
			if (entry.IsCrypted)
			{
				stream = this.CreateAndInitEncryptionStream(stream, entry);
			}
			CompressionMethod compressionMethod = entry.CompressionMethod;
			if (compressionMethod != CompressionMethod.Stored)
			{
				if (compressionMethod != CompressionMethod.Deflated)
				{
					throw new ZipException("Unknown compression method " + entry.CompressionMethod);
				}
				stream = new DeflaterOutputStream(stream, new Deflater(9, true))
				{
					IsStreamOwner = false
				};
			}
			else
			{
				stream = new ZipFile.UncompressedStream(stream);
			}
			return stream;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000BEAC File Offset: 0x0000A0AC
		private void AddEntry(ZipFile workFile, ZipFile.ZipUpdate update)
		{
			Stream stream = null;
			if (update.Entry.IsFile)
			{
				stream = update.GetSource();
				if (stream == null)
				{
					stream = this.updateDataSource_.GetSource(update.Entry, update.Filename);
				}
			}
			if (stream != null)
			{
				using (stream)
				{
					long length = stream.Length;
					if (update.OutEntry.Size < 0L)
					{
						update.OutEntry.Size = length;
					}
					else if (update.OutEntry.Size != length)
					{
						throw new ZipException("Entry size/stream size mismatch");
					}
					workFile.WriteLocalEntryHeader(update);
					long position = workFile.baseStream_.Position;
					using (Stream outputStream = workFile.GetOutputStream(update.OutEntry))
					{
						this.CopyBytes(update, outputStream, stream, length, true);
					}
					long position2 = workFile.baseStream_.Position;
					update.OutEntry.CompressedSize = position2 - position;
					if ((update.OutEntry.Flags & 8) == 8)
					{
						ZipHelperStream zipHelperStream = new ZipHelperStream(workFile.baseStream_);
						zipHelperStream.WriteDataDescriptor(update.OutEntry);
					}
				}
			}
			else
			{
				workFile.WriteLocalEntryHeader(update);
				update.OutEntry.CompressedSize = 0L;
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000C028 File Offset: 0x0000A228
		private void ModifyEntry(ZipFile workFile, ZipFile.ZipUpdate update)
		{
			workFile.WriteLocalEntryHeader(update);
			long position = workFile.baseStream_.Position;
			if (update.Entry.IsFile && update.Filename != null)
			{
				using (Stream outputStream = workFile.GetOutputStream(update.OutEntry))
				{
					using (Stream inputStream = this.GetInputStream(update.Entry))
					{
						this.CopyBytes(update, outputStream, inputStream, inputStream.Length, true);
					}
				}
			}
			long position2 = workFile.baseStream_.Position;
			update.Entry.CompressedSize = position2 - position;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000C100 File Offset: 0x0000A300
		private void CopyEntryDirect(ZipFile workFile, ZipFile.ZipUpdate update, ref long destinationPosition)
		{
			bool flag = false;
			if (update.Entry.Offset == destinationPosition)
			{
				flag = true;
			}
			if (!flag)
			{
				this.baseStream_.Position = destinationPosition;
				workFile.WriteLocalEntryHeader(update);
				destinationPosition = this.baseStream_.Position;
			}
			long num = 0L;
			long num2 = update.Entry.Offset + 26L;
			this.baseStream_.Seek(num2, SeekOrigin.Begin);
			uint num3 = (uint)this.ReadLEUshort();
			uint num4 = (uint)this.ReadLEUshort();
			num = this.baseStream_.Position + (long)((ulong)num3) + (long)((ulong)num4);
			if (flag)
			{
				if (update.OffsetBasedSize != -1L)
				{
					destinationPosition += update.OffsetBasedSize;
				}
				else
				{
					destinationPosition += num - num2 + 26L + update.Entry.CompressedSize + (long)this.GetDescriptorSize(update);
				}
			}
			else
			{
				if (update.Entry.CompressedSize > 0L)
				{
					this.CopyEntryDataDirect(update, this.baseStream_, false, ref destinationPosition, ref num);
				}
				this.CopyDescriptorBytesDirect(update, this.baseStream_, ref destinationPosition, num);
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000C20C File Offset: 0x0000A40C
		private void CopyEntry(ZipFile workFile, ZipFile.ZipUpdate update)
		{
			workFile.WriteLocalEntryHeader(update);
			if (update.Entry.CompressedSize > 0L)
			{
				long offset = update.Entry.Offset + 26L;
				this.baseStream_.Seek(offset, SeekOrigin.Begin);
				uint num = (uint)this.ReadLEUshort();
				uint num2 = (uint)this.ReadLEUshort();
				this.baseStream_.Seek((long)((ulong)(num + num2)), SeekOrigin.Current);
				this.CopyBytes(update, workFile.baseStream_, this.baseStream_, update.Entry.CompressedSize, false);
			}
			this.CopyDescriptorBytes(update, workFile.baseStream_, this.baseStream_);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000C2A4 File Offset: 0x0000A4A4
		private void Reopen(Stream source)
		{
			if (source == null)
			{
				throw new ZipException("Failed to reopen archive - no source");
			}
			this.isNewArchive_ = false;
			this.baseStream_ = source;
			this.ReadEntries();
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000C2CC File Offset: 0x0000A4CC
		private void Reopen()
		{
			if (this.Name == null)
			{
				throw new InvalidOperationException("Name is not known cannot Reopen");
			}
			this.Reopen(File.Open(this.Name, FileMode.Open, FileAccess.Read, FileShare.Read));
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000C304 File Offset: 0x0000A504
		private void UpdateCommentOnly()
		{
			long length = this.baseStream_.Length;
			ZipHelperStream zipHelperStream;
			if (this.archiveStorage_.UpdateMode == FileUpdateMode.Safe)
			{
				Stream stream = this.archiveStorage_.MakeTemporaryCopy(this.baseStream_);
				zipHelperStream = new ZipHelperStream(stream);
				zipHelperStream.IsStreamOwner = true;
				this.baseStream_.Close();
				this.baseStream_ = null;
			}
			else if (this.archiveStorage_.UpdateMode == FileUpdateMode.Direct)
			{
				this.baseStream_ = this.archiveStorage_.OpenForDirectUpdate(this.baseStream_);
				zipHelperStream = new ZipHelperStream(this.baseStream_);
			}
			else
			{
				this.baseStream_.Close();
				this.baseStream_ = null;
				zipHelperStream = new ZipHelperStream(this.Name);
			}
			using (zipHelperStream)
			{
				long num = zipHelperStream.LocateBlockWithSignature(101010256, length, 22, 65535);
				if (num < 0L)
				{
					throw new ZipException("Cannot find central directory");
				}
				zipHelperStream.Position += 16L;
				byte[] rawComment = this.newComment_.RawComment;
				zipHelperStream.WriteLEShort(rawComment.Length);
				zipHelperStream.Write(rawComment, 0, rawComment.Length);
				zipHelperStream.SetLength(zipHelperStream.Position);
			}
			if (this.archiveStorage_.UpdateMode == FileUpdateMode.Safe)
			{
				this.Reopen(this.archiveStorage_.ConvertTemporaryToFinal());
			}
			else
			{
				this.ReadEntries();
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000C484 File Offset: 0x0000A684
		private void RunUpdates()
		{
			long num = 0L;
			long length = 0L;
			bool flag = false;
			long position = 0L;
			ZipFile zipFile;
			if (this.IsNewArchive)
			{
				zipFile = this;
				zipFile.baseStream_.Position = 0L;
				flag = true;
			}
			else if (this.archiveStorage_.UpdateMode == FileUpdateMode.Direct)
			{
				zipFile = this;
				zipFile.baseStream_.Position = 0L;
				flag = true;
				this.updates_.Sort(new ZipFile.UpdateComparer());
			}
			else
			{
				zipFile = ZipFile.Create(this.archiveStorage_.GetTemporaryOutput());
				zipFile.UseZip64 = this.UseZip64;
				if (this.key != null)
				{
					zipFile.key = (byte[])this.key.Clone();
				}
			}
			try
			{
				foreach (object obj in this.updates_)
				{
					ZipFile.ZipUpdate zipUpdate = (ZipFile.ZipUpdate)obj;
					if (zipUpdate != null)
					{
						switch (zipUpdate.Command)
						{
						case ZipFile.UpdateCommand.Copy:
							if (flag)
							{
								this.CopyEntryDirect(zipFile, zipUpdate, ref position);
							}
							else
							{
								this.CopyEntry(zipFile, zipUpdate);
							}
							break;
						case ZipFile.UpdateCommand.Modify:
							this.ModifyEntry(zipFile, zipUpdate);
							break;
						case ZipFile.UpdateCommand.Add:
							if (!this.IsNewArchive && flag)
							{
								zipFile.baseStream_.Position = position;
							}
							this.AddEntry(zipFile, zipUpdate);
							if (flag)
							{
								position = zipFile.baseStream_.Position;
							}
							break;
						}
					}
				}
				if (!this.IsNewArchive && flag)
				{
					zipFile.baseStream_.Position = position;
				}
				long position2 = zipFile.baseStream_.Position;
				foreach (object obj2 in this.updates_)
				{
					ZipFile.ZipUpdate zipUpdate2 = (ZipFile.ZipUpdate)obj2;
					if (zipUpdate2 != null)
					{
						num += (long)zipFile.WriteCentralDirectoryHeader(zipUpdate2.OutEntry);
					}
				}
				byte[] comment = (this.newComment_ == null) ? ZipConstants.ConvertToArray(this.comment_) : this.newComment_.RawComment;
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(zipFile.baseStream_))
				{
					zipHelperStream.WriteEndOfCentralDirectory(this.updateCount_, num, position2, comment);
				}
				length = zipFile.baseStream_.Position;
				foreach (object obj3 in this.updates_)
				{
					ZipFile.ZipUpdate zipUpdate3 = (ZipFile.ZipUpdate)obj3;
					if (zipUpdate3 != null)
					{
						if (zipUpdate3.CrcPatchOffset > 0L && zipUpdate3.OutEntry.CompressedSize > 0L)
						{
							zipFile.baseStream_.Position = zipUpdate3.CrcPatchOffset;
							zipFile.WriteLEInt((int)zipUpdate3.OutEntry.Crc);
						}
						if (zipUpdate3.SizePatchOffset > 0L)
						{
							zipFile.baseStream_.Position = zipUpdate3.SizePatchOffset;
							if (zipUpdate3.OutEntry.LocalHeaderRequiresZip64)
							{
								zipFile.WriteLeLong(zipUpdate3.OutEntry.Size);
								zipFile.WriteLeLong(zipUpdate3.OutEntry.CompressedSize);
							}
							else
							{
								zipFile.WriteLEInt((int)zipUpdate3.OutEntry.CompressedSize);
								zipFile.WriteLEInt((int)zipUpdate3.OutEntry.Size);
							}
						}
					}
				}
			}
			catch
			{
				zipFile.Close();
				if (!flag && zipFile.Name != null)
				{
					File.Delete(zipFile.Name);
				}
				throw;
			}
			if (flag)
			{
				zipFile.baseStream_.SetLength(length);
				zipFile.baseStream_.Flush();
				this.isNewArchive_ = false;
				this.ReadEntries();
			}
			else
			{
				this.baseStream_.Close();
				this.Reopen(this.archiveStorage_.ConvertTemporaryToFinal());
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000C928 File Offset: 0x0000AB28
		private void CheckUpdating()
		{
			if (this.updates_ == null)
			{
				throw new InvalidOperationException("BeginUpdate has not been called");
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000C940 File Offset: 0x0000AB40
		private void DisposeInternal(bool disposing)
		{
			if (!this.isDisposed_)
			{
				this.isDisposed_ = true;
				this.entries_ = new ZipEntry[0];
				if (this.IsStreamOwner && this.baseStream_ != null)
				{
					Stream obj = this.baseStream_;
					lock (obj)
					{
						this.baseStream_.Close();
					}
				}
				this.PostUpdateCleanup();
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000C9C8 File Offset: 0x0000ABC8
		protected virtual void Dispose(bool disposing)
		{
			this.DisposeInternal(disposing);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000C9D4 File Offset: 0x0000ABD4
		private ushort ReadLEUshort()
		{
			int num = this.baseStream_.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException("End of stream");
			}
			int num2 = this.baseStream_.ReadByte();
			if (num2 < 0)
			{
				throw new EndOfStreamException("End of stream");
			}
			return (ushort)num | (ushort)(num2 << 8);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000CA28 File Offset: 0x0000AC28
		private uint ReadLEUint()
		{
			return (uint)((int)this.ReadLEUshort() | (int)this.ReadLEUshort() << 16);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000CA3C File Offset: 0x0000AC3C
		private ulong ReadLEUlong()
		{
			return (ulong)this.ReadLEUint() | (ulong)this.ReadLEUint() << 32;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000CA50 File Offset: 0x0000AC50
		private long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize, int maximumVariableData)
		{
			long result;
			using (ZipHelperStream zipHelperStream = new ZipHelperStream(this.baseStream_))
			{
				result = zipHelperStream.LocateBlockWithSignature(signature, endLocation, minimumBlockSize, maximumVariableData);
			}
			return result;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000CAAC File Offset: 0x0000ACAC
		private void ReadEntries()
		{
			if (!this.baseStream_.CanSeek)
			{
				throw new ZipException("ZipFile stream must be seekable");
			}
			long num = this.LocateBlockWithSignature(101010256, this.baseStream_.Length, 22, 65535);
			if (num < 0L)
			{
				throw new ZipException("Cannot find central directory");
			}
			ushort num2 = this.ReadLEUshort();
			ushort num3 = this.ReadLEUshort();
			ulong num4 = (ulong)this.ReadLEUshort();
			ulong num5 = (ulong)this.ReadLEUshort();
			ulong num6 = (ulong)this.ReadLEUint();
			long num7 = (long)((ulong)this.ReadLEUint());
			uint num8 = (uint)this.ReadLEUshort();
			if (num8 > 0U)
			{
				byte[] array = new byte[num8];
				StreamUtils.ReadFully(this.baseStream_, array);
				this.comment_ = ZipConstants.ConvertToString(array);
			}
			else
			{
				this.comment_ = string.Empty;
			}
			bool flag = false;
			if (num2 == 65535 || num3 == 65535 || num4 == 65535UL || num5 == 65535UL || num6 == (ulong)-1 || num7 == (long)((ulong)-1))
			{
				flag = true;
				long num9 = this.LocateBlockWithSignature(117853008, num, 0, 4096);
				if (num9 < 0L)
				{
					throw new ZipException("Cannot find Zip64 locator");
				}
				this.ReadLEUint();
				ulong num10 = this.ReadLEUlong();
				uint num11 = this.ReadLEUint();
				this.baseStream_.Position = (long)num10;
				long num12 = (long)((ulong)this.ReadLEUint());
				if (num12 != 101075792L)
				{
					throw new ZipException(string.Format("Invalid Zip64 Central directory signature at {0:X}", num10));
				}
				ulong num13 = this.ReadLEUlong();
				int num14 = (int)this.ReadLEUshort();
				int num15 = (int)this.ReadLEUshort();
				uint num16 = this.ReadLEUint();
				uint num17 = this.ReadLEUint();
				num4 = this.ReadLEUlong();
				num5 = this.ReadLEUlong();
				num6 = this.ReadLEUlong();
				num7 = (long)this.ReadLEUlong();
			}
			this.entries_ = new ZipEntry[num4];
			if (!flag && num7 < num - (long)(4UL + num6))
			{
				this.offsetOfFirstEntry = num - (long)(4UL + num6 + (ulong)num7);
				if (this.offsetOfFirstEntry <= 0L)
				{
					throw new ZipException("Invalid embedded zip archive");
				}
			}
			this.baseStream_.Seek(this.offsetOfFirstEntry + num7, SeekOrigin.Begin);
			for (ulong num18 = 0UL; num18 < num4; num18 += 1UL)
			{
				if (this.ReadLEUint() != 33639248U)
				{
					throw new ZipException("Wrong Central Directory signature");
				}
				int madeByInfo = (int)this.ReadLEUshort();
				int versionRequiredToExtract = (int)this.ReadLEUshort();
				int num19 = (int)this.ReadLEUshort();
				int method = (int)this.ReadLEUshort();
				uint num20 = this.ReadLEUint();
				uint num21 = this.ReadLEUint();
				long num22 = (long)((ulong)this.ReadLEUint());
				long num23 = (long)((ulong)this.ReadLEUint());
				int num24 = (int)this.ReadLEUshort();
				int num25 = (int)this.ReadLEUshort();
				int num26 = (int)this.ReadLEUshort();
				int num27 = (int)this.ReadLEUshort();
				int num28 = (int)this.ReadLEUshort();
				uint externalFileAttributes = this.ReadLEUint();
				long offset = (long)((ulong)this.ReadLEUint());
				byte[] array2 = new byte[Math.Max(num24, num26)];
				StreamUtils.ReadFully(this.baseStream_, array2, 0, num24);
				string name = ZipConstants.ConvertToStringExt(num19, array2, num24);
				ZipEntry zipEntry = new ZipEntry(name, versionRequiredToExtract, madeByInfo, (CompressionMethod)method);
				zipEntry.Crc = (long)((ulong)num21 & (ulong)-1);
				zipEntry.Size = (num23 & (long)((ulong)-1));
				zipEntry.CompressedSize = (num22 & (long)((ulong)-1));
				zipEntry.Flags = num19;
				zipEntry.DosTime = (long)((ulong)num20);
				zipEntry.ZipFileIndex = (long)num18;
				zipEntry.Offset = offset;
				zipEntry.ExternalFileAttributes = (int)externalFileAttributes;
				if ((num19 & 8) == 0)
				{
					zipEntry.CryptoCheckValue = (byte)(num21 >> 24);
				}
				else
				{
					zipEntry.CryptoCheckValue = (byte)(num20 >> 8 & 255U);
				}
				if (num25 > 0)
				{
					byte[] array3 = new byte[num25];
					StreamUtils.ReadFully(this.baseStream_, array3);
					zipEntry.ExtraData = array3;
				}
				zipEntry.ProcessExtraData(false);
				if (num26 > 0)
				{
					StreamUtils.ReadFully(this.baseStream_, array2, 0, num26);
					zipEntry.Comment = ZipConstants.ConvertToStringExt(num19, array2, num26);
				}
				this.entries_[(int)(checked((IntPtr)num18))] = zipEntry;
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000CEB0 File Offset: 0x0000B0B0
		private long LocateEntry(ZipEntry entry)
		{
			return this.TestLocalHeader(entry, ZipFile.HeaderTest.Extract);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000CEBC File Offset: 0x0000B0BC
		private Stream CreateAndInitDecryptionStream(Stream baseStream, ZipEntry entry)
		{
			if (entry.Version >= 50 && (entry.Flags & 64) != 0)
			{
				throw new ZipException("Decryption method not supported");
			}
			PkzipClassicManaged pkzipClassicManaged = new PkzipClassicManaged();
			this.OnKeysRequired(entry.Name);
			if (!this.HaveKeys)
			{
				throw new ZipException("No password available for encrypted stream");
			}
			CryptoStream cryptoStream = new CryptoStream(baseStream, pkzipClassicManaged.CreateDecryptor(this.key, null), CryptoStreamMode.Read);
			ZipFile.CheckClassicPassword(cryptoStream, entry);
			return cryptoStream;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000CF3C File Offset: 0x0000B13C
		private Stream CreateAndInitEncryptionStream(Stream baseStream, ZipEntry entry)
		{
			CryptoStream cryptoStream = null;
			if (entry.Version < 50 || (entry.Flags & 64) == 0)
			{
				PkzipClassicManaged pkzipClassicManaged = new PkzipClassicManaged();
				this.OnKeysRequired(entry.Name);
				if (!this.HaveKeys)
				{
					throw new ZipException("No password available for encrypted stream");
				}
				cryptoStream = new CryptoStream(new ZipFile.UncompressedStream(baseStream), pkzipClassicManaged.CreateEncryptor(this.key, null), CryptoStreamMode.Write);
				if (entry.Crc < 0L || (entry.Flags & 8) != 0)
				{
					ZipFile.WriteEncryptionHeader(cryptoStream, entry.DosTime << 16);
				}
				else
				{
					ZipFile.WriteEncryptionHeader(cryptoStream, entry.Crc);
				}
			}
			return cryptoStream;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000CFE4 File Offset: 0x0000B1E4
		private static void CheckClassicPassword(CryptoStream classicCryptoStream, ZipEntry entry)
		{
			byte[] array = new byte[12];
			StreamUtils.ReadFully(classicCryptoStream, array);
			if (array[11] != entry.CryptoCheckValue)
			{
				throw new ZipException("Invalid password");
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000D01C File Offset: 0x0000B21C
		private static void WriteEncryptionHeader(Stream stream, long crcValue)
		{
			byte[] array = new byte[12];
			Random random = new Random();
			random.NextBytes(array);
			array[11] = (byte)(crcValue >> 24);
			stream.Write(array, 0, array.Length);
		}

		// Token: 0x0400015D RID: 349
		private const int DefaultBufferSize = 4096;

		// Token: 0x0400015E RID: 350
		public ZipFile.KeysRequiredEventHandler KeysRequired;

		// Token: 0x0400015F RID: 351
		private bool isDisposed_;

		// Token: 0x04000160 RID: 352
		private string name_;

		// Token: 0x04000161 RID: 353
		private string comment_;

		// Token: 0x04000162 RID: 354
		private string rawPassword_;

		// Token: 0x04000163 RID: 355
		private Stream baseStream_;

		// Token: 0x04000164 RID: 356
		private bool isStreamOwner;

		// Token: 0x04000165 RID: 357
		private long offsetOfFirstEntry;

		// Token: 0x04000166 RID: 358
		private ZipEntry[] entries_;

		// Token: 0x04000167 RID: 359
		private byte[] key;

		// Token: 0x04000168 RID: 360
		private bool isNewArchive_;

		// Token: 0x04000169 RID: 361
		private UseZip64 useZip64_ = UseZip64.Dynamic;

		// Token: 0x0400016A RID: 362
		private ArrayList updates_;

		// Token: 0x0400016B RID: 363
		private long updateCount_;

		// Token: 0x0400016C RID: 364
		private Hashtable updateIndex_;

		// Token: 0x0400016D RID: 365
		private IArchiveStorage archiveStorage_;

		// Token: 0x0400016E RID: 366
		private IDynamicDataSource updateDataSource_;

		// Token: 0x0400016F RID: 367
		private bool contentsEdited_;

		// Token: 0x04000170 RID: 368
		private int bufferSize_ = 4096;

		// Token: 0x04000171 RID: 369
		private byte[] copyBuffer_;

		// Token: 0x04000172 RID: 370
		private ZipFile.ZipString newComment_;

		// Token: 0x04000173 RID: 371
		private bool commentEdited_;

		// Token: 0x04000174 RID: 372
		private IEntryFactory updateEntryFactory_ = new ZipEntryFactory();

		// Token: 0x02000024 RID: 36
		[Flags]
		private enum HeaderTest
		{
			// Token: 0x04000176 RID: 374
			Extract = 1,
			// Token: 0x04000177 RID: 375
			Header = 2
		}

		// Token: 0x02000025 RID: 37
		private enum UpdateCommand
		{
			// Token: 0x04000179 RID: 377
			Copy,
			// Token: 0x0400017A RID: 378
			Modify,
			// Token: 0x0400017B RID: 379
			Add
		}

		// Token: 0x02000026 RID: 38
		private class UpdateComparer : IComparer
		{
			// Token: 0x060001A1 RID: 417 RVA: 0x0000D05C File Offset: 0x0000B25C
			public int Compare(object x, object y)
			{
				ZipFile.ZipUpdate zipUpdate = x as ZipFile.ZipUpdate;
				ZipFile.ZipUpdate zipUpdate2 = y as ZipFile.ZipUpdate;
				int num;
				if (zipUpdate == null)
				{
					if (zipUpdate2 == null)
					{
						num = 0;
					}
					else
					{
						num = -1;
					}
				}
				else if (zipUpdate2 == null)
				{
					num = 1;
				}
				else
				{
					int num2 = (zipUpdate.Command != ZipFile.UpdateCommand.Copy && zipUpdate.Command != ZipFile.UpdateCommand.Modify) ? 1 : 0;
					int num3 = (zipUpdate2.Command != ZipFile.UpdateCommand.Copy && zipUpdate2.Command != ZipFile.UpdateCommand.Modify) ? 1 : 0;
					num = num2 - num3;
					if (num == 0)
					{
						long num4 = zipUpdate.Entry.Offset - zipUpdate2.Entry.Offset;
						if (num4 < 0L)
						{
							num = -1;
						}
						else if (num4 == 0L)
						{
							num = 0;
						}
						else
						{
							num = 1;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x02000027 RID: 39
		private class ZipUpdate
		{
			// Token: 0x060001A2 RID: 418 RVA: 0x0000D124 File Offset: 0x0000B324
			public ZipUpdate(string fileName, ZipEntry entry)
			{
				this.command_ = ZipFile.UpdateCommand.Add;
				this.entry_ = entry;
				this.filename_ = fileName;
			}

			// Token: 0x060001A3 RID: 419 RVA: 0x0000D15C File Offset: 0x0000B35C
			[Obsolete]
			public ZipUpdate(string fileName, string entryName, CompressionMethod compressionMethod)
			{
				this.command_ = ZipFile.UpdateCommand.Add;
				this.entry_ = new ZipEntry(entryName);
				this.entry_.CompressionMethod = compressionMethod;
				this.filename_ = fileName;
			}

			// Token: 0x060001A4 RID: 420 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
			[Obsolete]
			public ZipUpdate(string fileName, string entryName) : this(fileName, entryName, CompressionMethod.Deflated)
			{
			}

			// Token: 0x060001A5 RID: 421 RVA: 0x0000D1BC File Offset: 0x0000B3BC
			[Obsolete]
			public ZipUpdate(IStaticDataSource dataSource, string entryName, CompressionMethod compressionMethod)
			{
				this.command_ = ZipFile.UpdateCommand.Add;
				this.entry_ = new ZipEntry(entryName);
				this.entry_.CompressionMethod = compressionMethod;
				this.dataSource_ = dataSource;
			}

			// Token: 0x060001A6 RID: 422 RVA: 0x0000D210 File Offset: 0x0000B410
			public ZipUpdate(IStaticDataSource dataSource, ZipEntry entry)
			{
				this.command_ = ZipFile.UpdateCommand.Add;
				this.entry_ = entry;
				this.dataSource_ = dataSource;
			}

			// Token: 0x060001A7 RID: 423 RVA: 0x0000D248 File Offset: 0x0000B448
			public ZipUpdate(ZipEntry original, ZipEntry updated)
			{
				throw new ZipException("Modify not currently supported");
			}

			// Token: 0x060001A8 RID: 424 RVA: 0x0000D280 File Offset: 0x0000B480
			public ZipUpdate(ZipFile.UpdateCommand command, ZipEntry entry)
			{
				this.command_ = command;
				this.entry_ = (ZipEntry)entry.Clone();
			}

			// Token: 0x060001A9 RID: 425 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
			public ZipUpdate(ZipEntry entry) : this(ZipFile.UpdateCommand.Copy, entry)
			{
			}

			// Token: 0x17000052 RID: 82
			// (get) Token: 0x060001AA RID: 426 RVA: 0x0000D2D0 File Offset: 0x0000B4D0
			public ZipEntry Entry
			{
				get
				{
					return this.entry_;
				}
			}

			// Token: 0x17000053 RID: 83
			// (get) Token: 0x060001AB RID: 427 RVA: 0x0000D2D8 File Offset: 0x0000B4D8
			public ZipEntry OutEntry
			{
				get
				{
					if (this.outEntry_ == null)
					{
						this.outEntry_ = (ZipEntry)this.entry_.Clone();
					}
					return this.outEntry_;
				}
			}

			// Token: 0x17000054 RID: 84
			// (get) Token: 0x060001AC RID: 428 RVA: 0x0000D304 File Offset: 0x0000B504
			public ZipFile.UpdateCommand Command
			{
				get
				{
					return this.command_;
				}
			}

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x060001AD RID: 429 RVA: 0x0000D30C File Offset: 0x0000B50C
			public string Filename
			{
				get
				{
					return this.filename_;
				}
			}

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x060001AE RID: 430 RVA: 0x0000D314 File Offset: 0x0000B514
			// (set) Token: 0x060001AF RID: 431 RVA: 0x0000D31C File Offset: 0x0000B51C
			public long SizePatchOffset
			{
				get
				{
					return this.sizePatchOffset_;
				}
				set
				{
					this.sizePatchOffset_ = value;
				}
			}

			// Token: 0x17000057 RID: 87
			// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000D328 File Offset: 0x0000B528
			// (set) Token: 0x060001B1 RID: 433 RVA: 0x0000D330 File Offset: 0x0000B530
			public long CrcPatchOffset
			{
				get
				{
					return this.crcPatchOffset_;
				}
				set
				{
					this.crcPatchOffset_ = value;
				}
			}

			// Token: 0x17000058 RID: 88
			// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000D33C File Offset: 0x0000B53C
			// (set) Token: 0x060001B3 RID: 435 RVA: 0x0000D344 File Offset: 0x0000B544
			public long OffsetBasedSize
			{
				get
				{
					return this._offsetBasedSize;
				}
				set
				{
					this._offsetBasedSize = value;
				}
			}

			// Token: 0x060001B4 RID: 436 RVA: 0x0000D350 File Offset: 0x0000B550
			public Stream GetSource()
			{
				Stream result = null;
				if (this.dataSource_ != null)
				{
					result = this.dataSource_.GetSource();
				}
				return result;
			}

			// Token: 0x0400017C RID: 380
			private ZipEntry entry_;

			// Token: 0x0400017D RID: 381
			private ZipEntry outEntry_;

			// Token: 0x0400017E RID: 382
			private ZipFile.UpdateCommand command_;

			// Token: 0x0400017F RID: 383
			private IStaticDataSource dataSource_;

			// Token: 0x04000180 RID: 384
			private string filename_;

			// Token: 0x04000181 RID: 385
			private long sizePatchOffset_ = -1L;

			// Token: 0x04000182 RID: 386
			private long crcPatchOffset_ = -1L;

			// Token: 0x04000183 RID: 387
			private long _offsetBasedSize = -1L;
		}

		// Token: 0x02000028 RID: 40
		private class ZipString
		{
			// Token: 0x060001B5 RID: 437 RVA: 0x0000D378 File Offset: 0x0000B578
			public ZipString(string comment)
			{
				this.comment_ = comment;
				this.isSourceString_ = true;
			}

			// Token: 0x060001B6 RID: 438 RVA: 0x0000D390 File Offset: 0x0000B590
			public ZipString(byte[] rawString)
			{
				this.rawComment_ = rawString;
			}

			// Token: 0x17000059 RID: 89
			// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000D3A0 File Offset: 0x0000B5A0
			public bool IsSourceString
			{
				get
				{
					return this.isSourceString_;
				}
			}

			// Token: 0x1700005A RID: 90
			// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000D3A8 File Offset: 0x0000B5A8
			public int RawLength
			{
				get
				{
					this.MakeBytesAvailable();
					return this.rawComment_.Length;
				}
			}

			// Token: 0x1700005B RID: 91
			// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000D3B8 File Offset: 0x0000B5B8
			public byte[] RawComment
			{
				get
				{
					this.MakeBytesAvailable();
					return (byte[])this.rawComment_.Clone();
				}
			}

			// Token: 0x060001BA RID: 442 RVA: 0x0000D3D0 File Offset: 0x0000B5D0
			public void Reset()
			{
				if (this.isSourceString_)
				{
					this.rawComment_ = null;
				}
				else
				{
					this.comment_ = null;
				}
			}

			// Token: 0x060001BB RID: 443 RVA: 0x0000D3F0 File Offset: 0x0000B5F0
			private void MakeTextAvailable()
			{
				if (this.comment_ == null)
				{
					this.comment_ = ZipConstants.ConvertToString(this.rawComment_);
				}
			}

			// Token: 0x060001BC RID: 444 RVA: 0x0000D410 File Offset: 0x0000B610
			private void MakeBytesAvailable()
			{
				if (this.rawComment_ == null)
				{
					this.rawComment_ = ZipConstants.ConvertToArray(this.comment_);
				}
			}

			// Token: 0x060001BD RID: 445 RVA: 0x0000D430 File Offset: 0x0000B630
			public static implicit operator string(ZipFile.ZipString zipString)
			{
				zipString.MakeTextAvailable();
				return zipString.comment_;
			}

			// Token: 0x04000184 RID: 388
			private string comment_;

			// Token: 0x04000185 RID: 389
			private byte[] rawComment_;

			// Token: 0x04000186 RID: 390
			private bool isSourceString_;
		}

		// Token: 0x02000029 RID: 41
		private class ZipEntryEnumerator : IEnumerator
		{
			// Token: 0x060001BE RID: 446 RVA: 0x0000D440 File Offset: 0x0000B640
			public ZipEntryEnumerator(ZipEntry[] entries)
			{
				this.array = entries;
			}

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x060001BF RID: 447 RVA: 0x0000D458 File Offset: 0x0000B658
			public object Current
			{
				get
				{
					return this.array[this.index];
				}
			}

			// Token: 0x060001C0 RID: 448 RVA: 0x0000D468 File Offset: 0x0000B668
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x060001C1 RID: 449 RVA: 0x0000D474 File Offset: 0x0000B674
			public bool MoveNext()
			{
				return ++this.index < this.array.Length;
			}

			// Token: 0x04000187 RID: 391
			private ZipEntry[] array;

			// Token: 0x04000188 RID: 392
			private int index = -1;
		}

		// Token: 0x0200002A RID: 42
		private class UncompressedStream : Stream
		{
			// Token: 0x060001C2 RID: 450 RVA: 0x0000D49C File Offset: 0x0000B69C
			public UncompressedStream(Stream baseStream)
			{
				this.baseStream_ = baseStream;
			}

			// Token: 0x060001C3 RID: 451 RVA: 0x0000D4AC File Offset: 0x0000B6AC
			public override void Close()
			{
			}

			// Token: 0x1700005D RID: 93
			// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000D4B0 File Offset: 0x0000B6B0
			public override bool CanRead
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060001C5 RID: 453 RVA: 0x0000D4B4 File Offset: 0x0000B6B4
			public override void Flush()
			{
				this.baseStream_.Flush();
			}

			// Token: 0x1700005E RID: 94
			// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000D4C4 File Offset: 0x0000B6C4
			public override bool CanWrite
			{
				get
				{
					return this.baseStream_.CanWrite;
				}
			}

			// Token: 0x1700005F RID: 95
			// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000D4D4 File Offset: 0x0000B6D4
			public override bool CanSeek
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000060 RID: 96
			// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000D4D8 File Offset: 0x0000B6D8
			public override long Length
			{
				get
				{
					return 0L;
				}
			}

			// Token: 0x17000061 RID: 97
			// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000D4DC File Offset: 0x0000B6DC
			// (set) Token: 0x060001CA RID: 458 RVA: 0x0000D4EC File Offset: 0x0000B6EC
			public override long Position
			{
				get
				{
					return this.baseStream_.Position;
				}
				set
				{
				}
			}

			// Token: 0x060001CB RID: 459 RVA: 0x0000D4F0 File Offset: 0x0000B6F0
			public override int Read(byte[] buffer, int offset, int count)
			{
				return 0;
			}

			// Token: 0x060001CC RID: 460 RVA: 0x0000D4F4 File Offset: 0x0000B6F4
			public override long Seek(long offset, SeekOrigin origin)
			{
				return 0L;
			}

			// Token: 0x060001CD RID: 461 RVA: 0x0000D4F8 File Offset: 0x0000B6F8
			public override void SetLength(long value)
			{
			}

			// Token: 0x060001CE RID: 462 RVA: 0x0000D4FC File Offset: 0x0000B6FC
			public override void Write(byte[] buffer, int offset, int count)
			{
				this.baseStream_.Write(buffer, offset, count);
			}

			// Token: 0x04000189 RID: 393
			private Stream baseStream_;
		}

		// Token: 0x0200002B RID: 43
		private class PartialInputStream : Stream
		{
			// Token: 0x060001CF RID: 463 RVA: 0x0000D50C File Offset: 0x0000B70C
			public PartialInputStream(ZipFile zipFile, long start, long length)
			{
				this.start_ = start;
				this.length_ = length;
				this.zipFile_ = zipFile;
				this.baseStream_ = this.zipFile_.baseStream_;
				this.readPos_ = start;
				this.end_ = start + length;
			}

			// Token: 0x060001D0 RID: 464 RVA: 0x0000D558 File Offset: 0x0000B758
			public override int ReadByte()
			{
				if (this.readPos_ >= this.end_)
				{
					return -1;
				}
				Stream obj = this.baseStream_;
				int result;
				lock (obj)
				{
					Stream stream = this.baseStream_;
					long offset;
					this.readPos_ = (offset = this.readPos_) + 1L;
					stream.Seek(offset, SeekOrigin.Begin);
					result = this.baseStream_.ReadByte();
				}
				return result;
			}

			// Token: 0x060001D1 RID: 465 RVA: 0x0000D5E0 File Offset: 0x0000B7E0
			public override void Close()
			{
			}

			// Token: 0x060001D2 RID: 466 RVA: 0x0000D5E4 File Offset: 0x0000B7E4
			public override int Read(byte[] buffer, int offset, int count)
			{
				Stream obj = this.baseStream_;
				int result;
				lock (obj)
				{
					if ((long)count > this.end_ - this.readPos_)
					{
						count = (int)(this.end_ - this.readPos_);
						if (count == 0)
						{
							return 0;
						}
					}
					this.baseStream_.Seek(this.readPos_, SeekOrigin.Begin);
					int num = this.baseStream_.Read(buffer, offset, count);
					if (num > 0)
					{
						this.readPos_ += (long)num;
					}
					result = num;
				}
				return result;
			}

			// Token: 0x060001D3 RID: 467 RVA: 0x0000D698 File Offset: 0x0000B898
			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060001D4 RID: 468 RVA: 0x0000D6A0 File Offset: 0x0000B8A0
			public override void SetLength(long value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060001D5 RID: 469 RVA: 0x0000D6A8 File Offset: 0x0000B8A8
			public override long Seek(long offset, SeekOrigin origin)
			{
				long num = this.readPos_;
				switch (origin)
				{
				case SeekOrigin.Begin:
					num = this.start_ + offset;
					break;
				case SeekOrigin.Current:
					num = this.readPos_ + offset;
					break;
				case SeekOrigin.End:
					num = this.end_ + offset;
					break;
				}
				if (num < this.start_)
				{
					throw new ArgumentException("Negative position is invalid");
				}
				if (num >= this.end_)
				{
					throw new IOException("Cannot seek past end");
				}
				this.readPos_ = num;
				return this.readPos_;
			}

			// Token: 0x060001D6 RID: 470 RVA: 0x0000D73C File Offset: 0x0000B93C
			public override void Flush()
			{
			}

			// Token: 0x17000062 RID: 98
			// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000D740 File Offset: 0x0000B940
			// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000D750 File Offset: 0x0000B950
			public override long Position
			{
				get
				{
					return this.readPos_ - this.start_;
				}
				set
				{
					long num = this.start_ + value;
					if (num < this.start_)
					{
						throw new ArgumentException("Negative position is invalid");
					}
					if (num >= this.end_)
					{
						throw new InvalidOperationException("Cannot seek past end");
					}
					this.readPos_ = num;
				}
			}

			// Token: 0x17000063 RID: 99
			// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000D79C File Offset: 0x0000B99C
			public override long Length
			{
				get
				{
					return this.length_;
				}
			}

			// Token: 0x17000064 RID: 100
			// (get) Token: 0x060001DA RID: 474 RVA: 0x0000D7A4 File Offset: 0x0000B9A4
			public override bool CanWrite
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000065 RID: 101
			// (get) Token: 0x060001DB RID: 475 RVA: 0x0000D7A8 File Offset: 0x0000B9A8
			public override bool CanSeek
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000066 RID: 102
			// (get) Token: 0x060001DC RID: 476 RVA: 0x0000D7AC File Offset: 0x0000B9AC
			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0400018A RID: 394
			private ZipFile zipFile_;

			// Token: 0x0400018B RID: 395
			private Stream baseStream_;

			// Token: 0x0400018C RID: 396
			private long start_;

			// Token: 0x0400018D RID: 397
			private long length_;

			// Token: 0x0400018E RID: 398
			private long readPos_;

			// Token: 0x0400018F RID: 399
			private long end_;
		}

		// Token: 0x02000072 RID: 114
		// (Invoke) Token: 0x060004B0 RID: 1200
		public delegate void KeysRequiredEventHandler(object sender, KeysRequiredEventArgs e);
	}
}
