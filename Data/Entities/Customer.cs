using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Data.Entities
{
    public class Customer : BaseEntity
    {
        [ForeignKey("Person")]
        public int PersonId { get; set; }

        public int DiscountValue { get; set; }

        public Person Person { get; set; }
        public ICollection<Receipt> Receipts { get; set; }

    }
}