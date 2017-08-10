using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using xinLongIDE.Controller.ConfigCMD;
using xinLongIDE.Model.Control;
using static xinLongIDE.Controller.ConfigCMD.toolboxController;

namespace xinLongIDE.View.MainForm
{
    public partial class frmToolBox : Form
    {
        public frmToolBox()
        {
            InitializeComponent();
            InitControl();
        }

        private toolboxController _toolController;

        private void InitControl()
        {
            _toolController = new toolboxController();
            this.lvwtoolboxitems.Clear();
            lvwtoolboxitems.View = System.Windows.Forms.View.List;
            ImageList imglst = _toolController.GetImageIconList();
            List<string> typeValuelst = _toolController.GetAllControlTypeValue();
            List<Boolean> controlVisibility = _toolController.GetALlControlTypeVisibility();
            imglst.ImageSize = new Size(40, 32);
            this.lvwtoolboxitems.SmallImageList = imglst;

            toolboxItem tag;
            for (int i = 0; i < imglst.Images.Count; i++)
            {
                ListViewItem newItem = new ListViewItem { ImageIndex = i, Text = ((rowsOfControlList)i).ToString() };
                tag = new toolboxItem(((rowsOfControlList)i).ToString(), typeValuelst[i], controlVisibility[i]);
                newItem.Tag = tag;
                lvwtoolboxitems.Items.Add(newItem);

            }
        }

        private void lstControls_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void lvwtoolboxitems_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void lvwtoolboxitems_MouseDown(object sender, MouseEventArgs e)
        {
            var selectedItem = ((ListView)sender).HitTest(e.Location);
            if (!object.Equals(selectedItem.Item, null))
            {
                lvwtoolboxitems.DoDragDrop(selectedItem.Item.Tag, DragDropEffects.Move);
            }
        }
    }
}
