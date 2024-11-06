using Microsoft.EntityFrameworkCore;
using ufoShopBack.Data;
using ufoShopBack.Services;
using ufoShopBack.Repos;
using ufoShopBack.Abstract;
using ufoShopBack.Data.Entities;
using ufoShopBack.Data.Enums;

namespace ufoShopBack.CRUDoperations
{
    public class UsersCRUD : ICRUDoperations<User>
    {
        private readonly Context _contextUsers;
        private readonly UserService _userServices;
        private readonly GenericCRUDoperations<User> _userRepository;
        private readonly DbSet<User> _users;
        private readonly RoleService _roleService;

        public UsersCRUD(Context context, UserService userServices)
        {
            _contextUsers = context;
            _userRepository = new GenericCRUDoperations<User>(_contextUsers);
            _userServices = userServices;
            _users = _contextUsers.Set<User>();
        }

        public async Task<List<User>> GetAsync() => await _userRepository.GetAsync();
        public async Task<User> GetAsync(int id) => await _userRepository.GetAsync(id);
        public async Task<User> GetAsync(string name)
        {
            var entityFromDb = await _users.FirstOrDefaultAsync(n => EF.Property<string>(n, "Nickname") == name);
            return entityFromDb ?? null!;
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            var entityFromDb = await _users.FirstOrDefaultAsync(e => EF.Property<string>(e, "Email") == email);
            return entityFromDb ?? null!;
        }
        public async Task CreateAsync(User user)
        {
            await _roleService.GiveRole(user, (int)RoleEnum.User);
            user.Password = _userServices.HashPassword(user.Password);
            await _userRepository.CreateAsync(user);
        }
        public async Task UpdateAsync(User user, int id)
        {
            user.Password = _userServices.HashPassword(user.Password);
            await _userRepository.UpdateAsync(user, id);
        }
        public async Task DeleteAsync(int id) => _userRepository.DeleteAsync(id);
    }
}
