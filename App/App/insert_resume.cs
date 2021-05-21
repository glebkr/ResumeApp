using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace App
{
    public partial class insert_resume : Form
    {
        public insert_resume()
        {
            InitializeComponent();
        }

        private MySqlConnection mycon;
        private MySqlCommand mycom;
        private MySqlDataAdapter myda;
        private DataTable table;
        string pos;
        public string connect = "server=localhost;user id=root;password=11234;database=resume_info";

        private void Form2_Load(object sender, EventArgs e)   //Вывод данных из таблицы Должности в listView
        {
            MySqlConnection mycon = new MySqlConnection(connect);
            mycon.Open();

            listView1.Items.Clear();

            MySqlDataReader dataReader = null;

            string query = "SELECT id_position, position_title FROM positions";

            try
            {
                MySqlCommand command = new MySqlCommand(query, mycon);

                dataReader = command.ExecuteReader();
                listView1.View = View.Details;
                ListViewItem item = null;

                while (dataReader.Read())
                {
                    item = new ListViewItem(new string[] { Convert.ToString(dataReader["id_position"]), Convert.ToString(dataReader["position_title"])});

                    listView1.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
                mycon.Close();
            }

            textBox8.Text = "+375XX-XXX-XX-XX";
            textBox8.ForeColor = Color.Gray;

            textBox2.Text = "yyyy-mm-dd";
            textBox2.ForeColor = Color.Gray;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)  //Кнопка добавления введенных данных в БД и вывод их в dataGridView
        {
            try
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" ||
                    textBox6.Text == "" || textBox7.Text == "" || textBox2.Text == "dd.mm.yyyy" || textBox8.Text == "+375XX-XXX-XX-XX")
                {
                    MessageBox.Show("Все поля должны быть заполнены!");
                }
                else 
                {
                    string[] arr2 = textBox8.Text.Split(new char[] { ' ' });
                    foreach (var num in arr2)
                    {
                        if (Regex.IsMatch(num, @"^(\+375|80)(44|29|25|33)[\-]?[0-9]{3}[\-]?[0-9]{2}[\-]?[0-9]{2}$"))
                        {
                            DateTime date = DateTime.Parse(textBox2.Text);
                            string query = $"INSERT INTO resume (id_position, FIO, birthdate, education, languages, computer_proficiency, work_experience, recommendations, phone_number)" +
                            $" VALUES ({int.Parse(pos)}, '{textBox1.Text}', '{date.Year}-{date.Month}-{date.Day}', '{textBox3.Text}', '{textBox4.Text}', '{textBox5.Text}', {int.Parse(textBox6.Text)}, '{textBox7.Text}', '{textBox8.Text}')";

                            mycon = new MySqlConnection(connect);
                            mycon.Open();

                            myda = new MySqlDataAdapter(query, connect);
                            table = new DataTable();

                            myda.Fill(table);
                            Program.mainform.dataGridView1.DataSource = table;

                            Program.mainform.button1_Click(null, null);

                            mycon.Close();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Введенный номер телефона некорректен");
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsLetter(ch) && (!Char.IsControl(ch)) && (ch != (int)Keys.Space))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && (!Char.IsControl(ch)) && (ch != '-'))
            {
                e.Handled = true;
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e) //Выбор id должности из listView
        {
            try
            {
                pos = listView1.SelectedItems[0].SubItems[0].Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void textBox8_Leave(object sender, EventArgs e)
        {
            if (textBox8.Text == "")
            {
                textBox8.Text = "+375XX-XXX-XX-XX";
                textBox8.ForeColor = Color.Gray;
            }
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            if (textBox8.Text == "+375XX-XXX-XX-XX")
            {
                textBox8.Text = null;
                textBox8.ForeColor = Color.Black;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "yyyy-mm-dd")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "yyyy-mm-dd";
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && (!Char.IsControl(ch)))
            {
                e.Handled = true;
            }
        }
    }
}
