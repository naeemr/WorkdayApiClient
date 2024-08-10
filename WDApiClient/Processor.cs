using WDApiClient.Client;
using WDApiClient.Model;

namespace WDApiClient;

internal class Processor
{
	internal static async Task ProcessData()
	{
		var data = Request.GetData();

		if (data == null || data.Count() == 0)
		{
			return;
		}

		List<Response> responses = new List<Response>();
		WebClient webClient = new RESTHttpClient();

		try
		{
			await Parallel.ForEachAsync(data, async (payload, ct) =>
			{
				var response = await webClient.ProcessRequest(payload, ct);
				responses.Add(response);
			});
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			Request.GenerateResponse(responses);
			Console.ReadKey();
		}
	}
}
