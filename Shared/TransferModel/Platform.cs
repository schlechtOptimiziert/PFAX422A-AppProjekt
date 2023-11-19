using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferModel;

public class Platform
{
    public long Id { get; set; }
    public string Name { get; set; }
}
