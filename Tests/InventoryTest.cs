using System.Data;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Xunit;

namespace Inventory
{
  public class InventoryTest : IDisposable
  {
    public InventoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Thing.DeleteAll();
    }
    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Thing.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Thing testThing = new Thing("Baseball Card", "Mickey Mantle Rookie");

      //Act
      testThing.Save();
      List<Thing> result = Thing.GetAll();
      List<Thing> testList = new List<Thing>{testThing};
      Console.WriteLine(result[0].GetDescription());
      Console.WriteLine(testList[0].GetDescription());
      Console.WriteLine(result[0].GetName());
      Console.WriteLine(testList[0].GetName());
      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Thing firstThing = new Thing("Baseball Card", "Mickey Mantle Rookie");
      Thing secondThing = new Thing("Baseball Card", "Mickey Mantle Rookie");

      //Assert
      Assert.Equal(firstThing, secondThing);
    }
    [Fact]
    public void Test_FindFindsThingInDatabase()
    {
      //Arrange
      Thing testThing = new Thing("Baseball Card", "Mickey Mantle Rookie");
      testThing.Save();

      //Act
      Thing foundThing = Thing.Find(testThing.GetId());

      //Assert
      Assert.Equal(testThing, foundThing);
    }
  }
}
