using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Firebase.Firestore;

namespace TechnicianApp.Core.Models
{
    public class AppTask
    {
        [FirestoreProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [FirestoreProperty("title")]
        public string Title { get; set; } = string.Empty;

        [FirestoreProperty("description")]
        public string Description { get; set; } = string.Empty;

        [FirestoreProperty("assignedTechnicianId")]
        public string AssignedTechnicianId { get; set; } = string.Empty;

        // Enum'ları Firestore'a int (sayı) olarak kaydetmek daha güvenlidir
        [FirestoreProperty("status")]
        public int StatusInt { get; set; } = (int)TaskStatus.Bekliyor;

        // Uygulama içinde (C# tarafında) kullanacağımız asıl Status enum'u
        // FirestoreProperty ETMİYORUZ, StatusInt üzerinden okutuyoruz.
        [FirestoreDocumentId] // Bu alan veritabanına doğrudan yazılmaz
        public TaskStatus Status
        {
            get => (TaskStatus)StatusInt;
            set => StatusInt = (int)value;
        }

        [FirestoreProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [FirestoreProperty("completedAt")]
        public DateTime? CompletedAt { get; set; }

        [FirestoreProperty("actionsTaken")]
        public string ActionsTaken { get; set; } = string.Empty;
    }
}