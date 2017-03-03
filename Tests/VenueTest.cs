using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BandTracker
{
    public class VenueTest : IDisposable
    {
        public VenueTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void GetAll_VenuesEmptyAtFirst_true()
        {
            int result = Venue.GetAll().Count;
            Assert.Equal(0, result);
        }

        [Fact]
        public void Equals_VenuesReturnEqualIfSameName_true()
        {
            //Arrange, Act
            Venue firstVenue = new Venue("The Moore");
            Venue secondVenue = new Venue("The Moore");

            //Assert
            Assert.Equal(firstVenue, secondVenue);
        }

        [Fact]
        public void Save_SavesVenueToDatabase_true()
        {
            //Arrange
            Venue testVenue = new Venue("Tractor Tavern");
            testVenue.Save();

            //Act
            List<Venue> result = Venue.GetAll();
            List<Venue> testList = new List<Venue>{testVenue};

            //Assert
            Assert.Equal(testList, result);
        }

        [Fact]
        public void Save_AssignsIdToVenueObject_true()
        {
            //Arrange
            Venue testVenue = new Venue("The Gorge");
            testVenue.Save();

            //Act
            Venue savedVenue = Venue.GetAll()[0];

            int result = savedVenue.GetId();
            int testId = testVenue.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

        [Fact]
        public void Find_FindsVenueInDatabase()
        {
            //Arrange
            Venue testVenue = new Venue("The Showbox");
            testVenue.Save();

            //Act
            Venue foundVenue = Venue.Find(testVenue.GetId());

            //Assert
            Assert.Equal(testVenue, foundVenue);
        }


        public void Dispose()
        {
            // Venue.DeleteAll();
            Venue.DeleteAll();
        }
    }
}
