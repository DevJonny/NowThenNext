namespace NowThenNext.Services;

/// <summary>
/// Exception thrown when localStorage quota is exceeded
/// </summary>
public class StorageQuotaExceededException : Exception
{
    public StorageQuotaExceededException(string message) : base(message) { }
    public StorageQuotaExceededException(string message, Exception innerException) : base(message, innerException) { }
}
