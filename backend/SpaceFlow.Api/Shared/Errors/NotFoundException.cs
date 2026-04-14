namespace SpaceFlow.Api.Shared.Errors;

public sealed class NotFoundException(string message) : Exception(message);