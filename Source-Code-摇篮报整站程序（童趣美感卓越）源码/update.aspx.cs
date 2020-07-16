using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Helper;
using ZoomLa.Common;
using ZoomLa.Components;
using ZoomLa.Model;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;

public partial class Tools_update : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        B_Admin.CheckIsLogged(Request.RawUrl);
        B_Admin.IsSuperManage();
    }
    //将节点数据导入ID不变
    protected void NodeXmlToDB_Btn_Click(object sender, EventArgs e)
    {
        B_Node nodeBll = new B_Node();
        //SiteConfig.SiteOption.TemplateDir = "/Template/Commit";// /Template/V3
        string nodemap = Server.MapPath(SiteConfig.SiteOption.TemplateDir + "/配置库/节点/Directory.node");
        string nodedir =  Server.MapPath(SiteConfig.SiteOption.TemplateDir + "/配置库/节点/NodeList/");//1.node
        string flag = function.GetRandomString(6);
        DataSet ds = new DataSet();
        ds.ReadXml(nodemap);
        DataTable nodemapDT = ds.Tables[0];
        SqlHelper.ExecuteNonQuery(CommandType.Text, "TRUNCATE TABLE ZL_Node");
        //------
        foreach (DataRow dr in nodemapDT.Rows)
        {
            M_Node model = new M_Node();
            model.NodeID = Convert.ToInt32(dr["NodeID"]);
            model.NodeName = ConverToStr(dr["NodeName"]);
            model.ParentID = ConvertToInt(dr["ParentID"]);
            model.OrderID = ConvertToInt(dr["OrderID"]);
            model.Depth = ConvertToInt(dr["Depth"]);
            model.NodeType = ConvertToInt(dr["NodeType"]);
            model.NodeListType = ConvertToInt(dr["NodeListType"]);
            if (File.Exists(nodedir + model.NodeID + ".node"))
            {
                try
                {
                    DataSet nodeds = new DataSet();
                    nodeds.ReadXml(nodedir + model.NodeID + ".node");
                    DataRow rdr = nodeds.Tables[0].Rows[0];
                    model.Child = ConvertToInt(rdr["Child"]);
                    model.Tips = ConverToStr(rdr["Tips"]);
                    model.NodeDir = ConverToStr(rdr["NodeDir"]);
                    model.NodePic = ConverToStr(rdr["NodePicUrl"]);
                    model.NodeUrl = ConverToStr(rdr["NodeUrl"]);
                    model.NodeListUrl = ConverToStr(rdr["NodeListUrl"]);
                    model.Description = ConverToStr(rdr["Description"]);
                    model.Meta_Keywords = ConverToStr(rdr["Meta_Keywords"]);
                    model.Meta_Description = ConverToStr(rdr["Meta_Description"]);
                    model.OpenNew = ConverToBool(rdr["OpenType"]);
                    model.PurviewType = ConverToBool(rdr["PurviewType"]);
                    model.CommentType = ConverToStr(rdr["CommentType"]);
                    model.HitsOfHot = ConvertToInt(rdr["HitsOfHot"]);
                    model.ListTemplateFile = ConverToStr(rdr["ListTemplateFile"]);
                    model.IndexTemplate = ConverToStr(rdr["IndexTemplate"]);
                    model.LastinfoTemplate = ConverToStr(rdr["LastinfoTemplate"]);
                    model.HotinfoTemplate = ConverToStr(rdr["HotinfoTemplate"]);
                    model.ProposeTemplate = ConverToStr(rdr["ProposeTemplate"]);
                    model.ContentModel = ConverToStr(rdr["ContentModel"]);
                    model.ItemOpenType = ConverToBool(rdr["ItemOpenType"]);
                    model.ContentPageHtmlRule = ConvertToInt(rdr["ContentHtmlRule"]);
                    model.ListPageHtmlEx = ConvertToInt(rdr["ListPageHtmlEx"]);
                    model.ListPageEx = ConvertToInt(rdr["ListPageEx"]);
                    model.LastinfoPageEx = ConvertToInt(rdr["LastinfoPageEx"]);
                    model.HotinfoPageEx = ConvertToInt(rdr["HotinfoPageEx"]);
                    model.ProposePageEx = ConvertToInt(rdr["ProposePageEx"]);
                    model.ContentFileEx = ConvertToInt(rdr["ContentFileEx"]);
                    model.HtmlPosition = ConvertToInt(rdr["HtmlPosition"]);


                    model.ConsumePoint = ConvertToInt(rdr["ConsumePoint"]);
                    model.ConsumeType = ConvertToInt(rdr["ConsumeType"]);
                    model.ConsumeTime = ConvertToInt(rdr["ConsumeTime"]);
                    model.ConsumeCount = ConvertToInt(rdr["ConsumeCount"]);
                    model.Shares = (float)ConverToDouble(rdr["Shares"]);
                    model.Custom = ConverToStr(rdr["Custom"]);
                    model.SafeGuard = ConvertToInt(rdr["SafeGuard"]);
                    model.OpenTypeTrue = ConverToStr(rdr["OpenTypeTrue"]);
                    model.ItemOpenTypeTrue = ConverToStr(rdr["ItemOpenTypeTrue"]);
                   
                    model.Contribute = ConvertToInt(rdr["Contribute"]);
                    model.SiteContentAudit = ConvertToInt(rdr["SiteContentAudit"]);
                    model.AuditNodeList = ConverToStr(rdr["AuditNodeList"]);
                    model.AddMoney = ConverToDouble(rdr["AddMoney"]);
                    model.AddPoint = ConvertToInt(rdr["AddPoint"]);
                    model.ClickTimeout = ConvertToInt(rdr["ClickTimeout"]);
                    model.Purview = ConverToStr(rdr["Purview"]);
                    model.AddUserExp = ConvertToInt(rdr["AddUserExp"]);
                    model.DeducUserExp = ConvertToInt(rdr["DeducUserExp"]);
                    model.Viewinglimit = ConverToStr(rdr["Viewinglimit"]);

                    model.NodeBySite = 0;
                    model.SiteConfige = flag;
                    model.CDate = DateTime.Now;
                    model.EditDate = DateTime.Now;
                }
                catch (Exception ex) { ZLLog.L("导入节点[" + model.NodeID + "," + model.NodeName + "]详情失败,原因:" + ex.Message); }
            }//for end;

            Insert(model);
        }
        function.WriteSuccessMsg("节点导入完成");
    }
    B_Model modBll = new B_Model();
    B_ModelField fieldBll = new B_ModelField();
    //将模型与模型字段导入,ID不变
    protected void ModelXmlToDB_Btn_Click(object sender, EventArgs e)
    {
        //SiteConfig.SiteOption.TemplateDir = "/Template/Commit";// /Template/V3
        string modmap = Server.MapPath(SiteConfig.SiteOption.TemplateDir + "/配置库/模型/Directory.model");
        //string moddir = Server.MapPath(SiteConfig.SiteOption.TemplateDir + "/配置库/模型/NodeList/");
        DataSet ds = new DataSet();
        ds.ReadXml(modmap);
        DataTable modmapdt = ds.Tables[0];
        foreach (string field in "ItemUnit,ContentTemplate,MultiFlag,NodeID,FromModel,Thumbnail,Islotsize".Split(','))
        {
            if (!modmapdt.Columns.Contains(field)) { modmapdt.Columns.Add(new DataColumn(field, typeof(string))); }
        }
        SqlHelper.ExecuteSql("TRUNCATE TABLE ZL_Model");
        SqlHelper.ExecuteSql("TRUNCATE TABLE ZL_ModelField");
        //---------------------
        foreach (DataRow rdr in modmapdt.Rows)
        {
            M_ModelInfo model = new M_ModelInfo().GetModelFromReader(rdr);
            if (rdr["ModelType"].ToString().Equals("会员模型")) { rdr["ModelType"] = "用户模型"; }
            if (rdr["ModelType"].ToString().Equals("店铺模型")) { rdr["ModelType"] = "店铺申请模型"; continue; }
            if (rdr["ModelType"].ToString().Equals("项目模型")) { continue; }
			
            model.ModelType = modBll.GetModelInt(rdr["ModelType"].ToString());
            model.ModelID = Insert(model);
            if (!DBCenter.DB.Table_Exist(model.TableName)) { DBCenter.DB.Table_Add(model.TableName, new M_SQL_Field() {ispk=true,fieldName="ID",fieldType="int" }); }
            //开始写入模板型字段,上方已将字段表清空
            string fieldxml = Server.MapPath(SiteConfig.SiteOption.TemplateDir + "/配置库/模型/" + rdr["ModelType"] + "/" + model.ModelName + ".field");
            DataSet fieldds = new DataSet();
            fieldds.ReadXml(fieldxml);
            //如果模型下未定义字段,则忽略
            if (fieldds.Tables.Count < 1) { continue; }
			DataTable fieldDT=fieldds.Tables[0];
			if (!fieldDT.Columns.Contains("IsChain")) { fieldDT.Columns.Add(new DataColumn("IsChain", typeof(string))); }
            foreach (DataRow fielddr in fieldDT.Rows)
            {
                //扩展表中字段不存在则新建,存在则不处理
                M_ModelField fieldMod = new M_ModelField().GetModelFromReader(fielddr);
                AddModelField(model, fieldMod);
            }
        }
        function.WriteSuccessMsg("模型与模型字段导入成功");
    }
    //----------------------------------------
    public void AddModelField(M_ModelInfo model, M_ModelField fieldMod)
    {
        if (StrHelper.StrNullCheck(fieldMod.FieldType)) { throw new Exception("表名:[" + model.TableName + "],[" + fieldMod.FieldName + "]字段类型不能为空"); }
        if (StrHelper.StrNullCheck(fieldMod.FieldName)) { throw new Exception("表名:[" + model.TableName + "],字段名不能为空"); }
        if (fieldMod.OrderID < 1) { fieldMod.OrderID = (fieldBll.GetMaxOrder(model.ModelID) + 1); }
        // string fName, string fAlias, string fType, string dataType
        fieldMod.ModelID = model.ModelID;
        //fieldMod.FieldName = fName;//字段名
        //fieldMod.FieldAlias = fAlias;//别名
        //fieldMod.Sys_type = false;//标识身份为系统字段,不可删
        //fieldMod.FieldType = fType;// "TextType";
        //fieldMod.IsCopy = 1;
        //fieldMod.IsChain = false;
        try { fieldBll.AddField(model.TableName, fieldMod.FieldName, GetTableFieldType(fieldMod), ""); }
        catch { }
        fieldMod.FieldID = InsertID(fieldMod);//其ID与添加入的ID一致
    }
    private int InsertID(M_ModelField model)
    {
        model.FieldName = model.FieldName.Replace(" ", "");
        if (string.IsNullOrEmpty(model.FieldName)) { throw new Exception("字段名不能为空"); }
        if (model.ModelID < 1) { throw new Exception("模型ID不能为空"); }
        return Sql.insertID(model.TbName, model.GetParameters(), BLLCommon.GetParas(model), BLLCommon.GetFields(model));
    }
    /// <summary>
    /// 传入控件类型,返回数据表字段类型
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    private string GetTableFieldType(M_ModelField fieldMod)
    {
        string fieldType = "";
        switch (fieldMod.FieldType)
        {
            //单行文本
            case "TextType":
                fieldType = "nvarchar";
                break;
            //多行文本(不支持Html)
            case "MultipleTextType":
                fieldType = "ntext";
                break;
            //多行文本(支持Html)
            case "MultipleHtmlType":
                fieldType = "ntext";
                break;
            //单选项
            case "OptionType":
                fieldType = "nvarchar";
                break;
            //多选项
            case "ListBoxType":
                fieldType = "ntext";
                break;
            //数字
            case "NumType":
                //int numstyle = 1;
                //if (Content.IndexOf(".") > -1)
                //{
                //    string[] contentarr = Content.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                //    for (int i = 0; i < contentarr.Length; i++)
                //    {
                //        if (contentarr[i].IndexOf("NumberType=") > -1)
                //        {
                //            string[] iiarr = contentarr[i].Split(new string[] { "NumberType=" }, StringSplitOptions.None);
                //            numstyle = DataConverter.CLng(iiarr[1]);
                //        }
                //    }
                //}
                //else
                //{
                //    if (Content.IndexOf("NumberType=") > -1)
                //    {
                //        string[] iiarr = Content.Split(new string[] { "NumberType=" }, StringSplitOptions.None);
                //        numstyle = DataConverter.CLng(iiarr[1]);
                //    }
                //}
                fieldType = "float";
                break;
            //日期时间
            case "DateType":
                fieldType = "datetime";
                break;
            //图片
            case "PicType":
                fieldType = "nvarchar";
                break;
            //图片入库
            case "SqlType":
                fieldType = "ntext";
                //AddFieldCopy(tablename, "FIT_" + tablename, "TextType", "", "");
                break;
            //文件入库
            case "SqlFile":
                fieldType = "ntext";
                //AddFieldCopy(tablename, "FIT_" + tablename, "TextType", "", "");
                break;
            //多图片
            case "MultiPicType":
                fieldType = "ntext";
                //bool ChkThumb = DataConverter.CBool(strArray[0].Split(new char[] { '=' })[1]);
                //string ThumbField = strArray[1].Split(new char[] { '=' })[1];
                //if (ChkThumb && !string.IsNullOrEmpty(ThumbField))
                //{
                //    AddFieldCopy(tablename, ThumbField, "TextType", "", "");
                //}
                break;
            //文件
            case "SmallFileType":
                fieldType = "nvarchar";
                break;
            //下拉文件
            case "PullFileType":
                fieldType = "nvarchar";
                break;
            //多文件
            case "FileType":
            case "SwfFileUpload":
                fieldType = "ntext";
                //bool ChkFileSize = DataConverter.CBool(strArray[0].Split(new char[] { '=' })[1]);
                //string FileSizeField = strArray[1].Split(new char[] { '=' })[1];
                //if (ChkFileSize && !string.IsNullOrEmpty(FileSizeField))
                //{
                //    AddFieldCopy(tablename, FileSizeField, "TextType", "", "");
                //}
                break;
            //运行平台
            case "OperatingType":
                fieldType = "nvarchar";
                break;
            //超链接
            case "SuperLinkType":
                fieldType = "nvarchar";
                break;
            case "GradeOptionType":
                fieldType = "nvarchar";
                break;
            //颜色字段
            case "ColorType":
                fieldType = "nvarchar";
                break;
            //货币字段
            case "MoneyType":
            case "MoneyType2": //货币字段
                fieldType = "money";
                break;
            case "CameraType"://拍照
            case "Upload":
            case "TableField":
            case "Random":
            case "autothumb":
                fieldType = "nvarchar";
                break;
            case "DoubleDateType"://双时间字段
            case "MobileSMS":     //手机短信
            case "Images"://组图
            case "MapType":
                fieldType = "ntext";
                break;
            default:
                break;
        }
        return fieldType;
    }
    //-----------------------------------
    public string ConverToStr(object obj)
    {
        return DataConvert.CStr(obj);
    }
    public int ConvertToInt(object obj)
    {
        return DataConvert.CLng(obj);
    }
    public double ConverToDouble(object o)
    {
        return DataConvert.CDouble(o);
    }
    public DateTime ConvertToDate(object o) { return DataConvert.CDate(o); }
    public bool ConverToBool(object o) { return DataConvert.CBool(DataConvert.CStr(o)); }

    //------------------------------------------数据库相关
    public static int Insert(M_Base data)
    {
        SqlModel model = new SqlModel() { cmd = ZoomLa.SQLDAL.SQL.SqlModel.SqlCmd.Insert, tbName = data.TbName, pk = data.PK };
        model.AddSpToList(data.GetParameters());
        model.fields = GetFields(data);
        model.values = GetParams(data);
        model.pk = "";
        PreSPList(model);
        return DB_InsertID(model);
    }
    private static void PreSPList(SqlModel model)
    {
        if (model.spList == null || model.spList.Count < 1) { return; }
        //剔除主键
        if (!string.IsNullOrEmpty(model.pk))
        {
            for (int i = 0; i < model.spList.Count; i++)
            {
                if (model.spList[i].ParameterName.Equals("@" + model.pk, StringComparison.CurrentCultureIgnoreCase))
                { model.spList.Remove(model.spList[i]); }
            }
        }
    }
    public static int DB_InsertID(SqlModel model)
    {
        string sql = DBCenter.DB.GetSQL(model);
        sql = "SET IDENTITY_INSERT [" + model.tbName + "] ON; " + sql + "SET IDENTITY_INSERT [" + model.tbName + "] OFF; ";
        return DataConvert.CLng(SqlHelper.ExecuteScalar(CommandType.Text,sql,model.spList.ToArray()));
    }
    public static string GetFields(M_Base model)
    {
        string str = string.Empty, PK = "";
        string[,] strArr = model.FieldList();
        for (int i = 0; i < strArr.GetLength(0); i++)
        {
            if (strArr[i, 0].ToLower() != PK)
            {
                str += "[" + strArr[i, 0] + "],";
            }
        }
        return str.Substring(0, str.Length - 1);
    }
    public static string GetParams(M_Base model)
    {
        string str = string.Empty, PK = "";
        string[,] strArr = model.FieldList();
        for (int i = 0; i < strArr.GetLength(0); i++)
        {
            if (strArr[i, 0].ToLower() != PK)
            {
                str += "@" + strArr[i, 0] + ",";
            }
        }
        return str.Substring(0, str.Length - 1);
    }
    public static string GetFieldAndParam(M_Base model)
    {
        string str = string.Empty, PK = "";
        string[,] strArr = model.FieldList();
        for (int i = 0; i < strArr.GetLength(0); i++)
        {
            if (strArr[i, 0].ToLower() != PK)
            {
                str += "[" + strArr[i, 0] + "]=@" + strArr[i, 0] + ",";
            }
        }
        return str.Substring(0, str.Length - 1);
    }
}