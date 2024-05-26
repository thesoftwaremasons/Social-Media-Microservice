using CQRS.Core.Commands;

namespace Post.Cmd.Api.Command
{
    public class EditCommentCommand:BaseCommand
    {
        public string CommentId { get;set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
    }
}
