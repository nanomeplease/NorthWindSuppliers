namespace DataLayer.Models
{
    public class ProductsDO
    {
        //Declaring all object properties for products.
        public string currentClass = "ProductsDO";
        public int SupplierId;
        public int ProductId;
        public string ProductName;
        public string QuantityPerUnit;
        public int UnitsInStock;
        public int UnitsOnOrder;
        public int ReorderLevel;
        public decimal UnitPrice;
    }
}
