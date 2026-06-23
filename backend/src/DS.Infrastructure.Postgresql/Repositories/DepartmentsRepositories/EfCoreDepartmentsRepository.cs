using CSharpFunctionalExtensions;
using DS.Application.Database;
using DS.Domain.Entities;
using DS.Domain.Relation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.AppFails;

namespace DS.Infrastructure.Postgresql.Repositories.DepartmentsRepositories;

public class EfCoreDepartmentsRepository : IDepartmentsRepository
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<EfCoreDepartmentsRepository> _logger;

    public EfCoreDepartmentsRepository(DirectoryServiceDbContext dbContext, ILogger<EfCoreDepartmentsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Add(
        Department department,
        IReadOnlyList<DepartmentLocation> relation,
        CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.Departments.Add(department);

            if (relation.Count > 0)
                _dbContext.DepartmentLocations.AddRange(relation);

            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return department.Id;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Не удалось сохранить подразделение {DepartmentId}", department.Id);

            return Error.Failure("department.add.failed", "Не удалось сохранить подразделение");
        }
    }

    public async Task<Result<Department, Error>> GetById(Guid departmentId, CancellationToken cancellationToken)
    {
        try
        {
            var department = await _dbContext.Departments.FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);

            if (department is null)
                return Error.NotFound("department.not_found", $"Подразделение {departmentId} не найдено");

            return department;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Не удалось получить подразделение {DepartmentId}", departmentId);
            return Error.Failure ("department.get.failed", "Не удалось получить подразделение");
        }
    }

    public async Task<UnitResult<Error>> Update(Department department, CancellationToken cancellationToken)
    {
        try
        {
            int rowsAffected = await _dbContext.Departments
                .Where(d => d.Id == department.Id)
                .ExecuteUpdateAsync(
                    setter => setter
                        .SetProperty(d => d.Name, department.Name)
                        .SetProperty(d => d.Identifier, department.Identifier)
                        .SetProperty(d => d.Depth, department.Depth)
                        .SetProperty(d => d.Path, department.Path)
                        .SetProperty(d => d.UpdatedAt, department.UpdatedAt), cancellationToken);

            if (rowsAffected == 0)
                return Error.NotFound("department.not_found", $"Подразделение {department.Id} не найдено");
            
            return UnitResult.Success<Error>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Не удалось обновить название подразделения {DepartmentId}",  department.Id);
            return Error.Failure("department.name.update.failed", "Не удалось обновить название подразделения");
        }
    }

    public async Task<UnitResult<Error>> UpdateName(Department department, CancellationToken cancellationToken)
    {
        try
        {
            int rowsAffected = await _dbContext.Departments
                .Where(d => d.Id == department.Id)
                .ExecuteUpdateAsync(
                    setter => setter
                    .SetProperty(d => d.Name, department.Name)
                    .SetProperty(d => d.UpdatedAt, department.UpdatedAt), cancellationToken);

            if (rowsAffected == 0)
                return Error.NotFound("department.not_found", $"Подразделение {department.Id} не найдено");
            
            return UnitResult.Success<Error>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Не удалось обновить название подразделения {DepartmentId}",  department.Id);
            return Error.Failure("departmnet.name.update.failed", "Не удалось обновить название подразделения");
        }
    }
}