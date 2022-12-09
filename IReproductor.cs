using System.Collections.Generic;

namespace Cutter
{
    interface IReproductor
    {
        void MakeReproduction(List<Codestring> population, out Codestring firstParent, out Codestring secondParent);
    }
}