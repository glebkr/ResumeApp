using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using ExcelObj = Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace App
{
    public partial class Form1 : Form
    {
        private ToolStripLabel dateLabel;
        private ToolStripLabel timeLabel;
        private ToolStripLabel infoLabel;
        private Timer timer;

        void statusstrip()
        {
            label1.BackColor = Color.DarkSalmon;
            this.dataGridView1.BackgroundColor = Color.LightCyan;
            this.BackColor = Color.DarkSalmon;

            infoLabel = new ToolStripLabel();
            infoLabel.Text = "Текущие дата и время: ";
            dateLabel = new ToolStripLabel();
            timeLabel = new ToolStripLabel();

            statusStrip1.Items.Add(infoLabel);
            statusStrip1.Items.Add(dateLabel);
            statusStrip1.Items.Add(timeLabel);

            infoLabel.BackColor = Form1.DefaultBackColor;
            dateLabel.BackColor = Form1.DefaultBackColor;
            timeLabel.BackColor = Form1.DefaultBackColor;

            timer = new Timer()
            {
                Interval = 1000
            };
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToLongDateString();
            timeLabel.Text = DateTime.Now.ToLongTimeString();
        }

        public Form1()
        {
            Program.form1 = this;
            InitializeComponent();
            statusstrip();
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (Convert.ToInt32(textBox1.Text) > Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value))
                {
                    dataGridView1.Rows.RemoveAt(i--);
                }
            }
        }

        private void сохранитьДанныеВExcelФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel файлы (*.xls)|*.xls";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ExcelObj.Application ExcelApp = new ExcelObj.Application();
                ExcelObj._Workbook ExcelWorkBook = ExcelApp.Workbooks.Add();
                ExcelObj.Worksheet ExcelWorkSheet = ExcelWorkBook.ActiveSheet;
                for (int i = 1; i < dataGridView1.RowCount + 1; i++)
                {
                    for (int j = 1; j < dataGridView1.ColumnCount + 1; j++)
                    {
                        ExcelWorkSheet.Rows[i].Columns[j] = dataGridView1.Rows[i - 1].Cells[j - 1].Value;
                    }
                }

                dataGridView1.Refresh();
                ExcelApp.AlertBeforeOverwriting = false;
                ExcelWorkBook.SaveAs(sfd.FileName);
                ExcelApp.Quit();
            }
        }

        private void загрузитьДанныеИзExcelФайлаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Excel файлы (*.xls)|*.xls";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                ExcelObj.Application ExcelApp = new ExcelObj.Application();
                ExcelObj._Workbook ExcelWorkBook;
                ExcelObj.Worksheet ExcelWorkSheet;

                ExcelWorkBook = ExcelApp.Workbooks.Open(ofd.FileName, 0, true, 5, "", "", true, ExcelObj.XlPlatform.xlWindows,
                    "\t", false, false, 0, true, 1, 0);
                ExcelWorkSheet = (ExcelObj.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);
                ExcelObj.Range ExcelRange = ExcelWorkSheet.UsedRange;
                for (int i = 0; i < ExcelRange.Rows.Count; i++)
                {
                    dataGridView1.Rows.Add(1);
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = ExcelApp.Cells[i + 1, j + 1].Value;
                    }
                }

                dataGridView1.Refresh();
                ExcelApp.Quit();
            }
        }

        private void сохранитьДанныеВТектовыйФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "txt файлы (*.txt)|*.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.OpenFile());
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        if (dataGridView1.Rows[i].Cells[j].Value == null)
                        {
                            sw.Write(dataGridView1.Rows[i].Cells[j].Value = "");
                        }
                        else
                        {
                            sw.Write(dataGridView1.Rows[i].Cells[j].Value + "\t");
                        }
                    }
                    sw.WriteLine();
                }

                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }

        private void загрузитьДанныеИзТекстовогоФайлаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "txt файлы (*.txt)|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                using (StreamReader sr = new StreamReader(ofd.FileName))
                {
                    while (!sr.EndOfStream)
                    {
                        int rowId = dataGridView1.Rows.Add();
                        DataGridViewRow row = dataGridView1.Rows[rowId];
                        string line = sr.ReadLine();
                        string[] s = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < s.Length; i++)
                        {
                            row.Cells[i].Value = s[i];
                        }
                    }
                    
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

