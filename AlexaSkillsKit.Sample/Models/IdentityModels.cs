// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="IdentityModels.cs">
//   
// </copyright>
// <summary>
//   The application user.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Sample .Models
    {
// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
        /// <summary>
        ///     The application user.
        /// </summary>
        public class ApplicationUser : Microsoft . AspNet . Identity . EntityFramework . IdentityUser
            {
                /// <summary>
                /// The generate user identity async.
                /// </summary>
                /// <param name="manager">
                /// The manager.
                /// </param>
                /// <param name="authenticationType">
                /// The authentication type.
                /// </param>
                /// <returns>
                /// The <see cref="System.Threading.Tasks.Task"/> .
                /// </returns>
                public async System . Threading . Tasks . Task<System . Security . Claims . ClaimsIdentity>
                    GenerateUserIdentityAsync(
                        Microsoft . AspNet . Identity . UserManager<ApplicationUser> manager,
                        string authenticationType )
                    {
                        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
                        var userIdentity = await manager . CreateIdentityAsync(this, authenticationType) ;

                        // Add custom user claims here
                        return userIdentity ;
                    }
            }

        /// <summary>
        ///     The application db context.
        /// </summary>
        public class ApplicationDbContext :
            Microsoft . AspNet . Identity . EntityFramework . IdentityDbContext<ApplicationUser>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class. 
                ///     Initializes a new instance of the
                ///     <see cref="ApplicationDbContext"/> class. Initializes a new
                ///     instance of the <see cref="ApplicationDbContext"/> class.
                /// </summary>
                public ApplicationDbContext( )
                    : base("DefaultConnection", false)
                    {}

                /// <summary>
                ///     The create.
                /// </summary>
                /// <returns>
                ///     The <see cref="ApplicationDbContext" /> .
                /// </returns>
                public static ApplicationDbContext Create( )
                    {
                        return new ApplicationDbContext() ;
                    }
            }
    }
