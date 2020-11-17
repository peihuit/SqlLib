using System;
using System.Collections.Generic;
using System.Text;

namespace SqlLib.Models
{
	public class RVal : Attribute
	{
		/// <summary>
		/// 回傳狀態
		/// </summary>
		public bool RStatus { get; set; }
		
		/// <summary>
		/// 回傳訊息
		/// </summary>
		public string RMsg { get; set; }

		/// <summary>
		/// 回傳的動態物件
		/// </summary>
		public dynamic DVal { get; set; }
	}
}
