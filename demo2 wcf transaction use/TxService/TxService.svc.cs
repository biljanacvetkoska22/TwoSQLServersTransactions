using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Transactions;

namespace TxService
{
    public class TxService : ITxService
    {
        [OperationBehavior(TransactionScopeRequired = true,
                            TransactionAutoComplete = true)]
        public bool TxOperation()
        {
            


            NetTcpBinding tcp = (NetTcpBinding)OperationContext.Current.Host.Description.Endpoints[0].Binding;
            TransactionProtocol pro = tcp.TransactionProtocol;


            WSHttpBinding ws = (WSHttpBinding)OperationContext.Current.Host.Description.Endpoints[1].Binding;
              
                     
            TransactionInformation info = Transaction.Current.TransactionInformation;
            string localId = String.Empty;
            Guid distributedId = Guid.Empty;
            localId = info.LocalIdentifier;
            distributedId = info.DistributedIdentifier;
            DBOperation1();

            localId = info.LocalIdentifier;
            distributedId = info.DistributedIdentifier;
            
            DBOperation2();
            
            localId = info.LocalIdentifier;
            distributedId = info.DistributedIdentifier;

            return true;
        }

        private void DBOperation1()
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            cnn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=DemoDB;Data Source=.\\SQLEXPRESS");
            cmd = new SqlCommand("insert into demotable values" +
                                 " ('test data')", cnn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private void DBOperation2()
        {
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            cnn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=DemoDB;Data Source=.\\SQLEXPRESSR2");
            cmd = new SqlCommand("insert into demotable values" +
                                 " ('test data')", cnn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
        
    }
}
