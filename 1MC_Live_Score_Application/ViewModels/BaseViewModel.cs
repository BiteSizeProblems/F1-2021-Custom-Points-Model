using _1MC_Live_Score_Application.Core;
using _1MC_Live_Score_Application.Core.UDP;

namespace _1MC_Live_Score_Application.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        protected UDPConnection UDPC = UDPConnection.GetInstance();
    }
}
