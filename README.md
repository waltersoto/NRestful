# NRestful
C# library to consume a Restful API from .NET

Sample:

Instantiate a client and pass the URL of the Resfult Api as a parameter and send a "GET" request.
```cs
   /***************
    * WebApi URL:
    ***************/
    const string Url = "http://localhost:3034/api/";
    var client = new Client(Url);
    
    //Send a GET request:
    var response = client.RequestAsync<string>(new Request {
                EndPoint = new EndPoint {
                    Method = Method.GET,
                    Uri = "sample/{id}"
                },
                UrlSegment = { { "id", "5" } }
            });

    var result = await response;

    Console.WriteLine(result.Content);
```

Send a "POST" request to a WebApi "POST" method

**WebApi Example:** public void Post([FromBody]string value)

```cs
         var response = client.RequestAsync<string>(new Request {
                EndPoint = new EndPoint {
                    Method = Method.POST,
                    Uri = "sample/"
                },
                Data = "'This is the value posted'"
            });
```

Send a "POST" request to a WebApi and send a JSON object

**WebApi Example:** public void Post([FromBody]User account)
```cs
   var response = client.RequestAsync<string>(new Request {
                EndPoint = new EndPoint {
                    Method = Method.POST,
                    Uri = "sample/"
                },
                Data = JsonHelper.ToJson(new {
                    firstName = "Walter",
                    lastName = "Soto",
                    Email = "email@email.net"
                })
            });
```




