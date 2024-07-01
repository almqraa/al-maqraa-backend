using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


public class DayService : GenericRepository<Day>
{
    public DayService(AlMaqraaDB context) : base(context)
    {

    }
    public async Task<User> GetUserByDayId(int dayId)
    {
        // Retrieve the statistic by ID
         Day day = await _context.Days
        .Include(s => s.User)
        .FirstOrDefaultAsync(ss => ss.Id == dayId);

        // Retrieve the user associated with the statistic
        var user = day.User;

        return user;
    }
    public async Task<Day?> CheckDayByUserId(string userId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        // Retrieve the statistic by ID
        User user = await _context.Users
       .Include(s => s.Days)
       .FirstOrDefaultAsync(ss => ss.Id == userId);

        List<Day> days = user.Days?.ToList();
      
        Day? day = days?.FirstOrDefault(d => d.Date==today);
        if (day == null)
        {
            return null;
        }
        return day;

        //return user;
    }
}