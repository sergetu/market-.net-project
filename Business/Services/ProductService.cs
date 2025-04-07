using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(ProductModel model)
        {
            //Product Service AddAsync Throws MarketException If Price Is Negative
            if (model.Price < 0)
            {
                throw new MarketException();
            }
            //Product Service AddAsync Throws MarketException If ProductName Is Null Or Empty
            if (String.IsNullOrEmpty(model.ProductName))
            {
                throw new MarketException();
            }
            var entity = _mapper.Map<Product>(model);
            await _unitOfWork.ProductRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (String.IsNullOrEmpty(categoryModel.CategoryName))
            {
                throw new MarketException();
            }
            var entity = _mapper.Map<ProductCategory>(categoryModel);
            await _unitOfWork.ProductCategoryRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var entities = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ProductModel>>(entities);
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var entities = await _unitOfWork.ProductCategoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductCategoryModel>>(entities);
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();

            if (filterSearch.CategoryId.HasValue)
            {
                products = products.Where(p => p.ProductCategoryId == filterSearch.CategoryId.Value);
            }

            if (filterSearch.MinPrice.HasValue)
            {
                products = products.Where(p => p.Price >= filterSearch.MinPrice.Value);
            }

            if (filterSearch.MaxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= filterSearch.MaxPrice.Value);
            }

            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ProductModel>(entity);
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await _unitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            if (String.IsNullOrEmpty(model.ProductName))
            {
                throw new MarketException();
            }
            var entity = _mapper.Map<Product>(model);
            _unitOfWork.ProductRepository.Update(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (String.IsNullOrEmpty(categoryModel.CategoryName))
            {
                throw new MarketException();
            }
            var entity = _mapper.Map<ProductCategory>(categoryModel);
            _unitOfWork.ProductCategoryRepository.Update(entity);
            await _unitOfWork.SaveAsync();
        }
    }
}
