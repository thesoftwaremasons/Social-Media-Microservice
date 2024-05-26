using CQRS.Core.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Common.Events
{
    public class CommentLikedEvent:BaseEvent
    {
        public CommentLikedEvent():base(nameof(CommentLikedEvent))
        {
                
        }
        public Guid CommentId { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
