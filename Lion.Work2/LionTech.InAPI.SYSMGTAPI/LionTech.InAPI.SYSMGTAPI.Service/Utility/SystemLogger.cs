using LionTech.AspNetCore.InApi;
using LionTech.AspNetCore.Utility.Extensions;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Raw.Interfaces;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Utility.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Utility
{
    public class SystemLogger : ISystemLogger
    {
        private readonly ILogger<SystemLogger> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAuthState _authState;
        private readonly ISystmeLogQueues _logQueues;
        private readonly ISysSettingRepository _sysSettingRepository;
        private readonly IRawRepository _rawRepository;

        public SystemLogger(
            ILogger<SystemLogger> logger,
            IHttpContextAccessor httpContext,
            IAuthState authState,
            ISystmeLogQueues logQueues,
            ISysSettingRepository sysSettingRepository,
            IRawRepository rawRepository)
        {
            _logger = logger;
            _httpContext = httpContext;
            _authState = authState;
            _logQueues = logQueues;
            _sysSettingRepository = sysSettingRepository;
            _rawRepository = rawRepository;
        }

        public async Task RecordLogAsync<TModel, TValue>(EnumMongoDocName mongoDocName, IEnumerable<TModel> model, Expression<Func<TModel, TValue>> condition, EnumSystemLogModify modify)
        {
            if (model != null)
            {
                try
                {
                    foreach (var groupByList in model.GroupBy(condition.Compile()).Select(group => group.ToList()).ToList())
                    {
                        await RecordLog(mongoDocName, groupByList, condition, modify);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"\n{ex.GetType()}: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }

        public Task RecordLogAsync<TModel, TValue>(EnumMongoDocName mongoDocName, TModel model, Expression<Func<TModel, TValue>> condition, EnumSystemLogModify modify)
        {
            if (model != null && model is not IEnumerable)
            {
                try
                {
                    return RecordLog(mongoDocName, new[] { model }, condition, modify);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"\n{ex.GetType()}: {ex.Message}\n{ex.StackTrace}");
                }
            }

            return Task.CompletedTask;
        }

        private static bool IsPrimitiveType(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
        }

        private static bool IsPrimitiveType(IList list)
        {
            Type listType = list.GetType();
            if (listType.IsGenericType)
            {
                Type[] typeArguments = listType.GetGenericArguments();
                if (typeArguments.Length > 0)
                {
                    return IsPrimitiveType(typeArguments[0]);
                }
            }
            return IsPrimitiveType(typeof(object));
        }

        private BsonDocument GetBsonDocument(object model)
        {
            var modelType = model.GetType();
            var isAnonymousType = IsAnonymousType(modelType);
            IEnumerable<PropertyInfo> propertys = modelType.GetProperties();
            if (isAnonymousType == false)
            {
                propertys = propertys
                .Where(w => w.GetCustomAttributes(typeof(BsonElementAttribute), inherit: false).Any());
            }

            var bsonDocument = new BsonDocument();
            foreach (var property in propertys)
            {
                var propValue = property.GetValue(model);
                var propName = property.Name;

                if (isAnonymousType == false)
                {
                    var bsonElementAttribute = property.GetCustomAttribute<BsonElementAttribute>();
                    propName = bsonElementAttribute.ElementName;
                }

                if (IsPrimitiveType(property.PropertyType))
                {
                    bsonDocument.Add(propName, BsonValue.Create(propValue));
                }
                else if (propValue is IList list)
                {
                    if (IsPrimitiveType(list))
                    {
                        bsonDocument.Add(propName, new BsonArray(list));
                    }
                    else
                    {
                        var bsonValues = new List<BsonValue>();
                        foreach (var element in list)
                        {
                            bsonValues.Add(GetBsonDocument(element));
                        }
                        bsonDocument.Add(propName, new BsonArray(bsonValues));
                    }
                }
                else if (propValue != null)
                {
                    bsonDocument.Add(propName, GetBsonDocument(propValue));
                }
                else
                {
                    bsonDocument.Add(propName, BsonValue.Create(null));
                }
            }

            return bsonDocument;
        }

        private static bool IsAnonymousType(Type type)
        {
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                    && type.IsGenericType
                    && type.Name.Contains("AnonymousType")
                    && (type.Name.StartsWith("<>", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.StartsWith("VB$", StringComparison.OrdinalIgnoreCase));
        }

        private async Task RecordLog<TModel, TValue>(EnumMongoDocName mongoDocName, IEnumerable<TModel> modelList, Expression<Func<TModel, TValue>> condition, EnumSystemLogModify modify)
        {
            if (_httpContext.HttpContext != null && modelList != null && modelList.Any())
            {
                var isAnonymousType = IsAnonymousType(typeof(TModel));
                var bsonDocuments = new List<BsonDocument>();
                TModel model = modelList.FirstOrDefault();

                var clientIpAddress = _httpContext.HttpContext.GetClientIPAddress();
                var clientSysId = _httpContext.HttpContext.Request.Query["ClientSysID"];
                var clientUserId = _httpContext.HttpContext.Request.Query["ClientUserID"];

                var systemMainTask = _sysSettingRepository.GetSystemMain(clientSysId);
                var rawUserTask = _rawRepository.GetRawUsers(clientUserId, 1);
                await Task.WhenAll(systemMainTask, rawUserTask);

                Func<TModel, TValue> conditionFun = condition.Compile();
                TValue conditionObj = conditionFun(model);

                IEnumerable<PropertyInfo> propertys = typeof(TModel).GetProperties();
                if (isAnonymousType == false)
                {
                    propertys = propertys
                    .Where(w => w.GetCustomAttributes(typeof(BsonElementAttribute), inherit: false).Any());
                }

                BsonDocument conditionBsonDocument = null;
                var conditionPropertys = conditionObj.GetType().GetProperties();
                if (conditionPropertys.Any())
                {
                    conditionBsonDocument = new BsonDocument();
                    foreach (var property in propertys)
                    {
                        var propName = property.Name;
                    
                        if (isAnonymousType == false)
                        {
                            var bsonElementAttribute = property.GetCustomAttribute<BsonElementAttribute>();
                            propName = bsonElementAttribute.ElementName;
                        }

                        if (conditionPropertys.Any(a => a.Name == property.Name))
                        {
                            conditionBsonDocument.Add(propName, BsonValue.Create(property.GetValue(model)));
                        }
                    }
                }
                
                foreach (dynamic listElement in modelList)
                {
                    BsonDocument bsonDocument;
                    if (conditionBsonDocument == null)
                    {
                        bsonDocument = new BsonDocument
                        {
                            { "API_NO", BsonValue.Create(_authState.APINo) }
                        };
                    }
                    else
                    {
                        bsonDocument = new BsonDocument
                        {
                            { "LOG_NO", BsonValue.Create(string.Empty) },
                            { "API_NO", BsonValue.Create(_authState.APINo) }
                        };
                    }

                    bsonDocument.AddRange(GetBsonDocument(listElement));
                    bsonDocuments.Add(bsonDocument);
                }

                var log = new SystemLog
                {
                    MongoDocName = mongoDocName,
                    Data = bsonDocuments,
                    Condition = conditionBsonDocument,
                    Modify = modify,
                    UpdDT = DateTime.Now,
                    UpdUserID = clientUserId,
                    UpdUserNM = rawUserTask.Result?.FirstOrDefault()?.UserNM,
                    ExecSysID = clientSysId,
                    ExecSysNM = systemMainTask.Result?.SysNMZHTW,
                    ExecIPAddress = clientIpAddress
                };
                _logQueues.Producer(log);
            }
        }
    }
}