<%@ WebHandler Language="C#" Class="FileReceive" %>

using System;
using System.Web;
using System.Data.SqlClient;

public class FileReceive : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        int a = HttpContext.Current.Request.Files.Count;
        try
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            HttpPostedFile file = files[0];
            DateTime time = DateTime.Now;
            string[] name = file.FileName.Split('.');
            string date = time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString()+time.Second.ToString() + time.Millisecond.ToString();
            SqlConnection conn = new SqlConnection(Room.Public.DataBaseUrl);
            SqlCommand comm = new SqlCommand("insert into ActiviteImage(ImageName) values('" +date+"."+name[name.Length-1]+ "')", conn);
            conn.Open();
            comm.ExecuteNonQuery();
            file.SaveAs("E:/TaoBaoPicture/活动图/"+date+"."+name[name.Length-1]);
            conn.Close();comm.Dispose();
            context.Response.Write("{\"result\":\"上传成功\"}");
        }
        catch(Exception e)
        {
            string ms = e.Message;
            context.Response.Write("{\"result\":\"保存失败，可能文件名重复！\"}");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}