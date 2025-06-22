using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIGEBI.Persistence.Repositori
{
    public class SpecialMaterialService : ISpecialMaterialService
    {
        private readonly ISpecialMaterialRepository _specialMaterialRepository;

        public SpecialMaterialService(ISpecialMaterialRepository specialMaterialRepository)
        {
            _specialMaterialRepository = specialMaterialRepository;
        }

        public SpecialMaterial? GetSpecialMaterialById(int materialId)
        {
            return _specialMaterialRepository.GetById(materialId);
        }

        public IEnumerable<SpecialMaterial> GetAllSpecialMaterials()
        {
            return _specialMaterialRepository.GetAll();
        }

        public void RequestSpecialMaterial(SpecialMaterial material)
        {
            // Asigna valores por defecto
            if (material.RequestDate == default)
            {
                material.RequestDate = DateTime.Now;
            }
            if (string.IsNullOrEmpty(material.ApprovalStatus))
            {
                material.ApprovalStatus = "Pending"; // Estado inicial al solicitar
            }
            _specialMaterialRepository.Add(material);
        }

        public void ApproveSpecialMaterial(int materialId, int approvedByUserId)
        {
            var material = _specialMaterialRepository.GetById(materialId);
            if (material != null)
            {
                material.ApprovalStatus = "Approved";
                material.ApprovedByUserId = approvedByUserId;
                material.AcquisitionDate = DateTime.Now; // Fecha de adquisición al aprobar
                _specialMaterialRepository.Update(material);
            }
        }

        public void RejectSpecialMaterial(int materialId, string reason)
        {
            var material = _specialMaterialRepository.GetById(materialId);
            if (material != null)
            {
                material.ApprovalStatus = $"Rejected - {reason}";
                material.ApprovedByUserId = null; // No aprobado por nadie
                material.AcquisitionDate = null;
                _specialMaterialRepository.Update(material);
            }
        }

        public IEnumerable<SpecialMaterial> GetSpecialMaterialsByRequestedUser(int userId)
        {
            return _specialMaterialRepository.GetByRequestedByUserId(userId);
        }

        public IEnumerable<SpecialMaterial> GetSpecialMaterialsByStatus(string status)
        {
            return _specialMaterialRepository.GetByApprovalStatus(status);
        }
    }
}
