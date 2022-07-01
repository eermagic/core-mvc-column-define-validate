using System.Text.RegularExpressions;

namespace CoreMVCColumnDefineValidate.ProjectClass
{
    public class FormatCheckUtil
    {
        /// <summary>
        /// 檢查格式 - 整數
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            return int.TryParse(value, out _);
        }

        /// <summary>
        /// 檢查格式 - 數字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNum(string value)
        {
            return double.TryParse(value, out _);
        }


        /// <summary>
        /// 檢查格式 - 英文&正整數
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEngAndInt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            return Regex.IsMatch(value, @"^[A-Za-z0-9]+$");
        }

        /// <summary>
        /// 檢查格式 - Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string value)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(value);
                return addr.Address.ToUpper() == value.ToUpper();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 檢查格式 - 手機
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPhone(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            return Regex.IsMatch(value, @"^09[0-9]{8}$");//規則:09開頭，後面接著8個0~9的數字
        }

        /// <summary>
        /// 檢查格式 - 市話
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTel(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            return Regex.IsMatch(value, @"^(\d{3,4}-)?\d{6,8}$");
        }

        /// <summary>
        /// 檢查格式 - 身份證
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPID(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            bool flag = Regex.IsMatch(value.ToUpper(), @"^[A-Z]{1}[1-2]{1}[0-9]{8}$");//先判定是否符合一個大寫字母+1或2開頭的1個數字+8個數字
            if (flag)//如果符合第一層格式
            {
                int Esum = 0;
                int Nsum = 0;
                int count = 0;
                string[] country = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "W", "Z", "I", "O" };
                for (int index = 0; index < country.Length; index++)
                {
                    if (value.ToUpper().Substring(0, 1) == country[index])
                    {
                        index += 10;//A是從10開始編碼,每個英文的碼都跟index差異10,先加回來
                        Esum = (((index % 10) * 9) + (index / 10));
                        //英文轉成的數字, 個位數(把數字/10取餘數)乘９再加上十位數
                        //加上十位數(數字/10,因為是int,後面會直接捨去)
                        break;
                    }
                }
                for (int i = 1; i < 9; i++)
                {//從第二個數字開始跑,每個數字*相對應權重
                    Nsum += (Convert.ToInt32(value[i].ToString())) * (9 - i);
                }
                count = 10 - ((Esum + Nsum) % 10);//把上述的總和加起來,取餘數後,10-該餘數為檢查碼,要等於最後一個數字
                if (count == Convert.ToInt32(value[9].ToString()))//判斷計算總和是不是等於檢查碼
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 檢查格式 - IP
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIP(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            return Regex.IsMatch(value, @"(\d{1,2}|1 \d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])");
        }

        /// <summary>
        /// 檢查格式 - 日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDate(string value)
        {
            value = value.Replace("/", "").Replace("-", "");
            if (!string.IsNullOrEmpty(value))
            {
                if ((value.Length != 8) && (value.Length != 7))
                {
                    return false;
                }
                try
                {
                    int year = (value.Length == 8) ? Convert.ToInt32(value.Substring(0, 4)) : (Convert.ToInt32(value.Substring(0, 3)) + 0x777);
                    int month = (value.Length == 8) ? Convert.ToInt32(value.Substring(4, 2)) : Convert.ToInt32(value.Substring(3, 2));
                    int day = (value.Length == 8) ? Convert.ToInt32(value.Substring(6, 2)) : Convert.ToInt32(value.Substring(5, 2));
                    if ((month < 1) || (month > 12))
                    {
                        return false;
                    }
                    if ((day < 1) || (day > 0x1f))
                    {
                        return false;
                    }
                    if ((((month == 4) || (month == 6)) || ((month == 9) || (month == 11))) && (day == 0x1f))
                    {
                        return false;
                    }
                    if (month == 2)
                    {
                        bool isleap = ((year % 4) == 0) && (((year % 100) != 0) || ((year % 400) == 0));
                        if ((day > 0x1d) || ((day == 0x1d) && !isleap))
                        {
                            return false;
                        }
                    }
                }
                catch
                {
                    return false;
                }

            }
            return true;
        }
    }
}
