namespace IdentityServerAspNetIdentity.Ldap
{
    public class LdapAuthenticationOptions
    {
        /// <summary>
        /// Gets or sets the LDAP server host name.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the TCP port on which the LDAP server is running. 
        /// </summary>
        public int Port { get; set; } = 389;

        /// <summary>
        /// Gets or sets the domain name to use as distinguished name in conjuction with the username
        /// </summary>
        public string Domain { get; set; }
    }

    public interface IUserLdapStore<TUser>
       where TUser : class
    {
        /// <summary>
        /// When implemented in a derived class, gets the DN that should be used to attempt an LDAP bind for validatio of a user's password.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetDistinguishedNameAsync(TUser user);
    }
}
