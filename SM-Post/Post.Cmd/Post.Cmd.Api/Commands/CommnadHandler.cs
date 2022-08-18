using CQRS.Core.Handlers;
using MongoDB.Driver.Core.Operations;
using Post.Cmd.Domain.Agregates;

namespace Post.Cmd.Api.Commands
{
    public class CommnadHandler : ICommnadHandler
    {
        private readonly IEventSourcingHandler<PostAgregate> _eventSourcingHandler;

        public CommnadHandler(IEventSourcingHandler<PostAgregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async Task HandleAsync(NewPostCommand command)
        {
            var aggregate = new PostAgregate(command.Id, command.Author, command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditMessageCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditMessage(command.Message);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(LikePostCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.LikePost();

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(AddCommnetCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.AddCommnet(command.Commnet, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditCommnetCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditComment(command.CommentId, command.Commnent, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RemoveCommentCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.RemoveComment(command.CommentId, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeletePostCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.DeletePost(command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
    }
}
