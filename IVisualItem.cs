namespace Cutter
{
    interface IVisualItem : IItem
    {
        int GetX();
        int GetY();
        void SetX(int xCoord);
        void SetY(int yCoord);

        void Print(IDrawByContext context);
    }
}
