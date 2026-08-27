using DVLD_DAL;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace DVLD_BLL
{

    public enum enApplicationTypeID
    {
        NewLocalDrivingLicenseService = 1,
        RenewDrivingLicenseService,
        Replacement_ForA_LostDrivingLicense,
        Replacement_ForA_DamagedDrivingLicense,
        ReleaseDetainedDrivingLicsense,
        NewIntemationalLicense,
        RetakeTest
    }
    public enum enApplicationStatus { New = 1, Cancelled, Completed }
    public enum enLicensesClassesID
    {
        Class1_SmallMotorcycle = 1,
        Class2_HeavyMotorcycleLicense,
        Class3_Ordinarydrivinglicense,
        Class4_Commercial,
        Class5_Agricultural,
        Class6_Smallandmediumbus,
        Class7_Truckandheavyvehicle,
    }
    public enum enDetainMode
    {
        Detain,
        Release
    }
    public enum enIssueReason { FirstTime = 1, Renew, DamagedReplacement, LostReplacement }




    public class clsPeople
    {
        enum enMode { Update, AddNew}
        enMode Mode = enMode.AddNew;

        public int PersonID {  get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int NationalityCountryID { get; set; }
        public string CountryName { get; private set; }
        public int Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }

        public clsPeople()
        {
            this.PersonID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.MinValue;
            this.NationalityCountryID = -1;
            this.Gendor = -1;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.ImagePath = "";

            Mode = enMode.AddNew;
        }

        private clsPeople(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
                            DateTime DateOfBirth, int NationalityCountryID, string CountryName, int Gendor, string Address, string Phone,
                            string Email, string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.NationalityCountryID = NationalityCountryID;
            this.CountryName = CountryName;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.ImagePath = ImagePath;

            Mode = enMode.Update;
        }

        private bool _AddNew()
        {
            this.PersonID = People_Data.AddNew(this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth,
                                        this.NationalityCountryID, this.Gendor, this.Address, this.Phone, this.Email, this.ImagePath);
            if (PersonID > 0)
            {
                Mode = enMode.Update;
                return true;
            }
            else
                return false;
        }

        private bool _Update()
        {
            return People_Data.Update(this.PersonID, this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName,
                                    this.DateOfBirth, this.NationalityCountryID, this.Gendor, this.Address,
                                    this.Phone, this.Email, this.ImagePath);
        }

        public static clsPeople Find(int PersonID)
        {
            string NationalNo = "";
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            DateTime DateOfBirth = DateTime.MinValue;
            int NationalityCountryID = -1;
            string CountryName = "";
            int Gendor = -1;
            string Address = "";
            string Phone = "";
            string Email = "";
            string ImagePath = "";

            if (People_Data.Find(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref NationalityCountryID,
                    ref CountryName, ref Gendor, ref Address, ref Phone, ref Email, ref ImagePath))
            {
                return new clsPeople(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, NationalityCountryID,
                CountryName, Gendor, Address, Phone, Email, ImagePath);
            }
            else
            {
                return null;
            }

        }

        public static clsPeople Find(string NationalNo)
        {
            int PersonID = -1;
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            DateTime DateOfBirth = DateTime.MinValue;
            int NationalityCountryID = -1;
            string CountryName = "";
            int Gendor = -1;
            string Address = "";
            string Phone = "";
            string Email = "";
            string ImagePath = "";

            if (People_Data.Find(NationalNo,ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref NationalityCountryID,
                    ref CountryName, ref Gendor, ref Address, ref Phone, ref Email, ref ImagePath))
            {
                return new clsPeople(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, NationalityCountryID,
                CountryName, Gendor, Address, Phone, Email, ImagePath);
            }
            else
            {
                return null;
            }

        }

        public static int FindByAppID(int AppID)
        {
            return People_Data.FindByAppID(AppID);
        }

        public static int FindByLocalAppID(int LocalAppID)
        {
            return People_Data.FindByLocalAppID(LocalAppID);
        }

        public static int FindByLicenseID(int LicenseID)
        {
            return People_Data.FindByLicenseID(LicenseID);
        }

        public static bool Delete(int PersonID, out string ImagePath)
        {
            
            return People_Data.Delete(PersonID, out ImagePath);
        }

        public static DataTable PeopleList()
        {
            return People_Data.GetallPeople();
        }

        public static DataTable CountriesList()
        {
            return People_Data.GetallCountries();
        }

        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.SecondName + " " + this.ThirdName + " " + this.LastName;
            }
        }

        public string GenderText
        {
           get
           {
                return (this.Gendor == 0)? "Male" : "Female";
           }      
        }

        public static bool isNationalNoExist(string NationalNo, int PersonID = -1)
        {
            return People_Data.isNationalNoExist(NationalNo, PersonID);
        }

        public static bool isNationalNoExist(string NationalNo)
        {
            return People_Data.isNationalNoExist(NationalNo);
        }

        public static bool isPersonExist(int PersonID)
        {
            return People_Data.isPersonExist(PersonID);
        }

        public static bool isValidEmail(string Email)
        {
            if (!string.IsNullOrEmpty(Email) || !string.IsNullOrWhiteSpace(Email))
            {
                try
                {
                MailAddress address = new MailAddress(Email);

                return address.Address == Email;
                }

                catch
                {
                return false;
                }

            }

            return true;
            
        }


       public virtual bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    return _AddNew();

                case enMode.Update:
                    return _Update();
            }

            return false;
        }

    }

    public class clsUser
    {
        private enum enMode { Update, AddNew}
        private enMode Mode = enMode.AddNew;


        public int UserID { get; set; }
        public clsPeople PersonInfo {  get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public static DataTable GetallUsers()
        {
            return User_Data.GetallUsers();
        }

        private bool _AddNew()
        {
            this.UserID = User_Data.AddNew(this.PersonInfo.PersonID, this.UserName, this.Password, this.IsActive);

           if(UserID > 0)
           {
                Mode = enMode.Update;
                return true;
           }

            return false;
        }

        private bool _Update()
        {
            return User_Data.Update(this.UserID, this.UserName, this.Password, this.IsActive);
        }

        public clsUser()
        {
            this.UserID = -1;
            this.PersonInfo = null;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;

            Mode = enMode.AddNew;
        }

        private clsUser(clsPeople PersonInfo, int UserID, string UserName, string Password, bool IsActive)
        {
            this.PersonInfo = PersonInfo;
            this.UserID = UserID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            Mode = enMode.Update;
        }

        public static bool Delete(int UserID)
        {
            return User_Data.Delete(UserID);
        }

        public static clsUser Find(string UserName, string Password)
        {
            int UserID = -1;
            int PersonID = -1;
            clsPeople PersonInfo = null;
            bool IsActive = false;

            if(User_Data.Find(UserName, Password,ref UserID,ref PersonID,ref IsActive))
            {
                PersonInfo = clsPeople.Find(PersonID);
                return new clsUser(PersonInfo, UserID, UserName, Password, IsActive);
            }

            return null;
                
        }

        public static clsUser Find(int UserID)
        {
            int PersonID = -1;
            string UserName = "";
            string Password = "";
            clsPeople PersonInfo = null;
            bool IsActive = false;

            if (User_Data.Find(UserID, ref PersonID, ref UserName, ref Password, ref IsActive))
            {
                PersonInfo = clsPeople.Find(PersonID);
                return new clsUser(PersonInfo, UserID, UserName, Password, IsActive);
            }

            return null;
        }

        public static bool IsUserExistByPersonID(int PersonID)
        {
            return User_Data.IsUserExistByPersonID(PersonID);
        }

        public static bool IsUserExistByUserID(int UserID)
        {
            return User_Data.IsUserExistByUserID(UserID);
        }

        public static bool IsUserNameExist(string UserName)
        {
            return User_Data.IsUserNameExist(UserName);
        }

        public static bool IsUserNameExist(string UserName, int UserID)
        {
            return User_Data.IsUserNameExist(UserName, UserID);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    return _AddNew();

                case enMode.Update:
                    return _Update();

                
            }

            return false;
        }
    }

    public class clsApplicationTypes
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal Fees { get; set; }


        clsApplicationTypes(int ID, string Title, decimal Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Fees = Fees;
        }

        public static DataTable GetallApplicationTypes()
        {
            return ApplicationsTypes_Data.GetallApplicationsTypes();
        }

        private bool _Update()
        {
            return ApplicationsTypes_Data.Update(this.ID, this.Title, this.Fees);
        }

        public static int GetAppTypeFees(enApplicationTypeID AppTypeID)
        {
            return ApplicationsTypes_Data.GetAppTypeFees((int)AppTypeID);
        }

        public static clsApplicationTypes Find(int ID)
        {
            string Title = "";
            decimal Fees = -1;

            if(ApplicationsTypes_Data.Find(ID, ref Title, ref Fees))
            {
                return new clsApplicationTypes(ID, Title, Fees);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            return _Update();
        }
    }

    public class clsApplications
    {

        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public int ApplicationID { get; set; }
        public int ApplicationTypeID { get; set; }
        public int ApplicationStatus { get; set; }
        public decimal PaidFees { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime LastStatusDate { get; set; }

        public clsApplications()
        {
            PersonID = 0;
            CreatedByUserID = 0;
            ApplicationTypeID = -1;
            ApplicationStatus = 0;
            PaidFees = 0;
            ApplicationDate = DateTime.Now;
            LastStatusDate = DateTime.Now;
        }

        private clsApplications(int ApplicationID, int PersonID, DateTime ApplicationDate,
           int ApplicationTypeID, int ApplicationStatus, DateTime LastStatusDate,
           decimal PaidFees, int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.PersonID = PersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
        }

        private bool _AddNew()
        {
            this.ApplicationID = Applications_Data.AddNew(this.PersonID, this.ApplicationTypeID, this.ApplicationStatus,
                                        this.PaidFees, this.ApplicationDate, this.CreatedByUserID);

            return (ApplicationID > 0);
        }

        public static bool Cancel(int LDLAppID)
        {
            return Applications_Data.CancelApplication(LDLAppID);
        }

        public static bool Complete(int PersonID, int ApplicationTypeID)
        {
            return Applications_Data.CompleteApplication(PersonID, ApplicationTypeID);
        }

        public static bool Complete(int ApplicationID)
        {
            return Applications_Data.CompleteApplication(ApplicationID);
        }

        public static clsApplications FindByAppID(int ApplicationID)
        {
            int PersonID = 0;
            DateTime ApplicationDate = DateTime.MinValue;
            int ApplicationTypeID = 0;
            int ApplicationStatus = 0;
            DateTime LastStatusDate = DateTime.MinValue;
            decimal PaidFees = 0;
            int CreatedByUserID = 0;

            if (Applications_Data.Find(ApplicationID, ref PersonID, ref ApplicationDate, ref ApplicationTypeID, ref
                ApplicationStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUserID))
            {
                return new clsApplications(ApplicationID, PersonID, ApplicationDate, ApplicationTypeID,
                    ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);
            }
            else
                return null;
        }

        public static clsApplications FindByLocalAppID(int LocalAppID)
        {
            int ApplicationID = 0;
            int PersonID = 0;
            DateTime ApplicationDate = DateTime.MinValue;         
            int ApplicationTypeID = 0;
            int ApplicationStatus = 0;
            DateTime LastStatusDate = DateTime.MinValue;        
            decimal PaidFees = 0;
            int CreatedByUserID = 0;

            if (Applications_Data.Find(LocalAppID, ref ApplicationID, ref PersonID, ref ApplicationDate, ref ApplicationTypeID, ref
                ApplicationStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUserID))
            {
                return new clsApplications(ApplicationID, PersonID, ApplicationDate, ApplicationTypeID,
                    ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);
            }
            else
                return null;
        }

        public virtual bool Save()
        {
            return _AddNew();
        }
        
    }

    public class clsTestTypes
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description {  get; set; }
        public decimal Fees { get; set; }


        clsTestTypes(int ID, string Title, string Description, decimal Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Description = Description;
            this.Fees = Fees;
        }

        public static DataTable GetallTestTypes()
        {
            return TestTypes_Data.GetallTestTypes();
        }

        public static int GetTestTypeFees(int TestTypeID)
        {
            return TestTypes_Data.GetTestTypeFees(TestTypeID);
        }

        private bool _Update()
        {
            return TestTypes_Data.Update(this.ID, this.Title, this.Description, this.Fees);
        }

        public static clsTestTypes Find(int ID)
        {
            string Title = "";
            string Description = "";
            decimal Fees = -1;

            if (TestTypes_Data.Find(ID, ref Title, ref Description, ref Fees))
            {
                return new clsTestTypes(ID, Title, Description, Fees);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            return _Update();
        }

    }

    public class clsTests
    {
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }

        private bool _AddNew()
        {
            this.TestID = Tests_Data.AddNew(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        
            return (this.TestID > 0);
        }

        public static bool HasCompletedTheTest(int LocalAppID, int TestTypeID)
        {
            return Tests_Data.HasCompletedTheTest(LocalAppID, TestTypeID);
        }

        public bool Save()
        {
            return _AddNew();
        }
    }

    public class clsLDL_Application : clsApplications
    {
        public int LocalAppID { get; set; }
        public int LicenseClassID { get; set; }

        
        public static DataTable Getall_LDL_Applications()
        {
            return LDL_Applications_Data.Getall_LDL_Applications();
        }

        public static bool HasActiveOrCompleteApplication(int ApplicantPersonID, int LicenseClassID)
        {
            return (LDL_Applications_Data.HasActiveOrCompleteApplication(ApplicantPersonID, LicenseClassID));
        }

        private bool _AddNew()
        {
            this.LocalAppID = LDL_Applications_Data.AddNew(ApplicationID, this.LicenseClassID);

            return (this.LocalAppID > 0);
        }

        public static bool Update(int LocalAppID, int LicenseClassID)
        {
            return LDL_Applications_Data.Update(LocalAppID, LicenseClassID);
        }

        public static bool Find(int LDL_AppID, ref string ClassName, ref int PassedTests)
        {
            return LDL_Applications_Data.Find(LDL_AppID, ref ClassName, ref PassedTests);
        }

        public static bool Delete(int LocalAppID)
        {
            return LDL_Applications_Data.Delete(LocalAppID);
        }

        public override bool Save()
        {
            if (!base.Save())
                return false;

            return this._AddNew();
        }
    }

    public class clsLicenseClasses
    {
        public int LicenseClassID { get; set; }
        public string ClassName {get; set;}
        public string ClassDescription {get; set;}
        public byte MinimumAllowedAge {get; set;}
        public byte DefaultValidityLength {get; set;}
        public decimal ClassFees { get; set; }

        clsLicenseClasses(int LicenseClassID, string ClassName, string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, decimal ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
        }

        public static DataTable GetallLicenseClassesNames()
        {
            return LicenseClasses_Data.GetallLicenseClassesNames();
        }

        public static clsLicenseClasses FindClassIDByClassID(int LicenseClassID)
        {
            string ClassName = "", ClassDescription = "";
            byte MinimumAllowedAge = 0, DefaultValidityLength = 0;
            decimal ClassFees = 0;

            if(LicenseClasses_Data.FindClassIDByClassID(LicenseClassID, ref ClassName, ref
                ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))

                return new clsLicenseClasses(LicenseClassID, ClassName, ClassDescription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFees);
            else
                return null;
        }

        public static int FindClassIDByClassName(string ClassName)
        {
            return LicenseClasses_Data.FindClassIDByClassName(ClassName);
        }

        public static int FindClassIDByLicenseID(int LicenseID)
        {
            return LicenseClasses_Data.FindClassIDByLicenseID(LicenseID);
        }

        public static int GetLicenseClassValidityLength(int ClassID)
        {
            return LicenseClasses_Data.GetLicenseClassValidityLength(ClassID);
        }

        public static int GetLicenseClassFees(int ClassID)
        {
            return LicenseClasses_Data.GetLicenseClassFees(ClassID);
        }

    }

    public class clsLicenses
    {

        public int LicenseID { get; set; }
        public int AppID { get; set; }
        public int DriverID { get; set; }
        public clsDrivers DriverInfo { get; set; }
        public int LicenseClass { get; set; }
        public clsLicenseClasses LicenseClassInfo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }
        public string IssueReasonText
        {
            get
            {
                return clsLicenses.GetIssueReasonText(this.IssueReason);
            }
        }
        public int CreatedByUserID { get; set; }

        public clsLicenses()
        {

        }

        clsLicenses(int licenseID, int appID, int driverID, int licenseClass, DateTime issueDate, DateTime expirationDate,
            string notes, decimal paidFees, bool isActive, enIssueReason issueReason, int createdByUserID)
        {
            LicenseID = licenseID;
            AppID = appID;
            DriverID = driverID;
            DriverInfo = clsDrivers.FindDriver(DriverID);
            LicenseClass = licenseClass;
            LicenseClassInfo = clsLicenseClasses.FindClassIDByClassID(LicenseClass);
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = issueReason;
            CreatedByUserID = createdByUserID;
        }

        private bool _AddNew()
        {
            this.LicenseID = Licenses_Data.AddNew(this.AppID, this.DriverID, this.LicenseClass, this.IssueDate, this.ExpirationDate,
             this.Notes, this.PaidFees, this.IsActive, (int)this.IssueReason, this.CreatedByUserID);

            return (LicenseID > 0);
        }

        public static clsLicenses Find(int LicenseID)
        {
            int AppID = 0, DriverID = 0, LicenseClass = 0, CreatedByUserID = 0;
            DateTime IssueDate = DateTime.MinValue, ExpirationDate = DateTime.MinValue;
            string Notes = "";
            decimal PaidFees = 0;
            bool IsActive = false;
            byte IssueReason = 0;


            if (Licenses_Data.Find(LicenseID, ref AppID, ref DriverID, ref LicenseClass, ref IssueDate, ref ExpirationDate, ref Notes
                , ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))

                return new clsLicenses(LicenseID, AppID, DriverID, LicenseClass, IssueDate, ExpirationDate,
                    Notes, PaidFees, IsActive,(enIssueReason) IssueReason, CreatedByUserID);
            else
                return null;
        }

        public static bool IsExistLicenseByLocalAppID(int LocalAppID)
        {
            return Licenses_Data.IsExistLicenseByLocalAppID(LocalAppID);
        }

        public static int FindLicenseIDByLocalAppID(int LocalAppID)
        {
            return Licenses_Data.FindLicenseIDByLocalAppID(LocalAppID);
        }

        public static bool DeactivateLicense(int LicenseID)
        {
            return Licenses_Data.DeactivateLicense(LicenseID);
        }

        public static bool GetLicenseInfoForRenew(int LicenseID, ref decimal PaidFees, ref string Notes, ref int
                                                    CreatedByUserID)
        {
            return Licenses_Data.GetLicenseInfoForRenew(LicenseID, ref PaidFees, ref Notes, ref CreatedByUserID);
        }

        public static DataTable GetPersonLicenses(int PersonID)
        {
            return Licenses_Data.GetPersonLicenses(PersonID);
        }

        public static string GetIssueReasonText(enIssueReason IssueReason)
        {

            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }

        public virtual bool Save()
        {
            return _AddNew();
        }
    }

    public class clsInterNationalLicenses : clsLicenses
    {
        public int InternationalLicenseID { get; set; }

        public int LocalLicenseID { get; set; }

        private bool _AddNew()
        {
            this.InternationalLicenseID = InterNationalLicenses_Data.AddNew(this.AppID, this.DriverID, this.LocalLicenseID,
                this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);

            return (InternationalLicenseID > 0);
        }

        public static bool IsLicenseExist(int LocalLicenseID)
        {
            return InterNationalLicenses_Data.IsLicenseExist(LocalLicenseID);
        }

        public static int GetLicenseID(int LocalLicenseID)
        {
            return InterNationalLicenses_Data.GetLicenseID(LocalLicenseID);
        }

        public static DataTable GetAllI_Licenses()
        {
            return InterNationalLicenses_Data.GetAllI_Licenses();
        }

        public static DataTable GetPersonInterationalLicenses(int PersonID)
        {
            return InterNationalLicenses_Data.GetPersonInterationalLicenses(PersonID);
        }

        public override bool Save()
        {
            return _AddNew();
        }
    }

    public class clsTestAppointments
    {
        enum enMode { Addnew, Update}
        enMode Mode = enMode.Addnew;

        public int AppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalAppID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }

        public int RetakeTestAppID { get; set; }

        public clsTestAppointments()
        {
            this.AppointmentID = 0;
            this.TestTypeID = 0;
            this.LocalAppID = 0;
            this.AppointmentDate = DateTime.MinValue;
            this.PaidFees = 0;
            this.CreatedByUserID = 0;
            this.IsLocked = false;

            Mode = enMode.Addnew;
        }

        clsTestAppointments(int TestAppointmentID, int TestTypeID, int LocalAppID,
                   DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestAppID)
        {
            this.AppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalAppID = LocalAppID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestAppID = RetakeTestAppID;

            Mode = enMode.Update;
        }

        private bool _Addnew()
        {
            this.AppointmentID = TestAppointments_Data.AddNew(this.TestTypeID, this.LocalAppID, this.AppointmentDate
                        , this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestAppID);
            if (this.AppointmentID > 0)
            {
                Mode = enMode.Update;
                return true;
            }
            else
                return false;
        }

        private bool _Update()
        {
            return TestAppointments_Data.Update(AppointmentID, AppointmentDate);
        }

        public static clsTestAppointments Find(int TestAppointmentID)
        {
            int TestTypeID = 0;
            int LocalAppID = 0;
            DateTime AppointmentDate = DateTime.Today;
            decimal PaidFees = 0;
            int CreatedByUserID = 0;
            bool IsLocked = false;
            int RetakeTestAppID = 0;

            if (TestAppointments_Data.Find(TestAppointmentID, ref TestTypeID, ref LocalAppID, ref AppointmentDate,
                ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestAppID))
            {
                return new clsTestAppointments(TestAppointmentID, TestTypeID, LocalAppID, AppointmentDate, PaidFees,
                    CreatedByUserID, IsLocked, RetakeTestAppID);
            }
            else
                return null;

        }

        public static DataTable GetTestAppointments(int LocalAppID, int TestTypeID)
        {
            return TestAppointments_Data.GetTestAppointments(LocalAppID, TestTypeID);
        }

        public static int TrialsNumber(int LocalAppID, int TestTypeID)
        {
           return TestAppointments_Data.TrialsNumber(LocalAppID, TestTypeID);
        }

        public static bool HasActiveAppointment(int LocalAppID, int TestTypeID)
        {
            return TestAppointments_Data.HasActiveAppointment(LocalAppID, TestTypeID);
        }

        public static bool HasReservtionAppointment(int LocalAppID)
        {
            return TestAppointments_Data.HasReservtionAppointment(LocalAppID);
        }

        public static bool CloseTheAppointment(int AppointmentID)
        {
            return TestAppointments_Data.CloseTheAppointment(AppointmentID);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.Addnew:
                    return _Addnew();

                case enMode.Update:
                    return _Update();
            }

            return false;
        }
    }

    public class clsDrivers
    {
        public int DriverID { get; set; }

        public int PersonID { get; set;  }

        public clsPeople PersonInfo { get; set; }

        public int CreatedByUserID { get; set; }

        public DateTime CreatedDate { get; set; }

       public clsDrivers()
       {

       }

        clsDrivers(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            this.DriverID = driverID;
            this.PersonID = personID;
            this.PersonInfo = clsPeople.Find(PersonID);
            this.CreatedByUserID = createdByUserID;
            this.CreatedDate = createdDate;
        }

        private bool _AddNewDriver()
        {
            this.DriverID = Drivers_Data.AddNew(this.PersonID, this.CreatedByUserID);

            return (DriverID > 0);
        }

        public static clsDrivers FindDriver(int DriverID)
        {
            int personID = 0, createdByUserID = 0;
            DateTime createdDate = DateTime.Now;

            if (Drivers_Data.Find(DriverID, ref personID, ref createdByUserID, ref createdDate))
                return new clsDrivers(DriverID, personID, createdByUserID, createdDate);
            else
                return null;
        }

        public static DataTable GetAllDrivers()
        {
            return Drivers_Data.GetAllDrivers();
        }

        public static int GetDriverID(int PersonID)
        {
            return Drivers_Data.GetDriverID(PersonID);
        }

        public bool Save()
        {
            return _AddNewDriver();
        }
    }

    public class clsDetainedLicenses
    {
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate {get; set;}
        public decimal FineFees {get; set;}
        public int CreatedByUserID {get; set;}
        public bool IsReleased {get; set;}
        public DateTime ReleaseDate {get; set;}
        public int ReleasedByUserID {get; set;}
        public int ReleaseApplicationID { get; set; }

        public static DataTable GetDetainedLicenses()
        {
            return DetainedLicenses_Data.GetDetainedLicenses();
        }

        public bool AddDetainLicense()
        {
            this.DetainID = DetainedLicenses_Data.AddDetainLicense(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);
            
            return (this.DetainID > 0);
        }

        public bool ReleaseLicense()
        {
            return DetainedLicenses_Data.ReleaseLicense(this.LicenseID, this.ReleaseDate, this.ReleasedByUserID, this.ReleaseApplicationID);
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return DetainedLicenses_Data.IsLicenseDetained(LicenseID);
        }

        public static int GetDetainID(int LicenseID)
        {
            return DetainedLicenses_Data.GetDetainID(LicenseID);
        }

        public static int GetDetainFees(int LicenseID)
        {
            return DetainedLicenses_Data.GetDetainFees(LicenseID);
        }

        public static int GetPersonDetainRecords(int LicenseID)
        {
            return DetainedLicenses_Data.GetPersonDetainRecords(LicenseID);
        }
    }

    public class clsBasicLoaclApplicationsInfo_View
    {
        public int LocalAppID {get; set;}
        public int AppID {get; set;}
        public string ClassName { get; set;}
        public string Status  {get; set;}
        public decimal Fees {get; set;}
        public string AppType {get; set;}
        public int PersonID { get; set;}
        public string Applicant {get; set;}
        public DateTime AppDate {get; set;}
        public DateTime LastStatusUpdate   {get; set;}
        public string UserName { get; set; }


        private clsBasicLoaclApplicationsInfo_View(int LocalID, string Class, string FullName, int PersonID)
        {
            this.LocalAppID = LocalID;
            this.ClassName = Class;
            this.Applicant = FullName;
            this.PersonID = PersonID;
        }

        clsBasicLoaclApplicationsInfo_View(int LocalAppID, int AppID, string Status, decimal Fees, string AppType,
         int PersonID,  string Applicant,  DateTime AppDate,  DateTime LastStatusUpdate,  string UserName)
        {
            this.LocalAppID = LocalAppID;
            this.AppID = AppID;
            this.Status = Status;
            this.Fees = Fees;
            this.AppType = AppType;
            this.PersonID = PersonID;
            this.Applicant = Applicant;
            this.AppDate = AppDate;
            this.LastStatusUpdate = LastStatusUpdate;
            this.UserName = UserName;
        }

        public static clsBasicLoaclApplicationsInfo_View Find(int LocalAppID)
        {           
            int AppID = 0;
            string Status = "";
            decimal Fees = 0;
            string AppType = "";
            int PersonID = 0;
            string Applicant = "";
            DateTime AppDate = DateTime.MinValue;
            DateTime LastStatusUpdate = DateTime.MinValue;
            string UserName = "";

            if (BasicLocalApplicationsInfo_View.Find(LocalAppID, ref AppID, ref Status, ref Fees, ref PersonID,
                                ref AppType, ref Applicant, ref AppDate, ref LastStatusUpdate, ref UserName))
            {
                return new clsBasicLoaclApplicationsInfo_View(LocalAppID, AppID, Status, Fees, AppType,
                    PersonID, Applicant, AppDate, LastStatusUpdate, UserName);
            }
            else
                return null;
            
        }

        public static clsBasicLoaclApplicationsInfo_View FindClassAndName(int LocalAppID)
        {
            string D_Class = "";
            string FullName = "";
            int PersonID = 0;

            if (BasicLocalApplicationsInfo_View.Find(LocalAppID, ref D_Class, ref FullName, ref PersonID))
            {
                return new clsBasicLoaclApplicationsInfo_View(LocalAppID, D_Class, FullName, PersonID);
            }
            else
                return null;
        }

    }

    public class clsLocalDriverLicensesInfo_View
    {
        public int LocalAppID {get; set;}
        public string ClassName {get; set;}
        public string FullName {get; set;}
        public int LicenseID  {get; set;}
        public string NationalNo {get; set;}
        public string Gender {get; set;}
        public DateTime IssueDate {get; set;}
        public string IssueReason {get; set;}
        public string Notes {get; set;}
        public bool IsActive {get; set;}
        public DateTime DateOfBirth {get; set;}
        public int DriverID {get; set;}
        public DateTime ExpirationDate { get; set;}
        public string ImagePath { get; set; }

        public clsLocalDriverLicensesInfo_View()
        {

        }

        clsLocalDriverLicensesInfo_View(int LocalAppID, string ClassName, string FullName, int LicenseID, string NationalNo
            , string Gender, DateTime IssueDate, string IssueReason, string Notes,
            bool IsActive, DateTime DateOfBirth, int DriverID, DateTime ExpirationDate, string ImagePath)
        {
            this.LocalAppID = LocalAppID;
            this.ClassName = ClassName;
            this.FullName = FullName;
            this.LicenseID = LicenseID;
            this.NationalNo = NationalNo;
            this.Gender = Gender;
            this.IssueDate = IssueDate;
            this.IssueReason = IssueReason;
            this.Notes = Notes;
            this.IsActive = IsActive;
            this.DateOfBirth = DateOfBirth;
            this.DriverID = DriverID;
            this.ExpirationDate = ExpirationDate;
            this.ImagePath = ImagePath;
        }

        public static clsLocalDriverLicensesInfo_View FindByLocalAppID(int LocalAppID)
        {
            string ClassName = "";
            string FullName = "";
            int LicenseID = 0;
            string NationalNo = "";
            string Gender = "";
            DateTime IssueDate = DateTime.MinValue;
            string IssueReason = "";
            string Notes = "";
            bool IsActive = false;
            DateTime DateOfBirth = DateTime.MinValue;
            int DriverID = 0;
            DateTime ExpirationDate = DateTime.MinValue;
            string ImagePath = "";

            if (LocalDriverLicensesInfo_View.FindByLocalAppID(LocalAppID, ref ClassName, ref FullName, ref LicenseID,
                ref NationalNo, ref Gender, ref IssueDate, ref IssueReason,
                ref Notes, ref IsActive, ref DateOfBirth, ref DriverID, ref ExpirationDate, ref ImagePath))
            {
                return new clsLocalDriverLicensesInfo_View(LocalAppID, ClassName, FullName, LicenseID,
                NationalNo, Gender, IssueDate, IssueReason, Notes, IsActive, DateOfBirth, DriverID, ExpirationDate, ImagePath);
            }
            else
                return null;
        }

        public static clsLocalDriverLicensesInfo_View FindByLicenseID(int LicenseID)
        {
            string ClassName = "";
            string FullName = "";
            int LocalAppID = 0;
            string NationalNo = "";
            string Gender = "";
            DateTime IssueDate = DateTime.MinValue;
            string IssueReason = "";
            string Notes = "";
            bool IsActive = false;
            DateTime DateOfBirth = DateTime.MinValue;
            int DriverID = 0;
            DateTime ExpirationDate = DateTime.MinValue;
            string ImagePath = "";

            if (LocalDriverLicensesInfo_View.FindByLicenseID(LicenseID, ref ClassName, ref FullName, ref LocalAppID,
                ref NationalNo, ref Gender, ref IssueDate, ref IssueReason,
                ref Notes, ref IsActive, ref DateOfBirth, ref DriverID, ref ExpirationDate, ref ImagePath))
            {
                return new clsLocalDriverLicensesInfo_View(LocalAppID, ClassName, FullName, LicenseID,
                NationalNo, Gender, IssueDate, IssueReason, Notes, IsActive, DateOfBirth, DriverID, ExpirationDate, ImagePath);
            }
            else
                return null;
        }

        public static bool IsExistLicense(int LicenseID)
        {
            return LocalDriverLicensesInfo_View.IsExistLicense(LicenseID);
        }

    }

    public class clsInternationalDriverLicensesInfo_View : clsLocalDriverLicensesInfo_View
    {
        public int InternationalLicenseID { get; set; }
        public int AppID { get; set; }

        clsInternationalDriverLicensesInfo_View(int InternationalLicenseID, int AppID, string FullName, int LicenseID, string NationalNo
            , string Gender, DateTime IssueDate,
            bool IsActive, DateTime DateOfBirth, int DriverID, DateTime ExpirationDate, string ImagePath)
        {
            this.InternationalLicenseID = InternationalLicenseID;
            this.AppID = AppID;
            this.FullName = FullName;
            this.LicenseID = LicenseID;
            this.NationalNo = NationalNo;
            this.Gender = Gender;
            this.IssueDate = IssueDate;
            this.IsActive = IsActive;
            this.DateOfBirth = DateOfBirth;
            this.DriverID = DriverID;
            this.ExpirationDate = ExpirationDate;
            this.ImagePath = ImagePath;
        }

        public static clsInternationalDriverLicensesInfo_View FindByLocalLicenseID(int LocalLicenseID)
        {
            int AppID = 0;
            string FullName = "";
            int InternationalLicenseID = 0;
            string NationalNo = "";
            string Gender = "";
            DateTime IssueDate = DateTime.MinValue;
            bool IsActive = false;
            DateTime DateOfBirth = DateTime.MinValue;
            int DriverID = 0;
            DateTime ExpirationDate = DateTime.MinValue;
            string ImagePath = "";

            if (InternationalDriverLicensesInfo_View.FindByLocalLicenseID(LocalLicenseID, ref InternationalLicenseID,ref AppID , ref FullName,
                ref NationalNo, ref Gender, ref IssueDate,
                ref IsActive, ref DateOfBirth, ref DriverID, ref ExpirationDate, ref ImagePath))
            {
                return new clsInternationalDriverLicensesInfo_View(InternationalLicenseID, AppID, FullName, LocalLicenseID,
                NationalNo, Gender, IssueDate, IsActive, DateOfBirth, DriverID, ExpirationDate, ImagePath);
            }
            else
                return null;
        }

        public static clsInternationalDriverLicensesInfo_View FindByInt_LicenseID(int InterNationalLicenseID)
        {
            int AppID = 0;
            string FullName = "";
            int LicenseID = 0;
            string NationalNo = "";
            string Gender = "";
            DateTime IssueDate = DateTime.MinValue;
            bool IsActive = false;
            DateTime DateOfBirth = DateTime.MinValue;
            int DriverID = 0;
            DateTime ExpirationDate = DateTime.MinValue;
            string ImagePath = "";

            if (InternationalDriverLicensesInfo_View.FindByLicenseID(InterNationalLicenseID, ref AppID, ref FullName, ref LicenseID,
                ref NationalNo, ref Gender, ref IssueDate,
                ref IsActive, ref DateOfBirth, ref DriverID, ref ExpirationDate, ref ImagePath))
            {
                return new clsInternationalDriverLicensesInfo_View(InterNationalLicenseID, AppID, FullName, LicenseID,
                NationalNo, Gender, IssueDate, IsActive, DateOfBirth, DriverID, ExpirationDate, ImagePath);
            }
            else
                return null;
        }

    }

}
