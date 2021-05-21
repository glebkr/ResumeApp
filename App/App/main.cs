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
using MySql.Data.MySqlClient;

namespace App
{
    public partial class main : Form
    {
        private ToolStripLabel dateLabel;
        private ToolStripLabel timeLabel;
        private ToolStripLabel infoLabel;
        private Timer timer;

        public main()
        {
            Program.mainform = this;
            InitializeComponent();
            statusstrip();
        }

        private MySqlConnection mycon;
        private MySqlDataAdapter myda;
        private DataTable table;
        private string connect = "server=localhost;user id=root;password=11234;database=resume_info";

        private void Form1_Load(object sender, EventArgs e)
        {
            this.dataGridView1.BackgroundColor = Color.LightCyan;

            label2.Text = "Нажмите на любую из трех кнопок";

            label1.Hide();
            textBox1.Hide();
            button4.Hide();
            button5.Hide();

            button3.Hide();
            button6.Hide();
            button8.Hide();
            button9.Hide();
            button10.Hide();
            button11.Hide();
            button12.Hide();
            button13.Hide();
            button14.Hide();
            button15.Hide();

        }

        void statusstrip() //Строка состояния
        {
            infoLabel = new ToolStripLabel();
            infoLabel.Text = "Текущие дата и время: ";
            dateLabel = new ToolStripLabel();
            timeLabel = new ToolStripLabel();

            statusStrip1.Items.Add(infoLabel);
            statusStrip1.Items.Add(dateLabel);
            statusStrip1.Items.Add(timeLabel);

            infoLabel.BackColor = main.DefaultBackColor;
            dateLabel.BackColor = main.DefaultBackColor;
            timeLabel.BackColor = main.DefaultBackColor;

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

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e) //Кнопка удаления всех данных из таблицы Резюме
        {
            try
            {
                if (MessageBox.Show("Вы точно хотите удалить все резюме?", "Удаление всех резюме", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {

                    string query = "DELETE FROM resume;";

                    mycon = new MySqlConnection(connect);
                    mycon.Open();

                    myda = new MySqlDataAdapter(query, connect);
                    table = new DataTable();
                    myda.Fill(table);

                    dataGridView1.DataSource = table;
                    button1_Click(null, null);
                    mycon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && (!Char.IsControl(ch)))
            {
                e.Handled = true;
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e) //Кнопка фильтрации по стажу работы
        {
            try
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (Convert.ToInt32(textBox1.Text) > Convert.ToInt32(dataGridView1.Rows[i].Cells[7].Value))
                    {
                        dataGridView1.Rows.RemoveAt(i--);
                    }
                }
                textBox1.Text = "";
            }
            catch
            {
                return;
            }
        }
       

        public void button1_Click(object sender, EventArgs e) //Вывод данных из таблицы Резюме в dataGridView
        {
            label2.Text = "Таблица Резюме";

            label1.Show();
            textBox1.Show();
            button4.Show();
            button5.Show();

            button10.Hide();
            button11.Hide();
            button12.Hide();
            button13.Hide();
            button14.Hide();
            button15.Hide();

            label2.Show();
            button3.Show();
            button6.Show();
            button8.Show();
            button9.Show();

            string query = "SELECT * FROM resume";

            mycon = new MySqlConnection(connect);
            mycon.Open();

            myda = new MySqlDataAdapter(query, connect);
            table = new DataTable();

            myda.Fill(table);
            dataGridView1.DataSource = table;

            mycon.Close();
        }

        public void button2_Click(object sender, EventArgs e) //Вывод данных из таблицы Должности в dataGridView
        {
            label2.Text = "Таблица Должности";

            label1.Hide();
            textBox1.Hide();
            button4.Hide();
            button5.Hide();

            button3.Hide();
            button6.Hide();
            button8.Hide();
            button9.Hide();
            button13.Hide();
            button14.Hide();
            button15.Hide();

            label2.Show();
            button10.Show();
            button11.Show();
            button12.Show();

            string script = "SELECT * from positions";

            mycon = new MySqlConnection(connect);
            mycon.Open();

            myda = new MySqlDataAdapter(script, connect);
            table = new DataTable();

            myda.Fill(table);
            dataGridView1.DataSource = table;

            mycon.Close();
        }

        public void button7_Click(object sender, EventArgs e) //Вывод данных из таблицы Структурные подразделения в dataGridView
        {
            label1.Hide();
            textBox1.Hide();
            button4.Hide();
            button5.Hide();

            button3.Hide();
            button6.Hide();
            button8.Hide();
            button9.Hide();
            button10.Hide();
            button11.Hide();
            button12.Hide();

            label2.Show();
            button13.Show();
            button14.Show();
            button15.Show();

            label2.Text = "Таблица Структурные подразделения";
            label2.Show();

            string script = "SELECT * from subvisions";

            mycon = new MySqlConnection(connect);
            mycon.Open();

            myda = new MySqlDataAdapter(script, connect);
            table = new DataTable();

            myda.Fill(table);
            dataGridView1.DataSource = table;

            mycon.Close();
        }

        private void button3_Click(object sender, EventArgs e) //Вызов формы добавления резюме
        {
            insert_resume insert_resume = new insert_resume();
            insert_resume.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e) //Вызов формы удаления резюме
        {
            del_resume del_resume = new del_resume();
            del_resume.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e) //Вызов формы изменения резюме
        {
            update_resume update_resume = new update_resume();
            update_resume.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e) //Вызов формы добавления должности
        {
            insert_position insert_pos = new insert_position();
            insert_pos.ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e) //Вызов формы добавления структурного подразделения
        {
            insert_subvision insert_subv = new insert_subvision();
            insert_subv.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e) //Вызов формы удаления должности
        {
            del_position del_pos = new del_position();
            del_pos.Show();
        }

        private void button14_Click(object sender, EventArgs e) //Вызов формы удаления структурного подразделения
        {
            del_subvision del_subv = new del_subvision();
            del_subv.Show();
        }

        private void button15_Click(object sender, EventArgs e) //Вызов формы изменения структурного подразделения
        {
            update_subvision update_subv = new update_subvision();
            update_subv.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e) //Вызов формы изменения должности
        {
            update_position update_pos = new update_position();
            update_pos.ShowDialog();
        }
    }
}

