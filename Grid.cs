using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor
{
    class Grid
    {
        private int _size;
        Block[,] _blocks;
        public Grid()
        {
            _size = 9;
            _blocks = new Block[_size, _size];
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                    _blocks[i, j] = new Block();
        }

        public int Size
        {
            get { return _size; }
        }

        public Block[,] Blocks
        {
            get { return _blocks; }
            set { _blocks = value; }
        }
    }

    class Block
    {
        private Border _borderTop;
        private Border _borderBot;
        private Border _borderLeft;
        private Border _borderRight;

        private bool _isEmpty;
        public Block()
        {
            _isEmpty = true;
            _borderTop = new Border();
            _borderBot = new Border();
            _borderLeft = new Border();
            _borderRight = new Border();
        }

       public Border BorderTop
        {
            get { return _borderTop; }
            set { _borderTop = value; }
        }

        public Border BorderBot
        {
            get { return _borderBot; }
            set { _borderBot = value; }
        }

        public Border BorderLeft
        {
            get { return _borderLeft; }
            set { _borderLeft = value; }
        }

        public Border BorderRight
        {
            get { return _borderRight; }
            set { _borderRight = value; }
        }

        public bool isEmpty
        {
            get { return _isEmpty; }
            set { _isEmpty = value; }
        }
    }

    class Border
    {
        private bool _isEmpty = true;

        public bool isEmpty
        {
            get { return _isEmpty; }
            set { _isEmpty = value; }
        }
    }
}
