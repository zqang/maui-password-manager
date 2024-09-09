using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Views;

public partial class AddPasswordPage : ContentPage
{
    public AddPasswordPage(AddPasswordViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navigate back or close the page
        Navigation.PopAsync();
    }

    private void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // Implement save logic here
    }

    private void OnShowPasswordClicked(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
    }
}