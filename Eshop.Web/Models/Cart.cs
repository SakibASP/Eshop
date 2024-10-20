﻿using Eshop.Models.BusinessDomains;
using Microsoft.AspNetCore.Mvc;
//using Eshop.Web.Binder;

namespace Eshop.Web.Models
{
    //[ModelBinder(BinderType = typeof(CartModelBinder))]
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        //adding items to ta cart list
        public void AddItem(Product product, int quantity)
        {
            var line = lineCollection.Where(p => p.Product.AutoId == product.AutoId).FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        //removing items from the cart list
        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(l => l.Product.AutoId == product.AutoId);
        }
        public double ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }
        // clearing the cart list
        public void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
        public class CartLine
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }

    }
}
