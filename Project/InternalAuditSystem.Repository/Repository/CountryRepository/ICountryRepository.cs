using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.CountryRepository
{
   public interface ICountryRepository
    {
        /// <summary>
        /// Get all the country under a region with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <returns>List of countries under a region</returns>
        Task<Pagination<EntityCountryAC>> GetAllCountryPageWiseAndSearchWiseAsync(Pagination<EntityCountryAC> pagination);

        /// <summary>
        /// Get data of a particular country under an auditable entity
        /// </summary>
        /// <param name="countryId">Id of the country</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular country data</returns>
        Task<EntityCountryAC> GetCountryByIdAsync(string countryId,string selectedEntityId);

        /// <summary>
        /// Method to add country
        /// </summary>
        /// <param name="countryDetails">Details of country</param>
        /// <returns>Added object of country</returns>
        Task<EntityCountryAC> AddCountryAsync(EntityCountryAC countryDetails);

        /// <summary>
        /// Update country under an auditable entity
        /// </summary>
        /// <param name="updatedCountryDetails">Updated details of country</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateCountryAsync(EntityCountryAC updatedCountryDetails);

        /// <summary>
        /// Method to delete country
        /// </summary>
        /// <param name="countryId">Id of country</param>
        /// <param name="selectedEntityId">selected entityId</param>
        /// <returns>Task</returns>
        Task DeleteCountryAsync(string countryId, string selectedEntityId);

        /// <summary>
        /// Get all countries based on the search term from active directory
        /// </summary>
        /// <param name="searchString">Search string on Name field</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Object of EntityCountryAC</returns>
        Task<EntityCountryAC> GetAllCountryBasedOnSearchAsync(string searchString, string selectedEntityId);


        /// <summary>
        /// Method for exporting countries
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportCountriesAsync(string entityId, int timeOffset);
    }
}
