using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using Flight_Low_fare_Search.Models;
using System.Web.Caching;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Flight_Low_fare_Search.Controllers
{
    public class FlightController : Controller
    {

        public string GetAuthenticationToken()
        {
            string url = "https://test.api.amadeus.com/v1/security/oauth2/token";
            string client_id = "HKT11MwpvGp3XCWy1bOBw39o4k7bKop6";
            string client_secret = "NyVfgSFrpmFr8V56";
            //request token
            var restclient = new RestClient(url);
            RestRequest request = new RestRequest("request/oauth") { Method = Method.POST };
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", client_id);
            request.AddParameter("client_secret", client_secret);
            request.AddParameter("grant_type", "client_credentials");
            var tResponse = restclient.Execute(request);
            var responseJson = tResponse.Content;
            var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson)["access_token"].ToString();
            return token.Length > 0 ? token : null;
        }


        public ActionResult GetData(string departure, string arrival, DateTime departureDate, int vrijednostValute, DateTime arrivalDate)
        {
            //testni podaci dep:ATL, arr:CUN, date=2019-08-01
            List<InfoLet> InfoLet = new List<InfoLet>();
            var datumPrepravka = departureDate.ToString("yyyy-MM-dd");


            var datumDolaskaPrepravka = arrivalDate.ToString("yyyy-MM-dd");
            var client = new RestClient($"https://test.api.amadeus.com/v1/shopping/flight-offers?origin={departure}&destination={arrival}&departureDate={datumPrepravka}&returnDate={datumDolaskaPrepravka}");


            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            var authKey = GetAuthenticationToken();
            request.AddHeader("authorization", "Bearer " + authKey);
            IRestResponse response = client.Execute(request);

            dynamic obj = JObject.Parse(response.Content);
            var valuta = "EUR";
            var i = 0;
            foreach (var item in obj.data)
            {
                //let broj i
                InfoLet a = new InfoLet();
                var noviLet = item.offerItems[0].services[0].segments;
                var cijenaLeta = item.offerItems[0].price.total;
                var novaCijena = 0;
                if (vrijednostValute == 1)
                {
                    novaCijena = cijenaLeta * 1.00;
                    valuta = "€";
                }
                else if (vrijednostValute == 2)
                {
                    novaCijena = cijenaLeta * 1.12921;
                    valuta = "$";
                }
                else if (vrijednostValute == 3)
                {
                    novaCijena = cijenaLeta * 7.41125;
                    valuta = "kn";
                }
                var j = 0;
                var slobodnaMjesta = 0;
                foreach (var itemTwo in noviLet)
                {
                    
                    var segmenti = itemTwo.flightSegment;
                    slobodnaMjesta = itemTwo.pricingDetailPerAdult.availability;
                    var polazak = (string)segmenti.departure.iataCode;
                    var datumP = (string)segmenti.departure.at;
                    var datumPolaska = datumP.Substring(0, datumP.IndexOf(" "));
                    var dolazak = (string)segmenti.arrival.iataCode;
                    var datumD = (string)segmenti.arrival.at;
                    var datumDolaska = datumD.Substring(0, datumD.IndexOf(" "));
                    
                    if (j == 0)
                    {
                        a.departure = polazak;
                        a.arrival = dolazak;
                        a.brojSlobodnihMjesta = slobodnaMjesta;
                        a.datumDolaska = datumDolaska;
                        a.datumPolaska = datumPolaska;
                        a.cijena = novaCijena;
                        a.valuta = valuta;
                        a.dolazakPresjedanje = "";
                        a.dolazakPresjedanjeDatum = "";
                        a.polazakPresjedanje = "";
                        a.polazakPresjedanjeDatum = "";

                    }
                    else if (j > 0)
                    {
                        a.dolazakPresjedanje = polazak;
                        a.dolazakPresjedanjeDatum = datumPolaska;
                        a.polazakPresjedanje = polazak;
                        a.polazakPresjedanjeDatum = datumDolaska;
                        a.arrival = dolazak;

                    }
                    a.brojPresjedanja = j;
                    j++;
                }

                InfoLet.Add(a);
                i++;
            }



            return View("GetData", InfoLet);
        }

        // GET: Flight
        public ActionResult Index()
        {

            return View();
        }
    }
}