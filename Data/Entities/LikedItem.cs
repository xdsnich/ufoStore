namespace ufoShopBack.Data.Entities
{
    public class LikedItem
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public int ProductId {  get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
