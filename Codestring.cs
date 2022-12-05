using System.Collections.Generic;

namespace Cutter
{
    class Codestring
    {
        public List<int> CodeStr { get; }
        public List<Detail> Sample { get; }
        public List<Detail> CurDetails { get; }
        public int Criterium { get; }

        public Codestring(List<Detail> details, List<Detail> sample)
        {
            CurDetails = details;
            Sample = sample;
            CodeStr = CodeCodestring(CurDetails, Sample);
            Criterium = CountCriterium();
        }
        public Codestring(List<int> codestring, List<Detail> sample)
        {
            CodeStr = codestring;
            Sample = sample;
            CurDetails = Uncode(codestring, sample);
            Criterium = CountCriterium();
        }

        private List<int> CodeCodestring(List<Detail> details ,List<Detail> sample)
        {
            List<int> CodeStr = new List<int>();
            for (int i = 0; i < sample.Count; i++)
            {
                for (int j = 0; j < details.Count; j++)
                {
                    if (details[j] == sample[i])
                    {
                        CodeStr.Add(i);
                    }
                }
            }

            return CodeStr;
        }

        private List<Detail> Uncode(List<int> codestring, List<Detail> sample)
        {
            List<Detail> details = new List<Detail>();
            for (int i = 0; i < codestring.Count; i++)
            {
                details.Add(sample[codestring[i]]);
            }
            return details;
        }

        private int CountCriterium()
        {
            return - 1;
        }
    }
}
