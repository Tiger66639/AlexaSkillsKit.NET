﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="ValuesController.cs">
//
// </copyright>
// <summary>
//   The values controller.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace Sample.Controllers
{
    /// <summary>
    ///     The values controller.
    /// </summary>
    [System.Web.Http.Authorize]
    public class ValuesController : System.Web.Http.ApiController
    {
        // DELETE api/values/5
        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public void Delete(int id)
        { }

        // GET api/values
        /// <summary>
        ///     The get.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerable" /> .
        /// </returns>
        public System.Collections.Generic.IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="System.String"/> .
        /// </returns>
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public void Post([System.Web.Http.FromBody] string value)
        { }

        // PUT api/values/5
        /// <summary>
        /// The put.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void Put(int id, [System.Web.Http.FromBody] string value)
        { }
    }
}