using Dapper;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperContext _context;

        public OrderRepository(DapperContext context)
        {
            _context = context;
        }

        // إنشاء أوردر جديد (Order + Items + Shipping)
        public async Task<int> CreateOrderAsync(CreateOrderDto dto)
        {
            using var connection = _context.CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                // 1️⃣ إنشاء الأوردر
                var orderId = await connection.ExecuteScalarAsync<int>(
                    "sp_CreateOrder",
                    new
                    {
                        UserId = dto.CustomerId,
                        TotalAmount = dto.Items.Sum(i => i.Quantity * i.Price)
                    },
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction
                );

                // 2️⃣ إضافة العناصر
                foreach (var item in dto.Items)
                {
                    await connection.ExecuteAsync(
                        "sp_AddOrderItem",
                        new
                        {
                            OrderId = orderId,
                            item.ProductId,
                            item.Quantity,
                            Price = item.Price
                        },
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction
                    );
                }

                // 3️⃣ إضافة تفاصيل الشحن
                await connection.ExecuteAsync(
                    @"INSERT INTO OrderShipping 
                        (OrderId, RecipientName, Phone, AddressLine1, AddressLine2, City, State, PostalCode, Country, Notes, CreatedAt) 
                      VALUES 
                        (@OrderId, @RecipientName, @Phone, @AddressLine1, @AddressLine2, @City, @State, @PostalCode, @Country, @Notes, GETDATE())",
                    new
                    {
                        OrderId = orderId,
                        dto.RecipientName,
                        Phone = dto.Phone ?? "N/A",
                        AddressLine1 = dto.ShippingAddress,
                        dto.AddressLine2,
                        dto.City,
                        dto.State,
                        dto.PostalCode,
                        dto.Country,
                        dto.Notes
                    },
                    transaction: transaction
                );

                transaction.Commit();
                return orderId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // جلب أوردر كامل بالتفاصيل (Order + Items + Shipping)
        public async Task<OrderDetailDto?> GetOrderByIdAsync(int orderId)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                "sp_GetOrderDetails",
                new { OrderId = orderId },
                commandType: CommandType.StoredProcedure
            );

            // 1) Order + User
            var order = await multi.ReadFirstOrDefaultAsync<OrderDetailDto>();
            if (order == null) return null;

            // 2) Items
            var items = (await multi.ReadAsync<OrderItemDto>()).ToList();
            order.Items = items;

            // 3) Shipping
            order.Shipping = await multi.ReadFirstOrDefaultAsync<OrderShippingDto>();

            return order;
        }

        // جلب أوردرات يوزر (كاملة)
        public async Task<IEnumerable<OrderDetailDto>> GetOrdersByCustomerAsync(int customerId)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                "sp_GetOrdersByCustomer",
                new { CustomerId = customerId },
                commandType: CommandType.StoredProcedure
            );

            // 🟢 Orders + Users
            var orders = (await multi.ReadAsync<OrderDetailDto>()).ToList();

            // 🟢 Items
            var items = (await multi.ReadAsync<OrderItemDto>()).ToList();

            // 🟢 Shipping
            var shippings = (await multi.ReadAsync<OrderShippingDto>()).ToList();

            // 🟢 ربط البيانات
            foreach (var order in orders)
            {
                order.Items = items.Where(i => i.OrderId == order.OrderId).ToList();
                order.Shipping = shippings.FirstOrDefault(s => s.OrderId == order.OrderId);
            }

            return orders;
        }

        // جلب كل الأوردرات (أدمِن) مع التفاصيل
        public async Task<IEnumerable<OrderDetailDto>> GetAllOrdersAsync()
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                "sp_GetAllOrders",
                commandType: CommandType.StoredProcedure
            );

            var orders = (await multi.ReadAsync<OrderDetailDto>()).ToList();
            var items = (await multi.ReadAsync<OrderItemDto>()).ToList();
            var shippings = (await multi.ReadAsync<OrderShippingDto>()).ToList();

            foreach (var order in orders)
            {
                order.Items = items.Where(i => i.OrderId == order.OrderId).ToList();
                order.Shipping = shippings.FirstOrDefault(s => s.OrderId == order.OrderId);
            }

            return orders;
        }

        // تحديث حالة الأوردر (Admin)
        public async Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
        {
            var sql = "UPDATE Orders SET Status = @Status WHERE OrderId = @OrderId";

            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(sql, new { Status = dto.Status.ToString(), dto.OrderId });

            return rows > 0;
        }

        // جلب تفاصيل الشحن فقط
        public async Task<OrderShippingDto?> GetShippingInfoAsync(int orderId)
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<OrderShippingDto>(
                "SELECT * FROM OrderShipping WHERE OrderId = @OrderId",
                new { OrderId = orderId }
            );
        }
    }
}
