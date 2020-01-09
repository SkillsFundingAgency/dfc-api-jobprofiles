using System;
using System.IO;
using System.Text;

namespace DFC.Api.JobProfiles.IntegrationTests.Support
{
    internal class ResourceManager
    {
        internal static byte[] GetResourceContent(string resourceName)
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
                string contents = streamReader.ReadToEnd();
                return Encoding.ASCII.GetBytes(contents);
            }
        }
    }
}
