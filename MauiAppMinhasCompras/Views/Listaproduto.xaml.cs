namespace MauiAppMinhasCompras.Views;

public partial class Listaproduto : ContentPage
{
	public Listaproduto()
	{
		InitializeComponent();
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
}