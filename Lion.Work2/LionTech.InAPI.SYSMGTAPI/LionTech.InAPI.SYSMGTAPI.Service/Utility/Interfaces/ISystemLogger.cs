using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Utility.Interfaces
{
    public interface ISystemLogger
    {
        public Task RecordLogAsync<TModel1, TValue>(EnumMongoDocName mongoDocName, IEnumerable<TModel1> model, Expression<Func<TModel1, TValue>> condition, EnumSystemLogModify modify);
        //public Task RecordLogAsync<TModel1, TValue>(EnumMongoDocName mongoDocName, List<TModel1> model, Expression<Func<TModel1, TValue>> condition, EnumSystemLogModify modify);
        public Task RecordLogAsync<TModel1, TValue>(EnumMongoDocName mongoDocName, TModel1 model, Expression<Func<TModel1, TValue>> condition, EnumSystemLogModify modify);
    }
}