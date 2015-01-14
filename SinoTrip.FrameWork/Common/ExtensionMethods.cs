using SinoTrip.FrameWork.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace SinoTrip.FrameWork.Common
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionMethods
    {

        /// <summary>
        /// 转换为string，出错返回err
        /// </summary>
        /// <param name="str"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static string ToStringEx(this object str, string err)
        {
            if (str != null)
                return str.ToString();
            return err;

        }

        /// <summary>
        /// 时间转换为UNIX时间戳
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns>UNIX时间戳</returns>
        public static int ToUnixInt(this DateTime dt)
        {
            try
            {
                return (int)((dt.ToUniversalTime() - Constant.MinDateTime).TotalSeconds);

            }
            catch (Exception)
            {

                return 0;
            }
        }

        /// <summary>
        /// UNIX时间戳转datetime
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static DateTime UnixIntToDT(this int ts)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(Constant.MinDateTime).AddSeconds(ts);
        }


        public static decimal ToDecimal(this object obj)
        {
            try
            {
                if (obj == null || DBNull.Value == obj)
                    return 0M;

                return Convert.ToDecimal(obj);
            }
            catch (Exception)
            {

                return 0M;
            }
           
        }

        public static decimal ToDecimal(this object obj, string format)
        {
            try
            {
                if (string.IsNullOrEmpty(format))
                    return obj.ToDecimal();
                return obj.ToInt32(0).ToString(format).ToDecimal();
            }
            catch
            {
                return obj.ToDecimal();
            }
        }

        public static DateTime ToDateTime(this string dt)
        {
            try
            {
                var elstr = @"-|年|月|日";

                Regex regEx = new Regex(elstr, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                dt = regEx.Replace(dt, "/");
                return Convert.ToDateTime(dt);

            }
            catch (Exception)
            {

                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="errorNo"></param>
        /// <returns>转换错误，返回错误-255</returns>
        public static int? ToInt(this string str, int? error)
        {
            try
            {
                return int.Parse(str);
            }
            catch (Exception)
            {
                return error;
            }
        }

        /// <summary>
        /// 对象转为JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            if (obj == null)
                return string.Empty;
            if (obj is DataTable)
            {
                return DataTable2Json(obj as DataTable);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonStr = serializer.Serialize(obj);

            return jsonStr;
        }


        /// <summary>
        /// json反序列化（非二进制方式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(this string json)
        {
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
            return (T)jsonSerialize.Deserialize<T>(json);
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize1<T>(this string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
        /// <summary>
        /// xml转实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static T XmlToEntity<T>(this string xmlStr) where T : new()
        {
            var model = new T();
            try
            {
                byte[] ret = Encoding.UTF8.GetBytes(xmlStr);
                using (System.IO.MemoryStream mem = new System.IO.MemoryStream(ret))
                {

                    XmlTextWriter writer = new XmlTextWriter(mem, Encoding.UTF8);
                    XmlSerializer xz = new XmlSerializer(typeof(T));
                    model = (T)xz.Deserialize(mem);
                }
                return model;
            }
            catch (Exception ex)
            {

                return new T();
            }
        }

        /// <summary>
        /// Ext分页Json数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static string ToExtPageJson(this object obj, long total)
        {
            string rs = ToJson(obj);
            return "{'result':" + rs + ",'total':" + total + "}";
        }

        public static string ToExtFormJson(this object obj)
        {
            string data = ToJson(obj);
            return "{success:true,data:" + data + "}";
        }


        /// <summary>
        /// HttpContext填充实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T FillByForm<T>(this HttpContext context) where T : new()
        {
            T entity = new T();
            PropertyInfo[] pInfos = typeof(T).GetProperties();
            string temp;
            foreach (PropertyInfo pInfo in pInfos)
            {
                temp = context.Request.Form[pInfo.Name];
                if (temp != null)
                {
                    try
                    {
                        if (pInfo.PropertyType.IsGenericType)
                            pInfo.SetValue(entity, Convert.ChangeType(temp.Trim(), pInfo.PropertyType.GetGenericArguments()[0]), null);
                        else
                            pInfo.SetValue(entity, Convert.ChangeType(temp.Trim(), pInfo.PropertyType), null);
                    }
                    catch
                    {
                    }
                }
            }
            return entity;
        }
        /// <summary>
        ///queryString 给实体类批量赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T FillByQueryString<T>(this HttpContext context) where T : new()
        {
            T entity = new T();
            PropertyInfo[] pInfos = typeof(T).GetProperties();
            string temp;
            foreach (PropertyInfo pInfo in pInfos)
            {
                temp = context.Request.QueryString[pInfo.Name];
                if (temp != null)
                {
                    try
                    {
                        if (pInfo.PropertyType.IsGenericType)
                            pInfo.SetValue(entity, Convert.ChangeType(temp.Trim(), pInfo.PropertyType.GetGenericArguments()[0]), null);
                        else
                            pInfo.SetValue(entity, Convert.ChangeType(temp.Trim(), pInfo.PropertyType), null);
                    }
                    catch
                    {
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTable2Json(DataTable dt)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            ArrayList arrayList = new ArrayList();

            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    Type type = dataColumn.DataType;
                    var strValue = dataRow[dataColumn.ColumnName].ToString();
                    if (string.IsNullOrEmpty(strValue))
                    {
                        dictionary.Add(dataColumn.ColumnName, string.Empty);
                    }
                    else
                        dictionary.Add(dataColumn.ColumnName, Convert.ChangeType(strValue.Trim(), type));

                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }

            return javaScriptSerializer.Serialize(arrayList);  //返回一个json字符串
        }

        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }


        /// <summary>  
        /// DataTable转List
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {

            // 定义集合  
            List<T> ts = new List<T>();

            // 获得此模型的类型  
            Type type = typeof(T);
            //定义一个临时变量  
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性  
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性  
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量  
                    //检查DataTable是否包含此列（列名==对象的属性名）    
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter  
                        if (!pi.CanWrite) continue;//该属性不可写，直接跳出  
                        //取值  
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性  

                        if (value != DBNull.Value)
                        {
                            if (pi.PropertyType.IsGenericType)
                                pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType.GetGenericArguments()[0]), null);
                            else
                                pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType), null);
                        }
                    }
                }
                //对象添加到泛型集合中  
                ts.Add(t);
            }

            return ts;

        }

        /// <summary>
        /// 转为int类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt32(this object obj, int error)
        {
            try
            {
                if (obj.GetType() == typeof(decimal))
                {
                    return Decimal.ToInt32((decimal)obj);
                }
                return int.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return error;
            }
        }

        /// <summary>
        /// 转为int类型,错误返回【Int32.MinValue】
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt32(this object obj)
        {
            try
            {
                if (obj.GetType() == typeof(decimal))
                {
                    return Decimal.ToInt32((decimal)obj);
                }
                return int.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return Int32.MinValue;
            }
        }


        /// <summary>
        /// 转为小时分钟数字类型,错误返回【Int32.MinValue】
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToDateHM(this string obj)
        {
            try
            {
                return obj.Replace(":", "").ToInt32();
            }
            catch (Exception)
            {
                return 0;
            }

        }


        /// <summary>
        /// 转为long类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ToInt64(this object obj, long error)
        {
            try
            {
                return long.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return error;
            }
        }

        /// <summary>
        /// 转为long类型【错误返回long.MinValue】
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ToInt64(this object obj)
        {
            try
            {
                return long.Parse(obj.ToString());
            }
            catch (Exception)
            {
                return long.MinValue;
            }
        }

        /// <summary>  
        /// 填充对象：用DataRow填充实体类
        /// </summary>  
        public static T FillModel<T>(this DataRow row) where T : new()
        {
            if (row == null)
            {
                return default(T);
            }

            //T model = (T)Activator.CreateInstance(typeof(T));  
            T model = new T();

            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(row.Table.Columns[i].ColumnName);

                if (propertyInfo != null && row[i] != DBNull.Value)
                {
                    if (propertyInfo.PropertyType.IsGenericType)
                        propertyInfo.SetValue(model, Convert.ChangeType(row[i], propertyInfo.PropertyType.GetGenericArguments()[0]), null);
                    else
                        propertyInfo.SetValue(model, Convert.ChangeType(row[i], propertyInfo.PropertyType), null);
                }

            }
            return model;
        }

        /// <summary>
        /// DataReader填充实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rdr"></param>
        /// <returns></returns>
        public static T DataReaderToObj<T>(this SqlDataReader rdr)
        {
            T t = System.Activator.CreateInstance<T>();
            Type obj = t.GetType();

            if (rdr.Read())
            {
                // 循环字段  
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    object tempValue = null;
                    tempValue = rdr.GetValue(i);
                    var propertyInfo = obj.GetProperty(rdr.GetName(i));
                    if (propertyInfo != null && tempValue != DBNull.Value)
                    {
                        if (propertyInfo.PropertyType.IsGenericType)
                            propertyInfo.SetValue(t, Convert.ChangeType(tempValue, propertyInfo.PropertyType.GetGenericArguments()[0]), null);
                        else
                            propertyInfo.SetValue(t, tempValue, null);
                    }


                }
                return t;
            }
            else
                return default(T);

        }


    }
}
