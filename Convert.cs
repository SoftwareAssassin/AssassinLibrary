using System;
namespace Assassin
{
	public static class Convert
	{
		public static string ToTSqlValueFormat(object input)
		{
			if (input == null)
				return "NULL";
			else if (input is string)
				return String.Format("N'{0}'", input.ToString());
			else if (input is short)
				return input.ToString();
			else if (input is int)
				return input.ToString();
			else if (input is long)
				return input.ToString();
			else if (input is decimal)
				return input.ToString();
			else if (input is double)
				return input.ToString();
			else if (input is DateTime)
				return "CONVERT(DATETIME,'" + ((DateTime)input).ToString("yyyy/MM/dd HH:mm:ss") + "',102)";
			else if (input is bool)
				return (bool)input ? "1" : "0";
			else
				return null;
		}
	}
}
