using CsvHelper;
using System.Globalization;
using System.Text;

namespace WDApiClient.Model;

internal class Request
{
	public string WID { get; set; }
	public string EffectiveDate { get; set; }

	internal static IEnumerable<Request> GetData()
	{
		IEnumerable<Request> data = default;

		var filePath = GetFilePath("data.csv");

		using (var reader = new StreamReader(filePath))
		using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
		{
			data = csv.GetRecords<Request>().ToList();
		}

		return data;
	}

	internal static void GenerateResponse<T>(List<T> rows)
	{
		var filePath = GetFilePath("Response.csv");

		if (File.Exists(filePath))
		{
			File.Delete(filePath);
		}

		using (StreamWriter sw = new StreamWriter(filePath))
		{
			CsvGenerator.CreateHeader(rows, sw);
			CsvGenerator.CreateRows(rows, sw);
		}
	}

	internal string CreateSoapEnvelope(string userName, string password)
	{
		StringBuilder soapXML = new StringBuilder();

		soapXML.Append(@"<env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/'
                            xmlns:xsd='http://www.w3.org/2001/XMLSchema'
                             xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
                            <env:Header>
								<wsse:Security env:mustUnderstand='1'>
                                    <wsse:UsernameToken>
                                        <wsse:Username>{userName}</wsse:Username>
                                        <wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>{password}</wsse:Password>
                                    </wsse:UsernameToken>
                                 </wsse:Security>
                            </env:Header>
							<env:Body>
								<wd:Organization_Inactivate xmlns:wd='urn:com.workday/bsvc' wd:version='v41.0'>
									<wd:Organization_Reference_Data>
										<wd:Integration_ID_Reference>
											<wd:ID wd:System_ID='WD-WID'>{WID}</wd:ID>
										</wd:Integration_ID_Reference>
									</wd:Organization_Reference_Data>
									<wd:Organization_Inactivate_Data wd:Effective_Date='{EffectiveDate}'></wd:Organization_Inactivate_Data>
								</wd:Organization_Inactivate>
							</env:Body>
						</env:Envelope>");


		soapXML.Replace("{userName}", userName);
		soapXML.Replace("{password}", password);
		soapXML.Replace("{WID}", WID);
		soapXML.Replace("{EffectiveDate}", EffectiveDate);

		return soapXML.ToString();
	}

	private static string GetFilePath(string filename)
	{
		var currentDirectory = Directory.GetCurrentDirectory();
		var dataDirectory = $"{currentDirectory}/Data";
		var fullFilename = Path.Combine(dataDirectory, filename);

		return fullFilename;
	}
}
