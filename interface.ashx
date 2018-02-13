<%@ WebHandler Language="C#" Class="Interface" %>

using System;
using System.Web;
using operation;
using System.Collections;
public class Interface : IHttpHandler {


    public void ProcessRequest (HttpContext context) {
        string id = HttpContext.Current.Request.QueryString["id"];
        Operation1 operate = new Operation1();
        SQLoperate sqloperate =new SQLoperate();
        Hashtable hash = new Hashtable();
        string storage;
        switch(id)
        {
            case "Product":
                storage = operate.ToProductJson(sqloperate.GetProduct());
                context.Response.Write(storage);break;
            case "SearchOrder":
                string Account=HttpContext.Current.Request.QueryString["Or"];
                storage = operate.ToOrderJson(sqloperate.SelectOrderAccount(Account));
                context.Response.Write(storage);break;
            case "SearchPerson":
                string Contract=HttpContext.Current.Request.QueryString["Co"];
                storage = operate.ToOrderJson(sqloperate.SelectContract(Contract));
                context.Response.Write(storage);break;
            case "RecentOrder":
                storage = operate.ToOrderJson(sqloperate.RecentOrder(-30));
                context.Response.Write(storage);break;
            case "ModifyInventory":
                int Inventory = int.Parse(HttpContext.Current.Request.QueryString["In"]);
                string Product = HttpContext.Current.Request.QueryString["Product"];
                context.Response.Write("{\"result\":\"" + sqloperate.ModifyProduct(Inventory, Product) + "\"}");
                break;
            case "ActiviteImage":
                storage = sqloperate.GetImage("ActiviteImage");
                context.Response.Write("{\"result\":\"" + storage + "\"}");
                break;
            case "Snapshot":
                storage = sqloperate.GetImage("Snapshot");
                context.Response.Write("{\"result\":\"" + storage + "\"}");
                break;
            case "Consignment":
                storage = operate.ToConsignmentJson(sqloperate.GetConsignment());
                context.Response.Write(storage);
                break;
            case "AddConsignment":
                hash.Add("Date", HttpContext.Current.Request.QueryString["Date"]);
                hash.Add("Product",HttpContext.Current.Request.QueryString["Product"]);
                hash.Add("PaidPrice",HttpContext.Current.Request.QueryString["PaidPrice"]);
                hash.Add("Weight",HttpContext.Current.Request.QueryString["Weight"]);
                hash.Add("PostPrice",HttpContext.Current.Request.QueryString["PostPrice"]);
                hash.Add("Address",HttpContext.Current.Request.QueryString["Address"]);
                context.Response.Write("{\"result\":\"" + sqloperate.AddConsignment(hash) + "\"}");
                break;
            case "PostPrice":
                context.Response.Write("{\"result\":\"" + sqloperate.GetPostPrice(HttpContext.Current.Request.QueryString["Address"],int.Parse(HttpContext.Current.Request.QueryString["Weight"])) + "\"}");
                break;
            case "ClearPostPrice":
                context.Response.Write("{\"result\":\"" + sqloperate.ClearPostPrice() + "\"}");
                break;
            case "ModifyConsignment":
                hash.Add("ID", HttpContext.Current.Request.QueryString["Key"]);
                hash.Add("Date", HttpContext.Current.Request.QueryString["Date"]);
                hash.Add("Product", HttpContext.Current.Request.QueryString["Product"]);
                hash.Add("PaidPrice", HttpContext.Current.Request.QueryString["PaidPrice"]);
                hash.Add("Weight", HttpContext.Current.Request.QueryString["Weight"]);
                hash.Add("PostPrice", HttpContext.Current.Request.QueryString["PostPrice"]);
                hash.Add("Address", HttpContext.Current.Request.QueryString["Address"]);
                hash.Add("Clear", HttpContext.Current.Request.QueryString["Clear"]);
                context.Response.Write("{\"result\":\"" + sqloperate.ModifyConsignment(hash) + "\"}");
                break;
            case "DeleteConsignment":
                context.Response.Write("{\"result\":\"" + sqloperate.DeleteConsignment(int.Parse(HttpContext.Current.Request.QueryString["Key"])) + "\"}");
                break;

        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}