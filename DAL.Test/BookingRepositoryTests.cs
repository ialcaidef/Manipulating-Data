using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System;
using DAL.Repository;
using DAL.Models;
using DAL.Database;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task AddTwoBookingsTest()
        {
            Booking fristBooking;
            Booking secondBooking;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                HotelBookingRepository repository = new HotelBookingRepository();
                fristBooking = await repository.Add(1, 1, DateTime.Now.AddDays(6), 4);
                secondBooking = await repository.Add(1, 2, DateTime.Now.AddDays(8), 3);
                scope.Complete();
            }

            using (MyDbContext context = new MyDbContext())
            {
                int bookingsCounter = context.Bookings.Where(booking => booking.BookingId == fristBooking.BookingId ||
                                                                        booking.BookingId == secondBooking.BookingId).ToList().Count;
                Assert.AreEqual(2, bookingsCounter);
            }
        }
    }
}
