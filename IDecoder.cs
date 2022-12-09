using System.Collections.Generic;

namespace Cutter
{
    interface IDecoder
    {
        int CountCriterium(List<IItem> items);
        IVisualItem GetVisualCriterium(List<IItem> items);
        List<IVisualItem> GetVisualItemsList(List<IItem> items);

        void SetFWidth(int fwidth);
        void SetFHeight(int fheight);
        int GetFWidth();
        int GetFHeight();
    }
}
