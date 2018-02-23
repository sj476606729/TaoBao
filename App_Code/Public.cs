using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Public 的摘要说明
/// </summary>
namespace Room
{ 
    public class Public
    {
        public static List<string> List_Data = new List<string>();
        public static DataTable Data_Table = new DataTable();
        //public static string DataBaseUrl = "server=QT-20180210RVOF\\SQLEXPRESS;user id=sa;pwd=sj76606729;database=taobao;";
        //public static string DataBaseUrl_ = "server=QT-20180210RVOF\\SQLEXPRESS;user id=sa;pwd=sj76606729;database=Product_db;";
        public static string DataBaseUrl = "server=LAPTOP-03G3SDKH\\SQLEXPRESS;user id=sa;pwd=sj76606729;database=taobao;";
        public static string DataBaseUrl_ = "server=LAPTOP-03G3SDKH\\SQLEXPRESS;user id=sa;pwd=sj76606729;database=Product_db;";
        public Public()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
    }
}