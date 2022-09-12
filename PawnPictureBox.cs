using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Quoridor
{
    public class PawnPictureBox:PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            using (var p = new System.Drawing.Drawing2D.GraphicsPath())
            {
                Rectangle _rectangle = new Rectangle(14, 0, 21, 21);
                Point[] _points = new Point[]{
                    new Point(17,20),
                    new Point(31,21),
                    new Point(43,56),
                    new Point(7,56),
                };


                p.AddEllipse(_rectangle);
                p.AddPolygon(_points); 
                this.Region = new Region(p);
                base.OnPaint(pe);
            }
        }
    }
}
