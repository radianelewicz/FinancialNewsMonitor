using FinancialNewsMonitor.DataAccessLayer.UnitOfWork;

namespace FinancialNewsMonitor.DataAccessLayer.Repositories.Abstracts;

public abstract class RepositoryBase
{
    protected readonly IUnitOfWork unitOfWork;

    public RepositoryBase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
}
