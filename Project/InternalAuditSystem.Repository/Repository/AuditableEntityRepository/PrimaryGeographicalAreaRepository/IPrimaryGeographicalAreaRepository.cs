using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditableEntityRepository.PrimaryGeographicalAreaRepository
{
    public interface IPrimaryGeographicalAreaRepository
    {
        /// <summary>
        /// Add PrimaryGeographicalArea to database
        /// </summary>
        /// <param name="primaryGeographicalAreaAC">PrimaryGeographicalArea model to be added</param>
        /// <returns>Added PrimaryGeographicalArea</returns>
        Task<PrimaryGeographicalAreaAC> AddPrimaryGeographicalAreaAsync(PrimaryGeographicalAreaAC primaryGeographicalAreaAC);

        /// <summary>
        /// Update PrimaryGeographicalArea
        /// </summary>
        /// <param name="primaryGeographicalAreaAC">PrimaryGeographicalArea AC object</param>
        /// <returns>Void</returns>
        Task UpdatePrimaryGeographicalAreaAsync(PrimaryGeographicalAreaAC primaryGeographicalAreaAC);

        /// <summary>
        /// Get country list by region
        /// </summary>
        /// <param name="regionId">Region Id</param>
        /// <param name="entityId">Entity Id</param>
        /// <returns>List of countries for dropdown</returns>
        Task<List<CountryAC>> GetCountryACListByRegionAsync(Guid regionId, Guid entityId);

        /// <summary>
        /// Get state list by country selection
        /// </summary>
        /// <param name="countryId">Country id</param>
        /// <param name="entityId">Entity Id</param>
        /// <returns>List of state</returns>
        Task<List<ProvinceStateAC>> GetStateACListByCountryAsync(Guid countryId, Guid entityId);

        /// <summary>
        /// Get PrimaryGeographicalArea List
        /// </summary>
        /// <param name="pagination">Pagination object</param>
        /// <returns>Pagination object</returns>
        Task<Pagination<PrimaryGeographicalAreaAC>> GetPrimaryGeographicalAreaListAsync(Pagination<PrimaryGeographicalAreaAC> pagination);

        /// <summary>
        /// Get primary geographical area details of edit and add 
        /// </summary>
        /// <param name="id">PrimaryGeographicalAreaid if on edit</param>
        /// <param name="entityId">Entity id</param>
        /// <returns>PrimaryGeographicalAreaAC</returns>
        Task<PrimaryGeographicalAreaAC> GetPrimaryGeographicalAreaDetailsAsync(Guid? id, Guid? entityId = null);

        /// <summary>
        /// Delete PrimaryGeographicalArea
        /// </summary>
        /// <param name="id">PrimaryGeographicalArea id</param>
        /// <returns>Void</returns>
        Task DeletePrimaryGeographicalAreaAync(Guid id);

        /// <summary>
        /// Method to Add Primary Geographical Area In New Version
        /// </summary>
        /// <param name="primaryGeographicalAreaACList">PrimaryGeographicalAreaAC list</param>
        /// <param name="versionId">New version entityId</param>
        /// <returns>Void</returns>
        Task AddPrimaryGeographicalAreaInNewVersionAsync(List<PrimaryGeographicalAreaAC> primaryGeographicalAreaACList, Guid versionId);
    }
}
