using System.Collections.Generic;
using System.Linq;

namespace SharedObjects.Extensions
{
    public static class IEnumerableResourceExtensions
    {
        public static IResource WithName(this IEnumerable<IResource> resourceSection, string name)
            => resourceSection.FirstOrDefault(r => r.Name.ToLower() == name.ToLower());

        public static IEnumerable<IResource> SectionForCulture(this IEnumerable<IResource> potentials, string key, string culture)
        {
            List<IResource> results = new();

            potentials.Where(r => r.Key.ToLower() == key.ToLower())
                .GroupBy(r => r.Name.ToLower())
                .ForEach(resGroup => results.Add(resGroup.GetClosestCulturalMatch(culture)));

            return results.Where(r => r != null);
        }

        public static IResource ForNameAndCulture(this IEnumerable<IResource> potentials, string name, string culture)
        {
            List<IResource> results = new();

            potentials.Where(r => r.Name.ToLower() == name.ToLower())
                .GroupBy(r => r.Name.ToLower())
                .ForEach(resGroup => results.Add(resGroup.GetClosestCulturalMatch(culture)));

            return results.FirstOrDefault(r => r != null);
        }

        public static IResource GetClosestCulturalMatch(this IEnumerable<IResource> potentials, string culture)
        {
            IResource result = null;
            List<string> cultureParts = culture.ToLower().Split('-').ToList();
            int take = cultureParts.Count;
            string resultCulture = "";

            // scan the cultural heirarchy in the code
            while (result == null && resultCulture != null)
            {
                resultCulture = string.Join("-", cultureParts.Take(take));
                result = potentials?.FirstOrDefault(r => r.Culture?.ToLowerInvariant() == resultCulture?.ToLowerInvariant());
                take--;
                if (take == 0)
                {
                    resultCulture = null;
                }
            }

            if (result == null)
            {
                result = potentials?.FirstOrDefault(r => r.Culture?.ToLowerInvariant() == string.Empty || r.Culture == null);
            }

            return result;
        }

        public static IResource ForKeyAndCulture(this IEnumerable<IResource> potentials, string cacheKey, string culture)
        {
            string[] keyAndName = cacheKey.Split('|');
            string
                key = keyAndName.Length > 1 ? keyAndName.First().ToLowerInvariant() : "default",
                name = keyAndName.Last().ToLowerInvariant();

            return potentials.Where(r => r.Key.ToLowerInvariant() == key && r.Name.ToLowerInvariant() == name).GetClosestCulturalMatch(culture);
        }
    }
}