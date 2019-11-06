using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//additional Namespace
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using FSE.PO.WEBAPI.Models;

namespace FSE.PO.WEBAPI.Repositories
{
    public class PurchaseOrderRepository: IPurchaseOrderRepository
    {
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["PurchaseOrderServiceConnection"].ConnectionString;
            }
        }

        public IEnumerable<PurchaseOrder> GetAll()
        {
            List<PurchaseOrder> purchaseOrderList = new List<PurchaseOrder>();
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
                                     select new PurchaseOrder()
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
            catch (Exception)
            {
                if (sqlDataReader != null || sqlConnection.State == ConnectionState.Open)
                {
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }

            return purchaseOrderList;
        }

        public PurchaseOrder GetData(string poNo)
        {
            try
            {
                PurchaseOrder purchaseOrder = null;
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("sp_GetPurchaseRecord", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //adding Parameters
                    sqlCommand.Parameters.Add(new SqlParameter("@poNO", poNo));

                    SqlDataReader sqlDataReader = null;

                    sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        purchaseOrder = new PurchaseOrder()
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
                return purchaseOrder;
            }
            catch (SqlException sqlException)
            {
                throw sqlException;
            }
        }
       
        public int Insert(PurchaseOrder purchaseOrder)
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
                    sqlCommand.Parameters.Add(new SqlParameter("@suplNo", purchaseOrder.SuplName));
                    sqlCommand.Parameters.Add(new SqlParameter("@poDate", purchaseOrder.PODate));
                    sqlCommand.Parameters.Add(new SqlParameter("@itCode", purchaseOrder.ITDesc));
                    sqlCommand.Parameters.Add(new SqlParameter("@qty", purchaseOrder.Qty));

                    numberOfRecords = sqlCommand.ExecuteNonQuery();

                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
                return numberOfRecords;
            }
            catch (SqlException sqlException)
            {
                throw sqlException;
            }
        }

        public int Delete(string poNo)
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
                throw sqlException;
            }
        }

        public int Update(PurchaseOrder purchaseOrder)
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
                    sqlCommand.Parameters.Add(new SqlParameter("@poNo", purchaseOrder.PONo));
                    sqlCommand.Parameters.Add(new SqlParameter("@suplNo", purchaseOrder.SuplNo));
                    sqlCommand.Parameters.Add(new SqlParameter("@poDate", purchaseOrder.PODate));
                    sqlCommand.Parameters.Add(new SqlParameter("@itCode", purchaseOrder.ITCode));
                    sqlCommand.Parameters.Add(new SqlParameter("@qty", purchaseOrder.Qty));

                    numberOfRecords = sqlCommand.ExecuteNonQuery();

                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
                return numberOfRecords;
            }
            catch (SqlException sqlException)
            {
                throw sqlException;
            }
        }

        IEnumerable<Supplier> IPurchaseOrderRepository.GetSupplier()
        {
            List<Supplier> supplierList = new List<Supplier>();
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
                                select new Supplier()
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

                throw exception;
            }

            return supplierList;
        }

        IEnumerable<Item> IPurchaseOrderRepository.GetItem()
        {
            List<Item> itemList = new List<Item>();
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
                            select new Item()
                            {
                                ItCode = dr["ITCODE"].ToString(),
                                ItDesc = dr["ITDESC"].ToString()
                            }).ToList();
            }
            catch (Exception exception)
            {
                if (sqlDataReader != null || sqlConnection.State == ConnectionState.Open)
                {
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }

                throw exception;
            }
            return itemList;
        }
    }
}