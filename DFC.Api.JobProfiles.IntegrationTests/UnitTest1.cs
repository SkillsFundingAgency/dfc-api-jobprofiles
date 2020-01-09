using Microsoft.Azure.ServiceBus;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.IntegrationTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task ServiceBusProofOfConcept()
        {
            TopicClient topicClient = new TopicClient("Endpoint=sb://dfc-sit-app-sharedresources-ns.servicebus.windows.net/;SharedAccessKeyName=monitor-cms-messages;SharedAccessKey=OoT1m7qwR9hwGOh/mNtQq/TVpETuKxjp/s5OE/8ZT1w=;", "cms-messages");
            string test = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.GetDirectories("Resource")[0].GetFiles()[0].FullName;
            byte[] messageBody = null;

            using (StreamReader streamReader = new StreamReader(test))
            {
                string contents = streamReader.ReadToEnd();
                messageBody = Encoding.Unicode.GetBytes(contents);
            }

            Message message = new Message();
            message.Body = messageBody;
            message.ContentType = "application/json";
            message.CorrelationId = Guid.NewGuid().ToString();
            message.Label = "Created by James";
            message.MessageId = Guid.NewGuid().ToString();

            message.UserProperties.Add("Id", "685e06e6-a42d-4bec-b660-c206399c534j");
            message.UserProperties.Add("ActionType", "Publish");
            message.UserProperties.Add("CType", "JobProfile");
            message.UserProperties.Add("Diagnostic-Id", Guid.NewGuid().ToString());

            await topicClient.SendAsync(message);
        }
    }
}