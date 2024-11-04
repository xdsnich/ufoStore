namespace ufoShopBack.Data.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId {  get; set; }
        public string ProductReview { get; set; }
        public Product Product { get; set; }
    }
}
