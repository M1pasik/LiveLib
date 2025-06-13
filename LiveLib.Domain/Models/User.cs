﻿namespace LiveLib.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public List<Review> Reviews { get; set; } = [];

        public List<Collection> Collections { get; set; } = [];

        public List<Collection> SubscribedCollections { get; set; } = [];

    }
}
