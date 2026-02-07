using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using Mono.Xml;

namespace System.Security
{
	// Token: 0x020003E6 RID: 998
	[ComVisible(true)]
	[Serializable]
	public sealed class SecurityElement
	{
		// Token: 0x06002910 RID: 10512 RVA: 0x00094E4C File Offset: 0x0009304C
		public SecurityElement(string tag) : this(tag, null)
		{
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x00094E58 File Offset: 0x00093058
		public SecurityElement(string tag, string text)
		{
			if (tag == null)
			{
				throw new ArgumentNullException("tag");
			}
			if (!SecurityElement.IsValidTag(tag))
			{
				throw new ArgumentException(Locale.GetText("Invalid XML string") + ": " + tag);
			}
			this.tag = tag;
			this.Text = text;
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x00094EAC File Offset: 0x000930AC
		internal SecurityElement(SecurityElement se)
		{
			this.Tag = se.Tag;
			this.Text = se.Text;
			if (se.attributes != null)
			{
				foreach (object obj in se.attributes)
				{
					SecurityElement.SecurityAttribute securityAttribute = (SecurityElement.SecurityAttribute)obj;
					this.AddAttribute(securityAttribute.Name, securityAttribute.Value);
				}
			}
			if (se.children != null)
			{
				foreach (object obj2 in se.children)
				{
					SecurityElement child = (SecurityElement)obj2;
					this.AddChild(child);
				}
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06002913 RID: 10515 RVA: 0x00094F88 File Offset: 0x00093188
		// (set) Token: 0x06002914 RID: 10516 RVA: 0x00095008 File Offset: 0x00093208
		public Hashtable Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					return null;
				}
				Hashtable hashtable = new Hashtable(this.attributes.Count);
				foreach (object obj in this.attributes)
				{
					SecurityElement.SecurityAttribute securityAttribute = (SecurityElement.SecurityAttribute)obj;
					hashtable.Add(securityAttribute.Name, securityAttribute.Value);
				}
				return hashtable;
			}
			set
			{
				if (value == null || value.Count == 0)
				{
					this.attributes.Clear();
					return;
				}
				if (this.attributes == null)
				{
					this.attributes = new ArrayList();
				}
				else
				{
					this.attributes.Clear();
				}
				IDictionaryEnumerator enumerator = value.GetEnumerator();
				while (enumerator.MoveNext())
				{
					this.attributes.Add(new SecurityElement.SecurityAttribute((string)enumerator.Key, (string)enumerator.Value));
				}
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06002915 RID: 10517 RVA: 0x00095084 File Offset: 0x00093284
		// (set) Token: 0x06002916 RID: 10518 RVA: 0x0009508C File Offset: 0x0009328C
		public ArrayList Children
		{
			get
			{
				return this.children;
			}
			set
			{
				if (value != null)
				{
					using (IEnumerator enumerator = value.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == null)
							{
								throw new ArgumentNullException();
							}
						}
					}
				}
				this.children = value;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06002917 RID: 10519 RVA: 0x000950E8 File Offset: 0x000932E8
		// (set) Token: 0x06002918 RID: 10520 RVA: 0x000950F0 File Offset: 0x000932F0
		public string Tag
		{
			get
			{
				return this.tag;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Tag");
				}
				if (!SecurityElement.IsValidTag(value))
				{
					throw new ArgumentException(Locale.GetText("Invalid XML string") + ": " + value);
				}
				this.tag = value;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06002919 RID: 10521 RVA: 0x0009512A File Offset: 0x0009332A
		// (set) Token: 0x0600291A RID: 10522 RVA: 0x00095132 File Offset: 0x00093332
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (value != null && !SecurityElement.IsValidText(value))
				{
					throw new ArgumentException(Locale.GetText("Invalid XML string") + ": " + value);
				}
				this.text = SecurityElement.Unescape(value);
			}
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x00095168 File Offset: 0x00093368
		public void AddAttribute(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.GetAttribute(name) != null)
			{
				throw new ArgumentException(Locale.GetText("Duplicate attribute : " + name));
			}
			if (this.attributes == null)
			{
				this.attributes = new ArrayList();
			}
			this.attributes.Add(new SecurityElement.SecurityAttribute(name, value));
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x000951D6 File Offset: 0x000933D6
		public void AddChild(SecurityElement child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			if (this.children == null)
			{
				this.children = new ArrayList();
			}
			this.children.Add(child);
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x00095208 File Offset: 0x00093408
		public string Attribute(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			SecurityElement.SecurityAttribute attribute = this.GetAttribute(name);
			if (attribute != null)
			{
				return attribute.Value;
			}
			return null;
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x00095236 File Offset: 0x00093436
		[ComVisible(false)]
		public SecurityElement Copy()
		{
			return new SecurityElement(this);
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x00095240 File Offset: 0x00093440
		public bool Equal(SecurityElement other)
		{
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			if (this.text != other.text)
			{
				return false;
			}
			if (this.tag != other.tag)
			{
				return false;
			}
			if (this.attributes == null && other.attributes != null && other.attributes.Count != 0)
			{
				return false;
			}
			if (other.attributes == null && this.attributes != null && this.attributes.Count != 0)
			{
				return false;
			}
			if (this.attributes != null && other.attributes != null)
			{
				if (this.attributes.Count != other.attributes.Count)
				{
					return false;
				}
				foreach (object obj in this.attributes)
				{
					SecurityElement.SecurityAttribute securityAttribute = (SecurityElement.SecurityAttribute)obj;
					SecurityElement.SecurityAttribute attribute = other.GetAttribute(securityAttribute.Name);
					if (attribute == null || securityAttribute.Value != attribute.Value)
					{
						return false;
					}
				}
			}
			if (this.children == null && other.children != null && other.children.Count != 0)
			{
				return false;
			}
			if (other.children == null && this.children != null && this.children.Count != 0)
			{
				return false;
			}
			if (this.children != null && other.children != null)
			{
				if (this.children.Count != other.children.Count)
				{
					return false;
				}
				for (int i = 0; i < this.children.Count; i++)
				{
					if (!((SecurityElement)this.children[i]).Equal((SecurityElement)other.children[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x00095414 File Offset: 0x00093614
		public static string Escape(string str)
		{
			if (str == null)
			{
				return null;
			}
			if (str.IndexOfAny(SecurityElement.invalid_chars) == -1)
			{
				return str;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int length = str.Length;
			int i = 0;
			while (i < length)
			{
				char c = str[i];
				if (c <= '&')
				{
					if (c != '"')
					{
						if (c != '&')
						{
							goto IL_96;
						}
						stringBuilder.Append("&amp;");
					}
					else
					{
						stringBuilder.Append("&quot;");
					}
				}
				else if (c != '\'')
				{
					if (c != '<')
					{
						if (c != '>')
						{
							goto IL_96;
						}
						stringBuilder.Append("&gt;");
					}
					else
					{
						stringBuilder.Append("&lt;");
					}
				}
				else
				{
					stringBuilder.Append("&apos;");
				}
				IL_9E:
				i++;
				continue;
				IL_96:
				stringBuilder.Append(c);
				goto IL_9E;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x000954D0 File Offset: 0x000936D0
		private static string Unescape(string str)
		{
			if (str == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(str);
			stringBuilder.Replace("&lt;", "<");
			stringBuilder.Replace("&gt;", ">");
			stringBuilder.Replace("&amp;", "&");
			stringBuilder.Replace("&quot;", "\"");
			stringBuilder.Replace("&apos;", "'");
			return stringBuilder.ToString();
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x00095544 File Offset: 0x00093744
		public static SecurityElement FromString(string xml)
		{
			if (xml == null)
			{
				throw new ArgumentNullException("xml");
			}
			if (xml.Length == 0)
			{
				throw new XmlSyntaxException(Locale.GetText("Empty string."));
			}
			SecurityElement result;
			try
			{
				SecurityParser securityParser = new SecurityParser();
				securityParser.LoadXml(xml);
				result = securityParser.ToXml();
			}
			catch (Exception inner)
			{
				throw new XmlSyntaxException(Locale.GetText("Invalid XML."), inner);
			}
			return result;
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x000955B0 File Offset: 0x000937B0
		public static bool IsValidAttributeName(string name)
		{
			return name != null && name.IndexOfAny(SecurityElement.invalid_attr_name_chars) == -1;
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x000955C5 File Offset: 0x000937C5
		public static bool IsValidAttributeValue(string value)
		{
			return value != null && value.IndexOfAny(SecurityElement.invalid_attr_value_chars) == -1;
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x000955DA File Offset: 0x000937DA
		public static bool IsValidTag(string tag)
		{
			return tag != null && tag.IndexOfAny(SecurityElement.invalid_tag_chars) == -1;
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x000955EF File Offset: 0x000937EF
		public static bool IsValidText(string text)
		{
			return text != null && text.IndexOfAny(SecurityElement.invalid_text_chars) == -1;
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x00095604 File Offset: 0x00093804
		public SecurityElement SearchForChildByTag(string tag)
		{
			if (tag == null)
			{
				throw new ArgumentNullException("tag");
			}
			if (this.children == null)
			{
				return null;
			}
			for (int i = 0; i < this.children.Count; i++)
			{
				SecurityElement securityElement = (SecurityElement)this.children[i];
				if (securityElement.tag == tag)
				{
					return securityElement;
				}
			}
			return null;
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x00095664 File Offset: 0x00093864
		public string SearchForTextOfTag(string tag)
		{
			if (tag == null)
			{
				throw new ArgumentNullException("tag");
			}
			if (this.tag == tag)
			{
				return this.text;
			}
			if (this.children == null)
			{
				return null;
			}
			for (int i = 0; i < this.children.Count; i++)
			{
				string text = ((SecurityElement)this.children[i]).SearchForTextOfTag(tag);
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x000956D4 File Offset: 0x000938D4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.ToXml(ref stringBuilder, 0);
			return stringBuilder.ToString();
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000956F8 File Offset: 0x000938F8
		private void ToXml(ref StringBuilder s, int level)
		{
			s.Append("<");
			s.Append(this.tag);
			if (this.attributes != null)
			{
				s.Append(" ");
				for (int i = 0; i < this.attributes.Count; i++)
				{
					SecurityElement.SecurityAttribute securityAttribute = (SecurityElement.SecurityAttribute)this.attributes[i];
					s.Append(securityAttribute.Name).Append("=\"").Append(SecurityElement.Escape(securityAttribute.Value)).Append("\"");
					if (i != this.attributes.Count - 1)
					{
						s.Append(Environment.NewLine);
					}
				}
			}
			if ((this.text == null || this.text == string.Empty) && (this.children == null || this.children.Count == 0))
			{
				s.Append("/>").Append(Environment.NewLine);
				return;
			}
			s.Append(">").Append(SecurityElement.Escape(this.text));
			if (this.children != null)
			{
				s.Append(Environment.NewLine);
				foreach (object obj in this.children)
				{
					((SecurityElement)obj).ToXml(ref s, level + 1);
				}
			}
			s.Append("</").Append(this.tag).Append(">").Append(Environment.NewLine);
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x000958A4 File Offset: 0x00093AA4
		internal SecurityElement.SecurityAttribute GetAttribute(string name)
		{
			if (this.attributes != null)
			{
				foreach (object obj in this.attributes)
				{
					SecurityElement.SecurityAttribute securityAttribute = (SecurityElement.SecurityAttribute)obj;
					if (securityAttribute.Name == name)
					{
						return securityAttribute;
					}
				}
			}
			return null;
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x0600292C RID: 10540 RVA: 0x000950E8 File Offset: 0x000932E8
		internal string m_strTag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x0600292D RID: 10541 RVA: 0x0009512A File Offset: 0x0009332A
		// (set) Token: 0x0600292E RID: 10542 RVA: 0x00095914 File Offset: 0x00093B14
		internal string m_strText
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x0600292F RID: 10543 RVA: 0x0009591D File Offset: 0x00093B1D
		internal ArrayList m_lAttributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06002930 RID: 10544 RVA: 0x00095084 File Offset: 0x00093284
		internal ArrayList InternalChildren
		{
			get
			{
				return this.children;
			}
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x00095928 File Offset: 0x00093B28
		internal string SearchForTextOfLocalName(string strLocalName)
		{
			if (strLocalName == null)
			{
				throw new ArgumentNullException("strLocalName");
			}
			if (this.tag == null)
			{
				return null;
			}
			if (this.tag.Equals(strLocalName) || this.tag.EndsWith(":" + strLocalName, StringComparison.Ordinal))
			{
				return SecurityElement.Unescape(this.text);
			}
			if (this.children == null)
			{
				return null;
			}
			foreach (object obj in this.children)
			{
				string text = ((SecurityElement)obj).SearchForTextOfLocalName(strLocalName);
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x04001ED6 RID: 7894
		private string text;

		// Token: 0x04001ED7 RID: 7895
		private string tag;

		// Token: 0x04001ED8 RID: 7896
		private ArrayList attributes;

		// Token: 0x04001ED9 RID: 7897
		private ArrayList children;

		// Token: 0x04001EDA RID: 7898
		private static readonly char[] invalid_tag_chars = new char[]
		{
			' ',
			'<',
			'>'
		};

		// Token: 0x04001EDB RID: 7899
		private static readonly char[] invalid_text_chars = new char[]
		{
			'<',
			'>'
		};

		// Token: 0x04001EDC RID: 7900
		private static readonly char[] invalid_attr_name_chars = new char[]
		{
			' ',
			'<',
			'>'
		};

		// Token: 0x04001EDD RID: 7901
		private static readonly char[] invalid_attr_value_chars = new char[]
		{
			'"',
			'<',
			'>'
		};

		// Token: 0x04001EDE RID: 7902
		private static readonly char[] invalid_chars = new char[]
		{
			'<',
			'>',
			'"',
			'\'',
			'&'
		};

		// Token: 0x020003E7 RID: 999
		internal class SecurityAttribute
		{
			// Token: 0x06002933 RID: 10547 RVA: 0x00095A34 File Offset: 0x00093C34
			public SecurityAttribute(string name, string value)
			{
				if (!SecurityElement.IsValidAttributeName(name))
				{
					throw new ArgumentException(Locale.GetText("Invalid XML attribute name") + ": " + name);
				}
				if (!SecurityElement.IsValidAttributeValue(value))
				{
					throw new ArgumentException(Locale.GetText("Invalid XML attribute value") + ": " + value);
				}
				this._name = name;
				this._value = SecurityElement.Unescape(value);
			}

			// Token: 0x17000508 RID: 1288
			// (get) Token: 0x06002934 RID: 10548 RVA: 0x00095AA0 File Offset: 0x00093CA0
			public string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x17000509 RID: 1289
			// (get) Token: 0x06002935 RID: 10549 RVA: 0x00095AA8 File Offset: 0x00093CA8
			public string Value
			{
				get
				{
					return this._value;
				}
			}

			// Token: 0x04001EDF RID: 7903
			private string _name;

			// Token: 0x04001EE0 RID: 7904
			private string _value;
		}
	}
}
