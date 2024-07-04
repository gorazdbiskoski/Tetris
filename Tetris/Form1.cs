using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();
        Shape currentShape;
        Bitmap canvasBitmap;
        Graphics canvasGraphics;
        Bitmap workingBitmap;
        Graphics workingGraphics;
        int canvasWidth = 23;
        int canvasHeight = 31;
        int[,] canvasDots;
        int dotSize = 20;
        int spawnX;
        int spawnY;
        Color background;
        int score;
        Shape nextShape;

        public Form1()
        {
            InitializeComponent();
            currentShape = getRandomShape();
            timer.Tick += Timer_Tick;
            timer.Interval = 300;
            timer.Start();
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            background = Color.LightGray;
            loadCanvas();
            nextShape = getNextShape();
        }


        private void loadCanvas()
        {
            pictureBox1.Width = canvasWidth * dotSize;
            pictureBox1.Height = canvasHeight * dotSize;
            canvasBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            canvasGraphics = Graphics.FromImage(canvasBitmap);
            canvasGraphics.FillRectangle(Brushes.LightGray, 0, 0, canvasBitmap.Width, canvasBitmap.Height);
            pictureBox1.Image = canvasBitmap;
            canvasDots = new int[canvasWidth, canvasHeight];
        }

        private Shape getRandomShape()
        {
            var shape = ShapesHandler.GetRandomShape();
            spawnX = 10;
            spawnY = -shape.Height;
            return shape;
        }

        private bool moveShape(int Down = 0, int Side = 0)
        {
            var newX = spawnX + Side;
            var newY = spawnY + Down;
            if (newX < 0 || newX + currentShape.Width > canvasWidth || newY + currentShape.Height > canvasHeight)
            {
                return false;
            }
            for (int i = 0; i < currentShape.Width; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (newY + j > 0 && canvasDots[newX + i, newY + j] == 1 && currentShape.Dots[j, i] == 1)
                    {
                        return false;
                    }
                }
            }
            spawnX = newX;
            spawnY = newY;
            drawShape();
            return true;
        }

        private void drawShape()
        {
            workingBitmap = new Bitmap(canvasBitmap);
            workingGraphics = Graphics.FromImage(workingBitmap);

            for (int i = 0; i < currentShape.Width; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (currentShape.Dots[j, i] == 1)
                    {
                        workingGraphics.FillRectangle(new SolidBrush(currentShape.Color), (spawnX + i) * dotSize, (spawnY + j) * dotSize, dotSize, dotSize);
                    }
                }
            }
            pictureBox1.Image = workingBitmap;
        }


        private void updateCanvas()
        {
            for (int i = 0; i < currentShape.Width; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (currentShape.Dots[j, i] == 1)
                    {
                        checkIfGameOver();
                        canvasDots[spawnX + i, spawnY + j] = 1;
                    }
                }
            }
        }

        private void checkIfGameOver()
        {
            if (spawnY < 0)
            {
                timer.Stop();
                MessageBox.Show("Game Over");
                Application.Exit();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            var isMoveSuccess = moveShape(Down: 1);
            if (!isMoveSuccess)
            {
                canvasBitmap = new Bitmap(workingBitmap);
                updateCanvas();
                currentShape = getRandomShape();
                currentShape = nextShape;
                nextShape = getNextShape();
                UpdateScore();

            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            var verticalMove = 0;
            var horizontalMove = 0;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    verticalMove--;
                    break;

                case Keys.Right:
                    verticalMove++;
                    break;

                case Keys.Down:
                    horizontalMove++;
                    break;

                case Keys.Up:
                    currentShape.turn();
                    break;

                default:
                    return;
            }

            var isMoveSuccess = moveShape(horizontalMove, verticalMove);
            if (!isMoveSuccess && e.KeyCode == Keys.Up)
            {
                currentShape.rollback();
            }
        }

        public void UpdateScore()
        {

            for (int i = 0; i < canvasHeight; i++)
            {
                int j;
                for (j = canvasWidth - 1; j >= 0; j--)
                {
                    if (canvasDots[j, i] == 0)
                        break;
                }

                if (j == -1)
                {
                    score++;
                    lbl1.Text = "Score: " + score;
                    lbl2.Text = "Level: " + score / 10;

                    timer.Interval -= 10;

                    for (j = 0; j < canvasWidth; j++)
                    {
                        for (int k = i; k > 0; k--)
                        {
                            canvasDots[j, k] = canvasDots[j, k - 1];
                        }

                        canvasDots[j, 0] = 0;
                    }
                }
            }


            for (int i = 0; i < canvasWidth; i++)
            {
                for (int j = 0; j < canvasHeight; j++)
                {
                    canvasGraphics = Graphics.FromImage(canvasBitmap);
                    if (canvasDots[i, j] == 0)
                    {
                        canvasGraphics.FillRectangle(Brushes.LightGray, i * dotSize, j * dotSize, dotSize, dotSize);
                    }
                }
            }

            pictureBox1.Image = canvasBitmap;
        }
        Bitmap nextShapeBitmap;
        Graphics nextShapeGraphics;

        private Shape getNextShape()
        {
            var shape = getRandomShape();
            nextShapeBitmap = new Bitmap(6 * dotSize, 6 * dotSize);
            nextShapeGraphics = Graphics.FromImage(nextShapeBitmap);

            nextShapeGraphics.FillRectangle(Brushes.LightGray, 0, 0, nextShapeBitmap.Width, nextShapeBitmap.Height);

            var startX = (6 - shape.Width) / 2;
            var startY = (6 - shape.Height) / 2;

            for (int i = 0; i < shape.Height; i++)
            {
                for (int j = 0; j < shape.Width; j++)
                {
                    nextShapeGraphics.FillRectangle(
                        shape.Dots[i, j] == 1 ? Brushes.Black : Brushes.LightGray,
                        (startX + j) * dotSize, (startY + i) * dotSize, dotSize, dotSize);
                }
            }

            pictureBox2.Size = nextShapeBitmap.Size;
            pictureBox2.Image = nextShapeBitmap;

            return shape;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}