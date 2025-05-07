using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.IntegrationSolution.FilePoller.Models;

public class PurchaseOrder
{
    public Guid Id { get; set; }
    public string Customer { get; set; }
    public DateTime ProcessingDate { get; set; }
    public List<PurchaseOrderItem> Items { get; set; }
}

public class PurchaseOrderItem
{
    public string ProductCode { get; set; }
    public int Quantity { get; set; }
}
