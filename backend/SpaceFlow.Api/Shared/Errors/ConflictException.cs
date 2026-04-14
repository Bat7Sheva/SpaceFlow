namespace SpaceFlow.Api.Shared.Errors;

public sealed class ConflictException(string message) : Exception(message);