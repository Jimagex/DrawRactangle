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
    /// 设备一张板卡，涵盖8个端口，8个指示灯
    /// </summary>
    public class Board
    {
        /// <summary>
        /// 端口角色：未指派
        /// </summary>
        public static int CHANNEL_UNASSIGN = -1;
        /// <summary>
        /// 端口角色：未安装
        /// </summary>
        public static int CHANNEL_UNINSTALL = -2;
        /// <summary>
        /// 板卡编号
        /// </summary>
        public int Index { get; set; } = 0;
        /// <summary>
        /// 板卡位置
        /// </summary>
        public Point Location { get; set; } = new Point(0, 0);
        /// <summary>
        /// 端口集合
        /// </summary>

        public List<Channel> Channels = new List<Channel>();
        /// <summary>
        /// 端口到上方的距离
        /// </summary>
        private const int ChannelTopMargin = 10;
        /// <summary>
        /// 端口到左边的距离
        /// </summary>
        private const int ChannelLeftMargin = 20;
        /// <summary>
        /// 端口大小
        /// </summary>
        private const int ChannelRadius = 36;
        /// <summary>
        /// 端口间间距
        /// </summary>
        private const int ChannelToChannelMargin = 20;
        /// <summary>
        /// 端口颜色
        /// </summary>
        private Color ChannelPenColor = Color.White;
        /// <summary>
        /// 端口字体
        /// </summary>
        private Font TextFont { get; set; } = new Font("Tahoma", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
        /// <summary>
        /// 背景图
        /// </summary>
        private Color BoardColor = Color.FromArgb(48,50,52);
        private Color BoardFocusColor = Color.FromArgb(0, 149, 218);
        public bool IsFocused { get; set; } = false;
        /// <summary>
        ///  物理端口数
        /// </summary>
        private const int physicsCount = 8;
        /// <summary>
        /// 端口角色定义：-2 端口未安装, -1 端口未配置, 0 端口配置为BS, 1 端口配置为UE
        /// </summary>
        private int[] Roles;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="physicsChannels">物理端口号</param>
        /// <returns></returns>
        public bool Initilize(Point location, int[] physicsChannels)
        {
            this.Location = location;
            if (physicsChannels == null)
                return true;
            if (physicsChannels.Length > 8)
                return false;

            for(int i = 0; i < physicsChannels.Length; i++)
            {
                Channel channel = new Channel();
                channel.Location = new Point(Location.X + ChannelLeftMargin + i * (ChannelRadius + ChannelToChannelMargin),
                    Location.Y + ChannelTopMargin);
                channel.PenColor = ChannelPenColor;
                channel.PenWidth = 2f;
                channel.Radius = ChannelRadius;
                channel.RFIndex = physicsChannels[i];
                this.Channels.Add(channel);
            }
            this.Roles = new int[physicsCount];
            //端口角色分配
            for(int i = 0; i < physicsCount; i++)
            {
                if (i < physicsChannels.Length)
                    Roles[i] = CHANNEL_UNASSIGN;
                else
                    Roles[i] = CHANNEL_UNINSTALL;
            }
            return true;
        }

        public void Draw(Graphics g)
        {
            Rectangle backgrand = new Rectangle(Location,
                new Size(ChannelLeftMargin * 2 + 8 * (ChannelRadius + ChannelToChannelMargin) - ChannelToChannelMargin,
                ChannelTopMargin * 2 + ChannelRadius + 15));
            Color backcolor = IsFocused ? BoardFocusColor : BoardColor;
            FillRoundRectangle(g, new SolidBrush(backcolor), backgrand, 5);
            for (int i = 0; i < Channels.Count; i++)
            {
                string Text = (Channels[i].RFIndex + 1).ToString();
                StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                Rectangle r = new Rectangle(Location.X + ChannelLeftMargin + i * (ChannelRadius + ChannelToChannelMargin),
                    Location.Y + ChannelTopMargin + ChannelRadius + 5, ChannelRadius, 10);
                g.DrawString(Text, TextFont, new SolidBrush(ChannelPenColor), r, sf);
                Channels[i].Draw(g);
            }
            for(int i = 0; i < 8 - Channels.Count; i++)
            {
                Circle circle = new Circle();
                circle.Location = new Point(Location.X + ChannelLeftMargin + (Channels.Count + i) * (ChannelRadius + ChannelToChannelMargin),
                    Location.Y + ChannelTopMargin);
                circle.Size = new Size(ChannelRadius, ChannelRadius);
                circle.BorderThinkness = 0;
                circle.BackColor = Color.FromArgb(37, 37, 38);
                circle.Draw(g);
            }

        }

        /// <summary>
        /// 设置聚焦
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Channel SetFocus(int x, int y)
        {
            Channel ch = null;
            foreach (Channel channel in this.Channels)
            {
                if (channel.IsFocused(x, y))
                {
                    channel.PenColor = Color.FromArgb(0, 149, 218);
                    ch = channel;
                }
                else
                    channel.PenColor = ChannelPenColor;
            }
            return ch;
        }
        /// <summary>
        /// 根据端口号设置聚焦
        /// </summary>
        /// <param name="rfindex">物理端口号</param>
        public void SetChannelFocus(int rfindex)
        {
            Channel channel = this.Channels.Find(x => x.RFIndex == rfindex);
            if(channel != null)
            {
                channel.PenColor = Color.FromArgb(0, 149, 218);
            }
        }
        /// <summary>
        /// 清除聚焦
        /// </summary>
        public void ClearFocus()
        {
            foreach (Channel channel in this.Channels)
            {
                channel.PenColor = ChannelPenColor;
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
