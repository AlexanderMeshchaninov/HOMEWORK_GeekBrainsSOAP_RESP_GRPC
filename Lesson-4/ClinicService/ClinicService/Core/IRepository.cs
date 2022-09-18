namespace ClinicService.Core;

public interface IRepository<TItem, TId>
{
    TId? Add(TItem item);
    void Update(TItem item);
    void Delete(TItem item);
    void Delete(TId id);
    IList<TItem> GetAll();
    TItem? GetById(TId id);
}