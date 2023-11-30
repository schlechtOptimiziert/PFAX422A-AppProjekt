using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TransferModel;

public class ItemPicture
{
    public long Id { get; set; }
    public long ItemId { get; set; }
    public string FileName { get; set; }
}
