using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FSE.PO.SVCAPP.Entity;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FSE.PO.SVCAPP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PurchaseOrderService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PurchaseOrderService.svc or PurchaseOrderService.svc.cs at the Solution Explorer and start debugging.
    public class PurchaseOrderService : IPurchaseOrderService
    {
        //Define a static string variable and get connection string details from web.config
        public static String ConnectionString =
            ConfigurationManager.ConnectionStrings["PurchaseOrderServiceConnection"].ConnectionString;

        public string ValidateService()
        {
            return string.Concat(string.Format("Response From Service Layer: {0}", DateTime.Now.ToString()));
        }

        public List<PurchaseOrderEntity> GetAllPurchaseOrder()
        {
            List<PurchaseOrderEntity> purchaseOrderList = new List<PurchaseOrderEntity>();
            DataTable purchaseOrderDataTable = new DataTable();

            SqlDataReader sqlDataReader = null;
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand("sp_GetAllPurchaseRecord", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            try
            {
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                purchaseOrderDataTable.Load(sqlDataReader);
                sqlDataReader.Close();
                sqlConnection.Close();

                purchaseOrderList = (from DataRow dr in purchaseOrderDataTable.Rows
                    select new PurchaseOrderEntity()
                    {
                        SuplNo = dr["SuplNo"].ToString(),
                        SuplName = dr["SuplName"].ToString(),
                        ITCode = dr["ITCode"].ToString(),
                        ITDesc = dr["ITDesc"].ToString(),
                        Qty = Convert.ToInt32(dr["Qty"]),
                        PONo = dr["PONo"].ToString(),
                        PODate = Convert.ToDateTime(dr["PODate"])
                    }).ToList();
            }
            catch (Exception exception)
            {
                if (sqlDataReader != null || sqlConnection.State == ConnectionState.Open)
                {
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }

                throw new FaultException<ExceptionMessage>(new ExceptionMessage(exception.Message));
            }

            return purchaseOrderList;
        }

        public PurchaseOrderEntity GetPurchaseOrderByPONO(string poNumber)
        {
            try
            {
                PurchaseOrderEntity purchaseOrderEntity = null;
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("sp_GetPurchaseRecord", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //adding Parameters
                    sqlCommand.Parameters.Add(new SqlParameter("@poNO", poNumber));

                    SqlDataReader sqlDataReader = null;

                    sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        purchaseOrderEntity = new PurchaseOrderEntity()
                        {
                            PONo = sqlDataReader.GetValue(0).ToString(),
                            SuplNo = sqlDataReader.GetValue(1).ToString(),
                            SuplName = sqlDataReader.GetValue(2).ToString(),
                            ITCode = sqlDataReader.GetValue(3).ToString(),
                            ITDesc = sqlDataReader.GetValue(4).ToString(),
                            Qty = Convert.ToInt32(sqlDataReader.GetValue(5)),
                            PODate = Convert.ToDateTime(sqlDataReader.GetValue(6))
                        };                        
                    }
                }
                return purchaseOrderEntity;
            }
            catch (SqlException sqlException)
            {
                throw new FaultException<ExceptionMessage>(new ExceptionMessage(sqlException.Message.ToString()));
            }
        }

        public int CreatePurchaseOrder(PurchaseOrderEntity purchaseOrderEntity)
        {
            int numberOfRecords = 0;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("sp_InsertPurchaseRecord", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    //adding Parameters
                    sqlCommand.Parameters.Add(new SqlParameter("@suplNo", purchaseOrderEntity.SuplName));
                    sqlCommand.Parameters.Add(new SqlParameter("@poDate", purchaseOrderEntity.PODate));
                    sqlCommand.Parameters.Add(new SqlParameter("@itCode", purchaseOrderEntity.ITDesc));
                    sqlCommand.Parameters.Add(new SqlParameter("@qty", purchaseOrderEntity.Qty));
                    
                    numberOfRecords = sqlCommand.ExecuteNonQuery();

                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
                return numberOfRecords;
            }
            catch (SqlException sqlException)
            {
                throw new FaultException<ExceptionMessage>(new ExceptionMessage(sqlException.Message));
            }
        }

        public int DeletePurchaseOrder(string poNo)
        {
            int numberOfRecords = 0;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("sp_DeletePurchaseRecord", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //adding Parameters
                    sqlCommand.Parameters.Add(new SqlParameter("@poNO", poNo));
                    numberOfRecords = sqlCommand.ExecuteNonQuery();

                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
                return numberOfRecords;
            }
            catch (SqlException sqlException)
            {
                throw new FaultException<ExceptionMessage>(new ExceptionMessage(sqlException.Message));
            }
        }       

        public int UpdatePurchaseOrder(PurchaseOrderEntity purchaseOrderEntity)
        {
            int numberOfRecords = 0;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("sp_UpdatePurchaseRecord", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    //adding Parameters
                    sqlCommand.Parameters.Add(new SqlParameter("@poNo", purchaseOrderEntity.PONo));
                    sqlCommand.Parameters.Add(new SqlParameter("@suplNo", purchaseOrderEntity.SuplNo));
                    sqlCommand.Parameters.Add(new SqlParameter("@poDate", purchaseOrderEntity.PODate));
                    sqlCommand.Parameters.Add(new SqlParameter("@itCode", purchaseOrderEntity.ITCode));
                    sqlCommand.Parameters.Add(new SqlParameter("@qty", purchaseOrderEntity.Qty));

                    numberOfRecords = sqlCommand.ExecuteNonQuery();

                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
                return numberOfRecords;
            }
            catch (SqlException sqlException)
            {
                throw new FaultException<ExceptionMessage>(new ExceptionMessage(sqlException.Message));
            }
        }
               
        IEnumerable<SupplierEntity> IPurchaseOrderService.GetSupplier()
        {            
            List<SupplierEntity> supplierList = new List<SupplierEntity>();
            DataTable supplierDataTable = new DataTable();

            SqlDataReader sqlDataReader = null;
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand("sp_GetSupplierRecord", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            try
            {
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                supplierDataTable.Load(sqlDataReader);
                sqlDataReader.Close();
                sqlConnection.Close();

                supplierList = (from DataRow dr in supplierDataTable.Rows
                                     select new SupplierEntity()
                                     {
                                         SuplNo = dr["SuplNo"].ToString(),
                                         SuplName = dr["SuplName"].ToString()                                         
                                     }).ToList();
            }
            catch (Exception exception)
            {
                if (sqlDataReader != null || sqlConnection.State == ConnectionState.Open)
                {
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }

                throw new FaultException<ExceptionMessage>(new ExceptionMessage(exception.Message));
            }

            return supplierList;
        }

        IEnumerable<ItemEntity> IPurchaseOrderService.GetItem()
        {
            List<ItemEntity> itemList = new List<ItemEntity>();
            DataTable itemDataTable = new DataTable();

            SqlDataReader sqlDataReader = null;
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand("sp_GetItemRecord", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            try
            {
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                itemDataTable.Load(sqlDataReader);
                sqlDataReader.Close();
                sqlConnection.Close();

                itemList = (from DataRow dr in itemDataTable.Rows
                                select new ItemEntity()
                                {
                                    ItCode = dr["ITCODE"].ToString(),
                                    ItDesc= dr["ITDESC"].ToString()
                                }).ToList();
            }
            catch (Exception exception)
            {
                if (sqlDataReader != null || sqlConnection.State == ConnectionState.Open)
                {
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }

                throw new FaultException<ExceptionMessage>(new ExceptionMessage(exception.Message));
            }

            return itemList;
        }
    }
}
