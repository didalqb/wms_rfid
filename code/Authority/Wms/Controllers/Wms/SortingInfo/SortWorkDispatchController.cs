﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.SignalR.Dispatch.Interfaces;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.SortingInfo
{
    public class SortWorkDispatchController : Controller
    {
        [Dependency]
        public ISortWorkDispatchService SortWorkDispatchService { get; set; }

        //
        // GET: /SortWorkDispatch/
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasDelete = true;
            ViewBag.hasAudit = true;
            ViewBag.hasAntiTrial = true;
            ViewBag.hasSettle = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //查询数据
        // GET: /SortWorkDispatch/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string OrderDate = collection["OrderDate"] ?? "";
            string SortingLineCode = collection["SortingLineCode"] ?? "";
            string DispatchStatus = collection["DispatchStatus"] ?? "";
            var sortOrder = SortWorkDispatchService.GetDetails(page, rows, OrderDate, SortingLineCode, DispatchStatus);
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        ////新增数据
        //// GET: /SortWorkDispatch/Dispatch/
        //public ActionResult Dispatch(string dispatchId)
        //{
        //    SortOrderWorkDispatchService.Dispatch(dispatchId);
        //    string msg = true ? "新增成功" : "新增失败";
        //    return Json(JsonMessageHelper.getJsonMessage(true, msg, null), "text", JsonRequestBehavior.AllowGet);
        //}

        //删除数据
        // POST: /SortWorkDispatch/Delete/
        public ActionResult Delete(string id)
        {
            string errorInfo = string.Empty;
            bool bResult = SortWorkDispatchService.Delete(id,ref errorInfo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //订单审核(出库审核，移库审核，分拣作业审核)
        // POST: /SortWorkDispatch/Audit/
        public ActionResult Audit(string id)
        {
            string errorInfo = string.Empty;
            bool bResult = SortWorkDispatchService.Audit(id, this.User.Identity.Name.ToString(),ref errorInfo);
            string msg = bResult ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //订单反审(出库审核，移库审核，分拣作业审核)
        // POST: /SortWorkDispatch/AntiTrial/
        public ActionResult AntiTrial(string id)
        {
            string errorInfo = string.Empty;
            bool bResult = SortWorkDispatchService.AntiTrial(id,ref errorInfo);
            string msg = bResult ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //订单结单(出库结单，移库结单，分拣作业结单)
        // POST: /SortWorkDispatch/Settle/
        public ActionResult Settle(string id)
        {
            string errorInfo = string.Empty;
            bool bResult = SortWorkDispatchService.Settle(id, ref errorInfo);
            string msg = bResult ? "结单成功" : "结单失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        #region /SortWorkDispatch/CreateExcelToClient/
        public FileStreamResult CreateExcelToClient()
        {
            int page = 0, rows = 0;
            string orderDate = Request.QueryString["orderDate"];
            string sortingLineCode = Request.QueryString["sortingLineCode"];
            string dispatchStatus = Request.QueryString["dispatchStatus"];

            System.Data.DataTable dt = SortWorkDispatchService.GetSortWorkDispatch(page, rows, orderDate, sortingLineCode, dispatchStatus);
            string headText = "分拣作业调度";
            string headFont = "微软雅黑"; Int16 headSize = 20;
            string colHeadFont = "Arial"; Int16 colHeadSize = 10;
            string[] HeaderFooder = {   
                                         "……"  //眉左
                                        ,"……"  //眉中
                                        ,"……"  //眉右
                                        ,"&D"    //脚左 日期
                                        ,"……"  //脚中
                                        ,"&P"    //脚右 页码
                                    };
            System.IO.MemoryStream ms = THOK.Common.ExportExcel.ExportDT(dt, null, headText, null, headFont, headSize
                , 0, true, colHeadFont, colHeadSize, 0, true, 0, HeaderFooder, null, 0);
            return new FileStreamResult(ms, "application/ms-excel");
        }
        #endregion
    }
}
