using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;

namespace Datalist
{
    public abstract class GenericDatalist<T> : AbstractDatalist where T : class
    {
        protected IEnumerable<PropertyInfo> AttributedProperties
        {
            get
            {
                return typeof(T)
                    .GetProperties()
                    .Where(property => property.GetCustomAttribute<DatalistColumnAttribute>(false) != null)
                    .OrderBy(property => property.GetCustomAttribute<DatalistColumnAttribute>(false).Position);
            }
        }

        public GenericDatalist()
        {
            foreach (PropertyInfo property in AttributedProperties)
                Columns.Add(GetColumnKey(property), GetColumnHeader(property));
        }
        protected virtual String GetColumnKey(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException("property");

            var column = property.GetCustomAttribute<DatalistColumnAttribute>(false);
            if (column != null && column.Relation != null)
                return property.Name + "." + GetColumnKey(GetRelationProperty(property, column.Relation));

            return property.Name;
        }
        protected virtual String GetColumnHeader(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException("property");

            var column = property.GetCustomAttribute<DatalistColumnAttribute>(false);
            if (column != null && !String.IsNullOrWhiteSpace(column.Relation))
                return GetColumnHeader(GetRelationProperty(property, column.Relation));

            var header = property.GetCustomAttribute<DisplayAttribute>(false);
            if (header != null) return header.GetName();
            return property.Name;
        }
        private PropertyInfo GetRelationProperty(PropertyInfo property, String relation)
        {
            var relationProperty = property.PropertyType.GetProperty(relation);
            if (relationProperty != null)
                return relationProperty;

            throw new DatalistException(String.Format("{0}.{1} does not have property named \"{2}\".",
                property.DeclaringType.Name,
                property.Name,
                relation));
        }

        public override DatalistData GetData()
        {
            var models = GetModels();
            models = Filter(models);
            models = Sort(models);

            return FormDatalistData(models);
        }
        protected abstract IQueryable<T> GetModels();

        private IQueryable<T> Filter(IQueryable<T> models)
        {
            if (!String.IsNullOrWhiteSpace(CurrentFilter.Id))
                return FilterById(models);

            if (CurrentFilter.AdditionalFilters.Count > 0)
                models = FilterByAdditionalFilters(models);

            return FilterBySearchTerm(models);
        }
        protected virtual IQueryable<T> FilterById(IQueryable<T> models)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
                throw new DatalistException(String.Format("Type {0} does not have property named Id.", typeof(T).Name));

            if (idProperty.PropertyType == typeof(String))
                return models.Where("Id = \"" + CurrentFilter.Id + "\"");

            Decimal temp;
            if (IsNumeric(idProperty.PropertyType) && Decimal.TryParse(CurrentFilter.Id, out temp))
                return models.Where("Id = " + CurrentFilter.Id);

            throw new DatalistException(String.Format("{0}.Id can not be filtered by \"{1}\", because of unconvertable types.", typeof(T).Name, CurrentFilter.Id));
        }
        protected virtual IQueryable<T> FilterByAdditionalFilters(IQueryable<T> models)
        {
            var queries = new List<String>();
            foreach (var filter in CurrentFilter.AdditionalFilters.Where(item => item.Value != null))
                queries.Add(FormFilterQuery(GetType(filter.Key), filter.Key, FilterType.Equals, filter.Value));

            queries = queries.Where(query => !String.IsNullOrWhiteSpace(query)).ToList();
            if (queries.Count == 0) return models;

            return models.Where(String.Join(" && ", queries));
        }
        protected virtual IQueryable<T> FilterBySearchTerm(IQueryable<T> models)
        {
            if (String.IsNullOrWhiteSpace(CurrentFilter.SearchTerm)) return models; // TODO: Remvoe all IsNullOrWhiteSpace

            var queries = new List<String>(); // TODO: Fix null values in javascript html code
            var term = CurrentFilter.SearchTerm.ToLower().Trim(); // TODO: Fix resizing on different datalists.
            foreach (var fullPropertyName in Columns.Keys)
                if (GetType(fullPropertyName) == typeof(String))
                    queries.Add(FormFilterQuery(null, fullPropertyName, FilterType.Contains, term)); // TODO: Remove null type if possible

            if (queries.Count == 0) return models; 
            return models.Where(String.Join(" || ", queries));
        }
        protected virtual IQueryable<T> Sort(IQueryable<T> models)
        {
            if (CurrentFilter.SortColumn != null && Columns.ContainsKey(CurrentFilter.SortColumn))
                return models.OrderBy(String.Format("{0} {1}", CurrentFilter.SortColumn, CurrentFilter.SortOrder));

            if (DefaultSortColumn != null && Columns.ContainsKey(DefaultSortColumn))
                return models.OrderBy(String.Format("{0} {1}", DefaultSortColumn, CurrentFilter.SortOrder));

            if (Columns.Count > 0)
                return models.OrderBy(String.Format("{0} {1}", Columns.First().Key, CurrentFilter.SortOrder));
            // TODO: Add errors on no property found.
            throw new DatalistException("Datalist columns can not be empty.");
        }

        protected virtual DatalistData FormDatalistData(IQueryable<T> models)
        {
            var data = new DatalistData();
            data.FilteredRecords = models.Count();
            data.Columns = Columns;

            var pagedModels = models
                .Skip(CurrentFilter.Page * CurrentFilter.RecordsPerPage)
                .Take(CurrentFilter.RecordsPerPage);

            foreach (T model in pagedModels)
            {
                var row = new Dictionary<String, String>();
                AddId(row, model);
                AddAutocomplete(row, model);
                AddColumns(row, model);
                AddAdditionalData(row, model);

                data.Rows.Add(row);
            }

            return data;
        }
        protected virtual void AddId(Dictionary<String, String> row, T model)
        {
            row.Add(IdKey, GetValue(model, "Id")); // TODO: Change to get id and get autocomplete values
        }
        protected virtual void AddAutocomplete(Dictionary<String, String> row, T model)
        {
            if (Columns.Count == 0)
                throw new DatalistException("Datalist columns can not be empty.");

            row.Add(AcKey, GetValue(model, Columns.Keys.First()));
        }
        protected virtual void AddColumns(Dictionary<String, String> row, T model)
        {
            foreach (String column in Columns.Keys)
                AddColumn(row, column, model);
        }
        protected virtual void AddAdditionalData(Dictionary<String, String> row, T model)
        {
        }

        private String FormFilterQuery(Type type, String fullPropertyName, FilterType filterType, Object term)
        {
            // TODO: It should not check for != null, on properties without relation
            // TODO: Check if != null coverts to proper sql in MsSql
            // TODO: Remove String.Empty queries
            if (filterType == FilterType.Contains)
                return String.Format(@"({0} && {1}.ToLower().Contains(""{2}""))", FormNotNullQuery(fullPropertyName), fullPropertyName, term);

            if (type == typeof(String))
                return String.Format(@"({0} && {1} == ""{2}"")", FormNotNullQuery(fullPropertyName), fullPropertyName, term);

            Decimal number;
            if (IsNumeric(type) && Decimal.TryParse(term.ToString(), out number))
                return String.Format("({0} && {1} == {2})", FormNotNullQuery(fullPropertyName), fullPropertyName, number.ToString().Replace(',', '.'));

            return String.Empty;
        }
        private String FormNotNullQuery(String fullPropertyName)
        {
            var queries = new List<String>();
            var properties = fullPropertyName.Split('.');
            for (Int32 i = 0; i < properties.Length; ++i)
                queries.Add(String.Join(".", properties.Take(i + 1)) + " != null");

            return String.Join(" && ", queries);
        }

        private void AddColumn(Dictionary<String, String> row, String column, T model)
        {
            row.Add(column, GetValue(model, column));
        }
        private String GetValue(Object model, String fullPropertyName)
        {
            if (model == null) return String.Empty; 

            Object value = null;
            Type type = model.GetType();
            String[] properties = fullPropertyName.Split('.');
            PropertyInfo property = type.GetProperty(properties[0]);
            if (property == null)
                throw new DatalistException(String.Format("Type {0} does not have property named {1}.", type.Name, properties[0]));

            if (properties.Length == 1)
                value = property.GetValue(model);
            else
                value = GetValue(property.GetValue(model), String.Join(".", properties.Skip(1)));

            return value != null ? value.ToString() : String.Empty;
        }
        private Type GetType(String fullPropertyName)
        {
            Type type = typeof(T);
            var properties = fullPropertyName.Split('.');
            foreach (String propertyName in properties)
            {
                var property = type.GetProperty(propertyName);
                if (property == null)
                    throw new DatalistException(String.Format("Type {0} does not have property named {1}.",
                        type.Name, propertyName));

                type = property.PropertyType;
            }

            return type;
        }
        private Boolean IsNumeric(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type.IsEnum) return false;

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

        private enum FilterType
        {
            Contains,
            Equals
        }
    }
}
