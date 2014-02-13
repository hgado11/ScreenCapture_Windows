using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace OneNote_ScreenCapture
{
    public partial class ActivePrintPreviewDialog : PrintPreviewDialog
    {
        private Bitmap image;
        public ActivePrintPreviewDialog()
        {
            InitializeComponent();
        }
        public ActivePrintPreviewDialog(Bitmap preivew): this()
        {
            if (preivew != null)
            {
                //this.Size = preivew.Size;
                this.image = preivew;
                preview();
            }
        }
        protected override void OnShown(EventArgs e)
        {
            //Activate();
           // base.OnShown(e);
        }
        protected  void preview()
        {
            ActivePrintPreviewDialog previewDlg = new ActivePrintPreviewDialog();
            //Printdoucment
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintImage);
            pd.OriginAtMargins = true;
            // Pass the Aspose.Words print document to the Print Preview dialog.
            previewDlg.Document = pd;
            // Specify additional parameters of the Print Preview dialog.
            previewDlg.ShowInTaskbar = true;
            previewDlg.MinimizeBox = true;
            previewDlg.PrintPreviewControl.Zoom = 1;
            previewDlg.Document.DocumentName = "TestName.doc";
            previewDlg.WindowState = FormWindowState.Maximized;
            // Show the appropriately configured Print Preview dialog.
            previewDlg.ShowDialog();
        }
        void PrintImage(object o, PrintPageEventArgs e)
        {
            float newWidth = image.Width * 100 / image.HorizontalResolution;
            float newHeight = image.Height * 100 / image.VerticalResolution;

            float widthFactor = newWidth / e.MarginBounds.Width;
            float heightFactor = newHeight / e.MarginBounds.Height;


            if (widthFactor > 1 | heightFactor > 1)
            {
                if (widthFactor > heightFactor)
                {
                    newWidth = newWidth / widthFactor;
                    newHeight = newHeight / widthFactor;
                }
                else
                {
                    newWidth = newWidth / heightFactor;
                    newHeight = newHeight / heightFactor;
                }
            }

            e.Graphics.DrawImage(image, 0, 0, (int)newWidth, (int)newHeight);

        }
    }
}
