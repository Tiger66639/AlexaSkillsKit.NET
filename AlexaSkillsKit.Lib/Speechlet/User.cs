// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="User.cs">
//   
// </copyright>
// <summary>
//   The user.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit .Speechlet
    {
        /// <summary>
        ///     The user.
        /// </summary>
        public class User
            {
                /// <summary>
                ///     Gets or sets the access token.
                /// </summary>
                public virtual string AccessToken { get ; set ; }

                /// <summary>
                ///     Gets or sets the id.
                /// </summary>
                public virtual string Id { get ; set ; }

                /// <summary>
                /// </summary>
                /// <param name="json">
                /// </param>
                /// <returns>
                /// The <see cref="User"/> .
                /// </returns>
                public static User FromJson( Newtonsoft . Json . Linq . JObject json )
                    {
                        return new User
                                   {
                                       Id = json . Value<string>("userId"),
                                       AccessToken = json . Value<string>("accessToken")
                                   } ;
                    }
            }
    }
