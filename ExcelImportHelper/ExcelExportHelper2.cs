using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelImportHelper
{
    public class ExcelExportHelper2
    {
        /// <summary>
        /// 列名集合
        /// </summary>
        public static System.Collections.SortedList ListColumnsName;
        /// <summary>
        /// 导出Excel通过文件路径
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="filePath">文件路径</param>
        public static void ExportExcel(DataTable dtSource, string filePath)
        {
            if (ListColumnsName == null || ListColumnsName.Count == 0)
                throw (new Exception("请对ListColumnsName设置要导出的列名！"));

            XSSFWorkbook excelWorkbook = CreateExcelFile();
            InsertRow(dtSource, excelWorkbook);
            SaveExcelFile(excelWorkbook, filePath);
        }
        /// <summary>
        /// 导出Excel通过流
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelStream">流操作</param>
        public static void ExportExcel(DataTable dtSource, Stream excelStream)
        {
            if (ListColumnsName == null || ListColumnsName.Count == 0)
                throw (new Exception("请对ListColumnsName设置要导出的列明！"));

            XSSFWorkbook excelWorkbook = CreateExcelFile(); //新建一个工作区域
            InsertRow(dtSource, excelWorkbook);//导入数据的方法
            SaveExcelFile(excelWorkbook, excelStream);//保存excel
        }

        /// <summary>
        /// 保存Excel文件通过路径
        /// </summary>
        /// <param name="excelWorkBook"></param>
        /// <param name="filePath"></param>
        protected static void SaveExcelFile(XSSFWorkbook excelWorkBook, string filePath)
        {
            FileStream file = null;
            try
            {
                file = new FileStream(filePath, FileMode.Create);
                excelWorkBook.Write(file);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }
        /// <summary>
        /// 保存Excel文件
        /// </summary>
        /// <param name="excelWorkBook"></param>
        /// <param name="filePath"></param>
        protected static void SaveExcelFile(XSSFWorkbook excelWorkBook, Stream excelStream)
        {
            try
            {
                excelWorkBook.Write(excelStream);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 创建Excel文件
        /// </summary>
        /// <param name="filePath"></param>
        protected static XSSFWorkbook CreateExcelFile()
        {
            XSSFWorkbook hssfworkbook = new XSSFWorkbook();
            return hssfworkbook;
        }

        /// <summary>
        /// 创建excel表头
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="excelSheet"></param>
        protected static void CreateHeader(ISheet excelSheet)
        {
            int cellIndex = 0;
            //循环导出列
            foreach (System.Collections.DictionaryEntry de in ListColumnsName)
            {
                IRow newRow = excelSheet.CreateRow(0);
                ICell newCell = newRow.CreateCell(cellIndex);
                newCell.SetCellValue(de.Value.ToString());
                cellIndex++;
            }
        }

        /// <summary>
        /// 插入数据行
        /// </summary>
        protected static void InsertRow(DataTable dtSource, XSSFWorkbook excelWorkbook)
        {
            int rowCount = 0;
            int sheetCount = 1;
            ISheet newsheet = null;

            //循环数据源导出数据集
            newsheet = excelWorkbook.CreateSheet("Sheet" + sheetCount); //创建第一个sheet
            CreateHeader(newsheet);//加载sheet的头信息
            foreach (DataRow dr in dtSource.Rows)  //循环DataTable里面的数据行
            {
                rowCount++;
                //超出10000条数据 创建新的工作簿
                if (rowCount == 65535)   //超过10000行数据就新建一个sheet
                {
                    rowCount = 1;
                    sheetCount++;
                    newsheet = excelWorkbook.CreateSheet("Sheet" + sheetCount); //新建sheet
                    CreateHeader(newsheet);//新建sheet的头信息
                }

                IRow newRow = newsheet.CreateRow(rowCount);//创建sheet的当前行
                InsertCell(dtSource, dr, newRow, newsheet, excelWorkbook);  //往当前行里面的每一列循环添加数据
            }
        }

        /// <summary>
        /// 导出数据行
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="drSource"></param>
        /// <param name="currentExcelRow"></param>
        /// <param name="excelSheet"></param>
        /// <param name="excelWorkBook"></param>
        protected static void InsertCell(DataTable dtSource, DataRow drSource, IRow currentExcelRow, ISheet excelSheet, XSSFWorkbook excelWorkBook)
        {
            for (int cellIndex = 0; cellIndex < ListColumnsName.Count; cellIndex++)
            {
                //列名称
                string columnsName = ListColumnsName.GetKey(cellIndex).ToString();
                ICell newCell = null;
                System.Type rowType = drSource[cellIndex].GetType();
                string drValue = drSource[cellIndex].ToString().Trim();
                switch (rowType.ToString())
                {
                    case "System.String"://字符串类型
                        drValue = drValue.Replace("&", "&");
                        drValue = drValue.Replace(">", ">");
                        drValue = drValue.Replace("<", "<");
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(drValue);
                        break;
                    case "System.DateTime"://日期类型
                        DateTime dateV;
                        DateTime.TryParse(drValue, out dateV);
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(dateV);

                        //格式化显示
                        ICellStyle cellStyle = excelWorkBook.CreateCellStyle();
                        IDataFormat format = excelWorkBook.CreateDataFormat();
                        cellStyle.DataFormat = format.GetFormat("yyyy-mm-dd hh:mm:ss");
                        newCell.CellStyle = cellStyle;

                        break;
                    case "System.Boolean"://布尔型
                        bool boolV = false;
                        bool.TryParse(drValue, out boolV);
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(boolV);
                        break;
                    case "System.Int16"://整型
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Byte":
                        int intV = 0;
                        int.TryParse(drValue, out intV);
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(intV.ToString());
                        break;
                    case "System.Decimal"://浮点型
                    case "System.Double":
                        double doubV = 0;
                        double.TryParse(drValue, out doubV);
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue(doubV);
                        break;
                    case "System.DBNull"://空值处理
                        newCell = currentExcelRow.CreateCell(cellIndex);
                        newCell.SetCellValue("");
                        break;
                    default:
                        throw (new Exception(rowType.ToString() + "：类型数据无法处理!"));
                }

            }
        }
    }
}
//排序实现接口 不进行排序 根据添加顺序导出

public class NoSort : System.Collections.IComparer
{
    public int Compare(object x, object y)
    {
        return -1;
    }
}
