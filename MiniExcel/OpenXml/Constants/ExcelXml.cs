using System;
using MiniExcelLibs.OpenXml.Models;

namespace MiniExcelLibs.OpenXml.Constants
{
	// Token: 0x02000060 RID: 96
	internal static class ExcelXml
	{
		// Token: 0x06000325 RID: 805 RVA: 0x00012548 File Offset: 0x00010748
		static ExcelXml()
		{
			ExcelXml.DefaultRels = ExcelOpenXmlUtils.MinifyXml(ExcelXml.DefaultRels);
			ExcelXml.DefaultWorkbookXml = ExcelOpenXmlUtils.MinifyXml(ExcelXml.DefaultWorkbookXml);
			ExcelXml.DefaultWorkbookXmlRels = ExcelOpenXmlUtils.MinifyXml(ExcelXml.DefaultWorkbookXmlRels);
			ExcelXml.DefaultSheetRelXml = ExcelOpenXmlUtils.MinifyXml(ExcelXml.DefaultSheetRelXml);
			ExcelXml.DefaultDrawing = ExcelOpenXmlUtils.MinifyXml(ExcelXml.DefaultDrawing);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00012604 File Offset: 0x00010804
		internal static string ContentType(string contentType, string partName)
		{
			return string.Concat(new string[]
			{
				"<Override ContentType=\"",
				contentType,
				"\" PartName=\"/",
				partName,
				"\" />"
			});
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00012631 File Offset: 0x00010831
		internal static string WorksheetRelationship(SheetDto sheetDto)
		{
			return string.Concat(new string[]
			{
				"<Relationship Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"/",
				sheetDto.Path,
				"\" Id=\"",
				sheetDto.ID,
				"\" />"
			});
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00012668 File Offset: 0x00010868
		internal static string ImageRelationship(FileDto image)
		{
			return string.Concat(new string[]
			{
				"<Relationship Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/image\" Target=\"",
				image.Path2,
				"\" Id=\"",
				image.ID,
				"\" />"
			});
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0001269F File Offset: 0x0001089F
		internal static string DrawingRelationship(int sheetId)
		{
			return string.Format("<Relationship Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing\" Target=\"../drawings/drawing{0}.xml\" Id=\"drawing{1}\" />", sheetId, sheetId);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x000126B8 File Offset: 0x000108B8
		internal static string DrawingXml(FileDto file, int fileIndex)
		{
			return string.Format("<xdr:oneCellAnchor>\r\n        <xdr:from>\r\n            <xdr:col>{0}</xdr:col>\r\n            <xdr:colOff>0</xdr:colOff>\r\n            <xdr:row>{1}</xdr:row>\r\n            <xdr:rowOff>0</xdr:rowOff>\r\n        </xdr:from>\r\n        <xdr:ext cx=\"609600\" cy=\"190500\" />\r\n        <xdr:pic>\r\n            <xdr:nvPicPr>\r\n                <xdr:cNvPr id=\"{2}\" descr=\"\" name=\"2a3f9147-58ea-4a79-87da-7d6114c4877b\" />\r\n                <xdr:cNvPicPr>\r\n                    <a:picLocks noChangeAspect=\"1\" />\r\n                </xdr:cNvPicPr>\r\n            </xdr:nvPicPr>\r\n            <xdr:blipFill>\r\n                <a:blip r:embed=\"{3}\" cstate=\"print\" />\r\n                <a:stretch>\r\n                    <a:fillRect />\r\n                </a:stretch>\r\n            </xdr:blipFill>\r\n            <xdr:spPr>\r\n                <a:xfrm>\r\n                    <a:off x=\"0\" y=\"0\" />\r\n                    <a:ext cx=\"0\" cy=\"0\" />\r\n                </a:xfrm>\r\n                <a:prstGeom prst=\"rect\">\r\n                    <a:avLst />\r\n                </a:prstGeom>\r\n            </xdr:spPr>\r\n        </xdr:pic>\r\n        <xdr:clientData />\r\n    </xdr:oneCellAnchor>", new object[]
			{
				file.CellIndex - 1,
				file.RowIndex - 1,
				fileIndex + 1,
				file.ID
			});
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0001270C File Offset: 0x0001090C
		internal static string Sheet(SheetDto sheetDto, int sheetId)
		{
			return string.Format("<x:sheet name=\"{0}\" sheetId=\"{1}\"{2} r:id=\"{3}\" />", new object[]
			{
				ExcelOpenXmlUtils.EncodeXML(sheetDto.Name),
				sheetId,
				string.IsNullOrWhiteSpace(sheetDto.State) ? string.Empty : (" state=\"" + sheetDto.State + "\""),
				sheetDto.ID
			});
		}

		// Token: 0x0400012F RID: 303
		internal static readonly string EmptySheetXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><x:worksheet xmlns:x=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><x:dimension ref=\"A1\"/><x:sheetData></x:sheetData></x:worksheet>";

		// Token: 0x04000130 RID: 304
		internal static readonly string DefaultRels = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">\r\n    <Relationship Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\" Id=\"Rfc2254092b6248a9\" />\r\n</Relationships>";

		// Token: 0x04000131 RID: 305
		internal static readonly string DefaultWorkbookXmlRels = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">\r\n    {{sheets}}\r\n    <Relationship Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"/xl/styles.xml\" Id=\"R3db9602ace774fdb\" />\r\n    <Relationship Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings\" Target=\"/xl/sharedStrings.xml\" Id=\"R3db9602ace778fdb\" />\r\n</Relationships>";

		// Token: 0x04000132 RID: 306
		internal static readonly string DefaultWorkbookXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<x:workbook xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\"\r\n    xmlns:x=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">\r\n    <x:sheets>\r\n        {{sheets}}\r\n    </x:sheets>\r\n</x:workbook>";

		// Token: 0x04000133 RID: 307
		internal static readonly string DefaultSheetRelXml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">\r\n    {{format}}\r\n</Relationships>";

		// Token: 0x04000134 RID: 308
		internal static readonly string DefaultDrawing = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>\r\n<xdr:wsDr xmlns:a=\"http://schemas.openxmlformats.org/drawingml/2006/main\"\r\n    xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\"\r\n    xmlns:xdr=\"http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing\">\r\n    {{format}}\r\n</xdr:wsDr>";

		// Token: 0x04000135 RID: 309
		internal static readonly string DefaultDrawingXmlRels = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>\r\n<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">\r\n    {{format}}\r\n</Relationships>";

		// Token: 0x04000136 RID: 310
		internal static readonly string DefaultSharedString = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?><sst xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" count=\"0\" uniqueCount=\"0\"></sst>";

		// Token: 0x04000137 RID: 311
		internal static readonly string StartTypes = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\"><Default ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.printerSettings\" Extension=\"bin\"/><Default ContentType=\"application/xml\" Extension=\"xml\"/><Default ContentType=\"image/jpeg\" Extension=\"jpg\"/><Default ContentType=\"image/png\" Extension=\"png\"/><Default ContentType=\"image/gif\" Extension=\"gif\"/><Default ContentType=\"application/vnd.openxmlformats-package.relationships+xml\" Extension=\"rels\"/>";

		// Token: 0x04000138 RID: 312
		internal static readonly string EndTypes = "</Types>";
	}
}
