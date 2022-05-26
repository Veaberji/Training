using System;

namespace MusiciansAPP.API.Services.Interfaces
{
    public interface IErrorHandler
    {
        void HandleError(Exception error, string method);
    }
}
