using Nancy;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Inventory
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] =_=>{
        return View["form.cshtml"];
      };

      Post["/inventory/created"] =_=> {
        Thing newInventroy = new Thing(Request.Form["name"], Request.Form["descrip"]);
        newInventroy.Save();
        List<Thing> allInventory = Thing.GetAll();
        return View["results.cshtml", allInventory];

      };
      Get["/inventory/deleted"] =_=> {
        Thing.DeleteAll();
        return View["deleted.cshtml"];
      };
      Post["/search/results"] = _ => {
        Thing foundThing = Thing.Find(Request.Form["searched-name"]);
        return View["search-results.cshtml", foundThing];
      };

    }
  }
}
