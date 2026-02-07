using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MiniExcelLibs.Attributes;

namespace MiniExcelLibs.OpenXml.Styles
{
	// Token: 0x02000053 RID: 83
	internal class DefaultSheetStyleBuilder : SheetStyleBuilderBase
	{
		// Token: 0x0600028B RID: 651 RVA: 0x0000DC92 File Offset: 0x0000BE92
		public DefaultSheetStyleBuilder(SheetStyleBuildContext context) : base(context)
		{
			this._context = context;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000DCA2 File Offset: 0x0000BEA2
		protected override SheetStyleElementInfos GetGenerateElementInfos()
		{
			return DefaultSheetStyleBuilder.GenerateElementInfos;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000DCAC File Offset: 0x0000BEAC
		protected override void GenerateNumFmt()
		{
			int num = 0;
			foreach (ExcelColumnAttribute excelColumnAttribute in this._context.ColumnsToApply)
			{
				num++;
				this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "numFmt", this._context.OldXmlReader.NamespaceURI);
				this._context.NewXmlWriter.WriteAttributeString("numFmtId", (166 + num + this._context.OldElementInfos.NumFmtCount).ToString());
				this._context.NewXmlWriter.WriteAttributeString("formatCode", excelColumnAttribute.Format);
				this._context.NewXmlWriter.WriteFullEndElement();
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000DD98 File Offset: 0x0000BF98
		protected override Task GenerateNumFmtAsync()
		{
			DefaultSheetStyleBuilder.<GenerateNumFmtAsync>d__5 <GenerateNumFmtAsync>d__;
			<GenerateNumFmtAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateNumFmtAsync>d__.<>4__this = this;
			<GenerateNumFmtAsync>d__.<>1__state = -1;
			<GenerateNumFmtAsync>d__.<>t__builder.Start<DefaultSheetStyleBuilder.<GenerateNumFmtAsync>d__5>(ref <GenerateNumFmtAsync>d__);
			return <GenerateNumFmtAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000DDDC File Offset: 0x0000BFDC
		protected override void GenerateFont()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "font", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "vertAlign", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("val", "baseline");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "sz", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("val", "11");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "color", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "FF000000");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "name", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("val", "Calibri");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "family", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("val", "2");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "font", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "vertAlign", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("val", "baseline");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "sz", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("val", "11");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "color", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "FFFFFFFF");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "name", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("val", "Calibri");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "family", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("val", "2");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000E22C File Offset: 0x0000C42C
		protected override Task GenerateFontAsync()
		{
			DefaultSheetStyleBuilder.<GenerateFontAsync>d__7 <GenerateFontAsync>d__;
			<GenerateFontAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateFontAsync>d__.<>4__this = this;
			<GenerateFontAsync>d__.<>1__state = -1;
			<GenerateFontAsync>d__.<>t__builder.Start<DefaultSheetStyleBuilder.<GenerateFontAsync>d__7>(ref <GenerateFontAsync>d__);
			return <GenerateFontAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000E270 File Offset: 0x0000C470
		protected override void GenerateFill()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "fill", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "patternFill", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("patternType", "none");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "fill", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "patternFill", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("patternType", "gray125");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "fill", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "patternFill", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("patternType", "solid");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "fgColor", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "284472C4");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000E4C8 File Offset: 0x0000C6C8
		protected override Task GenerateFillAsync()
		{
			DefaultSheetStyleBuilder.<GenerateFillAsync>d__9 <GenerateFillAsync>d__;
			<GenerateFillAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateFillAsync>d__.<>4__this = this;
			<GenerateFillAsync>d__.<>1__state = -1;
			<GenerateFillAsync>d__.<>t__builder.Start<DefaultSheetStyleBuilder.<GenerateFillAsync>d__9>(ref <GenerateFillAsync>d__);
			return <GenerateFillAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000E50C File Offset: 0x0000C70C
		protected override void GenerateBorder()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "border", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("diagonalUp", "0");
			this._context.NewXmlWriter.WriteAttributeString("diagonalDown", "0");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "left", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("style", "none");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "color", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "FF000000");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "right", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("style", "none");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "color", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "FF000000");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "top", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("style", "none");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "color", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "FF000000");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "bottom", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("style", "none");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "color", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "FF000000");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "diagonal", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("style", "none");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "color", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "FF000000");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "border", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("diagonalUp", "0");
			this._context.NewXmlWriter.WriteAttributeString("diagonalDown", "0");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "left", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("style", "thin");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "color", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "FF000000");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "right", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("style", "thin");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "color", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "FF000000");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "top", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("style", "thin");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "color", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "FF000000");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "bottom", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("style", "thin");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "color", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "FF000000");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "diagonal", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("style", "none");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "color", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("rgb", "FF000000");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000ED78 File Offset: 0x0000CF78
		protected override Task GenerateBorderAsync()
		{
			DefaultSheetStyleBuilder.<GenerateBorderAsync>d__11 <GenerateBorderAsync>d__;
			<GenerateBorderAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateBorderAsync>d__.<>4__this = this;
			<GenerateBorderAsync>d__.<>1__state = -1;
			<GenerateBorderAsync>d__.<>t__builder.Start<DefaultSheetStyleBuilder.<GenerateBorderAsync>d__11>(ref <GenerateBorderAsync>d__);
			return <GenerateBorderAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000EDBC File Offset: 0x0000CFBC
		protected override void GenerateCellStyleXf()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("numFmtId", "0");
			this._context.NewXmlWriter.WriteAttributeString("fontId", string.Format("{0}", this._context.OldElementInfos.FontCount));
			this._context.NewXmlWriter.WriteAttributeString("fillId", string.Format("{0}", this._context.OldElementInfos.FillCount));
			this._context.NewXmlWriter.WriteAttributeString("borderId", string.Format("{0}", this._context.OldElementInfos.BorderCount));
			this._context.NewXmlWriter.WriteAttributeString("applyNumberFormat", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyFill", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyBorder", "0");
			this._context.NewXmlWriter.WriteAttributeString("applyAlignment", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyProtection", "1");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "protection", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("locked", "1");
			this._context.NewXmlWriter.WriteAttributeString("hidden", "0");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("numFmtId", "14");
			this._context.NewXmlWriter.WriteAttributeString("fontId", string.Format("{0}", this._context.OldElementInfos.FontCount + 1));
			this._context.NewXmlWriter.WriteAttributeString("fillId", string.Format("{0}", this._context.OldElementInfos.FillCount + 2));
			this._context.NewXmlWriter.WriteAttributeString("borderId", string.Format("{0}", this._context.OldElementInfos.BorderCount + 1));
			this._context.NewXmlWriter.WriteAttributeString("applyNumberFormat", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyFill", "0");
			this._context.NewXmlWriter.WriteAttributeString("applyBorder", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyAlignment", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyProtection", "1");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "protection", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("locked", "1");
			this._context.NewXmlWriter.WriteAttributeString("hidden", "0");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("numFmtId", "0");
			this._context.NewXmlWriter.WriteAttributeString("fontId", string.Format("{0}", this._context.OldElementInfos.FontCount));
			this._context.NewXmlWriter.WriteAttributeString("fillId", string.Format("{0}", this._context.OldElementInfos.FillCount));
			this._context.NewXmlWriter.WriteAttributeString("borderId", string.Format("{0}", this._context.OldElementInfos.BorderCount + 1));
			this._context.NewXmlWriter.WriteAttributeString("applyNumberFormat", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyFill", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyBorder", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyAlignment", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyProtection", "1");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "protection", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("locked", "1");
			this._context.NewXmlWriter.WriteAttributeString("hidden", "0");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000F3B4 File Offset: 0x0000D5B4
		protected override Task GenerateCellStyleXfAsync()
		{
			DefaultSheetStyleBuilder.<GenerateCellStyleXfAsync>d__13 <GenerateCellStyleXfAsync>d__;
			<GenerateCellStyleXfAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateCellStyleXfAsync>d__.<>4__this = this;
			<GenerateCellStyleXfAsync>d__.<>1__state = -1;
			<GenerateCellStyleXfAsync>d__.<>t__builder.Start<DefaultSheetStyleBuilder.<GenerateCellStyleXfAsync>d__13>(ref <GenerateCellStyleXfAsync>d__);
			return <GenerateCellStyleXfAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000F3F8 File Offset: 0x0000D5F8
		protected override void GenerateCellXf()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("numFmtId", "0");
			this._context.NewXmlWriter.WriteAttributeString("fontId", string.Format("{0}", this._context.OldElementInfos.FontCount + 1));
			this._context.NewXmlWriter.WriteAttributeString("fillId", string.Format("{0}", this._context.OldElementInfos.FillCount + 2));
			this._context.NewXmlWriter.WriteAttributeString("borderId", string.Format("{0}", this._context.OldElementInfos.BorderCount + 1));
			this._context.NewXmlWriter.WriteAttributeString("xfId", "0");
			this._context.NewXmlWriter.WriteAttributeString("applyNumberFormat", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyFill", "0");
			this._context.NewXmlWriter.WriteAttributeString("applyBorder", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyAlignment", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyProtection", "1");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "alignment", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("horizontal", "left");
			this._context.NewXmlWriter.WriteAttributeString("vertical", "bottom");
			this._context.NewXmlWriter.WriteAttributeString("textRotation", "0");
			this._context.NewXmlWriter.WriteAttributeString("wrapText", "0");
			this._context.NewXmlWriter.WriteAttributeString("indent", "0");
			this._context.NewXmlWriter.WriteAttributeString("relativeIndent", "0");
			this._context.NewXmlWriter.WriteAttributeString("justifyLastLine", "0");
			this._context.NewXmlWriter.WriteAttributeString("shrinkToFit", "0");
			this._context.NewXmlWriter.WriteAttributeString("readingOrder", "0");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "protection", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("locked", "1");
			this._context.NewXmlWriter.WriteAttributeString("hidden", "0");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("numFmtId", "0");
			this._context.NewXmlWriter.WriteAttributeString("fontId", string.Format("{0}", this._context.OldElementInfos.FontCount));
			this._context.NewXmlWriter.WriteAttributeString("fillId", string.Format("{0}", this._context.OldElementInfos.FillCount));
			this._context.NewXmlWriter.WriteAttributeString("borderId", string.Format("{0}", this._context.OldElementInfos.BorderCount + 1));
			this._context.NewXmlWriter.WriteAttributeString("xfId", "0");
			this._context.NewXmlWriter.WriteAttributeString("applyNumberFormat", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyFill", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyBorder", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyAlignment", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyProtection", "1");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "alignment", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("horizontal", "general");
			this._context.NewXmlWriter.WriteAttributeString("vertical", "bottom");
			this._context.NewXmlWriter.WriteAttributeString("textRotation", "0");
			this._context.NewXmlWriter.WriteAttributeString("wrapText", "0");
			this._context.NewXmlWriter.WriteAttributeString("indent", "0");
			this._context.NewXmlWriter.WriteAttributeString("relativeIndent", "0");
			this._context.NewXmlWriter.WriteAttributeString("justifyLastLine", "0");
			this._context.NewXmlWriter.WriteAttributeString("shrinkToFit", "0");
			this._context.NewXmlWriter.WriteAttributeString("readingOrder", "0");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "protection", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("locked", "1");
			this._context.NewXmlWriter.WriteAttributeString("hidden", "0");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("numFmtId", "14");
			this._context.NewXmlWriter.WriteAttributeString("fontId", string.Format("{0}", this._context.OldElementInfos.FontCount));
			this._context.NewXmlWriter.WriteAttributeString("fillId", string.Format("{0}", this._context.OldElementInfos.FillCount));
			this._context.NewXmlWriter.WriteAttributeString("borderId", string.Format("{0}", this._context.OldElementInfos.BorderCount + 1));
			this._context.NewXmlWriter.WriteAttributeString("xfId", "0");
			this._context.NewXmlWriter.WriteAttributeString("applyNumberFormat", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyFill", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyBorder", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyAlignment", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyProtection", "1");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "alignment", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("horizontal", "general");
			this._context.NewXmlWriter.WriteAttributeString("vertical", "bottom");
			this._context.NewXmlWriter.WriteAttributeString("textRotation", "0");
			this._context.NewXmlWriter.WriteAttributeString("wrapText", "0");
			this._context.NewXmlWriter.WriteAttributeString("indent", "0");
			this._context.NewXmlWriter.WriteAttributeString("relativeIndent", "0");
			this._context.NewXmlWriter.WriteAttributeString("justifyLastLine", "0");
			this._context.NewXmlWriter.WriteAttributeString("shrinkToFit", "0");
			this._context.NewXmlWriter.WriteAttributeString("readingOrder", "0");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "protection", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("locked", "1");
			this._context.NewXmlWriter.WriteAttributeString("hidden", "0");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("numFmtId", "0");
			this._context.NewXmlWriter.WriteAttributeString("fontId", string.Format("{0}", this._context.OldElementInfos.FontCount));
			this._context.NewXmlWriter.WriteAttributeString("fillId", string.Format("{0}", this._context.OldElementInfos.FillCount));
			this._context.NewXmlWriter.WriteAttributeString("borderId", string.Format("{0}", this._context.OldElementInfos.BorderCount + 1));
			this._context.NewXmlWriter.WriteAttributeString("xfId", "0");
			this._context.NewXmlWriter.WriteAttributeString("applyBorder", "1");
			this._context.NewXmlWriter.WriteAttributeString("applyAlignment", "1");
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "alignment", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("horizontal", "fill");
			this._context.NewXmlWriter.WriteEndElement();
			this._context.NewXmlWriter.WriteEndElement();
			int num = 0;
			foreach (ExcelColumnAttribute excelColumnAttribute in this._context.ColumnsToApply)
			{
				num++;
				this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
				this._context.NewXmlWriter.WriteAttributeString("numFmtId", (166 + num + this._context.OldElementInfos.NumFmtCount).ToString());
				this._context.NewXmlWriter.WriteAttributeString("fontId", string.Format("{0}", this._context.OldElementInfos.FontCount));
				this._context.NewXmlWriter.WriteAttributeString("fillId", string.Format("{0}", this._context.OldElementInfos.FillCount));
				this._context.NewXmlWriter.WriteAttributeString("borderId", string.Format("{0}", this._context.OldElementInfos.BorderCount + 1));
				this._context.NewXmlWriter.WriteAttributeString("xfId", "0");
				this._context.NewXmlWriter.WriteAttributeString("applyNumberFormat", "1");
				this._context.NewXmlWriter.WriteAttributeString("applyFill", "1");
				this._context.NewXmlWriter.WriteAttributeString("applyBorder", "1");
				this._context.NewXmlWriter.WriteAttributeString("applyAlignment", "1");
				this._context.NewXmlWriter.WriteAttributeString("applyProtection", "1");
				this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "alignment", this._context.OldXmlReader.NamespaceURI);
				this._context.NewXmlWriter.WriteAttributeString("horizontal", "general");
				this._context.NewXmlWriter.WriteAttributeString("vertical", "bottom");
				this._context.NewXmlWriter.WriteAttributeString("textRotation", "0");
				this._context.NewXmlWriter.WriteAttributeString("wrapText", "0");
				this._context.NewXmlWriter.WriteAttributeString("indent", "0");
				this._context.NewXmlWriter.WriteAttributeString("relativeIndent", "0");
				this._context.NewXmlWriter.WriteAttributeString("justifyLastLine", "0");
				this._context.NewXmlWriter.WriteAttributeString("shrinkToFit", "0");
				this._context.NewXmlWriter.WriteAttributeString("readingOrder", "0");
				this._context.NewXmlWriter.WriteEndElement();
				this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "protection", this._context.OldXmlReader.NamespaceURI);
				this._context.NewXmlWriter.WriteAttributeString("locked", "1");
				this._context.NewXmlWriter.WriteAttributeString("hidden", "0");
				this._context.NewXmlWriter.WriteEndElement();
				this._context.NewXmlWriter.WriteEndElement();
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00010370 File Offset: 0x0000E570
		protected override Task GenerateCellXfAsync()
		{
			DefaultSheetStyleBuilder.<GenerateCellXfAsync>d__15 <GenerateCellXfAsync>d__;
			<GenerateCellXfAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateCellXfAsync>d__.<>4__this = this;
			<GenerateCellXfAsync>d__.<>1__state = -1;
			<GenerateCellXfAsync>d__.<>t__builder.Start<DefaultSheetStyleBuilder.<GenerateCellXfAsync>d__15>(ref <GenerateCellXfAsync>d__);
			return <GenerateCellXfAsync>d__.<>t__builder.Task;
		}

		// Token: 0x040000F1 RID: 241
		internal static SheetStyleElementInfos GenerateElementInfos = new SheetStyleElementInfos
		{
			NumFmtCount = 0,
			FontCount = 2,
			FillCount = 3,
			BorderCount = 2,
			CellStyleXfCount = 3,
			CellXfCount = 5
		};

		// Token: 0x040000F2 RID: 242
		private readonly SheetStyleBuildContext _context;
	}
}
