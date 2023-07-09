﻿namespace ChatAppAPI.Entities
{
    public class Connection
    {
        public string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
        public Guid UserId { get; set; }
    }
}
