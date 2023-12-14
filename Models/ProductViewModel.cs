namespace ASP.Models
{
    public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int InStock { get; set; }
    public string ImageUrl { get; set; } // Добавьте это свойство
}

}
