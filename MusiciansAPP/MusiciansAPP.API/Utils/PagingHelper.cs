using MusiciansAPP.API.Configs;

namespace MusiciansAPP.API.Utils
{
    public class PagingHelper
    {
        public static int GetCorrectPageSize(int pageSize)
        {
            int maxSize = AppConfigs.MaxPageSize;
            int size = pageSize;
            if (pageSize > maxSize)
            {
                size = maxSize;
            }
            else if (pageSize < 1)
            {
                size = AppConfigs.DefaultPageSize;
            }

            return size;
        }
    }
}
