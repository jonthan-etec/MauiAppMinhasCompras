using MauiAppMinhasCompras.Models;
using System;

namespace MauiAppMinhasCompras.Views
{
    public partial class EditarProduto : ContentPage
    {
        public EditarProduto()
        {
            InitializeComponent();
        }

        // Manipulador de evento para o click do ToolbarItem
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            // Verifica se os campos obrigatórios estão preenchidos
            if (string.IsNullOrWhiteSpace(txt_descricao.Text) ||
                string.IsNullOrWhiteSpace(txt_quantidade.Text) ||
                string.IsNullOrWhiteSpace(txt_preco.Text))
            {
                await DisplayAlert("Aviso", "Preencha todos os campos!", "OK");
                return;
            }

            // Validação de quantidade
            if (!double.TryParse(txt_quantidade.Text, out double quantidade))
            {
                await DisplayAlert("Erro", "Quantidade inválida! Digite um número válido.", "OK");
                return;
            }

            // Validação de preço
            if (!double.TryParse(txt_preco.Text, out double preco))
            {
                await DisplayAlert("Erro", "Preço inválido! Digite um número válido.", "OK");
                return;
            }

            try
            {
                // Obtém o produto a partir do BindingContext
                var produto = (Produto)BindingContext;

                // Verifica se o produto existe e possui um Id válido
                if (produto == null || produto.Id == 0)
                {
                    await DisplayAlert("Erro", "Produto não encontrado para atualização.", "OK");
                    return;
                }

                // Atualiza os valores do produto com os dados da interface
                produto.Descricao = txt_descricao.Text;
                produto.Quantidade = quantidade;
                produto.Preco = preco;

                // Atualiza o produto no banco de dados e obtém o retorno (provavelmente uma lista)
                var resultado = await App.Db.Update(produto);

                // Verifica se a lista não está vazia (ou seja, se houve alteração)
                if (resultado != null && resultado.Count > 0)
                {
                    await DisplayAlert("Sucesso", "Produto alterado com sucesso!", "OK");
                }
                else
                {
                    await DisplayAlert("Erro", "Erro ao atualizar o produto no banco de dados.", "OK");
                }

                // Volta para a tela anterior após salvar
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                // Exibe um erro, caso algo tenha dado errado
                await DisplayAlert("Erro", $"Erro ao salvar o produto: {ex.Message}", "OK");
            }
        }

        // Método para preencher os campos da tela de edição com os dados do produto
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Verifica se o BindingContext está definido
            var produto = (Produto)BindingContext;
            if (produto != null)
            {
                txt_descricao.Text = produto.Descricao;
                txt_quantidade.Text = produto.Quantidade.ToString();
                txt_preco.Text = produto.Preco.ToString("F2"); // Exibe o preço com duas casas decimais
            }
        }
    }
}
