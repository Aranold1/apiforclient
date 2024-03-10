using WebApiForGptBlazor.Models;

namespace WebApiForGptBlazor.Mapper
{
    public class MessageWithoutChat
    {

        public string? Body { get; set; }

        public bool? Isgptauthor { get; set; }
    }
}
