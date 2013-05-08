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
        public Hotfix GetHotFixInfo()
        {
            Hotfix hotFix = new Hotfix();
            ManagementObjectSearcher mosHotfix = new ManagementObjectSearcher("SELECT * FROM Win32_QuickFixEngineering");



            foreach (ManagementObject moHotfix in mosHotfix.Get())
            {
                hotFix.CSName = moHotfix["CSName"].ToString();
                hotFix.Description = moHotfix["Description"].ToString();
                //hotFix.InstallDate = (DateTime) moHotfix["InstallDate"];
                hotFix.InstalledBy = moHotfix["InstalledBy"].ToString();


                //Attributes details
                //string Caption;
                //string CSName;
                //string Description;
                //string FixComments;
                //string HotFixID;
                //datetime InstallDate;
                //string InstalledBy;
                //string InstalledOn;
                //string Name;
                //string ServicePackInEffect;
                //string Status;

            }

            return hotFix;
        }
    }
}
