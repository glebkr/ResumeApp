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
    public partial class update_subvision : Form
    {
        public update_subvision()
        {
            InitializeComponent();
        }

        private MySqlConnection mycon;
        private MySqlDataAdapter myda;
        private DataTable table;
        public string connect = "server=localhost;user id=root;password=11234;database=resume_info";

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) //Кнопка изменения данных в таблице Структурные подразделения и вывод их в dataGridView
        {
            try
            {
                string query = $"UPDATE subvisions SET subvision_title = '{textBox2.Text}' WHERE id_subvision = {int.Parse(textBox1.Text)}";

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
            catch
            { 
                return;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e) //Кнопка автоматического заполнения текстовый полей в соответствии с введенным id структурного подразделения
        {
            try
            {
                int rowID;
                int id_subv = int.Parse(textBox1.Text);

                for (int i = 0; i < Program.mainform.dataGridView1.Rows.Count; i++)
                {
                    int row = (Convert.ToInt32(Program.mainform.dataGridView1.Rows[i].Cells[0].Value));
                    if (id_subv == row)
                    {
                        rowID = Program.mainform.dataGridView1.Rows[i].Index;
                        textBox2.Text = Convert.ToString(Program.mainform.dataGridView1.Rows[rowID].Cells[1].Value);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Все поля должны быть заполнены!");
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && (!Char.IsControl(ch)))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsLetter(ch) && (!Char.IsControl(ch)) && (ch != (int)(Keys.Space)))
            {
                e.Handled = true;
            }
        }

    }
}
