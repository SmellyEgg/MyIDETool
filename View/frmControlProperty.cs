using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongIDE.Controller;
using xinLongIDE.Controller.ConfigCMD;
using xinLongIDE.Controller.Settings;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.View
{
    public partial class frmControlProperty : Form
    {
        //用于实现属性值改变
        public delegate void delegateForControlPropertyChanged(object obj);
        public event delegateForControlPropertyChanged controlPropertyChange;

        private ControlDetailForPage _currentControlProperty = new ControlDetailForPage();

        private controlPropertyController _propertyPageController;

        public frmControlProperty()
        {
            InitializeComponent();
            _propertyPageController = new controlPropertyController();
            InitControl();
        }

        private async void InitControl()
        {
            int result = await SetAllControlPropertyView(this.pnlProperty);
        }

        private void frmControlProperty_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void frmControlProperty_Load(object sender, EventArgs e)
        {

        }

        public void SetControlProperty(Object obj)
        {
            _currentControlProperty = obj as ControlDetailForPage;
            _propertyPageController.SetObjectToView(_currentControlProperty, pnlProperty);
        }

        private void ChangeProperty()
        {
            //int result = await ChangePropertyAsync();

            ChangePropertNoAysnc();
        }

        private Task<int> ChangePropertyAsync()
        {
            return Task.Run(() =>
            {
                foreach (var prop in _currentControlProperty.GetType().GetFields())
                {
                    string controlName = "txt_" + prop.Name;
                    System.Windows.Forms.Control ct = new System.Windows.Forms.Control();
                    _propertyPageController.GetControl(pnlProperty, controlName, ref ct);
                    string value = string.Empty;
                    _propertyPageController.GetControlText(ct, ref value);
                    prop.SetValue(_currentControlProperty, value);
                }
                //controlPropertyChange.BeginInvoke(_currentControlProperty, delegateForControlPropertyChanged);
                return 1;
            });
        }

        /// <summary>
        /// 非异步的回调属性函数
        /// </summary>
        private void ChangePropertNoAysnc()
        {
            foreach (var prop in _currentControlProperty.GetType().GetFields())
            {
                string controlName = "txt_" + prop.Name;
                System.Windows.Forms.Control ct = this.pnlProperty.Controls.Find(controlName, true)[0];
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

        private Task<int> SetAllControlPropertyView(Panel pnl)
        {
            return Task.Run(() =>
            {
                if (object.Equals(pnl, null))
                {
                    return 0;
                }
                int previousLocationY = 10;
                foreach (var prop in typeof(Model.returnJson.ControlDetailForPage).GetFields())
                {
                    Label lbl = new Label();
                    lbl.Name = "lbl_" + prop.Name;
                    lbl.Text = I18N.GetString(prop.Name) + ":";
                    //lbl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                    lbl.AutoSize = true;
                    lbl.Size = new System.Drawing.Size(10, 10);
                    lbl.Location = new System.Drawing.Point(10, previousLocationY + 4);
                    TextBox txt = new TextBox();
                    txt.Name = "txt_" + prop.Name;
                    txt.Size = new System.Drawing.Size(80, 10);
                    //txt.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                    txt.Location = new System.Drawing.Point(110, previousLocationY);
                    txt.KeyDown += Txt_KeyDown;
                    _propertyPageController.AddControl(lbl, pnl);
                    _propertyPageController.AddControl(txt, pnl);
                    previousLocationY += 40;
                }
                return 1;
            });
        }

        private void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangeProperty();
            }
        }
    }
}
