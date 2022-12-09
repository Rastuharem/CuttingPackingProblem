namespace Cutter
{
    interface ICrossover
    {
        void MakeCrossover(Codestring firstParent, Codestring secondParent, out Codestring child);
    }
}