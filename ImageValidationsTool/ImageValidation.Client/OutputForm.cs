using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Windows.Forms;

namespace ImageValidation.Client
{
    public partial class OutputForm : Form
    {
        private HtmlAssembler html_assembler;
        private string filename;

        public OutputForm()
        {
            InitializeComponent();
            init();
        }

        public OutputForm(string filename)
        {
            InitializeComponent();
            init();
            this.filename = filename;

            showHtmlOutput();
        }

        private void init()
        {
            textBox1.Size = new System.Drawing.Size(584, 537);
            webBrowser1.Size = new System.Drawing.Size(584, 537);
            textBox1.ReadOnly = true;
            html_assembler = new HtmlAssembler();
        }

        private void showHtmlOutput()
        {
            textBox1.Hide();
            webBrowser1.Visible = true;
            webBrowser1.Location = new System.Drawing.Point(0, 0);
            webBrowser1.DocumentText = html_assembler.readFileToHtml(this.filename);
        }

        private void showOutput()
        {
            showHtmlOutput();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            showOutput();
        }
        
        #region CREATE_SAMPLE_DATA
        private DataSet FormatToDataSet(string[] lines)
        {
            DataSet ds = new DataSet();

            var tb = webBrowser1.Document.GetElementById("tablecontent");

            int done = 0;
            DataTable dt = new DataTable();
            foreach (HtmlElement tr in tb.Children[0].Children)
            {
                Console.WriteLine("((((((((((( " + tr.InnerHtml + " )))))))))) ");
                if (tr.InnerHtml.StartsWith("<TD id=header-td"))
                {
                   Console.WriteLine("**NEW HEADER: " + tr.InnerText);
                    for (int i = 0; i < tr.Children.Count; i++)
                    {
                      //  Console.WriteLine("--------------- header item: " + tr.Children[i].InnerText);
                        dt.Columns.Add(tr.Children[i].InnerText, Type.GetType("System.String"));
                    }

                }
                else if (tr.InnerHtml.StartsWith("<TD"))
                {
                    Console.WriteLine(" TD COUNT: " + tr.Children.Count);
                    List<string> rowstring = new List<string>();
                    for (int i = 0; i < tr.Children.Count; i++)
                    {
                        //Console.WriteLine("--------------- td item: " + tr.Children[i].InnerText);
                        if (i == tr.Children.Count - 1)
                            rowstring.Add(tr.Children[i].InnerText);
                        else
                            rowstring.Add(tr.Children[i].InnerText + ", ");
                    }

                 //   Console.WriteLine("_>>>>>>>>>>>>>> " + rowstring.Count);
                   object[] objs = new object[rowstring.Count];
                    for (int s = 0; s < rowstring.Count; s++)
                    {
                        object o = new object();
                        o = rowstring[s];
                        objs[s] = o;
                    }

                    dt.Rows.Add(objs);
                 
                    done++;
                    if (done == 2)
                    {
                       
                    }
                }
                else
                {
                    Console.WriteLine("***********************************************************NEW SHEET: " + tr.InnerText);
                    if (done != 0)
                      ds.Tables.Add(dt);
                    dt = new DataTable();
                    dt.TableName = tr.InnerText;
                    
                }
            }

            ds.Tables.Add(dt);
            return ds;
        }
        private DataSet CreateSampleData()
        {
            DataSet ds = new DataSet();

            //  Create the first table of sample data
            DataTable dt1 = new DataTable("Drivers");
            dt1.Columns.Add("UserID", Type.GetType("System.Decimal"));
            dt1.Columns.Add("Surname", Type.GetType("System.String"));
            dt1.Columns.Add("Forename", Type.GetType("System.String"));
            dt1.Columns.Add("Sex", Type.GetType("System.String"));
            dt1.Columns.Add("Date of Birth", Type.GetType("System.DateTime"));

            dt1.Rows.Add(new object[] { 1, "James", "Brown", "M", new DateTime(1962,3,19) });
            dt1.Rows.Add(new object[] { 2, "Edward", "Jones", "M", new DateTime(1939,7,12) });
            dt1.Rows.Add(new object[] { 3, "Janet", "Spender", "F", new DateTime(1996,1,7) });
            dt1.Rows.Add(new object[] { 4, "Maria", "Percy", "F", new DateTime(1991,10,24) });
            dt1.Rows.Add(new object[] { 5, "Malcolm", "Marvelous", "M", new DateTime(1973,5,7) });
            ds.Tables.Add(dt1);


            //  Create the second table of sample data
            DataTable dt2 = new DataTable("Vehicles");
            dt2.Columns.Add("Vehicle ID", Type.GetType("System.Decimal"));
            dt2.Columns.Add("Make", Type.GetType("System.String"));
            dt2.Columns.Add("Model", Type.GetType("System.String"));

            dt2.Rows.Add(new object[] { 1001, "Ford", "Banana" });
            dt2.Rows.Add(new object[] { 1002, "GM", "Thunderbird" });
            dt2.Rows.Add(new object[] { 1003, "Porsche", "Rocket" });
            dt2.Rows.Add(new object[] { 1004, "Toyota", "Gas guzzler" });
            dt2.Rows.Add(new object[] { 1005, "Fiat", "Spangly" });
            dt2.Rows.Add(new object[] { 1006, "Peugeot", "Lawnmower" });
            dt2.Rows.Add(new object[] { 1007, "Jaguar", "Freeloader" });
            dt2.Rows.Add(new object[] { 1008, "Aston Martin", "Caravanette" });
            dt2.Rows.Add(new object[] { 1009, "Mercedes-Benz", "Hitchhiker" });
            dt2.Rows.Add(new object[] { 1010, "Renault", "Sausage" });
            dt2.Rows.Add(new object[] { 1011, "Saab", "Chickennuggetmobile" });
            ds.Tables.Add(dt2);


            //  Create the third table of sample data
            DataTable dt3 = new DataTable("Vehicle owners");
            dt3.Columns.Add("User ID", Type.GetType("System.Decimal"));
            dt3.Columns.Add("Vehicle_ID", Type.GetType("System.Decimal"));

            dt3.Rows.Add(new object[] { 1, 1002 });
            dt3.Rows.Add(new object[] { 2, 1000 });
            dt3.Rows.Add(new object[] { 3, 1010 });
            dt3.Rows.Add(new object[] { 5, 1006 });
            dt3.Rows.Add(new object[] { 6, 1007 });
            ds.Tables.Add(dt3);

            return ds;
        }
        #endregion

        private void exportBtn_Click(object sender, EventArgs e)
        {
            string filename = @"../../Report/ImageReport.csv";
            string xmlfilename = @"../../Report/Image_DetailedReport.xlsx";

            string[] lines = File.ReadAllLines(filename);

            string[] headers = lines[0].Split(',').Select(x => x.Trim('\"')).ToArray();

            //lines = (string[]) lines.Skip(1);

         //   DataSet ds = CreateSampleData();

            DataSet ds1 = FormatToDataSet(lines);

            try
            {
                CreateExcelFile.CreateExcelDocument(ds1, xmlfilename);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't create Excel file.\r\nException: " + ex.Message);
                return;
            }


            MessageBox.Show("Saved to:" + xmlfilename);
             
        }
    }
}
