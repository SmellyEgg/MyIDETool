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
            //ConnectionController cc = new ConnectionController();
            //this.textBox1.Text = cc.GetServiceUrl();
            datas ds = new datas("SELECT * FROM sly_test_phone GROUP BY city", "db");
            testType tt = new testType("execute", ds);
            //this.POST(_url, "");
            //string jsontext = SerializeToJson(tt);
            //testType te = DeSerializeToObject1(jsontext, typeof(testType)) as testType;

            ConnectionController cc = new ConnectionController();
            testType ty = cc.GetSqlResult(tt, typeof(testType)) as testType;
        }

        public class testType : Model.requestJson.BaseRequestJson
        {
            string api_type = string.Empty;
            datas data;
            public testType(string t, datas d)
            {
                api_type = t;
                data = d;
            }
        }

        [Serializable]
        public class datas
        {
            string sql = string.Empty;
            string type = string.Empty;
            string testproperty;
            public datas(string s, string t)
            {
                sql = s;
                type = t;
            }
        }
    }
}
