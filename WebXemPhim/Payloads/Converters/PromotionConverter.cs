using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataResponses;

namespace WebXemPhim.Payloads.Converters
{
    public class PromotionConverter
    {
        public DataRepsonsesPromotion ConvertDt(Promotion promotion)
        {
            return new DataRepsonsesPromotion
            {
                Description = promotion.Description,
                EndTime = promotion.EndTime,
                Id = promotion.Id,
                Name = promotion.Name,
                Percent = promotion.Percent,
                Quantity = promotion.Quantity,
                StartTime = promotion.StartTime,
                Type = promotion.Type
            };
        }
    }
}
