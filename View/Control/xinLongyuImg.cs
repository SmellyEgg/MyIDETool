using System;
using System.Drawing;
using System.Windows.Forms;

namespace xinLongIDE.View.Control
{
    public class xinLongyuImg : PictureBox
    {
        private const int grab = 5;
        public xinLongyuImg()
        {
            this.ResizeRedraw = true;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var rc = new Rectangle(this.ClientSize.Width + grab, this.ClientSize.Height + grab, grab, grab);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                //this.SizeMode = PictureBoxSizeMode.Normal;
                var pos = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                if (pos.X >= this.ClientSize.Width - grab && pos.Y >= this.ClientSize.Height - grab)
                    m.Result = new IntPtr(17);  // HT_BOTTOMRIGHT
                //this.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }
}
