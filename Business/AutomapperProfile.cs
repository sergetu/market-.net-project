using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();


            // mapping for Product and ProductModel
            CreateMap<Product, ProductModel>()
                      .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                      .ForMember(dest => dest.ReceiptDetailIds, opt => opt.MapFrom(src => src.ReceiptDetails.Select(rd => rd.Id)))
                      .ReverseMap()
                      .ForMember(dest => dest.Category, opt => opt.Ignore())
                      .ForMember(dest => dest.ReceiptDetails, opt => opt.Ignore());


            // mapping for ReceiptDetail and ReceiptDetailModel

            CreateMap<ReceiptDetail, ReceiptDetailModel>()
                       .ForMember(dest => dest.ReceiptId, opt => opt.MapFrom(src => src.ReceiptId))
                       .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                       .ForMember(dest => dest.DiscountUnitPrice, opt => opt.MapFrom(src => src.DiscountUnitPrice))
                       .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                       .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                       .ReverseMap();

            // mapping for ProductCategory and ProductCategoryModel     
            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(pcm => pcm.ProductIds, r => r.MapFrom(pc => pc.Products.Select(p => p.Id)))
                .ReverseMap();

            // mapping that combines Customer and Person into CustomerModel
            CreateMap<Customer, CustomerModel>()
                .ForMember(cm => cm.Id, c => c.MapFrom(x => x.Person.Id))
                .ForMember(cm => cm.Name, c => c.MapFrom(x => x.Person.Name))
                .ForMember(cm => cm.Surname, c => c.MapFrom(x => x.Person.Surname))
                .ForMember(cm => cm.BirthDate, c => c.MapFrom(x => x.Person.BirthDate))
                .ForMember(cm => cm.DiscountValue, c => c.MapFrom(x => x.DiscountValue))
                .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Receipts.Select(r => r.Id)))
                .ReverseMap();
        }
    }
}