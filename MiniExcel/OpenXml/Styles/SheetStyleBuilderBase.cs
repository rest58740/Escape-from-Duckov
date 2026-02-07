using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace MiniExcelLibs.OpenXml.Styles
{
	// Token: 0x02000057 RID: 87
	internal abstract class SheetStyleBuilderBase : ISheetStyleBuilder
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x0001116B File Offset: 0x0000F36B
		public SheetStyleBuilderBase(SheetStyleBuildContext context)
		{
			this._context = context;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0001117C File Offset: 0x0000F37C
		public virtual SheetStyleBuildResult Build()
		{
			this._context.Initialize(this.GetGenerateElementInfos());
			while (this._context.OldXmlReader.Read())
			{
				switch (this._context.OldXmlReader.NodeType)
				{
				case XmlNodeType.Element:
					this.GenerateElementBeforStartElement();
					this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, this._context.OldXmlReader.LocalName, this._context.OldXmlReader.NamespaceURI);
					this.WriteAttributes(this._context.OldXmlReader.LocalName);
					if (this._context.OldXmlReader.IsEmptyElement)
					{
						this.GenerateElementBeforEndElement();
						this._context.NewXmlWriter.WriteEndElement();
					}
					break;
				case XmlNodeType.Text:
					this._context.NewXmlWriter.WriteString(this._context.OldXmlReader.Value);
					break;
				case XmlNodeType.CDATA:
					this._context.NewXmlWriter.WriteCData(this._context.OldXmlReader.Value);
					break;
				case XmlNodeType.EntityReference:
					this._context.NewXmlWriter.WriteEntityRef(this._context.OldXmlReader.Name);
					break;
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.XmlDeclaration:
					this._context.NewXmlWriter.WriteProcessingInstruction(this._context.OldXmlReader.Name, this._context.OldXmlReader.Value);
					break;
				case XmlNodeType.Comment:
					this._context.NewXmlWriter.WriteComment(this._context.OldXmlReader.Value);
					break;
				case XmlNodeType.DocumentType:
					this._context.NewXmlWriter.WriteDocType(this._context.OldXmlReader.Name, this._context.OldXmlReader.GetAttribute("PUBLIC"), this._context.OldXmlReader.GetAttribute("SYSTEM"), this._context.OldXmlReader.Value);
					break;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					this._context.NewXmlWriter.WriteWhitespace(this._context.OldXmlReader.Value);
					break;
				case XmlNodeType.EndElement:
					this.GenerateElementBeforEndElement();
					this._context.NewXmlWriter.WriteFullEndElement();
					break;
				}
			}
			this._context.FinalizeAndUpdateZipDictionary();
			return new SheetStyleBuildResult(this.GetCellXfIdMap());
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00011418 File Offset: 0x0000F618
		public virtual Task<SheetStyleBuildResult> BuildAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			SheetStyleBuilderBase.<BuildAsync>d__4 <BuildAsync>d__;
			<BuildAsync>d__.<>t__builder = AsyncTaskMethodBuilder<SheetStyleBuildResult>.Create();
			<BuildAsync>d__.<>4__this = this;
			<BuildAsync>d__.cancellationToken = cancellationToken;
			<BuildAsync>d__.<>1__state = -1;
			<BuildAsync>d__.<>t__builder.Start<SheetStyleBuilderBase.<BuildAsync>d__4>(ref <BuildAsync>d__);
			return <BuildAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C6 RID: 710
		protected abstract SheetStyleElementInfos GetGenerateElementInfos();

		// Token: 0x060002C7 RID: 711 RVA: 0x00011464 File Offset: 0x0000F664
		protected virtual void WriteAttributes(string element)
		{
			if (this._context.OldXmlReader.NodeType == XmlNodeType.Element || this._context.OldXmlReader.NodeType == XmlNodeType.XmlDeclaration)
			{
				if (this._context.OldXmlReader.MoveToFirstAttribute())
				{
					this.WriteAttributes(element);
					this._context.OldXmlReader.MoveToElement();
					return;
				}
			}
			else if (this._context.OldXmlReader.NodeType == XmlNodeType.Attribute)
			{
				do
				{
					this._context.NewXmlWriter.WriteStartAttribute(this._context.OldXmlReader.Prefix, this._context.OldXmlReader.LocalName, this._context.OldXmlReader.NamespaceURI);
					string localName = this._context.OldXmlReader.LocalName;
					while (this._context.OldXmlReader.ReadAttributeValue())
					{
						if (this._context.OldXmlReader.NodeType == XmlNodeType.EntityReference)
						{
							this._context.NewXmlWriter.WriteEntityRef(this._context.OldXmlReader.Name);
						}
						else if (localName == "count")
						{
							if (!(element == "numFmts"))
							{
								if (!(element == "fonts"))
								{
									if (!(element == "fills"))
									{
										if (!(element == "borders"))
										{
											if (!(element == "cellStyleXfs"))
											{
												if (!(element == "cellXfs"))
												{
													this._context.NewXmlWriter.WriteString(this._context.OldXmlReader.Value);
												}
												else
												{
													this._context.NewXmlWriter.WriteString((this._context.OldElementInfos.CellXfCount + this._context.GenerateElementInfos.CellXfCount + this._context.CustomFormatCount).ToString());
												}
											}
											else
											{
												this._context.NewXmlWriter.WriteString((this._context.OldElementInfos.CellStyleXfCount + this._context.GenerateElementInfos.CellStyleXfCount).ToString());
											}
										}
										else
										{
											this._context.NewXmlWriter.WriteString((this._context.OldElementInfos.BorderCount + this._context.GenerateElementInfos.BorderCount).ToString());
										}
									}
									else
									{
										this._context.NewXmlWriter.WriteString((this._context.OldElementInfos.FillCount + this._context.GenerateElementInfos.FillCount).ToString());
									}
								}
								else
								{
									this._context.NewXmlWriter.WriteString((this._context.OldElementInfos.FontCount + this._context.GenerateElementInfos.FontCount).ToString());
								}
							}
							else
							{
								this._context.NewXmlWriter.WriteString((this._context.OldElementInfos.NumFmtCount + this._context.GenerateElementInfos.NumFmtCount + this._context.CustomFormatCount).ToString());
							}
						}
						else
						{
							this._context.NewXmlWriter.WriteString(this._context.OldXmlReader.Value);
						}
					}
					this._context.NewXmlWriter.WriteEndAttribute();
				}
				while (this._context.OldXmlReader.MoveToNextAttribute());
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000117E4 File Offset: 0x0000F9E4
		protected virtual Task WriteAttributesAsync(string element)
		{
			SheetStyleBuilderBase.<WriteAttributesAsync>d__7 <WriteAttributesAsync>d__;
			<WriteAttributesAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteAttributesAsync>d__.<>4__this = this;
			<WriteAttributesAsync>d__.element = element;
			<WriteAttributesAsync>d__.<>1__state = -1;
			<WriteAttributesAsync>d__.<>t__builder.Start<SheetStyleBuilderBase.<WriteAttributesAsync>d__7>(ref <WriteAttributesAsync>d__);
			return <WriteAttributesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00011830 File Offset: 0x0000FA30
		protected virtual void GenerateElementBeforStartElement()
		{
			int num;
			if (!SheetStyleBuilderBase._allElements.TryGetValue(this._context.OldXmlReader.LocalName, out num))
			{
				return;
			}
			if (!this._context.OldElementInfos.ExistsNumFmts && !this._context.GenerateElementInfos.ExistsNumFmts && SheetStyleBuilderBase._allElements["numFmts"] < num)
			{
				this.GenerateNumFmts();
				this._context.GenerateElementInfos.ExistsNumFmts = true;
				return;
			}
			if (!this._context.OldElementInfos.ExistsFonts && !this._context.GenerateElementInfos.ExistsFonts && SheetStyleBuilderBase._allElements["fonts"] < num)
			{
				this.GenerateFonts();
				this._context.GenerateElementInfos.ExistsFonts = true;
				return;
			}
			if (!this._context.OldElementInfos.ExistsFills && !this._context.GenerateElementInfos.ExistsFills && SheetStyleBuilderBase._allElements["fills"] < num)
			{
				this.GenerateFills();
				this._context.GenerateElementInfos.ExistsFills = true;
				return;
			}
			if (!this._context.OldElementInfos.ExistsBorders && !this._context.GenerateElementInfos.ExistsBorders && SheetStyleBuilderBase._allElements["borders"] < num)
			{
				this.GenerateBorders();
				this._context.GenerateElementInfos.ExistsBorders = true;
				return;
			}
			if (!this._context.OldElementInfos.ExistsCellStyleXfs && !this._context.GenerateElementInfos.ExistsCellStyleXfs && SheetStyleBuilderBase._allElements["cellStyleXfs"] < num)
			{
				this.GenerateCellStyleXfs();
				this._context.GenerateElementInfos.ExistsCellStyleXfs = true;
				return;
			}
			if (!this._context.OldElementInfos.ExistsCellXfs && !this._context.GenerateElementInfos.ExistsCellXfs && SheetStyleBuilderBase._allElements["cellXfs"] < num)
			{
				this.GenerateCellXfs();
				this._context.GenerateElementInfos.ExistsCellXfs = true;
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00011A30 File Offset: 0x0000FC30
		protected virtual Task GenerateElementBeforStartElementAsync()
		{
			SheetStyleBuilderBase.<GenerateElementBeforStartElementAsync>d__9 <GenerateElementBeforStartElementAsync>d__;
			<GenerateElementBeforStartElementAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateElementBeforStartElementAsync>d__.<>4__this = this;
			<GenerateElementBeforStartElementAsync>d__.<>1__state = -1;
			<GenerateElementBeforStartElementAsync>d__.<>t__builder.Start<SheetStyleBuilderBase.<GenerateElementBeforStartElementAsync>d__9>(ref <GenerateElementBeforStartElementAsync>d__);
			return <GenerateElementBeforStartElementAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00011A74 File Offset: 0x0000FC74
		protected virtual void GenerateElementBeforEndElement()
		{
			if (this._context.OldXmlReader.LocalName == "styleSheet" && !this._context.OldElementInfos.ExistsNumFmts && !this._context.GenerateElementInfos.ExistsNumFmts)
			{
				this.GenerateNumFmts();
				return;
			}
			if (this._context.OldXmlReader.LocalName == "numFmts")
			{
				this.GenerateNumFmt();
				return;
			}
			if (this._context.OldXmlReader.LocalName == "fonts")
			{
				this.GenerateFont();
				return;
			}
			if (this._context.OldXmlReader.LocalName == "fills")
			{
				this.GenerateFill();
				return;
			}
			if (this._context.OldXmlReader.LocalName == "borders")
			{
				this.GenerateBorder();
				return;
			}
			if (this._context.OldXmlReader.LocalName == "cellStyleXfs")
			{
				this.GenerateCellStyleXf();
				return;
			}
			if (this._context.OldXmlReader.LocalName == "cellXfs")
			{
				this.GenerateCellXf();
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00011B9C File Offset: 0x0000FD9C
		protected virtual Task GenerateElementBeforEndElementAsync()
		{
			SheetStyleBuilderBase.<GenerateElementBeforEndElementAsync>d__11 <GenerateElementBeforEndElementAsync>d__;
			<GenerateElementBeforEndElementAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateElementBeforEndElementAsync>d__.<>4__this = this;
			<GenerateElementBeforEndElementAsync>d__.<>1__state = -1;
			<GenerateElementBeforEndElementAsync>d__.<>t__builder.Start<SheetStyleBuilderBase.<GenerateElementBeforEndElementAsync>d__11>(ref <GenerateElementBeforEndElementAsync>d__);
			return <GenerateElementBeforEndElementAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00011BE0 File Offset: 0x0000FDE0
		protected virtual void GenerateNumFmts()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "numFmts", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("count", (this._context.OldElementInfos.NumFmtCount + this._context.GenerateElementInfos.NumFmtCount + this._context.CustomFormatCount).ToString());
			this.GenerateNumFmt();
			this._context.NewXmlWriter.WriteFullEndElement();
			if (!this._context.OldElementInfos.ExistsFonts)
			{
				this.GenerateFonts();
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00011C9C File Offset: 0x0000FE9C
		protected virtual Task GenerateNumFmtsAsync()
		{
			SheetStyleBuilderBase.<GenerateNumFmtsAsync>d__13 <GenerateNumFmtsAsync>d__;
			<GenerateNumFmtsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateNumFmtsAsync>d__.<>4__this = this;
			<GenerateNumFmtsAsync>d__.<>1__state = -1;
			<GenerateNumFmtsAsync>d__.<>t__builder.Start<SheetStyleBuilderBase.<GenerateNumFmtsAsync>d__13>(ref <GenerateNumFmtsAsync>d__);
			return <GenerateNumFmtsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002CF RID: 719
		protected abstract void GenerateNumFmt();

		// Token: 0x060002D0 RID: 720
		protected abstract Task GenerateNumFmtAsync();

		// Token: 0x060002D1 RID: 721 RVA: 0x00011CE0 File Offset: 0x0000FEE0
		protected virtual void GenerateFonts()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "fonts", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("count", (this._context.OldElementInfos.FontCount + this._context.GenerateElementInfos.FontCount).ToString());
			this.GenerateFont();
			this._context.NewXmlWriter.WriteFullEndElement();
			if (!this._context.OldElementInfos.ExistsFills)
			{
				this.GenerateFills();
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00011D90 File Offset: 0x0000FF90
		protected virtual Task GenerateFontsAsync()
		{
			SheetStyleBuilderBase.<GenerateFontsAsync>d__17 <GenerateFontsAsync>d__;
			<GenerateFontsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateFontsAsync>d__.<>4__this = this;
			<GenerateFontsAsync>d__.<>1__state = -1;
			<GenerateFontsAsync>d__.<>t__builder.Start<SheetStyleBuilderBase.<GenerateFontsAsync>d__17>(ref <GenerateFontsAsync>d__);
			return <GenerateFontsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002D3 RID: 723
		protected abstract void GenerateFont();

		// Token: 0x060002D4 RID: 724
		protected abstract Task GenerateFontAsync();

		// Token: 0x060002D5 RID: 725 RVA: 0x00011DD4 File Offset: 0x0000FFD4
		protected virtual void GenerateFills()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "fills", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("count", (this._context.OldElementInfos.FillCount + this._context.GenerateElementInfos.FillCount).ToString());
			this.GenerateFill();
			this._context.NewXmlWriter.WriteFullEndElement();
			if (!this._context.OldElementInfos.ExistsBorders)
			{
				this.GenerateBorders();
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00011E84 File Offset: 0x00010084
		protected virtual Task GenerateFillsAsync()
		{
			SheetStyleBuilderBase.<GenerateFillsAsync>d__21 <GenerateFillsAsync>d__;
			<GenerateFillsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateFillsAsync>d__.<>4__this = this;
			<GenerateFillsAsync>d__.<>1__state = -1;
			<GenerateFillsAsync>d__.<>t__builder.Start<SheetStyleBuilderBase.<GenerateFillsAsync>d__21>(ref <GenerateFillsAsync>d__);
			return <GenerateFillsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002D7 RID: 727
		protected abstract void GenerateFill();

		// Token: 0x060002D8 RID: 728
		protected abstract Task GenerateFillAsync();

		// Token: 0x060002D9 RID: 729 RVA: 0x00011EC8 File Offset: 0x000100C8
		protected virtual void GenerateBorders()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "borders", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("count", (this._context.OldElementInfos.BorderCount + this._context.GenerateElementInfos.BorderCount).ToString());
			this.GenerateBorder();
			this._context.NewXmlWriter.WriteFullEndElement();
			if (!this._context.OldElementInfos.ExistsCellStyleXfs)
			{
				this.GenerateCellStyleXfs();
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00011F78 File Offset: 0x00010178
		protected virtual Task GenerateBordersAsync()
		{
			SheetStyleBuilderBase.<GenerateBordersAsync>d__25 <GenerateBordersAsync>d__;
			<GenerateBordersAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateBordersAsync>d__.<>4__this = this;
			<GenerateBordersAsync>d__.<>1__state = -1;
			<GenerateBordersAsync>d__.<>t__builder.Start<SheetStyleBuilderBase.<GenerateBordersAsync>d__25>(ref <GenerateBordersAsync>d__);
			return <GenerateBordersAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002DB RID: 731
		protected abstract void GenerateBorder();

		// Token: 0x060002DC RID: 732
		protected abstract Task GenerateBorderAsync();

		// Token: 0x060002DD RID: 733 RVA: 0x00011FBC File Offset: 0x000101BC
		protected virtual void GenerateCellStyleXfs()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "cellStyleXfs", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("count", (this._context.OldElementInfos.CellStyleXfCount + this._context.GenerateElementInfos.CellStyleXfCount).ToString());
			this.GenerateCellStyleXf();
			this._context.NewXmlWriter.WriteFullEndElement();
			if (!this._context.OldElementInfos.ExistsCellXfs)
			{
				this.GenerateCellXfs();
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0001206C File Offset: 0x0001026C
		protected virtual Task GenerateCellStyleXfsAsync()
		{
			SheetStyleBuilderBase.<GenerateCellStyleXfsAsync>d__29 <GenerateCellStyleXfsAsync>d__;
			<GenerateCellStyleXfsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateCellStyleXfsAsync>d__.<>4__this = this;
			<GenerateCellStyleXfsAsync>d__.<>1__state = -1;
			<GenerateCellStyleXfsAsync>d__.<>t__builder.Start<SheetStyleBuilderBase.<GenerateCellStyleXfsAsync>d__29>(ref <GenerateCellStyleXfsAsync>d__);
			return <GenerateCellStyleXfsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002DF RID: 735
		protected abstract void GenerateCellStyleXf();

		// Token: 0x060002E0 RID: 736
		protected abstract Task GenerateCellStyleXfAsync();

		// Token: 0x060002E1 RID: 737 RVA: 0x000120B0 File Offset: 0x000102B0
		protected virtual void GenerateCellXfs()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "cellXfs", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("count", (this._context.OldElementInfos.CellXfCount + this._context.GenerateElementInfos.CellXfCount + this._context.CustomFormatCount).ToString());
			this.GenerateCellXf();
			this._context.NewXmlWriter.WriteFullEndElement();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00012154 File Offset: 0x00010354
		protected virtual Task GenerateCellXfsAsync()
		{
			SheetStyleBuilderBase.<GenerateCellXfsAsync>d__33 <GenerateCellXfsAsync>d__;
			<GenerateCellXfsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateCellXfsAsync>d__.<>4__this = this;
			<GenerateCellXfsAsync>d__.<>1__state = -1;
			<GenerateCellXfsAsync>d__.<>t__builder.Start<SheetStyleBuilderBase.<GenerateCellXfsAsync>d__33>(ref <GenerateCellXfsAsync>d__);
			return <GenerateCellXfsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E3 RID: 739
		protected abstract void GenerateCellXf();

		// Token: 0x060002E4 RID: 740
		protected abstract Task GenerateCellXfAsync();

		// Token: 0x060002E5 RID: 741 RVA: 0x00012198 File Offset: 0x00010398
		private Dictionary<string, string> GetCellXfIdMap()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			for (int i = 0; i < this._context.GenerateElementInfos.CellXfCount; i++)
			{
				dictionary.Add(i.ToString(), (this._context.OldElementInfos.CellXfCount + i).ToString());
			}
			return dictionary;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000121F0 File Offset: 0x000103F0
		// Note: this type is marked as 'beforefieldinit'.
		static SheetStyleBuilderBase()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary["numFmts"] = 0;
			dictionary["fonts"] = 1;
			dictionary["fills"] = 2;
			dictionary["borders"] = 3;
			dictionary["cellStyleXfs"] = 4;
			dictionary["cellXfs"] = 5;
			dictionary["cellStyles"] = 6;
			dictionary["dxfs"] = 7;
			dictionary["tableStyles"] = 8;
			dictionary["extLst"] = 9;
			SheetStyleBuilderBase._allElements = dictionary;
		}

		// Token: 0x04000108 RID: 264
		internal static readonly Dictionary<string, int> _allElements;

		// Token: 0x04000109 RID: 265
		private readonly SheetStyleBuildContext _context;
	}
}
