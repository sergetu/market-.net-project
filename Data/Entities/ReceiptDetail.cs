using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Data.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Data.Entities
{
    public class ReceiptDetail : BaseEntity
    {
        [ForeignKey("Receipt")]
        public int ReceiptId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public decimal DiscountUnitPrice { get; set; }

    
        public decimal UnitPrice { get; set; }


        public int Quantity { get; set; }

        public Receipt Receipt { get; set; }
        public Product Product { get; set; }

    }
}