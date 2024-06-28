using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraPrinting;

namespace DUTY1000
{
    public partial class rpt_2210_15 : DevExpress.XtraReports.UI.XtraReport
    {
        SilkRoad.Common.CustomFormatter cf = new SilkRoad.Common.CustomFormatter();
        SilkRoad.Common.DrawRptSignBox dsb = new SilkRoad.Common.DrawRptSignBox();

        public rpt_2210_15()
        {
            InitializeComponent();
        }

        private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell2.Text = GetCurrentColumnValue("WORKBDAY").ToString() == "" ? "" :
                                GetCurrentColumnValue("WORKBDAY").ToString().Substring(0, 4) + "-"
                              + GetCurrentColumnValue("WORKBDAY").ToString().Substring(4, 2) + "-"
                              + GetCurrentColumnValue("WORKBDAY").ToString().Substring(6, 2);
        }

        private void xrTableCell3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            if(GetCurrentColumnValue("직급").ToString() == "" && GetCurrentColumnValue("HOBONG").ToString() == "")
            {
                xrTableCell3.Text = "" + "　" + "";
            }
            else if (GetCurrentColumnValue("직급").ToString() != "" && GetCurrentColumnValue("HOBONG").ToString() != "")
            {
                xrTableCell3.Text = GetCurrentColumnValue("직급").ToString() + "　" + GetCurrentColumnValue("HOBONG").ToString();
            }
            else if (GetCurrentColumnValue("직급").ToString() == "" && GetCurrentColumnValue("HOBONG").ToString() != "")
            {
                xrTableCell3.Text = "" + "　" + GetCurrentColumnValue("HOBONG").ToString();
            }
            else if (GetCurrentColumnValue("직급").ToString() != "" && GetCurrentColumnValue("HOBONG").ToString() == "")
            {
                xrTableCell3.Text = GetCurrentColumnValue("직급").ToString() + "　" + "";
            }
        }

		private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			if (this.GetCurrentRow().Equals(this.GetNextRow()))
			{
				xrTableCell1.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell2.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell3.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell4.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell5.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell7.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell8.Borders = BorderSide.Bottom | BorderSide.Right;
			}
		}
	}
}
