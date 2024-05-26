using CQRS.Core.Commands;

namespace Post.Cmd.Api.Command
{
    public class DeletePostCommand:BaseCommand
    {
        public string UserName { get; set; }
    }
}
