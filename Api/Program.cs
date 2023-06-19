var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("{*path}", (HttpRequest httpRequest, HttpResponse httpResponse, string path) =>
{
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
  return (File.Exists(file)) ? File.ReadAllText(file) : "Not Found";
});

app.Run();
