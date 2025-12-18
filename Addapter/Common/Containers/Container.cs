using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Interface;

namespace Common.Containers
{
    public class Container
    {
        private decimal _price { get; set; }//цена продукта
        private List<IProduct> _products { get; set; }//Список продуктов в корзине

        public Container(decimal price, List<IProduct> products = null)
        {
            _price = price;
            _products = products;
        }
        public void AddProduct(IProduct product) 
        {
            _products.Add(product);
        }
        public decimal GetPrice() 
        {
            var totalPrice = _price;

            foreach (var product in _products) { }
            return totalPrice;

        }

    }
}
