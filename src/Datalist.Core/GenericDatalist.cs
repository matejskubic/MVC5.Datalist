using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web.Mvc;

namespace Datalist
{
    public abstract class GenericDatalist<T> : AbstractDatalist where T : class
    {
        public virtual IEnumerable<PropertyInfo> AttributedProperties
        {
            get
            {
                return typeof(T)
                    .GetProperties()
                    .Where(property => property.IsDefined(typeof(DatalistColumnAttribute), false))
                    .OrderBy(property => property.GetCustomAttribute<DatalistColumnAttribute>(false).Position);
            }
        }

        protected GenericDatalist()
        {
            foreach (PropertyInfo property in AttributedProperties)
                Columns.Add(GetColumnKey(property), GetColumnHeader(property), GetColumnCssClass(property));
        }
        protected GenericDatalist(UrlHelper url) : base(url)
        {
            foreach (PropertyInfo property in AttributedProperties)
                Columns.Add(GetColumnKey(property), GetColumnHeader(property), GetColumnCssClass(property));
        }
        public virtual String GetColumnKey(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            return property.Name;
        }
        public virtual String GetColumnHeader(PropertyInfo property)
        {
            return property?.GetCustomAttribute<DisplayAttribute>(false)?.GetShortName();
        }
        public virtual String GetColumnCssClass(PropertyInfo property)
        {
            return null;
        }

        public override DatalistData GetData()
        {
            IQueryable<T> models = GetModels();
            models = Filter(models);
            models = Sort(models);

            return FormDatalistData(models);
        }
        public abstract IQueryable<T> GetModels();

        private IQueryable<T> Filter(IQueryable<T> models)
        {
            if (CurrentFilter.Id != null)
                return FilterById(models);

            if (CurrentFilter.AdditionalFilters.Count > 0)
                models = FilterByAdditionalFilters(models);

            return FilterBySearchTerm(models);
        }
        public virtual IQueryable<T> FilterById(IQueryable<T> models)
        {
            PropertyInfo idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
                throw new DatalistException($"'{typeof(T).Name}' type does not have property named 'Id', required for automatic id filtering.");

            if (idProperty.PropertyType == typeof(String))
                return models.Where("Id = @0", CurrentFilter.Id);

            Decimal id;
            if (IsNumeric(idProperty.PropertyType) && Decimal.TryParse(CurrentFilter.Id, out id))
                return models.Where("Id = @0", id);

            throw new DatalistException($"'{typeof(T).Name}.Id' property type has to be a string or a number.");
        }
        public virtual IQueryable<T> FilterByAdditionalFilters(IQueryable<T> models)
        {
            foreach (KeyValuePair<String, Object> filter in CurrentFilter.AdditionalFilters.Where(item => item.Value != null))
                models = models.Where($@"({filter.Key} != null && {filter.Key} == @0)", filter.Value);

            return models;
        }
        public virtual IQueryable<T> FilterBySearchTerm(IQueryable<T> models)
        {
            if (String.IsNullOrEmpty(CurrentFilter.SearchTerm))
                return models;

            List<String> queries = new List<String>();

            foreach (String property in Columns.Keys)
                if (typeof(T).GetProperty(property)?.PropertyType == typeof(String))
                    queries.Add($"({property} != null && {property}.ToLower().Contains(@0))");

            if (queries.Count == 0) return models;

            return models.Where(String.Join(" || ", queries), CurrentFilter.SearchTerm.ToLower());
        }

        public virtual IQueryable<T> Sort(IQueryable<T> models)
        {
            String column = CurrentFilter.SortColumn ?? DefaultSortColumn ?? Columns.Keys.FirstOrDefault();
            if (String.IsNullOrWhiteSpace(column))
                return models;

            return models.OrderBy(column + " " + CurrentFilter.SortOrder);
        }

        public virtual DatalistData FormDatalistData(IQueryable<T> models)
        {
            DatalistData data = new DatalistData();
            data.FilteredRecords = models.Count();
            data.Columns = Columns;

            IQueryable<T> pagedModels = models
                .Skip(CurrentFilter.Page * CurrentFilter.RecordsPerPage)
                .Take(Math.Min(CurrentFilter.RecordsPerPage, 99));

            foreach (T model in pagedModels)
            {
                Dictionary<String, String> row = new Dictionary<String, String>();
                AddId(row, model);
                AddAutocomplete(row, model);
                AddColumns(row, model);
                AddAdditionalData(row, model);

                data.Rows.Add(row);
            }

            return data;
        }
        public virtual void AddId(Dictionary<String, String> row, T model)
        {
            row.Add(IdKey, GetValue(model, "Id"));
        }
        public virtual void AddAutocomplete(Dictionary<String, String> row, T model)
        {
            row.Add(AcKey, GetValue(model, Columns.Keys.FirstOrDefault() ?? ""));
        }
        public virtual void AddColumns(Dictionary<String, String> row, T model)
        {
            foreach (String column in Columns.Keys)
                row[column] = GetValue(model, column);
        }
        public virtual void AddAdditionalData(Dictionary<String, String> row, T model)
        {
        }

        private String GetValue(T model, String propertyName)
        {
            PropertyInfo property = typeof(T).GetProperty(propertyName);
            if (property == null) return null;

            DatalistColumnAttribute column = property.GetCustomAttribute<DatalistColumnAttribute>(false);
            if (column?.Format != null)
                return String.Format(column.Format, property.GetValue(model));

            return property.GetValue(model)?.ToString();
        }
        private Boolean IsNumeric(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }
    }
}
