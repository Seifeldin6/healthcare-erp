namespace Erp.SharedKernel.Interfaces;

public interface IVideoProvider
{
    string ServerUrl { get; }
    Task<string> CreateRoomAsync(string roomName, CancellationToken ct = default);
    Task<string> GenerateTokenAsync(string roomName, string participantIdentity, CancellationToken ct = default);
    Task EndRoomAsync(string roomName, CancellationToken ct = default);
}
