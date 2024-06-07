using Microsoft.EntityFrameworkCore;

public class UserService : GenericRepository<User>
{
    public UserService(AlMaqraaDB context) : base(context)
    {

    }
    public async Task<Statistics> GetStatisticByUserId(string userId)
    {
        // Retrieve the statistic by ID
        User user = await _context.Users
        .Include(s => s.Statistics)
        .FirstOrDefaultAsync(ss => ss.Id == userId);

        // Retrieve the user associated with the statistic
        var statistics = user.Statistics;

        return statistics;
    }
}

