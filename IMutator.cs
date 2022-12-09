namespace Cutter
{
    interface IMutator
    {
        void MakeMutation(Codestring child, out Codestring mutatedChild);
    }
}