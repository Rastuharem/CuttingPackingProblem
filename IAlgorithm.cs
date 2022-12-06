using System.Collections.Generic;

namespace Cutter
{
    interface IAlgorithm
    {
        void GetSolution(out List<IVisualItem> solution, out IVisualItem criterium);
        void Solve();
    }
}