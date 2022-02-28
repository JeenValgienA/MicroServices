using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVE.Product.DataEntity
{
    public class ProductDetail : ProductName
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public DateTime EntryDate { get; set; }
        public double Price { get; set; }
        public double TodayPrice { get; set; }
    }
}
