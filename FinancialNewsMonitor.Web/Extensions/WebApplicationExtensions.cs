namespace FinancialNewsMonitor.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication RegisterWebApplicationComponents(this WebApplication webApplication)
    {
        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI();
        }

        webApplication.UseHttpsRedirection();
        webApplication.UseStaticFiles();
        webApplication.UseRouting();
        webApplication.UseCors();
        //webApplication.UseAuthorization();

        webApplication.MapControllers();

        return webApplication;
    }
}
