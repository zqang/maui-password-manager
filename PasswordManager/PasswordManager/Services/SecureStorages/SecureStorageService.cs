using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Services.SecureStorages
{
    public class SecureStorageService : ISecureStorageService
    {
        private const string BiometricKey = "biometric_enabled";
        private const string AutoLockKey = "auto_lock_setting";
        private const string MasterPasswordKey = "master_password";

        public bool GetBiometricSetting()
        {
            return Preferences.Get(BiometricKey, false);
        }

        public void SetBiometricSetting(bool enabled)
        {
            Preferences.Set(BiometricKey, enabled);
        }

        public string GetAutoLockSetting()
        {
            return Preferences.Get(AutoLockKey, "After 5 minutes");
        }

        public void SetAutoLockSetting(string setting)
        {
            Preferences.Set(AutoLockKey, setting);
        }

        public bool VerifyMasterPassword(string password)
        {
            string storedHash = SecureStorage.GetAsync(MasterPasswordKey).Result;
            return storedHash == HashPassword(password);
        }

        public void ChangeMasterPassword(string currentPassword, string newPassword)
        {
            if (!VerifyMasterPassword(currentPassword))
            {
                throw new UnauthorizedAccessException("Current password is incorrect.");
            }

            string newHash = HashPassword(newPassword);
            SecureStorage.SetAsync(MasterPasswordKey, newHash).Wait();
        }

        public void ClearAllSecureData()
        {
            SecureStorage.RemoveAll();
            Preferences.Clear();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}