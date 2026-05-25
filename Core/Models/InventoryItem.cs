using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Firebase.Firestore;

namespace TechnicianApp.Core.Models
{
    public class InventoryItem
    {
        [FirestoreProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [FirestoreProperty("name")]
        public string Name { get; set; } = string.Empty;

        [FirestoreProperty("serialNumber")]
        public string SerialNumber { get; set; } = string.Empty;

        [FirestoreProperty("status")]
        public int StatusInt { get; set; } = (int)ItemStatus.Saglam;

        public ItemStatus Status
        {
            get => (ItemStatus)StatusInt;
            set => StatusInt = (int)value;
        }

        [FirestoreProperty("quantity")]
        public int Quantity { get; set; } = 1;

        [FirestoreProperty("currentLocationOrUnit")]
        public string CurrentLocationOrUnit { get; set; } = string.Empty;

        [FirestoreProperty("historyLogs")]
        public List<string> HistoryLogs { get; set; } = new();
    }
}