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
            //User user = new User("wepartner", "wepartner123");
            BaseController bc = new BaseController();
            //fieldObjeForControl dfff = new fieldObjeForControl("1", "1");
            //controlUpdateRequest cur = new controlUpdateRequest("1", "display", dfff);
            //bc.UpdateControlsConfig(cur);
            //bc.Login(user);
            //groupCreateRequest gcr = new groupCreateRequest("默认分组abc", "pc");
            //pageSaveRequest psr = new pageSaveRequest("wang111222", "210");
            //ctrlObjForSavePage cfs = new ctrlObjForSavePage("123", "456", "hello1234", "33");
            //ctrlObjForSavePage cfs1 = new ctrlObjForSavePage("123", "456", "world1234", "34");

            //psr.ctrls = new ctrlObjForSavePage[] { cfs, cfs1 };

            //bc.SagePageInfo(psr);
            // bc.GetControlConfigInfo();


            photoUploadRequest pr = new photoUploadRequest(null, "");
            //pageDetailRequest pdr = new pageDetailRequest(1, 1493086328);
            //bc.GetPageDetail(pdr);
            bc.PhotoUpload(pr);

            //bc.GetControlConfigInfo();

            //pageCreateRequest gur = new pageCreateRequest("28", "pc", "yemian1");
            //bc.CreatePage(gur);
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
