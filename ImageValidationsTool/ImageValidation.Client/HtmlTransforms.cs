using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageValidation.Client
{
    class HtmlTransforms
    {
        /** HTML tags **/
        string openErrorRow = "<tr class=\"error-tr\">";
        string openNoErrorRow = "<tr class=\"noerror-tr\">";
        string closeRow = "</tr>";

        string openErrorMismatchRow = "<tr class=\"mismatch-tr\">";
        string openErrorMismatchCell = "<td id=\"mismatch-td\" ";
        string openHeaderRow = "<tr class=\"header-tr\">";
        string openHeaderCell = "<td id=\"header-td\" ";

        //note: do no user openErrorCell or openNoErrorCell directly, use them getTableCell method
        string openErrorCell = "<td id=\"error-td\" ";
        string openNoErrorCell = "<td ";
        string closeCell = "</td>";
        /** End of HTML tags **/


        public string getTransformations(string[] data)
        {
            try
            {
                if (isHeader(data))
                {
                    return createRowHeader(data, getCellSpacing(data), false);
                }
                else
                {
                    if (isMismatch(data))
                        return createMismatchRow(data, getCellSpacing(data));

                    if (isErrorMessage(data))
                        return createRow(data, getCellSpacing(data), true);
                    else
                        return createRow(data, getCellSpacing(data), false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        private string createMismatchRow(string[] data, int[] cspace)
        {
            StringBuilder sb_row = new StringBuilder();
            string openRow = openErrorMismatchRow;
            string openCell = openErrorMismatchCell;
            sb_row.Append(openRow);
            for (int cnt = 0; cnt < data.Length; cnt++)
            {
                //Console.WriteLine(cnt.ToString() +"-----Cell: " + data[cnt] + " Space: " + cspace[cnt]);
                // skip over flag item 0 and 1
                if (cnt == 0 || cnt == 1) continue;
                sb_row.Append(getTableCell(openCell, cspace[cnt])).Append(data[cnt]).Append(closeCell);
            }
            sb_row.Append(closeRow);
            return sb_row.ToString();
        }
        private string createRowHeader(string[] data, int[] cspace, bool isError)
        {
            StringBuilder sb_row = new StringBuilder();
            string openRow = openHeaderRow;
            string openCell = openHeaderCell;
            sb_row.Append(openRow);
            for (int cnt = 0; cnt < data.Length; cnt++)
            {
                //Console.WriteLine(cnt.ToString() +"-----Cell: " + data[cnt] + " Space: " + cspace[cnt]);
                sb_row.Append(getTableCell(openCell, cspace[cnt])).Append(data[cnt]).Append(closeCell);
            }
            sb_row.Append(closeRow);
            return sb_row.ToString();
        }
        private string createRow(string[] data, int[] cspace, bool isError)
        {
            StringBuilder sb_row = new StringBuilder();
            string openRow = (isError) ? openErrorRow : openNoErrorRow;
            string openCell = (isError) ? openErrorCell : openNoErrorCell;
            sb_row.Append(openRow);
            for (int cnt = 0; cnt < data.Length; cnt++)
            {
                // skip over flag item 0 and 1
                if (cnt == 0 || cnt == 1) continue;
                string row = getTableCell(openCell, cspace[cnt]);
                //Console.WriteLine("ROW: : " + row + " DATA: " + data[cnt]);
                sb_row.Append(row).Append(data[cnt]).Append(closeCell);
            }
            sb_row.Append(closeRow);
            return sb_row.ToString();
        }

        private string getTableCell(string cellType, int colspan)
        {
            return cellType + "colspan="+colspan+">";
        }

        private bool isMismatch(string[] data)
        {
            if (data.Length > 2)
            {
                string mismatch = "";
                mismatch = data[1];
                //Console.WriteLine("CHECK[1]: " + data[1] + " CHECK MISMATCH. (1=true,else=false): " + mismatch);
                if (mismatch == "1") // for yellow color
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        private bool isErrorMessage(string[] data)
        {
            if(data.Length > 2)
            {
                string thirdmsg = data[1];
                if(thirdmsg == "2")
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        private bool isHeader(string[] data)
        {
            if (data.Length > 2)
            {
                int thirdcol = data[0].Length;
                if (thirdcol > 1) // Check for if greater than 1 letter
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        /**
         * Simplify later
         */

        private int[] getCellSpacing(string[] data)
        {
            string d = string.Concat(data);
            //Console.WriteLine("data: " + d);
            //Console.WriteLine("No of items in data[]: " + data.Length);
            
            int arraylen = data.Length;
            int max_cells = 17;
            int avglen;
            bool perfect_avg = true;
            int[] colspans = new int[data.Length];
            colspans[0] = 1100;
            colspans[1] = 3393;
            if (arraylen >= max_cells)
            {
                avglen = 1;
            }
            else
            {
                avglen = max_cells / arraylen;
                perfect_avg = (max_cells % arraylen == 0) ? true : false;
            }
            for(int cnt=0; cnt<data.Length; cnt++)
            {
               // if (perfect_avg)
               // {
                    for (int i = 0; i < data.Length; i++)
                    {
                     //   colspans[i] = avglen;
                        //Console.WriteLine("count: " + i + " value " + avglen);
                    }
              //  }
              //  else
              //  {
               //     colspans[cnt] = (cnt == (arraylen - 1)) ? max_cells - (cnt * avglen) : avglen;
               // }
            }
            return colspans;
        }

    }
}
