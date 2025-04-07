using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Data.Interfaces;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Product : BaseEntity
    {
        [ForeignKey("ProductCategory")]
        public int ProductCategoryId { get; set; }

  
        public string ProductName { get; set; }

 
        public decimal Price { get; set; }

        public ProductCategory Category { get; set; }
        public ICollection<ReceiptDetail> ReceiptDetails { get; set; }

    }
}