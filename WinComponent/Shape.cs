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
    /// 绘制矩形
    /// </summary>
    public class Shape
    {
        #region 属性
        /// <summary>
        /// 位置
        /// </summary>
        public Point Location
        {
            get { return this._location; }
            set { this._location = value; }
        }
        private Point _location = new Point(0, 0);

        /// <summary>
        /// 大小
        /// </summary>
        public Size Size
        {
            get { return this._size; }
            set { this._size = value; }
        }
        private Size _size = new Size(10, 10);
        /// <summary>
        /// 填充色
        /// </summary>
        public Color BackColor
        {
            get { return this._backColor; }
            set { this._backColor = value; }
        }
        private Color _backColor = Color.White;
        /// <summary>
        /// 内容位置
        /// </summary>
        public PointF ContentMargin { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get { return this._content; }
            set { this._content = value; }
        }
        private string _content = String.Empty;
        /// <summary>
        /// 字体
        /// </summary>
        public Font Font
        {
            get { return this._font; }
            set { this._font = value; }
        }
        private Font _font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color FontColor
        {
            get { return this._fontColor; }
            set { this._fontColor = value; }
        }
        private Color _fontColor = Color.Black;
        /// <summary>
        /// 边框厚度
        /// </summary>
        public int BorderThinkness
        {
            get { return this._borderThinkness; }
            set { this._borderThinkness = value; }
        }
        private int _borderThinkness = 1;

        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor
        {
            get { return this._borderColor; }
            set { this._borderColor = value; }
        }
        private Color _borderColor = Color.Black;
        /// <summary>
        /// 使能圆角
        /// </summary>
        public bool EnableRoundCorner
        {
            get { return this._enableRoundCorner; }
            set { this._enableRoundCorner = value; }
        }
        private bool _enableRoundCorner = false;
        /// <summary>
        /// 圆角半径
        /// </summary>
        public int RoundCornerRadius
        {
            get { return this._roundCornerRadius; }
            set { this._roundCornerRadius = value; }
        }
        private int _roundCornerRadius = 5;

        #endregion
        protected Graphics graph;

        public Shape() { }

        public virtual void Draw(Graphics g)
        {
            this.graph = g;
            graph.SmoothingMode = SmoothingMode.HighQuality;
            Rectangle rectangle = new Rectangle(Location, Size);
            if (EnableRoundCorner)
            {
                FillRoundRectangle(g, new SolidBrush(BackColor), rectangle, RoundCornerRadius);
            }
            else
                graph.FillRectangle(new SolidBrush(BackColor), rectangle);
            if (BorderThinkness > 0)
            {
                Pen borderPen = new Pen(BorderColor, BorderThinkness);
                graph.DrawRectangle(borderPen, rectangle);
            }
            if (Content != String.Empty)
            {
                PointF contentPointF = new PointF(Location.X + ContentMargin.X, Location.Y + ContentMargin.Y);
                graph.DrawString(Content, Font, new SolidBrush(FontColor), contentPointF);
            }
        }
        /// <summary>
        /// 模型是否被聚焦
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual bool IsFocused(int x, int y)
        {
            if ((this.Location.X <= x && this.Location.X + this.Size.Width >= x) &&
                (this.Location.Y <= y && this.Location.Y + this.Size.Height >= y))
                return true;
            else
                return false;
        }
        /// <summary>
        /// /模型是否在框选中
        /// </summary>
        /// <param name="beginX">左上角X</param>
        /// <param name="beginY">左上角Y</param>
        /// <param name="endX">右下角X</param>
        /// <param name="endY">右下角Y</param>
        /// <returns></returns>
        public bool IsInsideFrame(int beginX, int beginY, int endX, int endY)
        {
            if ((this.Location.X >= beginX && this.Location.X + this.Size.Width <= endX) &&
            (this.Location.Y >= beginY && this.Location.Y + this.Size.Height <= endY))
                return true;
            else
            {
                if ((this.Location.X <= beginX && this.Location.X + this.Size.Width >= endX) &&
                    (this.Location.Y <= beginY && this.Location.Y + this.Size.Height >= endY))
                    return true;
                else
                    return false;
            }
        }


        protected void DrawRoundRectangle(Graphics g, Pen pen, Rectangle rect, int cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                g.DrawPath(pen, path);
            }
        }

        protected void FillRoundRectangle(Graphics g, Brush brush, Rectangle rect, int cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                g.FillPath(brush, path);
            }
        }
        protected GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cRadius)
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
