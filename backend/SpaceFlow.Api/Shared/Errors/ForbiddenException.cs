namespace SpaceFlow.Api.Shared.Errors;

public sealed class ForbiddenException(string message) : Exception(message);