using AppEndCommon;
using System.Collections;
using System.Data;

namespace AppEndCommon
{
	public record User
	{
		public string Id { get; set; } = "-1";
		public string UserName { get; set; } = "nobody";
		public List<string> RoleIds { set; get; } = [];
		public Hashtable ExtraInfo { set; get; } = [];
	}

	public record UserTokenFriendly
	{
		public required string Id { get; set; } 
		public required string UserName { get; set; }
		public List<string> RoleIds { set; get; } = [];
		public List<string> RoleNames { set; get; } = []; 
	}

	public record Role
	{
		public string Id { get; set; } = "-1";
		public required string RoleName { get; set; }
		public Hashtable ExtraInfo { set; get; } = [];
	}

	public static class ActorExtensions
	{
		public static string Tokenize(this User actor, string EndFix = "")
		{
			return actor.ToTokenFriendly().Encode(ProjectHelpers.EncriptionSecret);
		}

		public static UserTokenFriendly ToTokenFriendly(this User actor)
		{
			return new UserTokenFriendly
			{
				Id = actor.Id,
				UserName = actor.UserName,
				RoleIds = actor.RoleIds,
				RoleNames = [.. ProjectHelpers.ApplicationRoles.Where(i => actor.RoleIds.Contains(i.Id)).Select(i => i.RoleName)],
			};
		}

		public static string GetCacheKey(this User actor, string EndFix = "")
		{
			return $"User::{actor.Id},{actor.UserName},{EndFix}";
		}

		public static void MemoryAdd(this User actor)
		{
			actor.MemoryAdd(actor.GetCacheKey("FullObject"));
		}

	}


}

