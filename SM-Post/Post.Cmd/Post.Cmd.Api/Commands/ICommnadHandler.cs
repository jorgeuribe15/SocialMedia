namespace Post.Cmd.Api.Commands
{
    public interface ICommnadHandler
    {
        Task HandleAsync(NewPostCommand command);
        Task HandleAsync(EditMessageCommand command);
        Task HandleAsync(LikePostCommand command);
        Task HandleAsync(AddCommnetCommand command);
        Task HandleAsync(EditCommnetCommand command);
        Task HandleAsync(RemoveCommentCommand command);
        Task HandleAsync(DeletePostCommand command);
    }
}
