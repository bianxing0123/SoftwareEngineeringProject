using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;

namespace ExamSystem
{
    
    public partial class user : Form
    {
        //题号，答案，数据库中对应科目id存储类
        public class res
        {
            private int num;
            private string ans;
            private int id;
            public res(int num, string ans,int id)
            {
                this.num = num;
                this.ans = ans;
                this.id = id;
            }
            public int getnum()
            {
                return num;
            }
            public string getans()
            {
                return ans;
            }
            public int getid()
            {
                return id;
            }
        }

        string ID = "";
        int minute = 60;
        int second = 0;
        bool ExamEnd = false;

        //题号
        int QNum = 0;

        //被标记题号数组
        List<int> sign = new List<int>();

        //选择题题号数组
        List<int> arychoice = new List<int>();

        //判断题题号数组
        List<int> aryjudge = new List<int>();

        //填空题题号数组
        List<int> aryfilling = new List<int>();

        //题号,答案,id对象数组
        res[] RChoice = new res[25];

        SqlConnection mycon = new SqlConnection();
        public user()
        {
            InitializeComponent();
            mycon.ConnectionString = "Data Source=HWH-PC\\SQLEXPRESS;Initial Catalog=ExamSystem;User ID=sa;Password=123456";
        }

        public user(string id)
            : this()
        {           
            ID = id;
        }

        

        private void user_Load(object sender, EventArgs e)
        {
            this.Owner.Hide();
            toolStripLabel1.Text = "学号：" + ID;

            SqlDataAdapter sda = new SqlDataAdapter("Select Name From student where ID = '"+ID+"'", mycon);
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "student");
            toolStripLabel2.Text = "姓名：" + Ds.Tables[0].Rows[0][0].ToString().Trim();
            选项A.Visible = true;
            选项B.Visible = true;
            选项C.Visible = true;
            选项D.Visible = true;
            正确.Visible = false;
            错误.Visible = false;
            textBox1.Visible = false;

            上一题.Enabled = false;
            下一题.Enabled = false;
            标记.Enabled = false;
            交卷.Enabled = false;
            选项A.Enabled = false;
            选项B.Enabled = false;
            选项C.Enabled = false;
            选项D.Enabled = false;
            正确.Enabled = false;
            错误.Enabled = false;
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            string min = "";
            string sec = "";
            if (minute<10)
            {
                min = "0" + minute.ToString();
            }
            else
                min = minute.ToString();
            if (second<10)
            {
                sec = "0" + second.ToString();
            }
            else
                sec= second.ToString();
            toolStripLabel4.Text = min + ":" + sec;
            if (second == 0 && minute != 0)
            {
                second = 60;
                minute--;
            }
            if (minute == 0 && second == 0)
                ExamEnd = true;
            second--;
            if (ExamEnd == true)
            {
                timer1.Stop();
                MessageBox.Show("考试时间结束，试卷已提交");
            }
        }

        private void 出题_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
                MessageBox.Show("请选择考试科目！");
            else
            {
                MessageBox.Show(comboBox1.Text+"考试开始，请在规定时间内完成考试，严禁作弊！");
                timer1.Start();
                出题.Enabled = false;
                上一题.Enabled = true;
                下一题.Enabled = true;
                标记.Enabled = true;
                交卷.Enabled = true;
                选项A.Enabled = true;
                选项B.Enabled = true;
                选项C.Enabled = true;
                选项D.Enabled = true;
                正确.Enabled = true;
                错误.Enabled = true;
                comboBox1.Enabled = false;


                //随机选择题id存入List
                SqlDataAdapter sda = new SqlDataAdapter("Select * From choice where subject = '" + comboBox1.Text + "'", mycon);
                DataSet Ds = new DataSet();
                sda.Fill(Ds, "choice"); 
                Random rd = new Random();
                while (arychoice.Count != 10)
                {
                    bool flag = false;
                    int number = rd.Next(1, Ds.Tables[0].Rows.Count + 1);
                    foreach(int n in arychoice)
                    {
                        if (n == number)
                            flag = true;
                    }
                    if (flag == false)
                        arychoice.Add(number);
                }
                arychoice.Sort();

                //随机判断题id存入List
                SqlDataAdapter sda1 = new SqlDataAdapter("Select * From judge where subject = '" + comboBox1.Text + "'", mycon);
                DataSet Ds1 = new DataSet();
                sda1.Fill(Ds1, "judge"); 
                while (aryjudge.Count != 10)
                {
                    bool flag = false;
                    int number = rd.Next(1, Ds1.Tables[0].Rows.Count + 1);
                    foreach (int n in aryjudge)
                    {
                        if (n == number)
                            flag = true;
                    }
                    if (flag == false)
                        aryjudge.Add(number);
                }
                aryjudge.Sort();

                //随机填空题id存入List
                SqlDataAdapter sda2 = new SqlDataAdapter("Select * From filling where subject = '" + comboBox1.Text + "'", mycon);
                DataSet Ds2 = new DataSet();
                sda2.Fill(Ds2, "filling");
                while (aryfilling.Count != 5)
                {
                    bool flag = false;
                    int number = rd.Next(1, Ds2.Tables[0].Rows.Count + 1);
                    foreach (int n in aryfilling)
                    {
                        if (n == number)
                            flag = true;
                    }
                    if (flag == false)
                        aryfilling.Add(number);
                }
                aryfilling.Sort();

                //显示初始的第一道题
                题目1.BackColor = ColorTranslator.FromHtml("#00FF00"); 
                SqlDataAdapter da = new SqlDataAdapter("Select * From choice where subject = '" + comboBox1.Text + "' and id = '"+arychoice[0]+"'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "choice");
                label1.Text = "1."+ds.Tables[0].Rows[0][2].ToString().Trim() + "\n" + ds.Tables[0].Rows[0][3].ToString().Trim() + "\n" + ds.Tables[0].Rows[0][4].ToString().Trim() + "\n" + ds.Tables[0].Rows[0][5].ToString().Trim() + "\n" + ds.Tables[0].Rows[0][6].ToString().Trim();
                
            }
        }

        private void 下一题_Click(object sender, EventArgs e)
        {
            
            string answer = "";
            //获取选择题题号,答案与数据库中id对象
            if (QNum < 10)
            {
                if (选项A.Checked)
                    answer = "A";
                if (选项B.Checked)
                    answer = "B";
                if (选项C.Checked)
                    answer = "C";
                if (选项D.Checked)
                    answer = "D";
                RChoice[QNum] = new res(QNum + 1, answer,arychoice[QNum]);
                
            }
            //获取判断题题号,答案与数据库中id对象
            else if (QNum < 20)
            {
                if(正确.Checked)
                    answer = "true";
                if (错误.Checked)
                    answer = "false";
                RChoice[QNum] = new res(QNum + 1, answer, aryjudge[QNum-10]);
                
            }
            //获取填空题题号,答案与数据库中id对象
            else if (QNum < 25)
            {
                answer = textBox1.Text;
                RChoice[QNum] = new res(QNum + 1, answer, aryfilling[QNum - 20]);
                
            }

            选项A.Checked = false;
            选项B.Checked = false;
            选项C.Checked = false;
            选项D.Checked = false;
            正确.Checked = false;
            错误.Checked = false;
            textBox1.Text = "";

            if(QNum<24)
                QNum++;


            //显示下一道选择题
            if (QNum < 10)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From choice where subject = '" + comboBox1.Text + "' and id = '" + arychoice[QNum] + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "choice");
                label1.Text = (QNum+1).ToString()+"."+ds.Tables[0].Rows[0][2].ToString().Trim() + "\n" + ds.Tables[0].Rows[0][3].ToString().Trim() + "\n" + ds.Tables[0].Rows[0][4].ToString().Trim() + "\n" + ds.Tables[0].Rows[0][5].ToString().Trim() + "\n" + ds.Tables[0].Rows[0][6].ToString().Trim();
                选项A.Visible = true;
                选项B.Visible = true;
                选项C.Visible = true;
                选项D.Visible = true;
                正确.Visible = false;
                错误.Visible = false;
                textBox1.Visible = false;
                
            }
            //显示下一道判断题
            else if (QNum < 20)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From judge where subject = '" + comboBox1.Text + "' and id = '" + aryjudge[QNum-10] + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "judge");
                label1.Text = (QNum+1).ToString() +"."+ ds.Tables[0].Rows[0][2].ToString().Trim();
                选项A.Visible = false;
                选项B.Visible = false;
                选项C.Visible = false;
                选项D.Visible = false;
                正确.Visible = true;
                错误.Visible = true;
                textBox1.Visible = false;
                
            }
            //显示下一道填空题
            else if (QNum < 25)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From filling where subject = '" + comboBox1.Text + "' and id = '" + aryjudge[QNum - 20] + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "filling");
                label1.Text = (QNum + 1).ToString() + "." + ds.Tables[0].Rows[0][2].ToString().Trim();
                选项A.Visible = false;
                选项B.Visible = false;
                选项C.Visible = false;
                选项D.Visible = false;
                正确.Visible = false;
                错误.Visible = false;
                textBox1.Visible = true;
                
            }
            foreach (Control cc in groupBox1.Controls)
            {               
                if (cc.Text.Trim() == (QNum+1).ToString().Trim())
                    cc.BackColor = ColorTranslator.FromHtml("#00FF00");
                else
                    cc.BackColor = Control.DefaultBackColor;
                foreach (int n in sign)
                {
                    if (n.ToString().Trim() == cc.Text.Trim())
                        cc.BackColor = ColorTranslator.FromHtml("#0000FF");
                }
            }
            foreach (Control cc in groupBox2.Controls)
            {
                if (cc.Text.Trim() == (QNum + 1).ToString().Trim())
                    cc.BackColor = ColorTranslator.FromHtml("#00FF00");
                else
                    cc.BackColor = Control.DefaultBackColor;
                foreach (int n in sign)
                {
                    if (n.ToString().Trim() == cc.Text.Trim())
                        cc.BackColor = ColorTranslator.FromHtml("#0000FF");
                }
            }
            foreach (Control cc in groupBox3.Controls)
            {
                if (cc.Text.Trim() == (QNum + 1).ToString().Trim())
                    cc.BackColor = ColorTranslator.FromHtml("#00FF00");
                else
                    cc.BackColor = Control.DefaultBackColor;
                foreach (int n in sign)
                {
                    if (n.ToString().Trim() == cc.Text.Trim())
                        cc.BackColor = ColorTranslator.FromHtml("#0000FF");
                }
            }
        }

        private void 上一题_Click(object sender, EventArgs e)
        {
            
            string answer = "";
            //获取选择题题号,答案与数据库中id对象
            if (QNum < 10)
            {
                if (选项A.Checked)
                    answer = "A";
                if (选项B.Checked)
                    answer = "B";
                if (选项C.Checked)
                    answer = "C";
                if (选项D.Checked)
                    answer = "D";
                RChoice[QNum] = new res(QNum + 1, answer, arychoice[QNum]);
                
            }
            //获取判断题题号,答案与数据库中id对象
            else if (QNum < 20)
            {
                if (正确.Checked)
                    answer = "true";
                if (错误.Checked)
                    answer = "false";
                RChoice[QNum] = new res(QNum + 1, answer, aryjudge[QNum - 10]);
                
            }
            //获取填空题题号,答案与数据库中id对象
            else if (QNum < 25)
            {
                answer = textBox1.Text;
                RChoice[QNum] = new res(QNum + 1, answer, aryfilling[QNum - 20]);
                
            }

            选项A.Checked = false;
            选项B.Checked = false;
            选项C.Checked = false;
            选项D.Checked = false;
            正确.Checked = false;
            错误.Checked = false;
            textBox1.Text = "";

            if (QNum >0)
                QNum--;


            //显示上一道选择题
            if (QNum < 10)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From choice where subject = '" + comboBox1.Text + "' and id = '" + arychoice[QNum] + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "choice");
                label1.Text = (QNum + 1).ToString() + "." + ds.Tables[0].Rows[0][2].ToString().Trim() + "\n" + ds.Tables[0].Rows[0][3].ToString().Trim() + "\n" + ds.Tables[0].Rows[0][4].ToString().Trim() + "\n" + ds.Tables[0].Rows[0][5].ToString().Trim() + "\n" + ds.Tables[0].Rows[0][6].ToString().Trim();
                选项A.Visible = true;
                选项B.Visible = true;
                选项C.Visible = true;
                选项D.Visible = true;
                正确.Visible = false;
                错误.Visible = false;
                textBox1.Visible = false;
                if (RChoice[QNum].getans() != "")
                {
                    if (RChoice[QNum].getans() == "A")
                        选项A.Checked = true;
                    if (RChoice[QNum].getans() == "B")
                        选项B.Checked = true;
                    if (RChoice[QNum].getans() == "C")
                        选项C.Checked = true;
                    if (RChoice[QNum].getans() == "D")
                        选项D.Checked = true;
                }
            }
            //显示上一道判断题
            else if (QNum < 20)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From judge where subject = '" + comboBox1.Text + "' and id = '" + aryjudge[QNum - 10] + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "judge");
                label1.Text = (QNum + 1).ToString() + "." + ds.Tables[0].Rows[0][2].ToString().Trim();
                选项A.Visible = false;
                选项B.Visible = false;
                选项C.Visible = false;
                选项D.Visible = false;
                正确.Visible = true;
                错误.Visible = true;
                textBox1.Visible = false;
                if (RChoice[QNum].getans() != "")
                {
                    if (RChoice[QNum].getans() == "true")
                        正确.Checked = true;
                    if (RChoice[QNum].getans() == "false")
                        错误.Checked = true;                  
                }
            }
            //显示上一道填空题
            else if (QNum < 25)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From filling where subject = '" + comboBox1.Text + "' and id = '" + aryjudge[QNum - 20] + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "filling");
                label1.Text = (QNum + 1).ToString() + "." + ds.Tables[0].Rows[0][2].ToString().Trim();
                选项A.Visible = false;
                选项B.Visible = false;
                选项C.Visible = false;
                选项D.Visible = false;
                正确.Visible = false;
                错误.Visible = false;
                textBox1.Visible = true;
                if (RChoice[QNum].getans() != "")
                {
                    textBox1.Text = RChoice[QNum].getans();
                }
            }

            foreach (Control cc in groupBox1.Controls)
            {
                if (cc.Text.Trim() == (QNum + 1).ToString().Trim())
                    cc.BackColor = ColorTranslator.FromHtml("#00FF00");
                else
                    cc.BackColor = Control.DefaultBackColor;
                foreach (int n in sign)
                {
                    if (n.ToString().Trim() == cc.Text.Trim())
                        cc.BackColor = ColorTranslator.FromHtml("#0000FF");
                }
            }
            foreach (Control cc in groupBox2.Controls)
            {
                if (cc.Text.Trim() == (QNum + 1).ToString().Trim())
                    cc.BackColor = ColorTranslator.FromHtml("#00FF00");
                else
                    cc.BackColor = Control.DefaultBackColor;
                foreach (int n in sign)
                {
                    if (n.ToString().Trim() == cc.Text.Trim())
                        cc.BackColor = ColorTranslator.FromHtml("#0000FF");
                }
            }
            foreach (Control cc in groupBox3.Controls)
            {
                if (cc.Text.Trim() == (QNum + 1).ToString().Trim())
                    cc.BackColor = ColorTranslator.FromHtml("#00FF00");
                else
                    cc.BackColor = Control.DefaultBackColor;
                foreach (int n in sign)
                {
                    if (n.ToString().Trim() == cc.Text.Trim())
                        cc.BackColor = ColorTranslator.FromHtml("#0000FF");
                }
            }

        }

        private void 标记_Click(object sender, EventArgs e)
        {
            bool flag = false;
            foreach (int n in sign)
            {
                if (n == QNum + 1)
                    flag = true;         
            }
            if (flag == false)
                sign.Add(QNum + 1);
            else
                sign.Remove(QNum+1);
        }

        private void 交卷_Click(object sender, EventArgs e)
        {
            int score = 100;
            for (int i = 0; i < 10; i++)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From choice where subject = '" + comboBox1.Text + "' and id = '" + RChoice[i].getid() + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "choice");
                if (RChoice[i].getans() != ds.Tables[0].Rows[0][7].ToString().Trim())
                {
                    score -= 4;
                }
            }
            for (int i = 10; i < 20; i++)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From judge where subject = '" + comboBox1.Text + "' and id = '" + RChoice[i].getid() + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "judge");
                if (RChoice[i].getans() != ds.Tables[0].Rows[0][3].ToString().Trim())
                {
                    score -= 4;
                }
            }
            for (int i = 20; i < 25; i++)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From filling where subject = '" + comboBox1.Text + "' and id = '" + RChoice[i].getid() + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "filling");
                if (RChoice[i].getans() != ds.Tables[0].Rows[0][3].ToString().Trim())
                {
                    score -= 4;
                }
            }
            MessageBox.Show("恭喜您！本次考试获得"+score+"分");
            string str = "";
            foreach (res n in RChoice)
            {
                str +="题" +n.getnum()+" 我的答案:"+n.getans()+"id:"+n.getid()+"\n";
            }
            label1.Text = str;
        }

    }
}
