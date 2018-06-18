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
    public partial class temp2 : Form
    {
        int f1;//f1删除或修改 f2学生或成绩
        SqlConnection mycon = new SqlConnection();
        public temp2(int f1)
        {
            InitializeComponent();
            mycon.ConnectionString = "Data Source=GWO-20140219FWK;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=123456";
            this.f1 = f1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (f1 == 0 )//删除学生
            {
                mycon.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from student where ID='" + textBox1.Text.ToString().Trim() + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "student");
                if (ds.Tables["student"].Rows.Count > 0)
                {
                    SqlCommand cmd = new SqlCommand("delete from student where ID='" + textBox1.Text.ToString().Trim() + "'", mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    this.Close();
                }
                else
                {
                    mycon.Close();
                    MessageBox.Show("学生不存在");
                }
            }
            if (f1 == 1)//修改学生
            {
                mycon.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from student where ID='" + textBox1.Text.ToString().Trim() + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "student");
                if (ds.Tables["student"].Rows.Count > 0)
                {
                    //修改
                    student s = new student(1, ds.Tables["student"].Rows[0].ItemArray[0].ToString(), ds.Tables["student"].Rows[0].ItemArray[1].ToString(), ds.Tables["student"].Rows[0].ItemArray[2].ToString(), ds.Tables["student"].Rows[0].ItemArray[3].ToString());
                    s.ShowDialog(this);
                    this.Close();
                }
                else
                {
                    mycon.Close();
                    MessageBox.Show("学生不存在");
                }
            }
        }
    }
}
