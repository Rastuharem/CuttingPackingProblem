using System.Collections.Generic;

namespace Cutter
{
    interface ISelector
    {
        void MakeSelection(List<Codestring> curPopulation, out List<Codestring> nextPopulation);
    }
}