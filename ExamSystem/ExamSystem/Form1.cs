using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ExamSystem
{
    public partial class Form1 : Form
    {
        SqlConnection mycon = new SqlConnection();
        public Form1()
        {
            InitializeComponent();
            mycon.ConnectionString = "Data Source=GWO-20140219FWK;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=123456";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            registe f = new registe();
            f.ShowDialog(this);
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                mycon.Open();                
                SqlCommand cmd = mycon.CreateCommand();
                cmd.CommandText = "SELECT * FROM student WHERE ID='" + textBox1.Text + "' and Password='" + textBox2.Text + "'";           
                long islogin = Convert.ToInt64(cmd.ExecuteScalar());
               
                if (islogin > 0 && radioButton1.Checked)
                {
                    MessageBox.Show("登录成功");
                    user form2 = new user(textBox1.Text);
                    form2.ShowDialog(this);
                    this.Close();
                    mycon.Close();
                }
                if (islogin > 0 && radioButton2.Checked)
                {
                    MessageBox.Show("管理员用户名或密码错误!");
                    mycon.Close();
                }
                if (textBox1.Text == "admin" && textBox1.Text == "admin" && radioButton2.Checked)
                {
                    MessageBox.Show("管理员登录成功");
                    admin form2 = new admin();
                    form2.ShowDialog(this);
                    this.Close();
                    mycon.Close();
                }
                if (!(radioButton1.Checked || radioButton2.Checked))
                {
                    MessageBox.Show("请选择用户类型！");
                    mycon.Close();
                }
                if (islogin == 0 && (radioButton1.Checked || radioButton2.Checked))
                {
                    MessageBox.Show("用户名或密码错误！请重新输入");
                    mycon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
