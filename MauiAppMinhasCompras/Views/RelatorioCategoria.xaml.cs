using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioCategoria : ContentPage
{
    public RelatorioCategoria()
    {
        InitializeComponent();
        CarregarRelatorio();
    }

    private async void CarregarRelatorio()
    {
        try
        {
            var relatorio = await App.Db.CalcularTotalGastoPorCategoria();
            var listaRelatorio = new ObservableCollection<RelatorioCategoriaModel>();

            foreach (var item in relatorio)
            {
                listaRelatorio.Add(new RelatorioCategoriaModel { Categoria = item.Key, Total = item.Value });
            }

            lst_relatorio.ItemsSource = listaRelatorio;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao carregar o relatório: {ex.Message}", "OK");
        }
    }

    public class RelatorioCategoriaModel
    {
        public string Categoria { get; set; }
        public double Total { get; set; }
    }
}