namespace FinancialNewsMonitor.Services.Builders;

internal class UriRequestQueryBuilder
{
    private string _uriRequest = string.Empty;
    
    internal UriRequestQueryBuilder AddQueryParam(string key, string value)
    {
        if (string.IsNullOrEmpty(_uriRequest))
        {
            _uriRequest = string.Concat(_uriRequest, key, "=", value);
        }
        else
        {
            _uriRequest = string.Concat(_uriRequest, "&", key, "=", value);
        }

        return this;
    }

    internal string Build()
        => _uriRequest;
}
