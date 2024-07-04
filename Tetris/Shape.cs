using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Shape
    {
        public int Width;
        public int Height;
        public int[,] Dots;
        public int[,] backupDots;
        public Color Color;

        public Shape(Color color)
        {
            Color = color;
        }

        public void turn()
        {
            backupDots = Dots;
            Dots = new int[Width, Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Dots[i, j] = backupDots[Height - 1 - j, i];
                }
            }

            var temp = Width;
            Width = Height;
            Height = temp;
        }

        public void rollback()
        {
            Dots = backupDots;
            var temp = Width;
            Width = Height;
            Height = temp;
        }
    }
}
