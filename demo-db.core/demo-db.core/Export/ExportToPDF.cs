using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace demo_db.core.ExportToPDF
{
    public class ExportToPDF
    {
        public static void GeneratePDFReport()
        {
            try
            {
                PdfPTable footer = new PdfPTable(1);
                PdfPTable name = new PdfPTable(1);
                PdfPTable table = new PdfPTable(3);
                var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);

                Chunk c1 = new Chunk("STUDENT REPORT", boldFont);
                footer.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                footer.DefaultCell.Border = 0;
                footer.AddCell(new Phrase(c1));

                Chunk studentName = new Chunk("Pesho Goshev", boldFont);
                name.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                name.DefaultCell.Border = 0;
                name.AddCell(new Phrase(studentName));

                PdfPCell courses = new PdfPCell(new Phrase("Courses", boldFont));
                PdfPCell assignments = new PdfPCell(new Phrase("Assignments", boldFont));
                PdfPCell grades = new PdfPCell(new Phrase("Grades", boldFont));

                table.TotalWidth = 400f;
                table.LockedWidth = true;
                table.AddCell(courses);
                table.AddCell(assignments);
                table.AddCell(grades);
                table.AddCell("DSA");

                PdfPTable nestedTabe1 = new PdfPTable(1);
                PdfPCell nestedCell1 = new PdfPCell(nestedTabe1);
                nestedCell1.Padding = 0f;
                nestedTabe1.AddCell(new Phrase("Linear"));
                nestedTabe1.AddCell(new Phrase("Tree"));
                table.AddCell(nestedCell1);

                PdfPTable nestedTable2 = new PdfPTable(1);
                PdfPCell nestedCell2 = new PdfPCell(nestedTable2);
                nestedCell2.Padding = 0f;
                nestedTable2.AddCell(new Phrase("32"));
                nestedTable2.AddCell(new Phrase("2"));
                table.AddCell(nestedCell2);

                string folderPath = ".\\PDF\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                int fileCount = Directory.GetFiles(@".\\PDF").Length;
                string strFileName = "StudentReport" + (fileCount + 1) + ".pdf";

                using (FileStream stream = new FileStream(folderPath + strFileName, FileMode.Create))
                {
                    Rectangle pageSize = new Rectangle(PageSize.A4);
                    pageSize.BackgroundColor = new BaseColor(255, 180, 203);
                    Document pdfDoc = new Document(pageSize);

                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(footer);
                    pdfDoc.Add(name);
                    pdfDoc.Add(table);
                    pdfDoc.NewPage();

                    pdfDoc.Close();
                    stream.Close();
                }
            }
            catch (Exception)
            {
                throw new Exception("Cannot export to PDF");
            }
        }
    }

}
