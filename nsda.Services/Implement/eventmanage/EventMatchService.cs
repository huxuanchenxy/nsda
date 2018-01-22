using nsda.Repository;
using nsda.Services.Contract.eventmanage;
using nsda.Services.Contract.member;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.request;
using nsda.Utilities;

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 对垒业务操作
    /// </summary>
    public class EventMatchService: IEventMatchService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventMatchService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }
        //替换教室
        public bool ReplaceRoom(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventMatchService.ReplaceRoom", ex);
            }
            return flag;
        }
        //替换裁判
        public bool ReplaceReferee(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventMatchService.ReplaceReferee", ex);
            }
            return flag;
        }
        //替换对垒位置
        public bool ReplaceMatch(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventMatchService.ReplaceMatch", ex);
            }
            return flag;
        }
        //预览评分标签
        public void PreviewRatingSinglLabel()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventMatchService.PreviewRatingSinglLabel", ex);
            }
        }
        //对垒表
        public void ListMatch(ListMatchRequest request)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventMatchService.ListMatch", ex);
            }
        }

        //录入成绩
        public bool RecordOfEntry(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventMatchService.RecordOfEntry", ex);
            }
            return flag;
        }
        //评分列表
        public void RecordOfList()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventMatchService.RecordOfList", ex);
            }
        }
        //评分详情
        public void RecordOfDetail()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventMatchService.RecordOfDetail", ex);
            }
        }
    }
}
