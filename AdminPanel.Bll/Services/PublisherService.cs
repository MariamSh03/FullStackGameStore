using System.ComponentModel.DataAnnotations;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;
using AutoMapper;

namespace AdminPanel.Bll.Services;
public class PublisherService : IPublisherService
{
    private readonly IGenericRepository<PublisherEntity> _publisherRepository;
    private readonly IMapper _mapper;

    public PublisherService(IGenericRepository<PublisherEntity> publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task AddPublisherAsync(PublisherDto publisher)
    {
        ValidatePublisherDto(publisher);

        bool exists = await _publisherRepository.ExistsAsync(p => p.CompanyName == publisher.CompanyName);
        if (exists)
        {
            throw new InvalidOperationException($"Publisher '{publisher.CompanyName}' already exists.");
        }

        var publisherEntity = _mapper.Map<PublisherEntity>(publisher);
        await _publisherRepository.AddAsync(publisherEntity);
    }

    public async Task UpdatePublisherAsync(Guid id, PublisherDto publisher)
    {
        ValidatePublisherDto(publisher);

        PublisherEntity publisherEntity = _mapper.Map<PublisherEntity>(publisher);
        publisherEntity.Id = id; // Ensure correct ID is retained
        await _publisherRepository.UpdateAsync(publisherEntity);
    }

    public async Task DeletePublisherAsync(Guid id)
    {
        var publisher = await _publisherRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Publisher not found");
        await _publisherRepository.DeleteAsync(publisher);
    }

    public async Task<IEnumerable<PublisherEntity>> GetAllPublishersAsync()
    {
        return await _publisherRepository.GetAllAsync();
    }

    public async Task<PublisherEntity> GetPublisherByCompanyAsync(string companyName)
    {
        return await _publisherRepository.SingleFind(p => p.CompanyName == companyName);
    }

    public async Task<PublisherEntity> GetPublisherByIdAsync(Guid id)
    {
        return await _publisherRepository.GetByIdAsync(id);
    }

    private static void ValidatePublisherDto(PublisherDto publisher)
    {
        var context = new ValidationContext(publisher);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(publisher, context, results, true))
        {
            string errors = string.Join("; ", results.Select(r => r.ErrorMessage));
            throw new ArgumentException($"Invalid publisher data: {errors}");
        }
    }
}
