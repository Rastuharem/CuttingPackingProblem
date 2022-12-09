using System.Collections.Generic;

namespace Cutter
{
    class FiftyFiftyCrossover : ICrossover
    {
        public void MakeCrossover(Codestring firstParent, Codestring secondParent, out Codestring child)
        {
            List<int> childCode = new List<int>(firstParent.CodeStr);
            int size = childCode.Count;
            int divider = 0;
            if (size % 2 == 1)
            {
                divider = (size + 1) / 2; 
            }
            else
            {
                divider = size / 2;
            }
            for (int i = divider; i < childCode.Count; i++)
            {
                childCode[i] = secondParent.CodeStr[i];
            }
            child = new Codestring(childCode, firstParent.Sample, firstParent.Decoder);
        }

        public override string ToString()
        {
            return "Crossover which takes half of genotype from one parent, half from another";
        }
    }
}