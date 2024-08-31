namespace PasswordManager.Views;

public partial class PasswordsPage : ContentPageBase
{
    public PasswordsPage(PasswordsViewModel viewModel)
    {
        BindingContext = viewModel;
        
        InitializeComponent();
    }
}