using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Blog.BotApplicationSample.Vision;

namespace Blog.BotApplicationSample
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        public IVisionConnector visionConnector;


        public MessagesController()  {
            visionConnector = new VisionConnector();
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {

            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            if (activity.Type == ActivityTypes.Message)
            {

                var analysisResult =await visionConnector.AnalizeImage(activity);
                Activity reply = activity.CreateReply("Did you upload an image? I'm more of a visual person. " +
                                      "Try sending me an image or an image url"); //default reply

                if (analysisResult != null)
                {
                    string imageCaption = analysisResult.Description.Captions[0].Text;
                    reply = activity.CreateReply("I think it's " + imageCaption);
                }
                await connector.Conversations.ReplyToActivityAsync(reply);
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {

            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}