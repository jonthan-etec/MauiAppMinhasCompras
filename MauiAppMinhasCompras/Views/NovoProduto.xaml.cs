using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txt_descricao.Text) ||
            string.IsNullOrWhiteSpace(txt_quantidade.Text) ||
            string.IsNullOrWhiteSpace(txt_preco.Text))
        {
            await DisplayAlert("Aviso", "Preencha todos os campos!", "OK");
            return;
        }

        if (!double.TryParse(txt_quantidade.Text, out double quantidade))
        {
            await DisplayAlert("Erro", "Quantidade inválida! Digite um número válido.", "OK");
            return;
        }

        if (!double.TryParse(txt_preco.Text, out double preco))
        {
            await DisplayAlert("Erro", "Preço inválido! Digite um número válido.", "OK");
            return;
        }

        try
        {
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = quantidade,
                Preco = preco,
                Categoria = picker_categoria.SelectedItem?.ToString() ?? "Outros",
                DataCadastro = date_cadastro.Date
            };

            await App.Db.Insert(p);

            await DisplayAlert("Sucesso", "Registro inserido com sucesso!", "OK");

            txt_descricao.Text = string.Empty;
            txt_quantidade.Text = string.Empty;
            txt_preco.Text = string.Empty;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro ao salvar: {ex.Message}", "OK");
        }
    }
}