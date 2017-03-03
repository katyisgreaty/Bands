using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using System.Linq;

namespace BandTracker
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                return View["index.cshtml"];
            };

            Get["/bands"] = _ => {
                List<Band> AllBands = Band.GetAll();
                return View["bands.cshtml", AllBands];
            };

            Get["/venues"] = _ => {
                List<Venue> AllVenues = Venue.GetAll();
                return View["venues.cshtml", AllVenues];
            };

            Get["/band/new"] = _ => {
                return View["band_form.cshtml"];
            };

            Post["/band/new"] = _ => {
                Band newBand = new Band(Request.Form["band-name"]);
                newBand.Save();
                return View["success.cshtml"];
            };

            Get["/venue/new"] = _ => {
                return View["venue_form.cshtml"];
            };

            Post["/venue/new"] = _ => {
                Venue newVenue = new Venue(Request.Form["venue-name"]);
                newVenue.Save();
                return View["success.cshtml"];
            };

            Post["/bands/delete_all"] = _ => {
                Band.DeleteAll();
                return View["cleared.cshtml"];
            };

            Post["/venues/delete_all"] = _ => {
                Venue.DeleteAll();
                return View["cleared.cshtml"];
            };

            Get["bands/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>();
                Band SelectedBand = Band.Find(parameters.id);
                List<Venue> BandVenues = SelectedBand.GetVenues();
                List<Venue> AllVenues = Venue.GetAll();
                model.Add("band", SelectedBand);
                model.Add("bandVenues", BandVenues);
                model.Add("allVenues", AllVenues);
                return View["band.cshtml", model];
            };

            Get["/venues/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>();
                Venue SelectedVenue = Venue.Find(parameters.id);
                List<Band> VenueBands = SelectedVenue.GetBands();
                List<Band> AllBands = Band.GetAll();
                model.Add("venue", SelectedVenue);
                model.Add("venueBands", VenueBands);
                model.Add("allBands", AllBands);
                return View["venue.cshtml", model];
            };

            Post["/band/add_venue"] = _ => {
                Venue venue = Venue.Find(Request.Form["venue-id"]);
                Band band = Band.Find(Request.Form["band-id"]);
                band.AddVenue(venue);
                return View["success.cshtml"];
            };

            Post["/venue/add_band"] = _ => {
                Band band = Band.Find(Request.Form["band-id"]);
                Venue venue = Venue.Find(Request.Form["venue-id"]);
                venue.AddBand(band);
                return View["success.cshtml"];
            };

            Get["bands/{id}/edit"] = parameters => {
                Band SelectedBand = Band.Find(parameters.id);
                return View["band_update_form.cshtml", SelectedBand];
            };

            Patch["bands/{id}/updated"] = parameters => {
                Band SelectedBand = Band.Find(parameters.id);
                SelectedBand.Update(Request.Form["new-band-name"]);
                return View["success.cshtml", SelectedBand];
            };

            Get["venues/{id}/edit"] = parameters => {
                Venue SelectedVenue = Venue.Find(parameters.id);
                return View["venue_update_form.cshtml", SelectedVenue];
            };

            Patch["venues/{id}/updated"] = parameters => {
                Venue SelectedVenue = Venue.Find(parameters.id);
                SelectedVenue.Update(Request.Form["new-venue-name"]);
                return View["success.cshtml", SelectedVenue];
            };

            Delete["/band/delete/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>{
                    {"Bands", Band.GetAll()},
                    {"Venues", Venue.GetAll()},
                };
                Band.Delete(parameters.id);

                return View["success.cshtml", model];
            };

            Delete["/venue/delete/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>{
                    {"Bands", Band.GetAll()},
                    {"Venues", Venue.GetAll()},
                };
                Venue.Delete(parameters.id);

                return View["success.cshtml", model];
            };
        }
    }
}
