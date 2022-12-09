using System;
using System.Collections.Generic;

namespace Cutter
{
    class EnumAlgorithm : AAlgorithm
    {
        public EnumAlgorithm(List<IItem> sample, IDecoder decoder, IPrinter printer = null) : base(sample, decoder, printer) { }
        public override void Solve()
        {
            Printer.Clear();
            if (Sample.Count > 9)
            {
                Printer.Print("Algorithm can't be used, because PC can't store 10! transpositions");
                return;
            }

            Printer.Print("Algorithm will use:");
            Printer.Print("     Decoder - " + Decoder.ToString());

            StartTimeCount();

            Printer.Print("Creating all Codestrings...");
            Printer.Print("");

            var AllCodes = ShowAllCombinations<int>(new Codestring(Sample, Sample, Decoder).CodeStr);
            var Floor = new List<Codestring>();
            for (int i = 0; i < AllCodes.Count; i++)
                Floor.Add(new Codestring(TranslateToListIntFromString(AllCodes[i]), Sample, Decoder));
            var answ = Floor[0];
            for (int i = 1; i < Floor.Count; i++)
                if (Floor[i].Criterium > answ.Criterium)
                    answ = Floor[i];

            BestSolution = answ.GetVisualItemsList();
            BestCriterium = answ.GetVisualCriterium();

            StopTimeCount();

            Printer.Print("Algorithm done in " + myStopWatch.Elapsed.ToString());
            Printer.Print("Solution is: ");
            Printer.Print(BestCriterium.ToString());
        }

        private List<string> ShowAllCombinations<T>(IList<T> arr, List<string> list = null, string current = "")
        {
            if (list == null) list = new List<string>();
            if (arr.Count == 0)
            {
                list.Add(current);
                return list;
            }
            for (int i = 0; i < arr.Count; i++)
            {
                List<T> lst = new List<T>(arr);
                lst.RemoveAt(i);
                ShowAllCombinations(lst, list, current + arr[i].ToString() + '.');
            }
            return list;
        }
        private List<int> TranslateToListIntFromString(string str)
        {
            List<int> CodeStr = new List<int>();
            string buf = "";
            for (int j = 0; j < str.Length; j++)
            {
                if (str[j] != '.')
                    buf += str[j];
                else
                {
                    CodeStr.Add(Convert.ToInt32(buf));
                    buf = "";
                }
            }
            return CodeStr;
        }
    }
}