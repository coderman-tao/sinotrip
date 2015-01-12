using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace SinoTrip.FrameWork.IO
{
    public static class FileCache
    {
        private static string CachePath;
        /// <summary>
        /// 读写操作锁
        /// </summary>
        static ReaderWriterLock rwl = new ReaderWriterLock();

        private static IFormatter formatter = new BinaryFormatter();
        static FileCache()
        {
            CachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["fileCachePath"].ToString());

            if (!Directory.Exists(CachePath))
                Directory.CreateDirectory(CachePath);
        }


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetCache(string name, object value, string folder)
        {
            FileStream fs = null;
            try
            {
                if (string.IsNullOrEmpty(name) || value == null)
                {
                    return false;
                }
                string path = CachePath;
                if (!string.IsNullOrEmpty(folder))
                {
                    path = Path.Combine(CachePath, folder);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                name = Path.Combine(path, name + ".dat");
                rwl.AcquireWriterLock(1000);//1秒未获取到写权限则放弃
                fs = new FileStream(name, FileMode.Create);
                formatter.Serialize(fs, value);

                return true;
            }
            catch (Exception ex)
            {

                Trace.WriteLine("写入缓存" + name + "失败" + DateTime.Now.ToString() + ":" + ex.Message);
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (rwl.IsWriterLockHeld)
                {
                    rwl.ReleaseWriterLock();
                }
            }
        }
        /// <summary>
        /// 读取文件缓存
        /// </summary>
        /// <param name="name"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public static object ReadCache(string name, DateTime last, string folder)
        {
            Stream stream = null;
            try
            {
                string path = CachePath;
                if (!string.IsNullOrEmpty(folder))
                {
                    path = Path.Combine(CachePath, folder);
                }
                name = Path.Combine(path, name + ".dat");
                var fi = new FileInfo(name);
                if (fi.LastWriteTime < last)
                {
                    return null;
                }

                rwl.AcquireReaderLock(1000);//读锁，1秒后未获取放弃
                stream = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read);
                var obj = formatter.Deserialize(stream);

                return obj;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("读取缓存" + name + "失败" + DateTime.Now.ToString() + ":" + ex.Message);
                return null;
            }
            finally
            {
                if (rwl.IsReaderLockHeld)
                {
                    rwl.ReleaseReaderLock();
                }
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }

        }
    }
}
