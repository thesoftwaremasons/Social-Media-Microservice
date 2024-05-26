using CQRS.Core.Commands;

namespace Post.Cmd.Api.Command
{
    public class EditMessageCommand:BaseCommand
    {
        public string CommandId { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}
