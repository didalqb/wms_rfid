﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class InBillDetailService:ServiceBase<InBillDetail>,IInBillDetailService
    {
        [Dependency]
        public IInBillDetailRepository InBillDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IInBillDetailService 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            if (BillNo!=""&&BillNo!=null)
            {
                IQueryable<InBillDetail> inBillDetailQuery = InBillDetailRepository.GetQueryable();
                var inBillDetail = inBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new { i.BillNo, i.ProductCode,i.Product.ProductName,i.UnitCode,i.Unit.UnitName,i.BillQuantity, i.RealQuantity, i.Price, i.Description });
                int total = inBillDetail.Count();
                inBillDetail = inBillDetail.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = inBillDetail.ToArray() };
            }
            return "";
        }

        public new bool Add(InBillDetail inBillDetail)
        {
            IQueryable<InBillDetail> inBillDetailQuery = InBillDetailRepository.GetQueryable();
            var isExistProduct = inBillDetailQuery.FirstOrDefault(i=>i.BillNo==inBillDetail.BillNo&&i.ProductCode==inBillDetail.ProductCode);
            if (isExistProduct == null)
            {
                var ibd = new InBillDetail();
                ibd.BillNo = inBillDetail.BillNo;
                ibd.ProductCode = inBillDetail.ProductCode;
                ibd.UnitCode = inBillDetail.UnitCode;
                ibd.Price = inBillDetail.Price;
                ibd.BillQuantity = inBillDetail.BillQuantity;
                ibd.AllotQuantity = 0;
                ibd.RealQuantity = 0;
                ibd.Description = inBillDetail.Description;

                InBillDetailRepository.Add(ibd);
                InBillDetailRepository.SaveChanges();
            }
            else
            {
                var ibd = inBillDetailQuery.FirstOrDefault(i => i.BillNo == inBillDetail.BillNo);
                ibd.BillQuantity = ibd.BillQuantity + inBillDetail.BillQuantity;
                InBillDetailRepository.SaveChanges();
            }
            return true;
        }

        public bool Delete(string BillNo)
        {
            throw new NotImplementedException();
        }

        public bool Save(InBillDetail inBillDetail)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
