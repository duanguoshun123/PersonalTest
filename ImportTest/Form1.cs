using ExcelImportHelper.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using ExcelImportHelper;
using System.Collections;

namespace ImportTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            List<Employee> em = new List<Employee> {
                new Employee() {
                    Name = "段国顺",
                    PhoneNumber = "18255182353",
                    Sex = "M",
                    Nation = "汉",
                    Birthday = "19910715",
                    Cardid = "34262619910715223X" }
            };

            this.comboBox1.Text = "附件名称：";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.ShowDialog();

            //得到上传文件的完整名
            string loadFullName = ofd.FileName.ToString();

            //上传文件的文件名
            string loadName = loadFullName.Substring(loadFullName.LastIndexOf("//") + 1);

            //上传文件的类型
            string loadType = loadFullName.Substring(loadFullName.LastIndexOf(".") + 1);

            this.comboBox1.Text += " " + loadFullName;
            ExcelImportService es = new ExcelImportService(loadName, @"../../XmlPath/Employee.xml");
            List<Employee> em = es.Import<Employee>();
            var datas = em.Select(p => new { 姓名 = p.Name, 手机号码 = p.PhoneNumber, 性别 = p.Sex, 民族 = p.Nation, 出生日期 = p.Birthday, 身份证号码 = p.Cardid }).ToList();
            DataTable dt = datas.ToDataTable();
            this.dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<LoversInfo> lovers = new List<LoversInfo>()
            {
                new LoversInfo() {
                    Name="阮婷婷",
                    Sex="女",
                    Birthday="1990-09-08",
                    Cardid="342626199009082223",
                    Nation="汉",
                    PhoneNumber="13871913800",
                    LoversName="段国顺"
                },
                new LoversInfo() {
                    Name="段国顺",
                    Sex="男",
                    Birthday="1991-07-15",
                    Cardid="34262619910715223X",
                    Nation="汉",
                    PhoneNumber="18255182353",
                    LoversName="阮婷婷"
                },
                new LoversInfo() {
                    Name="张巧巧",
                    Sex="女",
                    Birthday="1993-09-08",
                    Cardid="342626199309081125",
                    Nation="汉",
                    PhoneNumber="13871914551",
                    LoversName="朱承宇"
                },
                new LoversInfo() {
                    Name="朱承宇",
                    Sex="男",
                    Birthday="1993-01-25",
                    Cardid="342626199301252223",
                    Nation="汉",
                    PhoneNumber="13871913800",
                    LoversName="张巧巧"
                }
            };
            List<Employee> ems = new List<Employee>() {
                new Employee() {
                    Name="王杰",
                    Sex="男",
                    Birthday="1992-09-08",
                    Cardid="342626199201252223",
                    Nation="汉",
                    PhoneNumber="13871913800",
                },
                  new Employee() {
                    Name="张兴才",
                    Sex="男",
                    Birthday="1992-09-08",
                    Cardid="342626199201252223",
                    Nation="汉",
                    PhoneNumber="13871913800",
                }
            };
            List<AccountDetailsReport> listAccountDetailsReport = new List<AccountDetailsReport>()
            {

            };
            for (int i = 0; i < 100000; i++)
            {
                AccountDetailsReport data = new AccountDetailsReport()
                {
                    ReceiptRegisterNumber = "10021212122",
                    Year = "2018",
                    Corporation = "2018",
                    CorporationName = "山东凤祥食品发展有限公司" + i,
                    RegisterDate = new DateTime(2018, 2, 1),
                    TradeDate = new DateTime(2017, 10, 31),
                    RevenueAndExpenditureAttribute = "1",
                    Currency = "CNY",
                    TradeAmount = 600.56m,
                    OurAccount = "81010003000000003409",
                    OurOpenAccountName = "新凤祥财务有限公司",
                    OtherOpenAccountBank = "",
                    OurBankAccountName = "",
                    OurBankAttribute = "新凤祥财务公司",
                    OtherBankAccount = "16013701040003947",
                    OtherBankAccountName = "郑州红宇冷藏保鲜设备工程有限公司",
                    OurBankManageClass = "新凤祥财务公司",
                    OurBankType = "新凤祥财务公司",
                    Summary = "货款"
                };
                listAccountDetailsReport.Add(data);
            }
            KeyValuePair<string, string> keyPair = new KeyValuePair<string, string>("", "账户明细报表");
            List<KeyValuePair<List<object>, KeyValuePair<string, string>>> obj = new List<KeyValuePair<List<object>, KeyValuePair<string, string>>>() {
               new KeyValuePair<List<object>, KeyValuePair<string, string>>(ems.EntityToObjList(),new KeyValuePair<string, string>("员工列表信息","员工列表")),
               new KeyValuePair<List<object>, KeyValuePair<string, string>>(lovers.EntityToObjList(),keyPair),
            };
            //var objList = lovers.EntityToObjList();
            // var ms = ExportHepler.CreateExcelStreamByDatas(lovers, keyPair, @"../../XmlPath/LoversInfo.xml");
            //var ms2 = ExportHepler.CreateExcelStreamByDatas(obj, new List<string>() { @"../../XmlPath/EmployeeExport.xml", @"../../XmlPath/LoversInfo.xml" });
            listAccountDetailsReport.ForEach(x =>
            {
                if (x.RevenueAndExpenditureAttribute == "1")
                {
                    x.RevenueAndExpenditureAttribute = "收款";
                }
                else
                {
                    x.RevenueAndExpenditureAttribute = "付款";
                }
            });
            var listAccountDetailsReportNew = listAccountDetailsReport.Select(p => new
            {
                回单登记号 = p.ReceiptRegisterNumber,
                年度 = p.Year,
                公司名称 = p.CorporationName,
                登记日期 = p.RegisterDate,
                交易日期 = p.TradeDate,
                收付属性 = p.RevenueAndExpenditureAttribute,
                币种 = p.Currency,
                交易金额 = p.TradeAmount,
                我方账号 = p.OurAccount,
                我方开户户名 = p.OurOpenAccountName,
                我方银行户 = p.OurBankAccountName,
                我方所属银行 = p.OurBankAttribute,
                我方银行类别 = p.OurBankType,
                我方银行经营分类 = p.OurBankManageClass,
                对方银行账号 = p.OtherBankAccount,
                对方银行户名 = p.OtherBankAccountName,
                对方开户银行 = p.OtherOpenAccountBank,
                摘要 = p.Summary
            }).ToList();
            var dataTable = listAccountDetailsReportNew.ToDataTable();
            ExcelExportHelper2.ListColumnsName = new SortedList(new NoSort());
            ExcelExportHelper2.ListColumnsName.Add("ReceiptRegisterNumber", "回单登记号");
            ExcelExportHelper2.ListColumnsName.Add("Year", "年度");
            ExcelExportHelper2.ListColumnsName.Add("CorporationName", "公司名称");
            ExcelExportHelper2.ListColumnsName.Add("RegisterDate", "登记日期");
            ExcelExportHelper2.ListColumnsName.Add("TradeDate", "交易日期");
            ExcelExportHelper2.ListColumnsName.Add("RevenueAndExpenditureAttribute", "收付属性");
            ExcelExportHelper2.ListColumnsName.Add("Currency", "币种");
            ExcelExportHelper2.ListColumnsName.Add("TradeAmount", "交易金额");
            ExcelExportHelper2.ListColumnsName.Add("OurAccount", "我方账号");
            ExcelExportHelper2.ListColumnsName.Add("OurOpenAccountName", "我方开户户名");
            ExcelExportHelper2.ListColumnsName.Add("OurBankAccountName", "我方银行户");
            ExcelExportHelper2.ListColumnsName.Add("OurBankAttribute", "我方所属银行");
            ExcelExportHelper2.ListColumnsName.Add("OurBankType", "我方银行类别");
            ExcelExportHelper2.ListColumnsName.Add("OurBankManageClass", "我方银行经营分类");
            ExcelExportHelper2.ListColumnsName.Add("OtherBankAccount", "对方银行账号");
            ExcelExportHelper2.ListColumnsName.Add("OtherBankAccountName", "对方银行户名");
            ExcelExportHelper2.ListColumnsName.Add("OtherOpenAccountBank", "对方开户银行");
            ExcelExportHelper2.ListColumnsName.Add("Summary", "摘要");
            //List<KeyValuePair<List<AccountDetailsReport>, KeyValuePair<string, string>>> objMulSheet = new List<KeyValuePair<List<AccountDetailsReport>, KeyValuePair<string, string>>>()
            //{

            //};

            //var sheetNum = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(listAccountDetailsReport.Count) / 65536));//sheet个数

            //for (int i = 1; i <= sheetNum; i++)
            //{

            //    var newlist = listAccountDetailsReport.GetRange(0, listAccountDetailsReport.Count <= 65535 ? listAccountDetailsReport.Count : 65535);
            //    listAccountDetailsReport.RemoveRange(0, listAccountDetailsReport.Count <= 65535 ? listAccountDetailsReport.Count : 65535);
            //    objMulSheet.Add(new KeyValuePair<List<AccountDetailsReport>, KeyValuePair<string, string>>(newlist, new KeyValuePair<string, string>("账户明细报表", "账户明细报表_" + i)));
            //}
            ////var ms3 = ExportHepler.CreateExcelStreamByDatas(listAccountDetailsReport, keyPair, @"../../XmlPath/AccountDetailsReportsExport.xml");
            //var ms4 = ExportHepler.CreateExcelStreamByDatas(objMulSheet, @"../../XmlPath/AccountDetailsReportsExport.xml");
            //ExportHepler.ExportDataTableToExcel(dataTable);
            ExcelExportHelper2.ExportExcel(dataTable, System.Threading.Thread.GetDomain().BaseDirectory + "/Export/" + "信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
        }
    }
}
