// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Startup.cs">
//   
// </copyright>
// <summary>
//   The startup.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
[assembly : Microsoft . Owin . OwinStartup( typeof (Sample . Startup) )]

namespace Sample
    {
        /// <summary>
        ///     The startup.
        /// </summary>
        public partial class Startup
            {
                /// <summary>
                /// The configuration.
                /// </summary>
                /// <param name="app">
                /// The app.
                /// </param>
                public void Configuration( Owin . IAppBuilder app )
                    {
                        this . ConfigureAuth(app) ;
                    }
            }
    }
