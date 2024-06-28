using System;
using System.Data;
using System.Windows.Forms;
using SilkRoad.Common;
using System.Text;
using DevExpress.XtraGrid.Views;
using System.Drawing;
using System.IO;
using SilkRoad.UserControls;

namespace WAGE1000
{
    public partial class duty1006 : SilkRoad.Form.Base.FormX
    {
        CommonLibrary clib = new CommonLibrary();

        ClearNEnableControls cec = new ClearNEnableControls();
        public DataSet ds = new DataSet();
        DataProcFunc df = new DataProcFunc();
        SilkRoad.DataProc.GetData gd = new SilkRoad.DataProc.GetData();
        string _Flag = "";

        public duty1006()
        {
            InitializeComponent();
        }

        #region 0. Initialization

        /// <summary>
        ///컨트롤 초기화 및 활성,비활성 설정
        /// </summary>
        /// <param name="enable"></param>
        private void SetCancel()
        {
            _Flag = "";

            //grdv1.FocusedRowHandle = -1;
            cec.SetClearControls(srGroupBox5, new string[] { "txt_code" });
            cec.SetClearControls(grbx1, new string[] { "" });
			
            btn_save.Text = "저  장";
            btn_save.Image = WAGE1000.Properties.Resources.저장;

			txt_code.Enabled = true;		
            //srPanel7.Enabled = false;
			grbx1.Enabled = false;
            SetButtonEnable("1000");

            txt_code.Focus();
        }

        #endregion

        #region 1 Form

        private void duty1006_Load(object sender, EventArgs e)
        {
            SetCancel();
			btn_search_CK();
        }

        #endregion

        #region 2 Button
		
        /// <summary>엑셀버튼</summary>
        private void btn_exel_Click(object sender, EventArgs e)
        {
			clib.gridToExcel(grdv1, this.Name + "_" + clib.DateToText(DateTime.Now));
		}
        /// <summary> refresh버튼 </summary>
        private void btn_refresh_Click(object sender, EventArgs e)
        {
			btn_search_CK();
		}		
        private void btn_expand_Click(object sender, EventArgs e)
        {
            clib.SetExpandFold(btn_expand, srSplitContainer1, DevExpress.XtraEditors.SplitPanelVisibility.Panel1);
        }
        //처리
        private void btn_proc_Click(object sender, EventArgs e)
        {
            if (isNoError_um(1))
            {
				df.GetMSTEMBSDatas(txt_code.Text.ToString(), ds);
				if (ds.Tables["MSTEMBS"].Rows.Count > 0)
				{
					DataRow crow = ds.Tables["MSTEMBS"].Rows[0];

					//_code = crow["EMBSSABN"].ToString();
					txt_name.Text = crow["EMBSNAME"].ToString().Trim(); //성명

					//mssql 은 아래필드들 암호화변환작업 필요..일단 보류
					txt_jmno.Text = crow["D_JMNO"].ToString().Trim(); //주민번호
					txt_tlno.Text = crow["D_TLNO"].ToString().Trim(); //전화번호
					txt_hpno.Text = crow["D_HPNO"].ToString().Trim();
					txt_adr1.Text = crow["D_ADR1"].ToString().Trim();
					txt_adr2.Text = crow["D_ADR2"].ToString().Trim();
					txt_post.Text = crow["EMBSPOST"].ToString().Trim();

					cmb_stat.SelectedIndex = clib.TextToInt(crow["EMBSSTAT"].ToString()) - 1; //근무구분(1:재직.2:퇴직) 
					cmb_adgb.SelectedIndex = clib.TextToInt(crow["EMBSADGB"].ToString()); //관리자구분(1:계장,2:팀장,3:부서장,4,원장단,5.담당원장,6.대표원장) 
					//chk_adgb.Checked = crow["EMBSADGB"].ToString().Trim() == "1" ? true : false;
					dat_ipdt.DateTime = clib.TextToDate(crow["EMBSIPDT"].ToString().Trim()); //입사일
					//dat_grdt.DateTime = clib.TextToDate(crow["GIN_DATE"].ToString().Trim()); //그룹입사일
					dat_tsdt.DateTime = clib.TextToDate(crow["EMBSTSDT"].ToString().Trim()); //퇴사일
					//dat_gsdt.DateTime = clib.TextToDate(crow["TSG_DATE"].ToString().Trim()); //퇴직기산일

					//인트라넷
					txt_iden.Text = crow["EMBSIDEN"].ToString().Trim();
					txt_pswd.Text = crow["EMBSPSWD"].ToString().Trim();
					txt_emal.Text = crow["EMBSEMAL"].ToString().Trim();

					sl_glcd.EditValue = crow["EMBSGLCD"].ToString().Trim() == "" ? null : crow["EMBSGLCD"].ToString().Trim(); //사업부코드
					sl_dpcd.EditValue = crow["EMBSDPCD"].ToString().Trim() == "" ? null : crow["EMBSDPCD"].ToString().Trim(); //부서코드
					sl_jocd.EditValue = crow["EMBSJOCD"].ToString().Trim() == "" ? null : crow["EMBSJOCD"].ToString().Trim(); //직종코드
					sl_grcd.EditValue = crow["EMBSGRCD"].ToString().Trim() == "" ? null : crow["EMBSGRCD"].ToString().Trim(); //직급 

					txt_hobo.Text = crow["EMBSHOBO"].ToString().Trim(); //호봉
					mm_remk.Text = crow["EMBSDESC"].ToString().Trim();

					//dat_shdt.DateTime = clib.TextToDate(crow["SH_DATE"].ToString().Trim()); //차기승호일
																							//귀속일자
					//dat_frd3.DateTime = clib.TextToDate(crow["EMBSFRD3"].ToString().Trim()); //퇴직정산귀속일자
					//dat_tod3.DateTime = clib.TextToDate(crow["EMBSTOD3"].ToString().Trim());

					//PHOTO 컬럼 신규 생성!!(BLOB)
					//사진
					if (crow["PHOTO"].ToString().Trim() != "")
					{
						byte[] myByte = new byte[0];//버퍼 설정.
						myByte = (byte[])crow["PHOTO"]; //BLOB데이타를 byte[]로 변환
						if (myByte.Length != 0)
						{
							MemoryStream ImgStream = new MemoryStream(myByte);//메모리를 백업저장소로 사용해서 이미지 스트림을 생성
							pic_photo.Image = Image.FromStream(ImgStream);//스트림을 이미지로 변환해서 보여줌
						}
					}
					else
					{
						pic_photo.Image = null;
					}

					_Flag = "U";
					SetButtonEnable("0111");
					btn_save.Text = "수  정";
                    btn_save.Image = WAGE1000.Properties.Resources.수정;
				}
				else
				{
					dat_ipdt.DateTime = DateTime.Now;
					_Flag = "C";
					cmb_stat.SelectedIndex = 0;
					cmb_adgb.SelectedIndex = 0;
					SetButtonEnable("0101");
				}

				txt_code.Enabled = false;
				//srPanel7.Enabled = true;
				grbx1.Enabled = true;
				txt_name.Focus();				
			}
        }

        //저장버튼
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (isNoError_um(2))
            {
                Cursor = Cursors.WaitCursor;
                int outVal = 0;
                try
                {
                    DataRow hrow;

                    //if (_Flag == "C")  //신규
                    //{
                    //    hrow = ds.Tables["MSTEMBS"].NewRow();
                    //    hrow["G_CODE"] = txt_code.Text.ToString().Trim();
                    //}
                    //else //수정       //GNTECODE ,GNTE_SNM ,GNTE_FNM ,DP_COLOR ,GNMUTMFR ,GNMUTMTO ,GN_HOLTM ,GNMUTYPE ,AUTO_YN ,REQ_LIMT
                    //{
                    //    hrow = ds.Tables["DUTY_MSTGNMU"].Select("G_CODE = '" + txt_code.Text.ToString().Trim() + "'")[0];
                    //}
					
                    //hrow["USID"] = SilkRoad.Config.SRConfig.USID;

                    //if (_Flag == "C")  //신규
                    //{
                    //    hrow["INDT"] = gd.GetNow();
                    //    hrow["UPDT"] = "";
                    //    hrow["PSTY"] = "A";

                    //    ds.Tables["DUTY_MSTGNMU"].Rows.Add(hrow);
                    //}
                    //else //수정
                    //{
                    //    hrow["UPDT"] = gd.GetNow();
                    //    hrow["PSTY"] = "U";
                    //}
					
					if (_Flag == "C")  //신규
					{
						hrow = ds.Tables["MSTEMBS"].NewRow();
						hrow["EMBSSABN"] = txt_code.Text.ToString().Trim();
						hrow["EMBSINDT"] = gd.GetNow();
						hrow["EMBSUPDT"] = "";
						hrow["EMBSPSTY"] = "A";
						ds.Tables["MSTEMBS"].Rows.Add(hrow);
					}
					else //수정
					{
						hrow = ds.Tables["MSTEMBS"].Rows[0];
						hrow["EMBSUPDT"] = gd.GetNow();
						hrow["EMBSPSTY"] = "U";
					}

					hrow["EMBSNAME"] = txt_name.Text.ToString().Trim();
					hrow["EMBSJMNO"] = txt_jmno.Text.ToString().Trim().Replace("-", "");

					hrow["EMBSTLNO"] = txt_tlno.Text.ToString().Trim();
					hrow["EMBSHPNO"] = txt_hpno.Text.ToString().Trim();
					hrow["EMBSADR1"] = txt_adr1.Text.ToString().Trim();
					hrow["EMBSADR2"] = txt_adr2.Text.ToString().Trim();
					hrow["EMBSPOST"] = txt_post.Text.ToString().Trim().Replace("-", "");

					hrow["EMBSSTAT"] = (cmb_stat.SelectedIndex + 1).ToString();
					hrow["EMBSIPDT"] = clib.DateToText(dat_ipdt.DateTime); //입사일
					hrow["EMBSTSDT"] = clib.DateToText(dat_tsdt.DateTime); //퇴사일

					hrow["EMBSGLCD"] = sl_glcd.EditValue == null ? " " : sl_glcd.EditValue.ToString(); //사업부코드
					hrow["EMBSDPCD"] = sl_dpcd.EditValue == null ? " " : sl_dpcd.EditValue.ToString(); //부서코드
					hrow["EMBSJOCD"] = sl_jocd.EditValue == null ? " " : sl_jocd.EditValue.ToString(); //직종코드
					hrow["EMBSGRCD"] = sl_grcd.EditValue == null ? " " : sl_grcd.EditValue.ToString(); //직급 
					hrow["EMBSHOBO"] = txt_hobo.Text.Trim() == "" ? "" : txt_hobo.Text.Trim().PadLeft(3, '0');     //호봉
					
					//인트라넷
					hrow["EMBSIDEN"] = txt_iden.Text.Trim();
					hrow["EMBSPSWD"] = txt_pswd.Text.Trim();
					hrow["EMBSEMAL"] = txt_emal.Text.Trim();
					hrow["EMBSADGB"] = cmb_adgb.SelectedIndex.ToString();
					//hrow["EMBSADGB"] = chk_adgb.Checked == true ? "1" : "";
					hrow["EMBSDESC"] = mm_remk.Text.Trim();

					byte[] photo = ImageToByteArray(pic_photo.Image); //이미지를 byte로 반환
					hrow["PHOTO"] = photo.Length == 0 ? null : photo;
					hrow["EMBSUSID"] = SilkRoad.Config.SRConfig.USID;

					string[] tableNames = new string[] { "MSTEMBS" };

					int adgb = cmb_adgb.SelectedIndex;
					if (adgb == 1 || adgb == 2 || adgb == 5 || adgb == 6)  //팀장,부서장,담당원장,대표원장
					{
						df.GetCHK_MSTUSERDatas(txt_code.Text.ToString().Trim(), ds);
						if (ds.Tables["CHK_MSTUSER"].Rows.Count == 0)
						{
							DialogResult dr = MessageBox.Show("해당 사원의 로그인계정을 생성하시겠습니까?\r\n\r\n계정은 사번, 비번은 생년월일 6자리이며 메뉴권한은 따로 설정해주셔야 합니다.", "자동생성", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
							if (dr == DialogResult.OK)
							{
								df.GetMSTUSERDatas(ds);
								DataRow erow = ds.Tables["MSTUSER"].NewRow();
								erow["USERIDEN"] = txt_code.Text.ToString().Trim();
								erow["USERNAME"] = txt_name.Text.ToString().Trim();
								erow["USERPSWD"] = txt_jmno.Text.ToString().Trim().Replace("-", "").Substring(0, 6);
								erow["USERUPYN"] = "0";
								erow["USERMSYN"] = "0";
								erow["USERRDAT"] = "20991231";
								erow["USERINDT"] = gd.GetNow().Substring(0, 8);
								erow["USERUPDT"] = "";
								erow["USERUSID"] = SilkRoad.Config.SRConfig.USID;
								erow["USERPSTY"] = "A";
								ds.Tables["MSTUSER"].Rows.Add(erow);
								tableNames = new string[] { "MSTEMBS", "MSTUSER" };
							}
						}
					}

					SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
					outVal = cmd.setUpdate(ref ds, tableNames, null);

					if (outVal <= 0)
						MessageBox.Show("저장된 내용이 없습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
					else
						MessageBox.Show("저장되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ec)
                {
                    MessageBox.Show(ec.Message, "저장오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btn_refresh.PerformClick();
                    SetCancel();
                    Cursor = Cursors.Default;
                }
            }
        }
		
        //이미지를byte[]로 반환
        public byte[] ImageToByteArray(Image imageIn)
        {
            var ms = new MemoryStream();
            if (imageIn != null)            
                imageIn.Save(ms, imageIn.RawFormat);
            
            return ms.ToArray();
        }

        //삭제
        private void btn_del_Click(object sender, EventArgs e)
        {
			if (isNoError_um(3))
			{
				DialogResult dr = MessageBox.Show("해당 사원정보를 삭제하시겠습니까?", "삭제여부", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
				if (dr == DialogResult.OK)
				{
					int outVal = 0;
					try
					{
						df.GetMSTEMBSDatas(txt_code.Text.ToString(), ds);
						if (ds.Tables["MSTEMBS"].Rows.Count > 0)
						{

							ds.Tables["MSTEMBS"].Select("EMBSSABN = '" + txt_code.Text.ToString().Trim() + "'")[0].Delete();

							string[] tableNames = new string[] { "MSTEMBS" };
							SilkRoad.DbCmd_DT01.DbCmd_DT01 cmd = new SilkRoad.DbCmd_DT01.DbCmd_DT01();
							outVal = cmd.setUpdate(ref ds, tableNames, null);
						}

						if (outVal > 0)
							MessageBox.Show("해당 근무유형이 삭제되었습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					catch (Exception ec)
					{
						MessageBox.Show(ec.Message, "삭제오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					finally
					{
						btn_refresh.PerformClick();
						SetCancel();
						Cursor = Cursors.Default;
					}
				}
			}
        }

        /// <summary>취소버튼</summary>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("해당 내역을 취소하시겠습니까?", "취소", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                               == DialogResult.OK)
            {
                SetCancel();
            }
        }

		
		//우편번호검색
		private void btn_post_Click(object sender, EventArgs e)
		{			
            form_getPost2 post = new form_getPost2(txt_adr1.Text.ToString().Trim());
            if (post.ShowDialog() == DialogResult.OK)
            {
                txt_post.Text = post.postnum;
                txt_adr1.Text = post.address;
                txt_adr2.Focus();
                txt_adr2.SelectAll();
            }
		}

        //사진등록
        private void btn_saveImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "*.*";
            //openFileDialog.InitialDirectory = "c:\\silkroad\\progwage\\photo\\";
            openFileDialog.Filter = "JPG Files (*JPG)|*.JPG|GIF Files (*GIF)|*.GIF|PNG Files (*PNG)|*.PNG|All Files(*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.pic_photo.Image = Image.FromFile(openFileDialog.FileName);
            }

        }

        //사진삭제
        private void btn_delImage_Click(object sender, EventArgs e)
        {
            this.pic_photo.Image = null;
        }
        #endregion

        #region 3 EVENT

        private void duty1006_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   //취소
                btn_clear.PerformClick();            
        }
		
		//그리드 더블클릭시 코드 조회
		private void grdv1_DoubleClick(object sender, EventArgs e)
		{
			DataRow drow = grdv1.GetFocusedDataRow();
            if (drow == null)
                return;

			SetCancel();
			txt_code.Text = drow["EMBSSABN"].ToString().Trim();
			btn_proc.PerformClick();
		}		

        #endregion

        #region 7. Error Check

        /// <summary>
        /// 모드에 따른 컨트롤 유효성체크
        /// </summary>
        /// <param name="mode">1:처리모드(키값검사), 2:입력,수정모드 </param>
        /// <returns></returns>
        private bool isNoError_um(int mode)
        {
            bool isError = false;

			if (mode == 1)  //처리
			{
				if (txt_code.Text.ToString().Trim() == "")
				{
					MessageBox.Show(srLabel52.Text + "를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					txt_code.Focus();
					return false;
				}
				else if (txt_code.Text.ToString().Trim().Length < 5)
				{
					MessageBox.Show("5자리 사번을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					txt_code.Focus();
					return false;
				}
				else
				{
					isError = true;
				}
			}
			else if (mode == 2)  //저장
			{
				if (txt_name.Text.ToString().Trim() == "")
				{
					MessageBox.Show(labelControl2.Text + "을 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					txt_name.Focus();
					return false;
				}
				else if (txt_jmno.Text.ToString().Trim() == "")
				{
					MessageBox.Show(labelControl19.Text + "를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					txt_jmno.Focus();
					return false;
				}
				else if (clib.DateToText(dat_ipdt.DateTime) == "")
				{
					MessageBox.Show(labelControl16.Text + "를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					dat_ipdt.Focus();
					return false;
				}
				else if (sl_glcd.EditValue == null)
				{
					MessageBox.Show(labelControl10.Text + "를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_glcd.Focus();
					return false;
				}
				else if (sl_dpcd.EditValue == null)
				{
					MessageBox.Show(labelControl17.Text + "를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_dpcd.Focus();
					return false;
				}
				else if (sl_jocd.EditValue == null)
				{
					MessageBox.Show(labelControl9.Text + "를 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					sl_jocd.Focus();
					return false;
				}
				else if (Encoding.Default.GetByteCount(txt_iden.Text.ToString().Trim()) > 20)
				{
					MessageBox.Show(labelControl26.Text + "의 길이가 20byte를 초과하였습니다.\r\n(현재 " + Encoding.Default.GetByteCount(txt_iden.Text.ToString().Trim()) + "byte)", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					txt_iden.Focus();
					return false;
				}
				else if (Encoding.Default.GetByteCount(txt_pswd.Text.ToString().Trim()) > 10)
				{
					MessageBox.Show(labelControl27.Text + "의 길이가 10byte를 초과하였습니다.\r\n(현재 " + Encoding.Default.GetByteCount(txt_pswd.Text.ToString().Trim()) + "byte)", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
					txt_pswd.Focus();
					return false;
				}
				else
				{
					isError = true;
				}
			}
			//else if (mode == 3)  //삭제
			//{
			//	df.GetCHK_GNMUDatas(txt_code.Text.ToString(), ds);
			//	if (ds.Tables["CHK_GNMU"].Rows.Count > 0)
			//	{
			//		MessageBox.Show(txt_code.Text.ToString() + "[" + txt_name.Text.ToString() + "] 근무유형코드가 " + ds.Tables["CHK_GNMU"].Rows[0]["CHK_NM"].ToString() + " 테이블에서 사용중입니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//		txt_code.Focus();
			//		return false;
			//	}
			//	else
			//	{
			//		isError = true;
			//	}
			//}

            return isError;
        }

        #endregion

        #region 9. ETC

        /// <summary>
        /// 배열에따른 버튼상태설정
        /// </summary>
        /// <param name="mode"></param>
        private void SetButtonEnable(string arr)
        {
            btn_proc.Enabled = arr.Substring(0, 1) == "1" ? true : false;
            btn_save.Enabled = arr.Substring(1, 1) == "1" ? true : false;
            btn_del.Enabled  = arr.Substring(2, 1) == "1" ? true : false;
            btn_clear.Enabled = arr.Substring(3, 1) == "1" ? true : false;
        }

        private void btn_search_CK()
        {
			int r_index = grdv1.FocusedRowHandle;
			int t_index = grdv1.TopRowIndex;
			df.GetSEARCH_MSTEMBSDatas(ds);
			grd1.DataSource = ds.Tables["SEARCH_MSTEMBS"];
			grdv1.FocusedRowHandle = r_index;
			grdv1.TopRowIndex = t_index;

			df.GetWAGE_GLOVDatas(ds);
			sl_glcd.Properties.DataSource = ds.Tables["WAGE_GLOV"];
			df.GetWAGE_DEPRDatas(ds);
			sl_dpcd.Properties.DataSource = ds.Tables["WAGE_DEPR"];
			df.GetWAGE_JONGDatas(ds);
			sl_jocd.Properties.DataSource = ds.Tables["WAGE_JONG"];
			df.GetWAGE_GRADDatas(ds);
			sl_grcd.Properties.DataSource = ds.Tables["WAGE_GRAD"];
        }
		#endregion
		
	}
}
