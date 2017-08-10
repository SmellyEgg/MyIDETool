using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongIDE.Controller;
using xinLongIDE.Controller.CommonController;
using xinLongIDE.Controller.ConfigCMD;
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
        /// <summary>
        /// 当前控件实体
        /// </summary>
        private ControlDetailForPage _currentControlProperty = new ControlDetailForPage();

        private List<System.Windows.Forms.Control> _listControls;

        private controlPropertyController _propertyPageController;

        private List<string> _listAllPropertiesAndEvent;

        public frmControlProperty()
        {
            InitializeComponent();
            _propertyPageController = new controlPropertyController();
            _listControls = new List<System.Windows.Forms.Control>();
            _listAllPropertiesAndEvent = new List<string>();
            InitControl();
            this.Refresh();
        }

        private async void InitControl()
        {
            int result = await SetAllControlPropertyView();
        }

        public void SetControlProperty(Object obj)
        {
            _currentControlProperty = obj as ControlDetailForPage;
            _propertyPageController.SetObjectToView(_currentControlProperty, _listControls);
        }

        private void ChangeProperty()
        {
            ChangePropertNoAysnc();
        }


        /// <summary>
        /// 非异步的回调属性函数
        /// </summary>
        private void ChangePropertNoAysnc()
        {
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
            controlPropertyChange.Invoke(_currentControlProperty);
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
                _listControls.Clear();
                List<string> listProperty = this.GetControlPeoperties(Resources.zh_ControlProperty);
                AddControl(listProperty, this.tbgProperty);
                List<string> listEvent = this.GetControlPeoperties(Resources.zh_ControlEvent);
                AddControl(listEvent, this.tbgEvent);
                return 1;
            });
        }

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
                //lbl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                lbl.AutoSize = true;
                lbl.Size = new System.Drawing.Size(10, 10);
                lbl.Location = new System.Drawing.Point(10, previousLocationY + 4);
                TextBox txt = new TextBox();
                txt.Name = "txt_" + prop;
                txt.Size = new System.Drawing.Size(80, 10);
                //txt.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                txt.Location = new System.Drawing.Point(110, previousLocationY);
                txt.Tag = lbl.Text;
                txt.KeyDown += Txt_KeyDown;

                if (pnl == tbgEvent)
                {
                    txt.Size = new System.Drawing.Size(200, 10);
                    txt.TextChanged += Txt_TextChanged;
                    Button btn = new Button();
                    btn.Text = "…";
                    btn.Size = new System.Drawing.Size(25, 25);
                    btn.Location = new System.Drawing.Point(320, previousLocationY);
                    btn.Click += Btn_Click;
                    btn.Tag = txt.Name;
                    _propertyPageController.AddControl(btn, pnl);
                }
                _propertyPageController.AddControl(lbl, pnl);
                _propertyPageController.AddControl(txt, pnl);
                previousLocationY += 40;

                _listControls.Add(txt);
            }
            _propertyPageController.ShowTabPage(this.tabControl1);
        }

        private void Txt_TextChanged(object sender, EventArgs e)
        {
            ChangePropertNoAysnc();
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
            //throw new NotImplementedException();
            frmEventInput frm = new frmEventInput();
            frm.Tag = txt.Name;
            frm.Text = txt.Tag.ToString();

            frm.btnOkClicked += (s) => { if (s == 1)
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
                foreach(Form form in _listOpenedEventForm)
                {
                    form.TopMost = true;
                    form.BringToFront();
                    //form.Show();
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
