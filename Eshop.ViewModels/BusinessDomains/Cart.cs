using Eshop.Models.BusinessDomains;

namespace Eshop.ViewModels.BusinessDomains
{
    public class Cart
    {
        private List<CartLine> lineCollection = [];

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
