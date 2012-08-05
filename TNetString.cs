using System;
using System.Text;
using System.Collections.Generic;

namespace Assassin
{
	public class TNetString
	{
		public static TNetString Decode(string input)
		{
			TNetString retval = null;

			//get length
			string[] parts = input.Split(':');
			int size = System.Convert.ToInt32(parts[0]);

			//get type
			char type = input.Substring(size + parts[0].Length + 1, 1).ToCharArray()[0];

			//get body
			string body = input.Substring(parts[0].Length + 1, size);

			if (type == ',')
				retval = TNetString.Encode(body);
			else if (type == '#')
				retval = TNetString.Encode(System.Convert.ToInt32(body));
			else if (type == '^')
				retval = TNetString.Encode(System.Convert.ToDouble(body));
			else if (type == '!')
				retval = TNetString.Encode(body == "1" ? true : false);
			else if (type == '~')
				retval = TNetString.Encode(null);
			else if (type == ']')
			{
				List<object> retvalObj = new List<object>();

				int length = size;
				int activeLength = 0;
				while (activeLength < length)
				{
					string temp = body.Substring(activeLength);
					TNetString subNet = TNetString.Decode(temp);
					activeLength += subNet.EncodingLength;
					retvalObj.Add(subNet.Object);
				}

				retval = TNetString.Encode(retvalObj);
			}
			else if (type == '}')
			{
				Dictionary<string, object> retvalObj = new Dictionary<string, object>();

				int length = size;
				int activeLength = 0;
				while (activeLength < length)
				{
					TNetString subNet = null;
					string temp = null;
					string key = null;
					object val = null;

					temp = body.Substring(activeLength);
					subNet = TNetString.Decode(temp);
					key = subNet.Object.ToString();
					activeLength += subNet.EncodingLength;

					temp = body.Substring(activeLength);
					subNet = TNetString.Decode(temp);
					val = subNet.Object;
					activeLength += subNet.EncodingLength;

					retvalObj.Add(key, val);
				}

				retval = TNetString.Encode(retvalObj);
			}

			//custom data types
			else if (type == '@')
				retval = TNetString.Encode(System.Convert.ToDateTime(body));

			return retval;
		}
		public static TNetString Encode(object input)
		{
			char? type = null;
			StringBuilder data = new StringBuilder();

			//get type of data
			if (input == null)
				type = '~';
			else if (input is string)
				type = ',';
			else if (input is long || input is int)
				type = '#';
			else if (input is float || input is double)
				type = '^';
			else if (input is bool)
				type = '!';
			else if (input is List<object>)
				type = ']';
			else if (input is Dictionary<string, object>)
				type = '}';

			//special data types - not of
			else if (input is DateTime)
				type = '@';

			//create package body
			if (type.Value == '~')
				data.Append("");
			else if (type.Value == ',')
				data.Append(input.ToString());
			else if (type.Value == '^')
				data.Append(input.ToString());
			else if (type.Value == '#')
				data.Append(input.ToString());
			else if (type.Value == '!')
				data.Append(System.Convert.ToBoolean(input) ? "true" : "false");
			else if (type.Value == ']')
			{
				List<object> d = (List<object>)input;
				foreach (object val in d)
				{
					TNetString v = TNetString.Encode(val);
					data.Append(v.ToString());
					v = null;
				}
			}
			else if (type.Value == '}')
			{
				Dictionary<string, object> d = (Dictionary<string, object>)input;
				foreach (KeyValuePair<string, object> kvp in d)
				{
					TNetString k = TNetString.Encode(kvp.Key);
					TNetString v = TNetString.Encode(kvp.Value);

					data.Append(k.ToString());
					data.Append(v.ToString());

					k = null;
					v = null;
				}
			}

			//custom package types
			else if (type.Value == '@')
				data.Append(System.Convert.ToDateTime(input).ToString("yyyy-MM-ddTHH:mm:ss.fff"));

			//generate and return TNetString object
			return new TNetString(input
				, data.ToString()
				, type.Value
				);
		}

		#region Members
		private string m_data = null;
		private char? m_type = null;
		private string m_encoding = null;

		private object m_object;
		#endregion

		#region Properties
		public object Object
		{
			get
			{
				return this.m_object;
			}
		}

		public string Data
		{
			get
			{
				return this.m_data;
			}
		}
		public int DataSize
		{
			get
			{
				return this.m_data.Length;
			}
		}

		public char Type
		{
			get
			{
				return this.m_type.Value;
			}
		}

		public string Encoding
		{
			get
			{
				return this.m_encoding;
			}
		}
		public int EncodingLength
		{
			get
			{
				return this.m_encoding.Length;
			}
		}
		#endregion

		private TNetString(object obj, string data, char type)
		{
			this.m_object = obj;
			this.m_data = data;
			this.m_type = type;
			this.m_encoding = String.Format("{0}:{1}{2}"
				, this.DataSize.ToString()
				, this.Data
				, this.Type.ToString()
				);
		}

		public override string ToString()
		{
			return this.Encoding;
		}
	}
}