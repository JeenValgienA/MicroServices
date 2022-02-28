using DVE.Product.DataEntity;
using DVE.Product.Response.DataAccess;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DVE.Product.Response
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            //Read the configuration detail from appsettings.json
            IConfiguration config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", true, true)
          .Build();

            //Return the product name and its id from database
            List<ProductName> GetAllProduct(Byte dummyRequestParaeter)
            {
                try
                {
                    Console.WriteLine("Received Return all products");
                    ProductData productData = new ProductData(config);
                    return productData.GetProductNames();
                }
                catch (Exception ex)
                {
                    //Include log detail here
                    throw ex;
                }
            }

            //Return the product detail from db based on the product id
            ProductDetail GetProductDetail(string productId)
            {
                try
                {
                    Console.WriteLine("Received Return product details");
                    ProductData productData = new ProductData(config);
                    return productData.GetProductDetail(productId);
                }
                catch (Exception ex)
                {
                    //Include log detail here
                    throw ex;
                }
            }

            //Initializing the EaseNetQ with connection taken the appsettings.json
            var bus = RabbitHutch.CreateBus(config["MessageBroker:ConnectionString"]);

            await bus.Rpc.RespondAsync((Func<Byte, List<ProductName>>)GetAllProduct);
            await bus.Rpc.RespondAsync((Func<string, ProductDetail>)GetProductDetail);


            Console.ReadKey();

            bus.Dispose();
        }
    }
}
