using System.Collections.Generic;

namespace Cutter
{
    class Codestring
    {
        public List<int> CodeStr { get; }
        public List<IItem> Sample { get; }
        public List<IItem> CurItems { get; }
        public IDecoder Decoder { get; set; }
        public int Criterium { get; }

        public Codestring(List<IItem> items, List<IItem> sample, IDecoder decoder)
        {
            Decoder = decoder;
            CurItems = items;
            Sample = sample;
            CodeStr = Code(items, sample);
            Criterium = decoder.CountCriterium(items);

        }
        public Codestring(List<int> codestring, List<IItem> sample, IDecoder decoder)
        {
            Decoder = decoder;
            CodeStr = codestring;
            Sample = sample;
            CurItems = Decode(codestring, sample);
            Criterium = decoder.CountCriterium(CurItems);
        }

        public List<IItem> Decode(List<int> codestring, List<IItem> sample)
        {
            List<IItem> details = new List<IItem>();
            for (int i = 0; i < codestring.Count; i++)
            {
                details.Add(sample[codestring[i]]);
            }
            return details;
        }
        public List<int> Code(List<IItem> items, List<IItem> sample)
        {
            List<int> CodeStr = new List<int>();
            for (int i = 0; i < sample.Count; i++)
            {
                for (int j = 0; j < items.Count; j++)
                {
                    if (items[j] == sample[i])
                    {
                        CodeStr.Add(i);
                    }
                }
            }

            return CodeStr;
        }
    }
}