using System.Text;
using WDApiClient.Model;

namespace WDApiClient.Client
{
	internal class RESTHttpClient : WebClient
	{
		private static readonly HttpClient httpClient = new HttpClient();

		internal override async Task<Response> ProcessRequest(Request request, CancellationToken cancellation)
		{
			Response response = default;

			try
			{
				var text = request.CreateSoapEnvelope(userName, password);

				using (HttpContent content = new StringContent(text, Encoding.UTF8, "text/xml"))
				using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url))
				{
					requestMessage.Headers.Add("SOAPAction", "");
					requestMessage.Content = content;
					using (HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead))
					{
						//response.EnsureSuccessStatusCode(); // throws an Exception if 404, 500, etc.
						var result = await responseMessage.Content.ReadAsStringAsync();
						response = new(request.WID, Convert.ToInt32(responseMessage.StatusCode).ToString());

						if (!responseMessage.IsSuccessStatusCode)
						{
							PopulateError(result, response);
						}
					}
				}
			}
			catch (Exception ex)
			{
				response = new(request.WID, "500");
				response.SetMessage(ex.Message, "");
			}

			return response;
		}
	}
}
