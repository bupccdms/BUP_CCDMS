using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Word = Microsoft.Office.Interop.Word;

namespace qms.Utility
{
    public class ExportManager
    {

        public static void ExportToPdf(DataTable dt, string reportName, string user_id)
        {
            Document document = new Document();
            string fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/GeneratedReports/"), user_id + ".pdf");
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
            document.Open();

            Paragraph para = new Paragraph(reportName + "\n\n", new Font(Font.FontFamily.HELVETICA, 22));
            para.Alignment = Element.ALIGN_CENTER;

            document.Add(para);



            Font font5 = FontFactory.GetFont(FontFactory.HELVETICA, 5);
            Font font5Bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 5);

            PdfPTable table = new PdfPTable(dt.Columns.Count);
            //float[] widths = new float[] { 4f, 4f, 4f, 4f };

            //table.SetWidths(widths);

            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase(reportName));

            cell.Colspan = dt.Columns.Count;

            foreach (DataColumn c in dt.Columns)
            {

                table.AddCell(new Phrase(c.ColumnName.Replace("_", " "), font5Bold));
            }

            foreach (DataRow r in dt.Rows)
            {
                foreach (DataColumn c in dt.Columns)
                {
                    table.AddCell(new Phrase(r[c].ToString(), font5));
                    //table.AddCell(new Phrase(c.ColumnName, font5));
                }

            }
            document.Add(table);
            document.Close();

            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.AddHeader("Content-Disposition",
                               "attachment; filename=" + reportName + ".pdf;");
            HttpContext.Current.Response.TransmitFile(fileName);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public static void ExportToPdf<T>(List<T> list, string reportName, string user_id)
        {
            Document document = new Document();
            string fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/GeneratedReports/"), reportName + "_" + user_id + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".pdf");
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
            document.Open();

            Paragraph para = new Paragraph(reportName + "\n\n", new Font(Font.FontFamily.HELVETICA, 22));
            para.Alignment = Element.ALIGN_CENTER;

            document.Add(para);



            Font font5 = FontFactory.GetFont(FontFactory.HELVETICA, 5);
            Font font5Bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 5);
           

            PropertyInfo[] columns = list.FirstOrDefault().GetType().GetProperties();

            PdfPTable table = new PdfPTable(columns.Count());
            //float[] widths = new float[] { 4f, 4f, 4f, 4f };

            //table.SetWidths(widths);

            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase(reportName));

            cell.Colspan = columns.Count();

            foreach (PropertyInfo c in columns)
            {
                string name = GetDisplayName(c);
                if (name.Trim().Length > 0) table.AddCell(new Phrase(name, font5Bold));
            }

            foreach (T r in list)
            {
                foreach (PropertyInfo prop in r.GetType().GetProperties())
                {
                    string name = GetDisplayName(prop);
                    if (name.Trim().Length > 0)
                    {
                        var value = prop.GetValue(r, null);

                        table.AddCell(new Phrase((value == null ? "" : value.ToString()), font5));
                    }
                }
            }
            document.Add(table);
            document.Close();

            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.AddHeader("Content-Disposition",
                               "attachment; filename=" + reportName + ".pdf;");
            HttpContext.Current.Response.TransmitFile(fileName);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public static string GetDisplayName(PropertyInfo prop)
        {


            object[] attrs = prop.GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                DisplayAttribute Attr = attr as DisplayAttribute;
                if (Attr != null)
                {
                    return Attr.Name;
                }
            }


            return "";
        }

        public static void ExportToExcel(DataTable dt, string reportName)
        {
            string fileName = reportName + ".xls";
            string attachment = "attachment; filename=" + fileName;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                HttpContext.Current.Response.Write(tab + dc.ColumnName.Replace("_", " "));
                tab = "\t";
            }
            HttpContext.Current.Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
            HttpContext.Current.Response.End();
        }

        public static void ExportToExcel<T>(List<T> list, string reportName)
        {
            string fileName = reportName + ".xls";
            string attachment = "attachment; filename=" + fileName;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            string tab = "";

            PropertyInfo[] columns = list.FirstOrDefault().GetType().GetProperties();

            foreach (PropertyInfo c in columns)
            {
                string name = GetDisplayName(c);
                if (name.Trim().Length > 0)
                {
                    HttpContext.Current.Response.Write(tab + name);
                    tab = "\t";
                }
            }
           
            HttpContext.Current.Response.Write("\n");
            int i;
            foreach (T r in list)
            {
                tab = "";
                foreach (PropertyInfo prop in r.GetType().GetProperties())
                {
                    string name = GetDisplayName(prop);
                    if (name.Trim().Length > 0)
                    {
                        var value = prop.GetValue(r, null);
                        HttpContext.Current.Response.Write(tab + (value == null ? "" : value.ToString()));
                        tab = "\t";
                    }
                }
                HttpContext.Current.Response.Write("\n");
            }
            HttpContext.Current.Response.End();
        }

        public static void ExportToCSV(DataTable dt, string reportName)
        {
            string fileName = reportName + ".csv";
            string attachment = "attachment; filename=" + fileName;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                HttpContext.Current.Response.Write(tab + dc.ColumnName.Replace("_", " "));
                tab = ApplicationSetting.ReportCSVSeparator;
            }
            HttpContext.Current.Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write(tab + dr[i].ToString());
                    tab = ApplicationSetting.ReportCSVSeparator;
                }
                HttpContext.Current.Response.Write("\n");
            }
            HttpContext.Current.Response.End();
        }

        public static void ExportToCSV<T>(List<T> list, string reportName)
        {
            string fileName = reportName + ".csv";
            string attachment = "attachment; filename=" + fileName;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            string tab = "";

            if (list.Count == 0)
            {
                HttpContext.Current.Response.Write("No record found");
            }
            else
            {
                PropertyInfo[] columns = list.FirstOrDefault().GetType().GetProperties();

                foreach (PropertyInfo c in columns)
                {
                    string name = GetDisplayName(c);
                    if (name.Trim().Length > 0)
                    {
                        HttpContext.Current.Response.Write(tab + name);
                        tab = ApplicationSetting.ReportCSVSeparator;
                    }
                }

                HttpContext.Current.Response.Write("\n");
                foreach (T r in list)
                {
                    tab = "";
                    foreach (PropertyInfo prop in r.GetType().GetProperties())
                    {
                        string name = GetDisplayName(prop);
                        if (name.Trim().Length > 0)
                        {
                            var value = prop.GetValue(r, null);
                            HttpContext.Current.Response.Write(tab + (value == null ? "" : value.ToString()));
                            tab = ApplicationSetting.ReportCSVSeparator;
                        }
                    }
                    HttpContext.Current.Response.Write("\n");
                }
            }
            HttpContext.Current.Response.End();
        }

        public static void ExportToWord(DataTable dt, string reportName, string user_id)

        {
            Word.Application app = new Word.Application();
            Word.Document wd = app.Documents.Add();

            wd.Activate();

            //ADD TEXT

            //wd.Content.Text="<table></table>";

            //END ADD TEXT

            Object start = Type.Missing;

            Object end = Type.Missing;

            Word.Range rng = wd.Range(ref start, ref end);


            object oMissing = Missing.Value;
            object oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

            Word.Paragraph oPara2;
            object oRng = wd.Bookmarks.get_Item(ref oEndOfDoc).Range;
            oPara2 = wd.Content.Paragraphs.Add(ref oRng);
            oPara2.Range.Text = reportName;
            oPara2.Range.Font.Bold = 1;
            oPara2.Format.SpaceAfter = 24;
            oPara2.Range.InsertParagraphAfter();

            rng.Font.Size = 10;

            Object defaultTableBehavior = Type.Missing;

            Object autoFitBehavior = Type.Missing;

            object missing = Type.Missing;

            //ADD TABLE

            Word.Table tbl = wd.Tables.Add(rng, 1, dt.Columns.Count, ref missing, ref missing);
            

            //END ADD TABLE

            //SET HEADER


            for (int i = 0; i < dt.Columns.Count; i++)
            {
                SetHeadings(tbl.Cell(1, i + 1), dt.Columns[i].ColumnName.Replace("_", " "));
            }

            //END SET HEADER

            //ADD ROW

            for (int i = 0; i < dt.Rows.Count; i++)

            {
                Word.Row newRow = wd.Tables[1].Rows.Add(ref missing);

                newRow.Range.Font.Bold = 0;

                newRow.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    newRow.Cells[j + 1].Range.Text = dt.Rows[i][j].ToString();
                    Setborder(newRow.Cells[j + 1]);
                }

            }

            //END ROW

            object fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/GeneratedReports/"), user_id + ".doc");
            app.ActiveDocument.SaveAs2(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

            app.ActiveDocument.Close();

            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/msword";
            HttpContext.Current.Response.AddHeader("Content-Disposition",
                               "attachment; filename=" + reportName + ".doc;");
            HttpContext.Current.Response.TransmitFile(fileName.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public static void ExportToWord<T>(List<T> list, string reportName, string user_id)

        {
            Word.Application app = new Word.Application();
            Word.Document wd = app.Documents.Add();

            wd.Activate();

            //ADD TEXT

            //wd.Content.Text="<table></table>";

            //END ADD TEXT

            Object start = Type.Missing;

            Object end = Type.Missing;

            Word.Range rng = wd.Range(ref start, ref end);


            object oMissing = Missing.Value;
            object oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

            Word.Paragraph oPara2;
            object oRng = wd.Bookmarks.get_Item(ref oEndOfDoc).Range;
            oPara2 = wd.Content.Paragraphs.Add(ref oRng);
            oPara2.Range.Text = reportName;
            oPara2.Range.Font.Bold = 1;
            oPara2.Format.SpaceAfter = 24;
            oPara2.Range.InsertParagraphAfter();

            rng.Font.Size = 10;

            Object defaultTableBehavior = Type.Missing;

            Object autoFitBehavior = Type.Missing;

            object missing = Type.Missing;

            //ADD TABLE
            PropertyInfo[] columns = list.FirstOrDefault().GetType().GetProperties();

            Word.Table tbl = wd.Tables.Add(rng, 1, columns.Count(), ref missing, ref missing);


            //END ADD TABLE

            //SET HEADER


            for (int i = 0; i < columns.Count(); i++)
            {
                string name = GetDisplayName(columns[i]);
                if (name.Trim().Length > 0)
                {
                    SetHeadings(tbl.Cell(1, i + 1), name);
                }
            }

            //END SET HEADER

            //ADD ROW

            for (int i = 0; i < list.Count(); i++)
            {
                Word.Row newRow = wd.Tables[1].Rows.Add(ref missing);

                newRow.Range.Font.Bold = 0;

                newRow.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

                for (int j = 0; j < columns.Count(); j++)
                {
                    string name = GetDisplayName(columns[j]);
                    if (name.Trim().Length > 0)
                    {
                        var value = columns[j].GetValue(list[i], null);
                        newRow.Cells[j + 1].Range.Text = (value == null ? "" : value.ToString());
                        //Setborder(newRow.Cells[j + 1]);
                    }
                }

            }

            //END ROW

            object fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/GeneratedReports/"), user_id + ".doc");
            app.ActiveDocument.SaveAs2(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

            app.ActiveDocument.Close();

            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/msword";
            HttpContext.Current.Response.AddHeader("Content-Disposition",
                               "attachment; filename=" + reportName + ".doc;");
            HttpContext.Current.Response.TransmitFile(fileName.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        //ADD HEADER

        static void SetHeadings(Word.Cell tblCell, string text)

        {

            tblCell.Range.Text = text;

            tblCell.Range.Font.Bold = 1;

            tblCell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            Setborder(tblCell);

        }

        static void Setborder(Word.Cell tblCell)
        {
            tblCell.Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
            tblCell.Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
            tblCell.Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
            tblCell.Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
        }

    }
}