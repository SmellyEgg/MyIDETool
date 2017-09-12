using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using xinLongIDE.Controller.CommonController;
using xinLongIDE.Controller.dataDic;
using xinLongIDE.Controller.xinlongyuEnum;
using xinLongIDE.Model.Page;
using xinLongIDE.Model.requestJson;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.Controller.ConfigCMD
{
    public class pageConfigCMD
    {
        /// <summary>
        /// 离线组信息
        /// </summary>
        private string OffLineGroupInfo = System.Windows.Forms.Application.StartupPath + "\\GroupConfig\\OffLineGroupInfo.xml";
        /// <summary>
        /// 错误码
        /// </summary>
        public string errCode = string.Empty;
        /// <summary>
        /// 是否读取的是本地的配置文件
        /// </summary>
        public bool isReadLocal = false;
        /// <summary>
        /// 是否离线
        /// </summary>
        public bool isOffLine = false;

        /// <summary>
        /// 是否更新完成
        /// </summary>
        private bool _isUpdateFinished = true;

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
        public int AddGroup(string groupName, string platForm, pageGroupReturnData groupInfo)
        {
            //groupCreateRequest obj = new groupCreateRequest(groupName, platForm);
            //CommonReturn cmresult = _bsController.CreateGroup(obj);
            //string groupId = cmresult.data;
            ////添加到实体中
            pageGroupDetail newGroup = new pageGroupDetail();
            newGroup.group_id = -1;
            newGroup.group_name = groupName;

            List<pageGroupDetail> lstpagegroupdetail = new List<pageGroupDetail>();
            if (!object.Equals(groupInfo.data, null))
            {
                lstpagegroupdetail.AddRange(groupInfo.data);
            }
            lstpagegroupdetail.Add(newGroup);

            groupInfo.data = lstpagegroupdetail.ToArray();
            this.SaveCache(null, groupInfo, platForm);
            //return groupId;
            return -1;
        }

        /// <summary>
        /// 上传组信息
        /// </summary>
        /// <param name="groupInfo"></param>
        /// <returns></returns>
        public int UploadGroupInfo(pageGroupReturnData groupInfo,string platForm)
        {
            List<pageGroupDetail> listgroupInfo = new List<pageGroupDetail>();
            int groupId = -1;
            if (!object.Equals(groupInfo.data, null))
            {
                listgroupInfo.AddRange(groupInfo.data);
                foreach (pageGroupDetail obj in groupInfo.data)
                {
                    if (obj.group_id == -1)
                    {
                        groupCreateRequest objRequest = new groupCreateRequest(obj.group_name, platForm);
                        CommonReturn cmresult = _bsController.CreateGroup(objRequest);
                        groupId = xinLongyuConverter.StringToInt(cmresult.data);
                        int index = listgroupInfo.FindIndex(p => obj.group_name.Equals(p.group_name));
                        listgroupInfo[index].group_id = groupId;
                    }
                }
                groupInfo.data = listgroupInfo.ToArray();
                //上传组ID的时候, 顺便保存一下缓存
                this.SaveCache(null, groupInfo, platForm);
            }
            return 1;
        }
        /// <summary>
        /// 新增页面
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="pageName"></param>
        /// <param name="groupId"></param>
        /// <param name="platForm"></param>
        /// <param name="groupInfo"></param>
        public void AddPage(int pageid, string pageName, int groupId, string groupName, string platForm, pageGroupReturnData groupInfo)
        {
            //这里用了一大堆linq的逻辑，后续需要考虑优化一下算法
            pageDetailForGroup newpage = new pageDetailForGroup();
            newpage.page_id = pageid;
            newpage.page_name = pageName;
            List<pageGroupDetail> lstpagegroupdetail = new List<pageGroupDetail>();
            if (!object.Equals(groupInfo, null))
            {
                if (!object.Equals(groupInfo.data, null))
                {
                    lstpagegroupdetail.AddRange(groupInfo.data);
                }
                pageGroupDetail currentGroup;
                if (lstpagegroupdetail.FindIndex(p => groupId == p.group_id && groupName.Equals(p.group_name)) != -1)
                {
                    currentGroup = lstpagegroupdetail.Where(p => groupId == p.group_id && groupName.Equals(p.group_name)).ToList()[0];
                    List<pageDetailForGroup> lstpagedetail = new List<pageDetailForGroup>();
                    if (!object.Equals(currentGroup.page_list, null))
                    {
                        lstpagedetail.AddRange(currentGroup.page_list);
                    }
                    lstpagedetail.Add(newpage);
                    currentGroup.page_list = lstpagedetail.ToArray();

                    int groupIndex = lstpagegroupdetail.FindIndex(p => groupId == p.group_id && groupName.Equals(p.group_name));
                    groupInfo.data[groupIndex] = currentGroup;
                    this.SaveCache(null, groupInfo, platForm);
                }
            }


        }

        /// <summary>
        /// 获取新的页面ID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetNewPageId(pageGroupReturnData obj, pageGroupReturnData original, string platForm)
        {
            int maxId = 0;
            if (object.Equals(obj, null) && !object.Equals(obj.data, null))
            {
                if (object.Equals(original, null) && !object.Equals(original.data, null))
                {
                    return maxId;
                }
                foreach (pageGroupDetail pd in original.data)
                {
                    List<pageDetailForGroup> lstSecond = new List<pageDetailForGroup>();
                    if (!object.Equals(pd.page_list, null))
                    {
                        lstSecond.AddRange(pd.page_list);
                    }
                    if (lstSecond.Count > 0)
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
            }
            else
            {
                if (!object.Equals(obj.data, null))
                {
                    foreach (pageGroupDetail pd in obj.data)
                    {
                        List<pageDetailForGroup> lstSecond = new List<pageDetailForGroup>();
                        if (!object.Equals(pd.page_list, null))
                        {
                            lstSecond.AddRange(pd.page_list);
                        }
                        if (lstSecond.Count > 0)
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
                }
            }
            //两个平台的页面ID现在先用简单的规则进行区分，这里pc端使用的是1000之后的区间
            if (platForm.Equals(platFormEnum.pc.ToString()) && maxId < 1000)
            {
                maxId += 1000;
            }
            return maxId + 1;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        private void DeleteCache()
        {
            if (File.Exists(ConfigureFilePath.LocalOriginalGroupInfo))
            {
                File.Delete(ConfigureFilePath.LocalOriginalGroupInfo);
            }
            if (File.Exists(ConfigureFilePath.LocalGroupInfo))
            {
                File.Delete(ConfigureFilePath.LocalGroupInfo);
            }
        }

        /// <summary>
        /// 通讯业务层
        /// </summary>
        private BaseController _bsController = new BaseController();

        /// <summary>
        /// 缓存
        /// </summary>
        public Task<int> SaveCache(pageGroupReturnData originalGroupInfo, pageGroupReturnData tempGroupInfo, string platForm)
        {
            return Task.Run(() =>
            {
                _isUpdateFinished = false;
                string fileOriginalGroup = this.GetLocalOriginalGroupInfo(platForm);
                if (!object.Equals(originalGroupInfo, null) && !object.Equals(originalGroupInfo.data, null))
                {
                    if (File.Exists(fileOriginalGroup))
                    {
                        File.Delete(fileOriginalGroup);
                    }
                    xmlController.WriteToXmlFile<pageGroupReturnData>(fileOriginalGroup, originalGroupInfo);
                }
                string fileGroup = this.GetLocalGroupInfo(platForm);
                if (!object.Equals(tempGroupInfo, null) && !object.Equals(tempGroupInfo.data, null))
                {
                    if (File.Exists(fileGroup))
                    {
                        File.Delete(fileGroup);
                    }
                    xmlController.WriteToXmlFile<pageGroupReturnData>(fileGroup, tempGroupInfo);
                }
                return 1;
            });

        }

        /// <summary>
        /// 将离线里面的信息合并到其他版本信息中
        /// </summary>
        /// <param name="obj"></param>
        private void CombineOfflineGroupInfo(pageGroupReturnData obj)
        {
            isOffLine = false;
            if (File.Exists(OffLineGroupInfo))
            {
                if (object.Equals(obj, null))
                {
                    obj = new pageGroupReturnData();
                }
                pageGroupReturnData offLineObj = xmlController.ReadFromXmlFile<pageGroupReturnData>(OffLineGroupInfo);
                List<pageGroupDetail> listGroup = new List<pageGroupDetail>();
                if (!object.Equals(obj.data, null))
                {
                    listGroup.AddRange(obj.data);
                }
                if (!object.Equals(offLineObj.data, null))
                {
                    listGroup.AddRange(offLineObj.data);
                }
                obj.data = listGroup.ToArray();
                File.Delete(OffLineGroupInfo);
            }
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
                string fileName = this.GetLocalOriginalGroupInfo(platForm);
                if (File.Exists(fileName))
                {
                    return GetPageGroupInfoLocal(platForm, fileName);
                }
                else
                {
                    BaseController bc = new BaseController();
                    try
                    {
                        pageGroupReturnData obj = bc.GetPageGroupInfo(platForm);
                        xmlController.WriteToXmlFile<pageGroupReturnData>(fileName, obj);
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
            if (File.Exists(ConfigureFilePath.LocalOriginalGroupInfo))
            {
                File.Delete(ConfigureFilePath.LocalOriginalGroupInfo);
            }
        }

        /// <summary>
        /// 获取当前组信息
        /// </summary>
        /// <param name="platForm"></param>
        /// <returns></returns>
        public pageGroupReturnData GetPageGroupInfoTemp(string platForm)
        {
            string filePath = this.GetLocalGroupInfo(platForm);
            pageGroupReturnData groupInfo = GetPageGroupInfoLocal(string.Empty, filePath);
            return groupInfo;
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

        /// <summary>
        /// 下载页面时做的操作
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="groupId"></param>
        /// <param name="currentOriginalInfo"></param>
        /// <param name="currentGroupInfo"></param>
        public int DownLoadPage(int pageId, int groupId, pageGroupReturnData currentOriginalInfo, pageGroupReturnData currentGroupInfo)
        {
            List<pageGroupDetail> listGroupOriginal = new List<pageGroupDetail>();
            listGroupOriginal.AddRange(currentOriginalInfo.data);
            List<pageGroupDetail> listGrouptemp = new List<pageGroupDetail>();
            if (!object.Equals(currentGroupInfo.data, null))
            {
                listGrouptemp.AddRange(currentGroupInfo.data);
            }
            //判断是不是下载的是空分组,暂时不考虑一次性下载整个分组下所有页面信息的情况
            //if (groupId == -1)
            //{
            //    pageGroupDetail groupTemp = listGroupOriginal.Where(p => pageId.Equals(p.group_id)).ToList()[0];
            //    listGrouptemp.Add(groupTemp);
            //    currentGroupInfo.data = listGrouptemp.ToArray();
            //    return 1;
            //}

            pageGroupDetail group = listGroupOriginal.First(p => groupId.Equals(p.group_id));
            List<pageDetailForGroup> pagelist = new List<pageDetailForGroup>();
            pagelist.AddRange(group.page_list);
            //如果不包含这个组就先添加这个组,添加的这个组已经包含了要添加的页面信息，不用进行判断
            if (listGrouptemp.FindIndex(p => groupId.Equals(p.group_id)) == -1)
            {
                pageDetailForGroup page = pagelist.First(p => pageId.Equals(p.page_id));
                pageGroupDetail groupTemp = new pageGroupDetail();
                groupTemp.group_id = group.group_id;
                groupTemp.group_name = group.group_name;
                List<pageDetailForGroup> pagelistTemp = new List<pageDetailForGroup>();
                pagelistTemp.Add(page);
                groupTemp.page_list = pagelistTemp.ToArray();
                listGrouptemp.Add(groupTemp);
            }
            else
            {

                pageGroupDetail groupTemp = listGrouptemp.First(p => groupId.Equals(p.group_id));
                pageDetailForGroup page = pagelist.First(p => pageId.Equals(p.page_id));
                List<pageDetailForGroup> pagelistTemp = new List<pageDetailForGroup>();
                if (!object.Equals(groupTemp.page_list, null))
                {
                    pagelistTemp.AddRange(groupTemp.page_list);
                }
                if (pagelistTemp.FindIndex(p => pageId.Equals(p.page_id)) != -1)
                {
                    return -1;
                }
                pagelistTemp.Add(page);
                groupTemp.page_list = pagelistTemp.ToArray();
                int groupIndex = listGrouptemp.FindIndex(p => groupId.Equals(p.group_id));
                listGrouptemp[groupIndex] = groupTemp;
            }
            currentGroupInfo.data = listGrouptemp.ToArray();
            return 1;
        }

        /// <summary>
        /// 下载一整个分组信息
        /// </summary>
        public int DownLoadGroup(int groupId, pageGroupReturnData currentOriginalInfo, pageGroupReturnData currentGroupInfo)
        {
            try
            {
                List<pageGroupDetail> listCurrent = new List<pageGroupDetail>();
                List<pageGroupDetail> listOriginal = new List<pageGroupDetail>();
                if (!object.Equals(currentGroupInfo.data, null))
                {
                    listCurrent.AddRange(currentGroupInfo.data);
                }
                listOriginal.AddRange(currentOriginalInfo.data);

                int uniqueGroupId = -1;
                if (listCurrent.Count < 1)
                {
                    uniqueGroupId = 0;
                }
                else
                {
                    uniqueGroupId = listCurrent.Max(p => p.unique_id) + 1;
                }
                pageGroupDetail groupinfo = listOriginal.First(p => groupId == p.group_id);
                groupinfo.unique_id = uniqueGroupId;
                List<pageDetailForGroup> listPage = new List<pageDetailForGroup>();
                int maxUniquePageId = this.GetUniquePageId(listCurrent);

                foreach (pageDetailForGroup page in groupinfo.page_list)
                {
                    page.uniquePageid = maxUniquePageId;
                    maxUniquePageId++;
                    listPage.Add(page);
                }
                groupinfo.page_list = listPage.ToArray();
                listCurrent.Add(groupinfo);

                currentGroupInfo.data = listCurrent.ToArray();
                return 1;
            }
            catch (Exception ex)
            {
                Logging.Error("下载分组出错：" + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 获取页面唯一主键
        /// </summary>
        /// <param name="listOrigina"></param>
        /// <returns></returns>
        private int GetUniquePageId(List<pageGroupDetail> listOrigina)
        {
            List<pageDetailForGroup> listpages = new List<pageDetailForGroup>();
            foreach(pageGroupDetail group in listOrigina)
            {
                if (!object.Equals(group.page_list, null))
                {
                    listpages.AddRange(group.page_list);
                }
            }
            if (listpages.Count > 0)
            {
                return listpages.Max(p => p.uniquePageid) + 1;
            }
            else
            {
                return 0;
            }
        }

        private string GetLocalGroupInfo(string platform)
        {
            if (platFormEnum.app.ToString().Equals(platform))
            {
                return ConfigureFilePath.LocalGroupInfo;
            }
            else
            {
                return ConfigureFilePath.LocalPCGroupInfo;
            }
        }

        private string GetLocalOriginalGroupInfo(string platform)
        {
            if (platFormEnum.app.ToString().Equals(platform))
            {
                return ConfigureFilePath.LocalOriginalGroupInfo;
            }
            else
            {
                return ConfigureFilePath.LocalPCOriginalGroupInfo;
            }
        }

        /// <summary>
        /// 更改页面属性
        /// </summary>
        /// <param name="oldProperty"></param>
        /// <param name="newProperty"></param>
        /// <param name="groupInfo"></param>
        public void ChangePageProperty(object oldProperty, object newProperty, pageGroupReturnData groupInfo)
        {
            basePageProperty original = oldProperty as basePageProperty;
            basePageProperty current = newProperty as basePageProperty;
            //
            int groupIndex = 0;
            foreach(pageGroupDetail group in groupInfo.data)
            {
                if (object.Equals(group.page_list, null))
                {
                    continue;
                }
                List<pageDetailForGroup> pagelist = new List<pageDetailForGroup>();
                pagelist.AddRange(group.page_list);
                int pageIndex = pagelist.FindIndex(p => original.PageId == p.page_id);
                if (pageIndex != -1)
                {
                    pagelist[pageIndex].page_id = current.PageId;
                    pagelist[pageIndex].page_name = current.PageName;
                    groupInfo.data[groupIndex].page_list = pagelist.ToArray();
                }
                groupIndex++;
            }
        }

        public void DeleteGroup(pageGroupReturnData groupInfo, int groupId)
        {
            List<pageGroupDetail> listGroup = new List<pageGroupDetail>();
            listGroup.AddRange(groupInfo.data);
            //这个部分突然想到了一个问题，就是如果有多个新建的组的话，组ID都是一样的，这个地方是不是需要一个大的改造
            //需要一个新的自增标记，就是说存储的组信息跟从接口中获取的组信息是不一样的，要不要新建一个实体用来存放这个呢，这样的话还可以
            //直接下载已经相同页面ID的页面信息到本地，所以结论的话应该是需要进行改造


        }

        /// <summary>
        /// 删除页面
        /// </summary>
        /// <param name="groupInfo"></param>
        /// <param name="pageId"></param>
        public void DeletePage(pageGroupReturnData groupInfo, int pageId)
        {

        }

    }
}
