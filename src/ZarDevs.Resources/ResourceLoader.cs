using System.IO;
using System.Reflection;

namespace ZarDevs.Resources
{
    public interface IResourceLoader
    {
        #region Methods

        string GetTextEmbeddedResource(string resourceName, Assembly assembly);

        #endregion Methods
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