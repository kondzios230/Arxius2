
namespace Arxius.CrossLayer.PCL
{
    public static class CrossLayerData
    {
        public static bool IsOffline { get; set; }
        public static string BaseAddress
        {
            get
            {
                if (IsOffline)
                    return "http://192.168.0.16:8002{0}";
                return "https://zapisy.ii.uni.wroc.pl/{0}";
            }
        }
        public static string BaseAddressShort
        {
            get
            {
                if (IsOffline)
                    return "http://192.168.0.16:8002";
                return "https://zapisy.ii.uni.wroc.pl/";
            }
        }
    }
}
