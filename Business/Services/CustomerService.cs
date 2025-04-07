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
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public Task AddAsync(CustomerModel model)
        {
            if (model == null)
            {
                throw new MarketException();
            }
            if (string.IsNullOrEmpty( model.Name))
            {                 
                throw new MarketException();
            }
            if (string.IsNullOrEmpty(model.Surname))
            {
                throw new MarketException();
            }
            //birthday need to be more than 1000-1-1 and less than today
            if (model.BirthDate < new DateTime(1000, 1, 1, 1, 1, 1, DateTimeKind.Unspecified) || model.BirthDate > DateTime.Now)
            {
                throw new MarketException();
            }

            _unitOfWork.CustomerRepository.AddAsync(_mapper.Map<Data.Entities.Customer>(model));
            return _unitOfWork.SaveAsync();
        }

        public Task DeleteAsync(int modelId)
        {
            _unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
            return _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var entities = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<CustomerModel>>(entities);
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var entitie = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<CustomerModel>(entitie);
        }

        //Customer Service Get Customers By Product Id Async Returns Customers Who Bought Product
        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            customers = customers.Where(c => c.Receipts.Any(r => r.ReceiptDetails.Any(rd => rd.ProductId == productId)));
            return _mapper.Map<IEnumerable<CustomerModel>>(customers);
        }

        public  Task UpdateAsync(CustomerModel model)
        {
            if (model == null)
            {
                throw new MarketException();
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new MarketException();
            }
            if (string.IsNullOrEmpty(model.Surname))
            {
                throw new MarketException();
            }
            //birthday need to be more than 1000-1-1 and less than today
            if (model.BirthDate < new DateTime(1330, 1, 1, 1, 1, 1 , DateTimeKind.Unspecified) || model.BirthDate > DateTime.Now)
            {
                throw new MarketException();
            }
            var entity = _mapper.Map<Customer>(model); 
            _unitOfWork.PersonRepository.Update(entity.Person);
            _unitOfWork.CustomerRepository.Update(entity);
            return _unitOfWork.SaveAsync();
        }
    }
}
