﻿using Microsoft.AspNetCore.Identity;

namespace FantasyChas_Backend.Models
{
    public class ActiveStory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BasePrompt { get; set; }

        public virtual ICollection<Character> Characters { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
    }
}
