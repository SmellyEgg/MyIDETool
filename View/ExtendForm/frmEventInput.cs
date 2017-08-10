using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using xinLongIDE.Controller.CommonController;
using xinLongIDE.Controller.ConfigCMD;

namespace xinLongIDE.View.ExtendForm
{
    public partial class frmEventInput : Form
    {
        private string resultStr = string.Empty;

        public string ResultStr
        {
            get
            {
                return this.resultStr;
            }
        }

        public delegate void delegateForOkButton(int value);
        public event delegateForOkButton btnOkClicked;


        public frmEventInput()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 带入原来的字符串
        /// </summary>
        /// <param name="text"></param>
        public void SetOriginalText(string text)
        {
            if (!string.IsNullOrEmpty(text.Trim()))
            {
                this.rtxEvent.Lines = this.arrayStringToArray(text);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            resultStr = TextToArray(this.rtxEvent.Text);
            //用于指定是否正常赋值返回
            btnOkClicked.Invoke(1);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 将输入框的文本转换为实际的文本数组
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string TextToArray(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            List<string> list = new List<string>();
            foreach (var line in this.rtxEvent.Lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string realStr = Function.GetContrastTranslatedStr(line);
                    realStr = "\"" + realStr + "\"";
                    list.Add(realStr);
                }
            }
            string result = Function.ConvertarrayToString(list.ToArray());
            return result;
        }

        /// <summary>
        /// 对传入的字符串进行转义
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string[] arrayStringToArray(string text)
        {
            string arrayString = text.Substring(1, text.Length - 2);
            string[] array = arrayString.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                //这里需要去除掉双引号
                array[i] = array[i].Substring(1, array[i].Length - 2);
                //转换为正常的字符
                array[i] = Function.GetTranlatedStr(array[i]);
            }
            return array;
        }
        /// <summary>
        /// 快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtxEvent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                this.btnOk.PerformClick();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (lstEventList.Visible)
                {
                    //当事件选择框弹出来的时候对回车事件进行屏蔽并且传递到选择框控件中
                    e.Handled = true;
                    KeyEventArgs egg = new KeyEventArgs(Keys.Enter);
                    lstEventList_KeyDown(lstEventList, e);
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (lstEventList.SelectedIndex == -1)
                {
                    lstEventList.SelectedIndex = 0;
                }
                else
                {
                    if (lstEventList.SelectedIndex + 1 >= 0 && lstEventList.SelectedIndex + 1 <= lstEventList.Items.Count - 1)
                    {
                        lstEventList.SelectedIndex = lstEventList.SelectedIndex + 1;
                    }
                }
            } 
            else if (e.KeyCode == Keys.Up)
            {
                if (lstEventList.SelectedIndex == -1)
                {
                    lstEventList.SelectedIndex = lstEventList.Items.Count - 1;
                }
                else
                {
                    if (lstEventList.SelectedIndex + 1 >= 0 && lstEventList.SelectedIndex + 1 <= lstEventList.Items.Count - 1)
                    {
                        lstEventList.SelectedIndex = lstEventList.SelectedIndex - 1;
                    }
                }
            }
        }

        private int _searchCharStartIndex = -1;

        /// <summary>
        /// 监测文本输入框中的按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtxEvent_KeyUp(object sender, KeyEventArgs e)
        {
            int dotKeyValue = 190;
            if (e.KeyValue == dotKeyValue)
            {
                //可见性
                int cursorPosition = rtxEvent.SelectionStart;
                _searchCharStartIndex = cursorPosition;
                Point curPoint = this.rtxEvent.GetPositionFromCharIndex(cursorPosition);
                curPoint.Offset(rtxEvent.Left, rtxEvent.Top + rtxEvent.Font.Height + 5);
                lstEventList.Items.Clear();
                this.lstEventList.Items.AddRange(I18N._stringsTranslator.Keys.ToArray());
                
                lstEventList.Location = curPoint;
                lstEventList.Visible = true;
                lstEventList.BringToFront();
            } 
        }

        /// <summary>
        /// 设置搜索引擎
        /// </summary>
        /// <param name="list"></param>
        /// <param name="searchtext"></param>
        private void SetFilterSearchForListEvent(List<string> list, string searchtext)
        {
            if (string.IsNullOrEmpty(searchtext))
            {
                this.lstEventList.Items.Clear();
                this.lstEventList.Items.AddRange(list.ToArray());
                return;
            }
            this.lstEventList.Items.Clear();

            foreach (string str in list)
            {
                if (str.ToLower().StartsWith(searchtext.ToLower(), StringComparison.CurrentCultureIgnoreCase))
                {
                    lstEventList.Items.Add(str);
                }
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEventInput_FormClosed(object sender, FormClosedEventArgs e)
        {
            //用于回调给主界面是正常返回值还是手动关闭
            btnOkClicked.Invoke(0);
        }

        /// <summary>
        /// 窗体位置改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEventInput_LocationChanged(object sender, EventArgs e)
        {
            //改变窗体位置时将其层级设置到最前
            this.TopMost = true;
            this.BringToFront();
        }

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstEventList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetTextFromListbox();
            }
        }

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstEventList_DoubleClick(object sender, EventArgs e)
        {
            if (this.lstEventList.SelectedIndex != -1)
            {
                SetTextFromListbox();
            }
        }

        /// <summary>
        /// 回调文本
        /// </summary>
        private void SetTextFromListbox()
        {
            lstEventList.Visible = false;
            if (_searchCharStartIndex != -1)
            {
                this.rtxEvent.Text = this.rtxEvent.Text.Remove(_searchCharStartIndex, this.rtxEvent.Text.Length - _searchCharStartIndex);
            }
            this.rtxEvent.Text += lstEventList.SelectedItem.ToString();
            rtxEvent.SelectionStart = rtxEvent.Text.Length;
            rtxEvent.SelectionLength = 0;
            _searchCharStartIndex = -1;
        }

        /// <summary>
        /// 文本改变事件
        /// 这里用这个事件的话可以避免对除了字母数字之外的键值进行判断的情况，机智
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtxEvent_TextChanged(object sender, EventArgs e)
        {
            if (lstEventList.Visible && _searchCharStartIndex!= -1)
            {
                string searchText = this.rtxEvent.Text.Substring(_searchCharStartIndex, this.rtxEvent.Text.Length - _searchCharStartIndex);
                SetFilterSearchForListEvent(I18N._stringsTranslator.Keys.ToList(), searchText);
            }
        }
    }
}
