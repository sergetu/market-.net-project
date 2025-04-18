﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Data.Entities
{
    public class ProductCategory : BaseEntity
    {
        public string CategoryName { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}