using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("", () => "Api is running");

app.Map("{*path}", (HttpRequest httpRequest, HttpResponse httpResponse, string path) =>
{
  httpResponse.Headers.Add("Access-Control-Allow-Origin", "*");
  httpResponse.Headers.Add("Access-Control-Request-Method", "*");
  httpResponse.Headers.Add("Access-Control-Allow-Methods", "OPTIONS, GET, POST, PUT, DELETE");
  httpResponse.Headers.Add("Access-Control-Allow-Headers", "*");

  var method = httpRequest.Method.ToLower() == "get" ? "" : $".{httpRequest.Method.ToLower()}";
  var fileType = "";
  if (httpRequest.ContentType?.ToLower() == "text/csv")
  {
    fileType = "csv";
    httpResponse.Headers.Add("Content-Type", "text/csv");
  }
  else
  {
    fileType = "json";
    httpResponse.Headers.Add("Content-Type", "application/json");
  }

  var file = $"Files/{path}{method}.{fileType}";
  var content = (File.Exists(file)) ? File.ReadAllText(file) : "Not Found";
  // return login token as base64 encoded
  if (path.Equals("identity/login", StringComparison.InvariantCultureIgnoreCase))
  {
    var jsonObject = JsonSerializer.Deserialize<LoginToken>(content);
    var header = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(jsonObject?.header)));
    var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(jsonObject?.payload)));
    content = JsonSerializer.Serialize(new { IdentityToken = $"{header}.{payload}.{jsonObject?.signature}" });
  }

  // replace the word "RANDOM" with random integer
  var regex = new Regex("RANDOM");
  while (content.Contains("RANDOM"))
  {
    content = regex.Replace(content, new Random().Next(100).ToString(), 1);
  }
  return content;
});

app.Run();


