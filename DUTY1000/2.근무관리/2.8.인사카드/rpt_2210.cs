using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraPrinting;
using System.IO;

namespace DUTY1000
{
    public partial class rpt_2210 : DevExpress.XtraReports.UI.XtraReport
    {
        SilkRoad.Common.CustomFormatter cf = new SilkRoad.Common.CustomFormatter();
        SilkRoad.Common.DrawRptSignBox dsb = new SilkRoad.Common.DrawRptSignBox();
        DataSet sub_ds;
        DataProcFunc df = new DataProcFunc();
        //DataFuns.df1000 df = new DataFuns.df1000();

        public rpt_2210(DataSet ds)
        {
            InitializeComponent();
            sub_ds = ds;
        }

        public void writeReport(DataSet ds, int print)
        {
            df.GetMstEmplDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref ds);     //개인

            //양식구분
            //전체
            if (print == 0)
            {
                if (SilkRoad.Config.SRConfig.WorkPlaceSano != "4098205251") //(주)천주의성요한수도회 이외 업체
                {
                    SubBand1.Visible = true;
                    SubBand2.Visible = true;
                }
                else
                {
                    SubBand1.Visible = true;
                    SubBand3.Visible = true;
                }
            }
            else if (print == 1)        //양식1
            {
                SubBand1.Visible = true;
            }
            else
            {
                //양식2
                if (SilkRoad.Config.SRConfig.WorkPlaceSano != "4098205251") //(주)천주의성요한수도회 이외 업체
                {
                    SubBand2.Visible = true;
                }
                else
                {
                    SubBand3.Visible = true;
                }
            }
        }

        private void xrTableCell1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("사진").ToString().Trim() != "")
            {
                byte[] myByte = new byte[0];

                myByte = (byte[])GetCurrentColumnValue("사진");
                MemoryStream ImgStream = new MemoryStream(myByte);
                xrPictureBox1.Image = Image.FromStream(ImgStream);
            }
            else
            {
                xrPictureBox1.Image = null;
            }


            //DataRow drow = sub_ds.Tables["INSA_PIC"].Rows[0];

            //if (drow["사진"].ToString().Trim() != "")
            //{
            //    byte[] myByte = new byte[0];

            //    myByte = (byte[])drow["사진"];
            //    MemoryStream ImgStream = new MemoryStream(myByte);
            //    xrPictureBox1.Image = Image.FromStream(ImgStream);
            //}

        }


        #region 인사정보

        #region BASE
        private void Base_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell29.Text = "/　" + GetCurrentColumnValue("부서").ToString();     //부서
            xrTableCell30.Text = "/　" + GetCurrentColumnValue("직종").ToString();     //직종
            xrTableCell31.Text = "/　" + GetCurrentColumnValue("직급").ToString() + " " + GetCurrentColumnValue("호봉").ToString();      //직급_호봉

            xrTableCell23.Text = "/　" + GetCurrentColumnValue("퇴사일").ToString();    //퇴사일
            xrTableCell4.Text = GetCurrentColumnValue("주민번호").ToString() == "" ? "" : GetCurrentColumnValue("주민번호").ToString().Substring(0, 6) + "-" + GetCurrentColumnValue("주민번호").ToString().Substring(6, 7);  //주민번호
            xrTableCell13.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["입사유형"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["입사유형"].ToString();      //입사유형
            xrTableCell17.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLJNDT"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLJNDT"].ToString();      //장애일자
            xrTableCell25.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLJNDG"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLJNDG"].ToString();      //장애등급
            xrTableCell21.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLBHDG"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLBHDG"].ToString();      //보훈등급
            xrTableCell56.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["종교"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["종교"].ToString();      //종교
            xrTableCell57.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLTUKI"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLTUKI"].ToString();      //특기
            xrTableCell65.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLCHIM"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLCHIM"].ToString();      //취미
        }
        #endregion

        #region 기본사항
        private void Normal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //성별
            if (sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLJNDT"].ToString() == "1")
            {
                xrTableCell26.Text = "남";
            }
            else if (sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLJNDT"].ToString() == "2")
            {
                xrTableCell26.Text = "여";
            }
            else
            {
                xrTableCell26.Text = "";
            }

            //결혼여부
            if (sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLMRGU"].ToString() == "1")
            {
                xrTableCell35.Text = "미혼";
            }
            else if (sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLMRGU"].ToString() == "2")
            {
                xrTableCell35.Text = "기혼";
            }
            else
            {
                xrTableCell35.Text = "";
            }
            xrTableCell33.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLBDAY"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLBDAY"].ToString();      //생년월일
            xrTableCell37.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLMDAY"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLMDAY"].ToString();      //결혼일자

            xrTableCell43.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["주거구분"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["주거구분"].ToString();     //주거구분
            xrTableCell45.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["출신도"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["출신도"].ToString();    //출신도
            //xrTableCell47.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLBADD"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLBADD"].ToString();     //본적
            xrTableCell47.Text = GetCurrentColumnValue("주소1").ToString() == "" ? "" : GetCurrentColumnValue("주소1").ToString() + " " + GetCurrentColumnValue("주소2").ToString(); //주소
        }
        #endregion

        #region 신체사항
        private void Body_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //xrTableCell54.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLHIGH"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLHIGH"].ToString() + " Cm";   //신장
            //xrTableCell58.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLWEIG"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLWEIG"].ToString() + " Kg";   //체중
            //xrTableCell60.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLLSEE"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLLSEE"].ToString();      //시력(좌)
            //xrTableCell62.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLRSEE"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLRSEE"].ToString();      //시력(우)
            //xrTableCell66.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLBLOD"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLBLOD"].ToString();      //혈액형
        }
        #endregion

        #region 병역사항
        private void Army_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell81.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["군별"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["군별"].ToString();              //군별
            xrTableCell70.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["병역"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["병역"].ToString();              //병역
            xrTableCell84.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLIPDT"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLIPDT"].ToString();      //입대일
            xrTableCell72.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLKBUN"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLKBUN"].ToString();      //군번
            xrTableCell76.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLBGWA"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLBGWA"].ToString();      //병과
            xrTableCell78.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["계급"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["계급"].ToString();              //계급            
            xrTableCell85.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLJYDT"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLJYDT"].ToString();      //전역일
            xrTableCell80.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["전역구분"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["전역구분"].ToString();      //전역구분
        }
        #endregion

        #region 외국어
        private void LANG_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell119.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["외국어1"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["외국어1"].ToString();
            xrTableCell137.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["외국어2"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["외국어2"].ToString();
            xrTableCell134.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["외국어3"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["외국어3"].ToString();

            xrTableCell140.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLSPE1"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLSPE1"].ToString();
            xrTableCell141.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLSPE2"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLSPE2"].ToString();
            xrTableCell142.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLSPE3"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLSPE3"].ToString();
            xrTableCell121.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLWRI1"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLWRI1"].ToString();
            xrTableCell138.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLWRI2"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLWRI2"].ToString();
            xrTableCell135.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLWRI3"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLWRI3"].ToString();
            xrTableCell123.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLCON1"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLCON1"].ToString();
            xrTableCell139.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLCON2"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLCON2"].ToString();
            xrTableCell136.Text = sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLCON3"].ToString() == "" ? "" : sub_ds.Tables["MSTEMPL"].Rows[0]["EMPLCON3"].ToString();
        }




        private void Lang_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (((XRTableCell)sender).Text == "0")
            {
                ((XRTableCell)sender).Text = "";
            }
            if (((XRTableCell)sender).Text == "1")
            {
                ((XRTableCell)sender).Text = "상";
            }
            if (((XRTableCell)sender).Text == "2")
            {
                ((XRTableCell)sender).Text = "중";
            }
            if (((XRTableCell)sender).Text == "3")
            {
                ((XRTableCell)sender).Text = "하";
            }
        }






        #endregion

        #endregion


        public void Subreport_Table_Search(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //사원세부사항 - (개인,가족,학력,경력,자격)
            df.GetMstEmplDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);     //개인
            //df.GetMstFamiDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);     //가족
            df.GetTrsHakrDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);     //학력
            df.GetTrsHistDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);     //경력
            df.GetTrsPlicDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);     //자격

            // 1 PAGE
            insa_hark.ReportSource.DataSource = sub_ds.Tables["TRSHAKR"];
            //insa_gajok.ReportSource.DataSource = sub_ds.Tables["TRSFAMI"];
            insa_kungruk.ReportSource.DataSource = sub_ds.Tables["TRSHIST"];
            insa_jakuk.ReportSource.DataSource = sub_ds.Tables["TRSPLIC"];


            if (SilkRoad.Config.SRConfig.WorkPlaceSano != "4098205251") //(주)천주의성요한수도회 이외 업체
            {
                // 2_1 PAGE
                df.GetTrsAwdiDatas(GetCurrentColumnValue("사번").ToString().Trim(), "%", ref sub_ds);   //상벌
                df.GetTrsAwaiDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);   //교육
                //df.GetTrsProjDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);   //프로젝트 - 사용안함
                df.GetTrsDeptDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);   //부서이동내역
                df.GetTrsEstmDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);   //인사고과
                df.GetTrsWorkDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);   //발령

                insa_sangber.ReportSource.DataSource = sub_ds.Tables["TRSAWDI"];
                insa_edu.ReportSource.DataSource = sub_ds.Tables["TRSAWAI"];
                insa_dept.ReportSource.DataSource = sub_ds.Tables["TRSDEPT"];
                insa_gogua.ReportSource.DataSource = sub_ds.Tables["TRSESTM"];
                insa_balrung.ReportSource.DataSource = sub_ds.Tables["TRSWORK"];
            }
            else
            {
                // 2_2 PAGE
                
                df.GetTrsAwaiDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);     //교육
                df.GetTrsUpgrDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);     //승진급_관리
                df.GetTrsEstmDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);     //인사고과
                df.GetTrsWorkDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);     //발령
                df.GetTrsHjshDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);     //휴직관리
                df.GetTrsBgshDatas(GetCurrentColumnValue("사번").ToString().Trim(), ref sub_ds);     //병가관리

                insa_hae.ReportSource.DataSource = sub_ds.Tables["TRSTRAI"];
                insa_sangber_2.ReportSource.DataSource = sub_ds.Tables["TRSAWDI"];
                insa_edu_2.ReportSource.DataSource = sub_ds.Tables["TRSAWAI"];
                insa_upgrade.ReportSource.DataSource = sub_ds.Tables["TRSUPGR"];
                insa_gogua_2.ReportSource.DataSource = sub_ds.Tables["TRSESTM"];
                insa_balrung_2.ReportSource.DataSource = sub_ds.Tables["TRSWORK"];
                insa_bokmu.ReportSource.DataSource = sub_ds.Tables["TRSHJSH"];
                insa_bokmu_2.ReportSource.DataSource = sub_ds.Tables["TRSBGSH"];
            }
        }

		private void xrTable31_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{

		}
	}
}
