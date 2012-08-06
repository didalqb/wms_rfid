﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IDailyBalanceService : IService<DailyBalance>
    {
        object GetDetails(int page, int rows, string beginDate, string endDate, string warehouseCode);
        object GetInfoDetails(int page, int rows, string warehouseCode, string settleDate);
        object GetDailyBalanceInfos(int page, int rows, string warehouseCode, string settleDate);
        bool DoDailyBalance(string warehouseCode, string settleDate);
    }
}
