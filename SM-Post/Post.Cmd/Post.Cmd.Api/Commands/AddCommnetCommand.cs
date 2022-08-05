using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class AddCommnetCommand : BaseCommand
    {
        public string Commnet { get; set; }
        public string Username { get; set; }
    }
}
