using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Cutter
{
    public partial class Form1 : Form
    {
        private bool stop = false;
        private int fWidth = 100;
        private int fHeight = 100;
        private int fStep = 10;

        public Form1()
        {
            InitializeComponent();
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
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            SaveDetails(saveFileDialog1.FileName);
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
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            //Выбрать масштаб
            float scaleheight = 1.0f * pictureBox.Height / int.Parse(textBoxHeight.Text);
            float scalewidth = 1.0f * pictureBox.Width / int.Parse(textBoxWidth.Text);
            Detail.scale = scaleheight < scalewidth ? scaleheight : scalewidth;
            //Вывести Best
            foreach (var b in Best) b.Paint(e.Graphics);
            //Вывести лучший прямоугольник
            //чтобы не выходило за границу поля:
            if (BestRectangle.Width + BestRectangle.X >= fWidth * Detail.scale - 2)
                BestRectangle.Width = fWidth * Detail.scale - 2 - BestRectangle.X;
            if (BestRectangle.Height + BestRectangle.Y >= fHeight * Detail.scale - 2)
                BestRectangle.Height = fHeight * Detail.scale - 2 - BestRectangle.Y;

            e.Graphics.FillRectangle(Brushes.Green, BestRectangle);
            //Для ориентировки - границу поля
            e.Graphics.DrawRectangle(Pens.Red, 1, 1, fWidth * Detail.scale - 2, fHeight * Detail.scale - 2);
        }
        private void labelIndicator_Click(object sender, EventArgs e)
        {
            stop = true;
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


        List<Detail> Full = new List<Detail>(); //Список имеющихся
        List<Detail> Best = new List<Detail>(); //Лучший вариант
        //Очистить решение
        void ClearSolve()
        {
            Best.Clear();
            pictureBox.Refresh();
        }
        //Решить задачу
        private void solveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Прочитать настройки
            int.TryParse(textBoxHeight.Text, out fHeight);
            textBoxHeight.Text = fHeight.ToString();
            int.TryParse(textBoxWidth.Text, out fWidth);
            textBoxWidth.Text = fWidth.ToString();
            int.TryParse(textBoxStep.Text, out fStep);
            textBoxStep.Text = fStep.ToString();

            //Создать список деталей
            CreateDetailList();
            //Провести перебор возможных размещений (с шагом)
            Solve();
            //Сохранить
            if (saveFileDialog2.ShowDialog() != DialogResult.OK) return;
            SaveSolve(saveFileDialog2.FileName);
        }
        void CreateDetailList()
        {
            Full.Clear();
            //Для каждой строчки создать и добавить в список Full
            //с учетом количества
            foreach (DataGridViewRow Row in dgv.Rows)
            {
                int NDetails = Convert.ToInt32(Row.Cells[3].Value);
                for (int n=0; n<NDetails; n++)
                {
                    Detail d = new Detail()
                    {
                        height = Convert.ToInt32(Row.Cells[2].Value),
                        width = Convert.ToInt32(Row.Cells[1].Value),
                        rotated = false,
                        x = 0, y = 0,
                        id = Row.Cells[0].Value.ToString() + " экз. " + (n + 1).ToString()
                    };
                    Full.Add(d);
                }
            }
        }
        void Solve()
        {
            counter = 1;
            stop = false;
            //Лучшее решение очистить
            BestRect = 0;
            Best.Clear();
            //Создать карту
            Map = new bool[fHeight, fWidth];
            //Рекурсивно найти 
            int index = 0;
            for (int x = 0; x < fWidth; x += fStep)
                for (int y = 0; y < fHeight; y += fStep)
                    for (int r = 0; r < 2; r++)
                        solve(index, x, y, r == 1);
            pictureBox.Refresh(); //Обновить картинку
        }

        bool[,] Map;
        int BestRect = 0;
        void solve(int detail, int X, int Y, bool Rotated)
        {
            if (stop) return;
            //Поместить указанную деталь из общего списка на место
            //Для следующей детали найти место
            Full[detail].x = X;
            Full[detail].y = Y;
            Full[detail].rotated = Rotated;
            if (Full[detail].PlaceTo(Map))
            {
                //Если это последняя деталь, то сравнить с лучшим результатом
                //и запомнить, если лучше
                if (detail == Full.Count - 1)
                    CheckTheBest();
                else
                    //Иначе - разместить следующую деталь
                    for (int x = 0; x < fWidth; x += fStep)
                        for (int y = 0; y < fHeight; y += fStep)
                            for (int r = 0; r < 2; r++)
                                solve(detail + 1, x, y, r == 1);

                //Забрать деталь
                Full[detail].TakeFrom(Map);
            }
        }
        //Проверить решение на "быть лучшим"
        //и сохранить копию в Best
        int counter;
        RectangleF BestRectangle = new Rectangle(0, 0, 0, 0); //Найденный прямоугольник
        void CheckTheBest()
        {
            counter++;
            if (counter % 100 == 0)
            {
                labelIndicator.Text = "СТОП! " + (counter).ToString() + " S = " + BestRect.ToString();
                if (counter % 1000 == 0) pictureBox.Refresh();
                Application.DoEvents();
            }
            int MaxRect = MaxRectangle(Map);
            if (MaxRect > BestRect)
            {
                BestRect = MaxRect;
                //Сохранить копию
                Best.Clear();
                foreach (Detail d in Full)
                {
                    Best.Add(new Detail()
                    {
                        id = d.id,
                        height = d.height,
                        rotated = d.rotated,
                        width = d.width,
                        x = d.x,
                        y = d.y
                    });
                    BestRectangle.X = bestx * Detail.scale + 1;
                    BestRectangle.Y = besty * Detail.scale + 1;
                    BestRectangle.Width = (bestw) * Detail.scale;
                    BestRectangle.Height = (besth) * Detail.scale;
                }
            }
        }

        //Найти свободный прямоугольник максимальной площади
        int bestw = 0, besth = 0; //Лучшие ширина и высота
        int bestx = 0, besty = 0; //Лучшие координаты
        int MaxRectangle(bool[,] Map)
        {
            bestw = 0; besth = 0; //Лучшие ширина и высота
            //перебрав все возможные позиции левого верхнего угла
            for (int x = 0; x < fWidth; x += fStep)
                for (int y = 0; y < fHeight; y += fStep)
                    //и размеры прямоугольника
                    for (int w = fStep; w < fWidth; w += fStep)
                        for (int h = fStep; h < fHeight; h += fStep)
                            if (w * h >= bestw * besth)
                                //определить, занята ли хоть одна точка внутри него
                                if (IsFree(Map, x, y, w, h))
                                {
                                    bestw = w + fStep-1;
                                    besth = h + fStep-1;
                                    bestx = x;
                                    besty = y;
                                }
            //результат вернуть
            return bestw * besth;
        }

        bool IsFree(bool[,] Map, int x, int y, int w, int h)
        {
            if (x + w > fWidth) return false;
            if (y + h > fHeight) return false;
            for (int dx = 0; dx < w; dx += fStep)
                for (int dy = 0; dy < h; dy += fStep)
                    if (Map[y + dy, x + dx]) return false;
            return true;
        }

        //Сохранить решение
        void SaveSolve(string FileName)
        {
            using (StreamWriter stream = new StreamWriter(FileName))
            {
                foreach (Detail d in Best)
                    stream.WriteLine(d.ToString());
            }
        }







        private void SolutionButton_Click(object sender, EventArgs e)
        {

        }
    }
}