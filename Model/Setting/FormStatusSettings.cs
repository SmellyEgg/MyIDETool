using System.Drawing;

namespace xinLongIDE.Model.Setting
{
    public class FormStatusSettings 
    {
        private Size size;

        private Point point;

        private string platForm = string.Empty;


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

        public string PlatForm
        {
            get
            {
                return platForm;
            }

            set
            {
                platForm = value;
            }
        }
    }
}
