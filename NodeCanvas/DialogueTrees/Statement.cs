using System;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000FD RID: 253
	[Serializable]
	public class Statement : IStatement
	{
		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x000111CB File Offset: 0x0000F3CB
		// (set) Token: 0x0600052E RID: 1326 RVA: 0x000111D3 File Offset: 0x0000F3D3
		public string text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x000111DC File Offset: 0x0000F3DC
		// (set) Token: 0x06000530 RID: 1328 RVA: 0x000111E4 File Offset: 0x0000F3E4
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

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x000111ED File Offset: 0x0000F3ED
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x000111F5 File Offset: 0x0000F3F5
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

		// Token: 0x06000533 RID: 1331 RVA: 0x000111FE File Offset: 0x0000F3FE
		public Statement()
		{
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001121C File Offset: 0x0000F41C
		public Statement(string text)
		{
			this.text = text;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00011241 File Offset: 0x0000F441
		public Statement(string text, AudioClip audio)
		{
			this.text = text;
			this.audio = audio;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001126D File Offset: 0x0000F46D
		public Statement(string text, AudioClip audio, string meta)
		{
			this.text = text;
			this.audio = audio;
			this.meta = meta;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x000112A0 File Offset: 0x0000F4A0
		public IStatement BlackboardReplace(IBlackboard bb)
		{
			Statement statement = JSONSerializer.Clone<Statement>(this);
			statement.text = statement.text.ReplaceWithin('[', ']', delegate(string input)
			{
				object obj = null;
				if (bb != null)
				{
					Variable variable = bb.GetVariable(input, typeof(object));
					if (variable != null)
					{
						obj = variable.value;
					}
				}
				if (input.Contains("/"))
				{
					GlobalBlackboard globalBlackboard = GlobalBlackboard.Find(input.Split('/', 0).First<string>());
					if (globalBlackboard != null)
					{
						Variable variable2 = globalBlackboard.GetVariable(input.Split('/', 0).Last<string>(), typeof(object));
						if (variable2 != null)
						{
							obj = variable2.value;
						}
					}
				}
				if (obj == null)
				{
					return input;
				}
				return obj.ToString();
			});
			return statement;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x000112E1 File Offset: 0x0000F4E1
		public override string ToString()
		{
			return this.text;
		}

		// Token: 0x040002DF RID: 735
		[SerializeField]
		private string _text = string.Empty;

		// Token: 0x040002E0 RID: 736
		[SerializeField]
		private AudioClip _audio;

		// Token: 0x040002E1 RID: 737
		[SerializeField]
		private string _meta = string.Empty;
	}
}
