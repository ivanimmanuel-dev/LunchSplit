using LunchSplit;

var bill = new Bill(100m, 15m, TipMode.Percent, 20m);
var attendees = new List<Attendee>
{
    new Attendee("Johan", 1, true),
    new Attendee("Kevin", 1, true)
};

var validation = new BillValidator().Validate(bill, attendees);
if (!validation.IsValid)
{
    Console.WriteLine(validation.ErrorMessage);
    return;
}

var shares = new Splitter().CalculateShares(bill, attendees, RoundingMode.BankersRounding);

var receipt = new ReceiptFormatter().Format(bill, attendees, shares);
Console.WriteLine(receipt);