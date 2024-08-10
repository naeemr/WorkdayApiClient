using System.Xml;
using WDApiClient.Model;

namespace WDApiClient.Client
{
	internal abstract class WebClient
	{
		protected string url = "https://wd5-impl-services1.workday.com/{RequestUri}";
		protected string userName = "xxx";
		protected string password = "xxx";

		internal virtual Task<Response> ProcessRequest(Request vehicle, CancellationToken cancellation)
		{
			throw new NotImplementedException();
		}

		protected void PopulateError(string result, Response response)
		{
			try
			{
				XmlDocument xDoc = new XmlDocument();
				xDoc.LoadXml(result);
				XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(xDoc.NameTable);

				xmlnsManager.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/");
				xmlnsManager.AddNamespace("wd", "urn:com.workday/bsvc");

				var message = xDoc.SelectSingleNode("//wd:Validation_Error/wd:Message", xmlnsManager).InnerText;
				var detailMessage = xDoc.SelectSingleNode("//wd:Validation_Error/wd:Detail_Message", xmlnsManager).InnerText;

				response.SetMessage(message, detailMessage);
			}
			catch (Exception)
			{

			}
		}
	}
}
