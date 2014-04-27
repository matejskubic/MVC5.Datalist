using Datalist;
using DatalistSamples.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatalistSamples.Datalists
{
    public class DatalistDataDatalist : DefaultDatalist
    {
        protected override DatalistData FormDatalistData(IQueryable<UserModel> models)
        {
            DatalistData data = new DatalistData();
            data.FilteredRecords = models.Count();
            data.Columns.Add("FirstName", "First name");
            data.Columns.Add("LastName", "Last name");

            IQueryable<UserModel> pagedModels = models
                .Skip(CurrentFilter.Page * CurrentFilter.RecordsPerPage)
                .Take(CurrentFilter.RecordsPerPage);

            foreach (UserModel model in pagedModels)
            {
                Dictionary<String, String> row = new Dictionary<String, String>();
                row.Add(IdKey, model.Id);
                row.Add(AcKey, String.Format("{0} {1}", model.FirstName, model.LastName));
                row.Add("FirstName", model.FirstName);
                row.Add("LastName", model.LastName);
                row.Add("AdditionalData", "Additional data");

                data.Rows.Add(row);
            }

            return data;
        }
    }
}