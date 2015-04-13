using MessageModule.Interfaces;
using MessageModule.Interfaces.General;
using MessageModule.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Agent.AWSMail
{
    public class AWSMail : IMessageAgent
    {
        public Interfaces.Models.MessageModel SendMessage
            (Interfaces.Models.MessageModel MessageToSend)
        {
            //get config
            Dictionary<string, string> oMsjConfig = MessageConfig.AgentConfig[MessageToSend.Agent];

            //get destination address
            Amazon.SimpleEmail.Model.Destination oDestination = new Amazon.SimpleEmail.Model.Destination();

            if (MessageToSend.QueueProcessInfo != null &&
                MessageToSend.QueueProcessInfo.Count > 0 &&
                MessageToSend.QueueProcessInfo.Any(qpi => qpi.Item2 == Constants.C_Agent_To))
            {
                oDestination.ToAddresses = MessageToSend.QueueProcessInfo.
                    Where(qpi => qpi.Item2 == Constants.C_Agent_To).
                    Select(qpi => qpi.Item3).
                    Distinct().
                    ToList();
            }
            else
            {
                throw new Exception("El mensaje no posee destinatarios.");
            }

            //get message info
            Amazon.SimpleEmail.Model.Message oMessage = new Amazon.SimpleEmail.Model.Message();

            //get message body and subject
            string oMsjBody = oMsjConfig[Constants.C_Agent_AWS_MessageBody];
            string oMsjSubject = oMsjConfig[Constants.C_Agent_AWS_Subject];

            if (MessageToSend.QueueProcessInfo != null && MessageToSend.QueueProcessInfo.Count > 0)
            {
                MessageToSend.QueueProcessInfo.All(MsjParams =>
                {
                    oMsjBody = oMsjBody.Replace("{" + MsjParams.Item2 + "}", MsjParams.Item3);
                    oMsjSubject = oMsjSubject.Replace("{" + MsjParams.Item2 + "}", MsjParams.Item3);
                    return true;
                });
            }
            oMessage.Body = new Amazon.SimpleEmail.Model.Body();
            oMessage.Body.Html = new Amazon.SimpleEmail.Model.Content(oMsjBody);

            //get message subject
            oMessage.Subject = new Amazon.SimpleEmail.Model.Content(oMsjSubject);

            //get email request object
            Amazon.SimpleEmail.Model.SendEmailRequest oEmailRequest = new Amazon.SimpleEmail.Model.SendEmailRequest
                (oMsjConfig[Constants.C_Agent_AWS_From],
                oDestination,
                oMessage);

            //get aws ses client
            Amazon.SimpleEmail.AmazonSimpleEmailServiceClient oClient = new Amazon.SimpleEmail.AmazonSimpleEmailServiceClient
                (oMsjConfig[Constants.C_Agent_AWS_AccessKeyId],
                oMsjConfig[Constants.C_Agent_AWS_SecretAccessKey],
                Amazon.RegionEndpoint.GetBySystemName(oMsjConfig[Constants.C_Agent_AWS_RegionEndpoint]));

            //send email
            oClient.SendEmail(oEmailRequest);

            //set email status ok
            MessageToSend.IsSuccess = true;
            MessageToSend.ProcessResult = oMsjBody;

            return MessageToSend;
        }
    }
}
