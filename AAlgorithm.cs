using System.Collections.Generic;

namespace Cutter
{
    // Abstract class: represents all Algorithms
    //
    abstract class AAlgorithm : IAlgorithm
    {
        protected List<IVisualItem> BestSolution = new List<IVisualItem>();
        protected IVisualItem BestCriterium = null;

        protected List<IItem> Sample;

        public IDecoder Decoder { get; set; }
        public IPrinter Printer { get; set; }
        public System.Diagnostics.Stopwatch myStopWatch { get; }

        public AAlgorithm(List<IItem> sample, IDecoder decoder, IPrinter printer = null)
        {
            Decoder = decoder;
            Sample = new List<IItem>(sample);
            myStopWatch = new System.Diagnostics.Stopwatch();
            if (printer == null)
                printer = new PrintByNothing();
            else
                Printer = printer;
        }

        public void GetSolution(out List<IVisualItem> solution, out IVisualItem criterium)
        {
            Solve();
            solution = new List<IVisualItem>(BestSolution);
            if (BestCriterium == null)
                criterium = new VisualItem(new Detail("Error", 0, 0));
            else
                criterium = BestCriterium;
        }

        abstract public void Solve();

        protected void StartTimeCount() { myStopWatch.Start(); }
        protected void StopTimeCount() { myStopWatch.Stop(); }
    }
}
