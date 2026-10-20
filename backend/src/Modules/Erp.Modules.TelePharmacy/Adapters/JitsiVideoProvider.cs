using System.Security.Cryptography;
using System.Text;
using Erp.SharedKernel.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Erp.Modules.TelePharmacy.Adapters;

public sealed class JitsiVideoProvider : IVideoProvider
{
    private readonly IConfiguration _configuration;

    public JitsiVideoProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string ServerUrl
    {
        get
        {
            var url = _configuration["Video:ServerUrl"];
            if (string.IsNullOrWhiteSpace(url))
                throw new InvalidOperationException("Video:ServerUrl configuration is missing.");
            return url.TrimEnd('/');
        }
    }

    public Task<string> CreateRoomAsync(string roomName, CancellationToken ct = default)
    {
        var sanitized = SanitizeRoomName(roomName);
        return Task.FromResult(sanitized);
    }

    public Task<string> GenerateTokenAsync(string roomName, string participantIdentity, CancellationToken ct = default)
    {
        var appId = _configuration["Jitsi:AppId"] ?? "healthcare-erp";
        var secret = _configuration["Jitsi:AppSecret"] ?? string.Empty;

        if (!string.IsNullOrEmpty(secret))
        {
            var payload = $"{appId}:{roomName}:{participantIdentity}:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return Task.FromResult(Convert.ToBase64String(hash));
        }

        var anonymousToken = $"jitsi_anon_{Guid.NewGuid():N}";
        return Task.FromResult(anonymousToken);
    }

    public Task EndRoomAsync(string roomName, CancellationToken ct = default)
        => Task.CompletedTask;

    private static string SanitizeRoomName(string roomName)
    {
        var builder = new StringBuilder();
        foreach (var c in roomName)
        {
            if (char.IsLetterOrDigit(c) || c == '-' || c == '_')
                builder.Append(c);
        }
        return builder.Length > 0 ? builder.ToString() : Guid.NewGuid().ToString("N");
    }
}
