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

        //grid
        Grid grid = new Grid();
        Point[,] barrier = new Point[8, 8];
        Point[,] barrier1 = new Point[8, 8];
        Point[,] barrier2 = new Point[8, 8];


        //component to make the game turn-based
        int turn = 0;
        Point orginalLocation;


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
            Image[] PawnImages = { Image.FromFile("..\\..\\Resources\\greenPawn.png"), Image.FromFile("..\\..\\Resources\\redPawn.png"), Image.FromFile("..\\..\\Resources\\bluePawn.png"), Image.FromFile("..\\..\\Resources\\yellowPawn.png") };
            Point[] PawnLocations = { new Point(273, 542), new Point(5, 274), new Point(273, 6), new Point(542, 274) };
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
                int[] indexes = location_to_indexForPawns(PawnLocations[i]);
                grid.Blocks[indexes[0], indexes[1]].isEmpty = false;
                Pawns[i].BackgroundImageLayout = ImageLayout.None;
            }

            //init "your turns"
            pawnTurn.Image = Pawns[turn].Image;
            pawnTurn.SizeMode = PictureBoxSizeMode.CenterImage;

            //init grid barrier
            for (int i = 0; i < barrier1.GetLength(0); i++)
                for (int j = 0; j < barrier1.GetLength(1); j++)
                    barrier1[i, j] = new Point(61 + (j * 66), 0 + (i * 67));

            for (int i = 0; i < barrier2.GetLength(0); i++)
                for (int j = 0; j < barrier2.GetLength(1); j++)
                    barrier2[i, j] = new Point(0 + (j * 66), 61 + (i * 67));
        }

        private int[] location_to_indexForPawns(Point location)
        {
            int[] res = new int[2];

            res[0] = (location.Y - 6) / 67;
            res[1] = (location.X - 5 ) / 67;

            return res;
        }

        private bool outside_bound(int i,int m)
        {
            return i < 0 || i >= m;
        }

        private void PawnImg_Click(object sender, EventArgs e)
        {
            //if it isn't his turn, then don't allow him to click pawn.
            if ( int.Parse(((PawnPictureBox)sender).Name.Substring(5)) != turn)
                return;

            currPawn = (PawnPictureBox)sender;

            //generate "grey pawn" (dirs) 
            for (int i = 0; i < directions.Length; i++)
            {
                directions[i].Visible = false;

                Point loc = new Point(currPawn.Location.X + dirs[i].Item1, currPawn.Location.Y + dirs[i].Item2);
                int[] indexes = location_to_indexForPawns(loc);
                if (!outside_bound(indexes[0],grid.Blocks.GetLength(0)) && !outside_bound(indexes[1], grid.Blocks.GetLength(1)))
                {
                    //if there isn't any pawn in the cell
                    if (grid.Blocks[indexes[0], indexes[1]].isEmpty)
                    {
                        directions[i].Location = loc;
                        directions[i].Visible = true;
                    }
                    else
                    {
                        int index = 2;
                        bool isEmpty = true;
                        do
                        {
                            loc = new Point(currPawn.Location.X + dirs[i].Item1 * index, currPawn.Location.Y + dirs[i].Item2 * index);
                            indexes = location_to_indexForPawns(loc);

                            if (outside_bound(indexes[0], grid.Blocks.GetLength(0)) || outside_bound(indexes[1], grid.Blocks.GetLength(1)))
                            {
                                isEmpty = false;
                                break;
                            }
                        } while (!grid.Blocks[indexes[0], indexes[1]].isEmpty);

                        //if there is a cell where there isn't any pawn, then that direction is available
                        if (isEmpty)
                        {
                            directions[i].Location = loc;
                            directions[i].Visible = true;
                        }

                    }

                }
            }
        }

        private void movePawn(object sender, EventArgs e)
        {
            //update grid
            int[] indexes = location_to_indexForPawns(currPawn.Location);
            grid.Blocks[indexes[0], indexes[1]].isEmpty = true;

            //update currPawn location
            currPawn.Location = ((PictureBox)sender).Location;

            //update grid
            indexes = location_to_indexForPawns(currPawn.Location);
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

        private int[] location_to_indexForBarrier(Point location)
        {
            int[] res = new int[2];

            res[0] = (int)Math.Round((location.Y - 32) / 66.0) - 1;
            res[1] = (int)Math.Round((location.X - 75) / 68.0);

            label2.Text = location.ToString() + "\n" + " " + res[0] + " " + res[1];

            return res;
        }

        private void Line_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return;

            curr = (PictureBox)sender;

            if (curr.Name == "HorizontalLine")
                barrier = barrier2;
            else
                barrier = barrier1;

            Point MouseLocation = new Point(MousePosition.X - this.Location.X, MousePosition.Y - this.Location.Y);
            orginalLocation = curr.Location;

            move = true;
        }

        private void Line_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox newLine = new PictureBox();
            newLine.BackColor = ((PictureBox)sender).BackColor;
            newLine.Location = ((PictureBox)sender).Location;
            newLine.Size = ((PictureBox)sender).Size;

            gridPanel.Controls.Add(newLine);
            newLine.BringToFront();

            ((PictureBox)sender).Location = orginalLocation;
            move = false;
        }

        private void Line_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                Point MouseLocation = new Point(MousePosition.X - this.Location.X, MousePosition.Y - this.Location.Y);
                int[] indexes = location_to_indexForBarrier(MouseLocation);

                if (!outside_bound(indexes[0], barrier.GetLength(0)))
                    curr.Location = new Point (curr.Location.X, barrier[indexes[0], 0].Y);

                if (!outside_bound(indexes[1], barrier.GetLength(1)))
                      curr.Location = new Point(barrier[0, indexes[1]].X, curr.Location.Y);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(grid.Blocks[0, 0].BorderBot.isEmpty.ToString());
        }
    }
}
