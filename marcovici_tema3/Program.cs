using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace marcovici_tema3
{
    class Program
    {
         static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "Drive API .NET DATC Cristiana";
        static DriveService service;

        static void Main(string[] args)
        {
            Initialize();
        }
        
 static void Initialize(){
      UserCredential credential;

            using (var stream =
                new FileStream("client_id.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    Environment.UserName,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
 }
static void GetAllFiles(){
    //service.Files.List
    var request= (HttpWebRequest).WebRequest.Create("https://www.googleapis.com/drive/v3/files?q='root'%20in%20parents");
    using (var response=request.GetResponse())
    {
        using (Stream data=response.GetResponseStream())
        using (var reader=new StreamReader(data))
        {
            string text=reader.ReadToEnd();
            var myData=JObject.Parse(text);
            foreach(var file in myData["files"])
            {
                if(file["mimeType"].ToString()!="application/vnd.google-apps.folder")
                {
                    Console.WriteLine("File name: "+ file["name"]);
                }

            }
        }

    }
}
        
    }
}
