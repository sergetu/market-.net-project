using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TradeMarketDbContext context;
        private ICustomerRepository customerRepository;
        private IReceiptRepository receiptRepository;
        private IReceiptDetailRepository receiptDetailRepository;
        private IProductRepository productRepository;
        private IPersonRepository personRepository;
        private IProductCategoryRepository productCategoryRepository;


        public UnitOfWork(TradeMarketDbContext context)
        {
            this.context = context;
        }

        public ICustomerRepository CustomerRepository
        {
            get
            {
                if (customerRepository == null)
                {
                    customerRepository = new CustomerRepository(context);
                }
                return customerRepository;
            }
        }

        public IReceiptRepository ReceiptRepository
        {
            get
            {
                if (receiptRepository == null)
                {
                    receiptRepository = new ReceiptRepository(context);
                }
                return receiptRepository;
            }
        }

        public IReceiptDetailRepository ReceiptDetailRepository
        {
            get
            {
                if (receiptDetailRepository == null)
                {
                    receiptDetailRepository = new ReceiptDetailRepository(context);
                }
                return receiptDetailRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new ProductRepository(context);
                }
                return productRepository;
            }
        }

        public IPersonRepository PersonRepository
        {
            get
            {
                if (personRepository == null)
                {
                    personRepository = new PersonRepository(context);
                }
                return personRepository;
            }
        }

        public IProductCategoryRepository ProductCategoryRepository
        {
            get
            {
                if (productCategoryRepository == null)
                {
                    productCategoryRepository = new ProductCategoryRepository(context);
                }
                return productCategoryRepository;
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
