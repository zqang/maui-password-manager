namespace PasswordManager.Services.SecureStorages
{
    public interface ISecureStorageService
    {
        bool GetBiometricSetting();
        void SetBiometricSetting(bool enabled);
        string GetAutoLockSetting();
        void SetAutoLockSetting(string setting);
        bool VerifyMasterPassword(string password);
        void ChangeMasterPassword(string currentPassword, string newPassword);
        void ClearAllSecureData();
    }
}