using System;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;

namespace System.IO
{
	// Token: 0x02000B39 RID: 2873
	[Serializable]
	public sealed class FileInfo : FileSystemInfo
	{
		// Token: 0x060067B6 RID: 26550 RVA: 0x00161FFE File Offset: 0x001601FE
		private FileInfo()
		{
		}

		// Token: 0x060067B7 RID: 26551 RVA: 0x00162006 File Offset: 0x00160206
		public FileInfo(string fileName) : this(fileName, null, null, false)
		{
		}

		// Token: 0x060067B8 RID: 26552 RVA: 0x00162014 File Offset: 0x00160214
		internal FileInfo(string originalPath, string fullPath = null, string fileName = null, bool isNormalized = false)
		{
			if (originalPath == null)
			{
				throw new ArgumentNullException("fileName");
			}
			this.OriginalPath = originalPath;
			fullPath = (fullPath ?? originalPath);
			this.FullPath = (isNormalized ? (fullPath ?? originalPath) : Path.GetFullPath(fullPath));
			this._name = (fileName ?? Path.GetFileName(originalPath));
		}

		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x060067B9 RID: 26553 RVA: 0x0016206E File Offset: 0x0016026E
		public long Length
		{
			get
			{
				if ((base.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
				{
					throw new FileNotFoundException(SR.Format("Could not find file '{0}'.", this.FullPath), this.FullPath);
				}
				return base.LengthCore;
			}
		}

		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x060067BA RID: 26554 RVA: 0x0016209F File Offset: 0x0016029F
		public string DirectoryName
		{
			get
			{
				return Path.GetDirectoryName(this.FullPath);
			}
		}

		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x060067BB RID: 26555 RVA: 0x001620AC File Offset: 0x001602AC
		public DirectoryInfo Directory
		{
			get
			{
				string directoryName = this.DirectoryName;
				if (directoryName == null)
				{
					return null;
				}
				return new DirectoryInfo(directoryName);
			}
		}

		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x060067BC RID: 26556 RVA: 0x001620CB File Offset: 0x001602CB
		// (set) Token: 0x060067BD RID: 26557 RVA: 0x001620D8 File Offset: 0x001602D8
		public bool IsReadOnly
		{
			get
			{
				return (base.Attributes & FileAttributes.ReadOnly) > (FileAttributes)0;
			}
			set
			{
				if (value)
				{
					base.Attributes |= FileAttributes.ReadOnly;
					return;
				}
				base.Attributes &= ~FileAttributes.ReadOnly;
			}
		}

		// Token: 0x060067BE RID: 26558 RVA: 0x001620FB File Offset: 0x001602FB
		public StreamReader OpenText()
		{
			return new StreamReader(base.NormalizedPath, Encoding.UTF8, true);
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x0016210E File Offset: 0x0016030E
		public StreamWriter CreateText()
		{
			return new StreamWriter(base.NormalizedPath, false);
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x0016211C File Offset: 0x0016031C
		public StreamWriter AppendText()
		{
			return new StreamWriter(base.NormalizedPath, true);
		}

		// Token: 0x060067C1 RID: 26561 RVA: 0x0016212A File Offset: 0x0016032A
		public FileInfo CopyTo(string destFileName)
		{
			return this.CopyTo(destFileName, false);
		}

		// Token: 0x060067C2 RID: 26562 RVA: 0x00162134 File Offset: 0x00160334
		public FileInfo CopyTo(string destFileName, bool overwrite)
		{
			if (destFileName == null)
			{
				throw new ArgumentNullException("destFileName", "File name cannot be null.");
			}
			if (destFileName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "destFileName");
			}
			string fullPath = Path.GetFullPath(destFileName);
			FileSystem.CopyFile(this.FullPath, fullPath, overwrite);
			return new FileInfo(fullPath, null, null, true);
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x00162189 File Offset: 0x00160389
		public FileStream Create()
		{
			return File.Create(base.NormalizedPath);
		}

		// Token: 0x060067C4 RID: 26564 RVA: 0x00162196 File Offset: 0x00160396
		public override void Delete()
		{
			FileSystem.DeleteFile(this.FullPath);
		}

		// Token: 0x060067C5 RID: 26565 RVA: 0x001621A3 File Offset: 0x001603A3
		public FileStream Open(FileMode mode)
		{
			return this.Open(mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);
		}

		// Token: 0x060067C6 RID: 26566 RVA: 0x001621B5 File Offset: 0x001603B5
		public FileStream Open(FileMode mode, FileAccess access)
		{
			return this.Open(mode, access, FileShare.None);
		}

		// Token: 0x060067C7 RID: 26567 RVA: 0x001621C0 File Offset: 0x001603C0
		public FileStream Open(FileMode mode, FileAccess access, FileShare share)
		{
			return new FileStream(base.NormalizedPath, mode, access, share);
		}

		// Token: 0x060067C8 RID: 26568 RVA: 0x001621D0 File Offset: 0x001603D0
		public FileStream OpenRead()
		{
			return new FileStream(base.NormalizedPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, false);
		}

		// Token: 0x060067C9 RID: 26569 RVA: 0x001621E6 File Offset: 0x001603E6
		public FileStream OpenWrite()
		{
			return new FileStream(base.NormalizedPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}

		// Token: 0x060067CA RID: 26570 RVA: 0x001621F8 File Offset: 0x001603F8
		public void MoveTo(string destFileName)
		{
			if (destFileName == null)
			{
				throw new ArgumentNullException("destFileName");
			}
			if (destFileName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "destFileName");
			}
			string fullPath = Path.GetFullPath(destFileName);
			if (!new DirectoryInfo(Path.GetDirectoryName(this.FullName)).Exists)
			{
				throw new DirectoryNotFoundException(SR.Format("Could not find a part of the path '{0}'.", this.FullName));
			}
			if (!this.Exists)
			{
				throw new FileNotFoundException(SR.Format("Could not find file '{0}'.", this.FullName), this.FullName);
			}
			FileSystem.MoveFile(this.FullPath, fullPath);
			this.FullPath = fullPath;
			this.OriginalPath = destFileName;
			this._name = Path.GetFileName(fullPath);
			base.Invalidate();
		}

		// Token: 0x060067CB RID: 26571 RVA: 0x001622AF File Offset: 0x001604AF
		public FileInfo Replace(string destinationFileName, string destinationBackupFileName)
		{
			return this.Replace(destinationFileName, destinationBackupFileName, false);
		}

		// Token: 0x060067CC RID: 26572 RVA: 0x001622BA File Offset: 0x001604BA
		public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
		{
			if (destinationFileName == null)
			{
				throw new ArgumentNullException("destinationFileName");
			}
			FileSystem.ReplaceFile(this.FullPath, Path.GetFullPath(destinationFileName), (destinationBackupFileName != null) ? Path.GetFullPath(destinationBackupFileName) : null, ignoreMetadataErrors);
			return new FileInfo(destinationFileName);
		}

		// Token: 0x060067CD RID: 26573 RVA: 0x001622EE File Offset: 0x001604EE
		public void Decrypt()
		{
			File.Decrypt(this.FullPath);
		}

		// Token: 0x060067CE RID: 26574 RVA: 0x001622FB File Offset: 0x001604FB
		public void Encrypt()
		{
			File.Encrypt(this.FullPath);
		}

		// Token: 0x060067CF RID: 26575 RVA: 0x0015FF39 File Offset: 0x0015E139
		private FileInfo(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060067D0 RID: 26576 RVA: 0x00162308 File Offset: 0x00160508
		public FileSecurity GetAccessControl()
		{
			return File.GetAccessControl(this.FullPath, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x060067D1 RID: 26577 RVA: 0x00162317 File Offset: 0x00160517
		public FileSecurity GetAccessControl(AccessControlSections includeSections)
		{
			return File.GetAccessControl(this.FullPath, includeSections);
		}

		// Token: 0x060067D2 RID: 26578 RVA: 0x00162325 File Offset: 0x00160525
		public void SetAccessControl(FileSecurity fileSecurity)
		{
			File.SetAccessControl(this.FullPath, fileSecurity);
		}

		// Token: 0x060067D3 RID: 26579 RVA: 0x00162333 File Offset: 0x00160533
		internal FileInfo(string fullPath, bool ignoreThis)
		{
			this._name = Path.GetFileName(fullPath);
			this.OriginalPath = this._name;
			this.FullPath = fullPath;
		}

		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x060067D4 RID: 26580 RVA: 0x0016235A File Offset: 0x0016055A
		public override string Name
		{
			get
			{
				return this._name;
			}
		}
	}
}
