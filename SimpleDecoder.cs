using System.Collections.Generic;

namespace Cutter
{
    // Class: represents simple decoder, which places Details by finding first free spot
    //
    class SimpleDecoder : ADecoder
    {
        private List<IVisualItem> visualItems = new List<IVisualItem>();
        private IVisualItem visualCriterium = null;

        // Singleton pattern
        //
        private SimpleDecoder(int fHeight, int fWidth) : base(fHeight, fWidth) { }
        public static IDecoder GetInstance(int fHeight, int fWidth)
        {
            if (instance == null) instance = new SimpleDecoder(fHeight, fWidth);
            return instance;
        }
        
        // Returns int, which counts as most possible free square in task list
        // Also creates VisualItem VisualCriterium and List of VisualItem VisualItems for their Get methods
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
            int bestw = 0; int besth = 0; int bestx = 0; int besty = 0;
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
            visualCriterium = new VisualItem(new Detail("Criterium", besth, bestw), besty, bestx);
            return bestw * besth;
        }

        // Returns VisualItem of abstract Detail, which counts by most possible free square in task list
        //!!! Needs to call CountCriterium method first, or it'll return empty VisualItem
        //
        public override IVisualItem GetVisualCriterium(List<IItem> items)
        {
            CountCriterium(items);
            return visualCriterium;
        }
        // Returns List of VisualItem placed on a task list
        //!!! Needs to call CountCriterium method first, or it'll return empty List
        //
        public override List<IVisualItem> GetVisualItemsList(List<IItem> items)
        {
            CountCriterium(items);
            return visualItems;
        }

        private bool IsFree(int y, int x, int height, int width)
        {
            if (!Map[y, x]) return false;

            for (int i = y; i < height + y; i++)
                for (int j = x; j < width + x; j++)
                    if (!Map[i, j]) return false;
            return true;
        }

        public override string ToString()
        {
            return "Simple decoder, which places details line by line";
        }
    }
}