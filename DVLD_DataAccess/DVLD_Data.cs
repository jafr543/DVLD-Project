using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;


namespace DVLD_DAL
{
    public class People_Data
    {
        public static DataTable GetallPeople()
        {
            DataTable dtPeople = new DataTable();

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT   People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName,
 	                        CASE
	                        WHEN People.Gendor = 0 THEN 'Male'
	                        ELSE 'Female' 
	                        END as Gendor,
	                        CONVERT(varchar,People.DateOfBirth,101) AS DateOfBirth, Countries.CountryName AS Nationality, People.Phone, People.Email
                            FROM   Countries INNER JOIN
                            People ON Countries.CountryID = People.NationalityCountryID";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtPeople.Load(reader);
                }

                reader.Close();
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return dtPeople;
        }
    
        public static DataTable GetallCountries()
        {
            DataTable dtCountries = new DataTable();

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT  CountryID, CountryName
                            FROM  Countries";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtCountries.Load(reader);
                }

                reader.Close();
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return dtCountries;
        }

        public static int AddNew(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
                            DateTime DateOfBirth, int NationalityCountryID, int Gendor, string Address, string Phone,
                            string Email, string ImagePath)
        {
            int ID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"INSERT INTO People (NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth,
                            Gendor, Address, Phone, Email, NationalityCountryID, ImagePath)
                            Values (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName,
                                    @DateOfBirth, @Gendor, @Address,
                                    @Phone, @Email, @NationalityCountryID, @ImagePath);
                                SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@ImagePath", ImagePath);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if(Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    ID = InsertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ID;

        }

        public static bool Update(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
                            DateTime DateOfBirth, int NationalityCountryID, int Gendor, string Address, string Phone,
                            string Email, string ImagePath)
        {
            int rowseffected = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @" UPDATE People
                                   SET NationalNo = @NationalNo
                                      ,FirstName = @FirstName
                                      ,SecondName = @SecondName
                                      ,ThirdName = @ThirdName
                                      ,LastName = @LastName
                                      ,DateOfBirth = @DateOfBirth
                                      ,Gendor = @Gendor
                                      ,Address = @Address
                                      ,Phone = @Phone
                                      ,Email = @Email
                                      ,NationalityCountryID = @NationalityCountryID
                                      ,ImagePath = @ImagePath
                                        WHERE PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@ImagePath", ImagePath);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();
            }

            catch
            {
                rowseffected = 0;
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }


        public static bool Find(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName,
                            ref DateTime DateOfBirth, ref int NationalityCountryID,ref string CountryName, ref int Gendor, ref string Address, ref string Phone,
                            ref string Email, ref string ImagePath)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT   People.*, Countries.CountryName
                                FROM   Countries INNER JOIN
                                People ON Countries.CountryID = People.NationalityCountryID
                                WHERE	PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    isFound = true;

                    NationalNo = (string)reader["NationalNo"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];
                    ThirdName = (string)reader["ThirdName"];
                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    NationalityCountryID = (int)reader["NationalityCountryID"];
                    CountryName = (string)reader["CountryName"];
                    Gendor = Convert.ToInt32(reader["Gendor"]);
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];
                    if (reader["Email"] != DBNull.Value)
                        Email = (string)reader["Email"];
                    else
                        Email = "";
                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)reader["ImagePath"];
                    else
                        ImagePath = "";
                }

                reader.Close();
            }

            catch
            {
                isFound = false;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool Find(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName,
                           ref DateTime DateOfBirth, ref int NationalityCountryID, ref string CountryName, ref int Gendor, ref string Address, ref string Phone,
                           ref string Email, ref string ImagePath)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT   People.*, Countries.CountryName
                                FROM   Countries INNER JOIN
                                People ON Countries.CountryID = People.NationalityCountryID
                                WHERE	NationalNo = @NationalNo;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];
                    ThirdName = (string)reader["ThirdName"];
                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    NationalityCountryID = (int)reader["NationalityCountryID"];
                    CountryName = (string)reader["CountryName"];
                    Gendor = Convert.ToInt32(reader["Gendor"]);
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];
                    if (reader["Email"] != DBNull.Value)
                        Email = (string)reader["Email"];
                    else
                        Email = "";
                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)reader["ImagePath"];
                    else
                        ImagePath = "";
                }

                reader.Close();
            }

            catch
            {
                isFound = false;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int FindByAppID(int AppID)
        {
            int PersonID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT Applications.ApplicantPersonID
                            FROM Applications WHERE ApplicationID = @AppID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppID", AppID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int ID))
                {
                    PersonID = ID;
                }

            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return PersonID;
        }

        public static int FindByLocalAppID(int LocalAppID)
        {
            int PersonID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT Applications.ApplicantPersonID
                            FROM Applications INNER JOIN
                            LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            WHERE LocalDrivingLicenseApplicationID = @LocalAppID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if(int.TryParse(Result.ToString(), out int ID))
                {
                    PersonID = ID;
                }
                
            }

            catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return PersonID;
        }

        public static int FindByLicenseID(int LicenseID)
        {
            int PersonID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT PersonID  
                             FROM Drivers INNER JOIN
                             Licenses ON Drivers.DriverID = Licenses.DriverID
                             WHERE LicenseID = @LicenseID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int ID))
                {
                    PersonID = ID;
                }

            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return PersonID;
        }

        public static bool isNationalNoExist(string NationalNo, int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT Found = 1 FROM People WHERE (NationalNo = @NationalNo)
                                AND (@PersonID = -1 OR PersonID <> @PersonID)";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch
            {
                isFound = false;
            }

            finally 
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool isNationalNoExist(string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT Found = 1 FROM People WHERE NationalNo = @NationalNo;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch
            {
                isFound = false;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool isPersonExist(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @" SELECT PersonID FROM People
                                        WHERE PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                isFound = false;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool Delete(int PersonID, out string ImagePath)
        {
            int rowseffected = 0;
            ImagePath = "";

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string SelectQuery = @"SELECT ImagePath FROM People WHERE PersonID = @PersonID";

            string DeleteQuery = @"DELETE FROM People
                             WHERE PersonID = @PersonID;";

            SqlCommand Selectcommand = new SqlCommand(SelectQuery, connection);
            SqlCommand Deletecommand = new SqlCommand(DeleteQuery, connection);

            Selectcommand.Parameters.AddWithValue("@PersonID", PersonID);
            Deletecommand.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = Selectcommand.ExecuteReader();

                
                if(reader.Read())
                {
                    if(reader["ImagePath"] != DBNull.Value)
                       ImagePath = (string)reader["ImagePath"];
                }
                reader.Close();

                rowseffected = Deletecommand.ExecuteNonQuery();
            }

            catch (SqlException exception)
            {
                rowseffected = 0;
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }
    }

    public class User_Data
    {
        public static DataTable GetallUsers()
        {
            DataTable users = new DataTable();

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT  Users.UserID, Users.PersonID,CONCAT_WS(' ', People.FirstName , People.SecondName , People.ThirdName , People.LastName) as FullName,
                              Users.UserName, Users.IsActive
                               FROM People INNER JOIN
                                Users ON People.PersonID = Users.PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    users.Load(reader);
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return users;
        }

        public static int AddNew(int PersonID, string UserName, string Password, bool IsActive)
        {
            int ID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive)
                            Values (@PersonID, @UserName, @Password, @IsActive)
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int NewUserID))
                {
                    ID = NewUserID;
                }
            }

            catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ID;
        }

        public static bool Update(int UserID, string UserName, string Password,
                                                        bool IsActive)
        {
            int rowseffected = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"Update Users
                                Set UserName = @UserName,
                                    Password = @Password,
                                    IsActive = @IsActive
                                    Where UserID = @UserID;";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@UserID", UserID);


            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();
            }

            catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }

        public static bool Delete(int UserID)
        {
            int rowseffected = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"Delete FROM Users WHERE UserID = @UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

              rowseffected = command.ExecuteNonQuery();

            }

            catch (SqlException ex)
            {
                //throw new Exception(ex.Message);
                rowseffected = 0;
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }

        public static bool Find(string UserName, string Password, ref int UserID, ref int PersonID,
                                                        ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM  Users WHERE UserName = @UserName  AND Password = @Password;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    isFound = true;

                    UserID = (int)reader["UserID"];
                    PersonID = (int)reader["PersonID"];
                    IsActive = (bool)reader["IsActive"];
                }

                reader.Close();
            }

            catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;

        }

        public static bool Find(int UserID, ref int PersonID, ref string UserName, ref string Password, 
                                                        ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM  Users WHERE UserID = @UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;

        }

        public static bool IsUserExistByPersonID(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT PersonID FROM Users
                                        WHERE PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static bool IsUserExistByUserID(int UserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT UserID FROM Users
                                        WHERE UserID = @UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsUserNameExist(string UserName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT UserName FROM Users WHERE UserName = @UserName;";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsUserNameExist(string UserName, int UserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT UserName FROM Users WHERE UserName = @UserName AND UserID <> @UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

    }

    public class ApplicationsTypes_Data
    {
       public static DataTable GetallApplicationsTypes()
        {
            DataTable ApplicationsTypes = new DataTable();

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT ApplicationTypeID AS ID,
                            ApplicationTypeTitle AS Title,
                            ApplicationFees AS Fees
                            FROM ApplicationTypes";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    ApplicationsTypes.Load(reader);
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ApplicationsTypes;

        }

        public static bool Update(int ApplicationTypeID, string ApplicationTypeTitle, decimal ApplicationFees)
        {
            int rowseffected = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"Update ApplicationTypes
                                Set ApplicationTypeTitle = @ApplicationTypeTitle,
                                    ApplicationFees = @ApplicationFees
                                    Where ApplicationTypeID = @ApplicationTypeID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);


            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }

       public static bool Find(int ApplicationTypeID, ref string ApplicationTypeTitle, ref decimal ApplicationFees)
       {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM  ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                    ApplicationFees = (decimal)reader["ApplicationFees"];
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int GetAppTypeFees(int ApplicationTypeID)
        {
            decimal Fees  = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT ApplicationFees FROM ApplicationTypes
                            WHERE ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                connection.Open();

                Fees = Convert.ToDecimal(command.ExecuteScalar());
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (int)Fees;
        }
    }

    public class Applications_Data
    {
        public static int AddNew(int PersonID, int ApplicationTypeID, int ApplicationStatus, decimal Fees, DateTime ApplicationDate, int CreatedByUserID)
        {
            int ID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus,
                                            LastStatusDate, PaidFees, CreatedByUserID)
                            Values (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus,
                                    @LastStatusDate, @PaidFees, @CreatedByUserID)
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", ApplicationDate);
            command.Parameters.AddWithValue("@PaidFees", Fees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int NewUserID))
                {
                    ID = NewUserID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ID;
        }

        public static bool CancelApplication(int LocalDrivingLicenseApplicationID)
        {
            int rowseffected = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"Update A
                                Set A.ApplicationStatus = 2,
                                A.LastStatusDate = GETDATE()
                                FROM Applications A 
                                INNER JOIN
                                LocalDrivingLicenseApplications L ON A.ApplicationID = L.ApplicationID
                                Where L.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                                AND A.ApplicationStatus = 1;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }

        public static bool CompleteApplication(int PersonID, int ApplicationTypeID)
        {
            int rowseffected = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"Update Applications
                                Set ApplicationStatus = 3,
                                LastStatusDate = GETDATE()
                                WHERE ApplicationID = 
                                (
                                SELECT TOP 1 ApplicationID FROM
                                Applications
                                WHERE ApplicantPersonID = @PersonID AND ApplicationTypeID = @ApplicationTypeID
                                AND ApplicationStatus = 1
                                ORDER BY ApplicationID DESC
                                );";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }

        public static bool CompleteApplication(int ApplicationID)
        {
            int rowseffected = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"Update Applications
                                Set ApplicationStatus = 3,
                                LastStatusDate = GETDATE()
                                WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }

        public static bool Find(int LocalAppID, ref int ApplicationID, ref int PersonID, ref DateTime ApplicationDate,
           ref int ApplicationTypeID, ref int ApplicationStatus, ref DateTime LastStatusDate,
           ref decimal PaidFees, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT Applications.*
                             FROM Applications INNER JOIN
                             LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                             WHERE LocalDrivingLicenseApplicationID = @LocalAppID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ApplicationID = (int)reader["ApplicationID"];
                    PersonID = (int)reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];

                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool Find(int ApplicationID, ref int PersonID, ref DateTime ApplicationDate,
   ref int ApplicationTypeID, ref int ApplicationStatus, ref DateTime LastStatusDate,
   ref decimal PaidFees, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM Applications 
                             ApplicationID = @ApplicationID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    PersonID = (int)reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];

                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }


    }

    public class TestTypes_Data
    {
        public static DataTable GetallTestTypes()
        {
            DataTable TestTypes = new DataTable();

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT TestTypeID AS ID,
                            TestTypeTitle AS Title,
                            TestTypeDescription AS Description,
                            TestTypeFees AS Fees
                            FROM TestTypes";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    TestTypes.Load(reader);
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return TestTypes;
        }

        public static bool Update(int TestTypeID, string TestTypeTitle, string TestTypeDescription,
                                    decimal TestTypeFees)
        {
            int rowseffected = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"Update TestTypes
                                Set TestTypeTitle = @TestTypeTitle,
                                    TestTypeDescription = @TestTypeDescription,
                                    TestTypeFees = @TestTypeFees
                                    Where TestTypeID = @TestTypeID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }

        public static bool Find(int TestTypeID, ref string TestTypeTitle, ref string TestTypeDescription,
                                ref decimal TestTypeFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM  TestTypes WHERE TestTypeID = @TestTypeID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestTypeTitle = (string)reader["TestTypeTitle"];
                    TestTypeDescription = (string)reader["TestTypeDescription"];
                    TestTypeFees = (decimal)reader["TestTypeFees"];
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int GetTestTypeFees(int TestTypeID)
        {
            decimal Fees = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT TestTypeFees AS Fees FROM  TestTypes
                            WHERE TestTypeID = @TestTypeID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Fees = (decimal)reader["Fees"];
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return Convert.ToInt32(Fees);
        }

    }

    public class Tests_Data
    {
       public static int AddNew(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
       {
            int ID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"INSERT INTO Tests (TestAppointmentID, TestResult, Notes,
                                                            CreatedByUserID)
                            Values (@TestAppointmentID, @TestResult, @Notes,
                                    @CreatedByUserID)
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);

            if (string.IsNullOrWhiteSpace(Notes))
                command.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("@Notes", Notes);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int NewUserID))
                {
                    ID = NewUserID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ID;
        }

        public static bool HasCompletedTheTest(int LocalAppID, int TestTypeID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT Tests.TestResult
                             FROM TestAppointments INNER JOIN
                             Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             WHERE LocalDrivingLicenseApplicationID = @LocalAppID AND
                             TestTypeID = @TestTypeID AND TestResult = 1;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }

    public class LDL_Applications_Data
    {
        public static DataTable Getall_LDL_Applications()
        {
            DataTable LDL_Applications = new DataTable();

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT LocalDrivingLicenseApplicationID AS 'L.D.L.AppID', ClassName AS DrivingClass , NationalNo, FullName, ApplicationDate
                                , PassedTestCount AS PassedTests, Status
                                FROM LocalDrivingLicenseApplications_View";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    LDL_Applications.Load(reader);
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return LDL_Applications;
        }

        public static int AddNew(int ApplicationID, int LicenseClassID)
        {
            int ID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"INSERT INTO LocalDrivingLicenseApplications (ApplicationID, LicenseClassID)
                            Values (@ApplicationID, @LicenseClassID)
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int NewUserID))
                {
                    ID = NewUserID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ID;
        }

        public static bool Update(int LocalAppID, int ClassID)
        {
            int rowseffected = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"UPDATE LocalDrivingLicenseApplications
                                SET LicenseClassID = @ClassID
                                WHERE LocalDrivingLicenseApplicationID = @LocalAppID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassID", ClassID);
            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }

        public static bool Find(int LDL_AppID, ref string ClassName, ref int PassedTests)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT ClassName, PassedTestCount
                            FROM LocalDrivingLicenseApplications_View
                             WHERE LocalDrivingLicenseApplicationID = @LDL_AppID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ClassName = (string)reader["ClassName"];
                    PassedTests = (int)reader["PassedTestCount"];
                    
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool Delete(int LocalAppID)
        {
            int rowseffected = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"BEGIN TRY

                            BEGIN TRANSACTION;
                            DECLARE @ApplicationID INT;
                            SELECT @ApplicationID = ApplicationID
                            FROM LocalDrivingLicenseApplications
                            WHERE LocalDrivingLicenseApplicationID = @LocalAppID;
                            IF @ApplicationID IS NULL
                                THROW 50001, 'Local Application not found', 1;
                            DELETE FROM LocalDrivingLicenseApplications
                            WHERE LocalDrivingLicenseApplicationID = @LocalAppID;
                            DELETE FROM Applications
                            WHERE ApplicationID = @ApplicationID;
                            COMMIT;

                            END TRY

                            BEGIN CATCH
                            IF @@TRANCOUNT > 0
                                ROLLBACK;
                            THROW;

                            END CATCH;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();
            }

            catch
            {
                rowseffected = 0;
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }


        public static bool HasActiveOrCompleteApplication(int ApplicantPersonID, int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT  Applications.ApplicantPersonID, Applications.ApplicationStatus, LocalDrivingLicenseApplications.LicenseClassID
                                FROM Applications INNER JOIN
                                LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                                WHERE ApplicantPersonID = @ApplicantPersonID AND ApplicationStatus IN(1,3) AND LicenseClassID = @LicenseClassID;";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();

            }

            catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }

    public class LicenseClasses_Data
    {
        public static DataTable GetallLicenseClassesNames()
        {
            DataTable Classes = new DataTable();

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT LicenseClassID, ClassName FROM LicenseClasses";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Classes.Load(reader);
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return Classes;
        }

        public static bool FindClassIDByClassID(int LicenseClassID, ref string ClassName, ref string ClassDescription,
            ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref decimal ClassFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM LicenseClasses WHERE 
                               LicenseClassID = @LicenseClassID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ClassName = (string)reader["ClassName"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    ClassFees = (decimal)reader["ClassFees"];

                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int FindClassIDByClassName(string ClassName)
        {
            int ClassID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT LicenseClassID FROM LicenseClasses
                                        WHERE ClassName = @ClassName;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassName", ClassName);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result?.ToString() ?? "", out int ID))
                {
                    ClassID = ID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ClassID;
        }

        public static int FindClassIDByLicenseID(int LicenseID)
        {
            int ClassID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT LicenseClasses.LicenseClassID
                             FROM LicenseClasses INNER JOIN
                             Licenses ON LicenseClasses.LicenseClassID = Licenses.LicenseClass
                             WHERE LicenseID = @LicenseID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result?.ToString() ?? "", out int ID))
                {
                    ClassID = ID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ClassID;
        }

        public static int GetLicenseClassValidityLength(int ClassID)
        {
            int ValidityLength = 0;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT DefaultValidityLength FROM LicenseClasses
                             WHERE LicenseClassID = @ClassID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassID", ClassID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result?.ToString() ?? "", out int Length))
                {
                    ValidityLength = Length;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ValidityLength;
        }

        public static int GetLicenseClassFees(int ClassID)
        {
            int ClassFees = 0;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT ClassFees FROM LicenseClasses
                             WHERE LicenseClassID = @ClassID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassID", ClassID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result?.ToString() ?? "", out int Fees))
                {
                    ClassFees = Fees;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ClassFees;
        }

    }

    public class Licenses_Data
    {
        public static DataTable GetPersonLicenses(int PersonID)
        {
            DataTable LicensesRecords = new DataTable();

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT Licenses.LicenseID AS 'Lic.ID', Licenses.ApplicationID AS 'App.ID',
                             LicenseClasses.ClassName AS 'Class Name', Licenses.IssueDate AS 'Issue Date',
                             Licenses.ExpirationDate AS 'Expiration Date', Licenses.IsActive AS 'Is Active'

                             FROM Applications INNER JOIN
                             Licenses ON Applications.ApplicationID = Licenses.ApplicationID INNER JOIN
                             LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID

                             WHERE ApplicantPersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    LicensesRecords.Load(reader);
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return LicensesRecords;
        }

        public static bool Find(int LicenseID, ref int AppID, ref int DriverID, ref int LicenseClass, ref DateTime IssueDate, ref DateTime ExpirationDate,
           ref string Notes, ref decimal PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM Licenses WHERE LicenseID = @LicenseID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    AppID = (int)reader["ApplicationID"];
                    DriverID = (int)reader["DriverID"];
                    LicenseClass = (int)reader["LicenseClass"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];

                    if (reader["Notes"] != DBNull.Value)
                        Notes = (string)reader["Notes"];
                    else
                        Notes = "No Notes";

                    PaidFees = (decimal)reader["PaidFees"];
                    IsActive = (bool)reader["IsActive"];
                    IssueReason = (byte)reader["IssueReason"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];

                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsExistLicenseByLocalAppID(int LocalAppID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT  Found = 1     
                             FROM Applications INNER JOIN
                             Licenses ON Applications.ApplicationID = Licenses.ApplicationID INNER JOIN
                             LocalDrivingLicenseApplications ON
                             Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                             WHERE LocalDrivingLicenseApplicationID = @LocalAppID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int FindLicenseIDByLocalAppID(int LocalAppID)
        {
            int LicenseID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT  LicenseID   
                             FROM Applications INNER JOIN
                             Licenses ON Applications.ApplicationID = Licenses.ApplicationID INNER JOIN
                             LocalDrivingLicenseApplications ON
                             Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                             WHERE LocalDrivingLicenseApplicationID = @LocalAppID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (Result != null)
                {
                    int.TryParse(Result.ToString(), out int ID);
                    LicenseID = ID;
                }
                else
                    LicenseID = 0;
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return LicenseID;
        }

        public static bool DeactivateLicense(int LicenseID)
        {
            int rowseffected = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"UPDATE Licenses
                                SET IsActive = 0
                                WHERE LicenseID = @LicenseID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }

        public static bool GetLicenseInfoForRenew(int LicenseID, ref decimal PaidFees, ref string Notes, ref int
                                                    CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT LicenseID, PaidFees, Notes, 
                                     CreatedByUserID
                             FROM    Licenses
                             WHERE LicenseID = @LicenseID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    PaidFees = Convert.ToDecimal(reader["PaidFees"]);

                    if (reader["Notes"] != DBNull.Value)
                        Notes = (string)reader["Notes"];
                    else
                        Notes = "";

                    CreatedByUserID = (int)reader["CreatedByUserID"];
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int AddNew(int AppID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
            string Notes, decimal PaidFees, bool IsActive, int IssueReason, int CreatedByUserID)
        {
            int ID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"INSERT INTO Licenses (ApplicationID, DriverID, LicenseClass, IssueDate, 
                                            ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)

                            Values (@AppID,@DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes,
                                    @PaidFees, @IsActive, @IssueReason, @CreatedByUserID)
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppID", AppID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

            if(string.IsNullOrWhiteSpace(Notes) || Notes == "No Notes")
                command.Parameters.AddWithValue("@Notes",DBNull.Value);
            else
                command.Parameters.AddWithValue("@Notes", Notes);

            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int NewUserID))
                {
                    ID = NewUserID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ID;
        }
    }

    public class InterNationalLicenses_Data
    {
        public static DataTable GetAllI_Licenses()
        {
            DataTable LicensesRecords = new DataTable();

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT InternationalLicenseID AS 'Int.License ID', ApplicationID,
                              DriverID, IssuedUsingLocalLicenseID AS 'L.License ID',
                              IssueDate, ExpirationDate, IsActive
                              FROM InternationalLicenses;";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    LicensesRecords.Load(reader);
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return LicensesRecords;
        }

        public static DataTable GetPersonInterationalLicenses(int PersonID)
        {
            DataTable LicensesRecords = new DataTable();

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT InternationalLicenses.InternationalLicenseID AS 'Int.License ID',
                             InternationalLicenses.ApplicationID, InternationalLicenses.IssuedUsingLocalLicenseID AS 'L.License ID',
                             InternationalLicenses.IssueDate, InternationalLicenses.ExpirationDate, 
                             InternationalLicenses.IsActive
                             FROM Drivers INNER JOIN
                             InternationalLicenses ON Drivers.DriverID = InternationalLicenses.DriverID
                             WHERE Drivers.PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    LicensesRecords.Load(reader);
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return LicensesRecords;
        }

        public static int AddNew(int AppID, int DriverID, int LocalLicenseID, DateTime IssueDate,
            DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int ID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"  Update InternationalLicenses 
                               set IsActive=0
                               where DriverID=@DriverID;

                            INSERT INTO InternationalLicenses (ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, 
                                            ExpirationDate, IsActive, CreatedByUserID)

                            Values (@AppID, @DriverID, @LocalLicenseID, @IssueDate, @ExpirationDate,
                                    @IsActive ,@CreatedByUserID)
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppID", AppID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LocalLicenseID", LocalLicenseID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int NewID))
                {
                    ID = NewID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ID;
        }

        public static bool IsLicenseExist(int LocalLicense)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT InternationalLicenseID FROM InternationalLicenses
                             WHERE IssuedUsingLocalLicenseID = @LocalLicense;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalLicense", LocalLicense);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int GetLicenseID(int LocalLicenseID)
        {
            int LicenseID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT InternationalLicenseID FROM InternationalLicenses
                             WHERE IssuedUsingLocalLicenseID = @LocalLicenseID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalLicenseID", LocalLicenseID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int ID))
                {
                    LicenseID = ID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return LicenseID;
        }
    }

    public class TestAppointments_Data
    {
        public static DataTable GetTestAppointments(int LocalAppID, int TestTypeID)
        {
            DataTable dtTestAppointments = new DataTable();


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT TestAppointmentID AS 'Appointment ID',
                            AppointmentDate AS 'Appointment Date',
                            PaidFees AS 'Paid Fees', IsLocked From TestAppointments

                                WHERE LocalDrivingLicenseApplicationID = @LocalAppID AND TestTypeID = @TestTypeID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtTestAppointments.Load(reader);
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return dtTestAppointments;
        }

        public static bool HasActiveAppointment(int LocalAppID, int TestTypeID)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM TestAppointments
                              WHERE LocalDrivingLicenseApplicationID = @LocalAppID
                                AND TestTypeID = @TestTypeID AND IsLocked = 0";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool HasReservtionAppointment(int LocalAppID)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT TestAppointmentID FROM TestAppointments
                              WHERE LocalDrivingLicenseApplicationID = @LocalAppID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool CloseTheAppointment(int TestAppointmentID)
        {
            int rowseffected = 0;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"UPDATE TestAppointments
                                Set IsLocked = 1
                                    WHERE TestAppointmentID = @TestAppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }

        public static int TrialsNumber(int LocalAppID, int TestTypeID)
        {
            int Trials = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT COUNT(*) Trials FROM TestAppointments
                              WHERE LocalDrivingLicenseApplicationID = @LocalAppID
                                AND TestTypeID = @TestTypeID
                                AND IsLocked = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                Trials = Convert.ToInt32(command.ExecuteScalar());              
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return Trials;
        }

        public static int AddNew(int TestTypeID, int LocalAppID,DateTime AppointmentDate, decimal PaidFees,
                    int CreatedByUserID, bool IsLocked, int RetakeTestAppID)
        {
            int ID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"INSERT INTO TestAppointments (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate,
                                                            PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID)
                            Values (@TestTypeID, @LocalAppID, @AppointmentDate,
                                    @PaidFees, @CreatedByUserID, @IsLocked, @RetakeTestAppID)
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);

            if (RetakeTestAppID == 0)
                command.Parameters.AddWithValue("@RetakeTestAppID", DBNull.Value);
            else
                command.Parameters.AddWithValue("@RetakeTestAppID", RetakeTestAppID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int NewUserID))
                {
                    ID = NewUserID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ID;
        }
            
        public static bool Update(int TestAppointmentID, DateTime AppointmentDate)
        {
            int rowseffected = 0;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"Update TestAppointments
                                Set AppointmentDate = @AppointmentDate
                                    Where TestAppointmentID = @TestAppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);

            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();

            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }
            
            return (rowseffected > 0);
        }

        public static bool Find(int TestAppointmentID, ref int TestTypeID, ref int LocalAppID,
                   ref DateTime AppointmentDate, ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestAppID)
        {
            bool isFound = false;
            
            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM TestAppointments
                              WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestTypeID = (int)reader["TestTypeID"];
                    LocalAppID = (int)reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"] != DBNull.Value)
                        RetakeTestAppID = (int)reader["RetakeTestApplicationID"];
                    else
                        RetakeTestAppID = 0;
                }

                reader.Close();
     
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }                            

    public class Drivers_Data
    {
        public static DataTable GetAllDrivers()
        {
            DataTable dtDrivers = new DataTable();


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM Drivers_View";

            SqlCommand command = new SqlCommand(query, connection);


            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtDrivers.Load(reader);
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return dtDrivers;
        }

        public static bool Find(int DriverID, ref int personID, ref int createdByUserID, ref DateTime createdDate)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM Drivers
                                        WHERE DriverID = @DriverID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    personID = (int)reader["PersonID"];
                    createdByUserID = (int)reader["CreatedByUserID"];
                    createdDate = (DateTime)reader["CreatedDate"];
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int GetDriverID(int PersonID)
        {
            int DriverID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT DriverID FROM Drivers
                                        WHERE PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result?.ToString() ?? "", out int ID))
                {
                    DriverID = ID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return DriverID;
        }

        public static bool IsDriverExist(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT DriverID FROM Drivers
                                        WHERE PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int AddNew(int PersonID, int CreatedByUserID)
        {
            int ID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate)

                            Values (@PersonID,@CreatedByUserID, GETDATE())
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int NewUserID))
                {
                    ID = NewUserID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ID;
        }
    }

    public class DetainedLicenses_Data
    {
        public static DataTable GetDetainedLicenses()
        {
            DataTable dtDetainedLicenses = new DataTable();


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT DetainedLicenses.DetainID, DetainedLicenses.LicenseID,
                             DetainedLicenses.DetainDate, DetainedLicenses.IsReleased, DetainedLicenses.FineFees,
                             DetainedLicenses.ReleaseDate, People.NationalNo,
                             CONCAT_WS(' ',People.FirstName, People.SecondName, People.ThirdName, People.LastName) AS 'Full Name',
                             DetainedLicenses.ReleaseApplicationID AS 'Release App.ID'
                             
                             FROM DetainedLicenses INNER JOIN
                             Licenses ON DetainedLicenses.LicenseID = Licenses.LicenseID INNER JOIN
                             Drivers ON Licenses.DriverID = Drivers.DriverID INNER JOIN
                             People ON Drivers.PersonID = People.PersonID;";

            SqlCommand command = new SqlCommand(query, connection);


            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtDetainedLicenses.Load(reader);
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return dtDetainedLicenses;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
             bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT DetainID FROM DetainedLicenses
                                        WHERE LicenseID = @LicenseID AND IsReleased = 0;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int GetDetainID(int LicenseID)
        {
            int DetainID = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT DetainID FROM DetainedLicenses
                             WHERE LicenseID = @LicenseID AND IsReleased = 0;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int ID))
                {
                    DetainID = ID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return DetainID;
        }

        public static int GetDetainFees(int LicenseID)
        {
            int DetainFees = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT FineFees FROM DetainedLicenses
                             WHERE LicenseID = @LicenseID AND IsReleased = 0;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (decimal.TryParse(Result.ToString(), out decimal Fees))
                {
                    DetainFees = (int)Fees;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return DetainFees;
        }

        public static int GetPersonDetainRecords(int LicenseID)
        {
            int PersonDetainRecords = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT COUNT(*) AS Records FROM DetainedLicenses
                             WHERE LicenseID = @LicenseID AND IsReleased = 1;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if(string.IsNullOrWhiteSpace(Result.ToString()))
                {
                    PersonDetainRecords = 0;
                }
                else
                {
                    if (int.TryParse(Result.ToString(), out int ID))
                    {
                        PersonDetainRecords = ID;
                    }
                }

                
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return PersonDetainRecords;
        }

        public static int AddDetainLicense(int LicenseID, DateTime DetainDate, decimal FineFees,
            int CreatedByUserID)
        {
            int DetainLicense = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"INSERT INTO DetainedLicenses (LicenseID, DetainDate, FineFees, CreatedByUserID,
                                                        IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID)

                            Values (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased, @ReleaseDate,
                                @ReleasedByUserID, @ReleaseApplicationID)
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@FineFees", FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsReleased", false);
            command.Parameters.AddWithValue("@ReleaseDate", DBNull.Value);
            command.Parameters.AddWithValue("@ReleasedByUserID", DBNull.Value);
            command.Parameters.AddWithValue("@ReleaseApplicationID", DBNull.Value);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (int.TryParse(Result.ToString(), out int NewDetainRecordID))
                {
                    DetainLicense = NewDetainRecordID;
                }
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return DetainLicense;
        }

        public static bool ReleaseLicense(int LicenseID, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int rowseffected = 0;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"Update DetainedLicenses
                                Set IsReleased = @IsReleased,
                                    ReleaseDate = @ReleaseDate,
                                    ReleasedByUserID = @ReleasedByUserID,
                                    ReleaseApplicationID = @ReleaseApplicationID
                                    WHERE LicenseID = @LicenseID and IsReleased = 0;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@IsReleased", true);
            command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                rowseffected = command.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowseffected > 0);
        }
    }

    public class BasicLocalApplicationsInfo_View
    {                            
        public static bool Find(int  LocalAppID, ref int AppID, ref string Status, ref decimal Fees, ref int PersonID,
            ref string AppType, ref string Applicant, ref DateTime AppDate, ref DateTime LastStatusUpdate, ref string UserName)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM BasicLocalApplicationsInfo_View
                                Where LocalDrivingLicenseApplicationID = @LocalAppID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    AppID = (int)reader["ApplicationID"];
                    Status = (string)reader["Status"];
                    Fees = (decimal)reader["Fees"];
                    AppType = (string)reader["ApplicationTypeTitle"];
                    PersonID = (int)reader["PersonID"];
                    Applicant = (string)reader["FullName"];
                    AppDate = (DateTime)reader["ApplicationDate"];
                    LastStatusUpdate = (DateTime)reader["LastStatusDate"];
                    UserName = (string)reader["UserName"];
                                       
                }

                reader.Close();
            }

            catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool Find(int LocalAppID, ref string ClassName, ref string FullName, ref int PersonID)

        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT FullName, ClassName, PersonID FROM BasicLocalApplicationsInfo_View
                            WHERE LocalDrivingLicenseApplicationID = @LocalAppID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    ClassName = (string)reader["ClassName"];
                    FullName = (string)reader["FullName"];
                    PersonID = (int)reader["PersonID"];
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }


    }

    public class LocalDriverLicensesInfo_View
    {
        public static bool FindByLocalAppID(int LocalAppID, ref string ClassName, ref string FullName, ref int LicenseID, ref string NationalNo 
            , ref string Gender, ref DateTime IssueDate, ref string IssueReason, ref string Notes,
            ref bool IsActive, ref DateTime DateOfBirth, ref int DriverID, ref DateTime ExpirationDate, ref string ImagePath)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM LocalDriverLicenseInfo_View
                             WHERE LocalDrivingLicenseApplicationID = @LocalAppID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ClassName = (string)reader["ClassName"];
                    FullName = (string)reader["FullName"];
                    LicenseID = (int)reader["LicenseID"];
                    NationalNo = (string)reader["NationalNo"];
                    Gender = (string)reader["Gender"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    IssueReason = (string)reader["IssueReason"];

                    string notes = reader["Notes"] as string;
                    Notes = string.IsNullOrWhiteSpace(notes) ? "No Notes" : notes;

                    IsActive = (bool)reader["IsActive"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    DriverID = (int)reader["DriverID"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)reader["ImagePath"];
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }
    
    
        public static bool FindByLicenseID(int LicenseID, ref string ClassName, ref string FullName, ref int LocalAppID, ref string NationalNo
            , ref string Gender, ref DateTime IssueDate, ref string IssueReason, ref string Notes,
            ref bool IsActive, ref DateTime DateOfBirth, ref int DriverID, ref DateTime ExpirationDate, ref string ImagePath)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM LocalDriverLicenseInfo_View
                             WHERE LicenseID = @LicenseID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ClassName = (string)reader["ClassName"];
                    FullName = (string)reader["FullName"];
                    LocalAppID = (int)reader["LocalDrivingLicenseApplicationID"];
                    NationalNo = (string)reader["NationalNo"];
                    Gender = (string)reader["Gender"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    IssueReason = (string)reader["IssueReason"];

                    string notes = reader["Notes"] as string;
                    Notes = string.IsNullOrWhiteSpace(notes) ? "No Notes" : notes;

                    IsActive = (bool)reader["IsActive"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    DriverID = (int)reader["DriverID"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)reader["ImagePath"];
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static bool IsExistLicense(int LicenseID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT LicenseID FROM LocalDriverLicenseInfo_View
                             WHERE LicenseID = @LicenseID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

    }

    public class InternationalDriverLicensesInfo_View
    {
        public static bool FindByLocalLicenseID(int LocalLicenseID, ref int Int_LicenseID, ref int AppID, ref string FullName, ref string NationalNo
     , ref string Gender, ref DateTime IssueDate,
     ref bool IsActive, ref DateTime DateOfBirth, ref int DriverID, ref DateTime ExpirationDate, ref string ImagePath)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM InternationalDriverLicensesInfo_View
                             WHERE LocalLicenseID = @LocalLicenseID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalLicenseID", LocalLicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    FullName = (string)reader["FullName"];
                    Int_LicenseID = (int)reader["LicenseID"];
                    NationalNo = (string)reader["NationalNo"];
                    Gender = (string)reader["Gender"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    AppID = (int)reader["ApplicationID"];
                    IsActive = (bool)reader["IsActive"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    DriverID = (int)reader["DriverID"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    ImagePath = (string)reader["ImagePath"];
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool FindByLicenseID(int Int_LicenseID, ref int AppID, ref string FullName, ref int LicenseID, ref string NationalNo
     , ref string Gender, ref DateTime IssueDate,
     ref bool IsActive, ref DateTime DateOfBirth, ref int DriverID, ref DateTime ExpirationDate, ref string ImagePath)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(DAL_Settings.ConnectionString);

            string query = @"SELECT * FROM InternationalDriverLicensesInfo_View
                             WHERE InternationalLicenseID = @Int_LicenseID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Int_LicenseID", Int_LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    FullName = (string)reader["FullName"];
                    Int_LicenseID = (int)reader["InternationalLicenseID"];
                    LicenseID = (int)reader["LocalLicenseID"];
                    NationalNo = (string)reader["NationalNo"];
                    Gender = (string)reader["Gender"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    AppID = (int)reader["ApplicationID"];
                    IsActive = (bool)reader["IsActive"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    DriverID = (int)reader["DriverID"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)reader["ImagePath"];
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

    }

}