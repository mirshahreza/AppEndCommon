using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEndCommon
{
	public static class ExtHttpContext
	{
		public static string GetClientIp(this HttpContext context)
		{
			return context.Connection.RemoteIpAddress.MapToIPv4().ToString();
		}
		public static string GetClientAgent(this HttpContext context)
		{
			return context.Request.Headers["User-Agent"].ToString();
		}
		public static bool IsPostFace(this HttpContext context)
		{
			return context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put || context.Request.Method == HttpMethods.Patch;
		}

	}
}
