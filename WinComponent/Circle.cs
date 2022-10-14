using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace KSW.WirelessChannelEmulation.ViewBase.Equipment
{
    public class Circle : Shape
    {
        public string Tag { get; set; }
        public Point Central
        {
            get
            {
                return new Point(this.Location.X + this.Size.Width / 2, this.Location.Y + this.Size.Height / 2);
            }
        }
        public override void Draw(Graphics g)
        {
            this.graph = g;
            graph.SmoothingMode = SmoothingMode.HighQuality;
            Rectangle rectangle = new Rectangle(Location, Size);
            graph.FillEllipse(new SolidBrush(BackColor), rectangle);
            if (BorderThinkness > 0)
            {
                Pen borderPen = new Pen(BorderColor, BorderThinkness);
                graph.DrawEllipse(borderPen, rectangle);
            }
            if (Content != String.Empty)
            {
                StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                graph.DrawString(Content, Font, new SolidBrush(FontColor), new Rectangle(Location.X, Location.Y + 1, Size.Width, Size.Height), sf);
            }
        }
    }
}
