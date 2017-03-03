using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Data;

namespace BandTracker
{
    public class Venue
    {
        private int _id;
        private string _name;

        public Venue(string Name, int Id = 0)
        {
            _id = Id;
            _name = Name;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string newName)
        {
            _name = newName;
        }

        public override bool Equals(System.Object otherVenue)
        {
            if (!(otherVenue is Venue))
            {
                return false;
            }
            else
            {
                Venue newVenue = (Venue) otherVenue;
                bool idEquality = this.GetId() == newVenue.GetId();
                bool nameEquality = this.GetName() == newVenue.GetName();
                return (idEquality && nameEquality);
            }
        }

        public static List<Venue> GetAll()
        {
            List<Venue> allVenues = new List<Venue>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM venues;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int venueId = rdr.GetInt32(0);
                string venueName = rdr.GetString(1);
                Venue newVenue = new Venue(venueName, venueId);
                allVenues.Add(newVenue);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return allVenues;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO venues (name) OUTPUT INSERTED.id VALUES (@VenueName);", conn);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@VenueName";
            nameParameter.Value = this.GetName();
            cmd.Parameters.Add(nameParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }

        public static Venue Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM venues WHERE id = @VenueId;", conn);
            SqlParameter venueIdParameter = new SqlParameter();
            venueIdParameter.ParameterName = "@VenueId";
            venueIdParameter.Value = id.ToString();
            cmd.Parameters.Add(venueIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundVenueId = 0;
            string foundVenueDescription = null;

            while(rdr.Read())
            {
                foundVenueId = rdr.GetInt32(0);
                foundVenueDescription = rdr.GetString(1);
            }
            Venue foundVenue = new Venue(foundVenueDescription, foundVenueId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundVenue;
        }

        public void AddBand(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues(band_id, venue_id) VALUES (@BandId, @VenueId);", conn);
            cmd.Parameters.Add(new SqlParameter("@VenueId", this.GetId()));
            cmd.Parameters.Add(new SqlParameter("@BandId", id));

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Band> GetBands()
        {
            List<Band> allBands = new List<Band>{};
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT bands.* FROM bands_venues JOIN bands ON (bands.id = bands_venues.band_id) JOIN venues ON (venues.id = bands_venues.venue_id) WHERE venues.id = @VenueId;", conn);

            cmd.Parameters.Add(new SqlParameter("@VenueId", this.GetId()));

            SqlDataReader rdr= cmd.ExecuteReader();

            while(rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                string foundName = rdr.GetString(1);
                Band foundBand = new Band(foundName, foundId);
                allBands.Add(foundBand);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return allBands;
        }



        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM venues;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
