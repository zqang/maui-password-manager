using PasswordManager.ViewModels;

namespace PasswordManager.Views;

public partial class EditPasswordPage : ContentPageBase
{
    public EditPasswordPage(EditPasswordViewModel viewModel)
    {
        
        BindingContext = viewModel;
        InitializeComponent();
    }

    private void OnBackButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void OnShowPasswordClicked(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
    }
}