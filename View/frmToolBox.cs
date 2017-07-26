using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongIDE.Controller.ConfigCMD;
using xinLongIDE.Model.Control;
using static xinLongIDE.Controller.ConfigCMD.toolboxController;

namespace xinLongIDE.View
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
            imglst.ImageSize = new Size(16, 16);
            this.lvwtoolboxitems.SmallImageList = imglst;
            for (int i = 0; i < imglst.Images.Count; i++)
            {
                lvwtoolboxitems.Items.Add(new ListViewItem { ImageIndex = i, Text = ((rowsOfControlList)i).ToString() });
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
            toolboxItem obj = new toolboxItem(selectedItem.Item.Text);
            if (!object.Equals(selectedItem.Item, null))
            {
                lvwtoolboxitems.DoDragDrop(obj, DragDropEffects.Move);
            }
        }
    }
}
