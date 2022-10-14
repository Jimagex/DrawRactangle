using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KSW.WirelessChannelEmulation.ViewBase.Equipment;

namespace WinComponent
{
    public partial class Form1 : Form
    {
        Device DeviceMaster = new Device();
        /// <summary>
        /// 连接线颜色
        /// </summary>
        private Color LinkLineColor = Color.FromArgb(0, 149, 218);
        public Form1()
        {
            InitializeComponent();
            DeviceMaster = new Device();
            int[][] port = new int[2][];
            port[1] = new int[3] { 1, 2, 3 };
            port[0] = new int[3] { 4, 5, 6 };


            DeviceMaster.Initilize(new Point(100, 25), port);
        }

        private void _PictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;//抗锯齿
            g.Clear(Color.FromArgb(150, 150, 150));
            this.DeviceMaster?.Draw(g);

            //DrawLinkLine(g, rfFrom, rfTo);

        }

        private void DrawLinkLine(Graphics g, Point p1, Point p2)
        {
            Pen pen = new Pen(LinkLineColor) { Width = 2 };
            g.DrawCurve(pen, new Point[] { p1, new Point(p1.X + (int)((p2.X - p1.X) * 0.5) - 50, p1.Y - 40), p2 });
        }
    }
}
