using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongIDE.Controller.CommonController;
using xinLongIDE.Controller.ConfigCMD;
using xinLongIDE.Model.Page;
using xinLongIDE.Model.requestJson;
using xinLongIDE.Model.returnJson;
using xinLongIDE.Properties;
using xinLongIDE.View.ExtendForm;

namespace xinLongIDE.View.MainForm
{
    /// <summary>
    /// 属性控制窗体
    /// </summary>
    public partial class frmControlProperty : Form
    {
        //用于实现属性值改变
        public delegate void delegateForControlPropertyChanged(object obj);
        public event delegateForControlPropertyChanged controlPropertyChange;

        //用于页面属性值改变
        //public delegate void delegateForPagePropertyChanged(object obj);
        //public event delegateForPagePropertyChanged pagePropertyChange;
        /// <summary>
        /// 当前控件实体
        /// </summary>
        private ControlDetailForPage _currentControlProperty = new ControlDetailForPage();

        private basePageProperty _currentPageProperty = new basePageProperty();

        private List<System.Windows.Forms.Control> _listControls;

        private controlPropertyController _propertyPageController;

        private List<string> _listAllPropertiesAndEvent;

        private BaseController _bsController;

        public frmControlProperty()
        {
            InitializeComponent();
            _propertyPageController = new controlPropertyController();
            _listControls = new List<System.Windows.Forms.Control>();
            _listAllPropertiesAndEvent = new List<string>();
            _bsController = new BaseController();
            InitControl();
            this.Refresh();
        }

        private async void InitControl()
        {
            int result = await SetAllControlPropertyView();
        }

        public void SetControlProperty(Object obj)
        {
            isFinished = false;
            if (obj is ControlDetailForPage)
            {
                _currentControlProperty = obj as ControlDetailForPage;
                _propertyPageController.SetObjectToView(_currentControlProperty, _listControls);
            }
            else if (obj is basePageProperty)
            {
                _currentPageProperty = obj as basePageProperty;
                _propertyPageController.SetObjectToPageView(_currentPageProperty, _listControls);
            }
            isFinished = true;
        }

        private void ChangeProperty()
        {
            ChangePropertNoAysnc();

            ChangePagePropertyNoAsync();
        }


        /// <summary>
        /// 非异步的回调属性函数
        /// </summary>
        private void ChangePropertNoAysnc()
        {
            _currentControlProperty = new ControlDetailForPage();
            foreach (var prop in _currentControlProperty.GetType().GetFields())
            {
                string controlName = "txt_" + prop.Name;
                if (_listControls.FindIndex(p => controlName.Equals(p.Name)) == -1)
                {
                    continue;
                }
                System.Windows.Forms.Control ct = _listControls.First(p => controlName.Equals(p.Name));
                string value = ct.Text;
                if ("Boolean".Equals(prop.FieldType.Name))
                {
                    prop.SetValue(_currentControlProperty, xinLongyuConverter.StringToBool(value));
                }
                else if ("Int32".Equals(prop.FieldType.Name))
                {
                    prop.SetValue(_currentControlProperty, xinLongyuConverter.StringToInt(value));
                }
                else if ("String".Equals(prop.FieldType.Name))
                {
                    prop.SetValue(_currentControlProperty, value);
                }
            }
            //_currentControlProperty.d17 = _currentControlProperty.d0;
            controlPropertyChange.Invoke(_currentControlProperty);
        }

        private void ChangePagePropertyNoAsync()
        {
            _currentPageProperty = new basePageProperty();
            foreach (var prop in _currentPageProperty.GetType().GetProperties())
            {
                string controlName = "txt_" + prop.Name;
                if (_listControls.FindIndex(p => controlName.Equals(p.Name)) == -1)
                {
                    continue;
                }
                System.Windows.Forms.Control ct = _listControls.First(p => controlName.Equals(p.Name));
                string value = ct.Text;
                if ("Boolean".Equals(prop.PropertyType.Name))
                {
                    prop.SetValue(_currentPageProperty, xinLongyuConverter.StringToBool(value));
                }
                else if ("Int32".Equals(prop.PropertyType.Name))
                {
                    prop.SetValue(_currentPageProperty, xinLongyuConverter.StringToInt(value));
                }
                else if ("String".Equals(prop.PropertyType.Name))
                {
                    prop.SetValue(_currentPageProperty, value);
                }
            }
            controlPropertyChange.Invoke(_currentPageProperty);
        }

        private void property_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangeProperty();
            }
        }

        private Task<int> SetAllControlPropertyView()
        {
            return Task.Run(() =>
            {
                isFinished = false;
                _listControls.Clear();
                List<string> listProperty = this.GetControlPeoperties(Resources.zh_ControlProperty);
                AddControl(listProperty, this.tbgProperty);
                List<string> listEvent = this.GetControlPeoperties(Resources.zh_ControlEvent);
                AddControl(listEvent, this.tbgEvent);
                List<string> listPageProperty = this.GetControlPeoperties(Resources.zh_PageProperty);
                AddControl(listPageProperty, this.tbgPageProperty);

                isFinished = true;
                return 1;
            });
        }

        private bool isFinished = false;

        /// <summary>
        /// 添加控件到页面中
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="pnl"></param>
        private void AddControl(List<string> listName, TabPage pnl)
        {
            _listOpenedEventForm.Clear();
            pnl.Controls.Clear();
            int previousLocationY = 10;
            foreach (var prop in listName)
            {
                Label lbl = new Label();
                lbl.Name = "lbl_" + prop;
                lbl.Text = I18N.GetString(prop) + ":";
                lbl.AutoSize = true;
                lbl.Size = new System.Drawing.Size(10, 10);
                lbl.Location = new System.Drawing.Point(10, previousLocationY + 4);
                TextBox txt = new TextBox();
                txt.Name = "txt_" + prop;
                txt.Size = new System.Drawing.Size(80, 10);
                txt.Location = new System.Drawing.Point(110, previousLocationY);
                txt.Tag = lbl.Text;
                txt.KeyDown += Txt_KeyDown;
                txt.LostFocus += Txt_TextChanged;

                if ("d0".Equals(prop))
                {
                    Button btn = new Button();
                    btn.Name = "btn_" + prop;
                    btn.Text = "上传";
                    btn.Size = new System.Drawing.Size(50, 25);
                    btn.Location = new System.Drawing.Point(200, previousLocationY);
                    btn.Click += Btn_Click2;
                    //设置标志
                    btn.Tag = txt.Name + "upload";
                    _propertyPageController.AddControl(btn, pnl);
                    _listControls.Add(btn);
                }
                if (pnl == tbgEvent)
                {
                    //txt.TextChanged += Txt_TextChanged;
                    txt.Size = new System.Drawing.Size(200, 10);
                    Button btn = new Button();
                    btn.Text = "…";
                    btn.Size = new System.Drawing.Size(25, 25);
                    btn.Location = new System.Drawing.Point(320, previousLocationY);
                    btn.Click += Btn_Click;
                    btn.Tag = txt.Name;
                    _propertyPageController.AddControl(btn, pnl);
                }
                //为颜色增加一个按钮
                else if (lbl.Text.Contains("颜色") && pnl == tbgProperty)
                {
                    Button btn = new Button();
                    btn.Name = "btn_" + prop;
                    btn.Text = " ";
                    btn.Size = new System.Drawing.Size(25, 25);
                    btn.Location = new System.Drawing.Point(200, previousLocationY);
                    string pattern = @"\#\w{6}";
                    if (Regex.IsMatch(txt.Text, pattern))
                    {
                        btn.BackColor = System.Drawing.ColorTranslator.FromHtml(txt.Text);
                    }
                    else
                    {
                        btn.BackColor = System.Drawing.SystemColors.Control;
                    }
                    btn.Click += Btn_Click1;
                    //设置标志
                    btn.Tag = txt.Name;
                    //txt.Tag = btn.Name;
                    _propertyPageController.AddControl(btn, pnl);
                    _listControls.Add(btn);
                }
                _propertyPageController.AddControl(lbl, pnl);
                _propertyPageController.AddControl(txt, pnl);
                previousLocationY += 40;

                _listControls.Add(txt);
            }
            _propertyPageController.ShowTabPage(this.tabControl);
        }

        /// <summary>
        /// 图片选择窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Click2(object sender, EventArgs e)
        {
            string txttextbox = (sender as Button).Tag.ToString();
            TextBox txt = _listControls.First(p => txttextbox.Substring(0, txttextbox.Length - 6).Equals(p.Name)) as TextBox;
            //if (string.IsNullOrEmpty(txt.Text.Trim()))
            //{
            //    MessageBox.Show("SQL不能为空");
            //    return;
            //}
            //hrow new NotImplementedException();
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "JPEG files (*.jpg)|*.jpg|GIF files (*.gif)|*.gif|All files (*.*)|*.*";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                
                photoUploadRequest request = new photoUploadRequest(opf.FileName, txt.Text);
                photoUploadReturnData result =  _bsController.PhotoUpload(request);
                txt.Text = result.data.path;
                this.ChangePropertNoAysnc();
            }

        }



        /// <summary>
        /// 颜色选择窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Click1(object sender, EventArgs e)
        {
            TextBox txt = _listControls.First(p => (sender as Button).Tag.ToString().Equals(p.Name)) as TextBox;
            ColorDialog colorFrm = new ColorDialog();
            if (!string.IsNullOrEmpty(txt.Text))
            {
                //这里用个简单的正则来判断颜色字符串是否合法
                string pattern = @"\#\w{6}";
                if (Regex.IsMatch(txt.Text, pattern))
                {
                    colorFrm.Color = System.Drawing.ColorTranslator.FromHtml(txt.Text);
                }
                else
                {
                    MessageBox.Show("转换颜色格式出错！");
                }
            }
            if (colorFrm.ShowDialog() == DialogResult.OK)
            {
                string colorResult = System.Drawing.ColorTranslator.ToHtml(colorFrm.Color);
                txt.Text = colorResult;
                (sender as Button).BackColor = colorFrm.Color;
                ChangePropertNoAysnc();
            }
        }

        /// <summary>
        /// 文本改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_TextChanged(object sender, EventArgs e)
        {
            if (isFinished)
            {
                ChangePropertNoAysnc();
            }
        }

        private List<Form> _listOpenedEventForm = new List<Form>();

        private void Btn_Click(object sender, EventArgs e)
        {
            TextBox txt = _listControls.First(p => (sender as Button).Tag.ToString().Equals(p.Name)) as TextBox;
            int indexFrm = _listOpenedEventForm.FindIndex(p => txt.Name.Equals(p.Tag.ToString()));
            if (indexFrm != -1)
            {
                _listOpenedEventForm[indexFrm].TopMost = true;
                _listOpenedEventForm[indexFrm].BringToFront();
                return;
            }
            frmEventInput frm = new frmEventInput();
            frm.Tag = txt.Name;
            frm.Text = txt.Tag.ToString();

            frm.btnOkClicked += (s) =>
            {
                if (s == 1)
                {
                    txt.Text = frm.ResultStr;
                }
                else if (s == 0)
                {
                    _listOpenedEventForm.Remove(frm);
                }
            };
            frm.TopLevel = true;
            frm.SetOriginalText(txt.Text);
            frm.Show();
            if (_listOpenedEventForm.Count > 0)
            {
                foreach (Form form in _listOpenedEventForm)
                {
                    form.TopMost = true;
                    form.BringToFront();
                }
            }
            _listOpenedEventForm.Add(frm);
        }

        private void Frm_btnOkClicked()
        {

        }

        private void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangeProperty();
            }
        }

        /// <summary>
        /// 获取配置文件中的信息
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private List<String> GetControlPeoperties(string res)
        {
            using (var sr = new StringReader(res))
            {
                List<string> list = new List<string>();
                var line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                        continue;
                    if (line[0] == '#')
                        continue;

                    list.Add(line);
                }
                return list;
            }
        }
    }
}
