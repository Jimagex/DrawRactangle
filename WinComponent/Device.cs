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
    public class Device
    {
        public Point Location { get; set; } = new Point(0,0);
        public List<Board> Boards { get; set; } = new List<Board>();

        private Color DeviceColor = Color.FromArgb(48, 50, 52);

        
        public void Initilize(Point location, int[][] physicsChannel)
        {
            this.Location = location;
            if(physicsChannel == null)
            {
                return;
            }
            for (int i = 0; i < physicsChannel.Length;i++)
            {
                Board board = new Board();
                board.Initilize(new Point(this.Location.X + 10, this.Location.Y + 10 + i * 60), physicsChannel[i]);
                this.Boards.Add(board);
            }
        }

        public void Draw(Graphics g)
        {
            Rectangle backgrand = new Rectangle(Location,
                 new Size(490, 160));
            FillRoundRectangle(g, new SolidBrush(DeviceColor), backgrand, 5);
            backgrand = new Rectangle(new Point(Location.X + 5, Location.Y + 5), new Size(480, 150));
            DrawRoundRectangle(g, new Pen(Color.FromArgb(37,37,38)), backgrand, 5);
            backgrand = new Rectangle(new Point(Location.X + 20, Location.Y + 160),
                new Size(450, 15));
            g.FillRectangle(new SolidBrush(Color.FromArgb(58, 58, 58)), backgrand);
            g.DrawRectangle(new Pen(DeviceColor), backgrand);
            for (int i = 0; i < this.Boards.Count; i++)
                this.Boards[i].Draw(g);
        }
        /// <summary>
        /// 鼠标聚焦
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Channel SetFocus(int x, int y)
        {
            foreach (Board board in this.Boards)
            {
                Channel channel = board.SetFocus(x, y);
                if (channel != null)
                    return channel;
            }
            return null;
        }
        /// <summary>
        /// 端口聚焦
        /// </summary>
        /// <param name="rfindex">端口号</param>
        public void SetFocusChannel(int rfindex)
        {
            foreach (Board board in this.Boards)
            {
                board.SetChannelFocus(rfindex);
            }
        }
        /// <summary>
        /// 清除聚焦
        /// </summary>
        public void ClearFocus()
        {
            foreach (Board board in this.Boards)
            {
                board.ClearFocus();
                board.IsFocused = false;
            }
        }

        /// <summary>
        /// 返回所有按端口升序排列的端口集合
        /// </summary>
        /// <returns></returns>
        public List<Channel> GraphChannels()
        {
            List<Channel> channels = (from board in this.Boards from ch in board.Channels select ch).OrderBy(x => x.RFIndex).ToList();
            return channels;
        }
        private void DrawRoundRectangle(Graphics g, Pen pen, Rectangle rect, int cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                g.DrawPath(pen, path);
            }
        }
        private void FillRoundRectangle(Graphics g, Brush brush, Rectangle rect, int cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                g.FillPath(brush, path);
            }
        }
        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cRadius)
        {
            GraphicsPath myPath = new GraphicsPath();
            myPath.StartFigure();
            myPath.AddArc(new Rectangle(new Point(rect.X, rect.Y), new Size(2 * cRadius, 2 * cRadius)), 180, 90);
            myPath.AddLine(new Point(rect.X + cRadius, rect.Y), new Point(rect.Right - cRadius, rect.Y));
            myPath.AddArc(new Rectangle(new Point(rect.Right - 2 * cRadius, rect.Y), new Size(2 * cRadius, 2 * cRadius)), 270, 90);
            myPath.AddLine(new Point(rect.Right, rect.Y + cRadius), new Point(rect.Right, rect.Bottom - cRadius));
            myPath.AddArc(new Rectangle(new Point(rect.Right - 2 * cRadius, rect.Bottom - 2 * cRadius), new Size(2 * cRadius, 2 * cRadius)), 0, 90);
            myPath.AddLine(new Point(rect.Right - cRadius, rect.Bottom), new Point(rect.X + cRadius, rect.Bottom));
            myPath.AddArc(new Rectangle(new Point(rect.X, rect.Bottom - 2 * cRadius), new Size(2 * cRadius, 2 * cRadius)), 90, 90);
            myPath.AddLine(new Point(rect.X, rect.Bottom - cRadius), new Point(rect.X, rect.Y + cRadius));
            myPath.CloseFigure();
            return myPath;
        }
    }
}
