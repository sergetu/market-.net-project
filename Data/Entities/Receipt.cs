using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Data.Interfaces;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Receipt : BaseEntity
    {

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }


        public DateTime OperationDate { get; set; }


        public bool IsCheckedOut { get; set; }

        public Customer Customer { get; set; }
        public ICollection<ReceiptDetail> ReceiptDetails { get; set; }

    }
}