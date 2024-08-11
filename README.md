# WorkdayApiClient

This API client is developed to update partial information in Workday through the Workday SOAP web service.

## Installation

Developed this in .Net 6 with Visual Studio 2022.

Update the Workday URL, username, and password in the WebClient.cs file."

Use the test CSV data from the data.csv file, which contains the Workday Unique ID and a date that needs to be updated in Workday.

Map the CSV data to the Request model class. You can create your own model class based on the information that needs to be updated, and also update the Workday SOAP request in the Request model class.
