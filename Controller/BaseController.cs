using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xinLongIDE.Controller.data;
using xinLongIDE.Model;
using xinLongIDE.Model.requestJson;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.Controller
{
    public class BaseController
    {
        ConnectionController _ccController;

        ClassDecode _clsDecode;
        public BaseController()
        {
            _ccController = new ConnectionController();
            _clsDecode = new ClassDecode();
        }

        public photoUploadReturnData PhotoUpload(photoUploadRequest obj)
        {

            //string specilRequest = "{\"api_type\": \"{0}\",\"data\": {\"file\": {1} \"sql\": {2},\"params\":{3}}}";
            string apitype = dataAccessDictionary.upload;
            BaseRequestJson bj = new BaseRequestJson();
            bj.api_type = apitype;
            bj.data = obj;

            string result = _ccController.getReturnStr(bj);
            BaseReturnJson brj = _clsDecode.DecodeBaseReturnJson(result);
            photoUploadReturnData pur = _clsDecode.DecodephotoUploadReturnData(brj.data.ToString());
            return pur;
        }

        public pageDetailReturnData GetPageDetail(pageDetailRequest obj)
        {
            string apitype = dataAccessDictionary.page;
            BaseRequestJson bj = new BaseRequestJson();
            bj.api_type = apitype;
            bj.data = obj;

            string result = _ccController.getReturnStr(bj);
            BaseReturnJson brj = _clsDecode.DecodeBaseReturnJson(result);
            pageDetailReturnData prd = _clsDecode.DecodepageDetailReturnData(brj.data.ToString());
            return prd;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserData Login(User user)
        {
            if (object.Equals(user, null))
            {
                return null;
            }
            //
            string apitype = dataAccessDictionary.login;
            BaseRequestJson bj = new BaseRequestJson();
            bj.api_type = apitype;
            bj.data = user;
            //
            string result = _ccController.getReturnStr(bj);
            BaseReturnJson brj = _clsDecode.DecodeBaseReturnJson(result);
            UserData ud = _clsDecode.DecodeUserData(brj.data.ToString());
            return null;
        }

        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="gcr"></param>
        /// <returns></returns>
        public CommonReturn CreateGroup(groupCreateRequest gcr)
        {
            BaseRequestJson brj = new BaseRequestJson();
            brj.data = gcr;
            brj.api_type = dataAccessDictionary.groupCreate;

            return getCommonReturn(brj);
        }

        /// <summary>
        /// 获取页面分组信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public pageGroupReturnData GetPageGroupInfo(string type)
        {
            string apitype = dataAccessDictionary.groupPageGet;
            BaseRequestJson bj = new BaseRequestJson();
            pageGroupData pgd = new pageGroupData(type);
            bj.api_type = apitype;
            bj.data = pgd;

            string result = _ccController.getReturnStr(bj);
            BaseReturnJson brj = _clsDecode.DecodeBaseReturnJson(result);
            pageGroupReturnData pgr = _clsDecode.DecodepageGroupReturnData(brj.data.ToString());
            return pgr;
        }

        /// <summary>
        /// 更新组名
        /// </summary>
        /// <param name="pur"></param>
        /// <returns></returns>
        public CommonReturn UpdateGroupPageName(groupUpdateRequest pur)
        {
            string apitype = dataAccessDictionary.groupUpdate;
            BaseRequestJson bj = new BaseRequestJson();
            bj.data = pur;
            bj.api_type = apitype;

            return getCommonReturn(bj);
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public CommonReturn DeleteGroup(string groupId)
        {
            string apitype = dataAccessDictionary.groupDelete;
            BaseRequestJson bj = new BaseRequestJson();
            groupDelereRequest gdr = new groupDelereRequest(groupId);
            bj.data = gdr;
            bj.api_type = apitype;

            return getCommonReturn(bj);
        }

        /// <summary>
        /// 新建页面
        /// </summary>
        /// <returns></returns>
        public CommonReturn CreatePage(pageCreateRequest pcr)
        {
            string apitype = dataAccessDictionary.createPage;
            BaseRequestJson bj = new BaseRequestJson();
            bj.data = pcr;
            bj.api_type = apitype;

            return getCommonReturn(bj);
        }

        public CommonReturn SagePageInfo(pageSaveRequest obj)
        {
            string apitype = dataAccessDictionary.savePage;
            BaseRequestJson bj = new BaseRequestJson();
            bj.data = obj;
            bj.api_type = apitype;

            return getCommonReturn(bj);
        }

        public CommonReturn DeletePage(string pageId)
        {
            string apitype = dataAccessDictionary.deletePage;
            BaseRequestJson bj = new BaseRequestJson();
            pageDeleteRequest pdr = new pageDeleteRequest(pageId);
            bj.data = pdr;
            bj.api_type = apitype;

            return getCommonReturn(bj);
        }

        /// <summary>
        /// 获取工具表信息
        /// </summary>
        /// <returns></returns>
        public controlReturnData GetControlConfigInfo()
        {
            string apitype = dataAccessDictionary.getCtrl;
            BaseRequestJson bj = new BaseRequestJson();
            bj.api_type = apitype;

            string result = _ccController.getReturnStr(bj);
            BaseReturnJson brj = new BaseReturnJson();
            brj = _clsDecode.DecodeBaseReturnJson(result);
            controlReturnData cr = _clsDecode.DecodecontrolReturnData(brj.data.ToString());
            return cr;
        }

        public CommonReturn UpdateControlsConfig(controlUpdateRequest obj)
        {
            string apitype = dataAccessDictionary.updateCtrl;
            BaseRequestJson bj = new BaseRequestJson();
            bj.data = obj;
            bj.api_type = apitype;

            return getCommonReturn(bj);
        }

        private CommonReturn getCommonReturn(object obj)
        {
            string result = _ccController.getReturnStr(obj);
            BaseReturnJson brj = _clsDecode.DecodeBaseReturnJson(result);
            CommonReturn gcr = _clsDecode.DecodegroupCreateReturn(brj.data.ToString());
            return gcr;
        }
    }
}
