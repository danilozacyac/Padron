using System;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PadronApi.Reportes
{
    public class HeaderFooter : iTextSharp.text.pdf.PdfPageEventHelper
    {

        //override the OnStartPage event handler to add our header
        public override void OnStartPage(PdfWriter writer, Document doc)
        {
            //I use a PdfPtable with 2 columns to position my footer where I want it
            PdfPTable headerTbl = new PdfPTable(2);

            //set the width of the table to be the same as the document
            headerTbl.TotalWidth = doc.PageSize.Width;

            //Center the table on the page
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            //Create a paragraph that contains the footer text
            Paragraph para;// = new Paragraph("", Fuentes.footer);
            DateTime time = DateTime.Now;              // Use current time
            string format = "dd/MM/yyyy";    // Use this format
            para = new Paragraph(time.ToString(format));

            //add a carriage return
            para.Add(Environment.NewLine);
            para.Add("");

            var bmp = new System.Drawing.Bitmap(PadronApi.Properties.Resources.autho2);
            Image logo = Image.GetInstance(bmp,System.Drawing.Imaging.ImageFormat.Bmp);
                

            //create a cell instance to hold the text
            PdfPCell cell = new PdfPCell(logo);

            //set cell border to 0
            cell.Border = 0;

            //add some padding to bring away from the edge
            cell.PaddingLeft = 10;

            //add cell to table
            headerTbl.AddCell(cell);


            //Creo el contenido de la 2da celda
            int pageN = writer.PageNumber;
            //DateTime time = DateTime.Now;              // Use current time
            //string format = "dd/MM/yyyy";    // Use this format
            //para = new Paragraph(time.ToString(format), Fuentes.footer);

            //para.Add(Environment.NewLine);
            para = new Paragraph(pageN.ToString());

            //create new instance of cell to hold the text
            cell = new PdfPCell(para);

            //align the text to the right of the cell
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //set border to 0
            cell.Border = 0;

            // add some padding to take away from the edge of the page
            cell.PaddingRight = 10;

            //add the cell to the table
            headerTbl.AddCell(cell);

            //write the rows out to the PDF output stream.
            headerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 10), writer.DirectContent);
        }

        //override the OnPageEnd event handler to add our footer
        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            
        }


    }
}
