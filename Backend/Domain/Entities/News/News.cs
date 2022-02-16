using System;
using Domain.Entities.Account;

namespace Domain.Entities.News
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ImageSource { get; set; }

        public DateTime Published { get; set; }
        public DateTime Modified { get; set; }

        public int UserCreatorId { get; set; }
        public User UserCreator { get; set; }
    }
}
