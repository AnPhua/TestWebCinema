using WebXemPhim.Entities;

namespace WebXemPhim.Services.Implements
{
    public class BaseServices
    {
        public readonly AppDbContext _appDbContext;
        public BaseServices()
        {
            _appDbContext = new AppDbContext();
        }
    }
}
