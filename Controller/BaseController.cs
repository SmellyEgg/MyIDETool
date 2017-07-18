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

        public int PhotoUpload()
        {
            //这里考虑的机制是，如果是缓存的话，就调用cachecontroller进行处理
            //如果打算直接执行的，就直接调用connectController的一个公共方法进行执行
            return 1;
        }

        public object GetPageDetail(string pageID)
        {
            return null;
        }

        public UserData Login(User user)
        {
            if (object.Equals(user, null))
            {
                return null;
            }
            //
            string apitype = dataAccessDictionary.apiType.login.ToString();
            BaseRequestJson bj = new BaseRequestJson();
            bj.api_type = apitype;
            bj.data = user;
            //
            string result = _ccController.getReturnStr(bj);
            BaseReturnJson brj = _clsDecode.DecodeBaseReturnJson(result);
            UserData ud = _clsDecode.DecodeUserData(brj.data.ToString());
            return null;
        }

        public groupCreateReturn CreateGroup(groupCreateRequest gcr)
        {
            BaseRequestJson brj = new BaseRequestJson();
            brj.data = gcr;
            brj.api_type = dataAccessDictionary.apiType.groupCreate.ToString();

            string result = _ccController.getReturnStr(brj);
            BaseReturnJson brej = _clsDecode.DecodeBaseReturnJson(result);
            groupCreateReturn resultReturn = _clsDecode.DecodegroupCreateReturn(brej.data.ToString());
            return resultReturn;
        }

        /// <summary>
        /// 获取页面分组信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public pageGroupReturnData GetPageGroupInfo(string type)
        {
            string apitype = dataAccessDictionary.apiType.groupPageGet.ToString();
            BaseRequestJson bj = new BaseRequestJson();
            pageGroupData pgd = new pageGroupData(type);
            bj.api_type = apitype;
            bj.data = pgd;
            string result = _ccController.getReturnStr(bj);
            BaseReturnJson brj = _clsDecode.DecodeBaseReturnJson(result);
            pageGroupReturnData pgr = _clsDecode.DecodepageGroupReturnData(brj.data.ToString());
            return pgr;
        }

        public int UpdateGroupPageName()
        {
            return 1;
        }

        public int DeleteGroup()
        {
            return 1;
        }

        /// <summary>
        /// 新建页面
        /// </summary>
        /// <returns></returns>
        public int CreatePage()
        {
            return 1;
        }

        public int SagePageInfo(object obj)
        {
            //包括控件在内的所有页面信息
            return 1;
        }

        public int DeletePage()
        {
            //删除页面，这个还没考虑好要不要本地先进行缓存？
            //到时并发也不知道要肿么办，可以考虑加锁
            //就是提示其他需要打开或者编辑的人说，有人还没上传，所以你无法进行更改
            return 1;
        }

        /// <summary>
        /// 获取工具表信息
        /// </summary>
        /// <returns></returns>
        public object GetControlConfigInfo()
        {
            return null;
        }

        public int UpdateControlsConfig(object obj)
        {
            return 1;
        }
    }
}
