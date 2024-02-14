public class UserService : GenericRepository<User>
{
    public UserService(AlMaqraaDB context) : base(context)
    {

    }
}

