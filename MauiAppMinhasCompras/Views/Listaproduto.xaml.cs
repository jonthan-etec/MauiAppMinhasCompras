using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiAppMinhasCompras.Views;

public partial class Listaproduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    ObservableCollection<Produto> listaFiltrada = new ObservableCollection<Produto>();

    public Listaproduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        await AtualizarLista();
    }

    private async Task AtualizarLista()
    {
        try
        {
            List<Produto> tmp = await App.Db.GetAll();
            lista.Clear();
            tmp.ForEach(i => lista.Add(i));
            listaFiltrada = new ObservableCollection<Produto>(lista);
            lst_produtos.ItemsSource = listaFiltrada;
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
            DisplayAlert("Erro", $"Erro ao tentar abrir a página de novo produto: {ex.Message}", "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string q = e.NewTextValue;
        var produtosFiltrados = listaFiltrada.Where(p => p.Descricao.ToLower().Contains(q.ToLower())).ToList();
        lst_produtos.ItemsSource = produtosFiltrados;
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            var itensExibidos = lst_produtos.ItemsSource as List<Produto>;

            if (itensExibidos != null && itensExibidos.Any())
            {
                double? soma = itensExibidos.Sum(i => (double?)i.Total);
                string msg = soma.HasValue ? $"O total é {soma.Value:C}" : "Não há produtos para calcular o total.";
                DisplayAlert("Total dos Produtos", msg, "OK");
            }
            else
            {
                double? soma = listaFiltrada.Sum(i => (double?)i.Total);
                string msg = soma.HasValue ? $"O total é {soma.Value:C}" : "Não há produtos para calcular o total.";
                DisplayAlert("Total dos Produtos", msg, "OK");
            }
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
                        listaFiltrada.Remove(produto);
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

    private void picker_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        string categoriaSelecionada = picker_categoria.SelectedItem?.ToString();
        if (categoriaSelecionada == "Todas")
        {
            listaFiltrada = new ObservableCollection<Produto>(lista);
        }
        else
        {
            listaFiltrada = new ObservableCollection<Produto>(lista.Where(p => p.Categoria == categoriaSelecionada));
        }
        lst_produtos.ItemsSource = listaFiltrada;
    }

    private async void BtnRelatorio_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new RelatorioCategoria());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao abrir a página de relatório: {ex.Message}", "OK");
        }
    }
}