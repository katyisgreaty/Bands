using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Data;

namespace BandTracker
{
    public class Band
    {
        private int _id;
        private string _name;

        public Band(string Name, int Id = 0)
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

        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

        public override bool Equals(System.Object otherBand)
        {
            if (!(otherBand is Band))
            {
                return false;
            }
            else
            {
                Band newBand = (Band) otherBand;
                bool idEquality = this.GetId() == newBand.GetId();
                bool nameEquality = this.GetName() == newBand.GetName();
                return (idEquality && nameEquality);
            }
        }

        public static List<Band> GetAll()
        {
            List<Band> allBands = new List<Band>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM bands;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int bandId = rdr.GetInt32(0);
                string bandName = rdr.GetString(1);
                Band newBand = new Band(bandName, bandId);
                allBands.Add(newBand);
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

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO bands (name) OUTPUT INSERTED.id VALUES (@BandName);", conn);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@BandName";
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

        public static Band Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE id = @BandId;", conn);
            SqlParameter bandIdParameter = new SqlParameter();
            bandIdParameter.ParameterName = "@BandId";
            bandIdParameter.Value = id.ToString();
            cmd.Parameters.Add(bandIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundBandId = 0;
            string foundBandDescription = null;

            while(rdr.Read())
            {
                foundBandId = rdr.GetInt32(0);
                foundBandDescription = rdr.GetString(1);
            }
            Band foundBand = new Band(foundBandDescription, foundBandId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundBand;
        }

        public void AddVenue(Venue newVenue)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (band_id, venue_id) VALUES (@BandId, @VenueId);", conn);
            SqlParameter bandIdParameter = new SqlParameter();
            bandIdParameter.ParameterName = "@BandId";
            bandIdParameter.Value = this.GetId();
            cmd.Parameters.Add(bandIdParameter);

            SqlParameter venueIdParameter = new SqlParameter();
            venueIdParameter.ParameterName = "@VenueId";
            venueIdParameter.Value = newVenue.GetId();
            cmd.Parameters.Add(venueIdParameter);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Venue> GetVenues()
        {
            List<Venue> allVenues = new List<Venue>{};
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT venues.* FROM bands_venues JOIN venues ON (venues.id = bands_venues.venue_id) JOIN bands ON (bands.id = bands_venues.band_id) WHERE bands.id = @BandId;", conn);

            cmd.Parameters.Add(new SqlParameter("@BandId", this.GetId()));

            SqlDataReader rdr= cmd.ExecuteReader();

            while(rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                string foundName = rdr.GetString(1);
                Venue foundVenue = new Venue(foundName, foundId);
                allVenues.Add(foundVenue);
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

        public void Update(string newName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE bands SET name = @NewName OUTPUT INSERTED.* WHERE id = @BandId;", conn);

            SqlParameter newNameParameter = new SqlParameter();
            newNameParameter.ParameterName = "@NewName";
            newNameParameter.Value = newName;
            cmd.Parameters.Add(newNameParameter);

            SqlParameter bandIdParameter = new SqlParameter();
            bandIdParameter.ParameterName = "@BandId";
            bandIdParameter.Value = this.GetId();
            cmd.Parameters.Add(bandIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._name = rdr.GetString(1);
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

        public static void Delete(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM bands WHERE id=@BandId;", conn);
            cmd.Parameters.Add(new SqlParameter("@BandId", id));

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }


        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM bands;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
