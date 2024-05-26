using CQRS.Core.Domain;
using Post.Common.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;

        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
        }
        public PostAggregate()
        {
            
        }

        public PostAggregate(Guid id, string author, string message)
        {

            var newPostCreatedEvent = new PostCreatedEvent
            {
                Id = id, Author = author, Message = message, DatePosted = DateTime.Now
            };
            RaiseEvent(newPostCreatedEvent);
        }
        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author= @event.Author;

        }
        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit an inactive message");
            }
            if(!string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"{message} is not valid.");
            }
            var messageUpdatedEvent = new MessageUpdatedEvent
            {
                Id=_id,
                Message=message
            };
            RaiseEvent(messageUpdatedEvent);
        }
        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;
           
        }
        public void LikePost()
        {
            if(!_active) {
                throw new InvalidOperationException("You cannot like an inactive post");
            }
            var postLikedEvent = new PostLikedEvent
            {
                Id = _id
            };
            RaiseEvent(postLikedEvent);
        }
        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }

        public void AddComment(string comment,string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot add a comment for an inactive post");
            }
            if(string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The value of ${nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}");
            }
            var commentEvent = new CommentAddedEvent
            {
                Id = _id,
                Comment = comment,
                UserName = username,
                CommentId = Guid.NewGuid(),
                CommentDate=DateTime.Now                
              
            };
            RaiseEvent(commentEvent);
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.UserName));
        }

        public void EditComment(Guid commentId,string comment,string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Yo cannot edit a comment of an inactive post");
            }
            if (!_comments[commentId].Item2.Equals(userName, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user!");
            }
            var updateComment = new CommentUpdatedEvent
            {
                Id = _id,
                Comment = comment,
                CommentId=commentId,
                UserName = userName,
                EditDate=DateTime.Now
            };
            RaiseEvent(updateComment);
        }
        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId]=new Tuple<string, string>(@event.Comment,@event.UserName);
        }
        public void RemoveComment(Guid commentId,string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot delete a comment of an inactive post");
            }
            if (!_comments[commentId].Item2.Equals(userName, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to delete a comment that was made by another user!");
            }
            var removeComment = new CommentRemovedEvent
            {
                CommentId = commentId,
                Id = _id,

            };
            RaiseEvent(removeComment);
        }
        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }
        public void DeletePost(string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("The post have already been removed");
            }
            if (!_author.Equals(userName, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a post that was made by someone else");

            }

            RaiseEvent(new PostRemovedEvent
            {
                Id= _id,
            });
        }
        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }
    }
   
}
