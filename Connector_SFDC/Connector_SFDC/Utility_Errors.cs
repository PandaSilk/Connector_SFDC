using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Xml.Linq;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Connector_SFDC {
public class Utility_Errors {

public List<string> TotalErrors = new List<string>();

public void _Initialize() { 
    TotalErrors.Clear();
}

public string _Summarize() { 
    string tmpResult = "";
    tmpResult = string.Join(",", (string[])TotalErrors.ToArray());
    return tmpResult;
}

public void Add(string Type, string Function, string Message) {
	TotalErrors.Add("{type:'" + Type.Replace("'", "") + "',func:'" + Function.Replace("'", "") + "',msg:'" + Message.Replace("'", "") + "'}");
}

}
}