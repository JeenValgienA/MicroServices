using DVE.Product.DataEntity;
using EasyNetQ;
using System;
using System.Collections.Generic;
using Xunit;

namespace DVE.Product.Test
{
    public class ProductTest
    {
        /// <summary>
        /// List all the products available on the system. Currently 4 products are available.
        /// </summary>
        [Fact]
        public void Test_GetAllProducts()
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost;virtualHost=MQHost;username=admin;password=C0mplexPa$$w0rd"))
            {
                Console.WriteLine("Making request");
                var products = bus.Rpc.Request<Byte, List<ProductName>>(0);

                foreach (var product in products)
                {
                    Console.WriteLine("Got response: ID: '{0}', Name: '{1}'", product.ID, product.Name);
                }
                Console.WriteLine("Got response: '{0}'", products.Count.ToString());

                var model = Assert.IsAssignableFrom<List<ProductName>>(products);
                Assert.Equal(4, model.Count);
            }
        }

        /// <summary>
        /// The product ID 1 is valid on the system, hence it return the product detail.
        /// </summary>
        [Fact]
        public void Test_GetProductDetail()
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost;virtualHost=MQHost;username=admin;password=C0mplexPa$$w0rd"))
            {
                Console.WriteLine("Making request");
                var product = bus.Rpc.Request<string, ProductDetail>("1");


                Console.WriteLine("Got response: ID: '{0}', Name: '{1}', Price: '{2}'", product.ID, product.Name, product.TodayPrice.ToString());

                var model = Assert.IsAssignableFrom<ProductDetail>(product);
                Assert.Equal("1", model.ID);
            }
        }

        /// <summary>
        /// As the product ID 10 is invalid on the system, it should return Null.
        /// </summary>
        [Fact]
        public void Test_GetInvalidProductDetail()
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost;virtualHost=MQHost;username=admin;password=C0mplexPa$$w0rd"))
            {
                Console.WriteLine("Making request");
                var product = bus.Rpc.Request<string, ProductDetail>("10");

                if (product != null)
                {
                    Console.WriteLine("Got response: ID: '{0}', Name: '{1}', Price: '{2}'", product.ID, product.Name, product.TodayPrice.ToString());

                    var model = Assert.IsAssignableFrom<ProductDetail>(product);
                    Assert.Equal("1", model.ID);
                }
                else
                {
                    Console.WriteLine("Invalid product id)");
                    Assert.Null(product);
                }
            }
        }
    }
}
