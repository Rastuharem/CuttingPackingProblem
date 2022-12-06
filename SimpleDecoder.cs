using System.Collections.Generic;

namespace Cutter
{
    class SimpleDecoder : ADecoder
    {
        private List<IVisualItem> visualItems = new List<IVisualItem>();
        private IVisualItem visualCriterium = null;

        private SimpleDecoder(int fHeight, int fWidth) : base(fHeight, fWidth) { }
        public static IDecoder GetInstance(int fHeight, int fWidth)
        {
            if (instance == null) instance = new SimpleDecoder(fHeight, fWidth);
            return instance;
        }

        public override int CountCriterium(List<IItem> items)
        {
            for (int i = 0; i < fHeight; i++)
                for (int j = 0; j < fWidth; j++)
                    Map[i, j] = true;
            visualItems.Clear();
            int criterium = 0;

            for (int itemInd = 0; itemInd < items.Count; itemInd++)
            {
                bool success = PlaceTo(items[itemInd]);
                if (!success) return -1;
            }
            criterium = FindMaxFreeRectangle(Map);
            return criterium;
        }

        protected override bool PlaceTo(IItem item)
        {
            int xCoord = 0;
            int yCoord = 0;

            for (int rows = 0; rows < fHeight; rows++)
            {
                for (int cols = 0; cols < fWidth; cols++)
                {
                    if ((rows + item.GetHeight() < fHeight) && (cols + item.GetWidth() < fWidth) && IsFree(rows, cols, item.GetHeight(), item.GetWidth()))
                    {
                        yCoord = rows; xCoord = cols;
                        for (int placeRows = rows; placeRows < item.GetHeight() + rows; placeRows++)
                        {
                            for (int placeCols = cols; placeCols < item.GetWidth() + cols; placeCols++)
                            {
                                Map[placeRows, placeCols] = false;
                            }
                        }
                        visualItems.Add(new VisualItem(item, xCoord, yCoord));
                        return true;
                    }
                }
            }
            return false;
        }
        protected override int FindMaxFreeRectangle(bool[,] Map)
        {
            int bestw = 0; int besth = 0; int bestx = 0; int besty = 0; //Лучшие ширина и высота
            //перебрав все возможные позиции левого верхнего угла
            for (int rows = 0; rows < fHeight; rows++)
                for (int cols = 0; cols < fWidth; cols++)
                    if (Map[rows, cols])
                    {
                        int freeRows = rows, freeCols = cols;
                        int width = 1, height = 1;
                        while (Map[freeRows, cols] && freeRows < fHeight - 1)
                        {
                            height++;
                            freeRows++;
                        }
                        while (Map[rows, freeCols] && freeCols < fWidth - 1)
                        {
                            width++;
                            freeCols++;
                        }
                        if (width * height > bestw * besth)
                        {
                            besth = height;
                            bestw = width;
                            bestx = rows;
                            besty = cols;
                        }
                    }
            //результат вернуть
            visualCriterium = new VisualItem(new Detail("Criterium", besth, bestw), besty, bestx);
            return bestw * besth;
        }

        public override IVisualItem GetVisualCriterium()
        {
            if (visualCriterium == null)
            {
                return new VisualItem(new Detail("Критерий", 0, 0));
            }
            return visualCriterium;
        }
        public override List<IVisualItem> GetVisualItemsList() { return visualItems; }

        private bool IsFree(int y, int x, int height, int width)
        {
            if (!Map[y, x]) return false;

            for (int i = y; i < height + y; i++)
                for (int j = x; j < width + x; j++)
                    if (!Map[i, j]) return false;
            return true;
        }
    }
}