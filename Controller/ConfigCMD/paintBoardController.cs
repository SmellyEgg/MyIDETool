using System.Threading.Tasks;
using xinLongIDE.Model.requestJson;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.Controller.ConfigCMD
{
    public class paintBoardController
    {
        private BaseController _bcController;

        public paintBoardController()
        {
            _bcController = new BaseController();
        }

        public Task<pageDetailReturnData> GetPageDetailInfo(string pageId)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(pageId))
                {
                    return null;
                }
                int time = 0;
                pageDetailRequest requestObj = new pageDetailRequest(System.Convert.ToInt32(pageId), time);
                pageDetailReturnData pd = _bcController.GetPageDetail(requestObj);
                return pd;
            });
        }
    }
}
