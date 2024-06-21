using MessageSystem.API.BL;
using MessageSystem.API.DAL;
using Microsoft.AspNetCore.Mvc;

namespace MessageSystem.API.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // GET: api/messages/{countryCode}?date=yyyy-MM-ddTHH:mm:ss
        [HttpGet("{countryCode}")]
        public async Task<ActionResult<Message>> GetMessageForCountryAndDate(string countryCode, [FromQuery] DateTime date)
        {
            try
            {
                var message = await _messageService.GetMessageForCountryAndDateAsync(countryCode, date);
                if (message == null)
                {
                    return NotFound();
                }
                return Ok(message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/messages
        [HttpGet]
        public async Task<ActionResult<List<Message>>> GetAllMessages()
        {
            try
            {
                var messages = await _messageService.GetAllMessagesAsync();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/messages
        [HttpPost]
        public async Task<ActionResult<Message>> AddMessage([FromBody] Message message)
        {
            try
            {
                await _messageService.AddMessageAsync(message);
                return CreatedAtAction(nameof(GetMessageForCountryAndDate), new { countryCode = message.CountryCode, date = message.StartDate }, message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/messages/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] Message message)
        {
            try
            {
                if (id != message.Id)
                {
                    return BadRequest("Id mismatch between route parameter and message object");
                }

                await _messageService.UpdateMessageAsync(message);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/messages/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                await _messageService.DeleteMessageAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
