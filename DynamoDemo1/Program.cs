﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
namespace DynamoDemo1
{
    class Program
    {
        static void Main(string[] args)
        {
           
            // First, set up a DynamoDB client for DynamoDB Local
            AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
            //ddbConfig.
            ddbConfig.RegionEndpoint = RegionEndpoint.APSouth1;
            ddbConfig.AuthenticationRegion = "ap-south-1";
            ddbConfig.ServiceURL = "http://localhost:8000";
            AmazonDynamoDBClient client;
            try { client = new AmazonDynamoDBClient(ddbConfig); }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to create a DynamoDB client; " + ex.Message);
                goto PauseForDebugWindow;
            }

            // Build a 'CreateTableRequest' for the new table
            CreateTableRequest createRequest = new CreateTableRequest
            {
                TableName = "Movies",
                AttributeDefinitions = new List<AttributeDefinition>()
        {
          new AttributeDefinition
          {
            AttributeName = "year",
            AttributeType = "N"
          },
          new AttributeDefinition
          {
            AttributeName = "title",
            AttributeType = "S"
          }
        },
                KeySchema = new List<KeySchemaElement>()
        {
          new KeySchemaElement
          {
            AttributeName = "year",
            KeyType = "HASH"
          },
          new KeySchemaElement
          {
            AttributeName = "title",
            KeyType = "RANGE"
          }
        },
            };

            // Provisioned-throughput settings are required even though
            // the local test version of DynamoDB ignores them
            createRequest.ProvisionedThroughput = new ProvisionedThroughput(1, 1);

            // Using the DynamoDB client, make a synchronous CreateTable request
            CreateTableResponse createResponse;
            try { createResponse = client.CreateTable(createRequest); }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to create the new table; " + ex.Message);
                goto PauseForDebugWindow;
            }

            // Report the status of the new table...
            Console.WriteLine("\n\n Created the \"Movies\" table successfully!\n    Status of the new table: '{0}'", createResponse.TableDescription.TableStatus);

            // Keep the console open if in Debug mode...
            PauseForDebugWindow:
            Console.Write("\n\n ...Press any key to continue");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
