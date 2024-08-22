namespace Bookify.Domain.Apartments;

public interface IApartmentInterface
{
  Task<Apartment?> GetByIdAsync(Guid id,CancellationToken cancellationToken = default);
}