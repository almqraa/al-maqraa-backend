using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


public class StatisticsService : GenericRepository<Statistics>
{
    public StatisticsService(AlMaqraaDB context) : base(context)
    {

    }
    public async Task<User> GetUserByStatisticId(int statisticId)
    {
        // Retrieve the statistic by ID
         Statistics statistic = await _context.Statistics
         .Include(s => s.User)
         .FirstOrDefaultAsync(ss => ss.Id == statisticId);

        // Retrieve the user associated with the statistic
        var user = statistic.User;

        return user;
    }
}
