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
                PdfPTable pdfTableBlank = new PdfPTable(1);
                //Footer section
                PdfPTable pdfTableFooter = new PdfPTable(1);
                pdfTableFooter.DefaultCell.BorderWidth = 0;
                pdfTableFooter.WidthPercentage = 100;
                pdfTableFooter.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                Chunk cnkFooter = new Chunk("STUDENT REPORT", FontFactory.GetFont("Times New Roman"));
                cnkFooter.Font.Size = 10;
                pdfTableFooter.AddCell(new Phrase(cnkFooter));
                //End of footer section

                pdfTableBlank.AddCell(new Phrase(" "));
                pdfTableBlank.DefaultCell.Border = 0;

                PdfPTable pdfTable1 = new PdfPTable(1);
                PdfPTable pdfTable2 = new PdfPTable(1);
                PdfPTable pdfTable3 = new PdfPTable(2);

                pdfTable1.WidthPercentage = 80;
                pdfTable1.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable1.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                pdfTable1.DefaultCell.BorderWidth = 0;

                pdfTable2.WidthPercentage = 80;
                pdfTable2.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable2.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                pdfTable2.DefaultCell.BorderWidth = 0;

                pdfTable3.DefaultCell.Padding = 5;
                pdfTable3.WidthPercentage = 80;
                pdfTable3.DefaultCell.BorderWidth = 0.5f;


                Chunk c1 = new Chunk("STUDENT REPORT", FontFactory.GetFont("Times New Roman"));
                c1.Font.Color = new iTextSharp.text.BaseColor(0, 0, 0);
                c1.Font.SetStyle(0); //- 0 for normal font
                c1.Font.Size = 14;
                Phrase p1 = new Phrase();
                p1.Add(c1);
                pdfTable1.AddCell(p1);

                var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

                Chunk courses = new Chunk("Courses", boldFont);
                Chunk grades = new Chunk("Grades", boldFont);

                pdfTable3.AddCell(new Phrase(courses));
                pdfTable3.AddCell(new Phrase(grades));
                pdfTable3.AddCell(new Phrase("Veganstvo"));
                pdfTable3.AddCell(new Phrase("2.67"));

                
                string folderPath = ".\\PDF\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                int fileCount = Directory.GetFiles(@".\\PDF").Length;
                string strFileName = "StudentReport" + (fileCount + 1) + ".pdf";

                using (FileStream stream = new FileStream(folderPath + strFileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(pdfTable1);
                    pdfDoc.Add(pdfTable2);
                    pdfDoc.Add(pdfTableBlank);
                    pdfDoc.Add(pdfTable3);
                    pdfDoc.NewPage();

                    pdfDoc.Close();
                    stream.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
