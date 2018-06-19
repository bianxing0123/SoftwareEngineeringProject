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
        int t;
        public judge(int flag,string str)
        {
            InitializeComponent();
            t = 0;
            mycon.ConnectionString = "Data Source=DESKTOP-E28V9KP\\SQLEXPRESS;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=sa.123";
            this.flag = flag;
            textBox1.Text = str;
            mycon.Open();
            if (flag == 1)
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from judge where subject='" + textBox1.Text.ToString().Trim() + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "judge");
                textBox2.Text = Convert.ToString(ds.Tables["judge"].Rows.Count + 1);
            }
            else if (flag == 2)
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from filling where subject='" + textBox1.Text.ToString().Trim() + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "filling");
                textBox2.Text = Convert.ToString(ds.Tables["filling"].Rows.Count + 1);
            }
            mycon.Close();
        }
        public judge(int flag, string str1,string str2,string str3,string str4)
        {
            InitializeComponent();
            t = 1;
            mycon.ConnectionString = "Data Source=DESKTOP-E28V9KP\\SQLEXPRESS;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=sa.123";
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
                if (t == 1)
                {
                    SqlCommand cmd = new SqlCommand("update judge set question='" + textBox3.Text.ToString().Trim() + "',answer='" + textBox4.Text.ToString().Trim() + "' where id=" + int.Parse(textBox2.Text.Trim()) + " and subject='" + textBox1.Text.ToString().Trim() + "'", mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    this.Close();
                }
                else
                {
                    SqlDataAdapter da = new SqlDataAdapter("select * from judge where id=" + int.Parse(textBox2.Text.Trim()) + " and subject='" + textBox1.Text.ToString().Trim() + "'", mycon);
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
            }
            if (flag == 2)
            {
                mycon.Open();
                if (t == 1)
                {
                    SqlCommand cmd = new SqlCommand("update filling set question='" + textBox3.Text.ToString().Trim() + "',answer='" + textBox4.Text.ToString().Trim() + "' where id=" + int.Parse(textBox2.Text.Trim()) + " and subject='" + textBox1.Text.ToString().Trim() + "'", mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    this.Close();
                }
                else
                {
                    SqlDataAdapter da = new SqlDataAdapter("select * from filling where id=" + int.Parse(textBox2.Text.Trim()) + " and subject='" + textBox1.Text.ToString().Trim() + "'", mycon);
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
        }

        private void judge_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
        }
    }
}
