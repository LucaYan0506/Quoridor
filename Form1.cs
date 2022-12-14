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
        bool four_people = true;
        bool[] pawnWin = new bool[4];

        Tuple<int, int>[] dirs = new Tuple<int,int>[4];
        PawnPictureBox[] directions = new PawnPictureBox[4];

        PawnPictureBox[] Pawns = new PawnPictureBox[4];
        PawnPictureBox currPawn;

        TriangularPictureBox[] endArea = new TriangularPictureBox[4];

        //grid
        Grid grid;
        Point[,] barrier = new Point[8, 8];
        Point[,] barrier1 = new Point[8, 8];
        Point[,] barrier2 = new Point[8, 8];

        //component to make the game turn-based
        int turn;
        Point orginalLocation;
        int[] wallsLeft;

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
            //clear everything in gridPanel
            gridPanel.Controls.Clear();
            
            //reset turns and walls left
            turn = 0;
            wallsLeft = new int[]{10, 10, 10, 10};

            //reset pawnWin
            if (four_people)
                pawnWin = new bool[]
                    {
                    false,false,false,false
                    };
            else
                pawnWin = new bool[]
                    {
                    false,true,false,true
                    };

            //new grid
            grid = new Grid();

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
            dirs[0] = new Tuple<int, int>(distance, 0);  //right
            dirs[1] = new Tuple<int, int>(-distance, 0); //left
            dirs[2] = new Tuple<int, int>(0, distance);  //bot
            dirs[3] = new Tuple<int, int>(0, -distance); //top

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
                    barrier1[i, j] = new Point(61 + (j * 66), 29 + (i * 67));

            for (int i = 0; i < barrier2.GetLength(0); i++)
                for (int j = 0; j < barrier2.GetLength(1); j++)
                    barrier2[i, j] = new Point(0 + (j * 66), 90 + (i * 67));

            //if there are only 2 people disable other 2
            if (!four_people)
            {
                //update grid
                Pawns[1].Visible = false;
                int[] indexes = location_to_indexForPawns(Pawns[1].Location);
                grid.Blocks[indexes[0], indexes[1]].isEmpty = true;

                Pawns[3].Visible = false;
                indexes = location_to_indexForPawns(Pawns[3].Location);
                grid.Blocks[indexes[0], indexes[1]].isEmpty = true;
            }
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

        private bool win(PawnPictureBox curr, Point location)
        {
            switch (curr.Name)
            {
                case "Pawns0":
                    return location_to_indexForPawns(location)[0] == 0;
                case "Pawns1":
                    return location_to_indexForPawns(location)[1] == 8;
                case "Pawns2":
                    return location_to_indexForPawns(location)[0] == 8;
                case "Pawns3":
                    return location_to_indexForPawns(location)[1] == 0;
                default:
                    break;
            }

            return false;
        }

        private void PawnImg_Click(object sender, EventArgs e)
        {
            //disable the feature to choose the mode (2 players or 4 players)
            playerToolStripMenuItem1.Enabled = false;
            playerToolStripMenuItem.Enabled = false;

            //if it isn't his turn, then don't allow him to click pawn. if this pawn won, then don't allow to click the pawn
            if (int.Parse(((PawnPictureBox)sender).Name.Substring(5)) != turn || pawnWin[turn])
                return;

            currPawn = (PawnPictureBox)sender;

            int[] blockIndex = location_to_indexForPawns(new Point(currPawn.Location.X, currPawn.Location.Y));

            //borders
            Border[] borders =
            {
                grid.Blocks[blockIndex[0],blockIndex[1]].BorderRight,
                grid.Blocks[blockIndex[0],blockIndex[1]].BorderLeft,
                grid.Blocks[blockIndex[0],blockIndex[1]].BorderBot,
                grid.Blocks[blockIndex[0],blockIndex[1]].BorderTop,
            };


            //generate "grey pawn" (dirs) 
            for (int i = 0; i < directions.Length; i++)
            {
                directions[i].Visible = false;

                Point loc = new Point(currPawn.Location.X + dirs[i].Item1, currPawn.Location.Y + dirs[i].Item2);
                int[] indexes = location_to_indexForPawns(loc);
                if (!outside_bound(indexes[0],grid.Blocks.GetLength(0)) && !outside_bound(indexes[1], grid.Blocks.GetLength(1)) && borders[i].isEmpty)
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

                            index++;

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
            
            //check if someone win
            if (win(currPawn, currPawn.Location))
            {
                pawnWin[turn] = true;

                switch (currPawn.Name)
                {
                    case "Pawns0":
                        MessageBox.Show("Green Pawn Won!");
                        break;
                    case "Pawns1":
                        MessageBox.Show("Red Pawn Won!");
                        break;
                    case "Pawns2":
                        MessageBox.Show("Blue Pawn Won!");
                        break;
                    case "Pawns3":
                        MessageBox.Show("Yellow Pawn Won!");
                        break;
                    default:
                        break;
                }
            }

            hide_directions();

            nextTurn();

            //update image
            pawnTurn.Image = Pawns[turn].Image;
            //update walls left
            wallsLeftLbl.Text = $"({wallsLeft[turn]} wall left)";
        }

        private void hide_directions()
        {
            for (int i = 0; i < directions.Length; i++)
                directions[i].Visible = false;
        }

        private void nextTurn()
        {
            turn = (turn + 1) % Pawns.Length;

            //if pawn[turn] is the only pawn that didn't win that it lost
            int n_pawnLoss = 0;
            foreach (bool x in pawnWin)
                n_pawnLoss += + (x ? 0 : 1);

            //stop if there isn't any pawns
            if (n_pawnLoss == 0)
                return;

            if (n_pawnLoss == 1 && !pawnWin[turn])
            {
                pawnWin[turn] = true;
                switch (Pawns[turn].Name)
                {
                    case "Pawns0":
                        MessageBox.Show("Green Pawn Lost");
                        break;
                    case "Pawns1":
                        MessageBox.Show("Red Pawn Lost");
                        break;
                    case "Pawns2":
                        MessageBox.Show("Blue Pawn Lost");
                        break;
                    case "Pawns3":
                        MessageBox.Show("Yellow Pawn Lost");
                        break;
                    default:
                        break;
                }
                return;
            }
            
            if (pawnWin[turn])
                nextTurn();
        }

        private void grid_Click(object sender, EventArgs e)
        {
            hide_directions();
        }

        private int[] location_to_indexForBarrierMouse(Point location)
        {
            int[] res = new int[2];

            res[0] = (int)Math.Round((location.Y - 32) / 66.0) - 1;
            res[1] = (int)Math.Round((location.X - 75) / 68.0);

            return res;
        }
        private int[] location_to_indexForBarrierLine(Point location)
        {
            int[] res = new int[2];

            res[0] = (location.Y / 67);
            res[1] = (location.X / 66);

            return res;
        }

        private bool isWinPossible(PawnPictureBox curr, Point loc, ref bool[,] visited)
        {
            int[] indexes = location_to_indexForPawns(loc);
            if (outside_bound(indexes[0], 9) || outside_bound(indexes[1], 9) || visited[indexes[0], indexes[1]])
                return false;

            //MessageBox.Show(indexes[0] + "  " + indexes[1]);

            if (win(curr, loc))
                return true;

            //set curr "cell" as visited
            visited[indexes[0], indexes[1]] = true;

            Border[] borders = new Border[]
            {
                grid.Blocks[indexes[0],indexes[1]].BorderRight,
                grid.Blocks[indexes[0],indexes[1]].BorderLeft,
                grid.Blocks[indexes[0],indexes[1]].BorderBot,
                grid.Blocks[indexes[0],indexes[1]].BorderTop,
            };

            for (int i = 0; i < dirs.Length; i++)
                if (borders[i].isEmpty && isWinPossible(curr, new Point(loc.X + dirs[i].Item1, loc.Y + dirs[i].Item2),ref visited))
                    return true;

            return false;
        }

        private void Line_MouseDown(object sender, MouseEventArgs e)
        {
            //disable the feature to choose the mode (2 players or 4 players)
            playerToolStripMenuItem1.Enabled = false;
            playerToolStripMenuItem.Enabled = false;

            //hide all grey pans (if exists)
            hide_directions();

            //if the user click with the right button or if this pawn won, then don't allow to click the pawn
            if (e.Button == MouseButtons.Right || pawnWin[turn])
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
            if (!move)
                return;

            //location that the new line will be (in the form & in the grid/backend)
            Point lineLocation = new Point(((PictureBox)sender).Location.X, ((PictureBox)sender).Location.Y - 29);

            ((PictureBox)sender).Location = orginalLocation;
            move = false;

            Point MouseLocation = new Point(MousePosition.X - this.Location.X, MousePosition.Y - this.Location.Y - 29);
            int[] indexes = location_to_indexForBarrierMouse(MouseLocation);

            //if he stil has some walls left
            if (wallsLeft[turn] < 1)
            {
                MessageBox.Show("No walls left","Quoridor",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }

            //if it's not outside the bound and he has some walls left
            if (!outside_bound(indexes[0], barrier.GetLength(0)) && !outside_bound(indexes[1], barrier.GetLength(1)))
            {
                //generate the line in the grid (in the backend) if the line is valid
                int[] blockIndex = location_to_indexForBarrierLine(lineLocation);
              
                if (((PictureBox)sender).Name == "HorizontalLine")
                {
                    //check that there isn't another line in that block
                    if (grid.Blocks[blockIndex[0], blockIndex[1]].BorderBot.isEmpty == false ||
                        grid.Blocks[blockIndex[0] + 1, blockIndex[1]].BorderTop.isEmpty == false ||
                        grid.Blocks[blockIndex[0], blockIndex[1] + 1].BorderBot.isEmpty == false ||
                        grid.Blocks[blockIndex[0] + 1, blockIndex[1] + 1].BorderTop.isEmpty == false ||
                        (grid.Blocks[blockIndex[0], blockIndex[1]].BorderRight.isEmpty == false && grid.Blocks[blockIndex[0] + 1, blockIndex[1]].BorderRight.isEmpty == false))
                        return;

                    grid.Blocks[blockIndex[0], blockIndex[1]].BorderBot.isEmpty = false;
                    grid.Blocks[blockIndex[0] + 1, blockIndex[1]].BorderTop.isEmpty = false;

                    grid.Blocks[blockIndex[0], blockIndex[1] + 1].BorderBot.isEmpty = false;
                    grid.Blocks[blockIndex[0] + 1, blockIndex[1] + 1].BorderTop.isEmpty = false;

                    //make sure that it won't completely block the opponent
                    for (int i = 0; i < Pawns.Length; i++)
                    {
                        if (pawnWin[i])
                            continue;
                        bool[,] visited = new bool[9, 9];
                        if (!isWinPossible(Pawns[i], Pawns[i].Location, ref visited))
                        {
                            MessageBox.Show("You can't completely block the opponent", "Quoridor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //remove the line from grid and return
                            grid.Blocks[blockIndex[0], blockIndex[1]].BorderBot.isEmpty = true;
                            grid.Blocks[blockIndex[0] + 1, blockIndex[1]].BorderTop.isEmpty = true;

                            grid.Blocks[blockIndex[0], blockIndex[1] + 1].BorderBot.isEmpty = true;
                            grid.Blocks[blockIndex[0] + 1, blockIndex[1] + 1].BorderTop.isEmpty = true;
                            return;
                        }
                    }
                }
                else
                {
                    //check that there isn't another line in that block
                    if (grid.Blocks[blockIndex[0], blockIndex[1]].BorderRight.isEmpty == false ||
                        grid.Blocks[blockIndex[0], blockIndex[1] + 1].BorderLeft.isEmpty == false ||
                        grid.Blocks[blockIndex[0] + 1, blockIndex[1]].BorderRight.isEmpty == false ||
                        grid.Blocks[blockIndex[0] + 1, blockIndex[1] + 1].BorderLeft.isEmpty == false ||
                        (grid.Blocks[blockIndex[0], blockIndex[1]].BorderBot.isEmpty == false && grid.Blocks[blockIndex[0], blockIndex[1] + 1].BorderBot.isEmpty == false))
                        return;
                    
                    grid.Blocks[blockIndex[0], blockIndex[1]].BorderRight.isEmpty = false;
                    grid.Blocks[blockIndex[0], blockIndex[1] + 1].BorderLeft.isEmpty = false;

                    grid.Blocks[blockIndex[0] + 1, blockIndex[1]].BorderRight.isEmpty = false;
                    grid .Blocks[blockIndex[0] + 1, blockIndex[1] + 1].BorderLeft.isEmpty = false;

                    //make sure that it won't completely block the opponent
                    for (int i = 0; i < Pawns.Length; i++)
                    {
                        if (pawnWin[i])
                            continue;
                        bool[,] visited = new bool[9, 9];
                        if (!isWinPossible(Pawns[i], Pawns[i].Location, ref visited))
                        {
                            MessageBox.Show("You can't completely block the opponent", "Quoridor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //remove the line from grid and return
                            grid.Blocks[blockIndex[0], blockIndex[1]].BorderRight.isEmpty = true;
                            grid.Blocks[blockIndex[0], blockIndex[1] + 1].BorderLeft.isEmpty = true;

                            grid.Blocks[blockIndex[0] + 1, blockIndex[1]].BorderRight.isEmpty = true;
                            grid.Blocks[blockIndex[0] + 1, blockIndex[1] + 1].BorderLeft.isEmpty = true;
                            return;
                        }
                    }
                }

                //generate the line (in the form)
                PictureBox newLine = new PictureBox();
                newLine.BackColor = ((PictureBox)sender).BackColor;
                newLine.Location = lineLocation;
                newLine.Size = ((PictureBox)sender).Size;

                gridPanel.Controls.Add(newLine);
                newLine.BringToFront();

                //decrese walls left
                wallsLeft[turn]--;

                nextTurn();

                //update image
                pawnTurn.Image = Pawns[turn].Image;
                //update walls left
                wallsLeftLbl.Text = $"({wallsLeft[turn]} wall left)";
            }
        }

        private void Line_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                Point MouseLocation = new Point(MousePosition.X - this.Location.X, MousePosition.Y - this.Location.Y - 29);
                int[] indexes = location_to_indexForBarrierMouse(MouseLocation);

                if (outside_bound(indexes[0], barrier.GetLength(0)) || outside_bound(indexes[1], barrier.GetLength(1)))
                    curr.Location = orginalLocation;
                else
                    curr.Location = new Point(barrier[indexes[0], indexes[1]].X, barrier[indexes[0], indexes[1]].Y);
            }
        }

        private void playerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            playerToolStripMenuItem1.BackColor = SystemColors.ActiveCaption;
            playerToolStripMenuItem.BackColor = SystemColors.Control;

            four_people = true;

            InitGame();
        }

        private void playerToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            playerToolStripMenuItem.BackColor = SystemColors.ActiveCaption;
            playerToolStripMenuItem1.BackColor = SystemColors.Control;

            four_people = false;

            InitGame();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //enable the feature to choose the mode (2 players or 4 players)
            playerToolStripMenuItem1.Enabled = true;
            playerToolStripMenuItem.Enabled = true;

            InitGame();
        }
    }
}
