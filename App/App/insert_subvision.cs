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
    public partial class insert_subvision : Form
    {
        public insert_subvision()
        {
            InitializeComponent();
        }

        private MySqlConnection mycon;
        private MySqlDataAdapter myda;
        private DataTable table;

        private string connect = "server=localhost;user id=root;password=11234;database=resume_info";

        private void button2_Click(object sender, EventArgs e) //Кнопка добавления данных в БД и вывод их в dataGridView
        {
            try
            {
                if (textBox1.Text != "")
                {
                    string query = $"INSERT INTO subvisions (subvision_title) VALUES ('{textBox1.Text}')";
                    mycon = new MySqlConnection(connect);
                    mycon.Open();

                    myda = new MySqlDataAdapter(query, connect);
                    table = new DataTable();
                    myda.Fill(table);

                    Program.mainform.dataGridView1.DataSource = table;

                    Program.mainform.button7_Click(null, null);
                    mycon.Close();
                    this.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsLetter(ch) && (!Char.IsControl(ch)) && (ch != (int)(Keys.Space)))
            {
                e.Handled = true;
            }
        }
    }
}
