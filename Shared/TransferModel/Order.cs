using System;
using System.Collections.Generic;

namespace TransferModel;

public class Order
{
    public long Id { get; set; }
    public string UserId { get; set; }
    public DateTime Date { get; set; }

    public IEnumerable<Item> Items { get; set; }
}
