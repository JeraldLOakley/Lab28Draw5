using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lab28Draw.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult NewDeck()
		{ 
			HttpWebRequest WR = WebRequest.CreateHttp("https://deckofcardsapi.com/api/deck/new/draw/?count=5");
			WR.UserAgent = ".Net Framework Test Client";

			HttpWebResponse Response;
			try
			{
				Response = (HttpWebResponse)WR.GetResponse();
			}
			catch (WebException e)
			{

				ViewBag.Error = "Exception";
				ViewBag.ErrorDescription = e.Message;
				return View();
			}

			if (Response.StatusCode != HttpStatusCode.OK)
			{
				ViewBag.Error = Response.StatusCode;
				ViewBag.ErrorDescription = Response.StatusDescription;
				return View();
			}
			StreamReader reader = new StreamReader(Response.GetResponseStream());
			string DeckData = reader.ReadToEnd();

			try
			{
				JObject JsonData = JObject.Parse(DeckData);
				Session["DeckID"] = JsonData["deck_id"];
				
			}
			catch (Exception e)
			{
				ViewBag.Error = "JSON Issue";
				ViewBag.ErrorDescription = e.Message;
				return View();
			}

			return RedirectToAction("Dealing");
		}

		public ActionResult Dealing()
		{
			object ID = Session["DeckID"];
			HttpWebRequest WR = WebRequest.CreateHttp($"https://deckofcardsapi.com/api/deck/{ID}/draw/?count=5");
			WR.UserAgent = ".Net Framework Test Client";
			
			HttpWebResponse Response;
			try
			{
				Response = (HttpWebResponse)WR.GetResponse();
			}
			catch (WebException e)
			{

				ViewBag.Error = "Exception";
				ViewBag.ErrorDescription = e.Message;
				return View();
			}

			if (Response.StatusCode != HttpStatusCode.OK)
			{
				ViewBag.Error = Response.StatusCode;
				ViewBag.ErrorDescription = Response.StatusDescription;
				return View();
			}
			StreamReader reader = new StreamReader(Response.GetResponseStream());
			string DeckData = reader.ReadToEnd();

			try
			{
				JObject JsonData = JObject.Parse(DeckData);
				ViewBag.Cards = JsonData["cards"];
				ViewBag.Remaining = JsonData["remaining"];
				
			}
			catch (Exception e)
			{
				ViewBag.Error = "JSON Issue";
				ViewBag.ErrorDescription = e.Message;
				return View();
			}

			return View();
		}

	}
}