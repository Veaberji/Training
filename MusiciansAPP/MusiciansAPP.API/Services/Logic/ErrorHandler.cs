using Microsoft.Extensions.Logging;
using MusiciansAPP.API.Controllers;
using MusiciansAPP.API.Services.Interfaces;
using System;

namespace MusiciansAPP.API.Services.Logic;

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
        _logger.LogError(
            $"Exception in {method}, {error.Message}");
    }
}