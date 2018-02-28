using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using DataAcquisition_u;
using System.Collections;
using Room;
using Newtonsoft.Json;
using JsonModel;
using System.IO;
namespace operation
{
    public class Operation1
    {
        /// <summary>
        /// 将list订单数据列表转换成datatable表
        /// </summary>
        /// <param name="list1"></param>
        /// <returns></returns>
        public DataTable Load_Data(List<string> list1)
        {
            int i = 1;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("id"));
            table.Columns.Add(new DataColumn("object_id"));
            table.Columns.Add(new DataColumn("日期"));
            table.Columns.Add(new DataColumn("淘宝号"));
            table.Columns.Add(new DataColumn("产品"));
            table.Columns.Add(new DataColumn("数量"));
            table.Columns.Add(new DataColumn("应付价"));
            table.Columns.Add(new DataColumn("实付价"));
            table.Columns.Add(new DataColumn("赠品价"));
            table.Columns.Add(new DataColumn("联系方式"));
            table.Columns.Add(new DataColumn("地址"));
            table.Columns.Add(new DataColumn("重量"));
            table.Columns.Add(new DataColumn("邮费"));
            table.Columns.Add(new DataColumn("来源"));
            foreach (string _Data in list1)
            {
                string id = i.ToString() + "）," + _Data;
                string[] _data;
                _data = id.Split(',');
                table.Rows.Add(_data);
                i++;
            }
            return table;
            //Bind( gridview);

        }
 
        /// <summary>
        /// 将产品数据表准换成json数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string ToProductJson(DataTable table)
        {
            int i = 1;
            List<ProductJson> list1 = new List<ProductJson>();
            List<ProductJson> list2 = new List<ProductJson>();
            List<ProductJson> list3 = new List<ProductJson>();
            List<ProductJson> list4 = new List<ProductJson>();
            List<ProductJson> list5 = new List<ProductJson>();
            List<List<ProductJson>> list = new List<List<ProductJson>>();
            foreach (DataRow row in table.Rows)
                {

                ProductJson product=new ProductJson()
                {
                    Number = i,
                    Product = row["Product"].ToString(),SalePrice = float.Parse(row["SalePrice"].ToString()),
                    Cost = float.Parse(row["Cost"].ToString()),Specification = row["Specification"].ToString(),
                    Inventory =int.Parse( row["Inventory"].ToString()),RetailPrice=int.Parse(row["RetailPrice"].ToString()),
                    Activite = row["Activite"].ToString(),Sort = int.Parse(row["Sort"].ToString())
                };
                if(row["Product"].ToString().IndexOf("活")>=0|| row["Product"].ToString().IndexOf("补水") >= 0){
                    list1.Add(product);
                }else if (row["Product"].ToString().IndexOf("柔肤") >= 0 || row["Product"].ToString().IndexOf("美白") >= 0|| row["Product"].ToString().IndexOf("拍拍") >= 0|| row["Product"].ToString().IndexOf("滋养") >= 0)
                {
                    list2.Add(product);
                }else if (row["Product"].ToString().IndexOf("细肤") >= 0 || row["Product"].ToString().IndexOf("修护") >= 0 || row["Product"].ToString().IndexOf("肌能") >= 0 || row["Product"].ToString().IndexOf("冰晶") >= 0)
                {
                    list3.Add(product);
                }else if (row["Product"].ToString().IndexOf("cc") >= 0 || row["Product"].ToString().IndexOf("眼霜") >= 0 || row["Product"].ToString().IndexOf("凝乳") >= 0 || row["Product"].ToString().IndexOf("魔力") >= 0)
                {
                    list4.Add(product);
                }else
                {
                    list5.Add(product);
                }
                i++;
                }
            list.Add(list1); list.Add(list2); list.Add(list3); list.Add(list4); list.Add(list5);
            string jsonData = JsonConvert.SerializeObject(list);
            return jsonData;
        }
        /// <summary>
        /// 将订单数据表转换成Json
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string ToOrderJson(DataTable table)
        {
            int i = 1;
            List<OrderJson> list = new List<OrderJson>();
            foreach(DataRow row in table.Rows)
            {
                list.Add(new OrderJson()
                {
                    Number = i, Key = int.Parse(row["ID"].ToString()), Date = Convert.ToDateTime(row["Date"]).ToString("yyyy-MM-dd"),
                    Account = row["Account"].ToString(),
                    Product = row["Product"].ToString(), 
                    PayablePrice = float.Parse(row["PayablePrice"].ToString()), PaidPrice = float.Parse(row["PaidPrice"].ToString()),
                    GiftPrice = float.Parse(row["GiftPrice"].ToString()), Contract = row["Contract"].ToString(),
                    Address = row["Address"].ToString(),
                     Source = row["Source"].ToString(),
                    Remark = row["Remark"].ToString(),Label=row["Label"].ToString()
                });
                i++;
            }
            string JsonData = JsonConvert.SerializeObject(list);
            return JsonData;
        }
        /// <summary>
        /// 将全年月度销售额数据转换Json
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string ToMonthJson(DataTable table)
        {
            List<MonthName> list = new List<MonthName>();
            float[] month = new float[12];
            string year = DateTime.Now.AddYears(-1).Year.ToString();
            for(int i = 1; i < 13; i++)
            {
                foreach(DataRow row in table.Rows)
                {
                    if(year==row[0].ToString() && i.ToString() == row[1].ToString())
                    {
                        month[i-1] =float.Parse(row[2].ToString());goto start1;
                    }
                }
                month[i-1] = 0;
                start1:;
            }
            list.Add(new MonthName()
            {
                One = month[0], Two = month[1],Three=month[2],Four=month[3],Five=month[4],
                Six=month[5],Seven=month[6],Eight=month[7],Nine=month[8],Ten=month[9],
                Eleven=month[10],Twelve=month[11]

            });
            year = DateTime.Now.Year.ToString();
            for (int i = 1; i < 13; i++)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (year == row[0].ToString() && i.ToString() == row[1].ToString())
                    {
                        month[i - 1] = float.Parse(row[2].ToString()); goto start2;
                    }
                }
                month[i-1] = 0;
                start2:;
            }
            list.Add(new MonthName()
            {
                One = month[0],
                Two = month[1],
                Three = month[2],
                Four = month[3],
                Five = month[4],
                Six = month[5],
                Seven = month[6],
                Eight = month[7],
                Nine = month[8],
                Ten = month[9],
                Eleven = month[10],
                Twelve = month[11]

            });
            string JsonData = JsonConvert.SerializeObject(list);
            return JsonData;
        }
        /// <summary>
        /// 将买家账户表转换为JSON
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string ToBuyerJson(DataTable table)
        {
            int i = 1;
            List<BuyerJson> list = new List<BuyerJson>();
            foreach(DataRow row in table.Rows)
            {
                list.Add(new BuyerJson()
                {
                    Number = i,
                    Account = row["Account"].ToString(), AllPoints =float.Parse(row["AllPoints"].ToString()),
                    UsedPoints = float.Parse(row["UsedPoints"].ToString()), Remark = row["Remark"].ToString()
                });
                i++;
            }
            string JsonData = JsonConvert.SerializeObject(list);
            return JsonData;
        }
        /// <summary>
        /// 将每日数据转化成json
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string ToProductDataJson(DataTable table)
        {
            List<ProductData> list = new List<ProductData>();
            int i = 1;
            foreach (DataRow row in table.Rows)
            {
                
                list.Add(new ProductData()
                {
                    Number = i,Date=Convert.ToDateTime(row["日期"]).ToString("yyyy-MM-dd"),Fangke=int.Parse(row["访客"].ToString()),
                    liulan= int.Parse(row["浏览量"].ToString()),jiagou = int.Parse(row["加购数"].ToString()),
                    shoucang = int.Parse(row["收藏人数"].ToString()),tingliu = float.Parse(row["停留时长"].ToString())
                });
                i++;
            }
            string JsonData = JsonConvert.SerializeObject(list);
            return JsonData;
        }
        /// <summary>
        /// 将发货数据转化成json
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string ToConsignmentJson(DataTable table)
        {
            List<ConsignmentJson> list = new List<ConsignmentJson>();
            int i = 1;
            foreach(DataRow row in table.Rows)
            {
                list.Add(new ConsignmentJson()
                {
                    Number = i,
                    ID = (int)row["ID"], Date =Convert.ToDateTime( row["Date"]).ToString("yyyy\nMM-dd"),Product=row["Product"].ToString(),
                    PaidPrice=float.Parse(row["PaidPrice"].ToString()),
                    Weight=(int)row["Weight"],PostPrice=float.Parse( row["PostPrice"].ToString()),
                    Address=row["Address"].ToString(),Clear=row["Clear"].ToString()
                });
                i++;
            }
            string JsonData = JsonConvert.SerializeObject(list);
            return JsonData;
        }
        /// <summary>
        /// 计算并返回当月资金数据
        /// </summary>
        /// <param name="YearMonth"></param>
        /// <returns></returns>
        public Hashtable CalculationProfit(string YearMonth)
        {
            SQLoperate sqloperate = new SQLoperate();
            Hashtable MonthData = sqloperate.GetMonthData(YearMonth);
            Hashtable ProductCost = sqloperate.GetProductCost();
            List<string> list = sqloperate.GetMonthProduct(YearMonth);
            Hashtable result = new Hashtable();
            float Product_cost = 0, Profit, renfei, xichaoqun, zhuguangping, shajie, shajun,caozhen,PaidPrice;
            foreach(string data in list)
            {
                if (ProductCost.ContainsKey(data))
                {
                    Product_cost += float.Parse(ProductCost[data].ToString());
                }else { result.Add("产品不存在", data);return result; }
            }
            PaidPrice = float.Parse(MonthData["PaidPrice"].ToString());
            Profit = PaidPrice  - float.Parse(MonthData["PostPrice"].ToString())-Product_cost- 550;
            renfei = PaidPrice *(float)0.05;
            zhuguangping= PaidPrice * (float)0.04;
            caozhen= PaidPrice * (float)0.035;
            //shajie = (PaidPrice - Product_cost - float.Parse(MonthData["PostPrice"].ToString())) * (float)0.45 - (renfei + xichaoqun + zhuguangping + caozhen + PaidPrice * (float)0.04 + PaidPrice * (float)0.1) * (float)0.45;
            shajie = (Profit - renfei - zhuguangping  - caozhen - PaidPrice * (float)0.04- PaidPrice * (float)0.1 ) * (float)0.45+ PaidPrice * (float)0.04;
            shajun = (Profit - renfei - zhuguangping  - caozhen - PaidPrice * (float)0.04 - PaidPrice * (float)0.1) * (float)0.55 + PaidPrice * (float)0.1;
            result.Add("MonthPrice", MonthData["PaidPrice"]);
            result.Add("Profit", Profit);result.Add("PostPrice", MonthData["PostPrice"]);
            result.Add("ProductCost", Product_cost);result.Add("GiftPrice", MonthData["GiftPrice"]);
            result.Add("Expenditure", 550);result.Add("renfei", renfei);
            result.Add("zhuguangping", zhuguangping);result.Add("shajie", shajie);
            result.Add("caozhen", caozhen);
            result.Add("shajun", shajun);
            return result;
        }
        /// <summary>
        /// 将hashtable转化为Json
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public string ToHashtableJson(Hashtable hash)
        {
            string result = "{";
            foreach(DictionaryEntry d in hash)
            {
                result += "\"" + d.Key + "\":\"" + d.Value + "\",";
            }
            result = result.Substring(0, result.Length - 1) + "}";
            return result;
        }
        /// <summary>
        /// 将数组转化为Json
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public string ToArrayJson()
        {
            string[] name;float[] value;
            DataAnalyzing dataanalyzing = new DataAnalyzing();
            dataanalyzing.BuyerLabel(out name, out value);
            string result = "{";
            for(int i=0;i<name.Length;i++)
            {
                result += "\"" + name[i] + "\":\"" + value[i] + "\",";
            }
            result = result.Substring(0, result.Length - 1) + "}";
            return result;
        }
    }

     // 操作链接产品每日数据
    public class SQLProductData:Operation1
    {
        SqlConnection conn = new SqlConnection(Room.Public.DataBaseUrl_);
        SqlCommand comm;
        /// <summary>
        /// 添加产品链接每日数据
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool AddProductData(Hashtable hash,string table)
        {
            try
            {
                conn.Close(); conn.Open();
                string command = "insert into " + table + "(日期,访客,浏览量,加购数,收藏人数,停留时长) values('" +Convert.ToDateTime(hash["日期"]).ToString("yyyy-MM-dd") + "'," + hash["访客"] + "," + hash["浏览量"] + "," + hash["加购数"] + "," + hash["收藏人数"] + "," + hash["停留时长"] + ")";
                comm = new SqlCommand(command, conn);
                comm.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch(Exception e)
            {
                string ms = e.Message;
                conn.Close();comm.Dispose();
                return false;
            }
        }
        /// <summary>
        /// 获取指定链接所有数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public DataTable GetProductData(string table)
        {
            conn.Open();
            comm = new SqlCommand("select top 30 * from "+table+" order by 日期 desc", conn);
            SqlDataAdapter adapt = new SqlDataAdapter(comm);
            DataSet set = new DataSet();
            adapt.Fill(set);
            conn.Close();comm.Dispose();
            return set.Tables[0];
        }
        /// <summary>
        /// 更新产品链接每日数据
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool UpdateProductData(Hashtable hash,string table,string Date)
        {
            try
            {
                conn.Open();
                comm = new SqlCommand("update " + table + " set 日期='" + hash["日期"].ToString() + "',访客=" + hash["访客"] + ",浏览量=" + hash["浏览量"] + ",加购数=" + hash["加购数"] + ",收藏人数=" + hash["收藏人数"] + ",停留时长=" + hash["停留时长"] + " where 日期='" + Date + "'", conn);
                comm.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                string ma = e.Message;
                conn.Close();comm.Dispose();
                return false;
            }
            
        }
        /// <summary>
        /// 删除产品链接每日数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool DeleteProductData(string date,string table)
        {
            try
            {
                conn.Open();
                comm = new SqlCommand("delete from " + table + " where 日期='" + date + "'", conn);
                comm.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                string ms = e.Message;
                conn.Close();comm.Dispose();
                return false;
            }
            
        }
    }

    // 数据库操作类
    public class SQLoperate:ImageOperate
    {
        SqlConnection conn = new SqlConnection(Room.Public.DataBaseUrl);
        SqlCommand comm;
        /// <summary>
        /// 修改库存
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="PostPrice"></param>
        /// <param name="Product"></param>
        /// <returns></returns>
        public bool ModifyProduct(int Inventory, string Product)
        {
            try
            {
                conn.Open();
                comm = new SqlCommand("update Product_tb set Inventory=" + Inventory + " where Product='" + Product + "'", conn);
                comm.ExecuteNonQuery();
                conn.Close(); comm.Dispose();return true;
            }
            catch(Exception e)
            {
                string ms = e.Message;
                conn.Close();comm.Dispose();
                return false;
            }
            
        }
        /// <summary>
        /// 得到产品数据表
        /// </summary>
        /// <returns></returns>
        public DataTable GetProduct()
        {
            DataSet set = new DataSet();SqlDataAdapter adapt = new SqlDataAdapter();
            comm = new SqlCommand("select * from Product_tb order by Sort",conn);
            adapt = new SqlDataAdapter(comm);
            adapt.Fill(set);
            return set.Tables[0];
        }
        /// <summary>
        /// 添加产品数据
        /// </summary>
        /// <param name="Products"></param>
        /// <returns></returns>
        public bool CreateProduct(Hashtable Products)
        {
            try { 
                
            string name = Products["品名"].ToString();
            double saleprice =double.Parse( Products["卖价"].ToString());
            double cost =double.Parse( Products["成本"].ToString());
            string specification = Products["规格"].ToString();
            string activite = Products["活动"].ToString();
            int sort =int.Parse( Products["排序"].ToString());
                int inventory = int.Parse(Products["库存"].ToString());
                int retailprice = int.Parse(Products["零售价"].ToString());
                conn.Open();
                string sql = "insert into Product_tb(Product,SalePrice,Cost,Specification,Activite,Sort,Inventory,RetailPrice) values('" + name + "'," + saleprice + "," + cost + ",'" + specification + "','" + activite + "'," + sort + ","+inventory+","+retailprice+ ")";
                comm = new SqlCommand(sql, conn);
                comm.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                conn.Close();
                string news = e.Message;
                return false;
            }
        }
        /// <summary>
        /// 添加订单数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool CreateOrder(Hashtable table)
        {
            string command;
            bool x=false;
            float points=0;
            try
            {
                //添加积分表数据
                conn.Open();
                command = "select Account,AllPoints from Buyer_tb where Account='"+table["淘宝号"]+"'";
                comm = new SqlCommand(command, conn);
                SqlDataReader read = comm.ExecuteReader();
                read.Read();
                if (read.HasRows)
                {
                    points =float.Parse( read["AllPoints"].ToString())+float.Parse(table["实付价"].ToString());
                    read.Close();
                    command = "update Buyer_tb set AllPoints=" + points + " where Account='" + table["淘宝号"] + "'";
                    comm = new SqlCommand(command, conn);
                    comm.ExecuteNonQuery();
                }else
                {
                    read.Close();
                    points = float.Parse(table["实付价"].ToString());
                    command = "insert into Buyer_tb(Account,AllPoints,UsedPoints,Remark) values('" + table["淘宝号"] + "'," + table["实付价"] + ",0,'')";
                    comm = new SqlCommand(command, conn);
                    comm.ExecuteNonQuery();
                }
                x = true;
                //添加订单数据
                command = @"insert into Order_tb(Date,Account,Product,PayablePrice,PaidPrice,GiftPrice,Contract,Address,Source,Remark,Label) values('" +
                table["日期"] + "','" + table["淘宝号"] + "','" + table["产品"] + "'," + table["应付价"] + "," + table["实付价"] + "," + table["赠品价"] + ",'" + table["联系方式"] + "','" + table["地址"] + "','" + table["来源"] + "','"+table["备注"]+"','"+table["标签"]+ "')";
                comm = new SqlCommand(command, conn);
                comm.ExecuteNonQuery();
                conn.Close(); comm.Dispose();
                return true;
            }
            catch(Exception e)
            {
                string error = e.Message;
                conn.Close();comm.Dispose();
                if (x)//添加订单失败删除已添加积分
                {
                    conn.Open();
                     points -= float.Parse(table["实付价"].ToString());
                    command = "update Buyer_tb set AllPoints=" + points + " where Account='" + table["淘宝号"] + "'";
                    comm = new SqlCommand(command, conn);
                    comm.ExecuteNonQuery();conn.Close();comm.Dispose();
                }
                return false;
            }
            
        }
        /// <summary>
        /// 获取最近天数订单
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public DataTable RecentOrder(int day)
        {
            conn.Open();
            DateTime date = DateTime.Now.AddDays(day);
            comm = new SqlCommand("select * from Order_tb where Date>='" + date.ToString("yyyy-MM-dd")+"' order by Date desc",conn);
            SqlDataAdapter adapt = new SqlDataAdapter(comm);
            DataSet set = new DataSet();
            adapt.Fill(set);
            return set.Tables[0];
        }
        /// <summary>
        /// 搜索主键ID订单
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataTable SelectOrderID(int key)
        {
            conn.Open();
            comm = new SqlCommand("select * from Order_tb where ID=" + key, conn);
            SqlDataAdapter adapt = new SqlDataAdapter(comm);
            DataSet set = new DataSet();
            adapt.Fill(set);
            return set.Tables[0];
        }
        /// <summary>
        /// 修改订单数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool ModifyOrder(Hashtable list)
        {
            float Pre_points;
            bool M = false;
            try
            {
                conn.Open();
                string command = "update Order_tb set Date='" + list["Date"] + "',Account='" + list["Account"] + "',Product='" + list["Product"] +
                    "',PayablePrice=" + list["PayablePrice"] + ",PaidPrice=" + list["PaidPrice"] + ",GiftPrice=" + list["GiftPrice"] + ",Contract='" + list["Contract"] + "',Address='" +
                    list["Address"] + "',Source='" + list["Source"] + "',Remark='"+list["Remark"]+ "',Label='"+list["Label"]+"' where ID=" + list["Key"];
                comm = new SqlCommand(command, conn);
                comm.ExecuteNonQuery();
                M = true;
                comm = new SqlCommand("select Account,AllPoints from Buyer_tb where Account='" + list["Account"] + "'", conn);
                SqlDataReader read = comm.ExecuteReader();
                read.Read();
                Pre_points = float.Parse(read["AllPoints"].ToString());
                float Points = float.Parse(read["AllPoints"].ToString()) +float.Parse(list["PaidPrice"].ToString())- float.Parse(list["Points"].ToString());
                read.Close();
                comm = new SqlCommand("update Buyer_tb set AllPoints=" + Points + " where Account='" + list["Account"] + "'", conn);
                comm.ExecuteNonQuery();
                conn.Close();comm.Dispose();
                return true;
            }
            catch(Exception e)
            {
                string m = e.Message;
                if (M)
                {
                    comm = new SqlCommand("update Order_tb set PaidPrice=" + list["Points"] + " where ID=" + list["Key"], conn);
                    comm.ExecuteNonQuery();
                }
                conn.Close();comm.Dispose();
                return false;
            }
        }
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DeleteOrder(int key)
        {
            bool M = false;
            float Points = 0;
            string Account="";
            try
            {
                conn.Open();
                string command="select ID,PaidPrice,Account from Order_tb where ID="+key;
                comm = new SqlCommand(command, conn);
                SqlDataReader read = comm.ExecuteReader();
                read.Read();
                float PaidPrice = float.Parse(read["PaidPrice"].ToString());
                Account = read["Account"].ToString();
                read.Close();
                comm = new SqlCommand("select Account,AllPoints from Buyer_tb where Account='" + Account + "'", conn);
                read = comm.ExecuteReader();
                read.Read();
                if (read.HasRows)
                {
                    Points = float.Parse(read["AllPoints"].ToString());
                    read.Close();
                    comm = new SqlCommand("update Buyer_tb set AllPoints=" + (Points - PaidPrice) + " where Account='" + Account + "'", conn);
                    comm.ExecuteNonQuery();
                }
                M = true;
                comm = new SqlCommand("delete from Order_tb where ID=" + key, conn);
                comm.ExecuteNonQuery();
                comm.Dispose();conn.Close();return true;
            }
            catch(Exception e)
            {
                if (M==true && Points>0)
                {
                    comm = new SqlCommand("update Buyer_tb set AllPoints=" + Points + " where Account='" + Account + "'", conn);
                    comm.ExecuteNonQuery();
                }
                comm.Dispose(); conn.Close();
                return false;
            }
        }
        /// <summary>
        /// 查询部分账号订单
        /// </summary>
        /// <param name="Account_part"></param>
        /// <returns></returns>
        public DataTable SelectOrderAccount(string Account_part)
        {
            conn.Open();
            comm = new SqlCommand("select * from Order_tb where Account like '%" + Account_part + "%' order by Date desc", conn);
            SqlDataAdapter adapt = new SqlDataAdapter(comm);
            DataSet set = new DataSet();
            adapt.Fill(set);
            return set.Tables[0];

        }
        /// <summary>
        /// 查询部分联系人
        /// </summary>
        /// <param name="Contract_"></param>
        /// <returns></returns>
        public DataTable SelectContract(string Contract_)
        {
            conn.Open();
            comm = new SqlCommand("select * from Order_tb where Contract like '%" + Contract_ + "%' order by Date desc", conn);
            SqlDataAdapter adapt = new SqlDataAdapter(comm);
            DataSet set = new DataSet();
            adapt.Fill(set);
            conn.Close();comm.Dispose();
            return set.Tables[0];
        }
        /// <summary>
        /// 得到所有买家账户积分
        /// </summary>
        /// <returns></returns>
        public DataTable GetBuyer()
        {
            conn.Open();
            comm = new SqlCommand("select * from Buyer_tb order by AllPoints desc", conn);
            SqlDataAdapter adapt = new SqlDataAdapter(comm);
            DataSet set = new DataSet();
            adapt.Fill(set);conn.Close();comm.Dispose();
            return set.Tables[0];
        }
        /// <summary>
        /// 获取当月计算销售额，赠品价，邮费数据
        /// </summary>
        /// <returns></returns>
        public Hashtable GetMonthData(string YearMonth)
        {
            string[] YM = new string[2];
            Hashtable hash = new Hashtable();
            YM = YearMonth.Split('-');
            conn.Open();
            comm = new SqlCommand("select sum(PaidPrice),sum(GiftPrice) from Order_tb where year(Date)='" + YM[0] + "' and month(Date)='" + YM[1] + "'", conn);
            SqlDataReader read = comm.ExecuteReader();
            read.Read();
            hash.Add("PaidPrice", read[0]);
            hash.Add("GiftPrice", read[1]);
            read.Close();
            //hash.Add("PostPrice", read[2]);
            comm = new SqlCommand("select sum(PostPrice) from Consignment_tb where year(Date)='" + YM[0] + "' and month(Date)='" + YM[1] + "'", conn);
            read = comm.ExecuteReader();
            read.Read();
            hash.Add("PostPrice", read[0]);
            read.Close();
            conn.Close();comm.Dispose();
            return hash;
        }
        /// <summary>
        /// 获取当月卖出产品
        /// </summary>
        /// <param name="YearMonth"></param>
        /// <returns></returns>
        public List<string> GetMonthProduct(string YearMonth)
        {
            string[] YM = new string[2];
            List<string> list = new List<string>();
            YM = YearMonth.Split('-');
            conn.Open();
            comm = new SqlCommand("select Product from Order_tb where year(Date)='" + YM[0] + "' and month(Date)='" + YM[1] + "'", conn);
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())
            {
                string[] Product;
                Product = read[0].ToString().Split('+');
                foreach(string data in Product)
                {
                    string[] Product_ = data.Split('*');
                    if (Product_.Length == 1)
                    {
                        list.Add(Product_[0]);
                    }
                    else
                    {
                        for(int j = 0; j < int.Parse(Product_[1]); j++)
                        {
                            list.Add(Product_[0]);
                        }
                    }
                        
                }
            }
            conn.Close();comm.Dispose();read.Close();
            return list;

        }
        /// <summary>
        /// 获取所有产品成本
        /// </summary>
        /// <returns></returns>
        public Hashtable GetProductCost()
        {
            conn.Open();
            Hashtable hash = new Hashtable();
            comm = new SqlCommand("select Product,Cost from Product_tb", conn);
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())
            {
                hash.Add(read[0], read[1]);
            }
            read.Close();conn.Close();comm.Dispose();
            return hash;
        }
        /// <summary>
        /// 获取发货数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetConsignment()
        {
            conn.Open();
            comm = new SqlCommand("select top 30 * from Consignment_tb order by Date desc", conn);
            SqlDataAdapter adapt = new SqlDataAdapter(comm);
            DataSet set = new DataSet();
            adapt.Fill(set);
            conn.Close();comm.Dispose();
            return set.Tables[0];
        }
        /// <summary>
        /// 添加发货数据
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public bool AddConsignment(Hashtable hash)
        {
            bool add = false;
            try
            {
                conn.Open();
                string command = "insert into Consignment_tb(Date,Product,PaidPrice,Weight,PostPrice,Address) values(" +
                    "'" + Convert.ToDateTime(hash["Date"]).ToString("yyyy-MM-dd") + "','" + hash["Product"] + "'," + hash["PaidPrice"] + "," + hash["Weight"] + "," + hash["PostPrice"] + ",'" + hash["Address"] + "')";
                comm = new SqlCommand(command, conn);
                comm.ExecuteNonQuery();
                add = true;
                string[] Product_split = hash["Product"].ToString().Split('/');
                string[] Product;
                foreach( string data in Product_split)
                {
                    Product = data.Split('*');
                    int Inventory = 1,Old_In;
                    if (Product.Length > 1) { Inventory = int.Parse(Product[1]); }
                    comm = new SqlCommand("select Product,Inventory from Product_tb where Product='" + Product[0] + "'", conn);
                    SqlDataReader read = comm.ExecuteReader();
                    read.Read();
                    if (read.HasRows)
                    {
                        Old_In = (int)read["Inventory"];
                        read.Close();
                        command = "update Product_tb set Inventory=" + (Old_In- Inventory) + " where Product='" + Product[0] + "'";
                        comm = new SqlCommand(command, conn);
                        comm.ExecuteNonQuery();
                    }
                   
                }
                conn.Close(); comm.Dispose();
                return true;
            }catch(Exception e)
            {
                string ms = e.Message;
                conn.Close();comm.Dispose();
                if (add) { return true; }
                return false;
            }
            
        }
        /// <summary>
        /// 修改发货数据
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public bool ModifyConsignment(Hashtable hash)
        {
           try
            {
                conn.Open();
                comm = new SqlCommand("update Consignment_tb set Date='" + hash["Date"] + "',Product='" + hash["Product"] + "',PaidPrice=" + hash["PaidPrice"] + ",Weight=" + hash["Weight"] + ",PostPrice=" + hash["PostPrice"] + ",Address='" + hash["Address"] + "',Clear=" + hash["Clear"] + " where ID=" + hash["ID"], conn);
                comm.ExecuteNonQuery();
                conn.Close(); comm.Dispose();
                return true;
            }catch(Exception e)
            {
                string ms = e.Message;
                conn.Close();comm.Dispose();
                return false;
            }
            
            
        }
        /// <summary>
        /// 删除发货数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DeleteConsignment(int ID)
        {
            try
            {
                conn.Open();
                comm = new SqlCommand("delete from Consignment_tb where ID=" + ID, conn);
                comm.ExecuteNonQuery();
                conn.Close();comm.Dispose();
                return true;
            }catch(Exception e)
            {
                string ms = e.Message;
                conn.Close();comm.Dispose();
                return false;
            }
        }
        /// <summary>
        /// 获取地区邮费
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Weight"></param>
        /// <returns></returns>
        public string GetPostPrice(string Address,int Weight)
        {
            conn.Open();
            comm = new SqlCommand("select * from PostPrice_tb where charindex(Address,'" + Address+ "')>0", conn);
            SqlDataReader read = comm.ExecuteReader();
            read.Read();
            int result = 0;
            int result_ = 0;
            if (read.HasRows)
            {
                result= (int)read["FirstWeight"];
                if(Weight>1) { result+= (Weight - 1) * (int)read["NextWeight"]; }

            }
            read.Close();
            comm = new SqlCommand("select * from SFPostPrice_tb where charindex(Address,'" + Address + "')>0", conn);
            read = comm.ExecuteReader();
            read.Read();
            if (read.HasRows)
            {
                result_ = (int)read["FirstWeight"];
                if (Weight > 1) { result_ += (Weight - 1) * (int)read["NextWeight"]; }

            }
            return result.ToString()+" / "+result_.ToString();
        }
        /// <summary>
        /// 清算所有邮费
        /// </summary>
        /// <returns></returns>
        public bool ClearPostPrice()
        {
            try
            {
                conn.Open();
                comm = new SqlCommand("update Consignment_tb set Clear=1", conn);
                comm.ExecuteNonQuery();
                conn.Close(); comm.Dispose();
                return true;
            }
            catch(Exception e)
            {
                string ms = e.Message;
                conn.Close();comm.Dispose();
                return false;
            }
            
        }
        /// <summary>
        /// 关闭释放数据库
        /// </summary>
        /// <param name="command"></param>
        /// <param name="adapter"></param>
        private void CloseDataBase(SqlCommand command, SqlDataAdapter adapter)
        {
            command.Dispose();adapter.Dispose();
        }
        private void CloseDataBase(SqlConnection connection, SqlCommand command, SqlDataAdapter adapter)
        {
            connection.Close(); command.Dispose(); adapter.Dispose();
        }
        private void CloseDataBase(SqlConnection connection, SqlCommand command,SqlDataReader reader)
        {
            connection.Close(); command.Dispose(); reader.Close();
        }
    }
    //数据库图片操作
    public class ImageOperate
    {
        SqlConnection conn = new SqlConnection(Room.Public.DataBaseUrl);
        SqlCommand comm;
        /// <summary>
        /// 获取搜索活动图片
        /// </summary>
        /// <returns></returns>
        public string GetImage(string table)
        {
            conn.Open();
            comm = new SqlCommand("select * from "+table+" order by ImageName desc", conn);
            string result = "";
            SqlDataReader read = comm.ExecuteReader();
            string url="GiftPicture";
            if (table == "ActiviteImage")
            {
                url = "GiftPicture";
            }else if (table == "Snapshot") { url = "Snapshot"; }
            while (read.Read())
            {
                result += "http://sj476606729.oicp.net:25900/"+url+"/"+read[0].ToString()+"+";
            }
            if (result.Length > 0) { result = result.Substring(0, result.Length - 1); }
            conn.Close();comm.Dispose();read.Close();
            return result;
        }
        /// <summary>
        /// 删除活动图片
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteActiviteImage(string name,string table)
        {
            try
            {
                conn.Open();
                comm = new SqlCommand("select * from "+table+" where ImageName='" + name + "'", conn);
                SqlDataReader read = comm.ExecuteReader();
                read.Read();
                if (read.HasRows == false) { read.Close();conn.Close();comm.Dispose();return false; }
                read.Close();
                comm = new SqlCommand("delete from "+table+" where ImageName='" + name + "'", conn);
                comm.ExecuteNonQuery();
                if (table == "ActiviteImage")
                {
                    File.Delete("E:\\TaoBaoPicture\\活动图\\" + name);
                }else if (table == "Snapshot") { File.Delete("E:\\TaoBaoPicture\\打包快照\\" + name); }
                
                conn.Close(); comm.Dispose();
                return true;
            }
            catch(Exception e)
            {
                string ms = e.Message;
                conn.Close();comm.Dispose();
                return false;
            }
        }
    }
    
}
//数据采集空间
namespace DataAcquisition_u
{
    //数据分析
    public class DataAnalyzing
    {
        SqlConnection conn = new SqlConnection(Room.Public.DataBaseUrl);
        SqlCommand comm;
        /// <summary>
        /// 计算近4年各年总额
        /// </summary>
        /// <returns></returns>
        public Hashtable YearChart()
        {
            try
            {
                conn.Open();
                Hashtable list = new Hashtable();
                comm = new SqlCommand("select year(Date),sum(PaidPrice) from Order_tb  group by year(Date) order by year(Date) desc", conn);
                SqlDataReader read = comm.ExecuteReader();
                read.Read(); list.Add(read[0], read[1]);
                read.Read(); list.Add(read[0], read[1]);
                read.Read(); list.Add(read[0], read[1]);
                read.Read(); list.Add(read[0], read[1]);
                read.Close();
                conn.Close(); comm.Dispose();
                return list;
            }
            catch(Exception e)
            {
                string message = e.Message;
                conn.Close();comm.Dispose();
                return null;
            }
        }
        /// <summary>
        /// 今年和去年月度销售额
        /// </summary>
        /// <returns></returns>
        public DataTable MonthChart()
        {
            conn.Open();
            comm = new SqlCommand("select year(Date) as 年份,Month(Date) as 月份,sum(PaidPrice) as 总销售额 from Order_tb where year(Date)>='" + DateTime.Now.AddYears(-1).Year + "' group by year(Date),Month(Date) order by YEAR(Date) desc,MONTH(Date) desc", conn);
            SqlDataAdapter adapt = new SqlDataAdapter(comm);
            DataSet set = new DataSet();
            adapt.Fill(set);
            return set.Tables[0];
        }
        /// <summary>
        /// 计算买家关心重心分布
        /// </summary>
        /// <returns></returns>
        public void BuyerLabel(out string[] name,out float[] value)
        {
            conn.Open();
            comm = new SqlCommand("select Date,Label from Order_tb where Date>='" + DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd") + "'", conn);
            SqlDataReader read = comm.ExecuteReader();
            Hashtable hash = new Hashtable();
            Hashtable hash1 = new Hashtable();
            int count = 0;
            while (read.Read())
            {
                if (read["Label"].ToString() == "") { continue; }
                string[] str;
                str = read["Label"].ToString().Split('+');
                foreach(string data in str)
                {
                    if (hash.ContainsKey(data) == false)
                    {
                        hash.Add(data, 1);
                    }else { hash[data] = (int)hash[data] + 1; }
                    count++;
                }
            }
            name = new string[hash.Count];
            value = new float[hash.Count];
            int i = 0;
            foreach(DictionaryEntry list in hash)
            {
                name[i] = list.Key.ToString();value[i] = (int)list.Value;
                i++;
            }
            string storage;float storage1;
            for(int j = 0; j < name.Length - 1; j++)
            {
                for(int k = j + 1; k < name.Length; k++)
                {
                    if (value[j] < value[k]) { storage = name[j];storage1 = value[j];name[j] = name[k];value[j] = value[k];name[k] = storage;value[k] = storage1; }
                }

                value[j] = (float)Math.Round(value[j] * 100 / count, 1);
            }
            value[name.Length-1]= (float)Math.Round(value[name.Length - 1] * 100 / count, 1);
        }
    }
    //获取数据
    public class GetData
    {
        SqlConnection conn = new SqlConnection(Room.Public.DataBaseUrl);
        SqlCommand comm;
        /// <summary>
        /// 主页数据
        /// </summary>
        /// <returns></returns>
        public List<double> HomePageData()
        {
            List<double> list = new List<double>();
            conn.Open();
            string Old_date = DateTime.Now.AddYears(-1).ToString("yyyy-01-01");
            string command = "select sum(PaidPrice) from Order_tb where Date>='" + Old_date + "' and Date<'" + DateTime.Now.ToString("yyyy-01-01") + "'";
            comm = new SqlCommand(command, conn);
            SqlDataReader read = comm.ExecuteReader();//计算去年全年销售额
            read.Read();
            list.Add(double.Parse(read[0].ToString()));
            read.Close();
            command = "select sum(PaidPrice) from Order_tb where Date>='" + DateTime.Now.ToString("yyyy-01-01") + "' and Date<'" + DateTime.Now.AddYears(1).ToString("yyyy-01-01") + "'";
            comm = new SqlCommand(command, conn);
            read = comm.ExecuteReader();//计算今天全年
            read.Read();
            if (read.HasRows)
            {
                if (read[0].ToString().Length > 0) list.Add(double.Parse(read[0].ToString())); else list.Add(0);

                read.Close();
            }
            else { read.Close(); list.Add(0); }
            command = "select sum(PaidPrice) from Order_tb where Date>='" + DateTime.Now.AddYears(-1).ToString("yyyy-MM-01") + "' and Date<'" + DateTime.Now.AddYears(-1).AddMonths(1).ToString("yyyy-MM-01") + "'";
            comm = new SqlCommand(command, conn);
            read = comm.ExecuteReader();//计算去年当月
            read.Read();
            list.Add(double.Parse(read[0].ToString()));
            read.Close();
            command = "select sum(PaidPrice) from Order_tb where Date>='" + DateTime.Now.ToString("yyyy-MM-01") + "' and Date<'" + DateTime.Now.AddMonths(1).ToString("yyyy-MM-01") + "'";
            comm = new SqlCommand(command, conn);//计算今年当月
            read = comm.ExecuteReader();
            read.Read();
            if (read.HasRows)
            {
                if (read[0].ToString().Length > 0) list.Add(double.Parse(read[0].ToString())); else list.Add(0);
                read.Close();
            }
            else { read.Close(); list.Add(0); }
            command = "select sum(PaidPrice) from Order_tb";
            comm = new SqlCommand(command, conn);
            read = comm.ExecuteReader();
            read.Read();
            list.Add(double.Parse(read[0].ToString()));
            read.Close();
            conn.Close(); comm.Dispose();
            return list;
        }
    }

}
/// <summary>
/// Json数据模型类
/// </summary>
namespace JsonModel
{
    class ProductJson
    {
        public int Number { get; set; }
        public string Product { get; set; }
        public float SalePrice { get; set; }
        public float Cost { get; set; }
        public int Inventory { get; set; }
        public int RetailPrice { get; set; }
        public string Specification { get; set; }
        public string Activite { get; set; }
        public int Sort { get; set; }
    }
    class OrderJson
    {
        public int Number { get; set; }
        public int Key { get; set; }
        public string Date { get; set; }
        public string Account { get; set; }
        public string Product { get; set; }
        public float PayablePrice { get; set; }
        public float PaidPrice { get; set; }
        public float GiftPrice { get; set; }
        public string Contract { get; set; }
        public string Address { get; set; }
        public string Source { get; set; }
        public string Remark { get;set;}
        public string Label { get; set; }
    }
    class ConsignmentJson
    {
        public int Number { get; set; }
        public int ID { get; set; }
        public string Date { get; set; }
        public string Product { get; set; }
        public float PaidPrice { get; set; }
        public int Weight { get; set; }
        public float PostPrice { get; set; }
        public string Address { get; set; }
        public string Clear { get; set; }
    }
    class BuyerJson
    {
        public int Number { get; set; }
        public string Account { get; set; }
        public float AllPoints { get; set; }
        public float UsedPoints { get; set; }
        public string Remark { get; set; }
    }
    class MonthName
    {
        public float One { get; set; }
        public float Two { get; set; }
        public float Three { get; set; }
        public float Four { get; set; }
        public float Five { get; set; }
        public float Six { get; set; }
        public float Seven { get; set; }
        public float Eight { get; set; }
        public float Nine { get; set; }
        public float Ten { get; set; }
        public float Eleven { get; set; }
        public float Twelve { get; set; }
    }
    class ProductData
    {
        public int Number { get; set; }
        public string Date { get; set; }
        public int Fangke { get; set; }
        public int liulan { get; set; }
        public int jiagou { get; set; }
        public int shoucang { get; set; }
        public float tingliu { get; set; }
    }
}