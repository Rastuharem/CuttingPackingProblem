using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cutter
{
    class Detail
    {
        public static float scale = 1;
        static Font font = new Font("Courier new", 8);
        public string id;
        public int width;
        public int height;
        public int x;
        public int y;
        public bool rotated;

        public void Paint(Graphics g)
        {
            if (!rotated)
            {
                g.DrawRectangle(Pens.Black, scale * x, scale * y, scale * width, scale * height);
                g.DrawString(id, font, Brushes.Black, scale * x + 5, scale * y + 5);
            }
            else
            {
                g.DrawRectangle(Pens.Black, scale * x, scale * y, scale * height, scale * width);
                g.DrawString(id, font, Brushes.Black, scale * x + 5, scale * y + 5);
            }
        }

        public override string ToString()
        {
            return id + "\t" + width.ToString() + "*" + height.ToString() + "\t" +
                x.ToString() + "*" + y.ToString();
        }

        public bool PlaceTo(bool[,] Map)
        {
            if (rotated)
            {
                int temp = width; width = height; height = temp;
            }
            //Проверить, возможно ли разместить деталь на "карте"
            bool CanDoIt = true;

            for (int r = 0; r < height; r++)
            {
                if (r + y >= Map.GetLength(0))
                    return false;
                for (int c = 0; c < width; c++)
                {
                    if (c + x >= Map.GetLength(1)) return false;
                    if (Map[r + y, c + x])
                    {
                        CanDoIt = false;
                        break;
                    }
                }
                if (!CanDoIt) break;
            }

            //Если это возможно - пометить как занятое
            if (CanDoIt)
            {
                for (int r = 0; r < height; r++)
                    for (int c = 0; c < width; c++)
                        Map[r + y, c + x] = true;
            }

            if (rotated)
            {
                int temp = width; width = height; height = temp;
            }

            return CanDoIt;
        }

        public void TakeFrom(bool[,] Map)
        {
            //"Забрать" с карты
            if (rotated)
            {
                int temp = width; width = height; height = temp;
            }

            for (int r = 0; r < height; r++)
                for (int c = 0; c < width; c++)
                    Map[r + y, c + x] = false;

            if (rotated)
            {
                int temp = width; width = height; height = temp;
            }


        }

    }
}
