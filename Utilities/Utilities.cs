using System;
using System.IO;
using Microsoft.Win32;

namespace DVLD_Utilities
{
    public class clsRememberMe
    {
        private static string SubKeyPath = @"SOFTWARE\YourLoginInfo";
        private static string ValueName = "Login Info";

        public static bool SaveUserNameAndPassword(string ValueData)
        {

            try
            {
                using(RegistryKey Key = Registry.CurrentUser.CreateSubKey(SubKeyPath))
                {
                    if (Key != null)
                    { 
                        Key.SetValue(ValueName, ValueData, RegistryValueKind.String);
                        return true;
                    }
                    return false;
                }

            }

            catch(Exception  ex)
            {
                return false;
            }
        }

        public static bool DeleteUserLoginRecord()
        {
            try
            {
                using(RegistryKey Key = Registry.CurrentUser.OpenSubKey(SubKeyPath, true))
                {
                    if(Key != null)
                    {
                        Key.DeleteValue(ValueName, false);
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static bool Load(out string UserName, out string Password)
        {

            UserName = string.Empty;
            Password = string.Empty;

            try
            {
                using (RegistryKey Key = Registry.CurrentUser.OpenSubKey(SubKeyPath))
                {
                    if (Key != null)
                    {
                        string RowData = Key.GetValue(ValueName, null) as string;

                        if(!string.IsNullOrEmpty(RowData))
                        {
                            string[] LoginInfo = RowData.Split('|');

                            UserName = LoginInfo[0];
                            Password = LoginInfo[1];

                            return true;
                        }

                        return false;
                    }

                    return false;
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

    public class clsDGV_Validation
    {
        public static bool IsDGVEmpty_Or_SelectedRowNull(int RowCount, object CurrentRow)
        {
            if (RowCount < 1 && CurrentRow == null)
                return true;
            else
                return false;
        }
    }
}
