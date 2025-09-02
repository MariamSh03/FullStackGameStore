using AdminPanel.Bll.DTOs;
using AdminPanel.Entity;

namespace AdminPanel.Bll.Interfaces;
public interface IPublisherService
{
    Task<IEnumerable<PublisherEntity>> GetAllPublishersAsync();

    Task<PublisherEntity> GetPublisherByIdAsync(Guid id);

    Task AddPublisherAsync(PublisherDto publisher);

    Task UpdatePublisherAsync(Guid id, PublisherDto publisher);

    Task DeletePublisherAsync(Guid id);

    Task<PublisherEntity> GetPublisherByCompanyAsync(string companyName);
}
