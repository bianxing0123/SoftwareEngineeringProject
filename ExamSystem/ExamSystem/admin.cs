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
    public partial class admin : Form
    {
        SqlConnection mycon = new SqlConnection();
        int flag;
        public admin()
        {
            InitializeComponent();
            mycon.ConnectionString = "Data Source=GWO-20140219FWK;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=123456";
            showtiku();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            showtiku();
        }
        public void showtiku()
        {
            //选择题
            flag = 1;
            label1.Visible = true;
            comboBox1.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            string str = "";
            SqlCommand cmd;
            if (comboBox1.Text.ToString().Trim() == "")
            {
                cmd = new SqlCommand("Select * From choice", mycon);
            }
            else
                cmd = new SqlCommand("Select * From choice where subject='" + comboBox1.Text.ToString().Trim() + "'", mycon);
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd;
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "choice");
            str += "---------选择题---------\r\n";
            for (int i = 0; i < Ds.Tables["choice"].Rows.Count; ++i)
            {
                str += Ds.Tables["choice"].Rows[i].ItemArray[0];
                str += " ";
                str += Ds.Tables["choice"].Rows[i].ItemArray[1];
                str += ".";
                str += Ds.Tables["choice"].Rows[i].ItemArray[2];
                str += "\r\n";
                str += Ds.Tables["choice"].Rows[i].ItemArray[3];
                str += "\r\n";
                str += Ds.Tables["choice"].Rows[i].ItemArray[4];
                str += "\r\n";
                str += Ds.Tables["choice"].Rows[i].ItemArray[5];
                str += "\r\n";
                str += Ds.Tables["choice"].Rows[i].ItemArray[6];
                str += "\r\n";
                str += "正确答案:";
                str += Ds.Tables["choice"].Rows[i].ItemArray[7];
                str += "\r\n";
            }
            //判断题
            SqlCommand cmd1;
            if (comboBox1.Text.ToString().Trim() == "")
            {
                cmd1 = new SqlCommand("Select * From judge", mycon);
            }
            else
                cmd1 = new SqlCommand("Select * From judge where subject='" + comboBox1.Text.ToString().Trim() + "'", mycon);
            sda.SelectCommand = cmd1;
            sda.Fill(Ds, "judge");
            str += "---------判断题---------\r\n";
            for (int i = 0; i < Ds.Tables["judge"].Rows.Count; ++i)
            {
                str += Ds.Tables["judge"].Rows[i].ItemArray[0];
                str += " ";
                str += Ds.Tables["judge"].Rows[i].ItemArray[1];
                str += ".";
                str += Ds.Tables["judge"].Rows[i].ItemArray[2];
                str += " ";
                str += Ds.Tables["judge"].Rows[i].ItemArray[3];
                str += "\r\n";
            }
            //填空题
            SqlCommand cmd2;
            if (comboBox1.Text.ToString().Trim() == "")
            {
                cmd2 = new SqlCommand("Select * From filling", mycon);
            }
            else
                cmd2 = new SqlCommand("Select * From filling where subject='" + comboBox1.Text.ToString().Trim() + "'", mycon);
            sda.SelectCommand = cmd2;
            sda.Fill(Ds, "filling");
            str += "---------填空题---------\r\n";
            for (int i = 0; i < Ds.Tables["filling"].Rows.Count; ++i)
            {
                str += Ds.Tables["filling"].Rows[i].ItemArray[0];
                str += " ";
                str += Ds.Tables["filling"].Rows[i].ItemArray[1];
                str += ".";
                str += Ds.Tables["filling"].Rows[i].ItemArray[2];
                str += " ";
                str += Ds.Tables["filling"].Rows[i].ItemArray[3];
                str += "\r\n";
            }
            textBox1.Text = str;
            textBox1.Select(0, 0);
        }

        public void showxuesheng()
        {
            //学生信息
            flag = 2;
            label1.Visible = false;
            comboBox1.Visible = false;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            string str = "";
            SqlDataAdapter sda = new SqlDataAdapter("Select * From student", mycon);
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "student");
            str += "     学号         姓名       密码        邮箱\r\n";
            for (int i = 0; i < Ds.Tables["student"].Rows.Count; ++i)
            {
                str += Ds.Tables["student"].Rows[i].ItemArray[0];
                str += "     ";
                str += Ds.Tables["student"].Rows[i].ItemArray[1];
                str += "     ";
                str += Ds.Tables["student"].Rows[i].ItemArray[2];
                str += "     ";
                str += Ds.Tables["student"].Rows[i].ItemArray[3];
                str += "\r\n";
            }
            textBox1.Text = str;
            textBox1.Select(0, 0);
        }

        public void showchengji()
        {
            //成绩信息
            flag = 3;
            label1.Visible = false;
            comboBox1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            string str = "";
            SqlDataAdapter sda = new SqlDataAdapter("Select * From score", mycon);
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "score");
            str += "     学号         姓名       科目        成绩     日期\r\n";
            for (int i = 0; i < Ds.Tables["score"].Rows.Count; ++i)
            {
                str += Ds.Tables["score"].Rows[i].ItemArray[0];
                str += "     ";
                str += Ds.Tables["score"].Rows[i].ItemArray[1];
                str += "       ";
                str += Ds.Tables["score"].Rows[i].ItemArray[2];
                str += "          ";
                str += Ds.Tables["score"].Rows[i].ItemArray[3];
                str += "    ";
                str += Ds.Tables["score"].Rows[i].ItemArray[4];
                str += "\r\n";
            }
            textBox1.Text = str;
            textBox1.Select(0, 0);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            showxuesheng();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            showchengji();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {//增加
            if (flag == 1) 
            {//题库
                temp t = new temp(1);
                t.ShowDialog();
                showtiku();
            }
            if (flag == 2)
            {
                student s = new student(0);
                s.ShowDialog();
                showxuesheng();
            }
        }

        private void admin_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {//删除
            if (flag == 1)
            {//题库
                temp t = new temp(2);
                t.ShowDialog();
                showtiku();
            }
            if (flag == 2)
            {//学生
                temp2 t = new temp2(0);
                t.ShowDialog();
                showxuesheng();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {//修改
            if (flag == 1)
            {//题库
                temp t = new temp(3);
                t.ShowDialog();
                showtiku();
            }
            if (flag == 2)
            {//学生
                temp2 t = new temp2(1);
                t.ShowDialog();
                showxuesheng();
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            showtiku();
        }
    }
}
