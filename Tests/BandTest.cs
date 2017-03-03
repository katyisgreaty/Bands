using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BandTracker
{
    public class BandTest : IDisposable
    {
        public BandTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void GetAll_BandsEmptyAtFirst_true()
        {
            int result = Band.GetAll().Count;
            Assert.Equal(0, result);
        }

        [Fact]
        public void Equals_BandsReturnEqualIfSameName_true()
        {
            //Arrange, Act
            Band firstBand = new Band("The Lumineers");
            Band secondBand = new Band("The Lumineers");

            //Assert
            Assert.Equal(firstBand, secondBand);
        }

        [Fact]
        public void Save_SavesBandToDatabase_true()
        {
            //Arrange
            Band testBand = new Band("Bastille");
            testBand.Save();

            //Act
            List<Band> result = Band.GetAll();
            List<Band> testList = new List<Band>{testBand};

            //Assert
            Assert.Equal(testList, result);
        }

        [Fact]
        public void Save_AssignsIdToBandObject_true()
        {
            //Arrange
            Band testBand = new Band("Florence and the Machine");
            testBand.Save();

            //Act
            Band savedBand = Band.GetAll()[0];

            int result = savedBand.GetId();
            int testId = testBand.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

        [Fact]
        public void Find_FindsBandInDatabase()
        {
            //Arrange
            Band testBand = new Band("Sia");
            testBand.Save();

            //Act
            Band foundBand = Band.Find(testBand.GetId());

            //Assert
            Assert.Equal(testBand, foundBand);
        }

        [Fact]
        public void AddVenue_AddsVenueToBand()
        {
            //Arrange
            Band testBand = new Band("Huey Lewis and the News");
            testBand.Save();

            Venue testVenue = new Venue("The Moore");
            testVenue.Save();

            Venue testVenue2 = new Venue("Tractor Tavern");
            testVenue2.Save();

            //Act
            testBand.AddVenue(testVenue);
            testBand.AddVenue(testVenue2);

            List<Venue> result = testBand.GetVenues();
            List<Venue> testList = new List<Venue>{testVenue, testVenue2};

            //Assert
            Assert.Equal(testList, result);
        }

        [Fact]
        public void Delete_RemoveBandFromDatabase_Deleted()
        {
            Band newBand = new Band("Adele");
            newBand.Save();

            Band.Delete(newBand.GetId());

            Assert.Equal(0, Band.GetAll().Count);
        }
        [Fact]
        public void DeleteAll_DeleteAllBands_Deleted()
        {
            Band band1 = new Band("Lucas Graham");
            band1.Save();
            Band band2 = new Band("Ray LaMontagne");
            band2.Save();

            Band.DeleteAll();

            Assert.Equal(0, Band.GetAll().Count);
        }


        public void Dispose()
        {
            // Venue.DeleteAll();
            Band.DeleteAll();
        }
    }
}
