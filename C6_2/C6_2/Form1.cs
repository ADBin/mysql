using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace C6_2
{
    public partial class Form1 : Form
    {
        DataSet ds = new DataSet();
        int a;//,n;
        static string constructorString = "server=;User Id=;password=;Database=student;Charset=utf8";
        MySqlConnection myConnnect = new MySqlConnection(constructorString);
        private void sql_a()
        {
            ds.Clear();      
            MySqlCommand mycom = null;
            //MySqlDataAdapter myrec = null;
            mycom = myConnnect.CreateCommand();
            mycom.CommandText = "SELECT * FROM grade";
            MySqlDataAdapter adap = new MySqlDataAdapter(mycom);
            //DataSet ds = new DataSet();
            adap.Fill(ds);
            
            a = ds.Tables[0].Rows.Count;
            Console.WriteLine(a);
            //myConnnect.Close();
            adap.Dispose();
        }
        private void sql_search(int i)
        {
            //Console.WriteLine("..........");           
            toolStripTextBox1.Text = i.ToString();
            i -= 1;
            dataGridView1.DataSource = ds.Tables[0].DefaultView;

            string s_sid = ds.Tables[0].Rows[i]["sid"].ToString();
            string s_name = ds.Tables[0].Rows[i]["name"].ToString();
            string s_sex = ds.Tables[0].Rows[i]["sex"].ToString();
            string s_class = ds.Tables[0].Rows[i]["class"].ToString();
            string s_math = ds.Tables[0].Rows[i]["math"].ToString();
            string s_chinese = ds.Tables[0].Rows[i]["chinese"].ToString();
            string s_english = ds.Tables[0].Rows[i]["english"].ToString();

            s_sex = s_sex=="0" ? "女" : "男";

            
            textBox1.Text = s_sid;
            textBox2.Text = s_name;
            textBox3.Text = s_sex;
            textBox4.Text = s_class;
            textBox5.Text = s_math;
            textBox6.Text = s_chinese;
            textBox7.Text = s_english;
            
        }
        public Form1()
        {
            InitializeComponent();
            try
            {
                myConnnect.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("数据库连接失败！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sql_a();
            //n = a;
            toolStripLabel1.Text = "/"+a.ToString();
            sql_search(1);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)  //最前一个
        {            
            sql_search(1);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)  //向前一个
        {
            if (int.Parse(toolStripTextBox1.Text.ToString()) == 1)
                return;
            sql_search(int.Parse(toolStripTextBox1.Text.ToString()) - 1);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)  //向后一个
        {
            if (int.Parse(toolStripTextBox1.Text.ToString()) == a)
                return;
            sql_search(int.Parse(toolStripTextBox1.Text.ToString()) + 1);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)  //最后一个
        {
            //Console.WriteLine(a-1);
            sql_search(a);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)  //添加
        {            
            
            int sid = int.Parse(textBox1.Text);
            int sex = textBox3.Text == "男" ? 1 : 0;
            //Console.WriteLine(sid);
            //Console.WriteLine(textBox2.Text);
            //Console.WriteLine(sex);
            //Console.WriteLine(textBox4.Text);
            //Console.WriteLine(textBox5.Text);
            //Console.WriteLine(textBox6.Text);
            //Console.WriteLine(textBox7.Text);            
            //MySqlDataAdapter myrec = null;
            
            for (int i = 0; i < a; i++)//查重
            {
                if (sid == int.Parse(ds.Tables[0].Rows[i]["sid"].ToString()))
                {
                    MessageBox.Show("学号有重复！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }

            }
            MySqlCommand mycom = null;
            mycom = myConnnect.CreateCommand();
            try
            {
                if (textBox5.Text == "" && textBox6.Text == "" && textBox7.Text == "")
                {
                   // Console.WriteLine(".....");
                    //INSERT INTO grade (sid,name,sex,class,math,chinese,english) VALUES (@sid,@textBox2.Text,@sex,@textBox4.Text,@textBox5.Text,@textBox6.Text,@textBox7.Text)
                    mycom.CommandText = "INSERT INTO grade (sid,name,sex,class) VALUES (@sid,@name,@sex,@class)";
                    mycom.Parameters.AddWithValue("@sid", sid);
                    mycom.Parameters.AddWithValue("@name", textBox2.Text);
                    mycom.Parameters.AddWithValue("@sex", sex);
                    mycom.Parameters.AddWithValue("@class", textBox4.Text);
                    //Console.WriteLine("22222222");
                }
                else
                {
                    mycom.CommandText = "INSERT INTO grade (sid,name,sex,class,math,chinese,english) VALUES (@sid,@name,@sex,@class,@math,@chinese,@english)";

                    mycom.Parameters.AddWithValue("@sid", sid);
                    mycom.Parameters.AddWithValue("@name", textBox2.Text);
                    mycom.Parameters.AddWithValue("@sex", sex);
                    mycom.Parameters.AddWithValue("@class", textBox4.Text);
                    mycom.Parameters.AddWithValue("@math", double.Parse(textBox5.Text));
                    mycom.Parameters.AddWithValue("@chinese", double.Parse(textBox6.Text));
                    mycom.Parameters.AddWithValue("@english", double.Parse(textBox7.Text));    
                }
                //mycom = myConnnect.CreateCommand();
                //INSERT INTO grade (sid,name,sex,class,math,chinese,english) VALUES (@sid,@textBox2.Text,@sex,@textBox4.Text,@textBox5.Text,@textBox6.Text,@textBox7.Text)
                mycom.ExecuteNonQuery();
                MessageBox.Show("添加成功！", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                //myConnnect.Close();
                mycom.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("输入数据有误，请检查", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }


            sql_a();
            //a += 1;
            toolStripLabel1.Text = "/" + a.ToString();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)  //删除
        {
            DialogResult TS = MessageBox.Show("是否删除？？", "Waring", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (TS == DialogResult.No) return;
            MySqlCommand mycom = null;
            int sid = int.Parse(textBox1.Text);
            //Console.WriteLine(sid);
            //Console.WriteLine(textBox2.Text);
            ////Console.WriteLine(sex);
            //Console.WriteLine(textBox4.Text);
            //Console.WriteLine(textBox5.Text);
            //Console.WriteLine(textBox6.Text);
            //Console.WriteLine(textBox7.Text);
            //MySqlDataAdapter myrec = null;
            mycom = myConnnect.CreateCommand();
            try
            {
                mycom.CommandText = "DELETE FROM grade where sid = @sid";

                mycom.Parameters.AddWithValue("@sid", sid);

                mycom.ExecuteNonQuery();
                MessageBox.Show("删除成功！", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("错误，删除失败！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            mycom.Dispose();

            sql_a();
            //a -= 1;
            toolStripLabel1.Text = "/" + a.ToString();

            try
            {
                if (int.Parse(toolStripTextBox1.Text.ToString()) > a)
                {
                    sql_search(a);
                    return;
                }
                if (int.Parse(toolStripTextBox1.Text.ToString()) < 1)
                {
                    sql_search(1);
                    return;
                }
                //Console.WriteLine(toolStripTextBox1.Text.ToString());
                sql_search(int.Parse(toolStripTextBox1.Text.ToString()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)  //修改（保存）
        {
            MySqlCommand mycom = null;
            int sid = int.Parse(textBox1.Text);
            int sex = textBox3.Text == "男" ? 1 : 0;
            //Console.WriteLine(sid);
            //Console.WriteLine(textBox2.Text);
            //Console.WriteLine(sex);
            //Console.WriteLine(textBox4.Text);
            //Console.WriteLine(textBox5.Text);
            //Console.WriteLine(textBox6.Text);
            //Console.WriteLine(textBox7.Text);
            //MySqlDataAdapter myrec = null;
            mycom = myConnnect.CreateCommand();
             try
            { 
                if (textBox5.Text == "" && textBox6.Text == "" && textBox7.Text == "")
                {
                    mycom.CommandText = "UPDATE grade set name=@name,sex=@sex,class=@class where sid = @sid";
                    mycom.Parameters.AddWithValue("@sid", sid);
                    mycom.Parameters.AddWithValue("@name", textBox2.Text);
                    mycom.Parameters.AddWithValue("@sex", sex);
                    mycom.Parameters.AddWithValue("@class", textBox4.Text);
                }
                else
                {
                    mycom.CommandText = "UPDATE grade set name=@name,sex=@sex,class=@class,math=@math,chinese=@chinese,english=@english where sid = @sid";
                    mycom.Parameters.AddWithValue("@sid", sid);
                    mycom.Parameters.AddWithValue("@name", textBox2.Text);
                    mycom.Parameters.AddWithValue("@sex", sex);
                    mycom.Parameters.AddWithValue("@class", textBox4.Text);
                    mycom.Parameters.AddWithValue("@math", double.Parse(textBox5.Text));
                    mycom.Parameters.AddWithValue("@chinese", double.Parse(textBox6.Text));
                    mycom.Parameters.AddWithValue("@english", double.Parse(textBox7.Text));
                }
              
                mycom.ExecuteNonQuery();
                //myConnnect.Close();
                mycom.Dispose();
               
                MessageBox.Show("保存成功！", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入数据有误，请检查", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Console.WriteLine(ex);
            }
            
            mycom.Dispose();

            sql_a();
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(toolStripTextBox1.Text.ToString()) > a)
                {
                    sql_search(a);
                    return;
                }
                if (int.Parse(toolStripTextBox1.Text.ToString()) < 1)
                {
                    sql_search(1);
                    return;
                }
                //Console.WriteLine(toolStripTextBox1.Text.ToString());
                sql_search(int.Parse(toolStripTextBox1.Text.ToString()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }        
    }
}
