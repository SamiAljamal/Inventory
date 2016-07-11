using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Inventory
{
  public class Thing
  {
    private int _id;
    private string _description;
    private string _name;

    public Thing(string name, string description,int id = 0)
    {
      _id = id;
      _description = description;
      _name = name;
    }

    public int GetId()
    {
      return _id;
    }

    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public string GetName()
    {
      return _name;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM inventory;", conn);
      cmd.ExecuteNonQuery();
    }

    public static List<Thing> GetAll()
    {
      List<Thing> allInventory = new List<Thing>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM inventory;", conn);
      rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int thingId = rdr.GetInt32(0);
        string thingName = rdr.GetString(1);
        string thingDescription = rdr.GetString(2);

        Thing newThing = new Thing(thingName, thingDescription, thingId);
        allInventory.Add(newThing);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allInventory;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO inventory (description, name) OUTPUT INSERTED.id VALUES (@ThingDescription, @ThingName);", conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@ThingDescription";
      descriptionParameter.Value = this.GetDescription();

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@ThingName";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(descriptionParameter);
      cmd.Parameters.Add(nameParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (cmd != null)
      {
        conn.Close();
      }
    }
    public override bool Equals(System.Object otherThing)
    {
      if (!(otherThing is Thing))
      {
        return false;
      }
      else
      {
        Thing newThing = (Thing) otherThing;
        bool idEquality = this.GetId() == newThing.GetId();
        bool descriptionEquality = this.GetDescription() == newThing.GetDescription();
        bool nameEquality = this.GetName() == newThing.GetName();
        return (idEquality && descriptionEquality && nameEquality);
      }
    }
    public override int GetHashCode()
    {
         return this.GetName().GetHashCode();
    }
    public static Thing Find(string name)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM inventory WHERE name = @ThingName;", conn);
      SqlParameter thingNameParameter = new SqlParameter();
      thingNameParameter.ParameterName = "@ThingName";
      thingNameParameter.Value = name;
      cmd.Parameters.Add(thingNameParameter);
      rdr = cmd.ExecuteReader();

      int foundThingId = 0;
      string foundThingName = null;
      string foundThingDescription = null;

      while(rdr.Read())
      {
        foundThingId = rdr.GetInt32(0);
        foundThingName = rdr.GetString(1);
        foundThingDescription = rdr.GetString(2);
      }
      Thing foundThing = new Thing(foundThingName, foundThingDescription, foundThingId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundThing;
    }
  }
}
