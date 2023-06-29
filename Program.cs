using System.Text;
using AutoConfirmEventsInTimepad.Models;
using TimepadClient;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<string> emailDomens = new List<string>() {"@timepath.ru", "@pminst.ru"};


app.Run(async (context) =>
{
    Ticket ticket = new Ticket("5184211:83845994", 215813, 29963, "4955686", "2015-07-24 19:04:37", 361138, "забронировано", "booked", "test-mail@ya.ru", "Смирнов", "Владимир", false, "83845994", "83845994", 1000, null);
    await context.Response.WriteAsJsonAsync(ticket);
    StringBuilder email = new StringBuilder(ticket.email);
    email.Remove(0, ticket.email!.IndexOf('@'));
    await context.Response.WriteAsync(email.ToString());
    if (emailDomens.Contains(email.ToString()))
    {
        TimepadClient.TimepadClient timepadClient = new TimepadClient.TimepadClient(new HttpClient());
        await timepadClient.ApproveEventOrderAsync(ticket.event_id, int.Parse(ticket.order_id!));
    }

});

app.Run();

