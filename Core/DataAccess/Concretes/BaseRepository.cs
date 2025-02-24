//using Core.DataAccess.Abstracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Core.DataAccess.Concretes
//{
//    public class BaseRepository<T> : IBaseRepository<T> where T : class
//    {
//        private readonly DapperContext _context;
//        private readonly string _tableName;

//        public BaseRepository(DapperContext context)
//        {
//            _context = context;
//            _tableName = typeof(T).Name.ToLower() + "s"; // PostgreSQL için tablo isimlerini küçük harf yapıyoruz.
//        }

//        public async Task<IEnumerable<T>> GetAllAsync()
//        {
//            using var connection = _context.CreateConnection();
//            var sql = $"SELECT * FROM {_tableName}";
//            return await connection.QueryAsync<T>(sql);
//        }

//        public async Task<T> GetByIdAsync(int id)
//        {
//            using var connection = _context.CreateConnection();
//            var sql = $"SELECT * FROM {_tableName} WHERE id = @Id";
//            return await connection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
//        }

//        public async Task<int> AddAsync(T entity)
//        {
//            using var connection = _context.CreateConnection();
//            var sql = $"INSERT INTO {_tableName} VALUES (@entity) RETURNING id";
//            return await connection.ExecuteScalarAsync<int>(sql, entity);
//        }

//        public async Task<int> UpdateAsync(T entity)
//        {
//            using var connection = _context.CreateConnection();
//            var sql = $"UPDATE {_tableName} SET @entity WHERE id = @Id";
//            return await connection.ExecuteAsync(sql, entity);
//        }

//        public async Task<int> DeleteAsync(int id)
//        {
//            using var connection = _context.CreateConnection();
//            var sql = $"DELETE FROM {_tableName} WHERE id = @Id";
//            return await connection.ExecuteAsync(sql, new { Id = id });
//        }
//    }
//}
