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
                // ÇÖZÜM 1: <T> Geri eklendi
                var querySnapshot = await CrossFirebaseFirestore.Current.GetCollection(collectionPath).GetDocumentsAsync<T>();

                foreach (var document in querySnapshot.Documents)
                {
                    try
                    {
                        var item = document.Data;
                        if (item != null)
                        {
                            var idProp = typeof(T).GetProperty("Id");
                            if (idProp != null && string.IsNullOrEmpty(idProp.GetValue(item) as string))
                            {
                                idProp.SetValue(item, document.Reference.Id);
                            }
                            items.Add(item);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllAsync Hatası ({collectionPath}): {ex.Message}");
            }
            return items;
        }

        public async Task<T> GetByIdAsync(string collectionPath, string id)
        {
            try
            {
                // ÇÖZÜM 2: <T> Geri eklendi
                var document = await CrossFirebaseFirestore.Current.GetCollection(collectionPath).GetDocument(id).GetDocumentSnapshotAsync<T>();
                if (document != null && document.Data != null)
                {
                    var item = document.Data;
                    var idProp = typeof(T).GetProperty("Id");
                    if (idProp != null) idProp.SetValue(item, document.Reference.Id);
                    return item;
                }
            }
            catch { }
            return null;
        }

        public async Task AddAsync(string collectionPath, T entity, string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                await CrossFirebaseFirestore.Current.GetCollection(collectionPath).AddDocumentAsync(entity);
            }
            else
            {
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