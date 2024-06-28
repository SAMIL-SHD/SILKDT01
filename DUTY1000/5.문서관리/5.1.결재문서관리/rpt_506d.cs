using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace DUTY1000
{
    public partial class rpt_506d : DevExpress.XtraReports.UI.XtraReport
    {
        public rpt_506d(int gubn, string title)
        {
            InitializeComponent();

			if (gubn == 2)
				xrLabel1.Text = "간호부 간호간병비 수당";

			lab_title.Text = title;

			s_qt2.DataBindings.Add("Text", DataSource, "MM_CNT3", "");
            
            xrTableCell2.DataBindings.Add("Text", DataSource, "SD_AMT", "");
        }
    }
}
