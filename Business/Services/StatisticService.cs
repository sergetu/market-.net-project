using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            var customerReceipts = receipts.Where(r => r.CustomerId == customerId);

            var ReceiptDetailsList = customerReceipts
                .Where(receipt => receipt.ReceiptDetails != null)
                .SelectMany(receipt => receipt.ReceiptDetails);

            var popularProducts = ReceiptDetailsList
                .Where(receiptDetail => receiptDetail.Product != null)
                .GroupBy(receiptDetail => receiptDetail.Product)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(receiptDetail => receiptDetail.Quantity)
                );
            var popularProductsList = popularProducts.OrderByDescending(p => p.Value).Select(p => _mapper.Map<ProductModel>(p.Key)).Take(productCount);

            return popularProductsList;
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            var filteredReceipts = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);

            var income = filteredReceipts
                .SelectMany(r => r.ReceiptDetails)
                .Where(rd => rd.Product.Category.Id == categoryId)
                .Sum(rd => rd.Quantity * rd.DiscountUnitPrice);

            return income;
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receiptDetails = await _unitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();
            // return most popular products based on quantity of sold products
            var mostPopularProducts = receiptDetails
                .GroupBy(rd => rd.Product)
                .Select(group => new
                {
                    Product = group.Key,
                    Quantity = group.Sum(rd => rd.Quantity)
                })
                .OrderByDescending(p => p.Quantity)
                .Take(productCount)
                .Select(p => p.Product);

            return _mapper.Map<IEnumerable<ProductModel>>(mostPopularProducts);
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            var filteredReceipts = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);

            var customerSpending = filteredReceipts
                .GroupBy(r => r.CustomerId)
                .Select(group => new
                {
                    CustomerId = group.Key,
                    TotalSpent = group.Sum(r => r.ReceiptDetails
                        .Sum(rd => rd.Quantity * rd.DiscountUnitPrice)
                    )
                })
                .OrderByDescending(cs => cs.TotalSpent)
                .Take(customerCount);

            var valuableCustomers = receipts.Where(r => customerSpending.Select(cs => cs.CustomerId).Contains(r.Customer.Id)).Select(r => r.Customer).Distinct();

            var customerActivityModels = valuableCustomers.Select(c => new CustomerActivityModel
            {
                CustomerId = c.Id,
                CustomerName = c.Person.Name + " " + c.Person.Surname,
                ReceiptSum = customerSpending.First(cs => cs.CustomerId == c.Id).TotalSpent
            });

            return customerActivityModels;
        }
    }
}
