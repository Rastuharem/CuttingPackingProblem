using System.Collections.Generic;

namespace Cutter
{
    // Abstract class: represent decoders, which decode Codestring and place Details in task list
    // Can exist only one copy (Singleton pattern)
    //
    abstract class ADecoder : IDecoder
    {
        protected static ADecoder instance = null;

        protected int fWidth;
        protected int fHeight;
        protected bool[,] Map;

        protected ADecoder(int fHeight, int fWidth)
        {
            Map = new bool[fHeight, fWidth];
            this.fHeight = fHeight;
            this.fWidth = fWidth;

            for (int i = 0; i < fHeight; i++)
                for (int j = 0; j < fWidth; j++)
                    Map[i, j] = true;
        }

        abstract public int CountCriterium(List<IItem> items);
        abstract protected bool PlaceTo(IItem item);
        abstract protected int FindMaxFreeRectangle(bool[,] Map);

        abstract public IVisualItem GetVisualCriterium();
        abstract public List<IVisualItem> GetVisualItemsList();
    }
}