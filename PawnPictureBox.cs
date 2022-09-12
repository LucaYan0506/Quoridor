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
                p.AddEllipse(new Rectangle(17,0,20,20));
                p.AddPolygon(new Point[]
                {
                    new Point(20,20),
                    new Point(30,20),
                    new Point(37,46),
                    new Point(14,46),
                }); 
                this.Region = new Region(p);
                base.OnPaint(pe);
            }
        }
    }
}
