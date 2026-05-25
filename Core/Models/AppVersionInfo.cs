using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Firebase.Firestore;

namespace TechnicianApp.Core.Models
{
    public class AppVersionInfo
    {
        [FirestoreProperty("id")]
        public string Id { get; set; } = "CurrentVersion";

        [FirestoreProperty("versionCode")]
        public string VersionCode { get; set; } = "1.0"; // Uygulamanın versiyonu

        [FirestoreProperty("releaseNotes")]
        public string ReleaseNotes { get; set; } = "İlk sürüm yayınlandı.";

        [FirestoreProperty("downloadUrl")]
        public string DownloadUrl { get; set; } = string.Empty;

        // Firebase tarih çökmesini engellemek için Offset kullanıyoruz
        [FirestoreProperty("updatedAt")]
        public DateTimeOffset UpdatedAtOffset { get; set; } = DateTimeOffset.UtcNow;

        public DateTime UpdatedAt
        {
            get => UpdatedAtOffset.DateTime;
            set => UpdatedAtOffset = value;
        }
    }
}