// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="ApiDescriptionExtensions.cs">
//   
// </copyright>
// <summary>
//   The api description extensions.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Sample . Areas .HelpPage
    {
        /// <summary>
        ///     The api description extensions.
        /// </summary>
        public static class ApiDescriptionExtensions
            {
                /// <summary>
                /// Generates an URI-friendly ID for the
                ///     <see cref="System.Web.Http.Description.ApiDescription"/> . E.g.
                ///     "Get-Values-id_name" instead of "GetValues/{id}?name={name}"
                /// </summary>
                /// <param name="description">
                /// The <see cref="System.Web.Http.Description.ApiDescription"/> .
                /// </param>
                /// <returns>
                /// The ID as a string.
                /// </returns>
                public static string GetFriendlyId(
                    this System . Web . Http . Description . ApiDescription description )
                    {
                        string path = description . RelativePath ;
                        string[] urlParts = path . Split('?') ;
                        string localPath = urlParts[0] ;
                        string queryKeyString = null ;
                        if (urlParts . Length > 1)
                        {
                            string query = urlParts[1] ;
                            string[] queryKeys = System . Web . HttpUtility . ParseQueryString(query) . AllKeys ;
                            queryKeyString = string . Join("_", queryKeys) ;
                        }

                        System . Text . StringBuilder friendlyPath = new System . Text . StringBuilder() ;
                        friendlyPath . AppendFormat(
                            "{0}-{1}",
                            description . HttpMethod . Method,
                            localPath . Replace("/", "-") . Replace("{", string . Empty) . Replace("}", string . Empty)) ;
                        if (queryKeyString != null)
                        {
                            friendlyPath . AppendFormat("_{0}", queryKeyString . Replace('.', '-')) ;
                        }

                        return friendlyPath . ToString() ;
                    }
            }
    }
