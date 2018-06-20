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
    public partial class xuanze : Form
    {
        SqlConnection mycon = new SqlConnection();
        int t;
        public xuanze(string kecheng, string tixing)
        {
            InitializeComponent();
            t = 0;
            mycon.ConnectionString = "Data Source=DESKTOP-E28V9KP\\SQLEXPRESS;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=sa.123";
            textBox1.Text = kecheng;
            mycon.Open();
            SqlDataAdapter da = new SqlDataAdapter("select * from choice where subject='" + textBox1.Text.ToString().Trim()+"'", mycon);
            DataSet ds = new DataSet();
            da.Fill(ds, "choice");
            textBox2.Text = Convert.ToString(ds.Tables["choice"].Rows.Count+1);
            mycon.Close();
        }
        public xuanze(string str1, string str2, string str3, string str4, string str5, string str6, string str7, string str8)
        {
            InitializeComponent();
            t = 1;
            mycon.ConnectionString = "Data Source=DESKTOP-E28V9KP\\SQLEXPRESS;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=sa.123";
            textBox1.Text = str1;
            textBox2.Text = str2;
            textBox3.Text = str3;
            textBox4.Text = str4;
            textBox5.Text = str5;
            textBox6.Text = str6;
            textBox7.Text = str7;
            textBox8.Text = str8;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            mycon.Open();
            if (t == 1)
            {
                SqlCommand cmd = new SqlCommand("update choice set question='" + textBox3.Text.ToString().Trim() + "',keyA='" + textBox4.Text.ToString().Trim() + "',keyB='" + textBox5.Text.ToString().Trim() + "',keyC='" + textBox6.Text.ToString().Trim() + "',keyD='" + textBox7.Text.ToString().Trim() + "',answer='" + textBox8.Text.ToString().Trim() + "' where id=" + int.Parse(textBox2.Text.Trim()) + " and subject='" + textBox1.Text.ToString().Trim() + "'", mycon);
                cmd.ExecuteNonQuery();
                mycon.Close();
                this.Close();
            }
            else
            {
                string aa, bb, cc, dd;
                aa = "(A)" + textBox4.Text.ToString().Trim();
                bb = "(B)" + textBox5.Text.ToString().Trim();
                cc = "(C)" + textBox6.Text.ToString().Trim();
                dd = "(D)" + textBox7.Text.ToString().Trim();
                SqlDataAdapter da = new SqlDataAdapter("select * from choice where id=" + int.Parse(textBox2.Text.Trim()) + " and subject='" + textBox1.Text.ToString().Trim() + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "choice");
                if (ds.Tables["choice"].Rows.Count > 0)
                {
                    MessageBox.Show("题号已存在");
                    mycon.Close();
                }
                else
                {
                    string str = "insert into choice(subject,id,question,keyA,keyB,keyC,keyD,answer) values('"
                        + textBox1.Text.ToString().Trim() + "','" + int.Parse(textBox2.Text.Trim()) + "','" + textBox3.Text.ToString().Trim() + "','"
                        + aa + "','" + bb + "','" + cc + "','"
                        + dd + "','" + textBox8.Text.ToString().Trim() + "')";
                    SqlCommand cmd = new SqlCommand(str, mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    this.Close();
                }
            }
        }

        private void xuanze_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
        }
    }
}
