
using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Api.Command
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourceHandler<PostAggregate> _eventSourceHandler;

        public CommandHandler(IEventSourceHandler<PostAggregate> eventSourceHandler)
        {
            _eventSourceHandler = eventSourceHandler;
        } 

        public async Task HandlerAsync(NewPostCommand command)
        {
            var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(EditMessageCommand command)
        {
            var aggregate= await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.EditMessage(command.Message);
            await _eventSourceHandler.SaveAsync(aggregate);

        }

        public async Task HandlerAsync(LikePostCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.LikePost();
            await _eventSourceHandler.SaveAsync(aggregate);
        }

       public async Task HandlerAsync(AddCommentCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.AddComment(command.Comment,command.Author);
            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(EditCommentCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.EditComment(command.Id,command.Comment,command.UserName);
            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(RemoveCommentCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.RemoveComment(command.CommentId, command.UserName);
            await _eventSourceHandler.SaveAsync(aggregate); 
        }

        public async Task HandlerAsync(DeletePostCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.DeletePost(command.UserName);
            await _eventSourceHandler.SaveAsync(aggregate);
        }
    }
}
