using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Entity;
using AdminPanel.Web.DtoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Web.Controllers;

[ApiController]
[Route("publishers")]
public class PublisherController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    public PublisherController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    // Add a new publisher
    [HttpPost]
    public async Task<IActionResult> AddPublisher([FromBody] CreatePublisherRequest request)
    {
        if (request == null)
        {
            return BadRequest("Invalid publisher data.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _publisherService.AddPublisherAsync(request.Publisher);
            return Ok("Publisher added successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // Optional: distinguish known validation errors
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Get all publishers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PublisherEntity>>> GetAllPublishers()
    {
        var publishers = await _publisherService.GetAllPublishersAsync();
        return Ok(publishers);
    }

    [HttpGet("{companyName}")]
    public async Task<ActionResult<PublisherEntity>> GetPublisherByCompanyName(string companyName)
    {
        var publisher = await _publisherService.GetPublisherByCompanyAsync(companyName);
        return publisher;
    }

    // Delete publisher by ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePublisher(Guid id)
    {
        try
        {
            await _publisherService.DeletePublisherAsync(id);
            return Ok("Publisher deleted successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // Update publisher by id
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePublisher(Guid id, [FromBody] PublisherDto publisherDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (publisherDto == null)
        {
            return BadRequest("Invalid publisher data.");
        }

        await _publisherService.UpdatePublisherAsync(id, publisherDto);
        return Ok("Publisher updated successfully.");
    }
}
