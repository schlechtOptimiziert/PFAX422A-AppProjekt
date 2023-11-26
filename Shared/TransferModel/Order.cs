using System;
using System.Collections.Generic;

namespace TransferModel;

public class Order
{
    public long Id { get; set; }
    public string UserId { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public int? Postcode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public IEnumerable<Item> Items { get; set; }
}
