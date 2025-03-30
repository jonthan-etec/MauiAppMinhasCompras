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

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Validação dos campos
                if (string.IsNullOrWhiteSpace(txt_descricao.Text))
                {
                    await DisplayAlert("Atenção", "Por favor, preencha a descrição do produto.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_quantidade.Text) || !double.TryParse(txt_quantidade.Text, out _))
                {
                    await DisplayAlert("Atenção", "Por favor, preencha a quantidade com um valor numérico válido.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_preco.Text) || !double.TryParse(txt_preco.Text, out _))
                {
                    await DisplayAlert("Atenção", "Por favor, preencha o preço com um valor numérico válido.", "OK");
                    return;
                }

                if (picker_categoria.SelectedItem == null)
                {
                    await DisplayAlert("Atenção", "Por favor, selecione uma categoria.", "OK");
                    return;
                }

                var produto = (Produto)BindingContext;

                produto.Descricao = txt_descricao.Text;
                produto.Quantidade = double.Parse(txt_quantidade.Text);
                produto.Preco = double.Parse(txt_preco.Text);
                produto.Categoria = picker_categoria.SelectedItem.ToString();

                await App.Db.Update(produto);

                await DisplayAlert("Sucesso", "Produto alterado com sucesso!", "OK");

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao atualizar produto: {ex.Message}", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var produto = (Produto)BindingContext;
            if (produto != null)
            {
                txt_descricao.Text = produto.Descricao;
                txt_quantidade.Text = produto.Quantidade.ToString();
                txt_preco.Text = produto.Preco.ToString("F2");
                picker_categoria.SelectedItem = produto.Categoria;
            }
        }
    }
}