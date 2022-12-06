namespace Cutter
{
    class VisualItem : IVisualItem
    {
        private IItem Item;
        private int xCoord, yCoord;

        public VisualItem(IItem item, int xCoord = 0, int yCoord = 0)
        {
            Item = item;
            this.xCoord = xCoord;
            this.yCoord = yCoord;
        }

        public int GetHeight() { return Item.GetHeight(); }
        public string GetID() { return Item.GetID(); }
        public int GetWidth() { return Item.GetWidth(); }
        public bool IsRotated() { return Item.IsRotated(); }

        public int GetX() { return xCoord; }
        public int GetY() { return yCoord; }
        public void SetX(int xCoord) { this.xCoord = xCoord; }
        public void SetY(int yCoord) { this.yCoord = yCoord; }

        public void Print(IDrawByContext context) { context.Print(this); }

        public int GetSquare() { return Item.GetSquare(); }
        public override string ToString()
        {
            return Item.GetID() + " " + Item.GetWidth() + " * " + Item.GetHeight() + "; Its square = " + Item.GetSquare() +
                "; Its coords: x = " + xCoord + ", y = " + yCoord;
        }
    }
}
