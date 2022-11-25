using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SharedObjects.Dtos.Metadata
{
    public class MetadataContainerSet
    {
        [Required]
        public string Name { get; set; }

        public string UriBase { get; set; }

        public MetadataContainer[] Types { get; set; }

        public MetadataContainerSet Resource(string culture, IEnumerable<IResource> resources)
            => new()
            {
                Name = Name,
                UriBase = UriBase,
                Types = Types.Select(t => t.Resource(Name, culture, resources)).ToArray()
            };
    }
}