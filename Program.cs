using System.Text;

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
        TimepadClient
    }

});

app.Run();

public class Ticket
{
    public string? id { get; set; }
    public int event_id { get; set; }
    public int organization_id { get; set; }
    public string? order_id { get; set; }
    public string? reg_date { get; set; }
    public int reg_id { get; set; }
    public string? status { get; set; }
    public string? status_raw { get; set; }
    public string? email { get; set; }
    public string? surname { get; set; }
    public string? name { get; set; }
    public bool attended { get; set; }
    public string? code { get; set; }
    public string? barcode { get; set; }
    public int price_nominal { get; set; }
    public List<Answer>? answers { get; set; }

    public class Answer
    {
        public int id { get; set; }
        public string? type { get; set; }
        public string? name { get; set; }
        public bool mandatory { get; set; }
        public string? value { get; set; }
    }

    public Ticket(string Id, int EventId, int OrganizationId, string? OrderId, string? RegDate, int RegId, string? Status, string? StatusRaw,
                  string? Email, string? Surname, string? Name, bool Attended, string? Code, string? Barcode, int PriceNominal, List<Answer>? Answers)
    {
        id = Id;
        event_id = EventId;
        organization_id = OrganizationId;
        order_id = OrderId;
        reg_date = RegDate;
        reg_id = RegId;
        status = Status;
        status_raw = StatusRaw;
        email = Email;
        surname = Surname;
        name = Name;
        attended = Attended;
        code = code;
        barcode = Barcode;
        price_nominal = PriceNominal;
        answers = Answers;
    }
}