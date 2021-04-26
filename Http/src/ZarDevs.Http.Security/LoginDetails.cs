using System.Collections.Generic;

namespace ZarDevs.Http.Security
{
    public class LoginDetails
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Dictionary<string, object> Additional { get; set; } = new Dictionary<string, object>();
    }
}
