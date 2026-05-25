using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Firestore;
using TechnicianApp.Core.Interfaces;
using TechnicianApp.Core.Models;

namespace TechnicianApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private const string UsersCollection = "Users";

        public async Task<AppUser> LoginWithEmailAsync(string email, string password)
        {
            try
            {
                var user = await CrossFirebaseAuth.Current.SignInWithEmailAndPasswordAsync(email, password);
                var appUser = await GetUserFromFirestore(user.Uid);

                // Kullanıcı yoksa veya onaylanmamışsa
                if (appUser == null)
                {
                    await CrossFirebaseAuth.Current.SignOutAsync();
                    throw new Exception("Kullanıcı kaydı bulunamadı.");
                }

                if (!appUser.IsApproved)
                {
                    // Güvenlik: Oturumu geri kapat
                    await CrossFirebaseAuth.Current.SignOutAsync();
                    throw new Exception("Hesabınız henüz yönetici tarafından onaylanmadı. Lütfen bekleyin.");
                }

                return appUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"Giriş başarısız: {ex.Message}");
            }
        }

        public async Task<AppUser> RegisterWithEmailAsync(string fullName, string email, string password)
        {
            try
            {
                var user = await CrossFirebaseAuth.Current.CreateUserAsync(email, password);

                var newUser = new AppUser
                {
                    Id = user.Uid,
                    FullName = fullName,
                    Email = email,
                    Role = "Teknisyen",
                    IsApproved = false, // Varsayılan olarak onaysız!
                    CreatedAt = DateTime.UtcNow
                };

                await CrossFirebaseFirestore.Current
                    .GetCollection(UsersCollection)
                    .GetDocument(newUser.Id)
                    .SetDataAsync(newUser);

                // Kayıt olan kişinin de oturumunu hemen kapatıyoruz ki onaysız içeri giremesin
                await CrossFirebaseAuth.Current.SignOutAsync();

                return newUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"Kayıt başarısız: {ex.Message}");
            }
        }

        // CS1998 Uyarısı Çözümü (async kaldırıldı çünkü Task dönüyoruz)
        // async kelimesini kaldırdık ve Task dönüyoruz
        public Task<AppUser?> LoginWithGoogleAsync()
        {
            // Plugin.Firebase v4'te SignInWithGoogleAsync kaldırıldığı için
            // Native Credential Manager entegrasyonu gerektirir.
            // Projenin hızını kesmemek adına şimdilik pasif bırakıyoruz.
            throw new NotImplementedException("Google girişi v4 sürümünde native kurulum gerektirir. Şimdilik E-Posta ile devam edelim.");
        }

        public async Task LogoutAsync()
        {
            await CrossFirebaseAuth.Current.SignOutAsync();
        }

        // CS8603 Uyarısı Çözümü (? eklendi)
        public async Task<AppUser?> GetCurrentUserAsync()
        {
            var currentUser = CrossFirebaseAuth.Current.CurrentUser;
            if (currentUser == null) return null;

            return await GetUserFromFirestore(currentUser.Uid);
        }

        // CS8603 Uyarısı Çözümü (? eklendi)
        private async Task<AppUser?> GetUserFromFirestore(string uid)
        {
            var doc = await CrossFirebaseFirestore.Current
                        .GetCollection(UsersCollection)
                        .GetDocument(uid)
                        .GetDocumentSnapshotAsync<AppUser>();

            return doc?.Data;
        }
    }
}