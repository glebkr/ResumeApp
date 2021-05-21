using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class del_resume : Form
    {
        public del_resume()
        {
            InitializeComponent();
        }

        private MySqlConnection mycon;
        private MySqlDataAdapter myda;
        private DataTable table;
        private string connect = "server=localhost;user id=root;password=11234;database=resume_info";

        private void button1_Click(object sender, EventArgs e) //Кнопка удаления данных из таблицы Резюме по ФИО соискателя
        {
            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Все поля должны быть заполнены!");
                }
                else
                {
                    string query = $"DELETE FROM resume WHERE FIO = '{textBox1.Text}'";

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsLetter(ch) && (!Char.IsControl(ch)) && (ch != (int)Keys.Space))
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
