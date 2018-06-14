using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ExamSystem
{
    public partial class registe : Form
    {
        SqlConnection mycon = new SqlConnection();
        public registe()
        {
            InitializeComponent();
            mycon.ConnectionString = "Data Source=HWH-PC\\SQLEXPRESS;Initial Catalog=ExamSystem;User ID=sa;Password=123456";
        }

        private void registe_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.ShowDialog(this);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                mycon.Open();
                bool iskong = true;
                bool msgt1 = false;
                bool msgt2 = false;

                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
                {
                    MessageBox.Show("注册信息不完整");
                    iskong = false;
                }
                
                if (iskong == true)
                {
                    char[] t1 = textBox1.Text.ToCharArray();
                    if (t1.Length < 4 || t1.Length > 16)
                    {
                        msgt1 = true;
                        MessageBox.Show("用户名长度为4-16个字符");
                    }
                    char[] t2 = textBox2.Text.ToCharArray();                    
                    if (t2.Length < 6 || t1.Length > 16)
                    {
                        if (msgt1 == false)
                        {
                            MessageBox.Show("密码长度为6-16个字符");
                            msgt2 = true;
                        }
                    }                    
                    string id = " ";
                    SqlCommand ccc = mycon.CreateCommand();
                    ccc.CommandText = "SELECT * FROM student WHERE ID='" + textBox1.Text + "'";
                    int isrepeat = Convert.ToInt32(ccc.ExecuteScalar());
                    bool pwdrepeat = true;
                    bool layout = true;
                    if (isrepeat > 0)
                    {
                        MessageBox.Show("学号已存在！请重新输入");
                    }
                    else
                    {
                        id = textBox1.Text;
                    }
                    string pwd = " ";
                    if (textBox2.Text == textBox3.Text)
                    {
                        pwd = textBox2.Text;
                        pwdrepeat = true;
                    }
                    else
                    {
                        MessageBox.Show("密码确认错误！请重新输入");
                        pwdrepeat = false;
                        mycon.Close();
                    }
                    string male = " ";
                    string patten = @"[a-zA-Z0-9]+@[a-zA-Z]+\.com$";
                    Regex r = new Regex(patten);
                    Match m = r.Match(textBox4.Text);
                    if (m.Success)
                    {
                        male = textBox4.Text;
                        layout = true;
                    }
                    else
                    {
                        if(msgt1 == false && msgt2 == false)
                            MessageBox.Show("邮箱格式错误！请重新输入");
                        layout = false;
                        mycon.Close();
                    }
                    SqlCommand cmd = new SqlCommand("insert into student(ID,Name,Password, Malebox) values ('" + id + "','" + textBox5.Text + "', '" + pwd + "', '" + male + "')", mycon);
                    if (isrepeat == 0 && pwdrepeat == true && layout == true)
                    {
                        int ret = (int)cmd.ExecuteNonQuery();
                        if (ret > 0)
                        {
                            MessageBox.Show("注册成功！");
                            Form1 form1 = new Form1();
                            form1.ShowDialog(this);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("注册失败！");
                        }
                    }
                }
                mycon.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
