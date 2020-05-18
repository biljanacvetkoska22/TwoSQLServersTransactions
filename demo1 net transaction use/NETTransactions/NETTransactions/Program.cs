using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VolatileResourceManager;

namespace NETTransactions
{
    class Program
    {
        static void Main(string[] args)
        {
            //volatile rms
            /*
            Transactional<int[]> numbers1 = new Transactional<int[]>(new int[3]);
            Transactional<int[]> numbers2 = new Transactional<int[]>(new int[2]);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    VolatileOperation(numbers1);

                    VolatileOperation(numbers2);

                    scope.Complete();
                    Console.WriteLine("Transaction Completed...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Value is: " + numbers1.Value[0]);
            }*/

            //type 3 transaction with msmq
            /*  try
              {
                  using (TransactionScope scope = new TransactionScope())
                  {
                      MSMQOperation();
                      scope.Complete();
                      Console.WriteLine("Transaction Completed...");
                  }
              }
              catch (Exception ex)
              {
                  Console.WriteLine(ex.ToString());
              }*/

            // type 2 transaction
            /* try
             {
                 using (TransactionScope scope = new TransactionScope())
                 {
                     Transaction t = Transaction.Current;
                     Console.WriteLine("Isolation: " + t.IsolationLevel);
                     Console.WriteLine("");

                     DBOperation1();
                     Console.WriteLine("Call to 1st Durable RM");
                     Console.WriteLine("Local Identifier: " + t.TransactionInformation.LocalIdentifier);
                     Console.WriteLine("Distributed Identifier: " + t.TransactionInformation.DistributedIdentifier);
                     Console.WriteLine("");

                     DBOperation2();
                     Console.WriteLine("Call to 2nd Durable RM");
                     Console.WriteLine("Local Identifier: " + t.TransactionInformation.LocalIdentifier);
                     Console.WriteLine("Distributed Identifier: " + t.TransactionInformation.DistributedIdentifier);
                     Console.WriteLine("");

                     scope.Complete();
                     Console.WriteLine("Transaction Completed...");
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.ToString());
             }*/


            // type 1 transaction
            /*  try
              {

                  using (TransactionScope scope = new TransactionScope())
                  {
                      Console.WriteLine("Isolation: " + Transaction.Current.IsolationLevel);
                      DBOperation1();
                      scope.Complete();
                      Console.WriteLine("Transaction Completed...");
                  }
              }
              catch (Exception ex)
              {
                  Console.WriteLine(ex.ToString());
              }*/


            Console.ReadLine();
        }

        private static void DBOperation1()
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            cnn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=DemoDB;Data Source=.\\SQLEXPRESS");
            cmd = new SqlCommand("insert into demotable values" +
                                 " ('this is test data from operation1')", cnn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
           // throw new Exception("Test Exception");
           // cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private static void DBOperation2()
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            cnn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=DemoDB;Data Source=.\\SQLEXPRESSR2");
            cmd = new SqlCommand("insert into demotable values" +
                                 " ('this is test data from operation2')", cnn);
            cmd.Connection.Open();

            throw new Exception("test");
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private static void MSMQOperation()
        {
            MessageQueue queue = null;
            queue = new MessageQueue();
            queue.Path = @".\Private$\DemoQueue";
            queue.Send("Message Body", "Message Label",
                       MessageQueueTransactionType.Automatic);
        }

        private static void VolatileOperation(Transactional<int[]> numbers)
        {
            numbers.Value[0] = 11;
            numbers.Value[1] = 22;
            numbers.Value[2] = 33;
        }
    }
}
