using Dapper;
using Microsoft.Extensions.Caching.Memory;
using System.Data.SqlClient;

namespace CoreMVCColumnDefineValidate.ProjectClass
{
    public class ColumnDefine
    {
        static MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

        public object ColumnID { get; set; }
        public object ColumnName { get; set; }
        public object ColumnFormat { get; set; }
        public object ColumnMaxLength { get; set; }
        public object ColumnMinLength { get; set; }
        public object ColumnRangeStart { get; set; }
        public object ColumnRangeEnd { get; set; }

        /// <summary>
        /// 取得資料庫定義
        /// </summary>
        /// <returns></returns>
        public static List<ColumnDefine> GetList()
        {
            List<ColumnDefine> list = null;

            // 使用 Cache 暫存在記憶體內
            if (!cache.TryGetValue("ColumnDefine", out list))
            {
                // 不存在 Cache 時才從資料庫內取得設定
                IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
                string connStr = conf.GetConnectionString("SqlServer");
                using (var cn = new SqlConnection(connStr))
                {
                    string sql = "SELECT * FROM ColumnDefine";
                    list = (List<ColumnDefine>)cn.Query<ColumnDefine>(sql);
                }

                // 寫入 Cache
                MemoryCacheEntryOptions policy = new MemoryCacheEntryOptions();
                // 多久時間沒有使用 cache 就回收
                policy.SlidingExpiration = TimeSpan.FromHours(1);// 一小時
                cache.Set("ColumnDefine", list, policy);
            }
            return list;
        }
    }
}
