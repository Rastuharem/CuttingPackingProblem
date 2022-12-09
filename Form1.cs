using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Cutter
{
    public partial class Form1 : Form
    {
        private int fWidth = 100; // Width of task list
        private int fHeight = 100; // Height of task list

        private Graphics g;

        private List<IItem> Full = new List<IItem>(); // List of all details
        private List<IVisualItem> Best = new List<IVisualItem>(); // Best option
        private IVisualItem VisualCriterium; // Criterium abstract detail

        public Form1()
        {
            InitializeComponent();

            g = pictureBox.CreateGraphics();
        }

        // 'Exit' button
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        // 'Open' button
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            LoadDetails(openFileDialog1.FileName);
            ClearSolve();

            //g.DrawRectangle(Pens.Red, new Rectangle(0, 0, pictureBox.Width-1, pictureBox.Height-1));
        }
        // 'Save list of details' button
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() != DialogResult.OK) return;
            SaveDetails(saveFileDialog2.FileName);
        }
        // 'Save solution' button
        private void SaveSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() != DialogResult.OK) return;
            SaveSolution(saveFileDialog2.FileName);
        }
        // '+' button
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            dgv.Rows.Add();
        }
        // '-' button
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dgv.RowCount > 0)
                dgv.Rows.RemoveAt(dgv.RowCount - 1);
        }
        // Textboxes input checker
        private void textBoxWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (Char.IsLetter(number))
            {
                e.Handled = true;
            }
        }

        // Picturebox painter
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.Clear(Color.White);
            //Choose scale
            float scaleheight = 1.0f * pictureBox.Height / int.Parse(textBoxHeight.Text);
            float scalewidth = 1.0f * pictureBox.Width / int.Parse(textBoxWidth.Text);
            DrawByGraphics.scale = scaleheight < scalewidth ? scaleheight : scalewidth;
            e.Graphics.DrawRectangle(Pens.Red, new Rectangle(0, 0,
                Convert.ToInt32(fWidth * DrawByGraphics.scale) - 1, Convert.ToInt32(fHeight * DrawByGraphics.scale) - 1));
        }

        // Load list of details to 'dgv'
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
            pictureBox_Paint(Owner, new PaintEventArgs(g, pictureBox.DisplayRectangle));
        }
        // Save list of details from 'dgv'
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
        // Clear Solution
        void ClearSolve()
        {
            Best.Clear();
            pictureBox.Refresh();
        }
        // Save Solution
        void SaveSolution(string FileName)
        {
            using (StreamWriter stream = new StreamWriter(FileName))
            {
                foreach (IItem d in Best)
                    stream.WriteLine(d.ToString());
                stream.WriteLine(VisualCriterium.ToString());
            }
        }

        // Solve task with Enumeration method
        private void solveEnumMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsGoodSettings()) return;
            pictureBox_Paint(Owner, new PaintEventArgs(g, pictureBox.DisplayRectangle));

            DrawByGraphics drawer = new DrawByGraphics(g);
            PrintByListBox printer = new PrintByListBox(listBox1);
            Full = ParseDetailList();

            EnumAlgorithm alg = new EnumAlgorithm(Full, SimpleDecoder.GetInstance(fHeight, fWidth), printer);
            alg.GetSolution(out Best, out VisualCriterium);

            foreach (IVisualItem item in Best)
            {
                drawer.Print(item);
            }
            drawer.Print(VisualCriterium);
        }

        // Solve task with Evolution algorithm
        private void solveEvolAlgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsGoodSettings()) return;
            pictureBox_Paint(Owner, new PaintEventArgs(g, pictureBox.DisplayRectangle));

            DrawByGraphics drawer = new DrawByGraphics(g);
            PrintByListBox printer = new PrintByListBox(listBox1);
            Full = ParseDetailList();

            EvolutionAlgorithm alg = new EvolutionAlgorithm(Full, SimpleDecoder.GetInstance(fHeight, fWidth), printer);
            alg.GetSolution(out Best, out VisualCriterium);

            foreach (IVisualItem item in Best)
            {
                drawer.Print(item);
            }
            drawer.Print(VisualCriterium);
        }

        // Parse data from 'dgv' into list
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
        // Common data checker
        private bool IsGoodSettings()
        {
            if(textBoxHeight.Text == "" || textBoxWidth.Text == "")
            {
                MessageBox.Show("Type data in textboxes!!!");
                return false;
            }
            try
            {
                int.TryParse(textBoxHeight.Text, out fHeight);
                textBoxHeight.Text = fHeight.ToString();
                int.TryParse(textBoxWidth.Text, out fWidth);
                textBoxWidth.Text = fWidth.ToString();
            }
            catch
            {
                MessageBox.Show("Error in parsing textboxes. Please, check input.");
                return false;
            }
            SimpleDecoder.GetInstance(fHeight, fWidth).SetFHeight(fHeight);
            SimpleDecoder.GetInstance(fHeight, fWidth).SetFWidth(fWidth);
            return true;
        }
    }
}