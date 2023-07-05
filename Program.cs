using System.Text;
using AutoConfirmEventsInTimepad.Models;
using TimepadClient;
using System.Text.Json;

WebHookTicket DeserializeWebHookTicketBody<T>(Stream stream)
{
    try
    {
        return JsonSerializer.Deserialize<WebHookTicket>(stream);
    }
    catch (JsonException e)
    {
        throw new Exception(e.Message);
    }
}

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<string> emailDomens = new List<string>
{
    //"@timepath.ru", 
    //"@pminst.ru"
    "@gmail.com"
};


app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var requestBody = request.Body;
    var stringRequestBody = DeserializeWebHookTicketBody<WebHookTicket>(requestBody);
    //var requestBody = request.ReadFromJsonAsync<WebHookTicket>();
    var currentEmail = stringRequestBody.email;
    //var payload = Newtonsoft.Json.JsonConvert.DeserializeObject<WebHookTicket>(request);
    //WebHookTicket ticket = new WebHookTicket("5184211:83845994", 215813, 29963, "4955686", "2015-07-24 19:04:37", 361138, "забронировано", "booked", "test-mail@ya.ru", "Смирнов", "Владимир", false, "83845994", "83845994", 1000, null);
    //await response.WriteAsJsonAsync(ticket);
    StringBuilder email = new StringBuilder(currentEmail);
    email.Remove(0, currentEmail!.IndexOf('@'));
    await response.WriteAsync(email.ToString());
    
    if (emailDomens.Contains(email.ToString()))
    {
        TimepadClient.TimepadClient timepadClient = new TimepadClient.TimepadClient(new HttpClient());
        await timepadClient.ApproveEventOrderAsync(stringRequestBody.event_id, int.Parse(stringRequestBody.order_id!));
    }

});

app.Run();

