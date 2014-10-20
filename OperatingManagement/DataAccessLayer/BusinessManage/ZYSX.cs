#region
//------------------------------------------------------
//Assembly:OperatingManagement.DataAccessLayer
//FileName:ZYSX.cs
//Remark:资源属性管理类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120825    Create     
//------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using Oracle.DataAccess.Client;

namespace OperatingManagement.DataAccessLayer.BusinessManage
{
    [Serializable]
    public class ZYSX : BaseEntity<int, ZYSX>
    {
        public ZYSX()
        {
            _dataBase = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region Properties
        private OracleDatabase _dataBase = null;
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PName { get; set; }
        /// <summary>
        /// 属性编码
        /// </summary>
        public string PCode { get; set; }
        /// <summary>
        /// 属性类型,1(int);2(double);3(string);4(bool);5(enum);
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 属性值区间,1(0-xxxxxx);2(m.n);3(length);5(a,b,c)';
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// 属于,0(卫星),1(地面站),2(卫星和地面站),3(都不属于)
        /// </summary>
        public int Own { get; set; }

        /// <summary>
        /// 资源属性值，扩展，与数据库不对应
        /// </summary>
        public dynamic PValue { get; set; }
        public static List<ZYSX> _zysxCache = null;
        public List<ZYSX> Cache
        {
            get
            {
                if (_zysxCache == null)
                {
                    _zysxCache = SelectAll();
                }
                return _zysxCache;
            }
            set
            {
                _zysxCache = value;
            }
        }
        #endregion

        #region -Private Methods-
        private OracleParameter PrepareRefCursor()
        {
            return new OracleParameter()
            {
                ParameterName = "o_Cursor",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.RefCursor
            };
        }

        private OracleParameter PrepareOutputResult()
        {
            return new OracleParameter()
            {
                ParameterName = "v_Result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Int32,
            };
        }

        private void RefreshCache()
        {
            this.Cache = SelectAll();
        }
        #endregion

        #region -Public Method-
        /// <summary>
        /// 根据ID获得资源属性实体
        /// </summary>
        /// <returns>资源属性实体</returns>
        public ZYSX SelectByID()
        {
            if (this.Cache != null)
            {
                List<ZYSX> lstSX = Cache.Where(t => t.Id == this.Id).ToList();
                if (lstSX.Count > 0)
                    return lstSX[0];
                else
                    return null;
            }
            else
            {
                OracleParameter o_Cursor = PrepareRefCursor();
                DataSet ds = _dataBase.SpExecuteDataSet("UP_ZYSX_SelectByID", new OracleParameter[] { new OracleParameter("p_ID", Id), o_Cursor });

                ZYSX info = null;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    info = new ZYSX()
                    {
                        Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]),
                        PName = ds.Tables[0].Rows[0]["PName"].ToString(),
                        PCode = ds.Tables[0].Rows[0]["PCode"].ToString(),
                        Type = ds.Tables[0].Rows[0]["Type"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["Type"]),
                        Scope = ds.Tables[0].Rows[0]["Scope"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["Scope"].ToString(),
                        Own = ds.Tables[0].Rows[0]["Own"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["Own"]),
                    };
                }
                return info;
            }
        }

        /// <summary>
        /// 获得所有资源属性实体列表
        /// </summary>
        /// <returns>资源属性实体列表</returns>
        public List<ZYSX> SelectAll()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_ZYSX_SelectAll", new OracleParameter[] { o_Cursor });

            List<ZYSX> infoList = new List<ZYSX>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ZYSX info = new ZYSX()
                    {
                        Id = Convert.ToInt32(dr["ID"]),
                        PName = dr["PName"].ToString(),
                        PCode = dr["PCode"].ToString(),
                        Type = dr["Type"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Type"]),
                        Scope = dr["Scope"] == DBNull.Value ? string.Empty : dr["Scope"].ToString(),
                        Own = dr["Own"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Own"]),
                    };

                    infoList.Add(info);
                }
            }
            return infoList;
        }
        /// <summary>
        /// 获得与地面站相关的资源属性列表
        /// 属性归属,0(卫星);1(地面站);2(卫星和地面站);3(都不归属);
        /// </summary>
        /// <returns></returns>
        public List<ZYSX> GetGroundStationZYSXList()
        {
            List<ZYSX> infoList = Cache.Where(a => a.Own == 1 || a.Own == 2).OrderBy(a=> a.PName).ToList<ZYSX>();
            return infoList;
        }
        /// <summary>
        /// 获得与卫星相关的资源属性列表
        /// 属性归属,0(卫星);1(地面站);2(卫星和地面站);3(都不归属);
        /// </summary>
        /// <returns></returns>
        public List<ZYSX> GetSatelliteZYSXList()
        {
            List<ZYSX> infoList = Cache.Where(a => a.Own == 0 || a.Own == 2).OrderBy(a => a.PName).ToList<ZYSX>();
            return infoList;
        }
        /// <summary>
        /// 从控件中获得用户填写的值
        /// 属性类型,1(int);2(double);3(string);4(bool);5(enum);
        /// </summary>
        /// <param name="placeHolder"></param>
        public void GetPValueFromControl(PlaceHolder placeHolder)
        {
            Control ctl = null;
            switch (Type)
            {
                case 1:
                case 2:    
                case 3:
                    ctl = placeHolder.FindControl("txt" + PCode + "_" + Id.ToString());
                    if ((ctl as TextBox) != null)
                        PValue = (ctl as TextBox).Text;
                    break;
                case 4:
                case 5:
                    ctl = placeHolder.FindControl("dpl" + PCode + "_" + Id.ToString());
                    if ((ctl as DropDownList) != null)
                        PValue = (ctl as DropDownList).SelectedValue;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 生成控件
        /// 属性类型,1(int);2(double);3(string);4(bool);5(enum);
        /// </summary>
        /// <returns></returns>
        public List<Control> GenerateControls()
        {
            List<Control> controlList = new List<Control>();
            switch (Type)
            {
                case 1:
                    controlList = CreateIntegerControls();
                    break;
                case 2:
                    controlList = CreateDoubleControls();
                    break;
                case 3:
                    controlList = CreateStringControls();
                    break;
                case 4:
                    controlList = CreateBoolControls();
                    break;
                case 5:
                    controlList = CreateEnumControls();
                    break;
                default:
                    break;
            }
            return controlList;
        }
        /// <summary>
        /// 创建整型数据类型相关控件
        /// </summary>
        /// <returns></returns>
        protected List<Control> CreateIntegerControls()
        {
            List<Control> controlsList = new List<Control>();
            string textBoxID = "txt" + PCode + "_" + Id.ToString();
            TextBox textBox = new TextBox();
            textBox.ID = textBoxID;
            textBox.Text = PValue;
            controlsList.Add(textBox);

            string requiredFieldValidatorID = "rfv" + PCode + "_" + Id.ToString();
            RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
            requiredFieldValidator.ID = requiredFieldValidatorID;
            requiredFieldValidator.ControlToValidate = textBoxID;
            requiredFieldValidator.Display = ValidatorDisplay.Dynamic;
            requiredFieldValidator.ForeColor = Color.Red;
            requiredFieldValidator.ErrorMessage = "（必填）";
            controlsList.Add(requiredFieldValidator);

            List<int> minAndMaxValue = GetIntegerMinAndMaxValue();
            if (minAndMaxValue != null && minAndMaxValue.Count == 2)
            {
                string rangeValidatorID = "rv" + PCode + "_" + Id.ToString();
                RangeValidator rangeValidator = new RangeValidator();
                rangeValidator.ID = rangeValidatorID;
                rangeValidator.ControlToValidate = textBoxID;
                rangeValidator.Type = ValidationDataType.Integer;
                rangeValidator.MinimumValue = minAndMaxValue[0].ToString();
                rangeValidator.MaximumValue = minAndMaxValue[1].ToString();
                rangeValidator.Display = ValidatorDisplay.Dynamic;
                rangeValidator.ForeColor = Color.Red;
                rangeValidator.ErrorMessage = string.Format("（请输入{0}-{1}之间整数数字）", minAndMaxValue[0], minAndMaxValue[1]);
                controlsList.Add(rangeValidator);
            }
            return controlsList;
        }
        /// <summary>
        /// 创建双精度数据类型相关控件
        /// </summary>
        /// <returns></returns>
        protected List<Control> CreateDoubleControls()
        {
            List<Control> controlsList = new List<Control>();
            string textBoxID = "txt" + PCode + "_" + Id.ToString();
            TextBox textBox = new TextBox();
            textBox.ID = textBoxID;
            textBox.Text = PValue;
            controlsList.Add(textBox);

            string requiredFieldValidatorID = "rfv" + PCode + "_" + Id.ToString();
            RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
            requiredFieldValidator.ID = requiredFieldValidatorID;
            requiredFieldValidator.ControlToValidate = textBoxID;
            requiredFieldValidator.Display = ValidatorDisplay.Dynamic;
            requiredFieldValidator.ForeColor = Color.Red;
            requiredFieldValidator.ErrorMessage = "（必填）";
            controlsList.Add(requiredFieldValidator);

            List<int> mn = GetDoubleMN();
            if (mn != null && mn.Count == 2)
            {
                string validationExpression = @"^(-?\d{1,m})(\.\d{1,n})?$";
                string regularExpressionValidatorID = "rev" + PCode + "_" + Id.ToString();
                RegularExpressionValidator regularExpressionValidator = new RegularExpressionValidator();
                regularExpressionValidator.ID = regularExpressionValidatorID;
                regularExpressionValidator.ControlToValidate = textBoxID;
                regularExpressionValidator.ValidationExpression = validationExpression.Replace("m", mn[0].ToString()).Replace("n", mn[1].ToString());
                regularExpressionValidator.Display = ValidatorDisplay.Dynamic;
                regularExpressionValidator.ForeColor = Color.Red;
                regularExpressionValidator.ErrorMessage = string.Format("（请输入整数位小于等于{0}位，小数位小于等于{1}位的数字）", mn[0], mn[1]);
                controlsList.Add(regularExpressionValidator);
            }
            return controlsList;
        }
        /// <summary>
        /// 创建字符串类型相关控件
        /// </summary>
        /// <returns></returns>
        protected List<Control> CreateStringControls()
        {
            List<Control> controlsList = new List<Control>();
            string textBoxID = "txt" + PCode + "_" + Id.ToString();
            TextBox textBox = new TextBox();
            textBox.ID = textBoxID;
            textBox.Text = PValue;
            textBox.MaxLength = GetStringLength();
            controlsList.Add(textBox);

            string requiredFieldValidatorID = "rfv" + PCode + "_" + Id.ToString();
            RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
            requiredFieldValidator.ID = requiredFieldValidatorID;
            requiredFieldValidator.ControlToValidate = textBoxID;
            requiredFieldValidator.Display = ValidatorDisplay.Dynamic;
            requiredFieldValidator.ForeColor = Color.Red;
            requiredFieldValidator.ErrorMessage = "（必填）";
            controlsList.Add(requiredFieldValidator);

            return controlsList;
        }
        /// <summary>
        /// 创建Bool类型相关控件
        /// </summary>
        /// <returns></returns>
        protected List<Control> CreateBoolControls()
        {
            List<Control> controlsList = new List<Control>();
            string dropDownListID = "dpl" + PCode + "_" + Id.ToString();
            DropDownList dropDownList = new DropDownList();
            dropDownList.ID = dropDownListID;
            dropDownList.DataSource = GetBoolDataSource();
            dropDownList.DataTextField = "key";
            dropDownList.DataValueField = "value";
            dropDownList.DataBind();
            dropDownList.SelectedIndex = dropDownList.Items.IndexOf(dropDownList.Items.FindByValue(PValue));
            controlsList.Add(dropDownList);

            string requiredFieldValidatorID = "rfv" + PCode + "_" + Id.ToString();
            RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
            requiredFieldValidator.ID = requiredFieldValidatorID;
            requiredFieldValidator.ControlToValidate = dropDownListID;
            requiredFieldValidator.Display = ValidatorDisplay.Dynamic;
            requiredFieldValidator.ForeColor = Color.Red;
            requiredFieldValidator.ErrorMessage = "（必填）";
            controlsList.Add(requiredFieldValidator);

            return controlsList;
        }
        /// <summary>
        /// 创建枚举类型相关控件
        /// </summary>
        /// <returns></returns>
        protected List<Control> CreateEnumControls()
        {
            List<Control> controlsList = new List<Control>();
            string dropDownListID = "dpl" + PCode + "_" + Id.ToString();
            DropDownList dropDownList = new DropDownList();
            dropDownList.ID = dropDownListID;
            dropDownList.DataSource = GetEnumDataSource();
            dropDownList.DataTextField = "key";
            dropDownList.DataValueField = "value";
            dropDownList.DataBind();
            dropDownList.SelectedIndex = dropDownList.Items.IndexOf(dropDownList.Items.FindByValue(PValue));
            controlsList.Add(dropDownList);

            string requiredFieldValidatorID = "rfv" + PCode + "_" + Id.ToString();
            RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
            requiredFieldValidator.ID = requiredFieldValidatorID;
            requiredFieldValidator.ControlToValidate = dropDownListID;
            requiredFieldValidator.Display = ValidatorDisplay.Dynamic;
            requiredFieldValidator.ForeColor = Color.Red;
            requiredFieldValidator.ErrorMessage = "（必填）";
            controlsList.Add(requiredFieldValidator);

            return controlsList;
        }
        /// <summary>
        /// 校验扩展属性值是否合法
        /// 属性类型,1(int);2(double);3(string);4(bool);5(enum);
        /// 属性值区间,1(0-xxxxxx);2(m.n);3(length);5(a,b,c)';
        /// </summary>
        /// <returns>true:通过</returns>
        public bool ValidatePValue()
        {
            bool result = false;
            try
            {
                switch (Type)
                {
                    case 1:
                        int value1;
                        if (int.TryParse(PValue.ToString(), out value1))
                        {
                            List<int> minAndMaxValue = GetIntegerMinAndMaxValue();
                            if (minAndMaxValue != null && minAndMaxValue.Count == 2)
                            {
                                result = (value1 >= minAndMaxValue[0] && value1 <= minAndMaxValue[1]);
                            }
                        }
                        break;
                    case 2:
                        double value2;
                        if (double.TryParse(PValue.ToString(), out value2))
                        {
                            value2 = Math.Abs(value2);
                            List<int> mn = GetDoubleMN();
                            if (mn != null && mn.Count == 2)
                            {
                                string[] array = value2.ToString().Split('.');
                                result = (array[0].Length <= mn[0] && (array.Length < 2 || array[1].Length <= mn[1]));
                            }
                        }
                        break;
                    case 3:
                        result = (PValue.ToString().Length <= GetStringLength());
                        break;
                    case 4:
                        result = (PValue != null);
                        break;
                    case 5:
                        result = (PValue != null);
                        break;
                    default:
                        result = true;
                        break;
                }
            }
            catch
            { }
            return result;
        }
        
        /// <summary>
        /// 获得整型数据的最小值与最大值
        /// </summary>
        /// <returns></returns>
        protected List<int> GetIntegerMinAndMaxValue()
        {
            List<int> minAndMaxValue = new List<int>();
            string[] scopeArray = Scope.Split(new char[] { '-', '_', '—', }, StringSplitOptions.RemoveEmptyEntries);
            if (scopeArray != null && scopeArray.Length == 2)
            {
                int minValue = 0;
                int maxValue = 0;
                if (int.TryParse(scopeArray[0], out minValue) && int.TryParse(scopeArray[1], out maxValue))
                {
                    minAndMaxValue.Add(minValue);
                    minAndMaxValue.Add(maxValue);
                }
            }
            return minAndMaxValue;
        }
        /// <summary>
        /// 获得双精度数据的整数位数和小数位数
        /// </summary>
        /// <returns></returns>
        protected List<int> GetDoubleMN()
        {
            List<int> mn = new List<int>();
            string[] mnArray = Scope.Split(new char[] { '.', ',', ';', '，', '；' }, StringSplitOptions.RemoveEmptyEntries);
            if (mnArray != null && mnArray.Length == 2)
            {
                int m = 0;
                int n = 0;
                if (int.TryParse(mnArray[0], out m) && int.TryParse(mnArray[1], out n))
                {
                    mn.Add(m);
                    mn.Add(n);
                }
            }
            return mn;
        }
        /// <summary>
        /// 获得字符串长度
        /// </summary>
        /// <returns></returns>
        protected int GetStringLength()
        {
            int length = 0;
            int.TryParse(Scope, out length);
            return length;
        }
        /// <summary>
        /// 获得bool型数据源
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, string> GetBoolDataSource()
        {
            Dictionary<string, string> boolDataSource = new Dictionary<string, string>();
            string[] boolArray = Scope.Split(new char[] { '.', ',', ';', '，', '；' });
            if (boolArray != null && boolArray.Length == 2)
            {
                foreach (string s in boolArray)
                {
                    if (!boolDataSource.ContainsKey(s))
                    {
                        boolDataSource.Add(s, s);
                    }
                }
            }
            else
            {
                boolDataSource.Add("是", "1");
                boolDataSource.Add("否", "0");
            }
            return boolDataSource;
        }
        /// <summary>
        /// 获得枚举类型数据源
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, string> GetEnumDataSource()
        {
            Dictionary<string, string> enumDataSource = new Dictionary<string, string>();
            string[] enumArray = Scope.Split(new char[] { '.', ',', ';', '，', '；' });
            if (enumArray != null && enumArray.Length > 0)
            {
                foreach (string s in enumArray)
                {
                    if (!enumDataSource.ContainsKey(s))
                    {
                        enumDataSource.Add(s, s);
                    }
                }
            }
            return enumDataSource;
        }

        /// <summary>
        /// Insert a ZYSX.
        /// </summary>
        /// <returns></returns>
        public FieldVerifyResult Add()
        {
            OracleParameter p = new OracleParameter()
            {
                ParameterName = "v_result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            OracleParameter opId = new OracleParameter()
            {
                ParameterName = "v_Id",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            _dataBase.SpExecuteNonQuery("UP_ZYSX_Insert", new OracleParameter[]{
                new OracleParameter("p_PName",this.PName),
                new OracleParameter("p_PCode",this.PCode),
                new OracleParameter("p_Type",this.Type),
                new OracleParameter("p_Scope",this.Scope),
                new OracleParameter("p_Own",this.Own),
                opId,
                p
            });
            if (opId.Value != null && opId.Value != DBNull.Value)
                this.Id = Convert.ToInt32(opId.Value.ToString());
            RefreshCache();
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }

        public FieldVerifyResult Update()
        {
            OracleParameter p = new OracleParameter()
            {
                ParameterName = "v_result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            _dataBase.SpExecuteNonQuery("UP_ZYSX_Update", new OracleParameter[]{
                new OracleParameter("p_Id", this.Id),
                new OracleParameter("p_PName",this.PName),
                new OracleParameter("p_PCode",this.PCode),
                new OracleParameter("p_Type",this.Type),
                new OracleParameter("p_Scope",this.Scope),
                new OracleParameter("p_Own",this.Own),
                p
            });
            RefreshCache();
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }

        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
        }
        #endregion
    }

    [Serializable]
    public class ZYSXExt
    {
        public ZYSXExt()
        { 
        }
        public ZYSXExt(ZYSX zysx)
        {
            if (zysx != null)
            {
                ID = zysx.Id;
                PName = zysx.PName;
                PCode = zysx.PCode;
                Type = zysx.Type;
                Scope = zysx.Scope;
                Own = zysx.Own;
            }
        }
        #region Properties
        /// <summary>
        /// 属性ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PName { get; set; }
        /// <summary>
        /// 属性编码
        /// </summary>
        public string PCode { get; set; }
        /// <summary>
        /// 属性类型,1(int);2(double);3(string);4(bool);5(enum);
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 属性值区间,1(0-xxxxxx);2(m.n);3(length);5(a,b,c)';
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// 属于,0(卫星),1(地面站),2(卫星和地面站),3(都不属于)
        /// </summary>
        public int Own { get; set; }

        /// <summary>
        /// 资源属性值唯一编码，扩展，与数据库不对应
        /// </summary>
        public string PValueID { get; set; }
        /// <summary>
        /// 资源属性值，扩展，与数据库不对应
        /// </summary>
        public string PValue { get; set; }
        #endregion
    }
}
