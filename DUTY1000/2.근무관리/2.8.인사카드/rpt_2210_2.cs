using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraPrinting;

namespace DUTY1000
{
    public partial class rpt_2210_2 : DevExpress.XtraReports.UI.XtraReport
    {
        SilkRoad.Common.CustomFormatter cf = new SilkRoad.Common.CustomFormatter();
        SilkRoad.Common.DrawRptSignBox dsb = new SilkRoad.Common.DrawRptSignBox();      

        public rpt_2210_2()
        {
            InitializeComponent();
        }       

        private void xrTableCell5_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell5.Text = GetCurrentColumnValue("FAMIBIRT").ToString() == "" ? "" :
                GetCurrentColumnValue("FAMIBIRT").ToString().Substring(0, 4)
              + "-" + GetCurrentColumnValue("FAMIBIRT").ToString().Substring(4, 2)
              + "-" + GetCurrentColumnValue("FAMIBIRT").ToString().Substring(6, 2);
        }

        private void xrTableCell1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("FAMIDGGU").ToString() == "0")
            {
                xrTableCell1.Text = "비동거";
            }
            if (GetCurrentColumnValue("FAMIDGGU").ToString() == "1")
            {
                xrTableCell1.Text = "동　거";
            }
        }

        private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            switch (GetCurrentColumnValue("FAMIGUBU").ToString().Trim())
            {
                case "0":
                    xrTableCell2.Text = "소득자본인";
                    break;
                case "1":
                    xrTableCell2.Text = "소득자의직계존속";
                    break;
                case "2":
                    xrTableCell2.Text = "배우자의직계존속";
                    break;
                case "3":
                    xrTableCell2.Text = "배우자";
                    break;
                case "4":
                    xrTableCell2.Text = "직계비속";
                    break;
                case "5":
                    xrTableCell2.Text = "직계비속자녀 외";
                    break;
                case "6":
                    xrTableCell2.Text = "형제자매";
                    break;
                case "7":
                    xrTableCell2.Text = "수급자";
                    break;
                case "8":
                    xrTableCell2.Text = "위탁아동";
                    break;
                case "9":
                    xrTableCell2.Text = "해당없음";
                    break;
                default:
                    xrTableCell2.Text = " ";
                    break;
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
				xrTableCell6.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell7.Borders = BorderSide.Bottom | BorderSide.Right;
				xrTableCell8.Borders = BorderSide.Bottom | BorderSide.Right;
			}
		}
	}
}
