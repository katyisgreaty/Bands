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

        [Fact]
        public void AddBand_AddsBandToVenue()
        {
            //Arrange
            Venue testVenue = new Venue("Tractor Tavern");
            testVenue.Save();

            Band testBand = new Band("The Flaming Lips");
            testBand.Save();

            Band testBand2 = new Band("The Taverns");
            testBand2.Save();

            //Act
            testVenue.AddBand(testBand);
            testVenue.AddBand(testBand2);

            List<Band> result = testVenue.GetBands();
            List<Band> testList = new List<Band>{testBand, testBand2};

            //Assert
            Assert.Equal(testList, result);
        }

        [Fact]
        public void Delete_DeleteBandFromVenue()
        {
            Venue newVenue = new Venue("Tractor Tavern");
            newVenue.Save();

            Venue.Delete(newVenue.GetId());

            Assert.Equal(0, Venue.GetAll().Count);
        }

        [Fact]
        public void Update_UpdateInDatabase_true()
        {
            //Arrange
            string name = "The Crocodile";

            Venue testVenue = new Venue(name);
            testVenue.Save();
            string newName = "The Triple Door";

            //Act
            testVenue.Update(newName);
            Venue result = Venue.GetAll()[0];

            //Assert
            Assert.Equal(testVenue, result);
            // Assert.Equal(newName, result.GetName());
        }

        [Fact]
        public void Delete_RemoveVenueFromDatabase_Deleted()
        {
            Venue newVenue = new Venue("High Dive");
            newVenue.Save();

            Venue.Delete(newVenue.GetId());

            Assert.Equal(0, Venue.GetAll().Count);
        }

        [Fact]
        public void DeleteAll_DeleteAllVenues_Deleted()
        {
            Venue venue1 = new Venue("The Moore");
            venue1.Save();
            Venue venue2 = new Venue("The Crocodile");
            venue2.Save();

            Venue.DeleteAll();

            Assert.Equal(0, Venue.GetAll().Count);
        }




        public void Dispose()
        {
            // Venue.DeleteAll();
            Venue.DeleteAll();
        }
    }
}
