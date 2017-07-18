using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongIDE.Controller;
using xinLongIDE.Model.requestJson;

namespace xinLongIDE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User user = new User("wepartner", "wepartner123");
            BaseController bc = new BaseController();
            groupCreateRequest gcr = new groupCreateRequest("默认分组abc", "pc");
            bc.CreateGroup(gcr);
            //ConnectionController cc = new ConnectionController();
            //this.textBox1.Text = cc.GetServiceUrl();
            //datas ds = new datas("SELECT * FROM sly_test_phone GROUP BY city", "db");
            //testType tt = new testType("execute", ds);


            ////this.POST(_url, "");
            ////string jsontext = SerializeToJson(tt);

            ////testType te = DeSerializeToObject1(jsontext, typeof(testType)) as testType;

            //ConnectionController cc = new ConnectionController();
            //testType ty = cc.GetSqlResult(tt, typeof(testType)) as testType;

            //BaseController bc = new BaseController();
            //bc.Login();

            //this.textBox1.Text = JsonManager.SerializeToJson(tt);
        }

        public class testType : Model.requestJson.BaseRequestJson
        {
            public string api_type = string.Empty;
            datas data;
            public testType(string t, datas d)
            {
                api_type = t;
                data = d;
            }
        }

       
        public class datas
        {
            public string sql = string.Empty;
            public string type = string.Empty;
            public datas(string s, string t)
            {
                sql = s;
                type = t;
            }
        }
    }
}
