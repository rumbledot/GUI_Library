using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GUI_Library
{
    public class viRadioButton : RadioButton
    {
        private Color checked_color = Color.MediumSlateBlue;
        public Color CheckedColor
        {
            get { return checked_color; }
            set
            {
                checked_color = value;
                this.Invalidate();
            }
        }

        private Color unchecked_color = Color.Gray;
        public Color UnCheckedColor
        {
            get { return unchecked_color; }
            set
            {
                unchecked_color = value;
                this.Invalidate();
            }
        }

        public viRadioButton()
        {
            this.MinimumSize = new Size(0, 21);
            //Add a padding of 10 to the left to have a considerable distance between the text and the RadioButton.
            this.Padding = new Padding(10, 0, 0, 0);
        }

        //Overridden methods
        protected override void OnPaint(PaintEventArgs pevent)
        {
            //Fields
            Graphics graphics = pevent.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            float border_size = 18F;
            float check_size = 12F;
            RectangleF rect_border = new RectangleF()
            {
                X = 0.5F,
                Y = (this.Height - border_size) / 2, //Center
                Width = border_size,
                Height = border_size
            };
            RectangleF rect_check = new RectangleF()
            {
                X = rect_border.X + ((rect_border.Width - check_size) / 2), //Center
                Y = (this.Height - check_size) / 2, //Center
                Width = check_size,
                Height = check_size
            };

            using (Pen pen_border = new Pen(checked_color, 1.6F))
            using (SolidBrush brush_check = new SolidBrush(checked_color))
            using (SolidBrush brush_text = new SolidBrush(this.ForeColor))
            {
                graphics.Clear(this.BackColor);
                
                if (this.Checked)
                {
                    graphics.DrawEllipse(pen_border, rect_border);
                    graphics.FillEllipse(brush_check, rect_check);
                }
                else
                {
                    pen_border.Color = unchecked_color;
                    graphics.DrawEllipse(pen_border, rect_border);
                }
                
                graphics.DrawString(this.Text, this.Font, brush_text,
                    border_size + 8, (this.Height - TextRenderer.MeasureText(this.Text, this.Font).Height) / 2);
            }
        }
    }
}
