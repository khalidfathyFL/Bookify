using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings
{
    public sealed class Booking :Entity
  {
    private Booking(Guid id, DateRange duration, Money cleaningFee, Money amentiesUpCharge, Money totalPrice, BookingStatus status, DateTime createdOnUtc, Guid userId, Guid appartmentId) : base(id)
    {
      Duration = duration;
      CleaningFee = cleaningFee;
      AmentiesUpCharge = amentiesUpCharge;
      TotalPrice = totalPrice;
      Status = status;
      CreatedOnUtc = createdOnUtc;
      UserId = userId;
      AppartmentId = appartmentId;
    }

    public Guid UserId { get; private set; }
    public Guid AppartmentId { get; private set; }
    public DateRange Duration { get; private set; }

    public Money CleaningFee { get; private set; }
    public Money AmentiesUpCharge { get; private set; }
    public Money TotalPrice { get; private set; }

    public BookingStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ConfirmedOnUtc { get; private set; }
    public DateTime? RejectedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }
    public DateTime? CanceledOnUtc { get; private set; }


    public static Booking Reserve(Apartment apartment, Guid userId, DateRange dateRange, DateTime utcNow,PricingService pricingService)
    {
      var pricingDetails = pricingService.CalculatePrice(apartment, dateRange);
      var booking = new Booking(Guid.NewGuid(), dateRange, pricingDetails.CleaningFee, pricingDetails.AmenitiesUpCharge,
        pricingDetails.TotalPrice, BookingStatus.Reserved, utcNow, userId, apartment.Id);
      booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id) );
      return booking;
    }
  }
}
