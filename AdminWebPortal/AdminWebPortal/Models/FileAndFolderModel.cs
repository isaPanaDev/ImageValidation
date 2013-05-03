using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWebPortal.Models
{
    public class FileAndFolderModel
    {
        public List<FileFolder> FileFoler { get; set; }
        public string FilePath { get; set; }

        public string CurrentFilePath { get; set; }

        [AllowHtml]
        public string Note { get; set; }

        public int  FileFolderID { get; set; }
    }
}