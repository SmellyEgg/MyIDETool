using System.Drawing;

namespace xinLongIDE.Model.Setting
{
    public class FormStatusSettings 
    {
        private Size size;

        private Point point;

        public Size SizeConfig
        {
            get 
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        public Point pointConfig
        {
            get
            {
                return this.point;
            }
            set
            {
                this.point = value;
            }
        }
    }
}
