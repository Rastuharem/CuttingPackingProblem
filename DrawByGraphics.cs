using System.Drawing;

namespace Cutter
{
    class DrawByGraphics : IDrawByContext
    {
        private Graphics g;
        public static float scale = 1;
        static Font font = new Font("Courier new", 8);

        public DrawByGraphics(Graphics g)
        {
            this.g = g;
        }

        public void Print(IVisualItem item)
        {
            if (!item.IsRotated())
            {
                g.DrawRectangle(Pens.Black, scale * item.GetX(), scale * item.GetY(), scale * item.GetWidth(), scale * item.GetHeight());
                g.DrawString(item.GetID(), font, Brushes.Black, scale * item.GetX() + 5, scale * item.GetY() + 5);
            }
            else
            {
                g.DrawRectangle(Pens.Black, scale * item.GetX(), scale * item.GetY(), scale * item.GetHeight(), scale * item.GetWidth());
                g.DrawString(item.GetID(), font, Brushes.Black, scale * item.GetX() + 5, scale * item.GetY() + 5);
            }
        }
    }
}
