using DVE.Product.DataEntity;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace DVE.Product.Response.DataAccess
{
    public class ProductData
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        public ProductData(IConfiguration config)
        {
            _client = new MongoClient(config["ProductConnection:ConnectionString"].ToString());
            _database = _client.GetDatabase(config["ProductConnection:DatabaseName"]);
        }

        public List<ProductName> GetProductNames()
        {

            var productCollection = _database.GetCollection<ProductDetail>("Products");

            var productName = productCollection.Find(s => true).Project(s => new ProductName() { ID = s.ID, Name = s.Name }).ToList();

            return productName;
        }

        public ProductDetail GetProductDetail(string id)
        {

            var productCollection = _database.GetCollection<ProductDetail>("Products");
            var priceReductionCollection = _database.GetCollection<PriceReduction>("PriceReductions");

            var productDetail = productCollection.Find(s => s.ID == id).FirstOrDefault();

            if (productDetail != null)
            {
                //Monday = 1, Tuesday = 2 .... Sunday = 7.
                var today = ((int)DateTime.Today.DayOfWeek);
                if (today == 0)
                    today = 7;
                var reductionPercentage = priceReductionCollection.
                    Find(price => price.DayOfWeek == today).
                    Project(price => price.Reduction).First();

                productDetail.TodayPrice = Math.Round(productDetail.Price - (productDetail.Price * (reductionPercentage / 100)));
            }
            return productDetail;
        }

    }
}
