using Microsoft.Bot.Connector;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Threading.Tasks;

namespace Blog.BotApplicationSample.Vision
{
    public interface IVisionConnector
    {

        Task<AnalysisResult> AnalizeImage(Activity activity);

        VisualFeature[] getVisualFeatures();

        VisionServiceClient getVisionClient();
    }
}
