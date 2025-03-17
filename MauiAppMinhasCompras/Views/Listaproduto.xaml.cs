using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views
{
    public partial class Listaproduto : ContentPage
    {
        ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

        public Listaproduto()
        {
            InitializeComponent();
            lst_produtos.ItemsSource = lista;
        }

        protected async override void OnAppearing()
        {
            List<Produto> tmp = await App.Db.GetAll();
            lista.Clear();
            tmp.ForEach(i => lista.Add(i));
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new Views.NovoProduto());
            }
            catch
            {
            }
        }

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = e.NewTextValue;
            lista.Clear();
            List<Produto> tmp = await App.Db.Search(q);
            tmp.ForEach(i => lista.Add(i));
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            double soma = lista.Sum(i => i.Total);
            string msg = $"O total � {soma:C}";
            DisplayAlert("Total dos Produtos", msg, "OK");
        }

        private async void MenuItem_Clicked_1(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var produto = menuItem.BindingContext as Produto;
                if (produto != null)
                {
                    lista.Remove(produto);
                    await App.Db.Delete(produto.Id);
                    await DisplayAlert("Sucesso", "Produto removido com sucesso!", "OK");
                }
            }
        }
    }
}
