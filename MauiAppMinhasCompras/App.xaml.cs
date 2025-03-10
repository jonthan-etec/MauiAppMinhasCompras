using MauiAppMinhasCompras.Helpers;
using System;
using System.IO;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDataBaseHelpers _db;
        public static SQLiteDataBaseHelpers Db
        {
            get
            {
                if (_db == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "banco_sqlite_compras.db3" //para o banco de dados
                    );

                    _db = new SQLiteDataBaseHelpers(path);
                }
                return _db;
            }
        }

        public App()
        {
            InitializeComponent();

            // Define a página principal da aplicação
            MainPage = new NavigationPage(new Views.Listaproduto());
        }
    }
}
