using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Cutter
{
    public partial class Form1 : Form
    {
        private int fWidth = 100;
        private int fHeight = 100;

        private Graphics g;

        private List<IItem> Full = new List<IItem>(); //Список имеющихся
        private List<IVisualItem> Best = new List<IVisualItem>(); //Лучший вариант
        private IVisualItem VisualCriterium;

        public Form1()
        {
            InitializeComponent();

            g = pictureBox.CreateGraphics();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            LoadDetails(openFileDialog1.FileName);
            ClearSolve();

            //g.DrawRectangle(Pens.Red, new Rectangle(0, 0, pictureBox.Width-1, pictureBox.Height-1));
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() != DialogResult.OK) return;
            SaveDetails(saveFileDialog2.FileName);
        }
        private void SaveSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() != DialogResult.OK) return;
            SaveSolution(saveFileDialog2.FileName);
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            dgv.Rows.Add();
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dgv.RowCount > 0)
                dgv.Rows.RemoveAt(dgv.RowCount - 1);
        }
        private void textBoxWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (Char.IsLetter(number))
            {
                e.Handled = true;
            }
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.Clear(Color.White);
            //Выбрать масштаб
            float scaleheight = 1.0f * pictureBox.Height / int.Parse(textBoxHeight.Text);
            float scalewidth = 1.0f * pictureBox.Width / int.Parse(textBoxWidth.Text);
            DrawByGraphics.scale = scaleheight < scalewidth ? scaleheight : scalewidth;
            e.Graphics.DrawRectangle(Pens.Red, new Rectangle(0, 0,
                Convert.ToInt32(fWidth * DrawByGraphics.scale) - 1, Convert.ToInt32(fHeight * DrawByGraphics.scale) - 1));
        }

        //Загрузить список деталей в dgv
        void LoadDetails(string FileName)
        {
            string[] f = File.ReadAllLines(FileName);
            dgv.Rows.Clear();
            dgv.Rows.Add(f.Length);
            for (int row=0; row<f.Length; row++)
            {
                string []d = f[row].Split('\t');
                for (int col = 0; col < Math.Min(dgv.ColumnCount, d.Length); col++)
                    dgv.Rows[row].Cells[col].Value = d[col];
            }
        }
        //Сохраить список деталей из dgv
        void SaveDetails(string FileName)
        {
            using (StreamWriter stream = new StreamWriter(FileName))
            {
                foreach (DataGridViewRow Row in dgv.Rows)
                {
                    stream.Write(Row.Cells[0].Value);
                    for (int c = 1; c < dgv.ColumnCount; c++)
                    {
                        stream.Write('\t');
                        stream.Write(Row.Cells[c].Value);
                    }
                    stream.WriteLine();
                }
            }
        }
        //Очистить решение
        void ClearSolve()
        {
            Best.Clear();
            pictureBox.Refresh();
        }
        //Сохранить решение
        void SaveSolution(string FileName)
        {
            using (StreamWriter stream = new StreamWriter(FileName))
            {
                foreach (IItem d in Best)
                    stream.WriteLine(d.ToString());
                stream.WriteLine(VisualCriterium.ToString());
            }
        }

        //Решить задачу методом полного перебора
        private void solveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Прочитать настройки
            CheckTheSettings();
            int.TryParse(textBoxHeight.Text, out fHeight);
            textBoxHeight.Text = fHeight.ToString();
            int.TryParse(textBoxWidth.Text, out fWidth);
            textBoxWidth.Text = fWidth.ToString();

            DrawByGraphics drawer = new DrawByGraphics(g);

            //Создать список деталей
            Full = ParseDetailList();
            //Провести перебор возможных размещений (с шагом)
            Codestring cdstr = new Codestring(Full, Full, (SimpleDecoder)SimpleDecoder.GetInstance(fHeight, fWidth));
            int Criterium = cdstr.Decoder.CountCriterium(cdstr.CurItems);
            VisualCriterium = cdstr.Decoder.GetVisualCriterium();
            Best = cdstr.Decoder.GetVisualItemsList();
            listBox1.Items.Add("Criterium square = " + Criterium);

            foreach(IVisualItem item in Best)
            {
                drawer.Print(item);
            }
            drawer.Print(VisualCriterium);
        }

        private void SolutionButton_Click(object sender, EventArgs e)
        {

        }

        private List<IItem> ParseDetailList()
        {
            List<IItem> items = new List<IItem>();
            //Для каждой строчки создать и добавить в список Full
            //с учетом количества
            foreach (DataGridViewRow Row in dgv.Rows)
            {
                int Ndetails = Convert.ToInt32(Row.Cells[3].Value);
                for (int n = 0; n < Ndetails; n++)
                {
                    Detail d = new Detail(Row.Cells[0].Value.ToString() + " num. " + (n + 1).ToString(), Convert.ToInt32(Row.Cells[2].Value),
                                          Convert.ToInt32(Row.Cells[1].Value));
                    items.Add(d);
                }
            }
            return items;
        }
        private void CheckTheSettings()
        {

        }
    }
}