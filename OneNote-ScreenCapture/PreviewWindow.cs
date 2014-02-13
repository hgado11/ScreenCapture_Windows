using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace OneNote_ScreenCapture
{
    public partial class PreviewWindow : Form
    {
        private Bitmap image;
       
        public delegate void HideWindow(Form window, bool visible);
        public PreviewWindow()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.Selectable, false);
          
           
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public PreviewWindow(Bitmap preivew):this()
        {
            if (preivew != null)
            {
                //this.Size = preivew.Size;
                this.image = preivew;
                this.BackgroundImage = preivew;
                this.BackgroundImageLayout = ImageLayout.Center;
            }
        }

        const int WS_EX_NOACTIVATE = 0x08000000;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                cp.ExStyle |= WS_EX_NOACTIVATE;

                return cp;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

           // this.preivewTimer.Enabled = true;
        }

      

        private void printToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintImage);
            pd.OriginAtMargins = true;

            PrintDialog pdi = new PrintDialog();
            pdi.Document = pd;
            pdi.UseEXDialog = true;
            if (pdi.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
            else
            {
                MessageBox.Show("Print Cancelled");
            }
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
