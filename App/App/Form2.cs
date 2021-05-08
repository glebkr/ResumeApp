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

namespace App
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            textBox8.Text = "+375XX-XXX-XX-XX";
            textBox8.ForeColor = Color.Gray;

            textBox2.Text = "dd.mm.yyyy";
            textBox2.ForeColor = Color.Gray;

            this.BackColor = Color.NavajoWhite;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" ||
                    textBox6.Text == "" || textBox7.Text == "" || textBox2.Text == "dd.mm.yyyy" || textBox8.Text == "+375XX-XXX-XX-XX")
                {
                    throw new Exception("Все поля должны быть заполнены!");
                }
                else 
                {
                    string[] arr2 = textBox8.Text.Split(new char[] { ' ' });
                    foreach (var num in arr2)
                    {
                        if (Regex.IsMatch(num, @"^(\+375|80)(44|29|25|33)[\-]?[0-9]{3}[\-]?[0-9]{2}[\-]?[0-9]{2}$"))
                        {
                            int rowId = Program.form1.dataGridView1.Rows.Add();
                            DataGridViewRow row = Program.form1.dataGridView1.Rows[rowId];
                            row.Cells[0].Value = textBox1.Text;
                            row.Cells[1].Value = textBox2.Text;
                            row.Cells[2].Value = textBox3.Text;
                            row.Cells[3].Value = textBox4.Text;
                            row.Cells[4].Value = textBox5.Text;
                            row.Cells[5].Value = textBox6.Text;
                            row.Cells[6].Value = textBox7.Text;
                            row.Cells[7].Value = textBox8.Text;
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

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "dd.mm.yyyy")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "dd.mm.yyyy";
                textBox2.ForeColor = Color.Gray;
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

        private void textBox8_Leave(object sender, EventArgs e)
        {
            if (textBox8.Text == "")
            {
                textBox8.Text = "+375XX-XXX-XX-XX";
                textBox8.ForeColor = Color.Gray;
            }
   
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
    }
}
