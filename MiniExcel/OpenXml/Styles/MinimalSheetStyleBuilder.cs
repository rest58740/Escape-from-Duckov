using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MiniExcelLibs.Attributes;

namespace MiniExcelLibs.OpenXml.Styles
{
	// Token: 0x02000055 RID: 85
	internal class MinimalSheetStyleBuilder : SheetStyleBuilderBase
	{
		// Token: 0x0600029C RID: 668 RVA: 0x000103E9 File Offset: 0x0000E5E9
		public MinimalSheetStyleBuilder(SheetStyleBuildContext context) : base(context)
		{
			this._context = context;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x000103F9 File Offset: 0x0000E5F9
		protected override SheetStyleElementInfos GetGenerateElementInfos()
		{
			return MinimalSheetStyleBuilder.GenerateElementInfos;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00010400 File Offset: 0x0000E600
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

		// Token: 0x0600029F RID: 671 RVA: 0x000104EC File Offset: 0x0000E6EC
		protected override Task GenerateNumFmtAsync()
		{
			MinimalSheetStyleBuilder.<GenerateNumFmtAsync>d__5 <GenerateNumFmtAsync>d__;
			<GenerateNumFmtAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateNumFmtAsync>d__.<>4__this = this;
			<GenerateNumFmtAsync>d__.<>1__state = -1;
			<GenerateNumFmtAsync>d__.<>t__builder.Start<MinimalSheetStyleBuilder.<GenerateNumFmtAsync>d__5>(ref <GenerateNumFmtAsync>d__);
			return <GenerateNumFmtAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00010530 File Offset: 0x0000E730
		protected override void GenerateFont()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "font", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteFullEndElement();
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00010584 File Offset: 0x0000E784
		protected override Task GenerateFontAsync()
		{
			MinimalSheetStyleBuilder.<GenerateFontAsync>d__7 <GenerateFontAsync>d__;
			<GenerateFontAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateFontAsync>d__.<>4__this = this;
			<GenerateFontAsync>d__.<>1__state = -1;
			<GenerateFontAsync>d__.<>t__builder.Start<MinimalSheetStyleBuilder.<GenerateFontAsync>d__7>(ref <GenerateFontAsync>d__);
			return <GenerateFontAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x000105C8 File Offset: 0x0000E7C8
		protected override void GenerateFill()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "fill", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteFullEndElement();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0001061C File Offset: 0x0000E81C
		protected override Task GenerateFillAsync()
		{
			MinimalSheetStyleBuilder.<GenerateFillAsync>d__9 <GenerateFillAsync>d__;
			<GenerateFillAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateFillAsync>d__.<>4__this = this;
			<GenerateFillAsync>d__.<>1__state = -1;
			<GenerateFillAsync>d__.<>t__builder.Start<MinimalSheetStyleBuilder.<GenerateFillAsync>d__9>(ref <GenerateFillAsync>d__);
			return <GenerateFillAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00010660 File Offset: 0x0000E860
		protected override void GenerateBorder()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "border", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteFullEndElement();
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000106B4 File Offset: 0x0000E8B4
		protected override Task GenerateBorderAsync()
		{
			MinimalSheetStyleBuilder.<GenerateBorderAsync>d__11 <GenerateBorderAsync>d__;
			<GenerateBorderAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateBorderAsync>d__.<>4__this = this;
			<GenerateBorderAsync>d__.<>1__state = -1;
			<GenerateBorderAsync>d__.<>t__builder.Start<MinimalSheetStyleBuilder.<GenerateBorderAsync>d__11>(ref <GenerateBorderAsync>d__);
			return <GenerateBorderAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000106F8 File Offset: 0x0000E8F8
		protected override void GenerateCellStyleXf()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteFullEndElement();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0001074C File Offset: 0x0000E94C
		protected override Task GenerateCellStyleXfAsync()
		{
			MinimalSheetStyleBuilder.<GenerateCellStyleXfAsync>d__13 <GenerateCellStyleXfAsync>d__;
			<GenerateCellStyleXfAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateCellStyleXfAsync>d__.<>4__this = this;
			<GenerateCellStyleXfAsync>d__.<>1__state = -1;
			<GenerateCellStyleXfAsync>d__.<>t__builder.Start<MinimalSheetStyleBuilder.<GenerateCellStyleXfAsync>d__13>(ref <GenerateCellStyleXfAsync>d__);
			return <GenerateCellStyleXfAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00010790 File Offset: 0x0000E990
		protected override void GenerateCellXf()
		{
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteFullEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteFullEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteFullEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteAttributeString("numFmtId", "14");
			this._context.NewXmlWriter.WriteAttributeString("applyNumberFormat", "1");
			this._context.NewXmlWriter.WriteFullEndElement();
			this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
			this._context.NewXmlWriter.WriteFullEndElement();
			int num = 0;
			foreach (ExcelColumnAttribute excelColumnAttribute in this._context.ColumnsToApply)
			{
				num++;
				this._context.NewXmlWriter.WriteStartElement(this._context.OldXmlReader.Prefix, "xf", this._context.OldXmlReader.NamespaceURI);
				this._context.NewXmlWriter.WriteAttributeString("numFmtId", (166 + num).ToString());
				this._context.NewXmlWriter.WriteAttributeString("applyNumberFormat", "1");
				this._context.NewXmlWriter.WriteFullEndElement();
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x000109F8 File Offset: 0x0000EBF8
		protected override Task GenerateCellXfAsync()
		{
			MinimalSheetStyleBuilder.<GenerateCellXfAsync>d__15 <GenerateCellXfAsync>d__;
			<GenerateCellXfAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateCellXfAsync>d__.<>4__this = this;
			<GenerateCellXfAsync>d__.<>1__state = -1;
			<GenerateCellXfAsync>d__.<>t__builder.Start<MinimalSheetStyleBuilder.<GenerateCellXfAsync>d__15>(ref <GenerateCellXfAsync>d__);
			return <GenerateCellXfAsync>d__.<>t__builder.Task;
		}

		// Token: 0x040000F3 RID: 243
		internal static SheetStyleElementInfos GenerateElementInfos = new SheetStyleElementInfos
		{
			NumFmtCount = 0,
			FontCount = 1,
			FillCount = 1,
			BorderCount = 1,
			CellStyleXfCount = 1,
			CellXfCount = 5
		};

		// Token: 0x040000F4 RID: 244
		private readonly SheetStyleBuildContext _context;
	}
}
