using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task AddAsync(ReceiptModel model)
        {
            var entity = _mapper.Map<Receipt>(model);
            await _unitOfWork.ReceiptRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            if (receipt == null)
            {
                throw new MarketException();
            }

            Product product = null;
  
            // Validate the existence of the product
            if (_unitOfWork.ProductRepository != null)
            {
                product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    throw new MarketException();
                }
            }


            // Check if the product is already added to the receipt
            ReceiptDetail existingReceiptDetail = null;
            if (receipt.ReceiptDetails != null)
            {
                existingReceiptDetail = receipt.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId);
            }
            if (existingReceiptDetail != null)
            {
                // Update quantity if product was added to the receipt before
                existingReceiptDetail.Quantity += quantity;
                _unitOfWork.ReceiptDetailRepository.Update(existingReceiptDetail);
            }
            else
            {
                // Create new ReceiptDetail if product was not added before
#pragma warning disable S2259 // Null pointers should not be dereferenced
                var receiptDetail = new ReceiptDetail
                {
                    ProductId = productId,
                    ReceiptId = receiptId,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    DiscountUnitPrice = product.Price - (product.Price * (receipt.Customer.DiscountValue / 100.0m))
                };
#pragma warning restore S2259 // Null pointers should not be dereferenced
                await _unitOfWork.ReceiptDetailRepository.AddAsync(receiptDetail);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
            if (receipt != null)
            {
                receipt.IsCheckedOut = true; // Assuming there's an IsCheckedOut property
                _unitOfWork.ReceiptRepository.Update(receipt);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new MarketException();
            }
        }

        public async Task DeleteAsync(int modelId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);
            foreach (var detail in receipt.ReceiptDetails)
            {
                _unitOfWork.ReceiptDetailRepository.Delete(detail);
            }
            await _unitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var entities = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ReceiptModel>>(entities);
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ReceiptModel>(entity);
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var details = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            return _mapper.Map<IEnumerable<ReceiptDetailModel>>(details.ReceiptDetails);
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var filteredReceipts = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);
            return _mapper.Map<IEnumerable<ReceiptModel>>(filteredReceipts);
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

            if (receipt != null)
            {
                receipt.ReceiptDetails.Where(rd => rd.ProductId == productId).ToList().ForEach(rd =>
                {
                    if (rd.Quantity > quantity)
                    {
                        rd.Quantity -= quantity;
                    }
                    else
                    {
                        _unitOfWork.ReceiptDetailRepository.Delete(rd);
                    }
                });
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            var receiptDetails = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            return receiptDetails.ReceiptDetails.Sum(rd => (rd.Quantity * rd.DiscountUnitPrice));
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            var entity = _mapper.Map<Receipt>(model);
            _unitOfWork.ReceiptRepository.Update(entity);
            await _unitOfWork.SaveAsync();
        }
    }
}

