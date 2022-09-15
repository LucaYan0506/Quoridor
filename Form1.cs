using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Quoridor
{
    public partial class Form1 : Form
    {
        //component to init the game
        Tuple<int, int>[] dirs = new Tuple<int,int>[4];
        PawnPictureBox[] directions = new PawnPictureBox[4];

        PawnPictureBox[] Pawns = new PawnPictureBox[4];
        PawnPictureBox currPawn;

        TriangularPictureBox[] endArea = new TriangularPictureBox[4];

        Grid grid = new Grid();

        //component to make the game turn-based
        int turn = 0;


        //components for verticalLine and horizontalLine
        PictureBox curr;
        bool move = false;

        public Form1()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            //initialize endArea
            Image[] backgroundImages = { Properties.Resources.greenBackground, Properties.Resources.redBackground, Properties.Resources.blueBackground, Properties.Resources.yellowbackground };
            Point[] locations = { new Point(2, 0), new Point(534, 0), new Point(2, 536), new Point(0,0) };
            Size[] sizes = { new Size(600, 65), new Size(67, 600), new Size(600, 65), new Size(67, 600) };
            List<Point[]> points = new List<Point[]>();
            points.Add(
                new Point[]
                {
                    new Point(0, 0),
                    new Point(67, 67),
                    new Point(600 - 67, 67),
                    new Point(600, 0),
                }
            );
            points.Add(
                new Point[]
                {
                 new Point(67, 0),
                 new Point(63, 600),
                 new Point(0, 600 - 63),
                 new Point(0, 67),
                }
            );
            points.Add(
                new Point[]
                {
                 new Point(64, 0),
                 new Point(600 - 67, 0),
                 new Point(600, 67),
                 new Point(0, 64),
                }
            );
            points.Add(
                new Point[]
                {
                 new Point(0, 0),
                 new Point(67, 67),
                 new Point(67, 600 - 65),
                 new Point(2, 600),
                }
            );
            for (int i = 0; i < endArea.Length; i++)
            {
                endArea[i] = new TriangularPictureBox();
                endArea[i].BackgroundImage = backgroundImages[i];
                endArea[i].Location = locations[i];
                endArea[i].Name = "pictureBox4";
                endArea[i].Size = sizes[i];
                endArea[i].TabIndex = 3;
                endArea[i].TabStop = false;
                endArea[i].Points = points[i];

                this.gridPanel.Controls.Add(endArea[i]);
                endArea[i].BringToFront();
            }


            //initialize directions
            for (int i = 0; i < directions.Length; i++)
            {
                directions[i] = new PawnPictureBox();
                directions[i].BackColor = Color.Transparent;
                directions[i].Image = Image.FromFile("..\\..\\Resources\\Pawn.png");
                directions[i].BackgroundImageLayout = ImageLayout.None;
                directions[i].SizeMode = PictureBoxSizeMode.CenterImage;
                directions[i].Size = new Size(54, 54);
                directions[i].Visible = false;
                directions[i].Cursor = Cursors.Hand;
                directions[i].Click += movePawn;
                this.gridPanel.Controls.Add(directions[i]);
                directions[i].BringToFront();
            }

            //init dirs
            int distance = 67;
            dirs[0] = new Tuple<int, int>(distance, 0);
            dirs[1] = new Tuple<int, int>(-distance, 0);
            dirs[2] = new Tuple<int, int>(0, distance);
            dirs[3] = new Tuple<int, int>(0, -distance);

            //initialize pawns
            Image[] PawnImages = { Image.FromFile("..\\..\\Resources\\greenPawn.png"), Image.FromFile("..\\..\\Resources\\bluePawn.png"), Image.FromFile("..\\..\\Resources\\redPawn.png"), Image.FromFile("..\\..\\Resources\\yellowPawn.png") };
            Point[] PawnLocations = { new Point(273, 542), new Point(273, 6), new Point(5, 274), new Point(542, 274) };
            for (int i = 0; i < Pawns.Length; i++)
            {
                Pawns[i] = new PawnPictureBox();
                Pawns[i].BackColor = Color.Transparent;
                Pawns[i].BackgroundImageLayout = ImageLayout.None;
                Pawns[i].Cursor = Cursors.Hand;
                Pawns[i].Location = PawnLocations[i];
                Pawns[i].Name = "Pawns" + i;
                Pawns[i].Size = new Size(54, 54);
                Pawns[i].TabIndex = 1;
                Pawns[i].TabStop = false;
                Pawns[i].SizeMode = PictureBoxSizeMode.CenterImage;
                Pawns[i].Image = PawnImages[i];
                Pawns[i].Click += PawnImg_Click;
                this.gridPanel.Controls.Add(Pawns[i]);
                Pawns[i].BringToFront();

                //init grid
                int[] indexes = location_to_index(PawnLocations[i]);
                grid.Blocks[indexes[0], indexes[1]].isEmpty = false;
                Pawns[i].BackgroundImageLayout = ImageLayout.None;
            }

            //init "your turns"
            pawnTurn.Image = Pawns[turn].Image;
            pawnTurn.SizeMode = PictureBoxSizeMode.CenterImage;

        }

        private int[] location_to_index(Point location)
        {
            int[] res = new int[2];

            res[0] = (location.Y - 6) / 67;
            res[1] = (location.X - 5 ) / 67;


            return res;
        }

        private bool outside_bound(int i, int j, int m, int n)
        {
            return i < 0 || i >= m || j < 0 || j >= n;
        }

        private void PawnImg_Click(object sender, EventArgs e)
        {
            if ( int.Parse(((PawnPictureBox)sender).Name.Substring(5)) != turn)
                return;

            currPawn = (PawnPictureBox)sender;

            for (int i = 0; i < directions.Length; i++)
            {
                directions[i].Visible = false;

                Point loc = new Point(currPawn.Location.X + dirs[i].Item1, currPawn.Location.Y + dirs[i].Item2);
                int[] indexes = location_to_index(loc);
                if (!outside_bound(indexes[0],indexes[1],grid.Blocks.GetLength(0),grid.Blocks.GetLength(1)) && grid.Blocks[indexes[0], indexes[1]].isEmpty)
                {
                    directions[i].Location = loc;
                    directions[i].Visible = true;
                }
            }
        }

        private void movePawn(object sender, EventArgs e)
        {
            //update grid
            int[] indexes = location_to_index(currPawn.Location);
            grid.Blocks[indexes[0], indexes[1]].isEmpty = true;

            //update currPawn location
            currPawn.Location = ((PictureBox)sender).Location;

            //update grid
            indexes = location_to_index(currPawn.Location);
            grid.Blocks[indexes[0], indexes[1]].isEmpty = false;

            hide_directions();
            turn = (turn + 1) % Pawns.Length;

            pawnTurn.Image = Pawns[turn].Image;
        }

        private void hide_directions()
        {
            for (int i = 0; i < directions.Length; i++)
                directions[i].Visible = false;
        }

        private void grid_Click(object sender, EventArgs e)
        {
            hide_directions();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Line_MouseDown(object sender, MouseEventArgs e)
        {
            curr = (PictureBox)sender;
            move = true;
        }

        private void Line_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void Line_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
                curr.Location = new Point(MousePosition.X - this.Location.X - (MousePosition.X - this.Location.X - curr.Location.X), MousePosition.Y - this.Location.Y - (MousePosition.Y - this.Location.Y - curr.Location.Y));
        }
    }
}
