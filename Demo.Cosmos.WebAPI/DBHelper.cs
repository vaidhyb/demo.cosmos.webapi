using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Cosmos.WebAPI
{
    public class DBHelper
    {
        public List<Family> GetAllFamilyDocuments(DocumentClient client, string databaseName, string collectionName)
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Family> familyQueryInSql = client.CreateDocumentQuery<Family>(
                               UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                               "SELECT * FROM Family",
                               queryOptions);

            return familyQueryInSql.ToList();
        }

        public Family GetFamilyDocumentByID(DocumentClient client, string databaseName, string collectionName, string familyID)
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Family> familyQuery = client.CreateDocumentQuery<Family>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                    .Where(f => f.Id == familyID);

            List<Family> lstFamily = familyQuery.ToList();

            if (lstFamily.Count > 0)
            {
                return lstFamily.First();
            }
            return null;
        }


        public string UpdateFamilyDocument(DocumentClient client, string databaseName, string collectionName, string familyID, Family familyDetails)
        {
            client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, familyID), familyDetails);
            return familyDetails.Id;
        }

        public string AddFamilyDocument(DocumentClient client, string databaseName, string collectionName, Family familyDetails)
        {
            if (string.IsNullOrEmpty(familyDetails.Id))
            {
                familyDetails.Id = Guid.NewGuid().ToString();
            }
            client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), familyDetails);
            return familyDetails.Id;
        }

        public string DeleteFamilyDocument(DocumentClient client, string databaseName, string collectionName, string id)
        {
            client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, id));
            return id;
        }
    }
}
