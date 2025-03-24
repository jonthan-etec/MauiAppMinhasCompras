using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Linq;

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
            try
            {
                List<Produto> tmp = await App.Db.GetAll();
                lista.Clear();
                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao carregar a lista de produtos: {ex.Message}", "OK");
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new Views.NovoProduto());
            }
            catch (Exception ex)
            {
                // Exibir erro caso ocorra algum problema na navegação
                DisplayAlert("Erro", $"Erro ao tentar abrir a página de novo produto: {ex.Message}", "OK");
            }
        }

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = e.NewTextValue;
            lista.Clear();
            try
            {
                List<Produto> tmp = await App.Db.Search(q);
                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao buscar produtos: {ex.Message}", "OK");
            }
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                double? soma = lista.Sum(i => (double?)i.Total); // Certifique-se de que `Total` é uma propriedade do modelo Produto e que aceita null
                string msg = soma.HasValue ? $"O total é {soma.Value:C}" : "Não há produtos para calcular o total.";
                DisplayAlert("Total dos Produtos", msg, "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", $"Erro ao calcular o total: {ex.Message}", "OK");
            }
        }

        private async void MenuItem_Clicked_1(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var produto = menuItem.BindingContext as Produto;
                if (produto != null)
                {
                    bool resposta = await DisplayAlert("Confirmação", "Tem certeza que deseja excluir este produto?", "Sim", "Não");
                    if (resposta)
                    {
                        try
                        {
                            lista.Remove(produto);
                            await App.Db.Delete(produto.Id);
                            await DisplayAlert("Sucesso", "Produto removido com sucesso!", "OK");
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Erro", $"Erro ao excluir produto: {ex.Message}", "OK");
                        }
                    }
                }
            }
        }

        private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Produto P = e.SelectedItem as Produto;
                if (P != null)
                {
                    await Navigation.PushAsync(new Views.EditarProduto
                    {
                        BindingContext = P
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("OPS", $"Erro ao selecionar produto: {ex.Message}", "OK");
            }
        }
    }
}
