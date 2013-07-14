using System;
using AWS.Test.Library;

namespace AWS.Test.Console
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			RunDynamoDBClientTests();
			RunDynamoDBTableTests();
			RunDynamoDBTableCacheTests();
			System.Console.WriteLine("\nTests Completed");
			System.Console.Read();
		}

		public static void RunDynamoDBClientTests()
		{
			DynamoDBClientTests dynamoDBClientTests = new DynamoDBClientTests();
			System.Console.WriteLine("DynamoDBClientTests: Constructor           {0}", dynamoDBClientTests.Constructor() ? "Pass" : "Fail");
			System.Console.WriteLine("DynamoDBClientTests: CreateTable           {0}", dynamoDBClientTests.CreateTable() ? "Pass" : "Fail");
			System.Console.WriteLine("DynamoDBClientTests: DeleteTable           {0}", dynamoDBClientTests.DeleteTable() ? "Pass" : "Fail");
			System.Console.WriteLine("DynamoDBClientTests: Index                 {0}", dynamoDBClientTests.Index() ? "Pass" : "Fail");
		}

		public static void RunDynamoDBTableTests()
		{
			DynamoDBTableTests dynamoDBTableTests = new DynamoDBTableTests();
			System.Console.WriteLine("DynamoDBTableTests:  Constructor           {0}", dynamoDBTableTests.Constructor() ? "Pass" : "Fail");
			System.Console.WriteLine("DynamoDBTableTests:  CreateTable           {0}", dynamoDBTableTests.CreateTable() ? "Pass" : "Fail");
			System.Console.WriteLine("DynamoDBTableTests:  DeleteTable           {0}", dynamoDBTableTests.DeleteTable() ? "Pass" : "Fail");
			System.Console.WriteLine("DynamoDBTableTests:  GetDictionaryOfTables {0}", dynamoDBTableTests.GetDictionaryOfTables() ? "Pass" : "Fail");
			System.Console.WriteLine("DynamoDBTableTests:  GetItem               {0}", dynamoDBTableTests.GetItem() ? "Pass" : "Fail");
			System.Console.WriteLine("DynamoDBTableTests:  GetListOfTables       {0}", dynamoDBTableTests.GetListOfTables() ? "Pass" : "Fail");
			System.Console.WriteLine("DynamoDBTableTests:  PutItem               {0}", dynamoDBTableTests.PutItem() ? "Pass" : "Fail");
			System.Console.WriteLine("DynamoDBTableTests:  UpdateItem            {0}", dynamoDBTableTests.UpdateItem() ? "Pass" : "Fail");
		}

		public static void RunDynamoDBTableCacheTests()
		{
			DynamoDBTableCacheTests dynamoDBTableCacheTests = new DynamoDBTableCacheTests();
			System.Console.WriteLine("DynamoDBTableCacheTests:  Constructor      {0}", dynamoDBTableCacheTests.Constructor() ? "Pass" : "Fail");
			System.Console.WriteLine("DynamoDBTableCacheTests:  PutItem          {0}", dynamoDBTableCacheTests.PutItem() ? "Pass" : "Fail");
			System.Console.WriteLine("DynamoDBTableCacheTests:  GetItem          {0}", dynamoDBTableCacheTests.GetItem() ? "Pass" : "Fail");
		}
	}
}