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
    public partial class insert_position : Form
    {
        public insert_position()
        {
            InitializeComponent();
        }

        private MySqlConnection mycon;
        private MySqlDataAdapter myda;
        private DataTable table;
        string subv;

        private string connect = "server=localhost;user id=root;password=11234;database=resume_info";


        private void button2_Click(object sender, EventArgs e) //Кнопка добавления данных в БД и вывод их в dataGridView
        {
            try
            {
                string query = $"INSERT INTO positions (id_subvision, position_title) VALUES ({int.Parse(subv)}, '{textBox1.Text}')";
                mycon = new MySqlConnection(connect);
                mycon.Open();

                myda = new MySqlDataAdapter(query, connect);
                table = new DataTable();
                myda.Fill(table);

                Program.mainform.dataGridView1.DataSource = table;

                Program.mainform.button2_Click(null, null);
                mycon.Close();
                this.Close();
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

        private void button2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && (!Char.IsControl(ch)))
            {
                e.Handled = true;
            }
        }

        private void insert_position_Load(object sender, EventArgs e) //Вывод данных из таблицы Структурные подразделения в listView
        {
            string query = "SELECT * FROM subvisions";
            mycon = new MySqlConnection(connect);
            mycon.Open();

            listView1.Items.Clear();
            ListViewItem item = null;
            listView1.View = View.Details;

            MySqlCommand command = new MySqlCommand(query, mycon);
            MySqlDataReader dataReader = null;

            dataReader = command.ExecuteReader();

            try
            {     
                while (dataReader.Read())
                {
                    item = new ListViewItem(new string[] { Convert.ToString(dataReader["id_subvision"]), Convert.ToString(dataReader["subvision_title"]) });

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

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e) //Выбор id структурного подразделения из listView
        {
            try
            {
                subv = listView1.SelectedItems[0].SubItems[0].Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
