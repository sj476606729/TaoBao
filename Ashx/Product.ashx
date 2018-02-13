<%@ WebHandler Language="C#" Class="Product" %>

using System;
using System.Web;
using operation;
using System.Collections.Generic;
using System.Collections;
public class Product : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        string x = HttpContext.Current.Request.Form["x"].ToString();
        SQLProductData sqlproduct = new SQLProductData();
        Hashtable hash = new Hashtable();
        string storage;
        switch (x)
        {
            case "添加每日数据":
                hash.Add("日期", HttpContext.Current.Request.Form["date"]);
                hash.Add("访客", HttpContext.Current.Request.Form["fangke"]);
                hash.Add("浏览量", HttpContext.Current.Request.Form["liulan"]);
                hash.Add("加购数", HttpContext.Current.Request.Form["jiagou"]);
                hash.Add("收藏人数", HttpContext.Current.Request.Form["shoucang"]);
                hash.Add("停留时长", HttpContext.Current.Request.Form["tingliu"]);
                context.Response.Write("{\"result\":\""+sqlproduct.AddProductData(hash, HttpContext.Current.Request.Form["id"].ToString())+"\"}");
                break;
            case "GetProductData":
                storage = sqlproduct.ToProductDataJson(sqlproduct.GetProductData(HttpContext.Current.Request.Form["id"].ToString()));
                context.Response.Write(storage);break;
            case "更新每日数据":
                hash.Add("日期", HttpContext.Current.Request.Form["date"]);
                hash.Add("访客", HttpContext.Current.Request.Form["fangke"]);
                hash.Add("浏览量", HttpContext.Current.Request.Form["liulan"]);
                hash.Add("加购数", HttpContext.Current.Request.Form["jiagou"]);
                hash.Add("收藏人数", HttpContext.Current.Request.Form["shoucang"]);
                hash.Add("停留时长", HttpContext.Current.Request.Form["tingliu"]);
                context.Response.Write("{\"result\":\""+sqlproduct.UpdateProductData(hash, HttpContext.Current.Request.Form["id"].ToString(),HttpContext.Current.Request.Form["OldDate"].ToString())+"\"}");
                break;
            case "删除每日数据":
                context.Response.Write("{\"result\":\""+sqlproduct.DeleteProductData(HttpContext.Current.Request.Form["date"].ToString(),HttpContext.Current.Request.Form["id"].ToString())+"\"}");
                break;
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}