using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Word;
using System.IO;
using OperatingManagement.DataAccessLayer.PlanManage;
using OperatingManagement.Framework.Storage;
using OperatingManagement.Framework.Components;

namespace ServicesKernel.File
{
    public class WordHandle
    {
        object oMissing = System.Reflection.Missing.Value;
        Word._Application oWord;
        Word._Document oDoc;

        public string DJZYJHFileFullName
        {
            get
            {
                string filename = "DJZYJH_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                string filepath = System.Configuration.ConfigurationManager.AppSettings["WordPath"];
                return filepath + filename;  //文件保存路径
            }
        }
        public object DJZYSQFileFullName
        {
            get
            {
                string filename = "DJZYSQ_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                string filepath = System.Configuration.ConfigurationManager.AppSettings["WordPath"];
                return filepath + filename;  //文件保存路径
            }
        }

        public string CreateDJZYSQFile(DJZYSQ objSQ)
        {
            //创建Word文档
            oWord = new Word.Application();
            oWord.Visible = false;
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            //页面设置，设置页面为纵向布局，设置纸张类型为A4纸  
            oDoc.PageSetup.Orientation = Word.WdOrientation.wdOrientPortrait;
            oDoc.PageSetup.PageWidth = oWord.CentimetersToPoints(29.7F);
            oDoc.PageSetup.PageHeight = oWord.CentimetersToPoints(21F);  

            //文档中创建表格
            Word.Table newTable = oDoc.Tables.Add(oWord.Selection.Range, 1, 9, ref oMissing, ref oMissing);
            //设置表格样式
            newTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleThickThinLargeGap;
            newTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            //newTable.Columns[1].Width = 100f;
            //newTable.Columns[2].Width = 220f;
            //newTable.Columns[3].Width = 105f;

            #region 填充表格表头内容						
            newTable.Cell(1, 1).Range.Text = "任务日期";
            newTable.Cell(1, 1).Range.Bold = 2;//设置单元格中字体为粗体
            newTable.Cell(1, 2).Range.Text = "站名";
            newTable.Cell(1, 2).Range.Bold = 2;//设置单元格中字体为粗体
            newTable.Cell(1, 3).Range.Text = "设备代号";
            newTable.Cell(1, 3).Range.Bold = 2;//设置单元格中字体为粗体
            newTable.Cell(1, 4).Range.Text = "频段";
            newTable.Cell(1, 4).Range.Bold = 2;//设置单元格中字体为粗体
            newTable.Cell(1, 5).Range.Text = "圈次";
            newTable.Cell(1, 5).Range.Bold = 2;//设置单元格中字体为粗体
            newTable.Cell(1, 6).Range.Text = "任务开始时间";
            newTable.Cell(1, 6).Range.Bold = 2;//设置单元格中字体为粗体
            newTable.Cell(1, 7).Range.Text = "跟踪开始时间";
            newTable.Cell(1, 7).Range.Bold = 2;//设置单元格中字体为粗体
            newTable.Cell(1, 8).Range.Text = "跟踪结束时间";
            newTable.Cell(1, 8).Range.Bold = 2;//设置单元格中字体为粗体
            newTable.Cell(1, 9).Range.Text = "任务结束时间";
            newTable.Cell(1, 9).Range.Bold = 2;//设置单元格中字体为粗体
            #endregion

            List<PlanParameter> listPara = PlanParameters.ReadParameters("DJZYSQPDXZ");
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime preTime = new DateTime();
            Word.Row row;
            string strDP = "";
            for (int i = 0; i < objSQ.DMJHTasks.Count; i++)
            {
                //在表格中增加行
                row = oDoc.Content.Tables[1].Rows.Add(ref oMissing);
                preTime = DateTime.ParseExact(objSQ.DMJHTasks[i].ZHB, "yyyyMMddHHmmss", provider);
                row.Cells[1].Range.Text = preTime.ToString("yyyy年MM月dd日");
                row.Cells[2].Range.Text = objSQ.DMJHTasks[i].GZDY;
                row.Cells[3].Range.Text = objSQ.DMJHTasks[i].SBDH;
                for (int j = 0; j < objSQ.DMJHTasks[i].GZDPs.Count;j++ )
                {
                    foreach (PlanParameter p in listPara)
                    {
                        if (p.Value == objSQ.DMJHTasks[i].GZDPs[j].PDXZ)
                        {
                            strDP += p.Text;
                            break;
                        }
                    }
                    if (j < objSQ.DMJHTasks[i].GZDPs.Count-1)
                    { strDP += "\r\n"; }
                }
               row.Cells[4].Range.Text = strDP;
               row.Cells[5].Range.Text = objSQ.DMJHTasks[i].QC;
               row.Cells[6].Range.Text = objSQ.DMJHTasks[i].RK;
               row.Cells[7].Range.Text = objSQ.DMJHTasks[i].GZK;
               row.Cells[8].Range.Text = objSQ.DMJHTasks[i].GZJ;
               row.Cells[9].Range.Text = objSQ.DMJHTasks[i].JS;
               
            }
            //插入换行符
            //newWordDoc.Sections[1].Range.InsertBreak(ref type); 

            object filename = DJZYSQFileFullName;
            oDoc.SaveAs(ref filename, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.Close(ref oMissing, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);

            return filename.ToString();
        }

        public string CreateDJZYJHFile(DJZYJH objJH)
        {
            //创建Word文档
            oWord = new Word.Application();
            oWord.Visible = false;
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            //页面设置，设置页面为纵向布局，设置纸张类型为A4纸  
            oDoc.PageSetup.Orientation = Word.WdOrientation.wdOrientPortrait;
            oDoc.PageSetup.PageWidth = oWord.CentimetersToPoints(29.7F);
            oDoc.PageSetup.PageHeight = oWord.CentimetersToPoints(21F);

            //文档中创建表格
            Word.Table newTable = oDoc.Tables.Add(oWord.Selection.Range, 1, 4, ref oMissing, ref oMissing);
            //设置表格样式
            newTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleThickThinLargeGap;
            newTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

            #region 填充表格表头内容
            //newTable.Cell(1, 1).Range.Text = "任务日期";
            //newTable.Cell(1, 1).Range.Bold = 2;//设置单元格中字体为粗体
            //newTable.Cell(1, 2).Range.Text = "站名";
            //newTable.Cell(1, 2).Range.Bold = 2;//设置单元格中字体为粗体
            //newTable.Cell(1, 3).Range.Text = "设备代号";
            //newTable.Cell(1, 3).Range.Bold = 2;//设置单元格中字体为粗体
            //newTable.Cell(1, 4).Range.Text = "频段";
            //newTable.Cell(1, 4).Range.Bold = 2;//设置单元格中字体为粗体
            #endregion

            List<PlanParameter> listPara = PlanParameters.ReadParameters("DJZYSQPDXZ");
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime preTime = new DateTime();
            Word.Row row;
            string strDP = "";
            for (int i = 0; i < objJH.DJZYJHPlans.Count; i++)
            {
                //在表格中增加行
                row = oDoc.Content.Tables[1].Rows.Add(ref oMissing);
                preTime = DateTime.ParseExact(objJH.DJZYJHPlans[i].ZHB, "yyyyMMddHHmmss", provider);
                row.Cells[1].Range.Text = preTime.ToString("yyyy年MM月dd日");
                row.Cells[2].Range.Text = objJH.DJZYJHPlans[i].GZDY;
                row.Cells[3].Range.Text = objJH.DJZYJHPlans[i].SBDH;
                for (int j = 0; j < objJH.DJZYJHPlans[i].DJZYJHGZDPs.Count; j++)
                {
                    foreach (PlanParameter p in listPara)
                    {
                        if (p.Value == objJH.DJZYJHPlans[i].DJZYJHGZDPs[j].PDXZ)
                        {
                            strDP += p.Text;
                            break;
                        }
                    }
                    if (j < objJH.DJZYJHPlans[i].DJZYJHGZDPs.Count - 1)
                    { strDP += "\r\n"; }
                }
                row.Cells[4].Range.Text = strDP;
                //row.Cells[5].Range.Text = objJH.DMJHTasks[i].QC;
                //row.Cells[6].Range.Text = objJH.DMJHTasks[i].RK;
                //row.Cells[7].Range.Text = objJH.DMJHTasks[i].GZK;
                //row.Cells[8].Range.Text = objJH.DMJHTasks[i].GZJ;
                //row.Cells[9].Range.Text = objJH.DMJHTasks[i].JS;

            }

            object filename = DJZYJHFileFullName;
            oDoc.SaveAs(ref filename, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.Close(ref oMissing, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);

            return filename.ToString();
        }
        


    }//
}
