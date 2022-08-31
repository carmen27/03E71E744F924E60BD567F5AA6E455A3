using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace BackEnd.Toolkit
{
    public class ConvertHelper
    {
        public static int GetDayOfWeek(DateTime date)
        {
            var valor = (int)date.DayOfWeek - 1;

            if (valor < 0)
            {
                return 6;
            }

            return valor;
        }

        public static IPAddress ParseIPAddress(string direccion)
        {
            if (!IsNonNull(direccion))
            {
                return null;
            }

            IPAddress ipAddress;

            if (!IPAddress.TryParse(direccion, out ipAddress))
            {
                return null;
            }

            return ipAddress;
        }

        public static MailAddress ParseMailAddress(string correo)
        {
            if (!IsNonNull(correo))
            {
                return null;
            }

            try
            {
                MailAddress objMA = new MailAddress(correo);

                if (objMA == null || objMA.Address != correo)
                {
                    return null;
                }

                return objMA;
            }
            catch
            {
                return null;
            }
        }

        public static Uri ParseUri(string cadena)
        {
            if (!IsNonNull(cadena))
            {
                return null;
            }

            try
            {
                return new Uri(cadena);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool ToBoolean(object obj)
        {
            try
            {
                return IsNonNull(obj) ? Convert.ToBoolean(obj) : false;
            }
            catch
            {
                return false;
            }
        }

        public static bool ToBoolean(object obj, bool def)
        {
            try
            {
                return IsNonNull(obj) ? Convert.ToBoolean(obj) : def;
            }
            catch
            {
                return def;
            }
        }

        public static char ToChar(object obj, char def)
        {
            if (IsNonNull(obj))
            {
                string aux = Convert.ToString(obj).Trim();

                return aux != String.Empty ? aux[0] : def;
            }
            else
            {
                return def;
            }
        }

        public static char ToChar(object obj)
        {
            string aux = string.Empty;

            try
            {
                aux = Convert.ToString(obj).Trim();
                return Convert.ToChar(aux);
            }
            catch
            {
                return ' ';
            }
        }

        public static DateTime ToDateTime(object obj, DateTime def)
        {
            return ToDateTimeWithFormat(obj, null, def);
        }

        public static DateTime ToDateTime(object obj)
        {
            return ToDateTimeWithFormat(obj, null, DateTime.Now);
        }

        public static string ToDateTimeString(object obj, string formato)
        {
            return ToDateTimeString(obj, formato, null);
        }

        public static string ToDateTimeString(object obj, string formato, string def)
        {
            if (IsNonNull(obj))
            {
                DateTime dt = DateTime.Now;

                if (DateTime.TryParse(Convert.ToString(obj), out dt))
                {
                    return dt.ToString(formato);
                }
                else
                {
                    return def;
                }
            }
            else
            {
                return def;
            }
        }

        public static DateTime ToDateTimeWithFormat(object obj, string format, DateTime def)
        {
            return ToNullDateTimeWithFormat(obj, format, def).Value;
        }

        public static DateTime ToDateTimeWithFormat(object obj, string format)
        {
            return ToDateTimeWithFormat(obj, format, DateTime.Now);
        }

        public static decimal ToDecimal(object obj)
        {
            return ToDecimal(obj, 0M);
        }

        public static decimal ToDecimal(object obj, decimal def)
        {
            try
            {
                if (obj == null)
                {
                    obj = 0;
                }

                if (String.IsNullOrEmpty(obj.ToString()))
                {
                    obj = "0";
                }

                return IsNonNull(obj) ? Convert.ToDecimal(obj) : def;
            }
            catch
            {
                return def;
            }
        }

        public static short ToInt16(object obj)
        {
            return ToInt16(obj, 0);
        }

        public static short ToInt16(object obj, short def)
        {
            try
            {
                return IsNonNull(obj) ? Convert.ToInt16(obj) : def;
            }
            catch
            {
                return def;
            }
        }

        public static int ToInt32(object obj)
        {
            return ToInt32(obj, 0);
        }

        public static int ToInt32(object obj, int def)
        {
            try
            {
                return IsNonNull(obj) ? Convert.ToInt32(obj) : def;
            }
            catch
            {
                return def;
            }
        }

        public static long ToInt64(object obj)
        {
            return ToInt64(obj, 0);
        }

        public static long ToInt64(object obj, long def)
        {
            try
            {
                return IsNonNull(obj) ? Convert.ToInt64(obj) : def;
            }
            catch (Exception)
            {
                return def;
            }
        }

        public static string ToNonNullString(object? obj)
        {
            return ToString(obj, String.Empty);
        }

        public static char? ToNullChar(object obj)
        {
            return ToNullChar(obj, null);
        }

        public static char? ToNullChar(object obj, char? def)
        {
            if (IsNonNull(obj))
            {
                string aux = ToNonNullString(obj).Trim();

                return aux != String.Empty ? aux[0] : def;
            }
            else
            {
                return def;
            }
        }

        public static DateTime? ToNullDateTime(object obj)
        {
            return ToNullDateTimeWithFormat(obj, null, null);
        }

        public static DateTime? ToNullDateTime(object obj, DateTime? def)
        {
            return ToNullDateTimeWithFormat(obj, null, def);
        }

        public static DateTime? ToNullDateTimeWithFormat(object obj, string format)
        {
            return ToNullDateTimeWithFormat(obj, format, null);
        }

        public static DateTime? ToNullDateTimeWithFormat(object obj, string format, DateTime? def)
        {
            if (IsNonNull(obj))
            {
                if (!String.IsNullOrEmpty(format))
                {
                    DateTime dt = DateTime.Now;

                    if (DateTime.TryParseExact(ToString(obj), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        return dt;
                    }
                    else
                    {
                        return def;
                    }
                }
                else
                {
                    DateTime dt = DateTime.Now;

                    if (DateTime.TryParse(ToString(obj), out dt))
                    {
                        return dt;
                    }
                    else
                    {
                        return def;
                    }
                }
            }
            else
            {
                return def;
            }
        }

        public static short? ToNullInt16(object obj, short? def)
        {
            try
            {
                return IsNonNull(obj) ? Convert.ToInt16(obj) : def;
            }
            catch
            {
                return def;
            }
        }

        public static short? ToNullInt16(object obj)
        {
            return ToNullInt16(obj, null);
        }

        public static int? ToNullInt32(object obj, int? def)
        {
            try
            {
                return IsNonNull(obj) ? Convert.ToInt32(obj) : def;
            }
            catch
            {
                return def;
            }
        }

        public static int? ToNullInt32(object obj)
        {
            return ToNullInt32(obj, null);
        }

        public static long? ToNullInt64(object obj, long? def)
        {
            try
            {
                return IsNonNull(obj) ? Convert.ToInt64(obj) : def;
            }
            catch (Exception)
            {
                return def;
            }
        }

        public static long? ToNullInt64(object obj)
        {
            return ToNullInt64(obj, null);
        }

        public static string ToString(object obj)
        {
            return ToString(obj, null);
        }

        public static string ToString(object? obj, string def)
        {
            try
            {
                return IsNonNull(obj) ? Convert.ToString(obj).Trim() : def;
            }
            catch
            {
                return def;
            }
        }

        public static ushort ToUInt16(object obj)
        {
            try
            {
                return ToUInt16(obj, 0);
            }
            catch
            {
                return 0;
            }
        }

        public static ushort ToUInt16(object obj, ushort def)
        {
            try
            {
                return IsNonNull(obj) ? Convert.ToUInt16(obj) : def;
            }
            catch
            {
                return 0;
            }
        }

        public static uint ToUInt32(object obj)
        {
            try
            {
                return ToUInt32(obj, 0);
            }
            catch
            {
                return 0;
            }
        }

        public static uint ToUInt32(object obj, uint def)
        {
            try
            {
                return IsNonNull(obj) ? Convert.ToUInt32(obj) : def;
            }
            catch
            {
                return 0;
            }
        }

        private static bool IsNonNull(object? obj)
        {
            return obj != null && obj != DBNull.Value;
        }
    }
}