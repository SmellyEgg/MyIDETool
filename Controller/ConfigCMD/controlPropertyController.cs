using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.Controller.ConfigCMD
{
    public class controlPropertyController
    {
        delegate void SetPanelControlCallback(Control ct, Panel pnl);
        delegate void SetControlTextCallback(Control ct, string text);
        delegate void GetControlTextCallback(Control ct, ref string text);
        delegate void GetControlCallback(Panel pnl, string controlName, ref Control ct);
        delegate void ShowTabPageCallback(TabControl pnl);

        /// <summary>
        /// 显示控件的属性到界面上
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Task<int> SetObjectToView(ControlDetailForPage obj, List<Control> list)
        {
            return Task.Run(() =>
            {
                if (object.Equals(list, null) || object.Equals(obj, null))
                {
                    return 0;
                }
                foreach (var prop in obj.GetType().GetFields())
                {
                    string value = prop.GetValue(obj).ToString();
                    string controlName = "txt_" + prop.Name;
                    int index = list.FindIndex(p => controlName.Equals(p.Name));
                    if (index != -1)
                    {
                        Control ct = list.First(p => controlName.Equals(p.Name));
                        SetControlText(ct, value);
                    }
                }
                return 1;
            });
        }

        /// <summary>
        /// 添加控件到容器中
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="pnl"></param>
        public void AddControl(Control ct, Panel pnl)
        {
            if (pnl.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!pnl.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (pnl.Disposing || pnl.IsDisposed)
                        return;
                }
                SetPanelControlCallback d = new SetPanelControlCallback(AddControl);
                pnl.Invoke(d, new object[] { ct, pnl });
            }
            else
            {
                pnl.Controls.Add(ct);
            }
        }

        /// <summary>
        /// 防止占用线程卡死
        /// </summary>
        /// <param name="pnl"></param>
        public void ShowTabPage(TabControl pnl)
        {
            if (pnl.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!pnl.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (pnl.Disposing || pnl.IsDisposed)
                        return;
                }
                ShowTabPageCallback d = new ShowTabPageCallback(ShowTabPage);
                pnl.Invoke(d, new object[] { pnl });
            }
            else
            {
                foreach (TabPage page in pnl.TabPages)
                {
                    pnl.SelectedTab = page;
                    page.Show();
                }
                pnl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 委托设置控件文本
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="text"></param>
        private void SetControlText(Control ct, string text)
        {
            if (ct.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!ct.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (ct.Disposing || ct.IsDisposed)
                        return;
                }
                SetControlTextCallback d = new SetControlTextCallback(SetControlText);
                ct.Invoke(d, new object[] { ct, text });
            }
            else
            {
                ct.Text = text;
            }
        }

        public void GetControlText(Control ct, ref string text)
        {
            if (ct.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!ct.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (ct.Disposing || ct.IsDisposed)
                        return;
                }
                GetControlTextCallback d = new GetControlTextCallback(GetControlText);
                ct.Invoke(d, new object[] { ct, text });
            }
            else
            {
                text = ct.Text;
            }
        }

        public void GetControl(Panel pnl, string controlName, ref Control ct)
        {
            if (pnl.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!pnl.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (pnl.Disposing || pnl.IsDisposed)
                        return;
                }
                GetControlCallback d = new GetControlCallback(GetControl);
                pnl.Invoke(d, new object[] { pnl, controlName, ct });
            }
            else
            {
                ct = pnl.Controls.Find(controlName, true)[0];
            }
        }

    }
}
