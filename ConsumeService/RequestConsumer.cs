using MassTransit;
using Model;

namespace InventoryService
{
    public class RequestConsumer : IConsumer<RequestMessage>
    {
        public async Task Consume(ConsumeContext<RequestMessage> context)
        {
            // Xử lý yêu cầu và tạo response
            var result = $"Processed: {context.Message.Message}";

            // Gửi response vào hàng đợi phản hồi
            await context.RespondAsync(new ResponseMessage { Response = "Bạn Quá Đẹp Trai"+result});
        }
    }
}
