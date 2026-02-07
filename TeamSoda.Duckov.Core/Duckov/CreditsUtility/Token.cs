using System;

namespace Duckov.CreditsUtility
{
	// Token: 0x0200030D RID: 781
	public struct Token
	{
		// Token: 0x060019B0 RID: 6576 RVA: 0x0005E184 File Offset: 0x0005C384
		public Token(TokenType type, string text = null)
		{
			this.type = type;
			this.text = text;
		}

		// Token: 0x040012B6 RID: 4790
		public TokenType type;

		// Token: 0x040012B7 RID: 4791
		public string text;
	}
}
