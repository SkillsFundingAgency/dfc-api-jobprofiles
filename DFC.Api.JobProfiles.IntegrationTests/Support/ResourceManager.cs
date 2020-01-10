using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace DFC.Api.JobProfiles.IntegrationTests.Support
{
    internal class ResourceManager
    {
        private static string GetResourceContent(string resourceName)
        {
            DirectoryInfo resourcesDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.GetDirectories("Resource")[0];
            FileInfo[] files = resourcesDirectory.GetFiles();
            FileInfo selectedResource = null;

            for(int fileIndex = 0; fileIndex < files.Length; fileIndex++)
            {
                if(files[fileIndex].Name.ToLower().StartsWith(resourceName.ToLower()))
                {
                    selectedResource = files[fileIndex];
                    break;
                }
            }

            if(selectedResource.FullName == null)
            {
                throw new Exception($"No resource with the name {resourceName} was found");
            }

            using (StreamReader streamReader = new StreamReader(selectedResource.FullName))
            {
                return streamReader.ReadToEnd();
            }
        }

        internal static T GetResource<T>(string resourceName)
        {
            string content = GetResourceContent(resourceName);
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
