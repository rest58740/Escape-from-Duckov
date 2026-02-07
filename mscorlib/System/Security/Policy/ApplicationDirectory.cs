using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x020003FF RID: 1023
	[ComVisible(true)]
	[Serializable]
	public sealed class ApplicationDirectory : EvidenceBase, IBuiltInEvidence
	{
		// Token: 0x060029D0 RID: 10704 RVA: 0x00097ED5 File Offset: 0x000960D5
		public ApplicationDirectory(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length < 1)
			{
				throw new FormatException(Locale.GetText("Empty"));
			}
			this.directory = name;
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x060029D1 RID: 10705 RVA: 0x00097F0B File Offset: 0x0009610B
		public string Directory
		{
			get
			{
				return this.directory;
			}
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x00097F13 File Offset: 0x00096113
		public object Copy()
		{
			return new ApplicationDirectory(this.Directory);
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x00097F20 File Offset: 0x00096120
		public override bool Equals(object o)
		{
			ApplicationDirectory applicationDirectory = o as ApplicationDirectory;
			if (applicationDirectory != null)
			{
				this.ThrowOnInvalid(applicationDirectory.directory);
				return this.directory == applicationDirectory.directory;
			}
			return false;
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x00097F56 File Offset: 0x00096156
		public override int GetHashCode()
		{
			return this.Directory.GetHashCode();
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x00097F64 File Offset: 0x00096164
		public override string ToString()
		{
			this.ThrowOnInvalid(this.Directory);
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.ApplicationDirectory");
			securityElement.AddAttribute("version", "1");
			securityElement.AddChild(new SecurityElement("Directory", this.directory));
			return securityElement.ToString();
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x00097FB2 File Offset: 0x000961B2
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			return (verbose ? 3 : 1) + this.directory.Length;
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[MonoTODO("IBuiltInEvidence")]
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			return 0;
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[MonoTODO("IBuiltInEvidence")]
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			return 0;
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x00097FC7 File Offset: 0x000961C7
		private void ThrowOnInvalid(string appdir)
		{
			if (appdir.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException(string.Format(Locale.GetText("Invalid character(s) in directory {0}"), appdir), "other");
			}
		}

		// Token: 0x04001F53 RID: 8019
		private string directory;
	}
}
