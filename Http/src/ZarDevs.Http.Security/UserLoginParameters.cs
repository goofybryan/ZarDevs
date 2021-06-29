using System.Collections.Generic;

namespace ZarDevs.Http.Security
{
    /// <summary>
    /// User login parameters that is used when logging in using the classing username and password combination.
    /// </summary>
    public class UserLoginParameters
    {
        #region Properties

        /// <summary>
        /// Get a dictional dictionary parameters that can be used. Example is adding the keyword
        /// 'Scopes' and the an array of valid scopes for the user.
        /// </summary>
        public Dictionary<string, object> Additional { get; set; } = new Dictionary<string, object>();

        ///<summary>
        /// Get or set a value that the process has been cancelled.
        /// </summary>
        public bool Cancelled { get; set; }

        /// <summary>
        /// Get or set the password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Get or set the username
        /// </summary>
        public string Username { get; set; }

        #endregion Properties
    }
}