using FinancialNewsMonitor.DataAccessLayer.Models;
using FinancialNewsMonitor.Responses;

namespace FinancialNewsMonitor.Mappers;

public interface IStockDataModelMapper
{
    StockDataResponse Map(StockDataModel symbolStockDataModel);
}

public class StockDataMapper : IStockDataModelMapper
{
    public StockDataResponse Map(StockDataModel symbolStockDataModel)
        => new StockDataResponse(
                new MetaDataResponse(
                    symbolStockDataModel.MetaData.Information,
                    symbolStockDataModel.MetaData.Symbol,
                    symbolStockDataModel.MetaData.LastRefreshed,
                    symbolStockDataModel.MetaData.TimeZone),
                symbolStockDataModel.StockValues.Select(Map));

    private StockValueResponse Map(StockValueModel value)
        => new StockValueResponse(
            value.Date,
            value.Open,
            value.High,
            value.Low,
            value.Close,
            value.Volume);
}
