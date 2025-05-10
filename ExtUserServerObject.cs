using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEndCommon
{
	public static class ExtUserServerObject
	{
		public static string Tokenize(this UserServerObject actor, string EndFix = "")
		{
			return actor.ToClientVersion().Encode(ExtConfig.EncriptionSecret);
		}

		public static UserClientObject ToClientVersion(this UserServerObject actor)
		{
			return new UserClientObject
			{
				Id = actor.Id,
				UserName = actor.UserName,
				Roles = actor.Roles,
				IsPubKey = actor.IsPubKey
			};
		}

		public static string GetCacheKey(this UserServerObject actor, string EndFix = "")
		{
			return $"User::{actor.Id},{actor.UserName},{EndFix}";
		}

		public static void ToCache(this UserServerObject actor)
		{
			actor.AddCache(actor.GetCacheKey("FullObject"));
		}
	}
}
