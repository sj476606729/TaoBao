<%@ WebHandler Language="C#" Class="Handler" %>
using System;
using System.Web;
using operation;
using System.Collections;
using System.Net;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
public class Handler : IHttpHandler,System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        string x = HttpContext.Current.Request.Form["x"].ToString();
        SqlConnection connection = new SqlConnection("server=SC-201706152014;user id=sa;pwd=sj76606729;database=taobao;");
        SqlCommand comm;SqlDataAdapter adapt;
        DataSet set;
        string storage;//储存变量
        SQLoperate sqloperate = new SQLoperate();//定义数据库操作单例
        Operation1 operate = new Operation1();//定义操作单例
        Hashtable table = new Hashtable();
        switch(x)
        {
            case "0":
                string account = HttpContext.Current.Request.Form["account"].ToString();
                HttpContext.Current.Session["account"] = account;
                HttpContext.Current.Session.Timeout = 1200;
                context.Response.Write("{\"result\":\"\"}");break;
            case "1":
                string clientIP = context.Request.UserHostAddress;//获取客户端的IP主机地址
                IPHostEntry hostEntry = Dns.GetHostEntry(clientIP);//获取IPHostEntry实体
                string Computer = hostEntry.HostName;//获取客户端计算机名称

                if (HttpContext.Current.Session["account"] != null)
                {
                    string account1 = HttpContext.Current.Session["account"].ToString();
                    if (account1 == "undefined") { account1 = "未登录"; }
                    context.Response.Write("{\"result\":\"" + account1 + "\"}"); break;
                }
                else
                {
                    if (Computer == "SC-201706152014"||Computer=="LAPTOP-03G3SDKH")
                    {
                        HttpContext.Current.Session["account"] = Computer;
                        context.Response.Write("{\"result\":\""+Computer+"\"}");break;
                    }
                }
                context.Response.Write("{}");break;
            case "order_add":
                table.Add("日期", HttpContext.Current.Request.Form["date"]);
                table.Add("淘宝号", HttpContext.Current.Request.Form["taobaohao"]);
                table.Add("产品", HttpContext.Current.Request.Form["product"]);
                table.Add("应付价", HttpContext.Current.Request.Form["payableprice"]);
                table.Add("实付价", HttpContext.Current.Request.Form["paidprice"]);
                table.Add("赠品价", HttpContext.Current.Request.Form["giftprice"]);
                table.Add("联系方式", HttpContext.Current.Request.Form["contract"]);
                table.Add("地址", HttpContext.Current.Request.Form["address"]);
                table.Add("来源", HttpContext.Current.Request.Form["source"]);
                table.Add("备注", HttpContext.Current.Request.Form["remark"]);
                table.Add("标签", HttpContext.Current.Request.Form["label"]);
                context.Response.Write("{\"result\":\""+sqloperate.CreateOrder(table)+"\"}");break;
            case "login":
                if (CheckAccount()==false) break;
                context.Response.Write("{\"result\":\"跳转\"}");break;
            case "empty_account":
                if (HttpContext.Current.Session["account"] != null)
                { HttpContext.Current.Session.Remove("account"); }
                context.Response.Write("{\"result\":\"返回\"}");break;
            case "RecentOrder":
                string day = HttpContext.Current.Request.Form["day"].ToString();
                string a = operate.ToOrderJson(sqloperate.RecentOrder(-int.Parse(day)));
                context.Response.Write(a);
                break;
            case "GetModifyOrder":
                if (CheckAccount()==false) break;
                storage = operate.ToOrderJson(sqloperate.SelectOrderID(int.Parse(HttpContext.Current.Request.Form["key"].ToString())));
                context.Response.Write(storage);break;
            case "ModifyOrder":
                Hashtable ModifyOrder_hash = new Hashtable();
                ModifyOrder_hash.Add("Date", HttpContext.Current.Request.Form["date"]);
                ModifyOrder_hash.Add("Account", HttpContext.Current.Request.Form["taobaohao"]);
                ModifyOrder_hash.Add("Product", HttpContext.Current.Request.Form["product"]);
                ModifyOrder_hash.Add("PayablePrice", HttpContext.Current.Request.Form["payableprice"]);
                ModifyOrder_hash.Add("PaidPrice", HttpContext.Current.Request.Form["paidprice"]);
                ModifyOrder_hash.Add("GiftPrice", HttpContext.Current.Request.Form["giftprice"]);
                ModifyOrder_hash.Add("Contract", HttpContext.Current.Request.Form["contract"]);
                ModifyOrder_hash.Add("Address", HttpContext.Current.Request.Form["address"]);
                ModifyOrder_hash.Add("Weight", HttpContext.Current.Request.Form["weight"]);
                ModifyOrder_hash.Add("PostPrice", HttpContext.Current.Request.Form["postprice"]);
                ModifyOrder_hash.Add("Source", HttpContext.Current.Request.Form["source"]);
                ModifyOrder_hash.Add("Remark", HttpContext.Current.Request.Form["remark"]);
                ModifyOrder_hash.Add("Label", HttpContext.Current.Request.Form["label"]);
                ModifyOrder_hash.Add("Key", HttpContext.Current.Request.Form["key"]);
                ModifyOrder_hash.Add("Points", HttpContext.Current.Request.Form["points"]);
                context.Response.Write("{\"result\":\""+sqloperate.ModifyOrder(ModifyOrder_hash).ToString()+"\"}");
                break;
            case "DeleteOrder":
                if (CheckAccount() == false) { break; }
                context.Response.Write("{\"result\":\""+sqloperate.DeleteOrder(int.Parse(HttpContext.Current.Request.Form["Key"].ToString()))+"\"}");
                break;
            case "SearchOrder":
                context.Response.Write(operate.ToOrderJson(sqloperate.SelectOrderAccount(HttpContext.Current.Request.Form["Account"].ToString())));
                break;
            case "Product":
                storage = operate.ToProductJson(sqloperate.GetProduct());
                context.Response.Write(storage);
                break;
            case "CreateProduct":
                Hashtable products = new Hashtable();
                products.Add("品名", HttpContext.Current.Request.Form["name"]);
                products.Add("卖价", HttpContext.Current.Request.Form["saleprice"]);
                products.Add("成本", HttpContext.Current.Request.Form["cost"]);
                products.Add("规格", HttpContext.Current.Request.Form["specification"]);
                products.Add("活动", HttpContext.Current.Request.Form["activite"]);
                products.Add("排序", HttpContext.Current.Request.Form["sort"]);
                products.Add("库存", HttpContext.Current.Request.Form["inventory"]);
                products.Add("零售价", HttpContext.Current.Request.Form["retailprice"]);
                storage = sqloperate.CreateProduct(products).ToString();
                context.Response.Write("{\"result\":\""+storage+"\"}");
                break;
            case "modify_pro":
                if (CheckAccount()==false) break;
                connection.Open();
                comm = new SqlCommand("select * from Product_tb where Product='" + HttpContext.Current.Request.Form["id"].ToString() + "'", connection);
                adapt = new SqlDataAdapter(comm);
                set = new DataSet();
                adapt.Fill(set);
                storage = operate.ToProductJson(set.Tables[0]);
                context.Response.Write(storage);
                adapt.Dispose();connection.Close();
                break;
            case "modify_pro_data":
                try{
                    connection.Open();
                    string name=HttpContext.Current.Request.Form["name"].ToString();
                    double saleprice=double.Parse(HttpContext.Current.Request.Form["saleprice"].ToString());
                    double cost=double.Parse(HttpContext.Current.Request.Form["cost"].ToString());
                    string specification=HttpContext.Current.Request.Form["specification"].ToString();
                    string activite=HttpContext.Current.Request.Form["activite"].ToString();
                    int sort=int.Parse(HttpContext.Current.Request.Form["sort"].ToString());
                    int inventory = int.Parse(HttpContext.Current.Request.Form["inventory"].ToString());
                    int retailprice = int.Parse(HttpContext.Current.Request.Form["retailprice"].ToString());
                    comm =new SqlCommand( "update Product_tb set Product='" + name + "', SalePrice=" + saleprice + ",Cost="+cost+",Specification='"+specification+"',Activite='"+activite+"',Sort="+sort+",Inventory="+inventory+",RetailPrice="+retailprice+
                        " where Product='" + HttpContext.Current.Request.Form["id"].ToString() + "'",connection);
                    comm.ExecuteNonQuery();
                    connection.Close();comm.Dispose();
                    context.Response.Write("{\"result\":\"修改成功\"}");
                }
                catch (Exception e) { connection.Close(); context.Response.Write("{\"result\":\"修改失败,"+e.Message+"\"}"); }
                break;
            case "delete_pro":
                if (CheckAccount()==false) break;
                try
                {
                    string id = HttpContext.Current.Request.Form["id"].ToString();
                    connection.Open();
                    comm = new SqlCommand("delete from Product_tb where Product='" + HttpContext.Current.Request.Form["id"].ToString() + "'", connection);
                    comm.ExecuteNonQuery();
                    context.Response.Write("{\"result\":\"删除成功\"}");
                }
                catch (Exception e) { context.Response.Write("{\"result\":\"删除失败\"}"); }
                break;
            case "Buyer":
                storage = operate.ToBuyerJson(sqloperate.GetBuyer());
                context.Response.Write(storage);break;
            case "ToExcel":
                if (CheckAccount()==false) break;
                context.Response.Write("{}");break;
            case "account":
                storage = HttpContext.Current.Session["account"].ToString();
                context.Response.Write("{\"result\":\"" + storage + "\"}");
                break;
        }

    }

    public bool IsReusable {
        get {
            return false;
        }
    }
    public bool CheckAccount()
    {

        if (HttpContext.Current.Session["account"] != null)
        {
            if (HttpContext.Current.Session["account"].ToString() == "SC-201706152014" ) { return true; }
            if (HttpContext.Current.Session["account"].ToString() != "a295574220" && HttpContext.Current.Session["account"].ToString() != "caozhen" && HttpContext.Current.Session["account"].ToString() != "sj476606729" && HttpContext.Current.Session["account"].ToString() != "xichaoqun") { return false; } else return true;
        }
        return false;
    }
}
