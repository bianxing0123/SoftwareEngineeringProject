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
            mycon.ConnectionString = "Data Source=DESKTOP-E28V9KP\\SQLEXPRESS;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=sa.123";
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
            int[] a=new int[5]{0,0,0,0,0};
            label1.Visible = true;
            comboBox1.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            string str = "";
            string[] sub=new string[5]{"c#","软件测试","图像处理","图形学","嵌入式"};
            DataSet Ds = new DataSet();
            for (int i = 0; i < 5;++i )
            {
                SqlDataAdapter da0 = new SqlDataAdapter("select * from choice where subject='"+sub[i]+"'",mycon);
                da0.Fill(Ds, sub[i]);
            }
            switch (comboBox1.Text.ToString().Trim())
            {
                case "": a[0] = 1; a[1] = 1; a[2] = 1; a[3] = 1; a[4] = 1; break;
                case "c#": a[0] = 1; break;
                case "软件测试": a[1] = 1; break;
                case "图像处理": a[2] = 1; break;
                case "图形学": a[3] = 1; break;
                case "嵌入式": a[4] = 1; break;
            }
            str += "---------选择题---------\r\n";
            for (int j = 0; j < 5; ++j)
            {
                if (a[j] == 0)
                    continue;
                for (int i = 0; i < Ds.Tables[sub[j]].Rows.Count; ++i)
                {
                    str += Ds.Tables[sub[j]].Rows[i].ItemArray[0];
                    str += " ";
                    str += Ds.Tables[sub[j]].Rows[i].ItemArray[1];
                    str += ".";
                    str += Ds.Tables[sub[j]].Rows[i].ItemArray[2];
                    str += "\r\n";
                    str += Ds.Tables[sub[j]].Rows[i].ItemArray[3];
                    str += "\r\n";
                    str += Ds.Tables[sub[j]].Rows[i].ItemArray[4];
                    str += "\r\n";
                    str += Ds.Tables[sub[j]].Rows[i].ItemArray[5];
                    str += "\r\n";
                    str += Ds.Tables[sub[j]].Rows[i].ItemArray[6];
                    str += "\r\n";
                    str += "正确答案:";
                    str += Ds.Tables[sub[j]].Rows[i].ItemArray[7];
                    str += "\r\n";
                    str += "\r\n";
                }
            }
            //判断题
            DataSet ds1 = new DataSet();
            for (int i = 0; i < 5; ++i)
            {
                SqlDataAdapter da0 = new SqlDataAdapter("select * from judge where subject='" + sub[i] + "'", mycon);
                da0.Fill(ds1, sub[i]);
            }
            str += "---------判断题---------\r\n";
            for (int j = 0; j < 5; ++j)
            {
                if (a[j] == 0)
                    continue;
                for (int i = 0; i < ds1.Tables[sub[j]].Rows.Count; ++i)
                {
                    str += ds1.Tables[sub[j]].Rows[i].ItemArray[0];
                    str += " ";
                    str += ds1.Tables[sub[j]].Rows[i].ItemArray[1];
                    str += ".";
                    str += ds1.Tables[sub[j]].Rows[i].ItemArray[2];
                    str += " ";
                    str += ds1.Tables[sub[j]].Rows[i].ItemArray[3];
                    str += "\r\n";
                    str += "\r\n";
                }
            }
            //填空题
            DataSet ds2 = new DataSet();
            for (int i = 0; i < 5; ++i)
            {
                SqlDataAdapter da0 = new SqlDataAdapter("select * from filling where subject='" + sub[i] + "'", mycon);
                da0.Fill(ds2, sub[i]);
            }
            str += "---------填空题---------\r\n";
            for (int j = 0; j < 5; ++j)
            {
                if (a[j] == 0)
                    continue;
                for (int i = 0; i < ds2.Tables[sub[j]].Rows.Count; ++i)
                {
                    str += ds2.Tables[sub[j]].Rows[i].ItemArray[0];
                    str += " ";
                    str += ds2.Tables[sub[j]].Rows[i].ItemArray[1];
                    str += ".";
                    str += ds2.Tables[sub[j]].Rows[i].ItemArray[2];
                    str += " ";
                    str += ds2.Tables[sub[j]].Rows[i].ItemArray[3];
                    str += "\r\n";
                    str += "\r\n";
                }
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
