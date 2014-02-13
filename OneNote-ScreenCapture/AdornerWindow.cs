using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace OneNote_ScreenCapture
{
    internal partial class AdornerWindow : Form
    {
        #region Fields

        Point dragStart = Point.Empty;
        Point dragStop = Point.Empty;
        bool mousePressed = false;
        private System.ComponentModel.IContainer components = null;
        public event EventHandler BitmapCropped;

        #endregion

        #region properties
        public Point DragStart
        {
            get { return dragStart; }
        }

        public Point DragStop
        {
            get { return dragStop; }
        }
        #endregion

        #region Ctor
        public AdornerWindow()
        {
            InitializeComponent();
            
            this.Cursor = Cursors.Cross;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.AutoScaleMode = AutoScaleMode.None;

            this.Size = SystemInformation.VirtualScreen.Size;
            this.Location = SystemInformation.VirtualScreen.Location;

            this.TopMost = true;
        }
        #endregion

        #region Overrides
        
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            ResetPoints();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            HideAdorner();
        }

        private void HideAdorner()
        {
            this.Hide();

            if (this.BitmapCropped != null)
                BitmapCropped(this, EventArgs.Empty);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            mousePressed = e.Button == MouseButtons.Left;

            dragStop = Control.MousePosition;

            this.Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            this.dragStart = Control.MousePosition;

            this.Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            this.dragStop = Control.MousePosition;
            
            HideAdorner();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Region clip = e.Graphics.Clip;

            if (mousePressed && dragStart!= Point.Empty && dragStart != dragStop)
            {
                Rectangle rect = Rectangle.FromLTRB(dragStart.X, dragStart.Y, dragStop.X, dragStop.Y);

                using (Pen pen = new Pen(Color.Black))
                {
                    e.Graphics.DrawRectangle(pen, Rectangle.Inflate(rect,-1,-1));
                }

                e.Graphics.SetClip(rect, CombineMode.Exclude);
            }

            using (Brush brush = new SolidBrush(Color.FromArgb(210,Color.WhiteSmoke)))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }

            e.Graphics.SetClip(clip, CombineMode.Replace);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0200 /*WM_MOUSEMOVE*/)
            {
                this.Refresh();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Implementation

        public void ResetPoints()
        {
            this.dragStart = Point.Empty;
            this.dragStop = Point.Empty;
            this.mousePressed = false;
        }

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AdornerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdornerWindow";
            this.Text = "AdornerWindow";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion
    }
}
