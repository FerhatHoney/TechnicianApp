using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Firebase.Firestore;

namespace TechnicianApp.Core.Models
{
    public class AppNote
    {
        [FirestoreProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [FirestoreProperty("technicianId")]
        public string TechnicianId { get; set; } = string.Empty;

        [FirestoreProperty("content")]
        public string Content { get; set; } = string.Empty;

        [FirestoreProperty("date")]
        public DateTimeOffset DateOffset { get; set; } = DateTimeOffset.UtcNow;

        public DateTime Date
        {
            get => DateOffset.DateTime;
            set => DateOffset = value;
        }

        [FirestoreProperty("isReminder")]
        public bool IsReminder { get; set; } = false;

        [FirestoreProperty("reminderDate")]
        public DateTimeOffset? ReminderDateOffset { get; set; }

        public DateTime? ReminderDate
        {
            get => ReminderDateOffset?.DateTime;
            set => ReminderDateOffset = value;
        }
    }
}