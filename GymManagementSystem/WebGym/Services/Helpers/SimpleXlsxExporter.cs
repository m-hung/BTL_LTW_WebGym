using System.Globalization;
using System.IO.Compression;
using System.Security;
using System.Text;
using WebGym.Models.DTOs.Admin;

namespace WebGym.Services.Helpers
{
    public static class SimpleXlsxExporter
    {
        public static byte[] ExportRevenueReport(RevenueReportDto report)
        {
            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                AddZipEntry(archive, "[Content_Types].xml", BuildContentTypesXml());
                AddZipEntry(archive, "_rels/.rels", BuildRootRelsXml());
                AddZipEntry(archive, "xl/workbook.xml", BuildWorkbookXml());
                AddZipEntry(archive, "xl/_rels/workbook.xml.rels", BuildWorkbookRelsXml());
                AddZipEntry(archive, "xl/styles.xml", BuildStylesXml());
                AddZipEntry(archive, "xl/worksheets/sheet1.xml", BuildWorksheetXml(report));
            }

            return memoryStream.ToArray();
        }

        private static void AddZipEntry(ZipArchive archive, string entryPath, string content)
        {
            var entry = archive.CreateEntry(entryPath, CompressionLevel.Optimal);
            using var writer = new StreamWriter(entry.Open(), new UTF8Encoding(false));
            writer.Write(content);
        }

        private static string EscapeXml(string? input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var clean = new StringBuilder(input.Length);
            foreach (char ch in input)
            {
                if (ch == '\t' || ch == '\n' || ch == '\r' || ch >= 0x20)
                {
                    clean.Append(ch);
                }
            }
            return SecurityElement.Escape(clean.ToString()) ?? string.Empty;
        }

        private static string GetColLetter(int colIndex)
        {
            var dividend = colIndex;
            var colName = string.Empty;
            while (dividend > 0)
            {
                var modulo = (dividend - 1) % 26;
                colName = Convert.ToChar(65 + modulo) + colName;
                dividend = (dividend - modulo) / 26;
            }
            return colName;
        }

        private static string StrCell(int row, int col, string? value, int style = 3)
        {
            var cellRef = $"{GetColLetter(col)}{row}";
            return $"<c r=\"{cellRef}\" t=\"inlineStr\" s=\"{style}\"><is><t>{EscapeXml(value)}</t></is></c>";
        }

        private static string NumCell(int row, int col, decimal value, int style = 4)
        {
            var cellRef = $"{GetColLetter(col)}{row}";
            return $"<c r=\"{cellRef}\" t=\"n\" s=\"{style}\"><v>{value.ToString(CultureInfo.InvariantCulture)}</v></c>";
        }

        private static string BuildWorksheetXml(RevenueReportDto report)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sb.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");

            // Column widths
            sb.Append("<cols>");
            sb.Append("<col min=\"1\" max=\"1\" width=\"8\" customWidth=\"1\"/>");   // STT
            sb.Append("<col min=\"2\" max=\"2\" width=\"18\" customWidth=\"1\"/>");  // Thời gian
            sb.Append("<col min=\"3\" max=\"3\" width=\"15\" customWidth=\"1\"/>");  // Phân hệ
            sb.Append("<col min=\"4\" max=\"4\" width=\"20\" customWidth=\"1\"/>");  // Mã chứng từ
            sb.Append("<col min=\"5\" max=\"5\" width=\"26\" customWidth=\"1\"/>");  // Đối tượng
            sb.Append("<col min=\"6\" max=\"6\" width=\"38\" customWidth=\"1\"/>");  // Diễn giải
            sb.Append("<col min=\"7\" max=\"7\" width=\"16\" customWidth=\"1\"/>");  // Phương thức
            sb.Append("<col min=\"8\" max=\"8\" width=\"12\" customWidth=\"1\"/>");  // Loại Thu/Chi
            sb.Append("<col min=\"9\" max=\"9\" width=\"18\" customWidth=\"1\"/>");  // Số tiền
            sb.Append("<col min=\"10\" max=\"10\" width=\"16\" customWidth=\"1\"/>"); // Trạng thái
            sb.Append("</cols>");

            sb.Append("<sheetData>");

            int r = 1;

            // Row 1: Title (merged A1:J1)
            string periodLabel = report.Month.HasValue && report.Month > 0
                ? $"Tháng {report.Month}/{report.Year}"
                : $"Cả năm {report.Year}";
            sb.Append($"<row r=\"{r}\" ht=\"24\" customHeight=\"1\">{StrCell(r, 1, $"BÁO CÁO DOANH THU HỢP NHẤT WEBGYM ({periodLabel.ToUpper()})", 1)}</row>");
            r++;

            // Row 2: Export Metadata (merged A2:J2)
            sb.Append($"<row r=\"{r}\">{StrCell(r, 1, $"Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm}", 0)}</row>");
            r++;
            r++; // Blank row 3

            // Summary Financial Block (Rows 4..8, merged A..G for labels, H for Count, I..J for Value)
            sb.Append($"<row r=\"{r}\" ht=\"22\" customHeight=\"1\">");
            sb.Append(StrCell(r, 1, "TỔNG HỢP TÀI CHÍNH KỲ BÁO CÁO", 2));
            for (int c = 2; c <= 7; c++) sb.Append(StrCell(r, c, "", 2));
            sb.Append(StrCell(r, 8, "SỐ LƯỢNG", 2));
            sb.Append(StrCell(r, 9, "GIÁ TRỊ (VNĐ)", 2));
            sb.Append(StrCell(r, 10, "", 2));
            sb.Append("</row>");
            r++;

            void AppendSummaryRow(string label, int count, decimal val, int labelStyle, int valStyle)
            {
                sb.Append($"<row r=\"{r}\" ht=\"20\" customHeight=\"1\">");
                sb.Append(StrCell(r, 1, label, labelStyle));
                for (int c = 2; c <= 7; c++) sb.Append(StrCell(r, c, "", labelStyle));
                sb.Append(NumCell(r, 8, count, labelStyle));
                sb.Append(NumCell(r, 9, val, valStyle));
                sb.Append(StrCell(r, 10, "", labelStyle));
                sb.Append("</row>");
                r++;
            }

            AppendSummaryRow("1. Tổng Thu Bán Gói Tập (Payments)", report.PackageTransactionsCount, report.PackageRevenue, 3, 7);
            AppendSummaryRow("2. Tổng Thu Bán Hàng Shop & POS (Orders)", report.ShopOrdersCount, report.ShopRevenue, 3, 7);
            AppendSummaryRow("3. Tổng Chi Phí Sửa Chữa / Bảo Trì Máy (MaintenanceLogs)", report.MaintenanceLogsCount, -report.MaintenanceCost, 3, 8);
            AppendSummaryRow("TỔNG THU RÒNG HỢP NHẤT (Gói tập + Shop - Bảo trì)", report.ConsolidatedItems.Count, report.NetRevenue, 5, 6);
            r++; // Blank row 9

            // Detail Table Header (Row 10)
            sb.Append($"<row r=\"{r}\" ht=\"22\" customHeight=\"1\">");
            sb.Append(StrCell(r, 1, "STT", 2));
            sb.Append(StrCell(r, 2, "Thời Gian", 2));
            sb.Append(StrCell(r, 3, "Nguồn Giao Dịch", 2));
            sb.Append(StrCell(r, 4, "Mã Chứng Từ", 2));
            sb.Append(StrCell(r, 5, "Khách Hàng / Thiết Bị", 2));
            sb.Append(StrCell(r, 6, "Nội Dung / Diễn Giải", 2));
            sb.Append(StrCell(r, 7, "Phương Thức", 2));
            sb.Append(StrCell(r, 8, "Loại", 2));
            sb.Append(StrCell(r, 9, "Số Tiền (VNĐ)", 2));
            sb.Append(StrCell(r, 10, "Trạng Thái", 2));
            sb.Append("</row>");
            r++;

            int stt = 1;
            foreach (var item in report.ConsolidatedItems)
            {
                decimal signedAmount = item.IsExpense ? -item.Amount : item.Amount;
                int amountStyle = item.IsExpense ? 8 : 7;

                sb.Append($"<row r=\"{r}\">");
                sb.Append(NumCell(r, 1, stt++, 3));
                sb.Append(StrCell(r, 2, item.Date.ToString("dd/MM/yyyy HH:mm"), 3));
                sb.Append(StrCell(r, 3, item.SourceType, 3));
                sb.Append(StrCell(r, 4, item.ReferenceCode, 3));
                sb.Append(StrCell(r, 5, item.CustomerOrTarget, 3));
                sb.Append(StrCell(r, 6, item.Description, 3));
                sb.Append(StrCell(r, 7, item.PaymentMethod, 3));
                sb.Append(StrCell(r, 8, item.IsExpense ? "Chi phí" : "Khoản thu", 3));
                sb.Append(NumCell(r, 9, signedAmount, amountStyle));
                sb.Append(StrCell(r, 10, item.Status, 3));
                sb.Append("</row>");
                r++;
            }

            // Final Footer Row
            sb.Append($"<row r=\"{r}\" ht=\"22\" customHeight=\"1\">");
            sb.Append(StrCell(r, 1, "TỔNG CỘNG DOANH THU RÒNG HỢP NHẤT", 5));
            for (int c = 2; c <= 7; c++) sb.Append(StrCell(r, c, "", 5));
            sb.Append(StrCell(r, 8, "Ròng", 5));
            sb.Append(NumCell(r, 9, report.NetRevenue, 6));
            sb.Append(StrCell(r, 10, "", 5));
            sb.Append("</row>");
            int lastRow = r;

            sb.Append("</sheetData>");
            sb.Append("<mergeCells count=\"13\">");
            sb.Append("<mergeCell ref=\"A1:J1\"/>");
            sb.Append("<mergeCell ref=\"A2:J2\"/>");
            for (int sr = 4; sr <= 8; sr++)
            {
                sb.Append($"<mergeCell ref=\"A{sr}:G{sr}\"/>");
                sb.Append($"<mergeCell ref=\"I{sr}:J{sr}\"/>");
            }
            sb.Append($"<mergeCell ref=\"A{lastRow}:G{lastRow}\"/>");
            sb.Append("</mergeCells>");
            sb.Append("</worksheet>");
            return sb.ToString();
        }

        private static string BuildContentTypesXml() =>
@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Types xmlns=""http://schemas.openxmlformats.org/package/2006/content-types"">
  <Default Extension=""rels"" ContentType=""application/vnd.openxmlformats-package.relationships+xml""/>
  <Default Extension=""xml"" ContentType=""application/xml""/>
  <Override PartName=""/xl/workbook.xml"" ContentType=""application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml""/>
  <Override PartName=""/xl/worksheets/sheet1.xml"" ContentType=""application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml""/>
  <Override PartName=""/xl/styles.xml"" ContentType=""application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml""/>
</Types>";

        private static string BuildRootRelsXml() =>
@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships"">
  <Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"" Target=""xl/workbook.xml""/>
</Relationships>";

        private static string BuildWorkbookXml() =>
@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<workbook xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships"">
  <sheets>
    <sheet name=""BaoCaoDoanhThu"" sheetId=""1"" r:id=""rId1""/>
  </sheets>
</workbook>";

        private static string BuildWorkbookRelsXml() =>
@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships"">
  <Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet"" Target=""worksheets/sheet1.xml""/>
  <Relationship Id=""rId2"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles"" Target=""styles.xml""/>
</Relationships>";

        private static string BuildStylesXml() =>
@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<styleSheet xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"">
  <numFmts count=""1"">
    <numFmt numFmtId=""164"" formatCode=""#,##0""/>
  </numFmts>
  <fonts count=""6"">
    <font><sz val=""11""/><name val=""Calibri""/></font>
    <font><b/><sz val=""14""/><color rgb=""FF0F172A""/><name val=""Calibri""/></font>
    <font><b/><sz val=""11""/><color rgb=""FFFFFFFF""/><name val=""Calibri""/></font>
    <font><b/><sz val=""11""/><color rgb=""FF0F172A""/><name val=""Calibri""/></font>
    <font><b/><sz val=""11""/><color rgb=""FF15803D""/><name val=""Calibri""/></font>
    <font><b/><sz val=""11""/><color rgb=""FFB91C1C""/><name val=""Calibri""/></font>
  </fonts>
  <fills count=""4"">
    <fill><patternFill patternType=""none""/></fill>
    <fill><patternFill patternType=""gray125""/></fill>
    <fill><patternFill patternType=""solid""><fgColor rgb=""FF1E3A8A""/></patternFill></fill>
    <fill><patternFill patternType=""solid""><fgColor rgb=""FFF1F5F9""/></patternFill></fill>
  </fills>
  <borders count=""2"">
    <border><left/><right/><top/><bottom/><diagonal/></border>
    <border>
      <left style=""thin""><color rgb=""FFCBD5E1""/></left>
      <right style=""thin""><color rgb=""FFCBD5E1""/></right>
      <top style=""thin""><color rgb=""FFCBD5E1""/></top>
      <bottom style=""thin""><color rgb=""FFCBD5E1""/></bottom>
      <diagonal/>
    </border>
  </borders>
  <cellStyleXfs count=""1"">
    <xf numFmtId=""0"" fontId=""0"" fillId=""0"" borderId=""0""/>
  </cellStyleXfs>
  <cellXfs count=""9"">
    <xf numFmtId=""0"" fontId=""0"" fillId=""0"" borderId=""0"" xfId=""0""/>
    <xf numFmtId=""0"" fontId=""1"" fillId=""0"" borderId=""0"" xfId=""0"" applyFont=""1""/>
    <xf numFmtId=""0"" fontId=""2"" fillId=""2"" borderId=""1"" xfId=""0"" applyFont=""1"" applyFill=""1"" applyBorder=""1"" applyAlignment=""1""><alignment horizontal=""center"" vertical=""center""/></xf>
    <xf numFmtId=""0"" fontId=""0"" fillId=""0"" borderId=""1"" xfId=""0"" applyBorder=""1"" applyAlignment=""1""><alignment vertical=""center""/></xf>
    <xf numFmtId=""164"" fontId=""0"" fillId=""0"" borderId=""1"" xfId=""0"" applyNumberFormat=""1"" applyBorder=""1"" applyAlignment=""1""><alignment horizontal=""right"" vertical=""center""/></xf>
    <xf numFmtId=""0"" fontId=""3"" fillId=""3"" borderId=""1"" xfId=""0"" applyFont=""1"" applyFill=""1"" applyBorder=""1"" applyAlignment=""1""><alignment vertical=""center""/></xf>
    <xf numFmtId=""164"" fontId=""3"" fillId=""3"" borderId=""1"" xfId=""0"" applyNumberFormat=""1"" applyFont=""1"" applyFill=""1"" applyBorder=""1"" applyAlignment=""1""><alignment horizontal=""right"" vertical=""center""/></xf>
    <xf numFmtId=""164"" fontId=""4"" fillId=""0"" borderId=""1"" xfId=""0"" applyNumberFormat=""1"" applyFont=""1"" applyBorder=""1"" applyAlignment=""1""><alignment horizontal=""right"" vertical=""center""/></xf>
    <xf numFmtId=""164"" fontId=""5"" fillId=""0"" borderId=""1"" xfId=""0"" applyNumberFormat=""1"" applyFont=""1"" applyBorder=""1"" applyAlignment=""1""><alignment horizontal=""right"" vertical=""center""/></xf>
  </cellXfs>
</styleSheet>";
    }
}
