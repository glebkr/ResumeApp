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
    public partial class update_position : Form
    {
        public update_position()
        {
            InitializeComponent();
        }

        private MySqlConnection mycon;
        private MySqlDataAdapter myda;
        private DataTable table;
        string subv;
        public string connect = "server=localhost;user id=root;password=11234;database=resume_info";

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) //Кнопка изменения данных в таблице Должности и вывод их в dataGridView
        {
            try
            {
                string query = $"UPDATE positions SET id_subvision = {int.Parse(subv)}, position_title = '{textBox2.Text}' WHERE id_position = {int.Parse(textBox1.Text)}";

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
            catch
            {
                return;
            }
        }

        private void update_position_Load(object sender, EventArgs e) //Вывод данных из таблицы Структурные подразделения в listView
        {
            MySqlDataReader dataReader = null;
            try
            {
                MySqlConnection mycon = new MySqlConnection(connect);
                mycon.Open();

                listView1.Items.Clear();

                listView1.View = View.Details;

                string query = "SELECT * FROM subvisions";

                MySqlCommand command = new MySqlCommand(query, mycon);

                dataReader = command.ExecuteReader();
                ListViewItem item = null;

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
            }
        }

        private void textBox1_Leave(object sender, EventArgs e) //Кнопка автоматического заполнения текстовый полей в соответствии с введенным id должности
        {
            try
            {
                foreach (var s in listView1.SelectedIndices)
                {
                    listView1.Items[Convert.ToInt32(s)].Selected = false;
                }

                int rowID;
                int id_resume = int.Parse(textBox1.Text);

                for (int i = 0; i < Program.mainform.dataGridView1.Rows.Count; i++)
                {
                    int row = Convert.ToInt32(Program.mainform.dataGridView1.Rows[i].Cells[0].Value);
                    if (id_resume == row)
                    {
                        rowID = Program.mainform.dataGridView1.Rows[i].Index;
                        textBox2.Text = Convert.ToString(Program.mainform.dataGridView1.Rows[rowID].Cells[2].Value);

                        if (listView1.Items.Count > 0)
                        {
                            for (int j = 0; j < listView1.Items.Count; j++)
                            {
                                if (listView1.Items[j].SubItems[0].Text == Convert.ToString(Program.mainform.dataGridView1.Rows[rowID].Cells[1].Value))
                                {
                                    listView1.Items[j].Focused = true;
                                    listView1.Items[j].Selected = true;
                                    listView1.Items[j].EnsureVisible();

                                }
                            }

                        }
                    }
                }
            }
            catch
            {
                return;
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

        private void listView1_MouseClick(object sender, MouseEventArgs e)
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
