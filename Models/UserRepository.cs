namespace Ninel_INFASS2.Models
{
    public class UserRepository
    {
        private static List<RegisterModel> Users { get; } = new();
        private static int _nextId = 1;

        public static List<RegisterModel> GetAll()
        {
            return Users.ToList();
        }

        public static RegisterModel GetById(int id)
        {
            return Users.FirstOrDefault(u => u.Id == id);
        }

        public static bool ExistsByUsername(string username, int? excludeId = null)
        {
            return Users.Any(x => x.Username == username && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public static bool ExistsByEmail(string email, int? excludeId = null)
        {
            return Users.Any(x => x.Email == email && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public static RegisterModel Add(RegisterModel model)
        {
            model.Id = _nextId++;
            Users.Add(model);
            return model;
        }

        public static bool Update(RegisterModel model)
        {
            var user = Users.FirstOrDefault(u => u.Id == model.Id);
            if (user == null) return false;

            user.Name = model.Name;
            user.Email = model.Email;
            user.Gender = model.Gender;
            user.Age = model.Age;
            user.Address = model.Address;
            user.Username = model.Username;
            if (!string.IsNullOrEmpty(model.Password))
                user.Password = model.Password;

            return true;
        }

        public static bool Delete(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;
            return Users.Remove(user);
        }
    }
}
