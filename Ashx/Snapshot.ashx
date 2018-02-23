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
            string date =time.Year.ToString()+ time.Month.ToString("00") + time.Day.ToString("00") + time.Hour.ToString("00") + time.Minute.ToString("00")+time.Second.ToString("00") + time.Millisecond.ToString("000")+"1";
            string[] name = file.FileName.Split('.');
            SqlConnection conn = new SqlConnection(Room.Public.DataBaseUrl);
            SqlCommand comm = new SqlCommand("insert into Snapshot(ImageName) values('" +date+"."+name[name.Length-1] + "')", conn);
            conn.Open();
            comm.ExecuteNonQuery();
            file.SaveAs("E:/TaoBaoPicture/打包快照/"+date+"."+name[name.Length-1]);
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