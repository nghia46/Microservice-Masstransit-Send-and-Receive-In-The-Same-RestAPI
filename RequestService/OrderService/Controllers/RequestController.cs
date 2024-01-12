using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RequestController : ControllerBase
{
    private readonly IRequestClient<RequestMessage> _requestClient;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public RequestController(IRequestClient<RequestMessage> requestClient, ISendEndpointProvider sendEndpointProvider)
    {
        _requestClient = requestClient;
        _sendEndpointProvider = sendEndpointProvider;
    }

    [HttpGet]
    public async Task<IActionResult> MakeRequest(string message)
    {
        try
        {
            var request = new RequestMessage { Message = message };

            // Send the request to Dịch vụ B
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:request-queue"));
            await endpoint.Send(request);

            // Wait for the response
            var response = await _requestClient.GetResponse<ResponseMessage>(request);

            // Extract the response data
            var responseData = response.Message;

            // Return the response data to the API caller
            return Ok(responseData.Response);
        }
        catch (RequestTimeoutException)
        {
            // Handle timeout if needed
            return BadRequest("The request timed out.");
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}
