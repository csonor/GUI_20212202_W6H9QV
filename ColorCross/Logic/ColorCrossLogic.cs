using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using static ColorCross.Logic.LineOfColors;
using Color = System.Windows.Media.Color;

namespace ColorCross.Logic
{
    interface IColorCrossLogic
    {
        List<Color> Colors { get; }
        LineOfColors[] Columns { get; }
        LineOfColors[] Rows { get; }

        void ImageReader(string fileName);
        public List<List<CellData>> GetCurrentStatuses();
        public void Click(int x, int y, int color);
    }

    class ColorCrossLogic : IColorCrossLogic
    {
        //Color[,] pixels;
        int[,] pixels;
        List<List<CellData>> status;
        List<Color> colors;
        LineOfColors[] rows;
        LineOfColors[] columns;
        int numberOfColoredLinesAndColumns;
        int currentCorrectLines;
        Bitmap bmp;

        public LineOfColors[] Rows { get => rows; }
        public LineOfColors[] Columns { get => columns; }
        public List<Color> Colors { get => colors; }

        public ColorCrossLogic()
        {

            //pixels = new Color[0, 0];
            pixels = new int[0, 0];
            status = new List<List<CellData>>();
            colors = new List<Color>();
            rows = new LineOfColors[0];
            columns = new LineOfColors[0];
            numberOfColoredLinesAndColumns = 0;
            currentCorrectLines = 0;
            bmp = new Bitmap(1, 1);
        }

        public void ImageReader(string fileName)
        {
            bmp = new Bitmap(fileName);
            rows = new LineOfColors[bmp.Height];
            columns = new LineOfColors[bmp.Width];
            pixels = new int[bmp.Height, bmp.Width];


            for (int i = 0; i < bmp.Height; i++)
            {
                List<CellData> rd = new List<CellData>();
                for (int j = 0; j < bmp.Width; j++)
                {
                    var color = bmp.GetPixel(j, i);
                    var newColor = Color.FromArgb(color.A, color.R, color.G, color.B);
                    if (!colors.Contains(newColor) && color.Name != "0")
                    {
                        colors.Add(newColor);
                    }
                    pixels[i, j] = colors.IndexOf(newColor);
                    CellData cd = new CellData() { X = i, Y = j, Color = pixels[i, j] };
                    rd.Add(cd);
                }
                status.Add(rd);
            }
            CountRowColors();
            CountColumnColors();
        }

        void CountRowColors()
        {
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    int k = j + 1;
                    int sum = 0;
                    var color = bmp.GetPixel(j, i);
                    var newColor = Color.FromArgb(color.A, color.R, color.G, color.B);
                    while (k < bmp.Width && bmp.GetPixel(k, i) == color)
                    {
                        sum++;
                        k++;
                    }
                    if (color.Name != "0")
                    {
                        if (rows[i].Colors == null)
                            rows[i].Colors = new List<ColorNumber>();
                        rows[i].Colors.Add(new ColorNumber { Color = newColor, Count = sum + 1 });
                    }
                    j = k - 1;
                }
                rows[i].IsDone = false;
            }
        }

        void CountColumnColors()
        {
            for (int i = 0; i < columns.Length; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int k = j + 1;
                    int sum = 0;
                    var color = bmp.GetPixel(i, j);
                    var newColor = Color.FromArgb(color.A, color.R, color.G, color.B);
                    while (k < bmp.Height && bmp.GetPixel(i, k) == color)
                    {
                        sum++;
                        k++;
                    }
                    if (color.Name != "0")
                    {
                        if (columns[i].Colors == null)
                            columns[i].Colors = new List<ColorNumber>();
                        columns[i].Colors.Add(new ColorNumber { Color = newColor, Count = sum + 1 });
                    }
                    j = k - 1;
                }
                columns[i].IsDone = false;
            }
        }
        public List<List<CellData>> GetCurrentStatuses()
        {
            return status;
        }

        public void Click(int x, int y, int color)
        {
            status[x][y].Color = color;
        }
    }
}
