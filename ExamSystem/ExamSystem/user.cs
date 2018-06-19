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
        string NAME = "";
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
            mycon.ConnectionString = "Data Source=DESKTOP-E28V9KP\\SQLEXPRESS;Initial Catalog=ExamSystem;Persist Security Info=True;User ID=sa;Password=sa.123";
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
            NAME = Ds.Tables[0].Rows[0][0].ToString().Trim();
            toolStripLabel2.Text = "姓名：" + NAME;
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

            for (int i = 0; i < 25; i++)
            {
                RChoice[i] = new res(1,"",1);
            }
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
                交卷_Click(交卷, new EventArgs());
               
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
                SqlDataAdapter sda2 = new SqlDataAdapter("Select * From filling where subject = '" + comboBox1.Text.Trim() + "'", mycon);
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
                answer = textBox1.Text.Trim();
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
                if (RChoice[QNum].getans() != "")
                {
                    if (RChoice[QNum].getans() == "true")
                        正确.Checked = true;
                    if (RChoice[QNum].getans() == "false")
                        错误.Checked = true;
                }
            }
            //显示下一道填空题
            else if (QNum < 25)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From filling where subject = '" + comboBox1.Text.Trim() + "' and id = '" + aryfilling[QNum - 20] + "'", mycon);
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
                answer = textBox1.Text.Trim();
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
                SqlDataAdapter da = new SqlDataAdapter("Select * From filling where subject = '" + comboBox1.Text.Trim() + "' and id = '" + aryjudge[QNum - 20] + "'", mycon);
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
            //若该题未点击上下题按钮则该题答案未录入，需手动
            string answer1 = "";
            //获取选择题题号,答案与数据库中id对象
            if (QNum < 10)
            {
                if (选项A.Checked)
                    answer1 = "A";
                if (选项B.Checked)
                    answer1 = "B";
                if (选项C.Checked)
                    answer1 = "C";
                if (选项D.Checked)
                    answer1 = "D";
                RChoice[QNum] = new res(QNum + 1, answer1, arychoice[QNum]);

            }
            //获取判断题题号,答案与数据库中id对象
            else if (QNum < 20)
            {
                if (正确.Checked)
                    answer1 = "true";
                if (错误.Checked)
                    answer1 = "false";
                RChoice[QNum] = new res(QNum + 1, answer1, aryjudge[QNum - 10]);

            }
            //获取填空题题号,答案与数据库中id对象
            else if (QNum < 25)
            {
                answer1 = textBox1.Text.Trim();
                RChoice[QNum] = new res(QNum + 1, answer1, aryfilling[QNum - 20]);

            }
            //计算分数
            int score = 0;
            for (int i = 0; i < 10; i++)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From choice where subject = '" + comboBox1.Text + "' and id = '" + arychoice[i] + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "choice");
                if (RChoice[i].getans().Trim() == ds.Tables[0].Rows[0][7].ToString().Trim())
                {
                    score += 4;
                }
            }
            for (int i = 10; i < 20; i++)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From judge where subject = '" + comboBox1.Text + "' and id = '" + aryjudge[i - 10] + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "judge");
                if (RChoice[i].getans().Trim() == ds.Tables[0].Rows[0][3].ToString().Trim())
                {
                    score += 4;
                }
            }
            for (int i = 20; i < 25; i++)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From filling where subject = '" + comboBox1.Text.Trim() + "' and id = '" + aryfilling[i - 20] + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "filling");
                if (RChoice[i].getans().Trim() == ds.Tables[0].Rows[0][3].ToString().Trim())
                {
                    score += 4;
                }
            }
            MessageBox.Show("恭喜您！本次考试获得"+score+"分");
            //插入记录
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            mycon.Open();
            SqlDataAdapter da0 = new SqlDataAdapter("select * from score where ID=" + ID + " and subject='" + comboBox1.Text.ToString().Trim()+"'", mycon);
            DataSet ds0 = new DataSet();
            da0.Fill(ds0,"score");
            if (ds0.Tables["score"].Rows.Count == 0)
            {
                SqlCommand cmd = new SqlCommand("insert into score(ID, Name, subject,score,date) values ('" + ID + "', '" + NAME + "', '" + comboBox1.Text + "', '" + score + "', '" + date + "')", mycon);
                cmd.ExecuteNonQuery();
                mycon.Close();
            }
            else
            {
                if (score > int.Parse(ds0.Tables["score"].Rows[0].ItemArray[3].ToString().Trim()))
                {
                    SqlCommand cmd = new SqlCommand("update score set score=" + score + " where ID=" + ID + " and subject='" + comboBox1.Text.ToString().Trim() + "'", mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                }
            }

            //导出试卷及标准答案
            string myanswer = NAME+"的答案：\r\n";
            int k = 1;
            foreach (res s in RChoice)
            {
                myanswer += k+"."+s.getans()+"\r\n";
                k++;
            }
            string num = DateTime.Now.Millisecond.ToString();
            string textname = comboBox1.Text + "试卷" + num;
            string ExamPaper = "                         " + comboBox1.Text + "试题\r\n\r\n\r\n"+"一.选择题"+"\r\n";
            string answer = "标准答案：\r\n";
            for (int j = 0; j < 10; j++)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From choice where subject = '" + comboBox1.Text + "' and id = '" + arychoice[j] + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "choice");
                ExamPaper += (j + 1).ToString() + "." + ds.Tables[0].Rows[0][2].ToString().Trim() + "\r\n" + ds.Tables[0].Rows[0][3].ToString().Trim() + "\r\n" + ds.Tables[0].Rows[0][4].ToString().Trim() + "\r\n" + ds.Tables[0].Rows[0][5].ToString().Trim() + "\r\n" + ds.Tables[0].Rows[0][6].ToString().Trim() + "\r\n\r\n";
                answer += (j + 1).ToString() + "." + ds.Tables[0].Rows[0][7].ToString().Trim()+" ";
            }
            answer += "\r\n";
            ExamPaper +="\r\n"+"二.判断题" + "\r\n";
            for (int j = 10; j < 20; j++)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From judge where subject = '" + comboBox1.Text + "' and id = '" + aryjudge[j - 10] + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "judge");
                ExamPaper += (j + 1).ToString() + "." + ds.Tables[0].Rows[0][2].ToString().Trim() + "\r\n\r\n";
                answer += (j + 1).ToString() + "." + ds.Tables[0].Rows[0][3].ToString().Trim()+" ";
            }
            answer += "\r\n";
            ExamPaper += "\r\n" + "三.填空题" + "\r\n";
            for (int j = 20; j < 25; j++)
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From filling where subject = '" + comboBox1.Text + "' and id = '" + aryfilling[j - 20] + "'", mycon);
                DataSet ds = new DataSet();
                da.Fill(ds, "filling");
                ExamPaper += (j + 1).ToString() + "." + ds.Tables[0].Rows[0][2].ToString().Trim() + "\r\n\r\n";
                answer += (j + 1).ToString() + "." + ds.Tables[0].Rows[0][3].ToString().Trim() + " ";
            }
            ExamPaper += "\r\n" + answer;
            ExamPaper += "\r\n" + myanswer; 
            System.IO.File.WriteAllText("E:\\SoftwareEngineeringProject\\ExamSystem\\ExamSystem\\生成试卷\\"+textname+".txt",ExamPaper);
            Application.Exit(); 
        }

    }
}
