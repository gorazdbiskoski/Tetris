using System;
using System.Drawing;

namespace Tetris
{
    static class ShapesHandler
    {
            private static Shape[] shapesArray;
            static ShapesHandler()
            {
                shapesArray = new Shape[]
                    {
                    new Shape(Color.Yellow) {
                        Width = 2,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 1, 1 },
                            { 1, 1 }
                        }
                    },
                    new Shape(Color.Cyan) {
                        Width = 1,
                        Height = 4,
                        Dots = new int[,]
                        {
                            { 1 },
                            { 1 },
                            { 1 },
                            { 1 }
                        }
                    },
                    new Shape(Color.Purple) {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 0, 1, 0 },
                            { 1, 1, 1 }
                        }
                    },
                    new Shape(Color.Orange) {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 0, 0, 1 },
                            { 1, 1, 1 }
                        }
                    },
                    new Shape(Color.Blue) {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 1, 0, 0 },
                            { 1, 1, 1 }
                        }
                    },
                    new Shape(Color.Red) {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 1, 1, 0 },
                            { 0, 1, 1 }
                        }
                    },
                    new Shape(Color.Green) {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 0, 1, 1 },
                            { 1, 1, 0 }
                        }
                    }
                    };
            }

            public static Shape GetRandomShape()
            {
                var shape = shapesArray[new Random().Next(shapesArray.Length)];
                return shape;
            }       
    }
}