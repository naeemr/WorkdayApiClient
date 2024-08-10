using System.Reflection;

namespace WDApiClient;

internal class CsvGenerator
{
	internal static void CreateHeader<T>(List<T> list, StreamWriter sw)
	{
		PropertyInfo[] properties = typeof(T).GetProperties();
		for (int i = 0; i < properties.Length - 1; i++)
		{
			sw.Write(properties[i].Name + ",");
		}
		var lastProp = properties[properties.Length - 1].Name;
		sw.Write(lastProp + sw.NewLine);
	}

	internal static void CreateRows<T>(List<T> list, StreamWriter sw)
	{
		foreach (var item in list)
		{
			PropertyInfo[] properties = typeof(T).GetProperties();
			for (int i = 0; i < properties.Length - 1; i++)
			{
				var prop = properties[i];
				sw.Write(prop.GetValue(item) + ",");
			}
			var lastProp = properties[properties.Length - 1];
			sw.Write(lastProp.GetValue(item) + sw.NewLine);
		}
	}
}
