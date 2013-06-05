using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ImageValidation.Client
{
    class HtmlAssembler
    {
        /**
         * Injects logged data from csv files into
         * log_template to create HTML based logs
         */
        string html_template;
        StringBuilder driver_rows;
        StringBuilder app_rows;
        StringBuilder ff_rows;
        StringBuilder regs_rows;
        StringBuilder hotfixes_rows;
        HtmlTransforms ht;

        public HtmlAssembler()
        {
            ht = new HtmlTransforms();
            using(StreamReader reader = new StreamReader("..\\..\\report\\log_template.html"))
            {
                html_template = reader.ReadToEnd();
            }
        }
        private void AddHeaders(bool appf, bool driverf, bool fff, bool regf, bool hotfixf)
        {
            string[] happs = new string[5] { "DeviceName", "CurrentVersion", "Link", "InstallLoc", "InstallSrc" };
            string[] hdrivers = new string[5] { "DeviceName", "BaseVersion", "CurrentVersion", "ProviderName", "INFName" };
            string[] hfilefolders = new string[4] { "Location", "Note", "Type", "" };
            string[] hregs = new string[4] {"Registry Key", "FileName", "Note", "Type" };
            string[] hhotfixes = new string[3] { "Microsoft HotFixes", "", "" };
            if (appf)
            {
                string happsTransform = ht.getTransformations(happs);
                app_rows.Insert(0, happsTransform);
            }
            if (driverf)
            {
                string hdriversTransform = ht.getTransformations(hdrivers);
                driver_rows.Insert(0, hdriversTransform);
            }
            if (fff)
            {
                string hfilefoldersTransform = ht.getTransformations(hfilefolders);
                ff_rows.Insert(0, hfilefoldersTransform);
            }
            if (regf)
            {
                string hregsTransform = ht.getTransformations(hregs);
                regs_rows.Insert(0, hregsTransform);
            }
            if (hotfixf)
            {
                string hhotfixesTransform = ht.getTransformations(hhotfixes);
                hotfixes_rows.Insert(0, hhotfixesTransform);
            }
        }
        public string readFileToHtml(string csvfile)
        {
            app_rows = new StringBuilder();
            driver_rows = new StringBuilder();
            ff_rows = new StringBuilder();
            regs_rows = new StringBuilder();
            hotfixes_rows = new StringBuilder();
            using(StreamReader reader = new StreamReader(csvfile))
            {
                string line;
                bool appf=false, driverf=false, fff=false, regf=false, hotfixf=false;

                string html_copy = html_template;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                   // int i =0;
                   // foreach (string s in data)
                   // {
                     //   Console.WriteLine("data[" + i + "]=" + s);
                    //    i++;
                    //}
                    string dataTransform = ht.getTransformations(data);

                    if (data[0] == "a")
                    {   // App flag
                        app_rows.Append(dataTransform);
                        appf = true;
                    }
                    else if (data[0] == "d")
                    {   // Driver flag
                        driver_rows.Append(dataTransform);
                        driverf = true;
                    }
                    else if (data[0] == "f")
                    {   // file&folder flag
                        ff_rows.Append(dataTransform);
                        fff = true;
                    }
                    else if (data[0] == "r")
                    {   // Registry
                        regs_rows.Append(dataTransform);
                        regf = true;
                    }
                    else if (data[0] == "h")
                    {   // Microsoft HotFixes
                        hotfixes_rows.Append(dataTransform);
                        hotfixf = true;
                    }
                    else
                        continue;
                }

                AddHeaders(appf, driverf, fff, regf, hotfixf);

                html_copy = html_copy.Replace("<!--@data1-->", driver_rows.ToString());
                html_copy = html_copy.Replace("<!--@data2-->", app_rows.ToString());
                html_copy = html_copy.Replace("<!--@data3-->", ff_rows.ToString());
                html_copy = html_copy.Replace("<!--@data4-->", regs_rows.ToString());
                html_copy = html_copy.Replace("<!--@data5-->", hotfixes_rows.ToString());
               
                return html_copy;
            }
        }
    }
}
