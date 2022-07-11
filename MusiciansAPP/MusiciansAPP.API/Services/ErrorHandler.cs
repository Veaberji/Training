using System;
using Microsoft.Extensions.Logging;
using MusiciansAPP.API.Controllers;

namespace MusiciansAPP.API.Services;

public class ErrorHandler : IErrorHandler
{
    private readonly ILogger<ArtistsController> _logger;

    public ErrorHandler(ILogger<ArtistsController> logger)
    {
        _logger = logger;
    }

    public void HandleError(Exception error, string method)
    {
        LogError(error, method);
    }

    private void LogError(Exception error, string method)
    {
#pragma warning disable CA2254 // Template should be a static expression
        _logger.LogError($"Exception in {method}, {error.Message}");
#pragma warning restore CA2254 // Template should be a static expression
    }
}