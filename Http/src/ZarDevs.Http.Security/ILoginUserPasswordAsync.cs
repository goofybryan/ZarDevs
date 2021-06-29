using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Interface that is used to get the user name and password used during the login process.
    /// </summary>
    public interface ILoginUserPasswordAsync
    {
        /// <summary>
        /// Retrieve the login details and return the <see cref="UserLoginParameters"/>
        /// </summary>
        Task<UserLoginParameters> RetrieveAsync();
    }
}
