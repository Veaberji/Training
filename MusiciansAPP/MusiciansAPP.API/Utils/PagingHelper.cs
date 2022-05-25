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
                pageSize = maxSize;
            }
            else if (pageSize < 1)
            {
                pageSize = AppConfigs.DefaultPageSize;
            }

            return pageSize;
        }
    }
}
