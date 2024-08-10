namespace WDApiClient.Model;

internal class Response
{
	public string WID { get; private set; }
	public string StatusCode { get; private set; }
	public string Message { get; private set; }
	public string Detail_Message { get; private set; }

	internal Response(string wid, string code)
	{
		WID = wid;
		StatusCode = code;
	}

	internal void SetMessage(string message, string detailMessage)
	{
		Message = message;
		Detail_Message = detailMessage;
	}
}
