﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;

namespace FantasyChas_Backend.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Profession { get; set; }
        public string Species { get; set; }
        public int Level { get; set; }
        public int HealthPoints { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Constitution { get; set; }
        public int Charisma { get; set; }
        public string Backstory { get; set; }
        public bool Favourite { get; set; }
        public string? ImageURL { get; set; }

        public virtual IdentityUser User { get; set; }
        public virtual ActiveStory? ActiveStory { get; set; }
        public virtual SavedStory? SavedStory { get; set; }
    }
}
