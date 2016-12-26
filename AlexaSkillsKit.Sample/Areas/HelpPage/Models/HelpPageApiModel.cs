// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="HelpPageApiModel.cs">
//   
// </copyright>
// <summary>
//   The model that represents an API displayed on the help page.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Sample . Areas . HelpPage .Models
    {
        /// <summary>
        ///     The model that represents an API displayed on the help page.
        /// </summary>
        public class HelpPageApiModel
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="HelpPageApiModel"/> class. 
                ///     <para>
                /// Initializes a new instance of the <see cref="HelpPageApiModel"/>
                ///         class. Initializes a new instance of the
                ///         <see cref="HelpPageApiModel"/>
                ///     </para>
                /// <para>
                /// class.
                /// </para>
                /// </summary>
                public HelpPageApiModel( )
                    {
                        this . UriParameters =
                            new System . Collections . ObjectModel . Collection
                                <ModelDescriptions . ParameterDescription>() ;
                        this . SampleRequests =
                            new System . Collections . Generic . Dictionary
                                <System . Net . Http . Headers . MediaTypeHeaderValue, object>() ;
                        this . SampleResponses =
                            new System . Collections . Generic . Dictionary
                                <System . Net . Http . Headers . MediaTypeHeaderValue, object>() ;
                        this . ErrorMessages = new System . Collections . ObjectModel . Collection<string>() ;
                    }

                /// <summary>
                ///     <para>
                ///         Gets or sets the
                ///         <see cref="Sample.Areas.HelpPage.Models.HelpPageApiModel.ApiDescription" />
                ///     </para>
                ///     <para>that describes the API.</para>
                /// </summary>
                public System . Web . Http . Description . ApiDescription ApiDescription { get ; set ; }

                /// <summary>
                ///     Gets the error messages associated with this model.
                /// </summary>
                public System . Collections . ObjectModel . Collection<string> ErrorMessages { get ; private set ; }

                /// <summary>
                ///     Gets the request body parameter descriptions.
                /// </summary>
                public System . Collections . Generic . IList<ModelDescriptions . ParameterDescription> RequestBodyParameters
                    {
                        get
                            {
                                return GetParameterDescriptions(this . RequestModelDescription) ;
                            }
                    }

                /// <summary>
                ///     Gets or sets the documentation for the request.
                /// </summary>
                public string RequestDocumentation { get ; set ; }

                /// <summary>
                ///     Gets or sets the
                ///     <see cref="Sample.Areas.HelpPage.ModelDescriptions.ModelDescription" />
                ///     that describes the request body.
                /// </summary>
                public ModelDescriptions . ModelDescription RequestModelDescription { get ; set ; }

                /// <summary>
                ///     Gets or sets the
                ///     <see cref="Sample.Areas.HelpPage.ModelDescriptions.ModelDescription" />
                ///     that describes the resource.
                /// </summary>
                public ModelDescriptions . ModelDescription ResourceDescription { get ; set ; }

                /// <summary>
                ///     Gets the resource property descriptions.
                /// </summary>
                public System . Collections . Generic . IList<ModelDescriptions . ParameterDescription> ResourceProperties
                    {
                        get
                            {
                                return GetParameterDescriptions(this . ResourceDescription) ;
                            }
                    }

                /// <summary>
                ///     Gets the sample requests associated with the API.
                /// </summary>
                public System . Collections . Generic . IDictionary<System . Net . Http . Headers . MediaTypeHeaderValue, object> SampleRequests { get ; private set ;
                    }

                /// <summary>
                ///     Gets the sample responses associated with the API.
                /// </summary>
                public System . Collections . Generic . IDictionary<System . Net . Http . Headers . MediaTypeHeaderValue, object> SampleResponses { get ; private set ;
                    }

                /// <summary>
                ///     Gets or sets the
                ///     <see cref="Sample.Areas.HelpPage.ModelDescriptions.ParameterDescription" />
                ///     collection that describes the URI parameters for the API.
                /// </summary>
                public System . Collections . ObjectModel . Collection<ModelDescriptions . ParameterDescription> UriParameters { get ; private set ; }

                /// <summary>
                /// The get parameter descriptions.
                /// </summary>
                /// <param name="modelDescription">
                /// The model description.
                /// </param>
                /// <returns>
                /// The <see cref="IList"/> .
                /// </returns>
                private static System . Collections . Generic . IList<ModelDescriptions . ParameterDescription> GetParameterDescriptions( ModelDescriptions . ModelDescription modelDescription )
                    {
                        ModelDescriptions . ComplexTypeModelDescription complexTypeModelDescription =
                            modelDescription as ModelDescriptions . ComplexTypeModelDescription ;
                        if (complexTypeModelDescription != null)
                        {
                            return complexTypeModelDescription . Properties ;
                        }

                        ModelDescriptions . CollectionModelDescription collectionModelDescription =
                            modelDescription as ModelDescriptions . CollectionModelDescription ;
                        if (collectionModelDescription != null)
                        {
                            complexTypeModelDescription =
                                collectionModelDescription . ElementDescription as
                                    ModelDescriptions . ComplexTypeModelDescription ;
                            if (complexTypeModelDescription != null)
                            {
                                return complexTypeModelDescription . Properties ;
                            }
                        }

                        return null ;
                    }
            }
    }
