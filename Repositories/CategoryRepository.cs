using Dapper;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperContext _context;

        public CategoryRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var query = "sp_GetAllCategories";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Category>(query, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Category?> GetCategoryById(int categoryId)
        {
            var query = "sp_GetCategoryById";
            var parameters = new DynamicParameters();
            parameters.Add("CategoryId", categoryId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Category>(
                    query, parameters, commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task CreateCategory(Category category)
        {
            var query = "sp_AddCategory";
            var parameters = new DynamicParameters();
            parameters.Add("Name", category.Name, DbType.String);
            parameters.Add("Description", category.Description, DbType.String);
            parameters.Add("ImageUrl", category.ImageUrl, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            var query = "sp_UpdateCategory";
            var parameters = new DynamicParameters();
            parameters.Add("CategoryId", category.CategoryId, DbType.Int32);
            parameters.Add("Name", category.Name, DbType.String);
            parameters.Add("Description", category.Description, DbType.String);
            parameters.Add("ImageUrl", category.ImageUrl, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var rows = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return rows > 0;
            }
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            var query = "sp_DeleteCategory";
            var parameters = new DynamicParameters();
            parameters.Add("CategoryId", categoryId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var rows = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return rows > 0;
            }
        }
    }
}
