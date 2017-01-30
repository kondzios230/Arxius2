
namespace Arxius.CrossLayer.PCL
{
    public static class CrossLayerData
    {
        public static bool IsOffline { get; set; }
        public static string OfflineIP { get; set; }
        public static string BaseAddress
        {
            get
            {
                if (IsOffline)
                    return OfflineIP+"{0}";
                return "https://zapisy.ii.uni.wroc.pl/{0}";
            }
        }
        public static string BaseAddressShort
        {
            get
            {
                if (IsOffline)
                    return OfflineIP;
                return "https://zapisy.ii.uni.wroc.pl/";
            }
        }
    }
}
