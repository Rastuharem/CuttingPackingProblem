using System.Windows.Forms;

namespace Cutter
{
    class PrintByListBox : IPrinter
    {
        private ListBox listBox;

        public PrintByListBox(ListBox listBox)
        {
            this.listBox = listBox;
        }

        public void Clear()
        {
            listBox.Items.Clear();
        }
        public void Print(string text)
        {
            listBox.Items.Add(text);
        }
    }
}