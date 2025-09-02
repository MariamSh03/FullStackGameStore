using System.Linq.Expressions;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Services;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;
using AutoMapper;
using Moq;

namespace AdminPanel.Tests.Bll.Tests;

public class PublisherServiceTests
{
    private readonly Mock<IGenericRepository<PublisherEntity>> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly PublisherService _service;

    public PublisherServiceTests()
    {
        _mockRepository = new Mock<IGenericRepository<PublisherEntity>>();
        _mockMapper = new Mock<IMapper>();
        _service = new PublisherService(_mockRepository.Object, _mockMapper.Object);
    }

    // AddPublisherAsync Tests
    [Fact]
    public async Task AddPublisherAsync_WithValidPublisher_AddsSuccessfully()
    {
        // Arrange
        var publisherDto = new PublisherDto
        {
            CompanyName = "Test Publisher",
            Description = "Test Description",
            HomePage = "https://testpublisher.com",
        };

        var publisherEntity = new PublisherEntity
        {
            Id = Guid.NewGuid(),
            CompanyName = "Test Publisher",
            Description = "Test Description",
            HomePage = "https://testpublisher.com",
        };

        _mockRepository.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<PublisherEntity, bool>>>()))
                      .ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map<PublisherEntity>(publisherDto)).Returns(publisherEntity);
        _mockRepository.Setup(r => r.AddAsync(publisherEntity)).Returns(Task.CompletedTask);

        // Act
        await _service.AddPublisherAsync(publisherDto);

        // Assert
        _mockRepository.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<PublisherEntity, bool>>>()), Times.Once);
        _mockMapper.Verify(m => m.Map<PublisherEntity>(publisherDto), Times.Once);
        _mockRepository.Verify(r => r.AddAsync(publisherEntity), Times.Once);
    }

    [Fact]
    public async Task AddPublisherAsync_WithNullPublisher_ThrowsArgumentNullException()
    {
        // Arrange
        PublisherDto nullPublisher = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddPublisherAsync(nullPublisher!));
    }

    [Fact]
    public async Task AddPublisherAsync_WithEmptyCompanyName_ThrowsArgumentException()
    {
        // Arrange
        var publisherDto = new PublisherDto
        {
            CompanyName = string.Empty,
            Description = "Test Description",
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.AddPublisherAsync(publisherDto));
    }

    [Fact]
    public async Task AddPublisherAsync_WithNullCompanyName_ThrowsArgumentException()
    {
        // Arrange
        var publisherDto = new PublisherDto
        {
            CompanyName = null,
            Description = "Test Description",
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.AddPublisherAsync(publisherDto));
    }

    [Fact]
    public async Task AddPublisherAsync_WithExistingCompanyName_ThrowsInvalidOperationException()
    {
        // Arrange
        var publisherDto = new PublisherDto
        {
            CompanyName = "Existing Publisher",
            Description = "Test Description",
        };

        _mockRepository.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<PublisherEntity, bool>>>()))
                      .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddPublisherAsync(publisherDto));
        Assert.Contains("already exists", exception.Message);
    }

    // UpdatePublisherAsync Tests
    [Fact]
    public async Task UpdatePublisherAsync_WithValidData_UpdatesSuccessfully()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisherDto = new PublisherDto
        {
            CompanyName = "Updated Publisher",
            Description = "Updated Description",
        };

        var publisherEntity = new PublisherEntity
        {
            Id = publisherId,
            CompanyName = "Updated Publisher",
            Description = "Updated Description",
        };

        _mockMapper.Setup(m => m.Map<PublisherEntity>(publisherDto)).Returns(publisherEntity);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<PublisherEntity>())).Returns(Task.CompletedTask);

        // Act
        await _service.UpdatePublisherAsync(publisherId, publisherDto);

        // Assert
        Assert.Equal(publisherId, publisherEntity.Id); // Verify ID is retained
        _mockMapper.Verify(m => m.Map<PublisherEntity>(publisherDto), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(publisherEntity), Times.Once);
    }

    [Fact]
    public async Task UpdatePublisherAsync_WithNullPublisher_ThrowsArgumentNullException()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        PublisherDto nullPublisher = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdatePublisherAsync(publisherId, nullPublisher!));
    }

    [Fact]
    public async Task UpdatePublisherAsync_WithInvalidData_ThrowsArgumentException()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisherDto = new PublisherDto
        {
            CompanyName = string.Empty, // Invalid empty name
            Description = "Test Description",
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdatePublisherAsync(publisherId, publisherDto));
    }

    // DeletePublisherAsync Tests
    [Fact]
    public async Task DeletePublisherAsync_WithExistingPublisher_DeletesSuccessfully()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisher = new PublisherEntity
        {
            Id = publisherId,
            CompanyName = "Test Publisher",
        };

        _mockRepository.Setup(r => r.GetByIdAsync(publisherId)).ReturnsAsync(publisher);
        _mockRepository.Setup(r => r.DeleteAsync(publisher)).Returns(Task.CompletedTask);

        // Act
        await _service.DeletePublisherAsync(publisherId);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(publisherId), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(publisher), Times.Once);
    }

    [Fact]
    public async Task DeletePublisherAsync_WithNonExistentPublisher_ThrowsKeyNotFoundException()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(publisherId)).ReturnsAsync((PublisherEntity)null!);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeletePublisherAsync(publisherId));
    }

    // GetAllPublishersAsync Tests
    [Fact]
    public async Task GetAllPublishersAsync_WhenPublishersExist_ReturnsAllPublishers()
    {
        // Arrange
        var publishers = new List<PublisherEntity>
        {
            new() { Id = Guid.NewGuid(), CompanyName = "Publisher 1" },
            new() { Id = Guid.NewGuid(), CompanyName = "Publisher 2" },
            new() { Id = Guid.NewGuid(), CompanyName = "Publisher 3" },
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(publishers);

        // Act
        var result = await _service.GetAllPublishersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Contains(result, p => p.CompanyName == "Publisher 1");
        Assert.Contains(result, p => p.CompanyName == "Publisher 2");
        Assert.Contains(result, p => p.CompanyName == "Publisher 3");
    }

    [Fact]
    public async Task GetAllPublishersAsync_WhenNoPublishers_ReturnsEmptyList()
    {
        // Arrange
        var emptyList = new List<PublisherEntity>();
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(emptyList);

        // Act
        var result = await _service.GetAllPublishersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    // GetPublisherByCompanyAsync Tests
    [Fact]
    public async Task GetPublisherByCompanyAsync_WithExistingCompany_ReturnsPublisher()
    {
        // Arrange
        var companyName = "Test Company";
        var publisher = new PublisherEntity
        {
            Id = Guid.NewGuid(),
            CompanyName = companyName,
            Description = "Test Description",
        };

        _mockRepository.Setup(r => r.SingleFind(It.IsAny<Expression<Func<PublisherEntity, bool>>>()))
                      .ReturnsAsync(publisher);

        // Act
        var result = await _service.GetPublisherByCompanyAsync(companyName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(companyName, result.CompanyName);
        _mockRepository.Verify(r => r.SingleFind(It.IsAny<Expression<Func<PublisherEntity, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetPublisherByCompanyAsync_WithNonExistentCompany_ReturnsNull()
    {
        // Arrange
        var companyName = "Non-existent Company";
        _mockRepository.Setup(r => r.SingleFind(It.IsAny<Expression<Func<PublisherEntity, bool>>>()))
                      .ReturnsAsync((PublisherEntity)null!);

        // Act
        var result = await _service.GetPublisherByCompanyAsync(companyName);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPublisherByCompanyAsync_WithEmptyCompanyName_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.SingleFind(It.IsAny<Expression<Func<PublisherEntity, bool>>>()))
                      .ReturnsAsync((PublisherEntity)null!);

        // Act
        var result = await _service.GetPublisherByCompanyAsync(string.Empty);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPublisherByCompanyAsync_WithNullCompanyName_CallsRepository()
    {
        // Arrange
        _mockRepository.Setup(r => r.SingleFind(It.IsAny<Expression<Func<PublisherEntity, bool>>>()))
                      .ReturnsAsync((PublisherEntity)null!);

        // Act
        var result = await _service.GetPublisherByCompanyAsync(null!);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.SingleFind(It.IsAny<Expression<Func<PublisherEntity, bool>>>()), Times.Once);
    }

    // GetPublisherByIdAsync Tests
    [Fact]
    public async Task GetPublisherByIdAsync_WithExistingId_ReturnsPublisher()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisher = new PublisherEntity
        {
            Id = publisherId,
            CompanyName = "Test Publisher",
        };

        _mockRepository.Setup(r => r.GetByIdAsync(publisherId)).ReturnsAsync(publisher);

        // Act
        var result = await _service.GetPublisherByIdAsync(publisherId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(publisherId, result.Id);
        Assert.Equal("Test Publisher", result.CompanyName);
    }

    [Fact]
    public async Task GetPublisherByIdAsync_WithNonExistentId_ReturnsNull()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(publisherId)).ReturnsAsync((PublisherEntity)null!);

        // Act
        var result = await _service.GetPublisherByIdAsync(publisherId);

        // Assert
        Assert.Null(result);
    }

    // Edge Cases and Integration Tests
    [Fact]
    public async Task AddPublisherAsync_WithSpecialCharactersInName_HandlesCorrectly()
    {
        // Arrange
        var publisherDto = new PublisherDto
        {
            CompanyName = "Test & Co. Ümlaut Publisher™",
            Description = "Publisher with special characters",
        };

        var publisherEntity = new PublisherEntity
        {
            Id = Guid.NewGuid(),
            CompanyName = publisherDto.CompanyName,
            Description = publisherDto.Description,
        };

        _mockRepository.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<PublisherEntity, bool>>>()))
                      .ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map<PublisherEntity>(publisherDto)).Returns(publisherEntity);
        _mockRepository.Setup(r => r.AddAsync(publisherEntity)).Returns(Task.CompletedTask);

        // Act
        await _service.AddPublisherAsync(publisherDto);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(It.Is<PublisherEntity>(p => p.CompanyName == publisherDto.CompanyName)), Times.Once);
    }

    [Fact]
    public async Task UpdatePublisherAsync_EnsuresIdIsPreserved()
    {
        // Arrange
        var originalId = Guid.NewGuid();
        var publisherDto = new PublisherDto
        {
            CompanyName = "Updated Name",
            Description = "Updated Description",
        };

        var mappedEntity = new PublisherEntity
        {
            Id = Guid.NewGuid(), // Different ID from mapper
            CompanyName = publisherDto.CompanyName,
            Description = publisherDto.Description,
        };

        _mockMapper.Setup(m => m.Map<PublisherEntity>(publisherDto)).Returns(mappedEntity);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<PublisherEntity>())).Returns(Task.CompletedTask);

        // Act
        await _service.UpdatePublisherAsync(originalId, publisherDto);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.Is<PublisherEntity>(p => p.Id == originalId)), Times.Once);
    }
}