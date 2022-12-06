using System.Drawing;

namespace Cutter
{
    class Detail : IItem
    {
        public string ID;
        public int Width;
        public int Height;
        public bool Rotated;

        public Detail(string ID, int height, int width, bool rotated = false)
        {
            this.ID = ID;
            Height = height;
            Width = width;
            Rotated = rotated;
        }

        public string GetID() { return ID; }
        public int GetWidth() { return Width; }
        public int GetHeight() { return Height; }
        public bool IsRotated() { return Rotated; }

        public override string ToString()
        {
            return ID + " " + Width.ToString() + " * " + Height.ToString() + "; It's square = " + (Width * Height);
        }

        public int GetSquare()
        {
            return Height * Width;
        }
    }
}