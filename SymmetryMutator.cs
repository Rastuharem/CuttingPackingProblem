using System;
using System.Collections.Generic;

namespace Cutter
{
    class SymmetryMutator : IMutator
    {
        private double ProbabilityOfMutation;

        public SymmetryMutator(double probabilityOfMutations)
        {
            ProbabilityOfMutation = probabilityOfMutations;
        }

        public void MakeMutation(Codestring child, out Codestring mutatedChild)
        {
            Random rnd = new Random();
            List<int> childCode = new List<int>(child.CodeStr);

            for (int i = 0; i < child.CodeStr.Count; i++)
            {
                if (rnd.NextDouble() < ProbabilityOfMutation)
                {
                    int buf = childCode[i];
                    childCode[i] = childCode[childCode.Count - 1 - i];
                    childCode[childCode.Count - 1 - i] = buf;
                }
            }

            mutatedChild = new Codestring(childCode, child.Sample, child.Decoder);
        }

        public override string ToString()
        {
            return "Mutator replaces 2 positions in genotype";
        }
    }
}