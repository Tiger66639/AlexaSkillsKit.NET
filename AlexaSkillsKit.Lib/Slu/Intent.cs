// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Intent.cs">
//   
// </copyright>
// <summary>
//   The intent.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit .Slu
    {
        /// <summary>
        ///     The intent.
        /// </summary>
        public class Intent
            {
                /// <summary>
                ///     Gets or sets the name.
                /// </summary>
                public virtual string Name { get ; set ; }

                /// <summary>
                ///     Gets or sets the slots.
                /// </summary>
                public virtual System . Collections . Generic . Dictionary<string, Slot> Slots { get ; set ; }

                /// <summary>
                /// </summary>
                /// <param name="json">
                /// </param>
                /// <returns>
                /// The <see cref="Intent"/> .
                /// </returns>
                public static Intent FromJson( Newtonsoft . Json . Linq . JObject json )
                    {
                        var slots = new System . Collections . Generic . Dictionary<string, Slot>() ;
                        if (json["slots"] != null
                            && json . Value<Newtonsoft . Json . Linq . JObject>("slots") . HasValues)
                        {
                            foreach (var slot in json . Value<Newtonsoft . Json . Linq . JObject>("slots") . Children())
                            {
                                slots . Add(
                                    Newtonsoft . Json . Linq . Extensions . Value<Newtonsoft . Json . Linq . JProperty>(
                                        slot) . Name,
                                    Slot . FromJson(
                                        Newtonsoft . Json . Linq . Extensions
                                                . Value<Newtonsoft . Json . Linq . JProperty>(slot) . Value as
                                            Newtonsoft . Json . Linq . JObject)) ;
                            }
                        }

                        return new Intent { Name = json . Value<string>("name"), Slots = slots } ;
                    }
            }
    }
