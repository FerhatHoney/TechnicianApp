using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
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

        // Firebase sadece bu sayıyı okuyup/yazacak
        [FirestoreProperty("status")]
        public int StatusInt { get; set; } = (int)TaskStatus.Bekliyor;

        // Ekranımız (UI) bu Enum'ı kullanacak (Firebase bunu görmezden gelir)
        public TaskStatus Status
        {
            get => (TaskStatus)StatusInt;
            set => StatusInt = (int)value;
        }

        // Firebase tarihleri DateTimeOffset olarak ister
        [FirestoreProperty("createdAt")]
        public DateTimeOffset CreatedAtOffset { get; set; } = DateTimeOffset.UtcNow;

        // Ekranımız standart DateTime kullanır
        public DateTime CreatedAt
        {
            get => CreatedAtOffset.DateTime;
            set => CreatedAtOffset = value;
        }

        [FirestoreProperty("completedAt")]
        public DateTimeOffset? CompletedAtOffset { get; set; }

        public DateTime? CompletedAt
        {
            get => CompletedAtOffset?.DateTime;
            set => CompletedAtOffset = value;
        }

        [FirestoreProperty("actionsTaken")]
        public string ActionsTaken { get; set; } = string.Empty;
    }
}