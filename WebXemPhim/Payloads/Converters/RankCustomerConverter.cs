using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataResponses;

namespace WebXemPhim.Payloads.Converters
{
    public class RankCustomerConverter
    {
        public DataResponsesRankCustomer ConvertDt(RankCustomer rankCustomer)
        {
            return new DataResponsesRankCustomer
            {
                Description = rankCustomer.Description,
                Id = rankCustomer.Id,
                Name = rankCustomer.Name,
                Point = rankCustomer.Point,
            };
        }
    }
}
