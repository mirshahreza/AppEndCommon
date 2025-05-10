using AppEndCommon;
using System.Collections;
using System.Data;
using System.Text.Json.Nodes;

namespace AppEndCommon
{
	public record UserClientObject
	{
		public required int Id { get; set; } = 0;
		public required string UserName { get; set; }
		public List<Role> Roles { set; get; } = [];
		public bool IsPubKey { get; set; } = false;
		public DateTime GeneratedOn { get; } = DateTime.Now;
	}

	public record UserServerObject
	{
		public required int Id { get; set; } = 0;
		public required string UserName { get; set; }
		public List<Role> Roles { set; get; } = [];
		public bool IsPubKey { get; set; } = false;
		public DateTime GeneratedOn { get; } = DateTime.Now;

		public List<Action> AllowedActions { set; get; } = [];
		public JsonObject Data { set; get; } = [];

		public static UserServerObject? FromCache(string cacheKey)
		{
			ExtMemory.SharedMemoryCache.TryGetValue(cacheKey, out var cache);
			return (UserServerObject?)cache;
		}

	}
	public record Role
	{
		public int Id { get; set; } = 0;
		public required string RoleName { get; set; }
		public bool IsPubKey { get; set; } = false;

		public JsonObject Data { set; get; } = [];
	}
}

