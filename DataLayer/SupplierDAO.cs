using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DataLayer.Models;
using System.IO;
using System.Diagnostics;//For stack trace.

namespace DataLayer
{
    public class SupplierDAO
    {
        private string currentClass = "SupplierDAO";
        //Limiting access to the SQL server.(private string)
        private string connectionString = "Server=Admin2-PC\\SQLExpress;Database=NORTHWND;Trusted_Connection = true";


        /// <summary>
        /// This will CREATE a new supplier.
        /// </summary>
        /// <param name="contactName">The name of the person that owns the company.</param>
        /// <param name="contactTitle"></param>
        /// <param name="postalCode"></param>
        /// <param name="country"></param>
        /// <param name="phoneNumber"></param>
        public void CreateNewSuppliers(SupplierDO supplier)
        {
            string currentMethod = "CreateNewSuppliers";
            try
            {
                //Creates a new connections.
                using (SqlConnection northWndConn = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("CREATE_SUPPLIER", northWndConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;

                    //Parameters that are being passed to the stored procedures.
                    enterCommand.Parameters.AddWithValue("@ContactName", supplier.ContactName);
                    enterCommand.Parameters.AddWithValue("@ContactTitle", supplier.ContactTitle);
                    enterCommand.Parameters.AddWithValue("@PostalCode", supplier.PostalCode);
                    enterCommand.Parameters.AddWithValue("@Country", supplier.Country);
                    enterCommand.Parameters.AddWithValue("@PhoneNumber", supplier.PhoneNumber);

                    //Opening connection.
                    northWndConn.Open();
                    //Execute Non Query command.
                    enterCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //Prints error to console.
                SupplierErrorHandler(ex, currentClass, currentMethod, ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// This will DISPLAY a new supplier.
        /// </summary>
        /// <returns></returns>
        public List<SupplierDO> ViewAllSuppliers()
        {
            string currentMethod = "ViewAllSuppliers";
            try
            {
                List<SupplierDO> allSuppliers = new List<SupplierDO>();

                //Opening SQL connection.
                using (SqlConnection northWndConn = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("DISPLAY_SUPPLIER", northWndConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    northWndConn.Open();

                    //Using SqlDataAdapter to get SQL table.
                    DataTable supplyInfo = new DataTable();
                    using (SqlDataAdapter supplierAdapter = new SqlDataAdapter(enterCommand))
                    {
                        supplierAdapter.Fill(supplyInfo);
                        supplierAdapter.Dispose();
                    }

                    //Putting datarow into a List of the supplier object.
                    foreach (DataRow row in supplyInfo.Rows)
                    {
                        SupplierDO mappedRow = MapAllSuppliers(row);
                        allSuppliers.Add(mappedRow);
                    }
                }
                //Returning an updated list of the supplier object.
                return allSuppliers;
            }
            catch (Exception ex)
            {
                //Prints error to console and logs.
                SupplierErrorHandler(ex, currentClass, currentMethod, ex.StackTrace);
                throw ex;
            }


        }

        /// <summary>
        /// This will UPDATE a new supplier.
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="contactName"></param>
        /// <param name="contactTitle"></param>
        /// <param name="postalCode"></param>
        /// <param name="country"></param>
        /// <param name="phoneNumber"></param>
        public void UpdateSuppliers(SupplierDO supplier)
        {
            string currentMethod = "UpdateSuppliers";
            //Opening SQL connection.
            try
            {
                using (SqlConnection northWndConn = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("UPDATE_SUPPLIER", northWndConn);

                    //Parameters that are being passed to the stored procedures.
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@ContactName", supplier.ContactName);
                    enterCommand.Parameters.AddWithValue("@ContactTitle", supplier.ContactTitle);
                    enterCommand.Parameters.AddWithValue("@PostalCode", supplier.PostalCode);
                    enterCommand.Parameters.AddWithValue("@Country", supplier.Country);
                    enterCommand.Parameters.AddWithValue("@PhoneNumber", supplier.PhoneNumber);
                    enterCommand.Parameters.AddWithValue("@SupplierID", supplier.SupplierId);

                    //Opening connection.
                    northWndConn.Open();
                    //Execute Non Query.
                    enterCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                //Prints error to console and logs.
                SupplierErrorHandler(ex, currentClass, currentMethod, ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// This will DELETE a new supplier by thier Id
        /// </summary>
        /// <param name="supplierId">This is the supplierId</param>
        public void DeleteSuppliers(int contactId)
        {
            string currentMethod = "DeleteSuppliers";
            try
            {
                //Opening SQL connection to modify table using a stored procedure for deleting a row.
                using (SqlConnection northWndConn = new SqlConnection(connectionString))
                {
                    //Creating a new SqlCommand to use a stored procedure.
                    SqlCommand enterCommand = new SqlCommand("DELETE_SUPPLIER", northWndConn);

                    //Parameters that are being passed to the stored procedures.
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@SupplierID", contactId);

                    //Opening connection.
                    northWndConn.Open();
                    //Execute Non Query.
                    enterCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //Prints error to console and logs.
                SupplierErrorHandler(ex, currentClass, currentMethod, ex.StackTrace);
                throw ex;
            }
        }


        /// <summary>
        /// Maps all suppliers from the datarow and returns the SupplierDO object.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public SupplierDO MapAllSuppliers(DataRow dataRow)
        {
            string currentMethod = "MapAllSuppliers";
            try
            {
                SupplierDO supplier = new SupplierDO();

                //If the supplier Id is not null then add values to the supplier object from the database.
                if (dataRow["SupplierID"] != DBNull.Value)
                {
                    supplier.SupplierId = (int)dataRow["SupplierID"];
                }
                supplier.ContactName = dataRow["ContactName"].ToString();
                supplier.ContactTitle = dataRow["ContactTitle"].ToString();
                supplier.PostalCode = dataRow["PostalCode"].ToString();
                supplier.Country = dataRow["Country"].ToString();
                supplier.PhoneNumber = dataRow["Phone"].ToString();

                //Returning the object with a row updated from SQL.
                return supplier;
            }
            catch (Exception ex)
            {
                //Prints error to console and logs.

                SupplierErrorHandler(ex, currentClass, currentMethod, ex.StackTrace);
                throw ex;
            }
        }
        
        /// <summary>
        /// Error Method to write error to a file
        /// </summary>
        /// <param name="ex">The exeption that needs to be written to file.</param>
        public void SupplierErrorHandler(Exception error, string currentClass, string currentMethod, string stackTrace = null)
        {
            try
            {
                if (error.Data["Logged"] == null)
                {
                    StackTrace stack = new StackTrace();

                    //Gets the class of the calling method.
                    //stack.GetFrame(1).GetMethod().ReflectedType
                    //stack.GetFrame(1).GetMethod().Name

                    //using StreamWriter to write error message to a file.
                    using (StreamWriter logWriter = new StreamWriter("ErrorLog.data", true))
                    {
                        logWriter.WriteLine(new string('-', 120));
                        logWriter.WriteLine($"{DateTime.Now.ToString()} - {currentClass} - {currentMethod}");
                        logWriter.WriteLine(error);
                        if (!string.IsNullOrWhiteSpace(stackTrace))
                        {
                            logWriter.WriteLine(stackTrace);
                        }
                        logWriter.Dispose();
                        logWriter.Close();
                    }
                    error.Data["Logged"] = true;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}

