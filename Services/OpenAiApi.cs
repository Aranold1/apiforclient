using System.Runtime.CompilerServices;
using OpenAI.Managers;
using OpenAI;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

public class OpenAiApi(string ApiKey)
{
    OpenAIService openAIService = new OpenAIService(new OpenAiOptions()
    {
        ApiKey = ApiKey
    });

    public async Task<string> GetAnswerAsync(string message)
    {
        var completionResult = await openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromUser(message),
            },
            Model = "gpt-3.5-turbo-1106",

        });
        if (completionResult.Successful)
        {
            return completionResult.Choices.First().Message.Content ?? throw new Exception("Successful response is null wtf");
        }
        else
        {
            return completionResult.Error?.Message ?? throw new Exception("Error message is null");
        }
    }
}