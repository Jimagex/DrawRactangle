using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace KSW.WirelessChannelEmulation.ViewBase.Equipment
{
    /// <summary>
    /// 设备
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Point Location { get; set; } = new Point(0, 0);
        /// <summary>
        /// 半径
        /// </summary>
        public int Radius { get; set; } = 36;
        /// <summary>
        /// 中心点
        /// </summary>
        public Point Central { get { return new Point(this.Location.X + Radius / 2, this.Location.Y + Radius / 2); } }
        /// <summary>
        /// 线条宽度
        /// </summary>
        public float PenWidth { get; set; } = 2f;
        /// <summary>
        /// 颜色
        /// </summary>
        public Color PenColor { get; set; } = Color.FromArgb(215,215,215);
        /// <summary>
        /// 是否被选择
        /// </summary>
        public bool IsSelected { get; set; } = false;
        /// <summary>
        /// 显示字体
        /// </summary>
        public Font TextFont { get; set; } = new Font("Tahoma", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color TextColor { get; set; } = Color.FromArgb(215, 215, 215);
        /// <summary>
        /// 显示文字
        /// </summary>
        public String Text { get; set; } = String.Empty;
        /// <summary>
        /// 端口号
        /// </summary>
        public int RFIndex { get; set; }
        public void Draw(Graphics g)
        {
            Pen pen = new Pen(PenColor);

            Rectangle r = new Rectangle(Location, new Size(Radius, Radius));
            pen.Width = PenWidth;
            GraphicsPath path = new GraphicsPath();
            path.AddLine(Location.X + Radius * 0.25f, (Location.Y + Radius * 0.5f) - (Radius * 0.25f) * 1.732f,
                Location.X + Radius * 0.75f, (Location.Y + Radius * 0.5f) - (Radius * 0.25f) * 1.732f);
            path.AddArc(r, -60, 120);
            path.AddLine(Location.X + Radius * 0.75f, (Location.Y + Radius * 0.5f) + (Radius * 0.25f) * 1.732f,
                Location.X + Radius * 0.25f, (Location.Y + Radius * 0.5f) + (Radius * 0.25f) * 1.732f);
            path.AddArc(r, 120, 120);
            g.FillPath(new SolidBrush(Color.FromArgb(37, 37, 38)), path);
            g.DrawPath(pen, path);
            if (IsSelected)
            {
                StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                Text = String.Format("{0}", RFIndex+1);
                g.DrawString(Text, TextFont, new SolidBrush(TextColor), r, sf);
            }
            else
            {
                r = new Rectangle(Location.X + (int)(Radius * 0.30), Location.Y + (int)(Radius * 0.30),
                    (int)(Radius * 0.45), (int)(Radius * 0.45));
                g.DrawArc(pen, r, 0, 360);
            }
        }

        /// <summary>
        /// 模型是否被聚焦
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsFocused(int x, int y)
        {
            Size size = new Size(Radius, Radius);
            if ((this.Location.X <= x && this.Location.X + size.Width >= x) &&
                (this.Location.Y <= y && this.Location.Y + size.Height >= y))
                return true;
            else
                return false;
        }
    }
}
