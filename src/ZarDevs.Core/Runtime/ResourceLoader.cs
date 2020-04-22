using System.IO;
using System.Reflection;

namespace ZarDevs.Core.Runtime
{
    public interface IResourceLoader
    {
        string GetTextEmbeddedResource(string resourceName, Assembly assembly);
    }

    public class ResourceLoader : IResourceLoader
    {
        #region Methods

        public string GetTextEmbeddedResource(string resourceName, Assembly assembly)
        {
            resourceName = FormatResourceName(assembly, resourceName);
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    return null;
                }

                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private string FormatResourceName(Assembly assembly, string resourceName)
        {
            return assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                                                               .Replace("\\", ".")
                                                               .Replace("/", ".");
        }

        #endregion Methods
    }
}