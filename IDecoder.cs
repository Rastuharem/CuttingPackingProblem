using System.Collections.Generic;

namespace Cutter
{
    interface IDecoder
    {
        int CountCriterium(List<IItem> items);
        IVisualItem GetVisualCriterium();
        List<IVisualItem> GetVisualItemsList();
    }
}
