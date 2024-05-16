using MassTransit;
using Model;

namespace InventoryService
{
    public class RequestConsumer : IConsumer<RequestMessage>
    {
        public async Task Consume(ConsumeContext<RequestMessage> context)
        {
            // Xử lý yêu cầu và tạo response
            var result = context.Message.Message;

            // Gửi response vào hàng đợi phản hồi
            await context.RespondAsync(new ResponseMessage { Response = "This Message been execute in consume service: "+result});
        }
    }
}
