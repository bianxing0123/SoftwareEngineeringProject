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
    public partial class judge : Form
    {
        SqlConnection mycon = new SqlConnection();
        int flag;//1增加判断 2增加填空 
        public judge(int flag,string str)
        {
            InitializeComponent();
            mycon.ConnectionString = "Data Source=GWO-20140219FWK;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=123456";
            this.flag = flag;
            textBox1.Text = str;
        }
        public judge(int flag, string str1,string str2,string str3,string str4)
        {
            InitializeComponent();
            mycon.ConnectionString = "Data Source=GWO-20140219FWK;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=123456";
            this.flag = flag;
            textBox1.Text = str1;
            textBox2.Text = str2;
            textBox3.Text = str3;
            textBox4.Text = str4;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (flag == 1)
            {
                mycon.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from choice where id=" + int.Parse(textBox2.Text.Trim()), mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "choice");
                if (ds.Tables["choice"].Rows.Count > 0)
                {
                    MessageBox.Show("题号已存在");
                    mycon.Close();
                }
                else
                {
                    string str = "insert into judge(subject,id,question,answer) values('" + textBox1.Text.ToString().Trim() + "'," + int.Parse(textBox2.Text.ToString().Trim()) + ",'" + textBox3.Text.ToString().Trim() + "','" + textBox4.Text.ToString().Trim() + "')";
                    SqlCommand cmd = new SqlCommand(str, mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    this.Close();
                }
            }
            if (flag == 2)
            {
                mycon.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from choice where id=" + int.Parse(textBox2.Text.Trim()), mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "choice");
                if (ds.Tables["choice"].Rows.Count > 0)
                {
                    MessageBox.Show("题号已存在");
                    mycon.Close();
                }
                else
                {
                    string str = "insert into filling(subject,id,question,answer) values('" + textBox1.Text.ToString().Trim() + "'," + int.Parse(textBox2.Text.ToString().Trim()) + ",'" + textBox3.Text.ToString().Trim() + "','" + textBox4.Text.ToString().Trim() + "')";
                    SqlCommand cmd = new SqlCommand(str, mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    this.Close();
                }
            }
        }

        private void judge_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
        }
    }
}
