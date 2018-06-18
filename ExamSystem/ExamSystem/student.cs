using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ExamSystem
{
    public partial class student : Form
    {
        int f;//f0增加 1修改
        SqlConnection mycon = new SqlConnection();
        public student(int f)
        {
            InitializeComponent();
            mycon.ConnectionString = "Data Source=GWO-20140219FWK;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=123456";
            this.f = f;
        }
        public student(int f,string str1,string str2,string str3,string str4)
        {
            InitializeComponent();
            mycon.ConnectionString = "Data Source=GWO-20140219FWK;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=123456";
            this.f = f;
            textBox1.Text = str1;
            textBox2.Text = str2;
            textBox3.Text = str3;
            textBox4.Text = str4;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (f == 0)
            {
                string patten = @"[a-zA-Z0-9]+@[a-zA-Z]+\.com$";
                Regex r = new Regex(patten);
                Match m = r.Match(textBox4.Text);
                mycon.Open();
                SqlDataAdapter da=new SqlDataAdapter("select * from student where ID='"+textBox1.Text.ToString().Trim()+"'",mycon);
                DataSet ds=new DataSet();
                da.Fill(ds,"student");
                mycon.Close();
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("信息不完整");
                }
                else if(!m.Success)
                {
                    MessageBox.Show("邮箱格式错误！请重新输入");
                }
                else if(ds.Tables["student"].Rows.Count>0)
                {
                    MessageBox.Show("学号已存在");
                }
                else
                {
                    mycon.Open();
                    SqlCommand cmd = new SqlCommand("insert into student(ID,Name,Password, Malebox) values ('" + textBox1.Text.ToString().Trim() + "','" + textBox2.Text.ToString().Trim() + "', '" + textBox3.Text.ToString().Trim() + "', '" + textBox4.Text.ToString().Trim() + "')", mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    this.Close();
                }
            }
            if (f == 1)
            {
                string patten = @"[a-zA-Z0-9]+@[a-zA-Z]+\.com$";
                Regex r = new Regex(patten);
                Match m = r.Match(textBox4.Text);
                mycon.Open();
                SqlCommand cmd = new SqlCommand("delete from student where ID='" + textBox1.Text.ToString().Trim() + "'", mycon);
                cmd.ExecuteNonQuery();
                mycon.Close();
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("信息不完整");
                }
                else if (!m.Success)
                {
                    MessageBox.Show("邮箱格式错误！请重新输入");
                }
                else
                {
                    mycon.Open();
                    cmd = new SqlCommand("insert into student(ID,Name,Password, Malebox) values ('" + textBox1.Text.ToString().Trim() + "','" + textBox2.Text.ToString().Trim() + "', '" + textBox3.Text.ToString().Trim() + "', '" + textBox4.Text.ToString().Trim() + "')", mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    this.Close();
                }
            }
        }

        private void student_Load(object sender, EventArgs e)
        {
            if (f == 1)
            {
                this.Owner.Hide();
            }
        }
    }
}
