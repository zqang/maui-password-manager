using PasswordManager.Services;
using PasswordManager.ViewModels.Base;
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels;

public class AddPasswordViewModel : ViewModelBase
{
    public ObservableCollection<PasswordItem> Passwords { get; set; }
    public AddPasswordViewModel(INavigationService navigationService) : base(navigationService)
    {
        Passwords = new ObservableCollection<PasswordItem>
            {
                new PasswordItem { URL = "http://103.233.3.93:8080", IconSource = "network_icon.png" },
                new PasswordItem { URL = "http://127.0.0.1:8000", IconSource = "network_icon.png" },
                new PasswordItem { URL = "http://192.168.25.1", IconSource = "network_icon.png" },
                new PasswordItem { URL = "account.bbk.com", IconSource = "android_icon.png" },
                new PasswordItem { URL = "adobe.com", IconSource = "adobe_icon.png" },
                new PasswordItem { URL = "Airbnb", IconSource = "airbnb_icon.png" },
                new PasswordItem { URL = "alipay.com", IconSource = "alipay_icon.png" },
                new PasswordItem { URL = "amazon.com", IconSource = "amazon_icon.png" }
            };
    }

    public class PasswordItem
    {
        public string URL { get; set; }
        public string IconSource { get; set; }
    }
}