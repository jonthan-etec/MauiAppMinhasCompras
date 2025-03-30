using MauiAppMinhasCompras.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDataBaseHelpers
    {
        private readonly SQLiteAsyncConnection _connection;

        public SQLiteDataBaseHelpers(string path)
        {
            _connection = new SQLiteAsyncConnection(path);
            _connection.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p)
        {
            return _connection.InsertAsync(p);
        }

        public Task<int> Update(Produto p)
        {
            // Adiciona a coluna Categoria na instrução SQL UPDATE
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=?, Categoria=? WHERE Id=?";
            return _connection.ExecuteAsync(sql, p.Descricao, p.Quantidade, p.Preco, p.Categoria, p.Id);
        }

        public Task<int> Delete(int id)
        {
            return _connection.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> GetAll()
        {
            return _connection.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE ?";
            return _connection.QueryAsync<Produto>(sql, "%" + q + "%");
        }

        public async Task<Dictionary<string, double>> CalcularTotalGastoPorCategoria()
        {
            var categorias = new Dictionary<string, double>();
            var produtos = await _connection.Table<Produto>().ToListAsync();

            foreach (var produto in produtos)
            {
                if (categorias.ContainsKey(produto.Categoria))
                {
                    categorias[produto.Categoria] += produto.Total;
                }
                else
                {
                    categorias[produto.Categoria] = produto.Total;
                }
            }
            return categorias;
        }
    }
}