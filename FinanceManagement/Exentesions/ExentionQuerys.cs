using FinanceManagement.Message;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinanceManagement.Exentesions
{
    public class ExentionQuerys
    {
        public static void InsertUpdateObject<T>(T entity) where T : class
        {
            using (var dbContext = new dbFinanceManagementEntities())
            {
                dbContext.Set<T>().AddOrUpdate(entity);
                dbContext.SaveChanges();
            }
        }

        public static void DeleteObject<T>(T entity) where T : class
        {
            using (var dbContext = new dbFinanceManagementEntities())
            {
                if (dbContext.Entry(entity).State == EntityState.Detached)
                {
                    dbContext.Set<T>().Attach(entity);
                }

                dbContext.Set<T>().Remove(entity);
                dbContext.SaveChanges();
            }
        }
        public static void DeleteObjects<T>(List<T> entities) where T : class
        {
            using (var dbContext = new dbFinanceManagementEntities())
            {
                foreach (var entity in entities)
                {
                    if (dbContext.Entry(entity).State == EntityState.Detached)
                    {
                        dbContext.Set<T>().Attach(entity);
                    }

                    dbContext.Set<T>().Remove(entity);
                }

                dbContext.SaveChanges();
            }
        }


        public static List<T> GetListObjectByValueId<T>(T entity, int userId, string crrValueID, DbContext dbContext) where T : class
        {
            IQueryable<T> query = dbContext.Set<T>();

            if(!string.IsNullOrEmpty(crrValueID) && userId != -1)
            {
                // Kiểm tra xem đối tượng có thuộc tính UserId không
                var valueIdProperty = entity.GetType().GetProperty(crrValueID);
                if (valueIdProperty == null)
                {
                    throw new InvalidOperationException("Entity does not have a property named");
                }

                // Sử dụng biểu thức Lambda để truy cập thuộc tính của đối tượng
                var parameter = Expression.Parameter(typeof(T), "item");
                var valueIdExpression = Expression.Property(parameter, valueIdProperty);

                // Chuyển đổi giá trị userId thành System.Nullable<int>
                var crrIdValue = Expression.Constant((int?)userId, typeof(int?));

                var equalityExpression = Expression.Equal(valueIdExpression, crrIdValue);
                var lambda = Expression.Lambda<Func<T, bool>>(equalityExpression, parameter);

                // Lọc theo UserID
                query = query.Where(lambda);
            }

            // Chuyển đổi kết quả thành danh sách đối tượng
            List<T> result = query
                .AsNoTracking()
                .AsEnumerable()  // Chuyển sang kiểu IEnumerable để thực hiện lọc trên máy chủ
                .Cast<T>()  // Chuyển về kiểu object
                .ToList();

            return result;
        }

        public static T GetObjectByValueId<T>(T entity, int valueID, string crrValueID, DbContext dbContext) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            IQueryable<T> query = dbContext.Set<T>();
            var valueIdProperty = typeof(T).GetProperty(crrValueID);
            if (valueIdProperty == null)
            {
                throw new InvalidOperationException($"Entity {typeof(T).Name} does not have a property named '{crrValueID}'");
            }

            var parameter = Expression.Parameter(typeof(T), "item");
            var valuedExpression = Expression.Property(parameter, valueIdProperty);

            // Chuyển đổi giá trị valueID thành System.Nullable<int>
            var crrIdValue = Expression.Constant((int?)valueID, typeof(int?));

            // Sử dụng Expression.Equal và Expression.Convert để so sánh giá trị kiểu Nullable
            var equalityExpression = Expression.Equal(valuedExpression, Expression.Convert(crrIdValue, valuedExpression.Type));

            var lambda = Expression.Lambda<Func<T, bool>>(equalityExpression, parameter);

            T result = query.AsNoTracking().FirstOrDefault(lambda);

            return result;
        }

        public static List<T> GetListObjectSearchByValue<T>(
            T entity,
            int userId,
            string crrValueID,
            Func<T, bool> additionalPredicate,
            DbContext dbContext) where T : class
        {
            IQueryable<T> query = dbContext.Set<T>();

            if(userId != -1 && !string.IsNullOrEmpty(crrValueID))
            {
                // Kiểm tra xem đối tượng có thuộc tính UserId không
                var valueIdProperty = entity.GetType().GetProperty(crrValueID);
                if (valueIdProperty == null)
                {
                    throw new InvalidOperationException("Entity does not have a property named");
                }

                // Sử dụng biểu thức Lambda để truy cập thuộc tính của đối tượng
                var parameter = Expression.Parameter(typeof(T), "item");
                var valueIdExpression = Expression.Property(parameter, valueIdProperty);

                // Chuyển đổi giá trị userId thành System.Nullable<int>
                var crrIdValue = Expression.Constant((int?)userId, typeof(int?));

                var equalityExpression = Expression.Equal(valueIdExpression, crrIdValue);
                var lambda = Expression.Lambda<Func<T, bool>>(equalityExpression, parameter);

                // Lọc theo UserID và điều kiện bổ sung
                query = query.Where(lambda);
            }
            if (additionalPredicate != null)
            {
                query = query.Where(additionalPredicate).AsQueryable();
            }

            // Chuyển đổi kết quả thành danh sách đối tượng
            List<T> result = query
                .AsNoTracking()
                .ToList();

            return result;
        }

        public static string GetSumAmountByUserId<T>(
            T entity,
            int valueID,
            string crrValueID,
            Expression<Func<T, bool>> filter,
            Func<T, decimal> amountSelector,
            DbContext dbContext) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            IQueryable<T> query = dbContext.Set<T>();
            var valueIdProperty = typeof(T).GetProperty(crrValueID);
            if (valueIdProperty == null)
            {
                throw new InvalidOperationException($"Entity {typeof(T).Name} does not have a property named '{crrValueID}'");
            }

            var parameter = Expression.Parameter(typeof(T), "item");
            var valuedExpression = Expression.Property(parameter, valueIdProperty);

            // Chuyển đổi giá trị valueID thành System.Nullable<int>
            var crrIdValue = Expression.Constant((int?)valueID, typeof(int?));

            // Sử dụng Expression.Equal và Expression.Convert để so sánh giá trị kiểu Nullable
            var equalityExpression = Expression.Equal(valuedExpression, Expression.Convert(crrIdValue, valuedExpression.Type));

            var lambda = Expression.Lambda<Func<T, bool>>(equalityExpression, parameter);

            // Lọc theo filter
            query = query.Where(filter);

            // Tính tổng theo amountSelector
            decimal sum = query.Sum(amountSelector);

            // Tạo một CultureInfo cho vùng địa lý Việt Nam
            CultureInfo cultureInfo = new CultureInfo("vi-VN");

            // Định dạng số decimal với CultureInfo
            return sum.ToString("N0", cultureInfo).Replace('.', ',') + " VNĐ";
        }

        public static List<double> FilterMonthlyAmountsByYear<T>(List<T> objects, Func<T, DateTime?> createDateSelector, int selectedYear)
        {
            var monthlyAmounts = Enumerable.Range(1, 12)
                .Select(month =>
                {
                    DateTime startOfMonth = new DateTime(selectedYear, month, 1);
                    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                    return Convert.ToDouble(objects
                        .Count(item => createDateSelector(item)?.Date >= startOfMonth && createDateSelector(item)?.Date <= endOfMonth));
                })
                .ToList();

            return monthlyAmounts;
        }



        public static T DeepCopy<T>(T obj) where T : class
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException(MessagesValue.MesageCopyObject);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();

            using (stream)
            {
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}