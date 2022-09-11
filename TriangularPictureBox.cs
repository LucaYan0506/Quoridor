using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Quoridor
{
    public class TriangularPictureBox : PictureBox
    {
        private Point[] _points = { 
            new Point(0,0),
            new Point(0,0),
            new Point(0,0),
            new Point(0,0),
        };

        public Point[] Points
        {
            get { return _points; }
            set { _points = value; }
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            using (var p = new System.Drawing.Drawing2D.GraphicsPath())
            {
                p.AddPolygon(_points);

                this.Region = new Region(p);
                base.OnPaint(pe);
            }
        }
    }
}
