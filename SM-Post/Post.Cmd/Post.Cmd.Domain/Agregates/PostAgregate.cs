using CQRS.Core.Domain;
using Post.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Domain.Agregates
{
    public class PostAgregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new ();

        public bool Active
        {
            get => _active; set => _active = value; 
        }

        public PostAgregate()
        {

        }

        #region PostAgregate

        public PostAgregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now,
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;
        }

        #endregion PostAgregate


        #region EditMessage

        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you can not edit message of an inactive post !");
            }
            if(string.IsNullOrWhiteSpace(message) )
            {
                throw new InvalidOperationException($"The value of {nameof(message)} can not be empty or null. Please provide a valid {nameof(message)}");
            }

            RaiseEvent(new MessageUpdatedEvent
            {
                Id = _id,
                Message = message
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;
        }

        #endregion EditMessage


        #region LikePost

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("you cannot Like an inactive post!");
            }

            RaiseEvent(new PostLikeEvent
            {
                Id= _id,
            });
        }

        public void Apply(PostLikeEvent @event)
        {
            _id = @event.Id;
        }

        #endregion LikePost


        #region AddCommnet

        public void AddCommnet(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you can not add a comment to an inactive post !");
            }

            if(string.IsNullOrEmpty(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty, plase provide a valid {nameof(comment)}");
            }

            RaiseEvent(new CommentAddEvent
            {
                Id=_id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username,
                CommentDate = DateTime.Now,
            });
        }

        public void Apply(CommentAddEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        #endregion AddCommnet


        #region EditComment

        public void EditComment(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you can not edit a comment to an inactive post !");
            }

            if(!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user");
            }

            RaiseEvent(new CommentUpdatedEvent
            {
                Id = Id,
                CommentId = commentId,
                Comment = comment,
                Username =username,
                EditDate = DateTime.Now,
            });
        }
        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        #endregion EditComment


        #region RemoveComment

        public void RemoveComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("you can not remove a comment to an inactive post !");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a comment that was made by another user");
            }

            RaiseEvent(new CommentRemovedEvent
            {
                Id = _id,
                CommentId= commentId,
            });
        }

        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        #endregion RemoveComment


        #region DeletePost

        public void DeletePost(string username)
        {

            if (!_active)
            {
                throw new InvalidOperationException("The post has  already been deleted !");
            }

            if(!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to delete a psot that was maded by different user");
            }

            RaiseEvent(new PostRemovedEvent
            {
                Id = _id
            });
        }

        public void Apply(PostRemovedEvent @event)
        {
            _id = Id;
            _active = false;
        }

        #endregion DeletePost
    }
}
