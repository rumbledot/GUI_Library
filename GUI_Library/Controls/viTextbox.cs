using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace GUI_Library
{
    [DefaultEvent("_text_changed")]
    public partial class viTextbox : UserControl, IDisposable
    {
        private const int max_border_radius = 14;

        private Color border_color = Color.MediumSlateBlue;
        public Color BorderColor
        {
            get => border_color;
            set
            {
                border_color = value;
                this.Invalidate();
            }
        }

        private int border_size = 2;
        public int BorderSize
        {
            get => border_size;
            set
            {
                border_size = value;
                this.Invalidate();
            }
        }

        private int border_radius = 10;
        public int BorderRadius
        {
            get
            {
                return this.border_radius;
            }
            set
            {
                if (Multiline && (value <= max_border_radius))
                {
                    this.border_radius = value;
                }
                else if (Multiline && (value > max_border_radius))
                {
                    this.border_radius = max_border_radius;
                }
                else if (!Multiline && (value <= this.Height / 2))
                {
                    this.border_radius = value;
                }
                else if(!Multiline && (value > this.Height / 2))
                {
                    this.border_radius = this.Height / 2;
                }

                this.Invalidate();
            }
        }

        private bool underline_style = false;
        public bool UnderlineStyle
        {
            get => underline_style;
            set
            {
                underline_style = value;
                this.Invalidate();
            }
        }

        private Color border_focus_color = Color.HotPink;
        public Color BorderFocusColor
        {
            get => border_focus_color;
            set => border_focus_color = value;
        }

        private bool is_focused = false;

        private Color placeholder_color = Color.DarkGray;
        public Color PlaceholderColor 
        { 
            get => placeholder_color; 
            set 
            { 
                placeholder_color = value;
                if (is_passwordchar)
                {
                    tbox_text.ForeColor = value;
                }
            }
        }

        private string placeholder_text = "";
        public string PlaceholderText 
        {
            get 
            {
                return placeholder_text;
            }
            set 
            { 
                placeholder_text = value;
                tbox_text.Text = "";
                this.SetPlaceHolder();
            }
        }

        private bool is_placeholder = false;
        public bool IsPlaceholder 
        { 
            get => is_placeholder; 
            set => is_placeholder = value; 
        }

        private bool is_passwordchar = false;
        public bool IsPasswordchar 
        { 
            get => is_passwordchar;
            set 
            {
                is_passwordchar = value;
                tbox_text.UseSystemPasswordChar = value; 
            }
        }


        public bool PasswordChar
        {
            get
            {
                return tbox_text.UseSystemPasswordChar;
            }
            set
            {
                tbox_text.UseSystemPasswordChar = value;
            }
        }

        public bool Multiline
        {
            get
            {
                return tbox_text.Multiline;
            }
            set
            {
                tbox_text.Multiline = value;
            }
        }

        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                tbox_text.BackColor = value;
            }
        }

        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                tbox_text.ForeColor = value;
            }
        }

        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                tbox_text.Font = value;
                if (this.DesignMode)
                {
                    this.UpdateControlHeight();
                }
            }
        }

        public override string Text
        {
            get 
            {
                if (is_placeholder) return "";
                return tbox_text.Text.Trim();
            }
            set
            {
                tbox_text.Text = value;
                this.SetPlaceHolder();
            }
        }

        public event EventHandler _textbox_TextChanged;

        public viTextbox()
        {
            InitializeComponent();
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;

            if (this.border_radius > 1)
            {
                var rect_border_smooth = this.ClientRectangle;
                var rect_border = Rectangle.Inflate(rect_border_smooth, -border_size, -border_size);
                int smooth_size = border_size > 0 ? border_size : 1;

                using (GraphicsPath path_border_smooth = GetFigurePath(rect_border_smooth, border_radius))
                using (GraphicsPath path_border = GetFigurePath(rect_border, border_radius - border_size))
                using (Pen pen_border_smooth = new Pen(this.Parent.BackColor, smooth_size))
                using (Pen pen_border = new Pen(border_color, border_size))
                {
                    this.Region = new Region(path_border_smooth);

                    if (this.border_radius > 15) SetTextboxRoundedRegion();

                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    pen_border.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;

                    if (this.is_focused)
                    {
                        pen_border.Color = border_focus_color;
                    }

                    if (underline_style)
                    {
                        graph.DrawPath(pen_border_smooth, path_border_smooth);
                        graph.SmoothingMode = SmoothingMode.None;
                        graph.DrawLine(pen_border, 0, this.Height - 1, this.Width, this.Height - 1);
                    }
                    else
                    {
                        graph.DrawPath(pen_border_smooth, path_border_smooth);
                        graph.DrawPath(pen_border, path_border);
                    }
                }
            }
            else 
            {
                using (Pen pen_border = new Pen(border_color, border_size))
                {
                    this.Region = new Region(this.ClientRectangle);

                    pen_border.Alignment = PenAlignment.Inset;

                    if (this.is_focused)
                    {
                        pen_border.Color = border_focus_color;
                    }

                    if (underline_style)
                    {
                        graph.DrawLine(pen_border, 0, this.Height - 1, this.Width, this.Height - 1);
                    }
                    else
                    {
                        graph.DrawRectangle(pen_border, 0, 0, this.Width - 0.5f, this.Height - 0.5f);
                    }
                } 
            }
        }

        private void SetTextboxRoundedRegion()
        {
            GraphicsPath path_text;
            if (Multiline)
            {
                path_text = GetFigurePath(tbox_text.ClientRectangle, border_radius - border_size);
                tbox_text.Region = new Region(path_text);
            }
            else 
            {
                path_text = GetFigurePath(tbox_text.ClientRectangle, border_size * 2);
                tbox_text.Region = new Region(path_text);
            }
        }

        private void SetPlaceHolder()
        {
            if (string.IsNullOrEmpty(tbox_text.Text) && !placeholder_text.Equals(string.Empty)) 
            {
                is_placeholder = true;
                tbox_text.Text = placeholder_text;
                tbox_text.ForeColor = placeholder_color;
                if (is_passwordchar) 
                {
                    tbox_text.UseSystemPasswordChar = false;
                }
            }
        }

        private void RemovePlaceHolder()
        {
            if (is_placeholder && !placeholder_text.Equals(string.Empty))
            {
                is_placeholder = false;
                tbox_text.Text = "";
                tbox_text.ForeColor = this.ForeColor;
                if (is_passwordchar)
                {
                    tbox_text.UseSystemPasswordChar = true;
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.DesignMode)
            {
                this.UpdateControlHeight();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.UpdateControlHeight();
        }

        private void UpdateControlHeight()
        {
            if (!tbox_text.Multiline)
            {
                int text_height = TextRenderer.MeasureText("Text", this.Font).Height + 1;
                tbox_text.Multiline = true;
                tbox_text.MinimumSize = new Size(0, text_height);
                tbox_text.Multiline = false;

                this.Height = tbox_text.Height + this.Padding.Top + this.Padding.Bottom;
            }
        }

        private void textbox_TextChanged(object sender, EventArgs e)
        {
            if (this._textbox_TextChanged != null) 
            {
                this._textbox_TextChanged.Invoke(sender, e);
            }
        }

        private void textbox_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void textbox_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        private void textbox_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        private void textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        private void textbox_Enter(object sender, EventArgs e)
        {
            this.is_focused = true;
            this.Invalidate();
            this.RemovePlaceHolder();
        }

        private void textbox_Leave(object sender, EventArgs e)
        {
            this.OnLeave(e);
            this.is_focused = false;
            this.Invalidate();
            this.SetPlaceHolder();
        }
    }
}
