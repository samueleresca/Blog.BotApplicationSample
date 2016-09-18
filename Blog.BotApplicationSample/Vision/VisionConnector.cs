using Microsoft.Bot.Connector;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Blog.BotApplicationSample.Vision
{
    public class VisionConnector : IVisionConnector
    {

       
        private   VisualFeature[] visualFeatures = new VisualFeature[] {
                                        VisualFeature.Adult, //recognize adult content
                                        VisualFeature.Categories, //recognize image features
                                        VisualFeature.Description //generate image caption
                                        };

        private VisionServiceClient visionClient = new VisionServiceClient(Properties.Settings.Default.VisionAPIKEY);



        public async Task<AnalysisResult> AnalizeImage(Activity activity)  {
            //If the user uploaded an image, read it, and send it to the Vision API
            if (activity.Attachments.Any() && activity.Attachments.First().ContentType.Contains("image"))
            {
                //stores image url (parsed from attachment or message)
                string uploadedImageUrl = activity.Attachments.First().ContentUrl; ;
                uploadedImageUrl = HttpUtility.UrlDecode(uploadedImageUrl);


                //Create a WebRequest to get the file
                var fileReq = HttpWebRequest.Create(uploadedImageUrl);
                //Create a response for this request
                var fileResp = (HttpWebResponse)fileReq.GetResponse();

                using (Stream imageFileStream = fileResp.GetResponseStream())
                {
                    try
                    {
                        return  await this.visionClient.AnalyzeImageAsync(imageFileStream, visualFeatures);
                    }
                    catch (Exception e)
                    {
                           return null; //on error, reset analysis result to null
                    }
                }
            }
            //Else, if the user did not upload an image, determine if the message contains a url, and send it to the Vision API
            else
            {
                try
                {
                   return await visionClient.AnalyzeImageAsync(activity.Text, visualFeatures);
                }
                catch (Exception e)
                {
                   return null; //on error, reset analysis result to null
                }
            }
        }



        public VisualFeature[] getVisualFeatures() {
            return visualFeatures;
        }

        public VisionServiceClient getVisionClient()
        {
            return visionClient;
        }
    }
}