using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PaymentApp;

/// <summary>
/// Basic Newtonsoft.Json helpers (serialize, deserialize, parse).
/// </summary>
public static class JsonHelper
{
	private static readonly JsonSerializerSettings DefaultSettings = new()
	{
		Formatting = Formatting.Indented,
		NullValueHandling = NullValueHandling.Ignore
	};

	/// <summary>Serialize object to JSON string.</summary>
	public static string ToJson<T>(T value, bool indented = true)
	{
		return JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None, DefaultSettings);
	}

	/// <summary>Deserialize JSON string to object.</summary>
	public static T? FromJson<T>(string json)
	{
		return JsonConvert.DeserializeObject<T>(json, DefaultSettings);
	}

	/// <summary>Parse JSON to a dynamic JObject (read/query without a type).</summary>
	public static JObject ParseObject(string json)
	{
		return JObject.Parse(json);
	}
}
