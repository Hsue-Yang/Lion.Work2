using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using LionTech.EDI;
using LionTech.Entity;
using LionTech.Entity.ERP.EDIService;
using LionTech.Log;
using LionTech.Utility;
using MongoDB.Bson;
using MongoDB.Driver;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace LionTech.EDIService.ERPExternal
{
    public static class USER_SEEN_ORDER_LIST
    {
        private class NpoiMemoryStream : MemoryStream
        {
            public NpoiMemoryStream()
            {
                AllowClose = true;
            }

            public bool AllowClose { get; set; }

            public override void Close()
            {
                if (AllowClose)
                {
                    base.Close();
                }
            }
        }

        private enum EnumJobParameter
        {
            MailSubject
        }

        private enum EnumConnectionString
        {
            LionGroupMSERP
        }
        
        public static EnumJobResult EXE_SEN_REPORT(Flow flow, Job job)
        {
            EnumJobResult jobResult = EnumJobResult.Failure;
            Connection connection = flow.connections[job.connectionID];
            Connection mongoConnection = flow.connections[EnumConnectionString.LionGroupMSERP.ToString()];
            string exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;
            
            try
            {
                EntityERPExternal entity = new EntityERPExternal(connection.value, connection.providerName);
                string connectionString = $@"mongodb:{Security.Decrypt(mongoConnection.value)}";
                string database = "LionGroupSERP";
                string tableName = "LOG_ERP_USER_TRACE_ACTION";
                MongoClient mongoClinet = new MongoClient(connectionString);
                IMongoDatabase db = mongoClinet.GetDatabase(database);

                var builder = Builders<BsonDocument>.Filter;
                List<FilterDefinition<BsonDocument>> queryList = new List<FilterDefinition<BsonDocument>>();
                queryList.Add(builder.Ne("ACTION_PARAMETERS.OrderYear", string.Empty));
                queryList.Add(builder.Ne("ACTION_PARAMETERS.OrderYear", "undefined"));
                queryList.Add(builder.Gte("UPD_DT", Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd"))));
                queryList.Add(builder.Lte("UPD_DT", Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd 23:59:59.999"))));
                var col = db.GetCollection<BsonDocument>(tableName);
                var aggregate = col
                    .Aggregate()
                    .Match(builder.And(queryList))
                    .Group(new BsonDocument
                    {
                        {
                            "_id", new BsonDocument
                            {
                                { "OrderYear", "$ACTION_PARAMETERS.OrderYear" },
                                { "OrderSN", "$ACTION_PARAMETERS.OrderSN" },
                                { "USER_ID", "$USER_ID" },
                                {
                                    "UPD_DT", new BsonDocument
                                    {
                                        {
                                            "$dateToString", new BsonDocument
                                            {
                                                { "format", "%Y/%m/%d" },
                                                {
                                                    "date", new BsonDocument
                                                    {
                                                        {
                                                            "$add", new BsonArray(new List<BsonValue>
                                                            {
                                                                BsonValue.Create("$UPD_DT"),
                                                                BsonValue.Create(8 * 60 * 60000)
                                                            })
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        { "count", new BsonDocument("$sum", 1) }
                    })
                    .Group(new BsonDocument
                    {
                        {
                            "_id", new BsonDocument
                            {
                                { "OrderYear", "$_id.OrderYear" },
                                { "OrderSN", "$_id.OrderSN" },
                                { "USER_ID", "$_id.USER_ID" },
                                { "UPD_DT", "$_id.UPD_DT" }
                            }
                        },
                        { "count", new BsonDocument("$sum", 1) }
                    })
                    .Project(new BsonDocument
                    {
                        { "_id", 0 },
                        { "OrderYear", "$_id.OrderYear" },
                        { "OrderSN", "$_id.OrderSN" },
                        { "USER_ID", "$_id.USER_ID" },
                        { "UPD_DT", "$_id.UPD_DT" },
                        { "count", 1 }
                    });
                var result = aggregate.ToListAsync().Result;
                var userInfoList =
                    entity.SelectUserErpInfoList(
                        new EntityERPExternal.UserErpInfoPara
                        {
                            UserIDList = result.Select(s => (string)s["USER_ID"]).Distinct().Select(s=>new DBNVarChar(s)).ToList()
                        });

                var workbook = new XSSFWorkbook();
                
                int cellIndex = 0;
                int rowIndex = 0;
                ISheet sheet = workbook.CreateSheet("明細");
                IRow dataRow = sheet.CreateRow(rowIndex);
                rowIndex++;
                dataRow.CreateCell(cellIndex).SetCellValue("日期");
                cellIndex++;
                dataRow.CreateCell(cellIndex).SetCellValue("員編");
                cellIndex++;
                dataRow.CreateCell(cellIndex).SetCellValue("姓名");
                cellIndex++;
                dataRow.CreateCell(cellIndex).SetCellValue("單位代碼");
                cellIndex++;
                dataRow.CreateCell(cellIndex).SetCellValue("單位名稱");
                cellIndex++;
                dataRow.CreateCell(cellIndex).SetCellValue("職稱");
                cellIndex++;
                dataRow.CreateCell(cellIndex).SetCellValue("筆數");

                foreach (var row in (from s in result
                                     group s by new
                                     {
                                         userID = (string)s["USER_ID"],
                                         date = (string)s["UPD_DT"]
                                         //date = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd")
                                     }
                                     into g
                                     select new
                                     {
                                         g.Key.userID,
                                         g.Key.date,
                                         count = g.Count()
                                     }).ToList())
                {
                    var userInfo = userInfoList.Find(f => f.UserID.GetValue() == row.userID);
                    dataRow = sheet.CreateRow(rowIndex);
                    cellIndex = 0;
                    dataRow.CreateCell(cellIndex).SetCellValue(row.date);
                    cellIndex++;
                    dataRow.CreateCell(cellIndex).SetCellValue(userInfo?.UserID.GetValue());
                    cellIndex++;
                    dataRow.CreateCell(cellIndex).SetCellValue(userInfo?.UserNM.GetValue());
                    cellIndex++;
                    dataRow.CreateCell(cellIndex).SetCellValue(userInfo?.UserUnitID.GetValue());
                    cellIndex++;
                    dataRow.CreateCell(cellIndex).SetCellValue(userInfo?.UserUnitNM.GetValue());
                    cellIndex++;
                    dataRow.CreateCell(cellIndex).SetCellValue(userInfo?.UserJob1NM.GetValue());
                    cellIndex++;
                    dataRow.CreateCell(cellIndex).SetCellValue(row.count);
                    rowIndex++;
                }

                string fileName = flow.paths[EnumPathID.SRC.ToString()].value + "report.xlsx";
                //fileName = @"E:\report1.xlsx";
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    NpoiMemoryStream mss = new NpoiMemoryStream();
                    mss.AllowClose = false;
                    workbook.Write(mss);
                    mss.Flush();
                    mss.Position = 0;
                    mss.AllowClose = true;
                    mss.WriteTo(fileStream);
                }
                
                var client = new SmtpClient { Host = "smtp.liontravel.com" };
                var mailMessages = new MailMessage { From = new MailAddress("syssd@liontravel.com") };
                var subject = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd") + " " + job.parameters[EnumJobParameter.MailSubject.ToString()].value;
                
                Attachment attachment = new Attachment(fileName);
                mailMessages.Attachments.Add(attachment);

                foreach (var row in job.parameters.Where(w => w.id.StartsWith("SendUserMailList")))
                {
                    mailMessages.To.Add(new MailAddress(row.value));
                }

                mailMessages.Priority = System.Net.Mail.MailPriority.High;
                mailMessages.Subject = subject;
                mailMessages.SubjectEncoding = Encoding.GetEncoding("utf-8");
                mailMessages.BodyEncoding = Encoding.GetEncoding("utf-8");
                mailMessages.IsBodyHtml = true;
                client.Send(mailMessages);

                jobResult = EnumJobResult.Success;
            }
            catch (Exception ex)
            {
                FileLog.Write(exceptionPath, ex);
            }

            return jobResult;
        }
    }
}
