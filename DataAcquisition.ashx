<%@ WebHandler Language="C#" Class="DataAcquisition" %>

using System;
using System.Web;
using System.Collections;
using operation;
using DataAcquisition_u;
using System.Collections.Generic;
public class DataAcquisition : IHttpHandler,System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        string x = HttpContext.Current.Request.Form["x"].ToString();
        GetData getdata = new GetData();
        DataAnalyzing dataanalyzing = new DataAnalyzing();
        SQLoperate sqloperate = new SQLoperate();
        Operation1 operate = new Operation1();
        Hashtable hash;
        string storage;
        switch(x)
        {
            case "HomePage":
                if (CheckAccount()==false) break;
                List<double> list = getdata.HomePageData();
                context.Response.Write("{\"OldYear\":\""+list[0]+"\",\"NowYear\":\""+list[1]+"\",\"OldMonth\":\""+list[2]+"\",\"NowMonth\":\""+list[3]+"\",\"All\":\""+list[4]+"\"}");
                break;
            case "YearChart":
                hash = dataanalyzing.YearChart();
                if (hash == null) { break; }
                IDictionaryEnumerator mIDE = hash.GetEnumerator();
                string[,] yearchart = new string[4,2];int i = 0;
                while (mIDE.MoveNext())
                {
                    yearchart[i, 0] = mIDE.Key.ToString();
                    yearchart[i,1]= mIDE.Value.ToString();i++;
                }
                context.Response.Write("{\"" + yearchart[0, 0] + "\":\"" + yearchart[0, 1] + "\",\"" + yearchart[1, 0] + "\":\"" + yearchart[1, 1] +
                    "\",\"" + yearchart[2, 0] + "\":\"" + yearchart[2, 1] + "\",\"" + yearchart[3, 0] + "\":\"" + yearchart[3, 1] + "\"}");
                break;
            case "MonthChart":
                storage = operate.ToMonthJson(dataanalyzing.MonthChart());
                context.Response.Write(storage);
                break;
            case "SearchPerson":
                storage = operate.ToOrderJson(sqloperate.SelectContract(HttpContext.Current.Request.Form["Account"].ToString()));
                context.Response.Write(storage);
                break;
            case "CalculationProfit":
                if (CheckAccount()==false) break;
                storage=operate.ToHashtableJson(operate.CalculationProfit(HttpContext.Current.Request.Form["Date"].ToString()));
                context.Response.Write(storage);break;
            case "GetImage":
                storage = sqloperate.GetImage(HttpContext.Current.Request.Form["id"].ToString());
                context.Response.Write("{\"result\":\"" + storage + "\"}");
                break;
            case "DeleteImage":
                context.Response.Write("{\"result\":\"" + sqloperate.DeleteActiviteImage(HttpContext.Current.Request.Form["name"].ToString(),HttpContext.Current.Request.Form["id"].ToString()) + "\"}");
                break;
            case "BuyerLabel":
                storage = operate.ToArrayJson();
                context.Response.Write( storage);
                break;
            case "Consignment":
                storage = operate.ToConsignmentJson(sqloperate.GetConsignment());
                context.Response.Write(storage);
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