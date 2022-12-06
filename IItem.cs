namespace Cutter
{
    interface IItem
    {
        string GetID();
        int GetWidth();
        int GetHeight();
        bool IsRotated();
        int GetSquare();
    }
}