using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace CoreMVCColumnDefineValidate.ProjectClass
{
    public class ModelBase : IValidatableObject
    {
        /// <summary>
        /// 統一檢查欄位格式
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // 取得資料庫 DD 設定
            List<ColumnDefine> ColumnDDs = ColumnDefine.GetList();

            StringBuilder sbMsg = new StringBuilder();

            PropertyInfo[] propsM = this.GetType().GetProperties();
            for (int l = 0; l < propsM.Length; l++)
            {
                PropertyInfo pInfo = propsM[l];
                Type type = pInfo.PropertyType;

                if (pInfo.GetValue(this) != null && pInfo.GetValue(this).ToString() != "")
                {
                    string value = pInfo.GetValue(this).ToString();

                    ColumnDefine dd = ColumnDDs.Where(w => w.ColumnID.ToString() == pInfo.Name).FirstOrDefault();
                    if (dd != null)
                    {
                        // 自定名稱
                        string columnName = dd.ColumnName.ToString();
                        DisplayAttribute display = (DisplayAttribute)pInfo.GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();
                        if (display != null)
                        {
                            columnName = display.Name;
                        }
                        DisplayNameAttribute displayName = (DisplayNameAttribute)pInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault();
                        if (displayName != null)
                        {
                            columnName = displayName.DisplayName;
                        }

                        if (dd.ColumnFormat != null)
                        {
                            // 格式檢查
                            switch (dd.ColumnFormat.ToString())
                            {
                                case "INT":
                                    if (FormatCheckUtil.IsInt(value) == false)
                                    {
                                        sbMsg.AppendLine($"[{columnName}] 整數格式錯誤");
                                    }
                                    break;
                                case "NUM":
                                    if (FormatCheckUtil.IsNum(value) == false)
                                    {
                                        sbMsg.AppendLine($"[{columnName}] 數字格式錯誤");
                                    }
                                    break;
                                case "EMAIL":
                                    if (FormatCheckUtil.IsEmail(value) == false)
                                    {
                                        sbMsg.AppendLine($"[{columnName}] E-Mail 格式錯誤");
                                    }
                                    break;
                                case "PHONE":
                                    if (FormatCheckUtil.IsPhone(value) == false)
                                    {
                                        sbMsg.AppendLine($"[{columnName}] 手機格式錯誤");
                                    }
                                    break;
                                case "PID":
                                    if (FormatCheckUtil.IsPID(value) == false)
                                    {
                                        sbMsg.AppendLine($"[{columnName}] 身份證字號格式錯誤");
                                    }
                                    break;
                                case "DATE":
                                    if (FormatCheckUtil.IsDate(value) == false)
                                    {
                                        sbMsg.AppendLine($"[{columnName}] 日期格式錯誤");
                                    }
                                    break;
                            }
                        }

                        // 長度檢查
                        if (dd.ColumnMaxLength != null)
                        {
                            if (value.ToString().Length > Convert.ToInt32(dd.ColumnMaxLength))
                            {
                                sbMsg.AppendLine($"[{columnName}] 長度最多 {dd.ColumnMaxLength} 字元");
                            }
                        }
                        if (dd.ColumnMinLength != null)
                        {
                            if (value.ToString().Length < Convert.ToInt32(dd.ColumnMinLength))
                            {
                                sbMsg.AppendLine($"[{columnName}] 長度最少 {dd.ColumnMaxLength} 字元");
                            }
                        }

                        //數字範圍檢查
                        double d;
                        if (dd.ColumnRangeStart != null && double.TryParse(value, out d))
                        {
                            if (double.TryParse(value, out d))
                            {
                                if (Convert.ToDouble(value.ToString()) < Convert.ToDouble(dd.ColumnRangeStart))
                                {
                                    sbMsg.AppendLine($"[{columnName}] 數字範圍最小為 {dd.ColumnRangeStart}");
                                }
                            }

                        }
                        if (dd.ColumnRangeEnd != null)
                        {
                            if (double.TryParse(value, out d))
                            {
                                if (Convert.ToDouble(value.ToString()) > Convert.ToDouble(dd.ColumnRangeEnd))
                                {
                                    sbMsg.AppendLine($"[{columnName}] 數字範圍最大為 {dd.ColumnRangeEnd}");
                                }
                            }

                        }
                    }
                }

            }

            if (sbMsg.Length > 0)
            {
                yield return new ValidationResult(sbMsg.ToString());
            }
        }
    }
}
