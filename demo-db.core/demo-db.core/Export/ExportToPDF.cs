using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace demo_db.core.Export
{
    public class ExportToPDF
    {
        private readonly ICourseService serviceCourse;

        public ExportToPDF(ISessionState state, ICourseService service)
        {
            this.serviceCourse = service;
        }

        public static void GeneratePDFReport(IList<Services.ViewModels.GradeViewModel> grades, string username)
        {
            try
            {
                PdfPTable footer = new PdfPTable(1);
                PdfPTable name = new PdfPTable(1);
                PdfPTable table = new PdfPTable(2);
                var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 16);

                //Top of the page
                Chunk c1 = new Chunk("STUDENT REPORT", boldFont);
                footer.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                footer.DefaultCell.Border = 0;
                footer.AddCell(new Phrase(c1));
                
                Chunk studentName = new Chunk(username, boldFont);
                name.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                name.DefaultCell.Border = 0;
                name.AddCell(new Phrase(studentName));
                //Not changeable
                PdfPCell courses = new PdfPCell(new Phrase("Courses", boldFont));
                PdfPCell assignments = new PdfPCell(new Phrase("Assignments/Grades", boldFont));

                foreach (var item in grades)
                {

                }
                //Main table - Left column -> courses - right column -> nested tables
                table.TotalWidth = 400f;
                table.LockedWidth = true;
                table.AddCell(courses);
                table.AddCell(assignments);                
                table.AddCell("DSA");

                //Nested table with Assaignments and Grades
                PdfPTable nestedTable = new PdfPTable(2);
                nestedTable.DefaultCell.Border = 0;
                nestedTable.AddCell(new Phrase("Linear"));                
                nestedTable.AddCell(new Phrase("2"));
                table.AddCell(nestedTable);                
                nestedTable.DeleteBodyRows();
                table.AddCell("");
                nestedTable.AddCell(new Phrase("Tree"));
                nestedTable.AddCell(new Phrase("32"));
                table.AddCell(nestedTable);
                table.AddCell("");


                //Set up file name and directory
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
