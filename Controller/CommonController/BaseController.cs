using System.Collections.Generic;
using xinLongIDE.Controller.dataDic;
using xinLongIDE.Model.requestJson;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.Controller.CommonController
{
    /// <summary>
    /// 与服务的交互类
    /// </summary>
    public class BaseController
    {
        /// <summary>
        /// 连接逻辑层
        /// </summary>
        ConnectionController _ccController;
        /// <summary>
        /// json转换类逻辑层
        /// </summary>
        ClassDecode _clsDecode;
        public BaseController()
        {
            _ccController = new ConnectionController();
            _clsDecode = new ClassDecode();
        }

        /// <summary>
        /// 上传照片
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public photoUploadReturnData PhotoUpload(photoUploadRequest obj)
        {
            string apitype = jsonApiType.upload;
            var values = new[]
{
                new KeyValuePair<string, string>("api_type", apitype),
                new KeyValuePair<string, string>("sql", obj.sql),
                 //other values
            };
            string  result = _ccController.PostPhoto(obj.file, values);
            BaseReturnJson brj = _clsDecode.DecodeBaseReturnJson(result);
            photoUploadReturnData pur = _clsDecode.DecodephotoUploadReturnData(brj.data.ToString());
            return pur;
        }

        /// <summary>
        /// 获取页面详细信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public pageDetailReturnData GetPageDetail(pageDetailRequest obj)
        {
            string apitype = jsonApiType.page;
            BaseRequestJson bj = this.GetBaseRequest();
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
            string apitype = jsonApiType.login;
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
            string apitype = jsonApiType.groupCreate;

            BaseRequestJson bj = this.GetBaseRequest();
            bj.data = gcr;
            bj.api_type = apitype;

            return getCommonReturn(bj);
        }

        /// <summary>
        /// 获取页面分组信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public pageGroupReturnData GetPageGroupInfo(string type)
        {
            string apitype = jsonApiType.groupPageGet;
            BaseRequestJson bj = this.GetBaseRequest();
            pageGroupData pgd = new pageGroupData(type);
            bj.api_type = apitype;
            bj.data = pgd;

            string result = _ccController.getReturnStr(bj);
            BaseReturnJson brj = _clsDecode.DecodeBaseReturnJson(result);
            pageGroupReturnData pgr = _clsDecode.DecodepageGroupReturnData(brj.data.ToString());
            return pgr;
        }

        /// <summary>
        /// 通用的请求
        /// </summary>
        /// <returns></returns>
        private BaseRequestJson GetBaseRequest()
        {
            BaseRequestJson bj = new BaseRequestJson();
            //这里的用户后续要考虑是不是要从登陆中获取
            CommonLoginRequest loginRequest = new CommonLoginRequest("admin", "123456");
            bj.auth = loginRequest;
            return bj;
        }

        /// <summary>
        /// 更新组名
        /// </summary>
        /// <param name="pur"></param>
        /// <returns></returns>
        public CommonReturn UpdateGroupPageName(groupUpdateRequest pur)
        {
            string apitype = jsonApiType.groupUpdate;
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
            string apitype = jsonApiType.groupDelete;
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
            string apitype = jsonApiType.createPage;
            BaseRequestJson bj = this.GetBaseRequest();
            bj.data = pcr;
            bj.api_type = apitype;

            return getCommonReturn(bj);
        }
        /// <summary>
        /// 保存页面信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public CommonReturn SavePageInfo(pageSaveRequest obj)
        {
            string apitype = jsonApiType.savePage;
            BaseRequestJson bj = this.GetBaseRequest();
            bj.data = obj;
            bj.api_type = apitype;

            return getCommonReturn(bj);
        }
        /// <summary>
        /// 删除页面
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public CommonReturn DeletePage(string pageId)
        {
            string apitype = jsonApiType.deletePage;
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
            string apitype = jsonApiType.getCtrl;
            BaseRequestJson bj = new BaseRequestJson();
            bj.api_type = apitype;

            string result = _ccController.getReturnStr(bj);
            BaseReturnJson brj = new BaseReturnJson();
            brj = _clsDecode.DecodeBaseReturnJson(result);
            controlReturnData cr = _clsDecode.DecodecontrolReturnData(brj.data.ToString());
            return cr;
        }

        /// <summary>
        /// 更新控件配置
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public CommonReturn UpdateControlsConfig(controlUpdateRequest obj)
        {
            string apitype = jsonApiType.updateCtrl;
            BaseRequestJson bj = new BaseRequestJson();
            bj.data = obj;
            bj.api_type = apitype;

            return getCommonReturn(bj);
        }
        /// <summary>
        /// 通用返回
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private CommonReturn getCommonReturn(object obj)
        {
            string result = _ccController.getReturnStr(obj);
            BaseReturnJson brj = _clsDecode.DecodeBaseReturnJson(result);
            CommonReturn gcr = _clsDecode.DecodegroupCreateReturn(brj.data.ToString());
            return gcr;
        }
    }
}
