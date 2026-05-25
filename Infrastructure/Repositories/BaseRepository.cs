using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Firebase.Firestore;
using TechnicianApp.Core.Interfaces;

namespace TechnicianApp.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        public async Task<List<T>> GetAllAsync(string collectionPath)
        {
            var items = new List<T>();
            try
            {
                var querySnapshot = await CrossFirebaseFirestore.Current.GetCollection(collectionPath).GetDocumentsAsync<T>();

                foreach (var document in querySnapshot.Documents)
                {
                    var item = document.Data as T;

                    if (item != null)
                    {
                        // DÜZELTME: document.Id yerine document.Reference.Id kullanıyoruz
                        var idProp = typeof(T).GetProperty("Id");
                        if (idProp != null && string.IsNullOrEmpty((string)idProp.GetValue(item) as string))
                        {
                            idProp.SetValue(item, document.Reference.Id);
                        }

                        items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllAsync Hatası ({collectionPath}): {ex.Message}");
            }
            return items;
        }

        public async Task<T?> GetByIdAsync(string collectionPath, string id)
        {
            try
            {
                var document = await CrossFirebaseFirestore.Current.GetCollection(collectionPath).GetDocument(id).GetDocumentSnapshotAsync<T>();

                // DÜZELTME: document.Exists yerine document.Data != null kontrolü yapıyoruz
                if (document != null && document.Data != null)
                {
                    var item = document.Data;
                    var idProp = typeof(T).GetProperty("Id");

                    // DÜZELTME: document.Id yerine document.Reference.Id kullanıyoruz
                    if (idProp != null) idProp.SetValue(item, document.Reference.Id);

                    return item;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetByIdAsync Hatası ({collectionPath}): {ex.Message}");
            }
            return null;
        }

        public async Task AddAsync(string collectionPath, T entity, string? id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                // Rastgele ID oluştur (Firestore Auto-ID)
                await CrossFirebaseFirestore.Current.GetCollection(collectionPath).AddDocumentAsync(entity);
            }
            else
            {
                // Belirli ID ile ekle
                await CrossFirebaseFirestore.Current.GetCollection(collectionPath).GetDocument(id).SetDataAsync(entity);
            }
        }

        public async Task UpdateAsync(string collectionPath, T entity, string id)
        {
            await CrossFirebaseFirestore.Current.GetCollection(collectionPath).GetDocument(id).SetDataAsync(entity);
        }

        public async Task DeleteAsync(string collectionPath, string id)
        {
            await CrossFirebaseFirestore.Current.GetCollection(collectionPath).GetDocument(id).DeleteDocumentAsync();
        }
    }
}