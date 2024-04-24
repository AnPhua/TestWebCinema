using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataResponses;

namespace WebXemPhim.Payloads.Converters
{
    public class BannerConverter
    {
        public DataResponsesBanner ConvertDt(Banner banner)
        {
            return new DataResponsesBanner()
            {
                Id = banner.Id,
                ImageUrl = banner.ImageUrl,
                Title = banner.Title,
            };
        }
    }
}
