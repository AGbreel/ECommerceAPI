using Dapper;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        // ✅ Get All Products
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var query = "sp_GetAllProducts";

            using (var connection = _context.CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(query, commandType: CommandType.StoredProcedure))
            {
                var products = (await multi.ReadAsync<Product>()).ToList();
                var colors = await multi.ReadAsync<(int ProductId, string ColorName)>();
                var sizes = await multi.ReadAsync<(int ProductId, string SizeName)>();
                var images = await multi.ReadAsync<(int ProductId, string ImageUrl)>();

                foreach (var p in products)
                {
                    p.Colors = colors.Where(c => c.ProductId == p.ProductId)
                                     .Select(c => c.ColorName).ToList();
                    p.Sizes = sizes.Where(s => s.ProductId == p.ProductId)
                                   .Select(s => s.SizeName).ToList();
                    p.Images = images.Where(i => i.ProductId == p.ProductId)
                                     .Select(i => i.ImageUrl).ToList();
                }

                return products;
            }
        }

        // ✅ Get Product By Id
        public async Task<Product?> GetProductById(int productId)
        {
            var query = "sp_GetProductById";

            using (var connection = _context.CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(query, new { ProductId = productId }, commandType: CommandType.StoredProcedure))
            {
                var product = await multi.ReadSingleOrDefaultAsync<Product>();
                if (product == null) return null;

                var colors = await multi.ReadAsync<(int ProductId, string ColorName)>();
                var sizes = await multi.ReadAsync<(int ProductId, string SizeName)>();
                var images = await multi.ReadAsync<(int ProductId, int ImageId, string ImageUrl)>();

                product.Colors = colors.Select(c => c.ColorName).ToList();
                product.Sizes = sizes.Select(s => s.SizeName).ToList();
                product.Images = images.Select(i => i.ImageUrl).ToList();

                return product;
            }
        }

        // ✅ Create Product (مع الألوان والمقاسات + الصور)
        public async Task CreateProduct(Product product)
        {
            var query = "sp_AddProduct";
            var parameters = new DynamicParameters();
            parameters.Add("ProductName", product.ProductName, DbType.String);
            parameters.Add("Description", product.Description, DbType.String);
            parameters.Add("Price", product.Price, DbType.Decimal);
            parameters.Add("Stock", product.Stock, DbType.Int32);
            parameters.Add("CategoryId", product.CategoryId, DbType.Int32);
            parameters.Add("ImageUrl", product.ImageUrl, DbType.String);
            parameters.Add("RatingsAverage", product.RatingsAverage, DbType.Decimal);
            parameters.Add("PriceAfterDiscount", product.PriceAfterDiscount, DbType.Decimal);


            // ✅ Colors, Sizes, Images كـ CSV
            parameters.Add("Colors", product.Colors?.Any() == true ? string.Join(",", product.Colors) : null, DbType.String);
            parameters.Add("Sizes", product.Sizes?.Any() == true ? string.Join(",", product.Sizes) : null, DbType.String);
            parameters.Add("Images", product.Images?.Any() == true ? string.Join(",", product.Images) : null, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        // ✅ Update Product (هي نفس فكرة Create - باستخدام SP تعمل Update + CSV للـ Colors/Sizes/Images)
        public async Task<bool> UpdateProduct(Product product)
        {
            var query = "sp_UpdateProduct";
            var parameters = new DynamicParameters();
            parameters.Add("ProductId", product.ProductId, DbType.Int32);
            parameters.Add("ProductName", product.ProductName, DbType.String);
            parameters.Add("Description", product.Description, DbType.String);
            parameters.Add("Price", product.Price, DbType.Decimal);
            parameters.Add("Stock", product.Stock, DbType.Int32);
            parameters.Add("CategoryId", product.CategoryId, DbType.Int32);
            parameters.Add("ImageUrl", product.ImageUrl, DbType.String);
            parameters.Add("RatingsAverage", product.RatingsAverage, DbType.Decimal);
            parameters.Add("PriceAfterDiscount", product.PriceAfterDiscount, DbType.Decimal);

            parameters.Add("Colors", product.Colors?.Any() == true ? string.Join(",", product.Colors) : null, DbType.String);
            parameters.Add("Sizes", product.Sizes?.Any() == true ? string.Join(",", product.Sizes) : null, DbType.String);
            parameters.Add("Images", product.Images?.Any() == true ? string.Join(",", product.Images) : null, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var rows = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return rows > 0;
            }
        }

        // ✅ Delete Product
        public async Task<bool> DeleteProduct(int productId)
        {
            var query = "sp_DeleteProduct";
            var parameters = new DynamicParameters();
            parameters.Add("ProductId", productId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var rows = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return rows > 0;
            }
        }

        // ✅ Get Products By Category
        public async Task<IEnumerable<Product>> GetProductsByCategory(int categoryId)
        {
            var query = "sp_GetProductsByCategory";

            using (var connection = _context.CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(query, new { CategoryId = categoryId }, commandType: CommandType.StoredProcedure))
            {
                var products = (await multi.ReadAsync<Product>()).ToList();
                var colors = await multi.ReadAsync<(int ProductId, string ColorName)>();
                var sizes = await multi.ReadAsync<(int ProductId, string SizeName)>();
                var images = await multi.ReadAsync<(int ProductId, string ImageUrl)>();

                foreach (var p in products)
                {
                    p.Colors = colors.Where(c => c.ProductId == p.ProductId)
                                     .Select(c => c.ColorName).ToList();
                    p.Sizes = sizes.Where(s => s.ProductId == p.ProductId)
                                   .Select(s => s.SizeName).ToList();
                    p.Images = images.Where(i => i.ProductId == p.ProductId)
                                     .Select(i => i.ImageUrl).ToList();
                }

                return products;
            }
        }

        public async Task AddProductImage(int productId, string imageUrl)
        {
            var query = "sp_AddProductImage";
            var parameters = new DynamicParameters();
            parameters.Add("ProductId", productId, DbType.Int32);
            parameters.Add("Image", imageUrl, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
