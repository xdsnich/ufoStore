using ufoShopBack.Data.Entities;
using ufoShopBack.Data;
namespace ufoShopBack.Services
{
    public class RoleService
    {
        private readonly Context _context;
        public RoleService(Context context)
        {
            _context = context;
        }
        public async Task GiveRole(User user, int id)
        {
            if (user == null) return;
            if (id <= 0 || id > 3) return;

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = id
            };

            user.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
        }   
    }
}
