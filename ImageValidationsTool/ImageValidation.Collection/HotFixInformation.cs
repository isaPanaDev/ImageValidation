using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Management;
using Microsoft.Win32;
using ImageValidation.Core;

namespace ImageValidation.Collection
{
    public class HotFixInformation
    {
        #region Collect Hotfix information
        /// <summary>
        /// Get all hotfix information
        /// </summary>
        /// <returns>Hotfix list</returns>
        public List<HotFix> GetHotFixInfo()
        {           
            List<HotFix> ObjHotfixLst = new List<HotFix>();
            ManagementObjectSearcher mosHotfix = new ManagementObjectSearcher("SELECT * FROM Win32_QuickFixEngineering");
            

            foreach (ManagementObject moHotfix in mosHotfix.Get())
            {
                HotFix hotFix = new HotFix();
                if (moHotfix["CSName"] != null)
                {
                    hotFix.CSName = moHotfix["CSName"].ToString();
                }
                else
                {
                    hotFix.CSName = string.Empty;
                }

                if (moHotfix["Description"] != null)
                {
                    hotFix.Description = moHotfix["Description"].ToString();
                }
                else
                {
                    hotFix.Description = string.Empty;
                }

                if (moHotfix["HotFixID"] != null)
                {
                    hotFix.HotFixIDs = moHotfix["HotFixID"].ToString();
                }
                else
                {
                    hotFix.HotFixIDs = string.Empty;
                }

                if (moHotfix["InstallDate"] != null)
                {
                    hotFix.InstallDate = moHotfix["InstallDate"].ToString();
                }
                else
                {
                    hotFix.InstallDate = string.Empty;
                }

                if (moHotfix["InstalledBy"] != null)
                {
                    hotFix.InstalledBy = moHotfix["InstalledBy"].ToString();
                }
                else
                {
                    hotFix.InstalledBy = string.Empty;
                }

                hotFix.IsRequired = "0";
                ObjHotfixLst.Add(hotFix);
            }
            return ObjHotfixLst;
        }

        #endregion
    }
}
