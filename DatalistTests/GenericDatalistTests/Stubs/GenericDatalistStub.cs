using Datalist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DatalistTests.GenericDatalistTests.Stubs
{
    public class GenericDatalistStub<T> : GenericDatalist<T> where T : class
    {
        public List<T> Models
        {
            get;
            set;
        }
        public String BaseDefaultSortColumn
        {
            get
            {
                return DefaultSortColumn;
            }
            set
            {
                DefaultSortColumn = value;
            }
        }
        public IEnumerable<PropertyInfo> BaseAttributedProperties
        {
            get
            {
                return AttributedProperties;
            }
        }

        public GenericDatalistStub()
        {
            Models = new List<T>();
        }

        protected override IQueryable<T> GetModels()
        {
            return Models.AsQueryable();
        }

        public String BaseGetColumnKey(PropertyInfo property)
        {
            return GetColumnKey(property);
        }
        public String BaseGetColumnHeader(PropertyInfo property)
        {
            return GetColumnHeader(property);
        }

        public IQueryable<T> BaseFilterById(List<T> models)
        {
            return FilterById(models.AsQueryable());
        }
        public IQueryable<T> BaseFilterByAdditionalFilters(List<T> models)
        {
            return FilterByAdditionalFilters(models.AsQueryable());
        }
        public IQueryable<T> BaseFilterBySearchTerm(List<T> models)
        {
            return FilterBySearchTerm(models.AsQueryable());
        }
        public IQueryable<T> BaseSort(List<T> models)
        {
            return Sort(models.AsQueryable());
        }

        public DatalistData BaseFormDatalistData(IQueryable<T> models)
        {
            return FormDatalistData(models);
        }
        public void BaseAddId(Dictionary<String, String> row, T model)
        {
            AddId(row, model);
        }
        public void BaseAddAutocomplete(Dictionary<String, String> row, T model)
        {
            AddAutocomplete(row, model);
        }
        public void BaseAddColumns(Dictionary<String, String> row, T model)
        {
            AddColumns(row, model);
        }
        public void BaseAddAdditionalData(Dictionary<String, String> row, T model)
        {
            AddAdditionalData(row, model);
        }
    }
}
