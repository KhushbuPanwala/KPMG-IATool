using InternalAuditSystem.DomailModel.DatabaseContext;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomailModel.Models.WorkProgramModels;
using InternalAuditSystem.DomainModel.Enums;
using InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels;
using InternalAuditSystem.Utility.Constants;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.SeedData
{
    public class SeedData
    {
        /// <summary>
        /// Seed Initial data to database
        /// </summary>
        /// <param name="_dbContext">Instance of database constext</param>
        public static void Initialize(InternalAuditSystemContext _dbContext)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    #region Country and state seed

                    var json = File.ReadAllText(Path.Combine(System.IO.Directory.GetCurrentDirectory() + StringConstant.CountryStateFilePath));
                    List<Country> countryList = new List<Country>();

                    List<ProvinceState> provinceStateList = new List<ProvinceState>();

                    var jObject = JObject.Parse(json);

                    if (jObject != null)
                    {
                        JArray countriesArrary = (JArray)jObject["countries"];
                        if (countriesArrary != null)
                        {
                            foreach (var item in countriesArrary)
                            {
                                var country = item["country"];

                                Country countryObj = new Country();
                                countryObj.Id = Guid.NewGuid();
                                countryObj.Name = country.ToString();
                                countryObj.CreatedDateTime = DateTime.UtcNow;
                                countryObj.IsDeleted = false;
                                countryList.Add(countryObj);


                                var state = item["states"];
                                for (int j = 0; j < state.Count<object>(); j++)
                                {
                                    ProvinceState provinceStateObj = new ProvinceState();
                                    provinceStateObj.Id = Guid.NewGuid();
                                    provinceStateObj.Name = state[j].ToString();
                                    provinceStateObj.CountryId = countryObj.Id;
                                    provinceStateObj.CreatedDateTime = DateTime.UtcNow;
                                    provinceStateList.Add(provinceStateObj);
                                }
                            }

                        }
                    }

                    if (!_dbContext.Country.Any(x => !x.IsDeleted))
                    {
                        _dbContext.AddRangeAsync(countryList);
                        _dbContext.SaveChanges();
                    }

                    if (!_dbContext.ProvinceState.Any(x => !x.IsDeleted))
                    {
                        _dbContext.AddRangeAsync(provinceStateList);
                        _dbContext.SaveChanges();
                    }
                    #endregion

                    
                    _dbContext.SaveChanges();
                    transaction.Commit();

                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
