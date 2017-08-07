using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Configuration;

namespace Demo.Cosmos.WebAPI
{
    public class ValuesController : ApiController
    {
        string CosmosDB_EndpointUri { get; set; }
        string CosmosDB_PrimaryKey { get; set; }
        string CosmosDB_DBName { get; set; }
        string CosmosDB_Collection { get; set; }


        DocumentClient My_DocumentClient { get; set; }
        DBHelper My_DBHelper { get; set; }

        public ValuesController()
        {
            this.CosmosDB_EndpointUri = ConfigurationManager.AppSettings["EndpointUri"];
            this.CosmosDB_PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];
            this.CosmosDB_DBName = ConfigurationManager.AppSettings["DatabaseName"];
            this.CosmosDB_Collection = ConfigurationManager.AppSettings["CollectionName"];
            this.My_DocumentClient = new DocumentClient(new Uri(this.CosmosDB_EndpointUri), this.CosmosDB_PrimaryKey);
            this.My_DBHelper = new DBHelper();
        }

		// GET api/<controller>
		// GET api/<controller>
        public IEnumerable<Family> Get()
        {
            try
            {
                return My_DBHelper.GetAllFamilyDocuments(this.My_DocumentClient, this.CosmosDB_DBName, this.CosmosDB_Collection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET api/<controller>/5
        public Family Get(string id)
        {
            try
            {
                return My_DBHelper.GetFamilyDocumentByID(this.My_DocumentClient, this.CosmosDB_DBName, this.CosmosDB_Collection, id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST api/<controller>
        public string Post([FromBody]Family value)
        {
            try
            {
                if (value != null)
                {
                    string result = My_DBHelper.AddFamilyDocument(this.My_DocumentClient, this.CosmosDB_DBName, this.CosmosDB_Collection, value);
                    return result;
                }
                throw new Exception("Invalid operation");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT api/<controller>/5
        public string Put(string id, [FromBody]Family value)
        {
            try
            {
                if (!string.IsNullOrEmpty(id) && value != null)
                {
                    string result = My_DBHelper.UpdateFamilyDocument(this.My_DocumentClient, this.CosmosDB_DBName, this.CosmosDB_Collection, id, value);
                    return result;
                }
                throw new Exception("Invalid operation");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // DELETE api/<controller>/5
        public string Delete(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    string result = My_DBHelper.DeleteFamilyDocument(this.My_DocumentClient, this.CosmosDB_DBName, this.CosmosDB_Collection, id);
                    return result;
                }
                throw new Exception("Invalid operation");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}