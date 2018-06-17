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
    public partial class temp : Form
    {
        int flag;
        SqlConnection mycon = new SqlConnection();
        public temp(int flag)
        {//flag 1增加 2删除 3修改
            InitializeComponent();
            mycon.ConnectionString = "Data Source=GWO-20140219FWK;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=123456";
            this.flag = flag;
            if (flag == 1)
            {
                textBox1.Visible = false;
                label1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Trim() == "" || comboBox2.Text.Trim() == "")
            {
                MessageBox.Show("请选择课程和题型！");
            }
            else if (flag == 1)//增加
            {
                if (comboBox2.Text.ToString().Trim() == "选择题")
                {
                    xuanze x = new xuanze(comboBox1.Text, comboBox2.Text);
                    x.ShowDialog(this);
                    this.Close();
                }
                if (comboBox2.Text.ToString().Trim() == "判断题")
                {
                    judge j = new judge(1,comboBox1.Text.ToString().Trim());
                    j.ShowDialog(this);
                    this.Close();
                }
                if (comboBox2.Text.ToString().Trim() == "填空题")
                {
                    judge j = new judge(2,comboBox1.Text.ToString().Trim());
                    j.ShowDialog(this);
                    this.Close();
                }
            }
            else if (flag == 2)//删除
            {
                if (comboBox2.Text.ToString().Trim() == "选择题")
                {
                    mycon.Open();
                    string str = "delete from choice where id=" + int.Parse(textBox1.Text.Trim())
                        + " and subject='" + comboBox1.Text.ToString().Trim() + "'";
                    SqlCommand cmd = new SqlCommand(str, mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    this.Close();
                }
                if (comboBox2.Text.ToString().Trim() == "判断题")
                {
                    mycon.Open();
                    string str = "delete from judge where id=" + int.Parse(textBox1.Text.Trim())
                        + " and subject='" + comboBox1.Text.ToString().Trim() + "'";
                    SqlCommand cmd = new SqlCommand(str, mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    this.Close();
                }
                if (comboBox2.Text.ToString().Trim() == "填空题")
                {
                    mycon.Open();
                    string str = "delete from filling where id=" + int.Parse(textBox1.Text.Trim())
                        + " and subject='" + comboBox1.Text.ToString().Trim() + "'";
                    SqlCommand cmd = new SqlCommand(str, mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    this.Close();
                }
            }
            else if (flag == 3)//修改
            {
                if (comboBox2.Text.ToString().Trim() == "选择题")
                {
                    mycon.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("Select * From choice where id=" + int.Parse(textBox1.Text.Trim()) + " and subject='" + comboBox1.Text.ToString().Trim() + "'", mycon);
                    DataSet Ds = new DataSet();
                    sda.Fill(Ds, "timu");
                    string str = "delete from choice where id=" + int.Parse(textBox1.Text.Trim())
                        + " and subject='" + comboBox1.Text.ToString().Trim() + "'";
                    SqlCommand cmd = new SqlCommand(str, mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    xuanze x = new xuanze(Ds.Tables["timu"].Rows[0].ItemArray[0].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[1].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[2].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[3].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[4].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[5].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[6].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[7].ToString().Trim());
                    x.ShowDialog(this);
                    this.Close();
                }
                if (comboBox2.Text.ToString().Trim() == "判断题")
                {
                    mycon.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("Select * From judge where id=" + int.Parse(textBox1.Text.Trim()) + " and subject='" + comboBox1.Text.ToString().Trim() + "'", mycon);
                    DataSet Ds = new DataSet();
                    sda.Fill(Ds, "timu");
                    string str = "delete from judge where id=" + int.Parse(textBox1.Text.Trim())
                        + " and subject='" + comboBox1.Text.ToString().Trim() + "'";
                    SqlCommand cmd = new SqlCommand(str, mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    judge x = new judge(1,Ds.Tables["timu"].Rows[0].ItemArray[0].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[1].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[2].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[3].ToString().Trim());
                    x.ShowDialog(this);
                    this.Close();
                }
                if (comboBox2.Text.ToString().Trim() == "填空题")
                {
                    mycon.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("Select * From filling where id=" + int.Parse(textBox1.Text.Trim()) + " and subject='" + comboBox1.Text.ToString().Trim() + "'", mycon);
                    DataSet Ds = new DataSet();
                    sda.Fill(Ds, "timu");
                    string str = "delete from filling where id=" + int.Parse(textBox1.Text.Trim())
                        + " and subject='" + comboBox1.Text.ToString().Trim() + "'";
                    SqlCommand cmd = new SqlCommand(str, mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    judge x = new judge(2, Ds.Tables["timu"].Rows[0].ItemArray[0].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[1].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[2].ToString().Trim(),
                        Ds.Tables["timu"].Rows[0].ItemArray[3].ToString().Trim());
                    x.ShowDialog(this);
                    this.Close();
                }
            }
        }
    }
}
