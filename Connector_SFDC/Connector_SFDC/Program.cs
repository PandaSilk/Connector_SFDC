using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.Script.Serialization;
//using Connector_SFDC.SFDC_Personal;

namespace Connector_SFDC {

#region "About Me!"
// This class/region section must remain in the library under the License Terms. For more info see the ReadME.txt
public static class About { 
    public static string Version = @"1.0.0";
    public static string Author = @"Christian Rosen";
    public static string Email = @"Panda.Silk@gmail.com";
    public static string Website = @"https://github.com/PandaSilk";

    public static string PrintMe() { 
    return "Version: \t"+Version + "\nAuthor: \t"+Author+"\nE-mail: \t"+Email+"\nWebSite: \t"+Website;
    }
}
#endregion

public class SFDC_APP { 

	public class WSDLType {
	public const string PARTNER = "u";
	public const string ENTERPRISE = "c";
}

public class APIVersion { 
	public const string V32 = "32.0";
	public const string V33 = "33.0";
	public const string V34 = "34.0";
}

}

class Program {

	public Utility_Errors Err;

	public List<string> GetObjects(SFDC_Personal.SforceService SFService) {

	List<string> Results = new List<string>();

	try {            
	DescribeGlobalResult dgr = SFService.describeGlobal();
	DescribeGlobalSObjectResult[] sObjResults = dgr.sobjects;
	for (int i = 0; i < sObjResults.Length; i++) { Results.Add(sObjResults[i].name); }
	Results.Sort();
	return Results;

    } catch (Exception e) { Err.Add("ERROR", System.Reflection.MethodBase.GetCurrentMethod().ToString(), e.InnerException.Message.ToString()); return Results; }

}
	public QueryResult GetUsers(SFDC_Personal.SforceService SFService) {

	QueryResult Results = null;

	String soqlQuery = "SELECT FirstName, LastName, Username FROM User";
    Results = SFService.query(soqlQuery);

	return Results;
	}


static void Main(string[] args){

	// You must add a web reference to your SFDC deployment with the URL below
	// https://na10.salesforce.com/soap/wsdl.jsp?type=*
	// Right click References, Add Service Reference, click Advance, click Add Web Reference, enter URL, login to SFDC
	// give it the name 'SFDC_Personal' without '', then uncomment the above reference line 10.

	// dynamic method of connecting to any salesforce deployment type via direct login,
	// there are a few requirements and limitations.

	//begin!
	SFDC_Personal.SforceService SFService = new SFDC_Personal.SforceService();
	JavaScriptSerializer JSON = new JavaScriptSerializer();

	//configuration
	string[] Username = { @"username1", @"username2" };					//array of usernames
	string[] Password = { @"password1", @"password2" };					//array of passwords
	string[] Token = { @"securitytoken1", @"securitytoken2" };			//array of tokens
	string[] LoginSubDomain = { @"identURL1", @"identURL2"};			//array of tenants

	//default a tenant
	int Tenant = 0;

	//SFDC API Config
	string APIVersion = SFDC_APP.APIVersion.V32;
	string APIType = SFDC_APP.WSDLType.ENTERPRISE;
	
	//choose a deployment
	Tenant = 0;

	//connect and login dynamically!
	string LoginURL = @"https://" + LoginSubDomain[Tenant] + ".salesforce.com/services/Soap/" + APIType + "/" + APIVersion;
	SFService.Url = LoginURL;
	SFDC_Personal.LoginResult LR = new SFDC_Personal.LoginResult();
	LR.serverUrl = LoginURL;
	LR = SFService.login(Username[Tenant], Password[Tenant] + Token[Tenant]);
	SFService.Url = LR.serverUrl;														//Remember this, used for bulk API
	SFService.SessionHeaderValue = new SFDC_Personal.SessionHeader();
	SFService.SessionHeaderValue.sessionId = LR.sessionId;								//Remember this, used for bulk API

	string Result = "";

	#region Examples
	// *** Create Opportunity
	//DateTime dt = (DateTime)SFService.getServerTimestamp().timestamp;	
	//Opportunity newOpportunity = new Opportunity();
	//newOpportunity.Name = "TEST - OPP - TESTOPPDATA - " + String.Format("{0:d/M/yyyy HH:mm:ss}", dt);
	//newOpportunity.StageName = "Contacted";
	//newOpportunity.CloseDate = dt.AddDays(7);
	//newOpportunity.CloseDateSpecified = true;
	//create it
	//SFDC_Personal.SaveResult[] results = SFService.create(new SFDC_Personal.sObject[] { newOpportunity });

	// *** Get all objects
	//Result = JSON.Serialize(GetObjects(SFService));

	// *** Get all users
	//Result = JSON.Serialize(GetUsers(SFService));
	#endregion

	//good housekeeping!
	SFService.logout();
	SFService.Dispose();
	SFService = null;
	
	return Result;

}

}
}
