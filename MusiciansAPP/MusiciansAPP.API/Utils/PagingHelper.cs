using MusiciansAPP.API.Configs;

namespace MusiciansAPP.API.Utils
{
    public class PagingHelper
    {
        public static int GetCorrectPageSize(int pageSize)
        {
            int maxSize = AppConfigs.MaxPageSize;
            if (pageSize > maxSize)
            {
                return maxSize;
            }
            if (pageSize < 1)
            {
                return AppConfigs.DefaultPageSize;
            }

            return pageSize;
        }
    }
}
