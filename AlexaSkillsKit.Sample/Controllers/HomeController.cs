// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="HomeController.cs">
//   
// </copyright>
// <summary>
//   The home controller.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Sample .Controllers
    {
        /// <summary>
        ///     The home controller.
        /// </summary>
        public class HomeController : System . Web . Mvc . Controller
            {
                /// <summary>
                ///     The index.
                /// </summary>
                /// <returns>
                ///     The <see cref="ActionResult" /> .
                /// </returns>
                public System . Web . Mvc . ActionResult Index( )
                    {
                        this . ViewBag . Title = "Home Page" ;

                        return this . View() ;
                    }
            }
    }
