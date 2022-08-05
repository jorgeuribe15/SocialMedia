using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class EditCommnetCommand : BaseCommand
    {
        public Guid CommnetId { get; set; }
        public string Commnent { get; set; }
        public string Username { get; set; }
    }
}
