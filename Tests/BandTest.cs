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




        public void Dispose()
        {
            // Venue.DeleteAll();
            Band.DeleteAll();
        }
    }
}
