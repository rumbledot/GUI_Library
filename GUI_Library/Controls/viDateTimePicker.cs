using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GUI_Library.Controls
{
    public class viDateTimePicker : DateTimePicker
    {
        private bool dropped_down = false;
        private Image calendar_icon = Properties.Resources.calendar_white;
        private RectangleF icon_button_area;
        private const int calendar_icon_width = 34;
        private const int arrow_icon_width = 17;

        private Color skin_color = Color.MediumSlateBlue;
        public Color SkinColor
        {
            get { return skin_color; }
            set
            {
                skin_color = value;
                if (skin_color.GetBrightness() >= 0.8F)
                    calendar_icon = Properties.Resources.calendar_dark;
                else calendar_icon = Properties.Resources.calendar_white;
                this.Invalidate();
            }
        }

        private Color text_color = Color.White;
        public Color TextColor
        {
            get { return text_color; }
            set
            {
                text_color = value;
                this.Invalidate();
            }
        }

        private Color border_color = Color.PaleVioletRed;
        public Color BorderColor
        {
            get { return border_color; }
            set
            {
                border_color = value;
                this.Invalidate();
            }
        }

        private int border_size = 0;
        public int BorderSize
        {
            get { return border_size; }
            set
            {
                border_size = value;
                this.Invalidate();
            }
        }

        public viDateTimePicker()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.MinimumSize = new Size(0, 35);
            this.Font = new Font(this.Font.Name, 9.5F);
        }

        //Overridden methods
        protected override void OnDropDown(EventArgs eventargs)
        {
            base.OnDropDown(eventargs);
            dropped_down = true;
        }
        protected override void OnCloseUp(EventArgs eventargs)
        {
            base.OnCloseUp(eventargs);
            dropped_down = false;
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            e.Handled = true;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            using (Graphics graphics = this.CreateGraphics())
            using (Pen pen_border = new Pen(border_color, border_size))
            using (SolidBrush skin_brush = new SolidBrush(skin_color))
            using (SolidBrush open_icon_brush = new SolidBrush(Color.FromArgb(50, 64, 64, 64)))
            using (SolidBrush text_brush = new SolidBrush(text_color))
            using (StringFormat text_format = new StringFormat())
            {
                RectangleF clientArea = new RectangleF(0, 0, this.Width - 0.5F, this.Height - 0.5F);
                RectangleF iconArea = new RectangleF(clientArea.Width - calendar_icon_width, 0, calendar_icon_width, clientArea.Height);
                pen_border.Alignment = PenAlignment.Inset;
                text_format.LineAlignment = StringAlignment.Center;

                graphics.FillRectangle(skin_brush, clientArea);
                graphics.DrawString("   " + this.Text, this.Font, text_brush, clientArea, text_format);

                if (dropped_down == true) graphics.FillRectangle(open_icon_brush, iconArea);

                if (border_size >= 1) graphics.DrawRectangle(pen_border, clientArea.X, clientArea.Y, clientArea.Width, clientArea.Height);

                graphics.DrawImage(calendar_icon, this.Width - calendar_icon.Width - 9, (this.Height - calendar_icon.Height) / 2);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            int iconWidth = GetIconButtonWidth();
            icon_button_area = new RectangleF(this.Width - iconWidth, 0, iconWidth, this.Height);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (icon_button_area.Contains(e.Location))
            {
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        private int GetIconButtonWidth()
        {
            int textWidh = TextRenderer.MeasureText(this.Text, this.Font).Width;
            if (textWidh <= this.Width - (calendar_icon_width + 20))
            {
                return calendar_icon_width;
            }
            else 
            { 
                return arrow_icon_width; 
            }
        }
    }
}
