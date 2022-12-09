using System.Collections.Generic;

namespace Cutter
{
    // Class: represents codestrings to algorithms to work with
    //
    class Codestring
    {
        public List<int> CodeStr { get; } // Current codestring for CurItems
        public List<IItem> Sample { get; } // Sample for coding codestrings (input list of details)
        public List<IItem> CurItems { get; } // List of Detail equals to it's codestring CodeStr
        public IDecoder Decoder { get; set; } // Decoder for Criterium
        public int Criterium { get; } // Criterium: biggest square of free rectangle space

        // Builds CodeStr for List 'items' by 'sample' and counts it's Criterium with 'decoder'
        //
        public Codestring(List<IItem> items, List<IItem> sample, IDecoder decoder)
        {
            Decoder = decoder;
            CurItems = items;
            Sample = sample;
            CodeStr = Code(items, sample);
            Criterium = decoder.CountCriterium(items);
        }
        // Builds CurItems by 'codestring' by 'sample' and counts it's Criterium with 'decoder'
        //
        public Codestring(List<int> codestring, List<IItem> sample, IDecoder decoder)
        {
            Decoder = decoder;
            CodeStr = codestring;
            Sample = sample;
            CurItems = Decode(codestring, sample);
            Criterium = decoder.CountCriterium(CurItems);
        }

        private List<IItem> Decode(List<int> codestring, List<IItem> sample)
        {
            List<IItem> details = new List<IItem>();
            for (int i = 0; i < codestring.Count; i++)
            {
                details.Add(sample[codestring[i]]);
            }
            return details;
        }
        private List<int> Code(List<IItem> items, List<IItem> sample)
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

        public List<IVisualItem> GetVisualItemsList()
        {
            return this.Decoder.GetVisualItemsList(CurItems);
        }
        public IVisualItem GetVisualCriterium()
        {
            return this.Decoder.GetVisualCriterium(CurItems);
        }

        public static bool operator ==(Codestring c1, Codestring c2)
        {
            if (c1.CodeStr.Count != c2.CodeStr.Count) return false;
            for (int i = 0; i < c1.CodeStr.Count; i++)
            {
                if (c1.CodeStr[i] != c2.CodeStr[i])
                {
                    return false;
                }
            }
            return true;
        }
        public static bool operator !=(Codestring c1, Codestring c2)
        {
            if (c1.CodeStr.Count != c2.CodeStr.Count) return true;
            for (int i = 0; i < c1.CodeStr.Count; i++)
            {
                if (c1.CodeStr[i] != c2.CodeStr[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}