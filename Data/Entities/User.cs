using System.ComponentModel.DataAnnotations.Schema;
namespace ufoShopBack.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        [Column("PasswordHash")]
        public string Password { get; set; }
        public string Nickname { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

        [Column("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [Column("Address")]
        public string? DeliveryAddress { get; set; }
        public string? Lastname { get; set; }
        public ICollection<LikedItem>? LikedItems { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
