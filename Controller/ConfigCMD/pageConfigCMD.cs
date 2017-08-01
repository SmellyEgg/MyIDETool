using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using xinLongIDE.Model.requestJson;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.Controller.ConfigCMD
{
    public class pageConfigCMD
    {
        /// <summary>
        /// 本地组信息
        /// </summary>
        private string LocalGroupInfo = System.Windows.Forms.Application.StartupPath + "\\GroupConfig\\localGroupInfo.xml";
        /// <summary>
        /// 本地原始组信息
        /// </summary>
        private string LocalOriginalGroupInfo = System.Windows.Forms.Application.StartupPath + "\\GroupConfig\\LocalOriginalGroupInfo.xml";

        public string errCode = string.Empty;
        /// <summary>
        /// 是否读取的是本地的配置文件
        /// </summary>
        public bool isReadLocal = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public pageConfigCMD()
        {
            
        }
        /// <summary>
        /// 添加组
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="platForm"></param>
        /// <param name="groupInfo">当前的组信息</param>
        public string AddGroup(string groupName, string platForm, pageGroupReturnData groupInfo)
        {
            groupCreateRequest obj = new groupCreateRequest(groupName, platForm);
            CommonReturn cmresult = _bsController.CreateGroup(obj);
            string groupId = cmresult.data;
            //添加到实体中
            pageGroupDetail newGroup = new pageGroupDetail();
            newGroup.group_id = groupId;
            newGroup.group_name = groupName;

            List<pageGroupDetail> lstpagegroupdetail = new List<pageGroupDetail>();
            lstpagegroupdetail.AddRange(groupInfo.data);
            lstpagegroupdetail.Add(newGroup);

            groupInfo.data = lstpagegroupdetail.ToArray();
            return groupId;
        }

        /// <summary>
        /// 新增页面
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="pageName"></param>
        /// <param name="groupId"></param>
        /// <param name="platForm"></param>
        /// <param name="groupInfo"></param>
        public void AddPage(int pageid, string pageName, string groupId, string platForm, pageGroupReturnData groupInfo)
        {
            //这里用了一大堆linq的逻辑，后续需要考虑优化一下算法
            pageDetailForGroup newpage = new pageDetailForGroup();
            newpage.page_id = pageid;
            newpage.page_name = pageName;
            List<pageGroupDetail> lstpagegroupdetail = new List<pageGroupDetail>();
            lstpagegroupdetail.AddRange(groupInfo.data);

            pageGroupDetail currentGroup = lstpagegroupdetail.First(p => groupId.Equals(p.group_id));
            List<pageDetailForGroup> lstpagedetail = new List<pageDetailForGroup>();
            lstpagedetail.AddRange(currentGroup.page_list);
            lstpagedetail.Add(newpage);
            currentGroup.page_list = lstpagedetail.ToArray();

            int groupIndex = lstpagegroupdetail.FindIndex(p => groupId.Equals(p.group_id));
            groupInfo.data[groupIndex] = currentGroup;
        }

        /// <summary>
        /// 获取新的页面ID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetNewPageId(pageGroupReturnData obj)
        {
            int maxId = 0;
            foreach (pageGroupDetail pd in obj.data)
            {
                List<pageDetailForGroup> lstSecond = new List<pageDetailForGroup>();
                lstSecond.AddRange(pd.page_list);
                if (lstSecond.Count > 1)
                {
                    int id = lstSecond.Max(x => x.page_id);
                    if (id > maxId)
                    {
                        maxId = id;
                    }
                    lstSecond.Clear();
                    lstSecond = null;
                }
            }
            return maxId;
        }

        public void SavePage()
        {
            
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        private void DeleteCache()
        {
            if (File.Exists(LocalOriginalGroupInfo))
            {
                File.Delete(LocalOriginalGroupInfo);
            }
            if (File.Exists(LocalGroupInfo))
            {
                File.Delete(LocalGroupInfo);
            }
        }

        /// <summary>
        /// 通讯业务层
        /// </summary>
        private BaseController _bsController = new BaseController();
       
        private void DemoTest(string name, string id)
        {
            //ControlDetailForRequest objtext = new ControlDetailForRequest();
            //objtext.ctrl_id = "50";
            //objtext.ctrl_level = "10";
            //objtext.ctrl_type = "text";
            //objtext.d0 = "测试页";
            //objtext.d1 = "320";
            //objtext.d2 = "44";
            //objtext.d3 = "0";
            //objtext.d4 = "0";
            //objtext.d6 = "18";
            //objtext.d7 = "#22798A";
            //objtext.d8 = "#ffffff";
            //objtext.d18 = "1";
            //objtext.d19 = "1";
            //objtext.d26 = "0";
            //objtext.d36 = "15";

            //ControlDetailForRequest objimg = new ControlDetailForRequest();
            //objimg.ctrl_id = "51";
            //objimg.ctrl_level = "2";
            //objimg.ctrl_type = "img";
            //objimg.d0 = "http://img.sootuu.com/vector/2007-07-01/068/3/200.gif";
            //objimg.d1 = "44";
            //objimg.d2 = "44";
            //objimg.d3 = "0";
            //objimg.d4 = "0";
            //objimg.d5 = "1";
            //objimg.d6 = "11";
            //objimg.d7 = "#000000";
            //objimg.d11 = "#ffffff";
            //objimg.d18 = "1";
            //objimg.d19 = "1";
            //objimg.d26 = "0";
            //objimg.d36 = "15";

            //request.ctrls = new ControlDetailForRequest[] { objPage, objNavigationBar, objtext, objimg };

            //CommonReturn objReturn = _bsController.SavePageInfo(request);
        }

        /// <summary>
        /// 缓存
        /// </summary>
        public Task<int> SaveCache(pageGroupReturnData originalGroupInfo, pageGroupReturnData tempGroupInfo)
        {
            return Task.Run(() =>
            {
                if (!object.Equals(originalGroupInfo, null))
                {
                    if (File.Exists(LocalOriginalGroupInfo))
                    {
                        File.Delete(LocalOriginalGroupInfo);
                    }
                    xmlController.WriteToXmlFile<pageGroupReturnData>(LocalOriginalGroupInfo, originalGroupInfo);
                }
                if (!object.Equals(tempGroupInfo, null))
                {
                    if (File.Exists(LocalGroupInfo))
                    {
                        File.Delete(LocalGroupInfo);
                    }
                    xmlController.WriteToXmlFile<pageGroupReturnData>(LocalGroupInfo, tempGroupInfo);
                }
                return 1;
            });

        }
        /// <summary>
        /// 异步获取页面信息
        /// </summary>
        /// <param name="platForm"></param>
        /// <returns></returns>
        public Task<pageGroupReturnData> GetPageGroupInfo(string platForm)
        {
            return Task.Run(() =>
            {
                if (File.Exists(LocalGroupInfo))
                {
                    isReadLocal = true;
                    return GetPageGroupInfoLocal(platForm, LocalGroupInfo);
                }
                else
                {
                    isReadLocal = false;
                    BaseController bc = new BaseController();
                    try
                    {
                        pageGroupReturnData obj = bc.GetPageGroupInfo(platForm);
                        xmlController.WriteToXmlFile<pageGroupReturnData>(LocalGroupInfo, obj);
                        //
                        xmlController.WriteToXmlFile<pageGroupReturnData>(LocalOriginalGroupInfo, obj);
                        return obj;
                    }
                    catch (Exception ex)
                    {
                        errCode = "获取页面信息出错:" + ex.Message;
                        return null;
                    }
                }

            });
        }

        /// <summary>
        /// 获取原始组信息
        /// </summary>
        /// <param name="platForm"></param>
        /// <returns></returns>
        public Task<pageGroupReturnData> GetOriginalPageInfo(string platForm)
        {
            return Task.Run(() =>
            {
                if (File.Exists(LocalOriginalGroupInfo))
                {
                    isReadLocal = true;
                    return GetPageGroupInfoLocal(platForm, LocalOriginalGroupInfo);
                }
                else
                {
                    isReadLocal = false;
                    BaseController bc = new BaseController();
                    try
                    {
                        pageGroupReturnData obj = bc.GetPageGroupInfo(platForm);
                        xmlController.WriteToXmlFile<pageGroupReturnData>(LocalOriginalGroupInfo, obj);
                        return obj;
                    }
                    catch (Exception ex)
                    {
                        errCode = "获取页面信息出错:" + ex.Message;
                        return null;
                    }
                }

            });
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            if (File.Exists(LocalOriginalGroupInfo))
            {
                File.Delete(LocalOriginalGroupInfo);
            }
        }

        /// <summary>
        /// 获取本地页面配置信息
        /// </summary>
        /// <param name="platForm"></param>
        /// <returns></returns>
        private pageGroupReturnData GetPageGroupInfoLocal(string platForm, string filepath)
        {
            try
            {
                pageGroupReturnData obj = xmlController.ReadFromXmlFile<pageGroupReturnData>(filepath);
                return obj;
            }
            catch (Exception ex)
            {
                errCode = "获取页面信息出错:" + ex.Message;
                return null;
            }
        }

    }
}
