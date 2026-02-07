using System;
using NodeCanvas.DialogueTrees;
using SodaCraft.Localizations;
using UnityEngine;

namespace Dialogues
{
	// Token: 0x02000226 RID: 550
	[Serializable]
	public class LocalizedStatement : IStatement
	{
		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x00041B3F File Offset: 0x0003FD3F
		public string text
		{
			get
			{
				return LocalizationManager.ToPlainText(this.textKey);
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x00041B4C File Offset: 0x0003FD4C
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x00041B54 File Offset: 0x0003FD54
		public string textKey
		{
			get
			{
				return this._textKey;
			}
			set
			{
				this._textKey = value;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x00041B5D File Offset: 0x0003FD5D
		// (set) Token: 0x060010C1 RID: 4289 RVA: 0x00041B65 File Offset: 0x0003FD65
		public AudioClip audio
		{
			get
			{
				return this._audio;
			}
			set
			{
				this._audio = value;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x00041B6E File Offset: 0x0003FD6E
		// (set) Token: 0x060010C3 RID: 4291 RVA: 0x00041B76 File Offset: 0x0003FD76
		public string meta
		{
			get
			{
				return this._meta;
			}
			set
			{
				this._meta = value;
			}
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00041B7F File Offset: 0x0003FD7F
		public LocalizedStatement()
		{
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00041B9D File Offset: 0x0003FD9D
		public LocalizedStatement(string textKey)
		{
			this._textKey = textKey;
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x00041BC2 File Offset: 0x0003FDC2
		public LocalizedStatement(string textKey, AudioClip audio)
		{
			this._textKey = textKey;
			this.audio = audio;
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x00041BEE File Offset: 0x0003FDEE
		public LocalizedStatement(string textKey, AudioClip audio, string meta)
		{
			this._textKey = textKey;
			this.audio = audio;
			this.meta = meta;
		}

		// Token: 0x04000D72 RID: 3442
		[SerializeField]
		private string _textKey = string.Empty;

		// Token: 0x04000D73 RID: 3443
		[SerializeField]
		private AudioClip _audio;

		// Token: 0x04000D74 RID: 3444
		[SerializeField]
		private string _meta = string.Empty;
	}
}
