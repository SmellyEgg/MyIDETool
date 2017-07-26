using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using xinLongIDE.Controller;
using xinLongIDE.Controller.ConfigCMD;
using xinLongIDE.Model.Control;
using xinLongIDE.Model.returnJson;
using xinLongIDE.View.Control;

namespace xinLongIDE.View
{
    public partial class frmPaintBoard : Form
    {
        private paintBoardController _paintboardController;
        public frmPaintBoard()
        {
            InitializeComponent();
            _paintboardController = new paintBoardController();
        }

        public async void ShowPageDetail(object obj)
        {
            pageDetailForGroup pageObj = obj as pageDetailForGroup;
            pageDetailReturnData pageInfo = await _paintboardController.GetPageDetailInfo(pageObj.page_id);
            if (object.Equals(pageInfo.data, null))
            {
                return;
            }
            try
            {
                //对页面进行解析
                this.SetBaseInfo(pageInfo);
                this.SetPageDetailInfo(pageInfo);
            }
            catch
            {

            }
        }

        private void SetBaseInfo(pageDetailReturnData obj)
        {
            dataForPageDetail dtObj = obj.data;
            this.Text = dtObj.page_title;
            this.BackColor = System.Drawing.ColorTranslator.FromHtml(dtObj.page_background_color);
            dtObj = null;
            //this.Height = dtObj.page_height;
        }
        List<ControlDetailForPage> _lstControl = new List<ControlDetailForPage>();
        Controller.ClassDecode _clsDecode = new Controller.ClassDecode();
        private void SetPageDetailInfo(pageDetailReturnData obj)
        {
            dataForPageDetail dtObj = obj.data;

            
            try
            {
                _lstControl = Controller.ControlCaster.CastArrayToControl(obj.data.control_list);
                //_lstControl = _lstControl.OrderBy(p => p.ctrl_id).ToList();
                //这里代表page包含的所有控件
                ControlDetailForPage pageObj = _lstControl.Where(p => "page".Equals(p.ctrl_type)).ToList()[0];
                string[] pageControlStrList = _clsDecode.DecodeArray(pageObj.d0.ToString());
                foreach (string str in pageControlStrList)
                {
                    ControlDetailForPage control = _lstControl.Where(p => str.Equals(p.ctrl_id)).ToList()[0];
                    DrawControl(control);
                }
            }
            catch (System.Exception ex)
            {
                Logging.Error(ex.Message);
            }

        }

        private int _randomControlName = 0;

        /// <summary>
        /// 递归绘制界面
        /// </summary>
        /// <param name="obj"></param>
        private void DrawControl(ControlDetailForPage obj)
        {
            if ("text".Equals(obj.ctrl_type))
            {
                xinLongyuText textControl = new xinLongyuText();
                SetVisibleControl(textControl, obj);
                textControl.Text = obj.d0.ToString();
            }
            else if ("img".Equals(obj.ctrl_type))
            {
                xinLongyuImg imgControl = new xinLongyuImg();
                SetVisibleControl(imgControl, obj);

                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(obj.d0.ToString());
                using (var ms = new System.IO.MemoryStream(imageBytes))
                {
                    imgControl.Image = Image.FromStream(ms);
                }
            }
            else if ("uploadImage".Equals(obj.ctrl_type))
            {
                xinLongyuButton btnControl = new xinLongyuButton();
                btnControl.AutoSize = true;
                SetInvisibleControl(btnControl, obj);
                btnControl.Text = "图片上传";
            }
            else if ("navigationBar".Equals(obj.ctrl_type))
            {

            }
            else if ("superview".Equals(obj.ctrl_type))
            {
                Panel pnlControl = new Panel();
                string[] pageControlStrList = _clsDecode.DecodeArray(obj.d0.ToString());
                foreach (string str in pageControlStrList)
                {
                    ControlDetailForPage control = _lstControl.Where(p => str.Equals(p.ctrl_id)).ToList()[0];
                    DrawControl(control);
                }
            }
        }

        private List<System.Windows.Forms.Control> _lstInvisibleControl = new List<System.Windows.Forms.Control>();

        private void SetInvisibleControl(System.Windows.Forms.Control ct, ControlDetailForPage obj)
        {
            if (_lstInvisibleControl.Count < 1)
            {
                obj.d1 = "50";
                obj.d2 = "25";
                obj.d3 = "10";
                obj.d4 = "50";
            }
            else
            {
                System.Windows.Forms.Control previousControl = _lstInvisibleControl[_lstInvisibleControl.Count - 1];
                obj.d3 = (previousControl.Location.X + 10 + previousControl.Width).ToString();
                obj.d4 = (_lstInvisibleControl[_lstInvisibleControl.Count - 1].Location.Y).ToString();
            }
            SetControlBaseInfo(ct, obj, this.pnlInvisible);
            _lstInvisibleControl.Add(ct);
        }

        private void SetVisibleControl(System.Windows.Forms.Control ct, ControlDetailForPage obj)
        {
            SetControlBaseInfo(ct, obj, this.pnlPaintBoard);
        }

        private void SetControlBaseInfo(System.Windows.Forms.Control ct, ControlDetailForPage obj, Panel pnl)
        {
            ct.Name = obj.ctrl_type + "_" + _randomControlName.ToString();
            ct.Location = new Point(Convert.ToInt32(obj.d3), Convert.ToInt32(obj.d4));
            ct.Size = new System.Drawing.Size(Convert.ToInt32(obj.d1), Convert.ToInt32(obj.d2));
            ct.Tag = obj;
            pnl.Controls.Add(ct);
            ct.BringToFront();
            _randomControlName++;

        }

        private void pnlPaintBoard_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void pnlPaintBoard_DragDrop(object sender, DragEventArgs e)
        {
            toolboxItem data = e.Data.GetData(typeof(toolboxItem)) as toolboxItem;
            string text = data.ItemText;
        }
    }
}
