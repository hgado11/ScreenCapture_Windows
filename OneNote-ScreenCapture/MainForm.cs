using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Drawing.Imaging;
using System.IO;
using System.Security.AccessControl;
using System.Drawing.Printing;

namespace OneNote_ScreenCapture
{
    public partial class MainForm : Form
    {
        #region Pinvokes

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);

        #endregion

        #region Fields
        private const string APP_NAME = "Screen Capture Utility";
        private const int hotKeyId = 0xAFAF;
        private static Bitmap bitmapCache = null;
        private AdornerWindow adornerWindow = null;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem startWhenWindowsStartsToolStripMenuItem;
        private System.ComponentModel.IContainer components = null;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem autoSaveImagesToolStripMenuItem;
        private ToolStripMenuItem enablePreviewToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private const string RegKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string RegOptionsKey = "SOFTWARE\\B Inc\\Application Options";
        private bool previewEnabled = false;
        private bool autoSaveImages = false;
        private const string WelcomeToolTip = "Screen Capture utility, Press Windows + S to capture the screen.";
        private const string CopiedToClipBoardToolTip = "Cropped Image being Copied to Clipboard, you can paste it now!";
        #endregion

        #region Properties
        public bool AutoSaveImages
        {
            get { return autoSaveImages; }
            set { autoSaveImages = value; }
        }

        public bool PreivewEnabled
        {
            get { return previewEnabled; }
            set { previewEnabled = value; }
        }
        #endregion

        #region Ctor

        public MainForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;

            this.notifyIcon1.BalloonTipText = WelcomeToolTip;
        }

        #endregion

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startWhenWindowsStartsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.enablePreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoSaveImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipTitle = "Capture Screen";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Capture Screen";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enablePreviewToolStripMenuItem,
            this.autoSaveImagesToolStripMenuItem,
            this.toolStripSeparator2,
            this.startWhenWindowsStartsToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.contextMenuStrip1.Size = new System.Drawing.Size(257, 134);
            // 
            // startWhenWindowsStartsToolStripMenuItem
            // 
            this.startWhenWindowsStartsToolStripMenuItem.Name = "startWhenWindowsStartsToolStripMenuItem";
            this.startWhenWindowsStartsToolStripMenuItem.Size = new System.Drawing.Size(256, 24);
            this.startWhenWindowsStartsToolStripMenuItem.Text = "Start  when Windows starts";
            this.startWhenWindowsStartsToolStripMenuItem.Click += new System.EventHandler(this.startWhenWindowsStartsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(253, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(256, 24);
            this.toolStripMenuItem1.Text = "Exit";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.Exit_ItemClick);
            // 
            // enablePreviewToolStripMenuItem
            // 
            this.enablePreviewToolStripMenuItem.Name = "enablePreviewToolStripMenuItem";
            this.enablePreviewToolStripMenuItem.Size = new System.Drawing.Size(256, 24);
            this.enablePreviewToolStripMenuItem.Text = "Enable Preview";
            this.enablePreviewToolStripMenuItem.CheckedChanged += new System.EventHandler(this.enablePreviewToolStripMenuItem_CheckedChanged);
            this.enablePreviewToolStripMenuItem.Click += new System.EventHandler(this.enablePreviewToolStripMenuItem_Click);
            // 
            // autoSaveImagesToolStripMenuItem
            // 
            this.autoSaveImagesToolStripMenuItem.Name = "autoSaveImagesToolStripMenuItem";
            this.autoSaveImagesToolStripMenuItem.Size = new System.Drawing.Size(256, 24);
            this.autoSaveImagesToolStripMenuItem.Text = "Auto Save Images";
            this.autoSaveImagesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.autoSaveImagesToolStripMenuItem_CheckedChanged);
            this.autoSaveImagesToolStripMenuItem.Click += new System.EventHandler(this.autoSaveImagesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(253, 6);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 456);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Overrides

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                cp.Width = 0;
                cp.Height = 0;
                cp.X = -10000;
                cp.Y = -10000;

                return cp;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (adornerWindow == null)
            {
                adornerWindow = new AdornerWindow();

                adornerWindow.BitmapCropped += new EventHandler(adornerWindow_BitmapCropped);
            }

            try
            {
                RegisterHotKey(this.Handle, hotKeyId,/*MOD_WIN*/0x0008, Keys.A);

                InitializeContenxtMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            PersistApplicationOptions();

            if (adornerWindow != null)
            {
                adornerWindow.BitmapCropped -= new EventHandler(adornerWindow_BitmapCropped);

                adornerWindow.Dispose();

                adornerWindow = null;
            }

            try
            {
                UnregisterHotKey(this.Handle, hotKeyId);
            }
            catch
            {
 
            }

            base.OnHandleDestroyed(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312/*WM_HOTKEY*/)
            {
                OnWmHotKey(ref m);
            }
            
            base.WndProc(ref m);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                if(this.notifyIcon1!=null)
                    this.notifyIcon1.Visible = false;

                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.notifyIcon1.Visible = true;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.Hide();

            this.notifyIcon1.ShowBalloonTip(1000);
        }

        #endregion

        #region Implementations

        private void OnWmHotKey(ref Message m)
        {
            adornerWindow.BackgroundImage = GenerateScreenBitmap();
            adornerWindow.BackgroundImageLayout = ImageLayout.Stretch;

            adornerWindow.Show();

            adornerWindow.TopMost = true;
            adornerWindow.TopMost = false;
        }
        
        private void InitializeContenxtMenu()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey);

                this.startWhenWindowsStartsToolStripMenuItem.Checked = (key.GetValue(APP_NAME) != null);

                key = Registry.LocalMachine.OpenSubKey(RegOptionsKey,true);

                Options? options = key.GetValue(RegOptionsKey) as Options?;

                if (options != null && options.HasValue)
                {
                    this.autoSaveImagesToolStripMenuItem.Checked = options.Value.AutoSaveEnabled;
                    this.enablePreviewToolStripMenuItem.Checked = options.Value.PreviewEnabled;
                }
            }
            catch { }
        }

        private void CopyResultToClipBoard(Rectangle rect)
        {
            if (rect.Width < 0)
            {
                rect.X += rect.Width;
                rect.Width *= -1;
            }
            if (rect.Height < 0)
            {
                rect.Y += rect.Height;
                rect.Height *= -1;
            }

            Bitmap result = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(result);

            g.DrawImage(bitmapCache,new Rectangle(Point.Empty,result.Size), rect, GraphicsUnit.Pixel);

            bitmapCache = result;

            Clipboard.SetImage(result);
        }

        public static Bitmap GenerateScreenBitmap()
        {
            Rectangle scrBounds = new Rectangle(SystemInformation.VirtualScreen.Location, SystemInformation.VirtualScreen.Size);
            Bitmap bmp = new Bitmap(scrBounds.Width, scrBounds.Height);
            Graphics g = Graphics.FromImage(bmp);

            g.CopyFromScreen(Point.Empty, Point.Empty, scrBounds.Size, CopyPixelOperation.SourceCopy);

            bitmapCache = bmp;

            return bmp;
        }

        private void PersistApplicationOptions()
        {
            try
            {
                Options options = new Options(this.PreivewEnabled, this.AutoSaveImages);

                RegistryKey key = Registry.LocalMachine.CreateSubKey(RegOptionsKey, RegistryKeyPermissionCheck.ReadWriteSubTree);

                key.SetValue(RegOptionsKey, options);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Event Handlers

        void adornerWindow_BitmapCropped(object sender, EventArgs e)
        {
            if (adornerWindow.DragStop != adornerWindow.DragStart)
            {
                Rectangle rect = Rectangle.FromLTRB(adornerWindow.DragStart.X, adornerWindow.DragStart.Y, adornerWindow.DragStop.X, adornerWindow.DragStop.Y);

                CopyResultToClipBoard(rect);
                
                if (this.AutoSaveImages)
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    string fileName = "Snap_" + DateTime.Now.Ticks.ToString() + Guid.NewGuid().ToString().Substring(0, 2) + ".jpg";
                    bitmapCache.Save(Path.Combine(path, fileName), ImageFormat.Jpeg);
                }

               // if (this.PreivewEnabled)
                {
                    ActivePrintPreviewDialog preview = new ActivePrintPreviewDialog(bitmapCache);
                    preview.Tag = this;
                    preview.Show();
                }
                /* PrintBitmap(bitmapCache);
                 this.notifyIcon1.BalloonTipText = CopiedToClipBoardToolTip;
                 this.notifyIcon1.ShowBalloonTip(200);
                 
               
                 PrintDocument pd = new PrintDocument();
                 pd.PrintPage += new PrintPageEventHandler(PrintImage);
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
                 } */

            }
        }
       
        void PrintImage(object o, PrintPageEventArgs e)
        {
            int x = SystemInformation.WorkingArea.X;
            int y = SystemInformation.WorkingArea.Y;
            int width = this.Width;
            int height = this.Height;

            Rectangle bounds = new Rectangle(x, y, width, height);

            
            Point p = new Point(0, 0);
            e.Graphics.DrawImage(bitmapCache,p );
        }

        void PrintBitmap(Bitmap bm)
        {
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += (s, ev) =>
            {
                ev.Graphics.DrawImage(bm, Point.Empty); // adjust this to put the image elsewhere
                ev.HasMorePages = false;
            };
            doc.Print();
        }
        private void Exit_ItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void startWhenWindowsStartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey startUpKey = Registry.CurrentUser.OpenSubKey(RegKey, true);
                ToolStripMenuItem menuItem = (sender as ToolStripMenuItem);

                menuItem.Checked = !menuItem.Checked;

                if (menuItem.Checked)
                    startUpKey.SetValue(APP_NAME, Application.ExecutablePath);
                else
                    startUpKey.DeleteValue(APP_NAME, false);
            }
            catch { }
        }

        private void enablePreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.enablePreviewToolStripMenuItem.Checked = !this.enablePreviewToolStripMenuItem.Checked;
        }

        private void autoSaveImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.autoSaveImagesToolStripMenuItem.Checked = !this.autoSaveImagesToolStripMenuItem.Checked;
        }

        private void autoSaveImagesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            this.AutoSaveImages = (sender as ToolStripMenuItem).Checked;
        }

        private void enablePreviewToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            this.PreivewEnabled = (sender as ToolStripMenuItem).Checked;
        }

        #endregion
    }

    #region Helper Classes

    public struct Options
    {
        public Options(bool previewEnabled, bool autoSaveEnabled)
        {
            this.AutoSaveEnabled = autoSaveEnabled;
            this.PreviewEnabled = previewEnabled;
        }

        public bool AutoSaveEnabled;

        public bool PreviewEnabled;
    }

    #endregion
}
