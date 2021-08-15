using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace GUI_Library
{
    public class viButton : Button, IDisposable
    {
        private int border_size = 0;
        [Category("Advance Settings")]
        public int BorderSize
        {
            get
            {
                return this.border_size;
            }
            set
            {
                this.border_size = value;
                this.Invalidate();
            }
        }

        private int border_radius = 40;
        [Category("Advance Settings")]
        public int BorderRadius
        {
            get
            {
                return this.border_radius;
            }
            set
            {
                if (value <= this.Height)
                {
                    this.border_radius = value;
                }
                else
                {
                    this.border_radius = this.Height;
                }

                this.Invalidate();
            }
        }

        private Color border_color = Color.PaleVioletRed;
        [Category("Advance Settings")]
        public Color BorderColor
        {
            get
            {
                return this.border_color;
            }
            set
            {
                this.border_color = value;
                this.Invalidate();
            }
        }

        public viButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = Color.MediumSlateBlue;
            this.ForeColor = Color.White;

            this.Resize += viButton_Resize;
        }

        private GraphicsPath GetFigurePath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rect_surface = new RectangleF(0, 0, this.Width, this.Height);
            RectangleF rect_border = new RectangleF(1, 1, this.Width - 0.8f, this.Height - 1);

            if (border_radius > 2)
            {
                using (GraphicsPath path_surface = GetFigurePath(rect_surface, border_radius))
                using (GraphicsPath path_border = GetFigurePath(rect_border, border_radius - 1f))
                using (Pen pen_surface = new Pen(this.Parent.BackColor, 2))
                using (Pen pen_border = new Pen(border_color, border_size))
                {
                    pen_border.Alignment = PenAlignment.Inset;
                    this.Region = new Region(path_surface);

                    pevent.Graphics.DrawPath(pen_surface, path_surface);

                    if (border_size >= 1)
                    {
                        pevent.Graphics.DrawPath(pen_border, path_border);
                    }
                }
            }
            else
            {
                this.Region = new Region(rect_surface);

                if (border_size >= 1)
                {
                    using (Pen pen_border = new Pen(border_color, border_size))
                    {
                        pen_border.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(pen_border, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += new EventHandler(Parent_BackColorChanged);
        }

        private void Parent_BackColorChanged(object sender, EventArgs e)
        {
            if (sender is null) return;

            if (this.DesignMode)
            {
                this.Invalidate();
            }
        }

        private void viButton_Resize(object sender, EventArgs e)
        {
            if (this.border_radius > this.Height)
            {
                this.border_radius = this.Height;
            }
        }

        #region IDisposable Implementation
        private bool disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
                Console.WriteLine("Explicit call: Dispose is called by the user.");
            }

            this.BackColorChanged -= Parent_BackColorChanged;
            this.Resize -= viButton_Resize;

            disposed = true;

            base.Dispose(disposing);
        }
        #endregion
    }
}
