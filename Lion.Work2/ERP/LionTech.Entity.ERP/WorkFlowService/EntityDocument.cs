// 新增日期：2016-07-11
// 新增人員：王汶智
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.WorkFlowService
{
    public class EntityDocument : EntityWorkFlowService
    {
        public EntityDocument(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class AddDocumentPara
        {
            public enum ParaField
            {
                WF_NO, NODE_NO,
                WF_DOC_SEQ, DOC_USER_ID, DOC_FILE_NAME,
                DOC_ENCODE_NAME, DOC_PATH, DOC_LOCAL_PATH, IS_DELETE,
                UPD_USER_ID, REMARK
            }

            public DBChar WFNo;
            public DBChar NodeNo;
            public DBChar WFDocSeq;
            public DBVarChar DocUserID;
            public DBNVarChar DocFileNM;
            public DBVarChar DocEncodeNM;
            public DBNVarChar DocPath;
            public DBNVarChar DocLocalPath;
            public DBChar IsDelete;
            public DBVarChar UpdUserID;
            public DBNVarChar Remark;
        }

        public class AddDocumentExecuteResult : ExecuteResult
        {
            public DBVarChar SysID;
            public DBChar DocNo;
            public DBChar DocDate;
        }

        public enum EnumAddDocumentResult
        {
            Success, Failure
        }

        public AddDocumentExecuteResult AddDocument(AddDocumentPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "EXECUTE dbo.SP_WF_ADD_DOCUMENT {WF_NO}, {NODE_NO}, {WF_DOC_SEQ}" +
                        ", {DOC_USER_ID}, {DOC_FILE_NAME}, {DOC_ENCODE_NAME}, {DOC_PATH}, {DOC_LOCAL_PATH}" +
                        ", {UPD_USER_ID}, {REMARK};"
                    });

            dbParameters.Add(new DBParameter { Name = AddDocumentPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = AddDocumentPara.ParaField.NODE_NO, Value = para.NodeNo });
            dbParameters.Add(new DBParameter { Name = AddDocumentPara.ParaField.WF_DOC_SEQ, Value = para.WFDocSeq });
            dbParameters.Add(new DBParameter { Name = AddDocumentPara.ParaField.DOC_USER_ID, Value = para.DocUserID });
            dbParameters.Add(new DBParameter { Name = AddDocumentPara.ParaField.DOC_FILE_NAME, Value = para.DocFileNM });
            dbParameters.Add(new DBParameter { Name = AddDocumentPara.ParaField.DOC_ENCODE_NAME, Value = para.DocEncodeNM });
            dbParameters.Add(new DBParameter { Name = AddDocumentPara.ParaField.DOC_PATH, Value = para.DocPath });
            dbParameters.Add(new DBParameter { Name = AddDocumentPara.ParaField.DOC_LOCAL_PATH, Value = para.DocLocalPath });
            dbParameters.Add(new DBParameter { Name = AddDocumentPara.ParaField.IS_DELETE, Value = para.IsDelete });
            dbParameters.Add(new DBParameter { Name = AddDocumentPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = AddDocumentPara.ParaField.REMARK, Value = para.Remark });

            var result = GetEntityList<AddDocumentExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumAddDocumentResult.Failure.ToString())
            {
                throw new EntityExecuteResultException(result);
            }

            return result;
        }

        public class DeleteDocumentPara
        {
            public enum ParaField
            {
                WF_NO, DOC_NO, UPD_USER_ID
            }

            public DBChar WFNo;
            public DBChar DocNo;
            public DBVarChar UpdUserID;
        }

        public class DeleteDocumentExecuteResult : ExecuteResult
        {
            public DBVarChar SysID;
        }

        public enum EnumDeleteDocumentResult
        {
            Success, Failure
        }

        public DeleteDocumentExecuteResult DeleteDocument(DeleteDocumentPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "EXECUTE dbo.SP_WF_DELETE_DOCUMENT {WF_NO}, {DOC_NO}, {UPD_USER_ID};"
                    });

            dbParameters.Add(new DBParameter { Name = DeleteDocumentPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = DeleteDocumentPara.ParaField.DOC_NO, Value = para.DocNo });
            dbParameters.Add(new DBParameter { Name = DeleteDocumentPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<DeleteDocumentExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumDeleteDocumentResult.Failure.ToString())
            {
                throw new EntityExecuteResultException(result);
            }

            return result;
        }
    }
}