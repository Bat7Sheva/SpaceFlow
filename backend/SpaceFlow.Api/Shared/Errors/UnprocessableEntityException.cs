namespace SpaceFlow.Api.Shared.Errors;

public sealed class UnprocessableEntityException(string message) : Exception(message);